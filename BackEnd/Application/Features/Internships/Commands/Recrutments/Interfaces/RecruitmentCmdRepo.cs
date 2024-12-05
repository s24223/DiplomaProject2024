using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Internships.Mappers;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;
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


        //Cosntructor
        public RecruitmentCmdRepo
            (
            IProvider provider,
            IInternshipMapper mapper,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _mapper = mapper;
            _context = context;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        //DML
        public async Task<DomainRecruitment> CreateAsync
            (
            DomainRecruitment domain,
            CancellationToken cancellation
            )
        {
            try
            {
                var branchOfferId = domain.BranchOfferId.Value;

                var db = MapRecruitment(domain, null);
                await _context.Recruitments.AddAsync(db, cancellation);
                await _context.SaveChangesAsync(cancellation);
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
            var path = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y => y.Recruitments.Any(z =>
                        z.Id == recrutmentid.Value
                    ))
                ).FirstOrDefaultAsync(cancellation);

            if (path == null)
            {
                throw new RecruitmentException
                    (
                    Messages.Recruitment_Cmd_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return path.BranchOffers.First().Recruitments.First();
        }

        private Recruitment MapRecruitment(DomainRecruitment domain, Recruitment? db)
        {
            var database = db ?? new Recruitment();
            //As a comment is Readonly
            //dbRecrutment.PersonId = doamain.
            //dbRecrutment.BranchOfferId = doamain.
            //dbRecrutment.Id = doamain.
            database.Created = domain.Created;
            database.CvUrl = null;
            database.PersonMessage = domain.PersonMessage;
            database.CompanyResponse = domain.CompanyResponse;
            database.IsAccepted = domain.IsAccepted?.Code;

            return database;
        }
        /*
                private async Task CheckIsValidBranchOffer(BranchOfferId id, CancellationToken cancellation)
                {
                    var now = _provider.TimeProvider().GetDateTimeNow();
                    var database = await _context.BranchOffers
                        .Where(x => x.Id == id.Value)
                        .FirstOrDefaultAsync(cancellation);


                    if (database == null)
                    {
                        throw new BranchOfferException
                            (
                            $"{Messages.BranchOffer_Cmd_Id_NotFound}: {id.Value}",
                            DomainExceptionTypeEnum.NotFound
                            );
                    }

                    if (
                        database.PublishStart > now ||
                        (database.PublishStart <= now && database.PublishEnd <= now)
                        )
                    {
                        throw new BranchOfferException
                           (
                            $"{Messages.BranchOffer_Cmd_Time_Invalid}: {id.Value}",
                            DomainExceptionTypeEnum.NotFound
                           );
                    }



                }*/

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
                if (number == 50003)
                {
                    return new RecruitmentException($"{Messages.Recruitment_Cmd_BranchOffer_Invalid}");
                }
                if (number == 50004)
                {
                    return new RecruitmentException($"{Messages.Recruitment_Cmd_IntoHisCompany}");
                }
            }
            return ex;
        }
    }
}
