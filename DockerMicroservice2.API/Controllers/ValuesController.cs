using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerMicroservice2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Microservice2 Value");
        }
    }
}