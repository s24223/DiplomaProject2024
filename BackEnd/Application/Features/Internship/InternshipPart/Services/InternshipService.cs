using Application.Features.Internship.InternshipPart.DTOs.Create;
using Application.Features.Internship.InternshipPart.DTOs.Update;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Intership.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internship.InternshipPart.Services
{
    public class InternshipService : IInternshipService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IInternshipRepository _internshipRepository;


        //Cosntructor
        public InternshipService
            (
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IInternshipRepository internshipRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _internshipRepository = internshipRepository;
        }


        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Public Methods
        public async Task<ResponseItem<CreateInternshipResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            Guid offerId,
            DateTime created,
            Guid personId,
            CreateInternshipRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authentication.GetIdNameFromClaims(claims);
            var domainInternship = _domainFactory.CreateDomainInternship
                (
                personId,
                branchId,
                offerId,
                created,
                dto.ContactNumber
                );

            var internshipId = await _internshipRepository.CreateAsync
                (
                companyId,
                domainInternship,
                cancellation
                );

            return new ResponseItem<CreateInternshipResponseDto>
            {
                Item = new CreateInternshipResponseDto
                {
                    InternshipId = internshipId,
                },
            };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid internshipId,
            UpdateInternshipRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authentication.GetIdNameFromClaims(claims);
            var doaminInternship = await _internshipRepository.GetInternshipAsync
                (
                companyId,
                new IntershipId(internshipId),
                cancellation
                );
            doaminInternship.Update(dto.ContractNumber);

            await _internshipRepository.UpdateAsync
                (
                companyId,
                doaminInternship,
                cancellation
                );
            return new Response { };
        }
        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Private Methods
    }
}
