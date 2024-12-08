using Application.Databases.Relational.Models;
using Application.Features.Addresses.Queries.Interfaces;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.Offer.Entities;
using Domain.Shared.Factories;

namespace Application.Features.Companies.Mappers
{
    public class CompanyMapper : ICompanyMapper
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAddressQueryRepo _addressRepository;


        //Cosnructor
        public CompanyMapper
            (
            IDomainFactory domainFactory,
            IAddressQueryRepo addressRepository
            )
        {
            _domainFactory = domainFactory;
            _addressRepository = addressRepository;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        public DomainCompany DomainCompany(Company database)
        {
            return _domainFactory.CreateDomainCompany
                (
                database.UserId,
                database.UrlSegment,
                database.ContactEmail,
                database.Name,
                database.Regon,
                database.Description,
                database.Created
                );
        }

        public DomainBranch DomainBranch(Branch database)
        {

            return _domainFactory.CreateDomainBranch
                (
                database.Id,
                database.CompanyId,
                database.AddressId,
                database.UrlSegment,
                database.Name,
                database.Description
                );
        }

        public async Task<DomainBranch> DomainBranchAsync(Branch database, CancellationToken cancellation)
        {
            var branch = DomainBranch(database);
            var address = await _addressRepository.GetAddressAsync(branch.AddressId, cancellation);
            branch.Address = address;
            return branch;
        }

        public async Task<Dictionary<BranchId, DomainBranch>> DomainBranchesAsync
            (IEnumerable<Branch> databases, CancellationToken cancellation)
        {
            if (!databases.Any())
            {
                return [];
            }

            var addressIds = databases
                .Select(x => new AddressId(x.AddressId))
                .ToHashSet();
            var addresses = await _addressRepository
                .GetAddressDictionaryAsync(addressIds, cancellation);

            var dictionary = new Dictionary<BranchId, DomainBranch>();
            foreach (var item in databases)
            {
                var branch = DomainBranch(item);
                branch.Address = addresses[branch.AddressId];
                dictionary[branch.Id] = branch;
            }

            return dictionary;
        }

        public DomainOffer DomainOffer(Offer databaseOffer)
        {
            var domainOffer = _domainFactory.CreateDomainOffer
                (
                databaseOffer.Id,
                databaseOffer.Name,
                databaseOffer.Description,
                databaseOffer.MinSalary,
                databaseOffer.MaxSalary,
                databaseOffer.IsNegotiatedSalary,
                databaseOffer.IsForStudents
                );

            var characteristicIds = databaseOffer.OfferCharacteristics
                .Select(y =>
                    (
                        new CharacteristicId(y.CharacteristicId),
                        y.QualityId.HasValue ?
                            new QualityId(y.QualityId.Value) :
                            null
                    ));

            domainOffer.SetCharacteristics(characteristicIds);
            return domainOffer;
        }

        public DomainBranchOffer DomainBranchOffer(BranchOffer databaseBranchOffer)
        {
            return _domainFactory.CreateDomainBranchOffer
                (
                databaseBranchOffer.Id,
                databaseBranchOffer.BranchId,
                databaseBranchOffer.OfferId,
                databaseBranchOffer.Created,
                databaseBranchOffer.PublishStart,
                databaseBranchOffer.PublishEnd,
                databaseBranchOffer.WorkStart,
                databaseBranchOffer.WorkEnd,
                databaseBranchOffer.LastUpdate
                );
        }

        //================================================================================================
        //================================================================================================
        //================================================================================================


    }
}
