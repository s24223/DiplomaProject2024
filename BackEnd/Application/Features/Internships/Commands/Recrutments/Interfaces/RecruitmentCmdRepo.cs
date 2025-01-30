using Application.Databases.NonRelational;
using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Internships.Mappers;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internships.Commands.Recrutments.Interfaces
{
    public class RecruitmentCmdRepo : IRecruitmentCmdRepo
    {
        //Values
        private readonly IProvider _provider;
        private readonly IInternshipMapper _mapper;
        private readonly DiplomaProjectContext _context;
        private readonly NonRelationalDbRepository _nonRelational;


        //Constructor
        public RecruitmentCmdRepo
            (
            IProvider provider,
            IInternshipMapper mapper,
            DiplomaProjectContext context,
            NonRelationalDbRepository nonRelational
            )
        {
            _provider = provider;
            _mapper = mapper;
            _context = context;
            _nonRelational = nonRelational;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        //DML
        public async Task<DomainRecruitment> CreateAsync
            (
            DomainRecruitment domain,
            IFormFile? file,
            CancellationToken cancellation
            )
        {
            try
            {
                var exInvalid = await ReturnexceptionIfNotValidAsync(domain, cancellation);
                if (exInvalid != null)
                {
                    throw exInvalid;
                }

                var db = MapRecruitment(domain, null);
                await _context.Recruitments.AddAsync(db, cancellation);
                await _context.SaveChangesAsync(cancellation);

                if (file != null)
                {
                    var fileName = await _nonRelational.SaveAsync(file, cancellation);
                    db.CvUrl = fileName;
                    await _context.SaveChangesAsync(cancellation);
                    domain.Url = fileName;
                }

                return _mapper.DomainRecruitment(db);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, domain);
            }
        }


        public async Task<DomainRecruitment> UpdateAsync
            (
            UserId companyId,
            DomainRecruitment domain,
            CancellationToken cancellation
            )
        {
            try
            {
                var db = await GetDatabaseRecruitment(companyId, domain.Id, cancellation);
                db = MapRecruitment(domain, db);


                var dbFalse = new DatabaseBool(false).Code;
                if (db.IsAccepted == dbFalse)
                {
                    var internshipForDeleteng = await _context.Internships
                        .Include(x => x.Comments)
                        .Where(x => x.Id == db.Id)
                        .FirstOrDefaultAsync(cancellation);
                    if (internshipForDeleteng != null)
                    {
                        internshipForDeleteng.Comments.Clear();
                        _context.Internships.Remove(internshipForDeleteng);
                    }
                }

                await _context.SaveChangesAsync(cancellation);
                return _mapper.DomainRecruitment(db);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex, domain);
            }
        }

        //DQL
        public async Task<DomainRecruitment> GetRecruitmentAsync
            (
            UserId companyId,
            RecrutmentId recrutmentid,
            CancellationToken cancellation
            )
        {
            var db = await GetDatabaseRecruitment(companyId, recrutmentid, cancellation);
            return _mapper.DomainRecruitment(db);
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
        private async Task<Recruitment> GetDatabaseRecruitment
            (
            UserId companyId,
            RecrutmentId recrutmentid,
            CancellationToken cancellation
            )
        {
            var recrutment = await _context.Recruitments
                .Include(x => x.BranchOffer)
                .ThenInclude(x => x.Branch)
                .Where(x =>
                    x.Id == recrutmentid.Value &&
                    x.BranchOffer.Branch.CompanyId == companyId.Value
                    ).FirstOrDefaultAsync(cancellation);

            if (recrutment == null)
            {
                throw new RecruitmentException
                    (
                    Messages.Recruitment_Cmd_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return recrutment;
        }

        private Recruitment MapRecruitment(DomainRecruitment domain, Recruitment? db)
        {
            var database = db ?? new Recruitment();
            //database.Id

            if (db == null)
            {
                database.PersonId = domain.PersonId.Value;
                database.BranchOfferId = domain.BranchOfferId.Value;
            }
            database.Created = domain.Created;
            database.CvUrl = null;
            database.PersonMessage = domain.PersonMessage;
            database.CompanyResponse = domain.CompanyResponse;
            database.IsAccepted = domain.IsAccepted?.Code;

            return database;
        }

        private async Task<System.Exception?> ReturnexceptionIfNotValidAsync
            (DomainRecruitment domain, CancellationToken cancellation)
        {
            var value = await _context.BranchOffers
                .Include(x => x.Branch)
                .Where(x => x.Id == domain.BranchOfferId.Value)
                .FirstOrDefaultAsync(cancellation);
            var now = _provider.TimeProvider().GetDateTimeNow();

            if (value == null)
            {
                return new RecruitmentException
                           (
                           $"{Messages.Recruitment_Cmd_BranchOffer_NotFound}",
                           DomainExceptionTypeEnum.NotFound

                           );
            }
            if (value.PublishStart > now)
            {
                return new RecruitmentException($"{Messages.Recruitment_Cmd_BranchOffer_Future}");
            }
            if (value.PublishEnd != null && value.PublishEnd <= now)
            {
                return new RecruitmentException($"{Messages.Recruitment_Cmd_BranchOffer_Expired}");
            }
            if (value.Branch.CompanyId == domain.PersonId.Value)
            {
                return new RecruitmentException($"{Messages.Recruitment_Cmd_IntoHisCompany}");
            }

            return null;
        }

        private System.Exception HandleException(System.Exception ex, DomainRecruitment domain)
        {
            //2627  Unique, Pk 
            //547   Check, FK

            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                if (number == 2627 && message.Contains("Recruitment_UNIQUE"))
                {
                    return new RecruitmentException(Messages.Recruitment_Cmd_Recruitment_Exist);
                }
                if (number == 547)
                {
                    //App Exception Dtabase bool
                    if (message.Contains("Recruitment_CHECK_IsAccepted"))
                    {
                        return new RecruitmentException
                            (
                            $"{Messages.Recruitment_Cmd_IsAccepted_Invalid}: {domain.IsAccepted?.Code}",
                            DomainExceptionTypeEnum.AppProblem
                            );
                    }

                    //Recruitment_BranchOffer
                    if (message.Contains("Recruitment_BranchOffer"))
                    {
                        return new RecruitmentException
                            (
                            $"{Messages.Recruitment_Cmd_BranchOffer_NotFound}",
                            DomainExceptionTypeEnum.NotFound

                            );
                    }

                    if (message.Contains("Recruitment_Person"))
                    {
                        return new RecruitmentException
                            (
                            $"{Messages.Recruitment_Cmd_Person_NotFound}",
                            DomainExceptionTypeEnum.NotFound

                            );
                    }
                }
            }
            return ex;
        }
    }
}
