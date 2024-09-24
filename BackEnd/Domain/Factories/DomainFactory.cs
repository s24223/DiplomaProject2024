using Domain.Entities.AddressPart;
using Domain.Entities.CompanyPart;
using Domain.Entities.PersonPart;
using Domain.Entities.RecrutmentPart;
using Domain.Entities.UserPart;
using Domain.Providers;
using Domain.ValueObjects.PartUrlType;

namespace Domain.Factories
{
    public class DomainFactory : IDomainFactory
    {
        private readonly IDomainProvider _provider;

        public DomainFactory(IDomainProvider provider)
        {
            _provider = provider;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Company Part
        public DomainBranch CreateDomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            )
        {
            return new DomainBranch
                (
                id,
                companyId,
                addressId,
                urlSegment,
                name,
                description,
                _provider
                );
        }

        public DomainBranchOffer CreateDomainBranchOffer
           (
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

        public DomainCompany CreateDomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description,
            DateOnly? createDate
            )
        {
            return new DomainCompany
                (
                id,
                urlSegment,
                contactEmail,
                name,
                regon,
                description,
                createDate,
        _provider
                );
        }

        public DomainOffer CreateDomainOffer
           (
           Guid? id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? NegotiatedSalary,
           string forStudents
           )
        {
            return new DomainOffer
                (
                id,
                name,
                description,
                minSalary,
                maxSalary,
                NegotiatedSalary,
                forStudents,
            _provider
           );
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Person Part
        public DomainPerson CreateDomainPerson
            (
            Guid id,
            string? urlSegment,
            DateOnly? createDate,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId
            )
        {
            return new DomainPerson(
                id,
                urlSegment,
                createDate,
                contactEmail,
                name,
                surname,
                birthDate,
                contactPhoneNum,
                description,
                isStudent,
                isPublicProfile,
                addressId,
                _provider
                );
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Person Part
        public DomainComment CreateDomainComment
            (
            Guid internshipId,
            int commentTypeId,
            DateTime published,
            string description,
            int? evaluation
            )
        {
            return new DomainComment
                (
                 internshipId,
                 commentTypeId,
                 published,
                 description,
                 evaluation,
                 _provider
            );
        }

        public DomainIntership CreateDomainIntership
            (
            Guid? id,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            string contractNumber
            )
        {
            return new DomainIntership
                (
            id,
             personId,
             branchId,
             offerId,
             created,
             contractNumber,
             _provider
            );
        }

        public DomainRecrutment CreateDomainRecrutment
            (
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime? applicationDate,
            string? personMessage,
            string? companyResponse,
            string? acceptedRejected
            )
        {
            return new DomainRecrutment
                (
                 personId,
                 branchId,
                 offerId,
                 created,
                 applicationDate,
                 personMessage,
                 companyResponse,
                 acceptedRejected,
                 _provider
            );
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //User Part
        public DomainUrl CreateDomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime publishDate,
            string url,
            string? name,
            string? description
            )
        {
            return new DomainUrl
                (
                 userId,
                 urlTypeId,
                 publishDate,
                 url,
                 name,
                 description,
                _provider
            );
        }

        public DomainUser CreateDomainUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword
            )
        {
            return new DomainUser(id, loginEmail, lastLoginIn, lastUpdatePassword, _provider);
        }

        public DomainUserProblem CreateDomainUserProblem
            (
            Guid? id,
            DateTime? dateTime,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string? status,
            Guid? userId
            )
        {
            return new DomainUserProblem
                (
                id,
                dateTime,
                userMessage,
                response,
                previousProblemId,
                email,
                status,
                userId,
            _provider
            );
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //UrlPart

        public DomainUrl CreateDomainUrl(
            Guid UserId, 
            UrlType urlType,
            DateTime publishDate, 
            string url, 
            string? name, 
            string? description, 
            IDomainProvider provider)
        {
            return new DomainUrl
                (
                UserId,
                (int) urlType.Type,
                publishDate,
                url,
                name,
                description,
                provider
                );
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Address Part
        public DomainAddress CreateDomainAddress
            (
            Guid? id,
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

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //BranchOffer Part
        public DomainBranchOffer CreateDomainBranchOffer(Guid branchId, Guid offerId, DateTime created, DateTime publishStart, DateTime? publishEnd, DateOnly? workStart, DateOnly? workEnd, DateTime lastUpdate, IDomainProvider provider)
        {
            return new DomainBranchOffer(
                branchId,
                offerId,
                created,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                lastUpdate,
                provider);
        }
    }
}
