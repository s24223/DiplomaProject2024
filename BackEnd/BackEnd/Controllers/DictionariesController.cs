using Application.Features.Characteristics.Queries.Services;
using Application.Shared.Services.OrderBy;
using Domain.Features.Notification.Repositories;
using Domain.Features.Url.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        //Values
        private readonly IDomainNotificationDictionariesRepository _notificationDictionaries;
        private readonly IDomainUrlTypeDictionariesRepository _urlTypeDictionaries;
        private readonly ICharacteristicQueryService _characteristicService;
        private readonly IOrderBySvc _orderByService;


        //Constructor
        public DictionariesController
            (
            IDomainNotificationDictionariesRepository notificationDictionaries,
            IDomainUrlTypeDictionariesRepository urlTypeDictionaries,
            ICharacteristicQueryService characteristicService,
            IOrderBySvc orderByService
            )
        {
            _notificationDictionaries = notificationDictionaries;
            _urlTypeDictionaries = urlTypeDictionaries;
            _characteristicService = characteristicService;
            _orderByService = orderByService;
        }


        //===========================================================================================================
        //===========================================================================================================
        //===========================================================================================================
        //Public Methods
        [HttpGet("user/notifications/statuses")]
        public IActionResult GetNotificationStatuses()
        {
            return Ok(_notificationDictionaries.GetNotificationStatuses());
        }


        [HttpGet("user/notifications/senders")]
        public IActionResult GetNotificationSenders()
        {
            return Ok(_notificationDictionaries.GetNotificationSenders());
        }

        [HttpGet("user/notifications/orderBy")]
        public IActionResult GetUserNotificationsOrderBy()
        {
            return Ok(_orderByService.UserNotifications());
        }

        [HttpGet("user/urls/types")]
        public IActionResult GetDomainUrlTypeDictionary()
        {
            return Ok(_urlTypeDictionaries.GetDomainUrlTypeDictionary());
        }

        [HttpGet("user/urls/orderBy")]
        public IActionResult GetUserUrlsOrderBy()
        {
            return Ok(_orderByService.UserUrls());
        }

        [HttpGet("characteristics")]
        public IActionResult GetCharacteristicTypesAsync
            (
            bool isOrderByType = true
            )
        {
            if (isOrderByType)
            {
                return Ok(_characteristicService.GetCharacteristicTypes());
            }
            else
            {
                return Ok(_characteristicService.GetCharacteristics());
            }
        }

        //===========================================================================================================
        //===========================================================================================================
        //===========================================================================================================
        //Private Methods
    }
}
