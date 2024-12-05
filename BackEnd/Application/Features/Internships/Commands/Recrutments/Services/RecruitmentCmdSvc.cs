using Application.Features.Internships.Commands.Recrutments.DTOs;
using Application.Features.Internships.Commands.Recrutments.Interfaces;
using Application.Shared.DTOs.Features.Internships;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Internships.Commands.Recrutments.Services
{
    public class RecruitmentCmdSvc : IRecruitmentCmdSvc
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthJwtSvc _authentication;
        private readonly IRecruitmentCmdRepo _repository;


        //Cosntructor
        public RecruitmentCmdSvc
            (
            IDomainFactory domainFactory,
            IAuthJwtSvc authentication,
            IRecruitmentCmdRepo repository
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
        public async Task<ResponseItem<RecruitmentResp>> CreateByPersonAsync
            (
            IEnumerable<Claim> claims,
            Guid branchOfferId,
            CreateRecruitmentReq dto,
            CancellationToken cancellation
            )
        {
            var personId = GetUserId(claims);
            var domain = _domainFactory.CreateDomainRecruitment
                (
                    personId.Value,
                    branchOfferId,
                    dto.PersonMessage
                );

            domain = await _repository.CreateAsync(domain, cancellation);
            return new ResponseItem<RecruitmentResp>
            {
                Item = new RecruitmentResp(domain),
            };
        }

        public async Task<ResponseItem<RecruitmentResp>> SetAnswerByCompanyAsync
            (
            IEnumerable<Claim> claims,
            Guid recrutmentId,
            SetAnswerReq dto,
            CancellationToken cancellation
            )
        {
            var companyId = GetUserId(claims);
            var id = new RecrutmentId(recrutmentId);

            var domain = await _repository.GetRecruitmentAsync(companyId, id, cancellation);
            domain.SetAnswer(dto.CompanyResponse, dto.IsAccepted);

            await _repository.UpdateAsync(companyId, domain, cancellation);
            return new ResponseItem<RecruitmentResp>
            {
                Item = new RecruitmentResp(domain),
            };
        }
        //DQL

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
        private UserId GetUserId(IEnumerable<Claim> claims)
        {
            return _authentication.GetIdNameFromClaims(claims);
        }
    }
}
