using Domain.Features.Address.Entities;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Comment.Entities;
using Domain.Features.Company.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Shared.Providers;
using Domain.Shared.ValueObjects;

namespace Domain.Shared.Factories
{
    public class DomainFactory : IDomainFactory
    {
        //Values
        private readonly IProvider _provider;


        //Constructor
        public DomainFactory(IProvider provider)
        {
            _provider = provider;
        }


        //Methods
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================

        //User Module

        //DomainUser Part
        public DomainUser CreateDomainUser(string login)
        {
            return new DomainUser
                (
                null,
                login,
                null,
                null,
                _provider
                );
        }
        public DomainUser CreateDomainUser
            (
            Guid id,
            string? login,
            DateTime? lastLoginIn,
            DateTime lastPasswordUpdate
            )
        {
            return new DomainUser
                (
                id,
                login,
                lastLoginIn,
                lastPasswordUpdate,
                _provider
                );
        }

        //DomainUrl Part
        public DomainUrl CreateDomainUrl
           (
           Guid userId,
           int urlTypeId,
           string path,
           string? name,
           string? description
           )
        {
            return new DomainUrl
                (
                userId,
                urlTypeId,
                _provider.TimeProvider().GetDateTimeNow(),
                path,
                name,
                description,
                _provider
                );
        }

        public DomainUrl CreateDomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime created,
            string path,
            string? name,
            string? description
            )
        {
            return new DomainUrl
                (
                userId,
                urlTypeId,
                created,
                path,
                name,
                description,
                _provider
                );
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Person Module

        public DomainPerson CreateDomainPerson
            (
            Guid id,
            string? segmentUrl,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            bool isStudent,
            bool isPublicProfile,
            Guid? addressId
            )
        {
            return new DomainPerson
                (
                id,
                segmentUrl,
                null,
                contactEmail,
                name,
                surname,
                birthDate,
                contactPhoneNum,
                description,
                new DatabaseBool(isStudent).Code,
                new DatabaseBool(isPublicProfile).Code,
                addressId,
                _provider
                );
        }

        public DomainPerson CreateDomainPerson
            (
            Guid id,
            string? segmentUrl,
            DateOnly created,
            string? contactEmail,
            string? name,
            string? surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId
            )
        {

            return new DomainPerson
                (
                id,
                segmentUrl,
                null,
                contactEmail,
                name,
                surname,
                birthDate,
                contactPhoneNum,
                description,
                new DatabaseBool(isStudent).Code,
                new DatabaseBool(isPublicProfile).Code,
                addressId,
                _provider
                );
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Company Module

        //Company Part
        public DomainCompany CreateDomainCompany
            (
            Guid id,
            string? segmentUrl,
            string contactEmail,
            string name,
            string regon,
            string? description
            )
        {
            return new DomainCompany
                (
                id,
                segmentUrl,
                contactEmail,
                name,
                regon,
                description,
                null,
                _provider
                );
        }

        public DomainCompany CreateDomainCompany
            (
            Guid id,
            string? segmentUrl,
            string? contactEmail,
            string? name,
            string? regon,
            string? description,
            DateOnly created
            )
        {
            return new DomainCompany
                (
                 id,
                segmentUrl,
                contactEmail,
                name,
                regon,
                description,
                created,
                _provider
                );
        }


        //DomainBranch
        public DomainBranch CreateDomainBranch
           (
           Guid companyId,
           Guid addressId,
           string? segmentUrl,
           string name,
           string? description
           )
        {
            return new DomainBranch
                (
                null,
                companyId,
                addressId,
                segmentUrl,
                name,
                description,
                _provider
                );
        }

        public DomainBranch CreateDomainBranch
            (
            Guid id,
            Guid companyId,
            Guid? addressId,
            string? segmentUrl,
            string? name,
            string? description
            )
        {
            return new DomainBranch
                (
                id,
                companyId,
                addressId,
                segmentUrl,
                name,
                description,
                _provider
                );
        }

        //DomainOffer
        public DomainOffer CreateDomainOffer
           (
           string? name,
           string? description,
           decimal? minSalary,
           decimal? maxSalary,
           bool? isNegotiatedSalary,
           bool isForStudents
           )
        {
            return new DomainOffer
                (
                null,
                name,
                description,
                minSalary,
                maxSalary,
                isNegotiatedSalary == null ?
                            null : new DatabaseBool(isNegotiatedSalary.Value).Code,
                new DatabaseBool(isForStudents).Code,
                _provider
                );
        }

        public DomainOffer CreateDomainOffer
          (
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? isNegotiatedSalary,
           string isForStudents
          )
        {
            return new DomainOffer
                (
                null,
                name,
                description,
                minSalary,
                maxSalary,
                string.IsNullOrWhiteSpace(isNegotiatedSalary) ?
                            null : new DatabaseBool(isNegotiatedSalary).Code,
                new DatabaseBool(isForStudents).Code,
                _provider
                );
        }

        public DomainOffer CreateDomainOffer
           (
           Guid id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           bool? isNegotiatedSalary,
           bool isForStudents
           )
        {
            return new DomainOffer
                (
                id,
                name,
                description,
                minSalary,
                maxSalary,
                isNegotiatedSalary == null ?
                            null : new DatabaseBool(isNegotiatedSalary.Value).Code,
                new DatabaseBool(isForStudents).Code,
                _provider
                );
        }

        public DomainOffer CreateDomainOffer
           (
           Guid id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? isNegotiatedSalary,
           string isForStudents
           )
        {
            return new DomainOffer
                (
                id,
                name,
                description,
                minSalary,
                maxSalary,
                string.IsNullOrWhiteSpace(isNegotiatedSalary) ?
                            null : new DatabaseBool(isNegotiatedSalary).Code,
                new DatabaseBool(isForStudents).Code,
                _provider
                );
        }

        //BranchOffer
        public DomainBranchOffer CreateDomainBranchOffer
           (
            Guid branchId,
            Guid offerId,
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
           )
        {
            return new DomainBranchOffer
                (
                null,
                branchId,
                offerId,
                null,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                null,
                _provider
                );
        }
        public DomainBranchOffer CreateDomainBranchOffer
           (
            Guid id,
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            DateTime lastUpdate
           )
        {
            return new DomainBranchOffer
               (
               id,
               branchId,
               offerId,
               created,
               publishStart,
               publishEnd,
               workStart,
               workEnd,
               lastUpdate,
               _provider
               );
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Recruitment Part
        public DomainRecruitment CreateDomainRecruitment
            (
            Guid personId,
            Guid branchOfferId,
            string? personMessage
            )
        {
            return new DomainRecruitment
                (
                null,
                personId,
                branchOfferId,
                null,
                personMessage,
                null,
                null,
                _provider
                );
        }


        public DomainRecruitment CreateDomainRecruitment
            (
            Guid? id,
            Guid personId,
            Guid branchOfferId,
            DateTime created,
            string? personMessage,
            string? companyResponse,
            string? isAccepted
            )
        {
            return new DomainRecruitment
               (
               id,
               personId,
               branchOfferId,
               created,
               personMessage,
               companyResponse,
               isAccepted,
               _provider
               );
        }

        public DomainIntership CreateDomainInternship
            (
            Guid id,
            DateOnly contractStartDate,
            DateOnly? contractEndDate,
            string contractNumber
            )
        {
            return new DomainIntership
                (
                id,
                null,
                contractStartDate,
                contractEndDate,
                contractNumber,
                _provider
                );
        }

        public DomainIntership CreateDomainIntership
            (
            Guid id,
            DateTime created,
            DateOnly contractStartDate,
            DateOnly? contractEndDate,
            string contractNumber
            )
        {
            return new DomainIntership
                (
                id,
                created,
                contractStartDate,
                contractEndDate,
                contractNumber,
                _provider
                );
        }

        public DomainComment CreateDomainComment
             (
            Guid internshipId,
            int commentTypeId,
            string description,
            int? evaluation
            )
        {
            return new DomainComment
                (
                internshipId,
                commentTypeId,
                _provider.TimeProvider().GetDateTimeNow(),
                description,
                evaluation,
                _provider
                );
        }

        public DomainComment CreateDomainComment
             (
            Guid internshipId,
            int commentTypeId,
            DateTime created,
            string description,
            int? evaluation
            )
        {
            return new DomainComment
                (
                internshipId,
                commentTypeId,
                created,
                description,
                evaluation,
                _provider
                );
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Address Part
        public DomainAddress CreateDomainAddress
            (
            int divisionId,
            int streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode
            )
        {
            return new DomainAddress
                (
                null,
                divisionId,
                streetId,
                buildingNumber,
                apartmentNumber,
                zipCode,
            _provider
            );
        }

        public DomainAddress CreateDomainAddress
            (
            Guid id,
            int divisionId,
            int streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode
            )
        {
            return new DomainAddress
                (
                id,
                divisionId,
                streetId,
                buildingNumber,
                apartmentNumber,
                zipCode,
            _provider
            );
        }

    }
}
