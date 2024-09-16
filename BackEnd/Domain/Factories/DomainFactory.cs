using Domain.Entities.UserPart;
using Domain.Repositories;

namespace Domain.Factories
{
    public class DomainFactory : IDomainFactory
    {
        private readonly IDomainRepository _repository;

        public DomainFactory(IDomainRepository repository)
        {
            _repository = repository;
        }

        public User CreateUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword
            )
        {
            return new User(id, loginEmail, lastLoginIn, lastUpdatePassword, _repository);
        }
    }
}
