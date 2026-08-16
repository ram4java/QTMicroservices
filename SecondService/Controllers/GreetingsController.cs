using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecondService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        [HttpGet]
        [Route("GreetUsers")]
        public ActionResult GreetUsers()
        {
            return Ok("Welcome to Microservices Project");
        }
    }
}
