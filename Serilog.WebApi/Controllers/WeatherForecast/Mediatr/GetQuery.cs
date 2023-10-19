using MediatR;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Serilog.DataExchangeLogger.Mediatr;

namespace Serilog.WebApi.Controllers.WeatherForecast.Mediatr;

public record GetQuery : IRequest<WeatherForecastDto[]>, IDataExchangeRequest
{
}

public class GetQueryHandler : IRequestHandler<GetQuery, WeatherForecastDto[]>
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<WeatherForecastDto[]> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        //throw new InvalidOperationException("I want to log an exception");
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        return Task.FromResult(forecast);
    }
}
