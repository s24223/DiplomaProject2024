using Domain.Entities.UserPart;
using Domain.Providers;

namespace Domain.Factories
{
    public class DomainFactory : IDomainFactory
    {
        private readonly IDomainProvider _repository;

        public DomainFactory(IDomainProvider repository)
        {
            _repository = repository;
        }

        public DomainUser CreateDomainUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword
            )
        {
            return new DomainUser(id, loginEmail, lastLoginIn, lastUpdatePassword, _repository);
        }
    }
}
