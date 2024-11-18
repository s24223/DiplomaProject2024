using Application.Features.Internship.InternshipPart.DTOs.Create;
using Application.Features.Internship.InternshipPart.DTOs.Update;
using Application.Features.Internship.InternshipPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internship.InternshipPart.Services
{
    public class InternshipService : IInternshipService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationSvc _authentication;
        private readonly IInternshipRepository _internshipRepository;


        //Cosntructor
        public InternshipService
            (
            IDomainFactory domainFactory,
            IAuthenticationSvc authentication,
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
            Guid recrutmentId,
            CreateInternshipRequestDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authentication.GetIdNameFromClaims(claims);
            var start = (DateOnly)dto.ContractStartDate;
            var end = dto.ContractEndDate == null ? (DateOnly?)(null) : (DateOnly)dto.ContractEndDate;

            var domainInternship = _domainFactory.CreateDomainInternship
                (
                recrutmentId,
                start,
                 end,
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
            var start = (DateOnly)dto.ContractStartDate;
            var end = dto.ContractEndDate == null ? (DateOnly?)(null) : (DateOnly)dto.ContractEndDate;


            var doaminInternship = await _internshipRepository.GetInternshipAsync
                (
                companyId,
                new RecrutmentId(internshipId),
                cancellation
                );
            doaminInternship.Update(dto.ContractNumber, start, end);

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
