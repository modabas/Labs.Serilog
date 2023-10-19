using Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;
using Serilog.WebApi.Web.Controllers.WeatherForecast.Mediatr;

namespace Serilog.WebApi.Web.Controllers.WeatherForecast.ContextPopulators;

public class PostCommandPopulator : AbstractPropertyPopulator<PostCommand>
{
    public override Task SetProperties(PostCommand instance, CancellationToken cancellationToken)
    {
        SetProperty("Summary", instance.Request.WeatherForecast?.Summary);
        SetProperty("TemperatureC", instance.Request.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
