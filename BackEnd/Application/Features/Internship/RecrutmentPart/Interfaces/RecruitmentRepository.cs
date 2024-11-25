using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internship.RecrutmentPart.Interfaces
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public RecruitmentRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        //DML
        public async Task CreateAsync
            (
            DomainRecruitment recruitment,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseRecrutment = new Recruitment
                {
                    BranchOfferId = recruitment.BranchOfferId.Value,
                    PersonId = recruitment.PersonId.Value,
                    Created = recruitment.Created,
                    PersonMessage = recruitment.PersonMessage,
                };

                await _context.Recruitments.AddAsync(databaseRecrutment, cancellation);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }


        public async Task SetAnswerAsync
            (
            UserId companyId,
            DomainRecruitment recruitment,
            CancellationToken cancellation
            )
        {
            try
            {
                if (recruitment.IsAccepted != null)
                {
                    var databaseRecrutment = await GetDatabaseRecruitment
                        (
                        companyId,
                        recruitment.Id,
                        cancellation
                        );

                    databaseRecrutment.CompanyResponse = recruitment.CompanyResponse;
                    databaseRecrutment.IsAccepted = recruitment.IsAccepted.Code;

                    await _context.SaveChangesAsync(cancellation);
                }
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<DomainRecruitment> GetRecruitmentForSetAnswerAsync
            (
            UserId companyId,
            RecrutmentId id,
            CancellationToken cancellation
            )
        {
            var databaseRecruitment = await GetDatabaseRecruitment(companyId, id, cancellation);
            return _mapper.ToDomainRecruitment(databaseRecruitment);
        }

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
        private async Task<Recruitment> GetDatabaseRecruitment
            (
            UserId companyId,
            RecrutmentId id,
            CancellationToken cancellation
            )
        {
            var path = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y => y.Recruitments.Any(z =>
                        z.Id == id.Value
                    ))
                ).FirstOrDefaultAsync(cancellation);

            if (path == null)
            {
                throw new RecruitmentException
                    (
                    Messages2.Recruitment_Ids_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return path.BranchOffers.First().Recruitments.First();
        }
    }
}
