using MediatR;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Serilog.DataExchangeLogger.Mediatr;

namespace Serilog.WebApi.Controllers.WeatherForecast.Mediatr;

public record PostCommand(PostWeatherForecastRequest Request) : IRequest, IDataExchangeRequest
{
}

public class PostCommandHandler : IRequestHandler<PostCommand>
{
    public Task Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
