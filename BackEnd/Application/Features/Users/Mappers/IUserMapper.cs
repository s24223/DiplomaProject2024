using Application.Databases.Relational.Models;
using Domain.Features.Notification.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;

namespace Application.Features.Users.Mappers
{
    public interface IUserMapper
    {
        DomainUser DomainUser(User databaseUser);
        DomainUrl DomainUrl(Url databaseUrl);
        DomainNotification DomainNotification(Notification notification);
    }
}
