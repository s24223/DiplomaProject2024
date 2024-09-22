using Domain.Entities.CompanyPart;
using Domain.Entities.PersonPart;
using Domain.Entities.UserPart;
using Domain.Providers;

namespace Domain.Factories
{
    public class DomainFactory : IDomainFactory
    {
        private readonly IDomainProvider _provider;

        public DomainFactory(IDomainProvider provider)
        {
            _provider = provider;
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


    }
}
