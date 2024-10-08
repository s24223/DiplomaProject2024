using Application.Features.Company.BranchPart.DTOs.CreateProfile;
using Application.Features.Company.BranchPart.DTOs.UpdateProfile;
using Application.Features.Company.BranchPart.Interfaces;
using Domain.Shared.Factories;
using Domain.Shared.Providers;


namespace Application.Features.Company.BranchPart.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IDomainFactory _domainFactory;
        private readonly IProvider _domainProvider;

        public BranchService(IBranchRepository branchRepository,
            IDomainFactory domainFactory)
        {
            _branchRepository = branchRepository;
            _domainFactory = domainFactory;
        }

        public async Task CreateBranchAsync
            (
            CreateBranchProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var branch = _domainFactory.CreateDomainBranch
                (
                    dto.CompanyId,
                    dto.AddressId,
                    dto.Id,
                    dto.UrlSegment,
                    dto.Name,
                    dto.Description
                );
            await _branchRepository.CreateBranchProfileAsync(branch, cancellation);
        }
        public async Task UpdateBranchAsync
            (
            Guid id,
            UpdateBranchProfileRequestDto dto,
            CancellationToken cancellation
            )
        {
            var branch = _domainFactory.CreateDomainBranch
                (
                    id,
                    dto.UrlSegment,
                    dto.Name,
                    dto.Description
                );
            await _branchRepository.CreateBranchProfileAsync(branch, cancellation);
        }


    }
}
