using Application.Features.Characteristics.Services.Queries;
using Domain.Features.Notification.Repositories;
using Domain.Features.Url.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {

        private readonly IDomainNotificationDictionariesRepository _notificationDictionaries;
        private readonly IDomainUrlTypeDictionariesRepository _urlTypeDictionaries;
        private readonly ICharacteristicService _characteristicService;

        public DictionariesController
            (
            IDomainNotificationDictionariesRepository notificationDictionaries,
            IDomainUrlTypeDictionariesRepository urlTypeDictionaries,
            ICharacteristicService characteristicService
            )
        {
            _notificationDictionaries = notificationDictionaries;
            _urlTypeDictionaries = urlTypeDictionaries;
            _characteristicService = characteristicService;
        }

        [HttpGet("notification/statuses")]
        public IActionResult GetNotificationStatuses()
        {
            return Ok(_notificationDictionaries.GetNotificationStatuses());
        }


        [HttpGet("notification/senders")]
        public IActionResult GetNotificationSenders()
        {
            return Ok(_notificationDictionaries.GetNotificationSenders());
        }

        [HttpGet("url/types")]
        public IActionResult GetDomainUrlTypeDictionary()
        {
            return Ok(_urlTypeDictionaries.GetDomainUrlTypeDictionary());
        }

        [HttpGet("characteristicTypes")]
        public async Task<IActionResult> GetCharacteristicTypesAsync
            (
            CancellationToken cancellation
            )
        {
            return Ok(await _characteristicService.GetCharacteristicTypesAsync(cancellation));
        }
    }
}
