using MediatR;
using Serilog.WebApi.DataExchangeLogger.Mediatr;
using Serilog.WebApi.Web.Controllers.WeatherForecast.Dto;

namespace Serilog.WebApi.Web.Controllers.WeatherForecast.Mediatr;

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
