using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Shared.Interfaces.EntityToDomainMappers;
using Application.Shared.Interfaces.Exceptions;
using Domain.Features.Notification.Entities;
using Domain.Features.Notification.Exceptions;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Interfaces.CommandsNotification
{
    public class NotificationCommandRepository : INotificationCommandRepository
    {
        //Vaues
        private readonly IEntityToDomainMapper _mapper;
        private readonly IExceptionsRepository _exceptionsRepository;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public NotificationCommandRepository
            (
            IEntityToDomainMapper mapper,
            IExceptionsRepository exceptionsRepository,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _exceptionsRepository = exceptionsRepository;
            _context = context;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        //DML

        public async Task<Guid> CreateAsync
            (
            DomainNotification domainNotification,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseNotification = new Notification
                {
                    UserId = domainNotification.UserId?.Value,
                    Email = domainNotification.Email?.Value,
                    NotificationSenderId = domainNotification.NotificationSender.Id,
                    NotificationStatusId = domainNotification.NotificationStatus.Id,
                    Created = domainNotification.Created,
                    Completed = domainNotification.Completed,
                    PreviousProblemId = domainNotification.PreviousProblemId?.Value,
                    IdAppProblem = domainNotification.IdAppProblem == null ?
                                        null : domainNotification.IdAppProblem.Value,
                    UserMessage = domainNotification.UserMessage,
                    Response = domainNotification.Response,
                    IsReadedByUser = domainNotification.IsReadedAnswerByUser.Code
                };

                await _context.Notifications.AddAsync(databaseNotification, cancellation);
                await _context.SaveChangesAsync(cancellation);
                return databaseNotification.Id;
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task UpdateAsync
            (
            UserId userId,
            DomainNotification domainNotification,
            CancellationToken cancellation
            )
        {
            try
            {
                var databaseNotification = await GetDatabaseNotificationAsync
                    (userId,
                    domainNotification.Id,
                    cancellation
                    );

                databaseNotification.UserId = domainNotification.UserId?.Value;
                databaseNotification.Email = domainNotification.Email?.Value;
                databaseNotification.NotificationSenderId = domainNotification.NotificationSender.Id;
                databaseNotification.NotificationStatusId = domainNotification.NotificationStatus.Id;
                databaseNotification.Created = domainNotification.Created;
                databaseNotification.Completed = domainNotification.Completed;
                databaseNotification.PreviousProblemId = domainNotification.PreviousProblemId?.Value;
                databaseNotification.IdAppProblem = domainNotification.IdAppProblem == null ?
                                    null : domainNotification.IdAppProblem.Value;
                databaseNotification.UserMessage = domainNotification.UserMessage;
                databaseNotification.Response = domainNotification.Response;
                databaseNotification.IsReadedByUser = domainNotification.IsReadedAnswerByUser.Code;

                await _context.SaveChangesAsync(cancellation);
            }
            catch (System.Exception ex)
            {
                throw _exceptionsRepository.ConvertEFDbException(ex);
            }
        }

        public async Task<DomainNotification> GetNotificationAsync
            (
            UserId userId,
            NotificationId id,
            CancellationToken cancellation
            )
        {
            var databaseNotification = await GetDatabaseNotificationAsync
                    (userId,
                    id,
                    cancellation
                    );

            return _mapper.ToDomainNotification(databaseNotification);
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
        private async Task<Notification> GetDatabaseNotificationAsync
            (
            UserId userId,
            NotificationId id,
            CancellationToken cancellation
            )
        {
            var notification = await _context.Notifications.Where(x =>
                x.Id == id.Value && x.UserId == userId.Value
                ).FirstOrDefaultAsync(cancellation);

            if (notification == null)
            {
                throw new NotificationException
                    (
                    Messages.Notification_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return notification;
        }
    }
}
