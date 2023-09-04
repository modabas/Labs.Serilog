using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class PostWeatherForecastRequestPublisher : AbstractPublisher<PostWeatherForecastRequest>
{
    public override Task CreatePromotions(PostWeatherForecastRequest instance, CancellationToken cancellationToken)
    {
        CreatePromotion("Summary", instance.WeatherForecast?.Summary);
        CreatePromotion("TemperatureC", instance.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
