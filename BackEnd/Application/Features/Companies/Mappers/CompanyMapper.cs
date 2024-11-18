using Application.Databases.Relational.Models;
using Domain.Features.Branch.Entities;
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


        //Cosnructor
        public CompanyMapper
            (
            IDomainFactory domainFactory
            )
        {
            _domainFactory = domainFactory;
        }


        //================================================================================================================
        //================================================================================================================
        //================================================================================================================
        public DomainCompany DomainCompany(Company databaseCompany)
        {
            return _domainFactory.CreateDomainCompany
                (
                databaseCompany.UserId,
                databaseCompany.UrlSegment,
                databaseCompany.ContactEmail,
                databaseCompany.Name,
                databaseCompany.Regon,
                databaseCompany.Description,
                databaseCompany.Created
                );
        }

        public DomainBranch DomainBranch(Branch databaseBranch)
        {
            return _domainFactory.CreateDomainBranch
                (
                databaseBranch.Id,
                databaseBranch.CompanyId,
                databaseBranch.AddressId,
                databaseBranch.UrlSegment,
                databaseBranch.Name,
                databaseBranch.Description
                );
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

    }
}
