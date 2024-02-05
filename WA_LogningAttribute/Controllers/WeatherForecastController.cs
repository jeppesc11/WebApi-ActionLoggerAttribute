using Microsoft.AspNetCore.Mvc;
using WA_LogningAttribute.Attributes;

namespace WA_LogningAttribute.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [ActionLogger()]
        [HttpGet("/test/test/nothing")]
        public void GetNothing()
        {
            _logger.LogInformation("Nothing is fetched");
        }

        [ActionLogger()]
        [HttpPost]
        public void Post([FromBody] WeatherForecast weather)
        {
            _logger.LogInformation("Weather forecast is posted");
        }

       [ActionLogger]
       [HttpGet("{id}")]
       public ActionResult<WeatherForecast> Get(int id)
       {
           return new WeatherForecast();
       }

[ActionLogger]
[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
}
    }
}