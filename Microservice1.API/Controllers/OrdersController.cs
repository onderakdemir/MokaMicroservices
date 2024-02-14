using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedEvents;

namespace Microservice1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IPublishEndpoint _publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            // using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var newEvent = new OrderCreatedEvent() { OrderCode = "abc", TotalPrice = 1200 };

            await _publishEndpoint.Publish(newEvent);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update()
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:user.crated.event.queue"));


            var newEvent = new UserCreatedEvent("ahmet", "ahmet@outlook.com");

            await sendEndpoint.Send(newEvent);

            return Ok();
        }
    }
}