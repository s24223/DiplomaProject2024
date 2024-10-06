using Application.Features.Company.OfferBranchPart.DTOs;
using Application.Shared.DTOs.Response;

namespace Application.Features.Company.OfferBranchPart.Services
{
    public interface IBranchOfferService
    {
        Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation);
    }
}
