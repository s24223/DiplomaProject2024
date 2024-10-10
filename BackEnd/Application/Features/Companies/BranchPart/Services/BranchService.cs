﻿using Application.Features.Companies.BranchPart.DTOs.Create;
using Application.Features.Companies.BranchPart.DTOs.Update;
using Application.Features.Companies.BranchPart.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.BranchPart.Services
{
    public class BranchService : IBranchService
    {
        //Values
        //private readonly IProvider _domainProvider;
        private readonly IBranchRepository _repository;
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationService _authenticationRepository;


        //Cosntructor
        public BranchService
            (
            //IProvider domainProvider,
            IDomainFactory domainFactory,
            IBranchRepository repository,
            IAuthenticationService authentication
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;
            _authenticationRepository = authentication;
            //_domainProvider = domainProvider;
        }


        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Public Methods
        //DML
        public async Task<ResponseItem<CreateBranchResponseDto>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateBranchRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainBranch = _domainFactory.CreateDomainBranch
                (
                id.Value,
                dto.AddressId,
                dto.UrlSegment,
                dto.Name,
                dto.Description
                );
            var branchId = await _repository.CreateAsync(domainBranch, cancellation);
            return new ResponseItem<CreateBranchResponseDto>
            {
                Item = new CreateBranchResponseDto
                {
                    BranchId = branchId,
                },
            };
        }

        public async Task<Response> UpdateAsync
            (
            IEnumerable<Claim> claims,
            Guid branchId,
            UpdateBranchRequestDto dto,
            CancellationToken cancellation
            )
        {
            var id = _authenticationRepository.GetIdNameFromClaims(claims);
            var domainBranch = await _repository.GetBranchAsync
                (
                new BranchId(branchId),
                id,
                cancellation
                );
            domainBranch.Update
                (
                dto.AddressId,
                dto.UrlSegment,
                dto.Name,
                dto.Description
                );
            await _repository.UpdateAsync(domainBranch, cancellation);
            return new Response { };
        }

        //DQL
        //=========================================================================================================
        //=========================================================================================================
        //=========================================================================================================
        //Private Methods
    }
}
