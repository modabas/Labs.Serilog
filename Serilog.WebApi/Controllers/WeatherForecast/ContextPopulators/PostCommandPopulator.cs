using Serilog.WebApi.Controllers.WeatherForecast.Mediatr;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPopulators;

public class PostCommandPopulator : AbstractPropertyPopulator<PostCommand>
{
    public override Task SetProperties(PostCommand instance, CancellationToken cancellationToken)
    {
        SetProperty("Summary", instance.Request.WeatherForecast?.Summary);
        SetProperty("TemperatureC", instance.Request.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
