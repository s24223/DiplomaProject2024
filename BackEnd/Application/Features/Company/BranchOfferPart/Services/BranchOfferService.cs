using Application.Features.Company.BranchOfferPart.DTOs.CreateProfile;
using Application.Features.Company.OfferBranchPart.Interfaces;
using Application.Shared.DTOs.Response;
using Domain.Shared.Factories;
using Domain.Shared.Providers;

namespace Application.Features.Company.OfferBranchPart.Services
{
    public class BranchOfferService : IBranchOfferService
    {
        private readonly IBranchOfferRepository _branchOfferRepository;
        private readonly IDomainFactory _domainFactory;
        private readonly IProvider _domainProvider;

        public BranchOfferService(IBranchOfferRepository branchOfferRepository,
            IDomainFactory domainFactory)
        {
            _branchOfferRepository = branchOfferRepository;
            _domainFactory = domainFactory;
        }

        public async Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation)
        {
            var offerBranch = _domainFactory.CreateDomainBranchOffer(
                dto.BranchId,
                dto.OfferId,
                dto.Created,
                dto.PublishStart,
                dto.PublishEnd,
                dto.WorkStart,
                dto.WorkEnd,
                dto.LastUpdate
                );

            await _branchOfferRepository.CreateBranchOfferAsync(offerBranch, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }
    }
}
