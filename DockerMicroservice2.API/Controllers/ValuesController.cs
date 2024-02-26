using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerMicroservice2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var headers = Request.Headers;

            using (var activity = ActivitySourceProvider.Source.StartActivity(parentContext: new ActivityContext()))
            {
            }

            var orderId = Activity.Current?.GetBaggageItem("order.id");
            return Ok("Microservice2 Value");
        }
    }
}