using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        [Route("SayHello")]
        public ActionResult SayHello()
        {
            return Ok("Hello Everyone");
        }
    }
}
