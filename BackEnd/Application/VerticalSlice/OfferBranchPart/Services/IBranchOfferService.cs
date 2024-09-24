using Application.Shared.DTOs.Response;
using Application.VerticalSlice.OfferBranchPart.DTOs;

namespace Application.VerticalSlice.OfferBranchPart.Services
{
    public interface IBranchOfferService
    {
        Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation);
    }
}
