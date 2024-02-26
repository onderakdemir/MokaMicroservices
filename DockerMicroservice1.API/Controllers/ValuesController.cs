using DockerMicroservice1.API.Models;
using DockerMicroservice1.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DockerMicroservice1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(Microservice2Services microservice2Services, AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = context.Products.ToList();

            //return Ok(products);

            return Ok(await microservice2Services.GetMicroservice2Value());
        }
    }
}