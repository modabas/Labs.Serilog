using Microsoft.AspNetCore.Mvc;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.LogClasses;
using Serilog.WebApi.Serilog.Loggers;

namespace Serilog.WebApi.Controllers.WeatherForecast;
[ApiController]
[Route("[controller]")]
public partial class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IDataExchangeLogger<WeatherForecastController> _auditLogger;
    private readonly IInterchangeContext _interchangeContext;

    public WeatherForecastController(IDataExchangeLogger<WeatherForecastController> auditLogger, IInterchangeContext interchangeContext)
    {
        _auditLogger = auditLogger;
        _interchangeContext = interchangeContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecastDto>> Get(CancellationToken cancellationToken)
    {
        try
        {
            //throw new InvalidOperationException("I want to log an exception");
            var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            var propertyBag = new List<SampleMessageItem>();
            for (var i = 0; i < 10; i++)
            {
                propertyBag.Add(new SampleMessageItem() { Id = i, Name = $"Data{i:00}" });
            }

            await _interchangeContext.SetProperty("ManuallyPopulatedProperties", propertyBag, cancellationToken);
            await _auditLogger.LogInformation(forecast, cancellationToken);

            return forecast;
        }
        catch (Exception ex)
        {
            await _auditLogger.LogError(ex, "Error while returning weather forecast.");
            throw;
        }
    }

    [HttpPost(Name = "PostWeatherForecast")]
    public async Task<IActionResult> Post([FromBody] PostWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _auditLogger.LogInformation("Post request received.", cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            await _auditLogger.LogError(ex, "Error while returning weather forecast.");
            throw;
        }
    }
}
