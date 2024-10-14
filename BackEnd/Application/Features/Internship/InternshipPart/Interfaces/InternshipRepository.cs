﻿using Application.Database;
using Application.Database.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Intership.Entities;
using Domain.Features.Intership.Exceptions.Entities;
using Domain.Features.Intership.ValueObjects.Identificators;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Internship.InternshipPart.Interfaces
{
    public class InternshipRepository : IInternshipRepository
    {
        //Values
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public InternshipRepository
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


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public async Task<Guid> CreateAsync
            (
            UserId companyId,
            DomainIntership intership,
            CancellationToken cancellation
            )
        {
            try
            {
                var recrutment = await GetDatabseRecruitmentAsync
                    (
                    companyId,
                    intership.RecrutmentId,
                    cancellation
                    );

                var databseIntership = new Application.Database.Models.Internship
                {
                    Recruitment = recrutment,
                    ContractNumber = intership.ContractNumber.Value,
                };

                await _context.Internships.AddAsync(databseIntership, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databseIntership.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            UserId companyId,
            DomainIntership intership,
            CancellationToken cancellation
            )
        {
            try
            {
                var databseIntership = await GetDatabseInternshipAsync
                    (
                    companyId,
                    intership.Id,
                    cancellation
                    );

                databseIntership.ContractNumber = intership.ContractNumber.Value;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        //DQL
        public async Task<DomainIntership> GetInternshipAsync
            (
            UserId companyId,
            IntershipId intershipId,
            CancellationToken cancellation
            )
        {
            var databaseIntership = await GetDatabseInternshipAsync
                   (
                   companyId,
                   intershipId,
                   cancellation
                   );
            return _mapper.ToDomainIntership(databaseIntership);
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
                        z.OfferId == recrutmentId.BranchOfferId.OfferId.Value &&
                        z.BranchId == recrutmentId.BranchOfferId.BranchId.Value &&
                        z.Created == recrutmentId.BranchOfferId.Created &&
                        z.PersonId == recrutmentId.PersonId.Value &&
                        z.IsAccepted == databaseBoolTrue
                    ))
                ).FirstOrDefaultAsync(cancellation);
            if (pathToRecrutment == null)
            {
                throw new RecruitmentException(Messages.Recruitment_IdsAccepted_NotFound);
            }

            return pathToRecrutment.BranchOffers.First().Recruitments.First();
        }

        private async Task<Application.Database.Models.Internship> GetDatabseInternshipAsync
            (
            UserId companyId,
            IntershipId intershipId,
            CancellationToken cancellation
            )
        {
            var databaseBoolTrue = new DatabaseBool(true).Code;
            var pathToRecrutment = await _context.Branches
                .Include(x => x.BranchOffers)
                .ThenInclude(x => x.Recruitments)
                .ThenInclude(x => x.Internships)
                .Where(x =>
                    x.CompanyId == companyId.Value &&
                    x.BranchOffers.Any(y =>
                        y.Recruitments.Any(z =>
                            z.Internships.Any(e =>
                                e.Id == intershipId.Value

                    )))
                ).FirstOrDefaultAsync(cancellation);
            if (pathToRecrutment == null)
            {
                throw new IntershipException(Messages.Intership_Ids_NotFound);
            }

            return pathToRecrutment.BranchOffers.First().Recruitments.First().Internships.First();
        }
    }
}
