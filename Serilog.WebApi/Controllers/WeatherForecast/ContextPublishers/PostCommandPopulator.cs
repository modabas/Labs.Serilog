using Serilog.WebApi.Controllers.WeatherForecast.Mediatr;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class PostCommandPopulator : AbstractPropertyPopulator<PostCommand>
{
    public override Task AddProperties(PostCommand instance, CancellationToken cancellationToken)
    {
        AddProperty("Summary", instance.Request.WeatherForecast?.Summary);
        AddProperty("TemperatureC", instance.Request.WeatherForecast?.TemperatureC);
        return Task.CompletedTask;
    }
}
