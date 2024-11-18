﻿using Application.Features.Users.Commands.Notifications.DTOs.Create;
using Application.Features.Users.Commands.Notifications.DTOs.Create.Authorize;
using Application.Features.Users.Commands.Notifications.DTOs.Create.Unauthorize;
using Application.Features.Users.Commands.Notifications.Interfaces;
using Application.Shared.DTOs.Response;
using Application.Shared.Services.Authentication;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Shared.Factories;
using System.Security.Claims;

namespace Application.Features.Users.Commands.Notifications.Services
{
    public class NotificationCommandService : INotificationCommandService
    {
        //Values
        private readonly IDomainFactory _domainFactory;
        private readonly IAuthenticationSvc _authentication;
        private readonly INotificationCommandRepository _notificationRepository;


        //Cosntructor
        public NotificationCommandService
            (
            IDomainFactory domainFactory,
            IAuthenticationSvc authentication,
            INotificationCommandRepository notificationRepository
            )
        {
            _domainFactory = domainFactory;
            _authentication = authentication;
            _notificationRepository = notificationRepository;
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        //Unautorize
        public async Task<ResponseItem<CreateNotificationRequestDto>> CreateUnauthorizeAsync
            (
            CreateUnauthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            )
        {
            var domainNotification = _domainFactory.CreateDomainNotification
                (
                null,
                dto.Email,
                dto.IdAppProblem,
                dto.PreviousProblemId,
                dto.UserMessage,
                1,
                1
                );
            var id = await _notificationRepository.CreateAsync(domainNotification, cancellation);

            return new ResponseItem<CreateNotificationRequestDto>
            {
                Item = new CreateNotificationRequestDto
                {
                    NotificationId = id,
                }
            };
        }


        //Autorize
        public async Task<ResponseItem<CreateNotificationRequestDto>> CreateAuthorizeAsync
            (
            IEnumerable<Claim> claims,
            CreateAuthorizeNotificationRequestDto dto,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var domainNotification = _domainFactory.CreateDomainNotification
                (
                userId.Value,
                null,
                dto.IdAppProblem,
                dto.PreviousProblemId,
                dto.UserMessage,
                1,
                1
                );
            var id = await _notificationRepository.CreateAsync(domainNotification, cancellation);

            return new ResponseItem<CreateNotificationRequestDto>
            {
                Item = new CreateNotificationRequestDto
                {
                    NotificationId = id,
                }
            };
        }

        public async Task<Response> AnnulAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var domainNotification = await _notificationRepository.GetNotificationAsync
                (
                userId,
                new NotificationId(notificationId),
                cancellation
                );
            domainNotification.Annul();

            await _notificationRepository.UpdateAsync
                (
                userId,
                domainNotification,
                cancellation
                );
            return new Response { };
        }

        public async Task<Response> ReadAsync
            (
            IEnumerable<Claim> claims,
            Guid notificationId,
            CancellationToken cancellation
            )
        {
            var userId = _authentication.GetIdNameFromClaims(claims);
            var domainNotification = await _notificationRepository.GetNotificationAsync
                (
                userId,
                new NotificationId(notificationId),
                cancellation
                );
            domainNotification.SetReadedByUser();

            await _notificationRepository.UpdateAsync
                (
                userId,
                domainNotification,
                cancellation
                );
            return new Response { };
        }



        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
    }
}
