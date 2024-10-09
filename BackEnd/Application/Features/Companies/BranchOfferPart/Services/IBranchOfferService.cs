using Application.Features.Companies.BranchOfferPart.DTOs.CreateProfile;
using Application.Shared.DTOs.Response;

namespace Application.Features.Companies.BranchOfferPart.Services
{
    public interface IBranchOfferService
    {
        Task<Response> CreateBranchOfferAsync(CreateBranchOfferDto dto, CancellationToken cancellation);
    }
}
