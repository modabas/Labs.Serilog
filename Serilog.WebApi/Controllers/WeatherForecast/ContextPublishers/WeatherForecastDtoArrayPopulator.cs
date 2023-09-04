using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Controllers.WeatherForecast.LogClasses;
using Serilog.WebApi.InterchangeContext.Services;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class WeatherForecastDtoArrayPopulator : AbstractPropertyPopulator<WeatherForecastDto[]>
{
    public override Task SetProperties(WeatherForecastDto[] instance, CancellationToken cancellationToken)
    {
        var propertyBag = new List<SampleMessageItem>();
        for (var i = 0; i < 10; i++)
        {
            propertyBag.Add(new SampleMessageItem() { Id = i, Name = $"Data{i:00}" });
        }

        SetProperty("ManuallyPopulatedProperties", propertyBag);
        return Task.CompletedTask;
    }
}
