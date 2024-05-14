using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Threading;

using WebApiTools.TestService;
using WebApiTools.Tools.Event_Bus;
using WebApiTools.Tools.Kafkas;


namespace WebApiTools.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController(ITestServices TestService) : ControllerBase
    {

        [HttpPost("KafkaSend",Name = "KafkaSend")]
        public async Task KafkaSend([FromBody] string o)
        {
            await Producer.SendAsync(o);
        }


        [HttpGet("Publish",Name = "Publish")]
        public async Task PublishAsync(MyEvent @event)
        {
            await TestService.PublishAsync(@event, default);
        }


    }
}
