using EPR.Producer.PRN.Facade.API.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Producer.PRN.Facade.API.Controllers
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
        private readonly IFeatureManager _featureManager;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var addAdditionalFeature = await  _featureManager.IsEnabledAsync(FeatureFlags.AdditionalFeature);


            if (addAdditionalFeature)
            {
                return Ok(Enumerable.Range(1, 5).Select(index => new {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    AdditionalFlag = "With feature flag"
                })
                .ToArray());

            }
            else
            {
                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray());
            }
        }

        [FeatureGate(FeatureFlags.UpdateWeatherForecast)]
        [HttpPost(Name = "UpdateWeatherForecast")]
        public IActionResult Post()
        {
            return Ok("Testing Feature Management");
        }
    }
}
