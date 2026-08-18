using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SecondService.Messages;

namespace SecondService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private const string _qt_cache_Key = "GreetingsKey";       
        private readonly RabbitmqMessage _rabbitMessage;


        public GreetingsController(
                RabbitmqMessage message, 
                IMemoryCache cache) 
        { 
            _rabbitMessage = message;
            _cache = cache;
        }

        [HttpGet]
        //[Route("GreetUsers")]
        public async Task<ActionResult> GreetUsers()
        {
            //return Ok("Welcome to Microservices Project");
            return Ok(new {greetings = "Welcome to Microservices Project"});
        }

        [HttpGet]
        [Route("SendMessage")]
        public ActionResult sendMessage(string message)
        {
            _rabbitMessage.SendMessage(message);
            return Ok("message sent successfully");
        }

        [HttpGet]
        [Route("CourseList")]
        public async Task<ActionResult> CourseList()
        {
            // 1. Attempt to get data from cache
            if (_cache.TryGetValue(_qt_cache_Key, out List<string>? courses))
            {
                // Cache hit: return data immediately without calling the database
                return Ok(courses);
            }

            //2. if the cache does not have the specified key, get the courses from DB
            courses = await GetCoruses();

            // 3. Configure Cache settings
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)) //expires after 5 mins
                .SetSlidingExpiration(TimeSpan.FromMinutes(3))//lifespan will be re-initiated if api is called within 3mins
                .SetPriority(CacheItemPriority.Normal);//keep in low memory

            // 4. Save data into the cache of RAM
            _cache.Set(_qt_cache_Key, courses, cacheOptions);

            return Ok(courses);
        }

        private async Task<List<string>> GetCoruses()
        {
            await Task.Delay(10000);
            return new List<string> { "DotnetFS", "JavaFS", "PytonFS" };
        }
    }
}
