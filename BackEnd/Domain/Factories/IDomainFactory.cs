using Domain.Entities.UserPart;
using Domain.Exceptions.UserExceptions;

namespace Domain.Factories
{
    public interface IDomainFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginEmail"></param>
        /// <param name="lastLoginIn"></param>
        /// <param name="lastUpdatePassword"></param>
        /// <returns></returns>
        /// <exception cref="EmailException"></exception>
        public DomainUser CreateDomainUser
           (
           Guid? id,
           string loginEmail,
           DateTime? lastLoginIn,
           DateTime? lastUpdatePassword
           );
    }
}
