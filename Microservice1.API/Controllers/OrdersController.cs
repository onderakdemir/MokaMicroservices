using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedEvents;

namespace Microservice1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IPublishEndpoint _publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            // using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var newEvent = new OrderCreatedEvent() { OrderCode = "abc", TotalPrice = 1200 };

            await _publishEndpoint.Publish(newEvent);

            return Ok();
        }
    }
}