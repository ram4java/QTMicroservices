using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HelloController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        //[Route("SayHello")]
        public async Task<ActionResult> SayHello()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ExternalApiClient");
                var response = await client.GetAsync("https://localhost:7000/Greetings");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    data += new { hello = "Hello Everyone" };
                    return Ok(data);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "External API failed");
                }
                //return Ok("Hello Everyone");
                //return Ok(new {hello = "Hello Everyone"});
            }
            catch (Exception ex)
            {
                // Circuit breaker will throw if the circuit is open
                return StatusCode(503, $"Request failed: {ex.Message}");
            }
        }
    }
}
