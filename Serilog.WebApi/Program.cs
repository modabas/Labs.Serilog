using Serilog.WrapAndTransform;
using Serilog.WebApi.Serilog.Extensions;
using Serilog.WebApi.Serilog.LogClasses;
using Serilog.WebApi.Serilog.Wrappers;
using Serilog.WebApi.Serilog.Loggers;
using Serilog.WebApi.InterchangeContext.Services;
using Serilog.WebApi.InterchangeContext.Filters;
using Serilog.WebApi.InterchangeContext;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLoggers((ctx, lc) =>
{
    lc
    .Destructure.ByWrappingAndTransforming<DataExchangeLogWrapper<WeatherForecastDto[]>, WeatherForecastDto[]>(x => x.ToAuditLog())
    .Destructure.ByWrappingAndTransforming<DataExchangeLogWrapper<SampleMessage>, SampleMessage>(x => x.ToAuditLog())
    .Destructure.ByWrappingAndTransforming<ArchiveLogWrapper<SampleMessage>, SampleMessage>(x => x.ToArchiveLog());
});
builder.Services.AddTransient(typeof(IDataExchangeLogger<>), typeof(DataExchangeLogger<>));
builder.Services.AddSingleton<IInterchangeContext, InterchangeContext>();
builder.Services.AddPropertyPopulator<PostWeatherForecastRequestPublisher>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<InterchangeContextFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
