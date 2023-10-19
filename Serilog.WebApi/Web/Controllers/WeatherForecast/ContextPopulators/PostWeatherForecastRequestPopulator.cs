using Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;
using Serilog.WebApi.Web.Controllers.WeatherForecast.Dto;

namespace Serilog.WebApi.Web.Controllers.WeatherForecast.ContextPopulators;

public class PostWeatherForecastRequestPopulator : AbstractPropertyPopulator<PostWeatherForecastRequest>
{
    public override Task SetProperties(PostWeatherForecastRequest instance, CancellationToken cancellationToken)
    {
        SetProperty("Summary", instance.WeatherForecast?.Summary);
        SetProperty("TemperatureC", instance.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
