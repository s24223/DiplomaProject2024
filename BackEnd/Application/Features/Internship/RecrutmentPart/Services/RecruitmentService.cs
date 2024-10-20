using Application.Features.Internship.RecrutmentPart.DTOs.Create;
using Application.Features.Internship.RecrutmentPart.DTOs.SetAnswerByCompany;
using Application.Features.Internship.RecrutmentPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internship.RecrutmentPart.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authentication;
        private readonly IRecruitmentRepository _repository;


        //Cosntructor
        public RecruitmentService
            (
            IDomainFactory domainFactory,
            IAuthenticationService authentication,
            IRecruitmentRepository repository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _repository = repository;
        }


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods

        //DML
        public async Task<Response> CreateByPersonAsync
            (
            IEnumerable<Claim> claims,
            Guid branchOfferId,
            CreateRecruitmentRequestDto dto,
            CancellationToken cancellation
            )
        {
            var personId = _authentication.GetIdNameFromClaims(claims);
            var recruitment = _domainFactory.CreateDomainRecruitment
                (
                    personId.Value,
                    branchOfferId,
                    dto.PersonMessage
                );

            await _repository.CreateAsync(recruitment, cancellation);
            return new Response { };
        }

        public async Task<Response> SetAnswerByCompanyAsync
            (
            IEnumerable<Claim> claims,
            Guid id,
            SetAnswerByCompanyRecrutmentDto dto,
            CancellationToken cancellation
            )
        {
            var companyId = _authentication.GetIdNameFromClaims(claims);
            var recrutmentId = new RecrutmentId(id);

            var recruitment = await _repository.GetRecruitmentForSetAnswerAsync(companyId, recrutmentId, cancellation);
            recruitment.SetAnswer(dto.CompanyResponse, dto.IsAccepted);

            await _repository.SetAnswerAsync(companyId, recruitment, cancellation);
            return new Response { };
        }
        //DQL

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
