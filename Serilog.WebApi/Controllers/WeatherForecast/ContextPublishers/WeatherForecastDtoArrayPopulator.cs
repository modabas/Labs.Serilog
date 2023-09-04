using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.Serilog.LogClasses;

namespace Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

public class WeatherForecastDtoArrayPopulator : AbstractPropertyPopulator<WeatherForecastDto[]>
{
    public override Task AddProperties(WeatherForecastDto[] instance, CancellationToken cancellationToken)
    {
        var propertyBag = new List<SampleMessageItem>();
        for (var i = 0; i < 10; i++)
        {
            propertyBag.Add(new SampleMessageItem() { Id = i, Name = $"Data{i:00}" });
        }

        AddProperty("ManuallyPopulatedProperties", propertyBag);
        return Task.CompletedTask;
    }
}
