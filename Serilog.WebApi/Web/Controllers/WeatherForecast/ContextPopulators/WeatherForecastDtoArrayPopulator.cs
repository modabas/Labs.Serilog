﻿using Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;
using Serilog.WebApi.Web.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Web.Controllers.WeatherForecast.LogClasses;

namespace Serilog.WebApi.Web.Controllers.WeatherForecast.ContextPopulators;

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
