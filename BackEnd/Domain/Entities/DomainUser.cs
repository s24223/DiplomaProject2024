using Domain.Entities.Template;
using Domain.Exceptions.UserExceptions;
using Domain.Providers;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    ///<exception cref="EmailException"></exception>
    public class DomainUser : Entity<UserId>
    {
        private readonly IDomainProvider _repository;

        //From Database
        public Email Login { get; private set; } = null!;
        public DateTime? LastLoginIn { get; private set; } = null;
        public DateTime LastUpdatePassword { get; private set; }


        public DomainUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword,
            IDomainProvider repository
            )
            : base(id: new UserId(id))
        {
            _repository = repository;

            Login = new Email(loginEmail);
            LastLoginIn = lastLoginIn;
            LastUpdatePassword = (lastUpdatePassword != null) ?
                lastUpdatePassword.Value :
                _repository.GetTimeProvider().GetDateTimeNow();
        }

    }
}
