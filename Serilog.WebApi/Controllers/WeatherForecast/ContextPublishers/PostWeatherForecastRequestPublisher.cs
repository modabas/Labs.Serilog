using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class PostWeatherForecastRequestPublisher : AbstractPropertyPopulator<PostWeatherForecastRequest>
{
    public override Task AddProperties(PostWeatherForecastRequest instance, CancellationToken cancellationToken)
    {
        AddProperty("Summary", instance.WeatherForecast?.Summary);
        AddProperty("TemperatureC", instance.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
