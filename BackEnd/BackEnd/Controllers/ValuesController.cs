using Domain.Features.Notification.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IDomainUserDictionariesRepository _rep;

        public ValuesController(IDomainUserDictionariesRepository rep)
        {
            _rep = rep;
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> Get()
        {
            return Ok(_rep.GetNotificationStatuses());
        }


        [HttpGet("senders")]
        public async Task<IActionResult> Get2()
        {
            return Ok(_rep.GetNotificationSenders());
        }
    }
}
