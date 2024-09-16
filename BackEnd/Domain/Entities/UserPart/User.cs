using Domain.Repositories;
using Domain.ValueObjects.ValueEmail;

namespace Domain.Entities.UserPart
{
    /// <summary>
    /// 
    /// </summary>
    ///<exception cref="EmailException"></exception>
    public class User
    {
        private readonly IDomainRepository _repository;

        //From Database
        public Guid? Id { get; private set; } = null; // If not exist in database
        public Email LoginEmail { get; private set; } = null!;
        public DateTime? LastLoginIn { get; private set; } = null;
        public DateTime? LastUpdatePassword { get; private set; } = null;


        public User
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword,
            IDomainRepository repository
            )
        {
            _repository = repository;

            Id = id;
            LoginEmail = new Email(loginEmail);
            LastLoginIn = lastLoginIn;
            LastUpdatePassword = lastUpdatePassword;
        }

    }
}
