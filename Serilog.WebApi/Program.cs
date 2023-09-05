using Serilog.WrapAndTransform;
using Serilog.WebApi.Controllers.WeatherForecast.Dto;
using Serilog.WebApi.Controllers.WeatherForecast.ContextPublishers;
using Serilog.WebApi.Controllers.WeatherForecast.Mediatr;
using Serilog.WebApi.Controllers.WeatherForecast.LogClasses;
using Serilog.WebApi.InterchangeContext.Extensions;
using Serilog.WebApi.Controllers.WeatherForecast.Extensions;
using MediatR;
using Serilog.WebApi.Serilog.DataExchangeLogger.Extensions;
using Serilog.WebApi.Serilog.DataExchangeLogger.Loggers;
using Serilog.WebApi.Serilog.DataExchangeLogger.Wrappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLoggers((ctx, lc) =>
{
    lc
    .Destructure.ByWrappingAndTransforming<DataExchangeLogWrapper<WeatherForecastDto[]>, WeatherForecastDto[]>(x => x.ToDataExchangeLog())
    .Destructure.ByWrappingAndTransforming<DataExchangeLogWrapper<PostCommand>, PostCommand>(x => x.ToDataExchangeLog())
    .Destructure.ByWrappingAndTransforming<DataExchangeLogWrapper<SampleMessage>, SampleMessage>(x => x.ToDataExchangeLog());
});
builder.Services.AddTransient(typeof(IDataExchangeLogger<>), typeof(DataExchangeLogger<>));
builder.Services.AddInterchangeContext();
builder.Services.AddPropertyPopulator<PostCommandPopulator>();
builder.Services.AddPropertyPopulator<WeatherForecastDtoArrayPopulator>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddMediatR(c =>
{
    //order is important!!
    c.AddInterchangeContextFactoryBehavior();
    c.AddInterchangeContextPopulateRequestPropertiesBehavior();
    c.AddDataExchangeLoggerBehavior();
    c.AddInterchangeContextPopulateResponsePropertiesBehavior();

    //AddMediatR method validates that at least one assembly has been passed on which it will apply a scan looking for e.g. handlers.
    //Since we must pass an assembly, but at the same time we want to avoid that all handlers in your assembly get found, we can pass any other assembly that doesn't contain any handlers.
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
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
