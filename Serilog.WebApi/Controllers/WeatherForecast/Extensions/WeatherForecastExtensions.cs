using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Controllers.WeatherForecast.Mediatr;

namespace Serilog.WebApi.Controllers.WeatherForecast.Extensions;

public static class WeatherForecastExtensions
{
    public static WeatherForecastDto ToDataExchangeLog(this WeatherForecastDto weatherForecast)
    {
        return new WeatherForecastDto
        {
            Date = weatherForecast.Date,
            TemperatureC = weatherForecast.TemperatureC,
            Summary = "**" + weatherForecast.Summary
        };
    }

    public static WeatherForecastDto[] ToDataExchangeLog(this WeatherForecastDto[] weatherForecasts)
    {
        return weatherForecasts.Select(w => w.ToDataExchangeLog()).ToArray();
    }

    public static PostWeatherForecastRequest ToDataExchangeLog(this PostWeatherForecastRequest request)
    {
        return new PostWeatherForecastRequest
        {
            WeatherForecast = request.WeatherForecast?.ToDataExchangeLog()
        };
    }

    public static PostCommand ToDataExchangeLog(this PostCommand command)
    {
        return command with { Request = command.Request.ToDataExchangeLog() };
    }
}
