using Application.Shared.DTOs.Response;
using Application.VerticalSlice.OfferBranchPart.DTOs;
using Application.VerticalSlice.OfferBranchPart.Interfaces;
using Domain.Factories;
using Domain.Providers;

namespace Application.VerticalSlice.OfferBranchPart.Services
{
    public class BranchOfferService : IBranchOfferService
    {
        private readonly IBranchOfferRepository _branchOfferRepository;
        private readonly IDomainFactory _domainFactory;
        private readonly IDomainProvider _domainProvider;

        public BranchOfferService(IBranchOfferRepository branchOfferRepository,
            IDomainFactory domainFactory)
        {
            _branchOfferRepository = branchOfferRepository;
            _domainFactory = domainFactory;
        }

        public async Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation)
        {
            var offerBranch = _domainFactory.CreateDomainBranchOffer(dto.BranchId, 
                dto.OfferId, 
                dto.Created, 
                dto.PublishStart, 
                dto.PublishEnd, 
                dto.WorkStart, 
                dto.WorkEnd,
                dto.LastUpdate, 
                _domainProvider);

            await _branchOfferRepository.CreateBranchOfferAsync(offerBranch, cancellation);
            return new Response
            {
                Status = EnumResponseStatus.Success,
                Message = Messages.ResponseSuccess
            };
        }
    }
}
