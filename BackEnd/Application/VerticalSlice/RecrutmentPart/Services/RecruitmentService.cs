using Application.Shared.DTOs.Response;
using Application.VerticalSlice.OfferBranchPart.Interfaces;
using Application.VerticalSlice.RecrutmentPart.DTOs.CreateProfile;
using Application.VerticalSlice.RecrutmentPart.Interfaces;
using Domain.Factories;
using Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VerticalSlice.RecrutmentPart.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IDomainFactory _domainFactory;
        private readonly IDomainProvider _domainProvider;

        public RecruitmentService(IRecruitmentRepository recruitmentRepository,
            IDomainFactory domainFactory)
        {
            _recruitmentRepository = recruitmentRepository;
            _domainFactory = domainFactory;
        }
        public async Task<Response> CreateRecruitmentAsync(CreateRecruitmentRequestDto dto, CancellationToken cancellation)
        {
            var recruitment = _domainFactory.CreateDomainRecrutment(
                    dto.PersonId,
                    dto.BranchId,
                    dto.OfferId,
                    dto.Created,
                    dto.ApplicationDate,
                    dto.PersonMessage,
                    dto.CompanyResponse,
                    dto.AcceptedRejected
                    
                );
            await _recruitmentRepository.CreateRecruitmentAsync(recruitment, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }

    }
}
