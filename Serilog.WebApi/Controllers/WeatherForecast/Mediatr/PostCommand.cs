using MediatR;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;

namespace Serilog.WebApi.Controllers.WeatherForecast.Mediatr;

public record PostCommand(PostWeatherForecastRequest Request) : IRequest
{
}

public class PostCommandHandler : IRequestHandler<PostCommand>
{
    public Task Handle(PostCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
