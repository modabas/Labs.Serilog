namespace Serilog.WebApi.Controllers.WeatherForecast.Dto;

public static class WeatherForecastExtensions
{
    public static WeatherForecastDto ToAuditLog(this WeatherForecastDto weatherForecast)
    {
        return new WeatherForecastDto
        {
            Date = weatherForecast.Date,
            TemperatureC = weatherForecast.TemperatureC,
            Summary = "**" + weatherForecast.Summary
        };
    }

    public static WeatherForecastDto[] ToAuditLog(this WeatherForecastDto[] weatherForecasts)
    {
        return weatherForecasts.Select(w => w.ToAuditLog()).ToArray();
    }
}
