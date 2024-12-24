//using Microsoft.AspNetCore.Mvc;

//namespace WebApplication1.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> _logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet(Name = "GetWeatherForecast")]
//        public IEnumerable<WeatherForecast> Get()
//        {
//            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateTime.Now.AddDays(index),
//                TemperatureC = Random.Shared.Next(-20, 55),
//                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
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
        private static readonly List<WeatherForecast> Forecasts = new();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

            // Seed with initial data if empty
            if (!Forecasts.Any())
            {
                Forecasts.AddRange(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }));
            }
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Forecasts;
        }

        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast newForecast)
        {
            if (newForecast == null)
            {
                return BadRequest("Invalid weather forecast data.");
            }

            Forecasts.Add(newForecast);
            return CreatedAtAction(nameof(Get), new { id = newForecast.Date }, newForecast);
        }

        [HttpPut]
        public IActionResult Put([FromBody] WeatherForecast updatedForecast)
        {
            if (updatedForecast == null)
            {
                return BadRequest("Invalid weather forecast data.");
            }

            var existingForecast = Forecasts.FirstOrDefault(f => f.Date == updatedForecast.Date);
            if (existingForecast == null)
            {
                return NotFound("Weather forecast not found.");
            }

            existingForecast.TemperatureC = updatedForecast.TemperatureC;
            existingForecast.Summary = updatedForecast.Summary;
            return NoContent();
        }

        [HttpDelete("{date}")]
        public IActionResult Delete(DateTime date)
        {
            var forecastToRemove = Forecasts.FirstOrDefault(f => f.Date.Date == date.Date);
            if (forecastToRemove == null)
            {
                return NotFound("Weather forecast not found.");
            }

            Forecasts.Remove(forecastToRemove);
            return NoContent();
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
