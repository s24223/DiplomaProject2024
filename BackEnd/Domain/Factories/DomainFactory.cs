using Domain.Entities.CompanyPart;
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
    }
}
