using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class PostWeatherForecastRequestPopulator : AbstractPropertyPopulator<PostWeatherForecastRequest>
{
    public override Task SetProperties(PostWeatherForecastRequest instance, CancellationToken cancellationToken)
    {
        SetProperty("Summary", instance.WeatherForecast?.Summary);
        SetProperty("TemperatureC", instance.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
