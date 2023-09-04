using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Controllers.WeatherForecast.Mediatr;

namespace Serilog.WebApi.Controllers.WeatherForecast;
[ApiController]
[Route("[controller]")]
public partial class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> Get(CancellationToken cancellationToken)
    {
        var ret = await _mediator.Send(new GetQuery(), cancellationToken);
        return Ok(ret);
    }

    [HttpPost(Name = "PostWeatherForecast")]
    public async Task<IActionResult> Post([FromBody] PostWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new PostCommand(request), cancellationToken);
        return Ok();
    }
}
