using Application.Features.Company.BranchOfferPart.DTOs.CreateProfile;
using Application.Shared.DTOs.Response;

namespace Application.Features.Company.OfferBranchPart.Services
{
    public interface IBranchOfferService
    {
        Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation);
    }
}
