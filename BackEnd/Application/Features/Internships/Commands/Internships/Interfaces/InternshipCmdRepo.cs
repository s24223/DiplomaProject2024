using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Internships.Mappers;
using Domain.Features.Intership.Entities;
using Domain.Features.Intership.Exceptions.Entities;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internships.Commands.Internships.Interfaces
{
    public class InternshipCmdRepo : IInternshipCmdRepo
    {
        //Values
        private readonly IInternshipMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public InternshipCmdRepo
            (
            IInternshipMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public async Task<DomainIntership> CreateAsync
            (
            UserId companyId,
            DomainIntership domain,
            CancellationToken cancellation
            )
        {
            try
            {
                var databse = MapInternship(domain, null);
                await _context.Internships.AddAsync(databse, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return _mapper.DomainIntership(databse);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public async Task<DomainIntership> UpdateAsync
            (
            UserId companyId,
            DomainIntership domain,
            CancellationToken cancellation
            )
        {
            try
            {
                var database = await GetDatabseInternshipAsync(companyId, domain.Id, cancellation);
                database = MapInternship(domain, database);
                await _context.SaveChangesAsync(cancellation);
                return _mapper.DomainIntership(database);
            }
            catch (System.Exception ex)
            {
                throw HandleException(ex);
            }
        }

        //DQL
        public async Task<DomainIntership> GetInternshipAsync
            (
            UserId companyId,
            RecrutmentId intershipId,
            CancellationToken cancellation
            )
        {
            var databaseIntership = await GetDatabseInternshipAsync(companyId, intershipId, cancellation);
            return _mapper.DomainIntership(databaseIntership);
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private async Task<Recruitment> GetDatabseRecruitmentAsync
            (
            UserId companyId,
            RecrutmentId recrutmentId,
            CancellationToken cancellation
            )
        {
            var databaseBoolTrue = new DatabaseBool(true).Code;
            var pathToRecrutment = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y => y.Recruitments.Any(z =>
                        z.Id == recrutmentId.Value &&
                        z.IsAccepted == databaseBoolTrue
                    ))
                ).FirstOrDefaultAsync(cancellation);
            if (pathToRecrutment == null)
            {
                throw new RecruitmentException
                    (
                    Messages.Internship_Cmd_Recruitment_PositiveNotExist,
                    DomainExceptionTypeEnum.NotFound
                    );
            }

            return pathToRecrutment.BranchOffers.First().Recruitments.First();
        }

        private async Task<Internship> GetDatabseInternshipAsync
            (
            UserId companyId,
            RecrutmentId intershipId,
            CancellationToken cancellation
            )
        {
            var databaseBoolTrue = new DatabaseBool(true).Code;
            var pathToRecrutment = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .ThenInclude(x => x.Internship)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y =>
                        y.Recruitments.Any(z =>
                            z.Internship != null &&
                             z.Internship.Id == intershipId.Value
                            ))
                ).FirstOrDefaultAsync(cancellation);
            if (pathToRecrutment == null)
            {
                throw new IntershipException
                    (
                    Messages.Internship_Cmd_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound

                    );
            }

            return pathToRecrutment.BranchOffers.First().Recruitments.First().Internship;
        }

        private Internship MapInternship(DomainIntership domain, Internship? database)
        {
            var db = database ?? new Internship();

            db.ContractNumber = domain.ContractNumber.Value;
            db.Created = domain.Created;
            db.ContractStartDate = domain.ContractStartDate;
            db.ContractEndDate = domain.ContractEndDate;

            return db;
        }

        private System.Exception HandleException(System.Exception ex)
        {
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                //2627  Unique, Pk 
                //547   Check, FK
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                if (number == 2627 && message.Contains("Internship_pk"))
                {
                    return new IntershipException(Messages.Internship_Cmd_Exist);
                }
                else if (number == 547 && message.Contains("Internship_Recruitment"))
                {
                    return new IntershipException(Messages.Internship_Cmd_Recruitment_NotExist);
                }
                else if (number == 50001)
                {
                    // POSITIVE RECRUTMENT NOT EXIST
                    return new IntershipException(Messages.Internship_Cmd_Recruitment_PositiveNotExist);
                }
                else if (number == 50002)
                {
                    //DUPLICATE
                    return new IntershipException(Messages.Internship_Cmd_ContractNum_Duplicate);
                }
            }

            throw ex;
        }
    }
}
