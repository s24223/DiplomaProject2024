using Application.Databases.Relational;
using Application.Databases.Relational.Models;
using Application.Features.Users.Mappers;
using Domain.Features.Notification.Entities;
using Domain.Features.Notification.Exceptions;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Templates.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.Notifications.Interfaces
{
    public class NotificationCmdRepo : INotificationCmdRepo
    {
        //Vaues
        private readonly IUserMapper _mapper;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public NotificationCmdRepo
            (
            IUserMapper mapper,
            DiplomaProjectContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        //DML
        public async Task<DomainNotification> CreateAsync(DomainNotification domain, CancellationToken cancellation)
        {
            try
            {
                var database = new Notification
                {
                    UserId = domain.UserId?.Value,
                    Email = domain.Email?.Value,
                    NotificationSenderId = domain.NotificationSender.Id,
                    NotificationStatusId = domain.NotificationStatus.Id,
                    Created = domain.Created,
                    Completed = domain.Completed,
                    PreviousProblemId = domain.PreviousProblemId?.Value,
                    IdAppProblem = domain.IdAppProblem == null ? null : domain.IdAppProblem.Value,
                    UserMessage = domain.UserMessage,
                    Response = domain.Response,
                    IsReadedByUser = domain.IsReadedAnswerByUser.Code
                };

                await _context.Notifications.AddAsync(database, cancellation);
                await _context.SaveChangesAsync(cancellation);

                return _mapper.DomainNotification(database);
            }
            catch (System.Exception ex)
            {
                throw ExceptionHandler(ex, domain);
            }
        }

        public async Task<DomainNotification> UpdateAsync
            (
            UserId userId,
            DomainNotification domain,
            CancellationToken cancellation
            )
        {
            try
            {
                var database = await GetDatabaseNotificationAsync(userId, domain.Id, cancellation);

                database.UserId = domain.UserId?.Value;
                database.Email = domain.Email?.Value;
                database.NotificationSenderId = domain.NotificationSender.Id;
                database.NotificationStatusId = domain.NotificationStatus.Id;
                database.Created = domain.Created;
                database.Completed = domain.Completed;
                database.PreviousProblemId = domain.PreviousProblemId?.Value;
                database.IdAppProblem = domain.IdAppProblem == null ? null : domain.IdAppProblem.Value;
                database.UserMessage = domain.UserMessage;
                database.Response = domain.Response;
                database.IsReadedByUser = domain.IsReadedAnswerByUser.Code;

                await _context.SaveChangesAsync(cancellation);

                return _mapper.DomainNotification(database);
            }
            catch (System.Exception ex)
            {
                throw ExceptionHandler(ex, domain);
            }
        }

        public async Task<DomainNotification> GetNotificationAsync
            (
            UserId userId,
            NotificationId id,
            CancellationToken cancellation
            )
        {
            var database = await GetDatabaseNotificationAsync(userId, id, cancellation);
            return _mapper.DomainNotification(database);
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
            var database = await _context.Notifications
                .Where(x => x.Id == id.Value && x.UserId == userId.Value)
                .FirstOrDefaultAsync(cancellation);

            if (database == null)
            {
                throw new NotificationException
                    (
                    $"{Messages.Notification_Cmd_Id_NotFound}: {id.Value}",
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            return database;
        }

        private System.Exception ExceptionHandler(System.Exception ex, DomainNotification domain)
        {
            /*
            Notification_CHECK_IsReadedByUser
            Notification_NotificationStatus
            Notification_NotificationSender
             */
            //547 - CHECK, FK

            Console.WriteLine(ex);
            var exceptions547 = new Dictionary<string, string>()
            {
                {
                    "Notification_CHECK_IsReadedByUser",
                    $"{Messages.Notification_Cmd_CHECK_IsReadedByUser}: {domain.IsReadedAnswerByUser.Code}"
                },
                {
                    "Notification_NotificationStatus",
                    $"{Messages.Notification_Cmd_FK_Notification_NotificationStatus}: {domain.NotificationStatus.Id}"
                },
                {
                    "Notification_NotificationSender",
                    $"{Messages.Notification_Cmd_FK_Notification_NotificationSender}: {domain.NotificationSender.Id}"
                },
            };


            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;

                foreach (var pair in exceptions547)
                {
                    if (message.Contains(pair.Key))
                    {
                        return new NotificationException
                            (
                            pair.Value,
                            DomainExceptionTypeEnum.AppProblem
                            );
                    }
                }
            }
            return ex;
        }
    }
}
