using iitSystemWeb;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Test_3.Models;

namespace WebAPI_Test_3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
       public readonly DBContext _DBContext;
       public readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(  IHttpContextAccessor httpContextAccessor, DBContext dBContext )
        {
            Utility.SetClientEnvironment(httpContextAccessor, ref _httpContextAccessor, dBContext, ref _DBContext);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
