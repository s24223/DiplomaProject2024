using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsBranch.Update;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Create;
using Application.Features.Companies.Commands.CompanyBranches.DTOs.CommandsCompany.Update;
using Application.Features.Companies.Commands.CompanyBranches.Interfaces;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Companies.Commands.CompanyBranches.Services
{
    public class CompanyBranchCmdSvc : ICompanyBranchCmdSvc
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly ICompanyBranchCmdRepo _repository;
        private readonly IAuthJwtSvc _authenticationRepository;

        //Cosntructor
        public CompanyBranchCmdSvc
            (
            IDomainFactory domainFactory,
            ICompanyBranchCmdRepo repository,
            IAuthJwtSvc authentication
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
        public async Task<ResponseItem<CreateCompanyResp>> CreateAsync
            (
            IEnumerable<Claim> claims,
            CreateCompanyReq dto,
            CancellationToken cancellation
            )
        {
            var id = GetCompanyId(claims);
            var domainCompany = _domainFactory.CreateDomainCompany
                (
                id.Value,
                dto.UrlSegment,
                dto.ContactEmail,
                dto.Name,
                dto.Regon,
                dto.Description
                );
            var domainBranches = dto.Branches.Select(x => _domainFactory.CreateDomainBranch
                (
                id.Value,
                x.AddressId,
                x.UrlSegment,
                x.Name,
                x.Description
                ));


            //Check Duplicates
            var companyStringDuplicates = await _repository
                .CheckDbDuplicatesCompanyAsync(domainCompany, true, cancellation);
            if (companyStringDuplicates != null)
            {
                throw new PersonException(companyStringDuplicates);
            }

            var dataBeforeDb = DomainBranch.SeparateAndFilterBranches(domainBranches);
            if (dataBeforeDb.HasDuplicates)
            {
                return new ResponseItem<CreateCompanyResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_InputDuplicate,
                    Item = new CreateCompanyResp
                        (domainCompany, dataBeforeDb.Items, true, dataBeforeDb.HasDuplicates),
                };
            }

            var dataAfterDb = await _repository
                .CheckDbDuplicatesBranchesCreateAsync(domainBranches, cancellation);
            if (dataAfterDb.HasDuplicates)
            {
                return new ResponseItem<CreateCompanyResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_DbDuplicate,
                    Item = new CreateCompanyResp
                        (domainCompany, dataAfterDb.Items, false, dataAfterDb.HasDuplicates),
                };
            }


            //Save DB
            domainCompany.AddBranches(domainBranches);
            domainCompany = await _repository.CreateCompanyAsync(domainCompany, cancellation);

            return new ResponseItem<CreateCompanyResp>
            {
                Status = EnumResponseStatus.Success,
                Item = new CreateCompanyResp(domainCompany),
            };
        }

        public async Task<ResponseItem<CompanyResp>> UpdateCompanyAsync
            (
            IEnumerable<Claim> claims,
            UpdateCompanyReq dto,
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

            var companyStringDuplicates = await _repository
                .CheckDbDuplicatesCompanyAsync(domainCompany, false, cancellation);
            if (companyStringDuplicates != null)
            {
                throw new PersonException(companyStringDuplicates);
            }


            await _repository.UpdateCompanyAsync(domainCompany, cancellation);
            return new ResponseItem<CompanyResp>
            {
                Item = new CompanyResp(domainCompany),
            };
        }


        //Branch
        public async Task<ResponseItems<CreateBranchesResp>> CreateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<CreateBranchReq> dtos,
            CancellationToken cancellation
            )
        {
            //Prepare Data
            var companyId = GetCompanyId(claims);
            var domainBranches = dtos.Select(x => _domainFactory.CreateDomainBranch
                (
                companyId.Value,
                x.AddressId,
                x.UrlSegment,
                x.Name,
                x.Description
                ));

            //Check Duplicates
            var dataBeforeDb = DomainBranch.SeparateAndFilterBranches(domainBranches);
            if (dataBeforeDb.HasDuplicates)
            {
                return new ResponseItems<CreateBranchesResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_InputDuplicate,
                    Items = dataBeforeDb.Items
                        .Select(x => new CreateBranchesResp
                            (x.Item, x.IsDuplicate, true, true))
                        .ToList(),
                };
            }

            var dataAfterDb = await _repository
                .CheckDbDuplicatesBranchesCreateAsync(domainBranches, cancellation);
            if (dataAfterDb.HasDuplicates)
            {
                return new ResponseItems<CreateBranchesResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_DbDuplicate,
                    Items = dataAfterDb.Items
                        .Select(x => new CreateBranchesResp
                            (x.Item, x.IsDuplicate, false, true))
                        .ToList(),
                };
            }

            //Save Database
            var outputData = await _repository.CreateBranchesAsync(domainBranches, cancellation);
            return new ResponseItems<CreateBranchesResp>
            {
                Items = outputData
                        .Select(x => new CreateBranchesResp
                            (x, false, false, false))
                        .ToList(),
            };
        }

        public async Task<ResponseItems<UpdateBranchesResp>> UpdateBranchesAsync
            (
            IEnumerable<Claim> claims,
            IEnumerable<UpdateBranchRequestDto> dtos,
            CancellationToken cancellation
            )
        {
            //Prepare Data
            var companyId = GetCompanyId(claims);

            //Here 
            var dictionaryDtos = dtos.ToDictionary(x => new BranchId(x.BranchId));
            var dictionaryBranches = await _repository.GetBranchesAsync
                (
                companyId,
                dictionaryDtos.Keys,
                cancellation
                );

            foreach (var key in dictionaryDtos.Keys)
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

            //Check Duplicates
            var dataBeforeDb = DomainBranch.SeparateAndFilterBranches(dictionaryBranches.Values);
            if (dataBeforeDb.HasDuplicates)
            {
                return new ResponseItems<UpdateBranchesResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_InputDuplicate,
                    Items = dataBeforeDb.Items
                        .Select(x => new UpdateBranchesResp
                            (x.Item, x.IsDuplicate, true))
                        .ToList(),
                };
            }
            var dataAfterDb = await _repository
                .CheckDbDuplicatesBranchesUpdateAsync(dictionaryBranches, cancellation);
            if (dataAfterDb.HasDuplicates)
            {
                return new ResponseItems<UpdateBranchesResp>
                {
                    Status = EnumResponseStatus.UserFault,
                    Message = Messages.Branch_Cmd_UrlSegmet_DbDuplicate,
                    Items = dataAfterDb.Items
                        .Select(x => new UpdateBranchesResp
                            (x.Item, x.IsDuplicate, false))
                        .ToList(),
                };
            }

            //Save DB
            var databaseData = await _repository.UpdateBranchesAsync(dictionaryBranches, cancellation);
            return new ResponseItems<UpdateBranchesResp>
            {
                Items = dataAfterDb.Items
                        .Select(x => new UpdateBranchesResp(x.Item))
                        .ToList(),
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
