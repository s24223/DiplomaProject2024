using Application.Features.Internships.Commands.Internships.DTOs;
using Application.Features.Internships.Commands.Internships.Interfaces;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Internships.Services
{
    public class InternshipCmdSvc : IInternshipCmdSvc
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authentication;
        private readonly IInternshipCmdRepo _internshipRepository;


        //Cosntructor
        public InternshipCmdSvc
            (
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication,
            IInternshipCmdRepo internshipRepository
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
        public async Task<ResponseItem<InternshipResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            Guid recrutmentId,
            CreateInternshipReq dto,
            CancellationToken cancellation
            )

        {
            var companyId = GetCompanyId(claims);

            var domain = _domainFactory.CreateDomainInternship
                (
                recrutmentId,
                (DateOnly)dto.ContractStartDate,
                (DateOnly?)dto.ContractEndDate,
                dto.ContactNumber
                );

            domain = await _internshipRepository.CreateAsync(companyId, domain, cancellation);
            return new ResponseItem<InternshipResp>
            {
                Item = new InternshipResp(domain),
            };
        }

        public async Task<ResponseItem<InternshipResp>> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid internshipId,
            UpdateInternshipReq dto,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);


            var domain = await _internshipRepository.GetInternshipAsync
                (
                companyId,
                new RecrutmentId(internshipId),
                cancellation
                );
            domain.Update
                (
                dto.ContractNumber,
                (DateOnly)dto.ContractStartDate,
                (DateOnly?)dto.ContractEndDate
                );

            domain = await _internshipRepository.UpdateAsync(companyId, domain, cancellation);
            return new ResponseItem<InternshipResp>
            {
                Item = new InternshipResp(domain),
            };
        }
        //===============================================================================================================
        //===============================================================================================================
        //===============================================================================================================
        //Private Methods
        private UserId GetCompanyId(IEnumerable<Claim> claims)
        {
            return _authentication.GetIdNameFromClaims(claims);
        }
    }
}
