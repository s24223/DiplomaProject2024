using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsBranch.Update;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Create;
using Application.Features.Companies.DTOs.CommandsCompanyBranch.CommandsCompany.Update;
using Application.Features.Companies.Interfaces.CommandsCompanyBranch;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.Services.CommandsCompanyBranch
{
    public class CompanyBranchCommandService : ICompanyBranchCommandService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly ICompanyBranchCommandRepository _repository;
        private readonly IAuthenticationService _authenticationRepository;

        //Cosntructor
        public CompanyBranchCommandService
            (
            IDomainFactory domainFactory,
            ICompanyBranchCommandRepository repository,
            IAuthenticationService authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods

        //Company
        public async Task<ResponseItem<CreateCompanyResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = GetCompanyId(claims);
            var domainBranches = dto.Branches.Select(x => _domainFactory.CreateDomainBranch
                (
                id.Value,
                x.AddressId,
                x.UrlSegment,
                x.Name,
                x.Description
                ));

            var data = DomainBranch.SeparateAndFilterBranches(domainBranches);
            if (dto.Branches.Count() != data.Correct.Count())
            {
                //return duplicates
            }

            var domainCompany = _domainFactory.CreateDomainCompany
                (
                id.Value,
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Regon,
                dto.Description
                );
            domainCompany.AddBranches(domainBranches);
            domainCompany = await _repository.CreateCompanyAsync(domainCompany, cancellation);

            return new ResponseItem<CreateCompanyResponseDto>
            {
                Item = new CreateCompanyResponseDto(domainCompany),
            };
        }

        public async Task<ResponseItem<CompanyResponseDto>> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = GetCompanyId(claims);

            var domainCompany = await _repository.GetCompanyAsync(id, cancellation);
            domainCompany.UpdateData
                (
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Description
                );

            await _repository.UpdateCompanyAsync(domainCompany, cancellation);
            return new ResponseItem<CompanyResponseDto>
            {
                Item = new CompanyResponseDto(domainCompany),
            };
        }


        //Branch
        public async Task<ResponseItems<BranchResponseDto>> CreateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);
            var domainBranches = dtos.Select(x => _domainFactory.CreateDomainBranch
                (
                companyId.Value,
                x.AddressId,
                x.UrlSegment,
                x.Name,
                x.Description
                ));


            domainBranches = await _repository.CreateBranchesAsync(domainBranches, cancellation);
            return new ResponseItems<BranchResponseDto>
            {
                Items = domainBranches.Select(x => new BranchResponseDto(x)).ToList(),
            };
        }

        public async Task<ResponseItems<BranchResponseDto>> UpdateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            var companyId = GetCompanyId(claims);
            var dictionaryDtos = dtos.ToDictionary(x => new BranchId(x.BranchId));
            var dictionaryBranches = await _repository.GetBranchesAsync
                (
                companyId,
                dictionaryDtos.Keys,
                cancellation
                );

            var intersectKeys = dictionaryDtos.Keys.ToHashSet()
                .Intersect(dictionaryBranches.Keys.ToHashSet());

            foreach (var key in intersectKeys)
            {
                var dto = dictionaryDtos[key];
                var branch = dictionaryBranches[key];
                branch.Update
                        (
                        dto.AddressId,
                        dto.UrlSegment,
                        dto.Name,
                        dto.Description
                        );
            }

            dictionaryBranches = await _repository.UpdateBranchesAsync(dictionaryBranches, cancellation);
            return new ResponseItems<BranchResponseDto>
            {
                Items = dictionaryBranches.Select(x => new BranchResponseDto(x.Value)).ToList(),
            };
        }
        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
        private UserId GetCompanyId(IEnumerable<Claim> claims)
        {
            return _authenticationRepository.GetIdNameFromClaims(claims);
        }
    }
}
