# Log Content Sanitization
Provides log content sanitization, context enrichment and in-process logging by utilizing serilog destructuring policies and mapper functions.

## Serilog.WrapAndTransform project
Implements serilog destructuring policy and log wrapper abstactions for log sanitization.

## Data Exchange Logger
Exposes various log operations, content logging with sanitization, exception logging, channel info logging. All types of logs can be correlated with intechange context id.

## Interchange Context
An optional dependency for data exchange logger. Along with its property populators, provides an easy way to enrich data exhange log context. Also provides a self disposing scoped service store.

Interchange context for the scope has to be initiated as early as possible. This project implements this initialization in two different places (api filter level and mediatr pipeline level) but implementation ensures that an initiated context is not overriden. This should be the case for any other implementations too.

## Channel Info Logs
Extra information not tied to content for data exchange channels can be logged with this mechanism. This project contains a sample for Web Api channel, providing action filter to log channel info during request and response.

```
builder.Services.AddControllers(options =>
{
    //Interchange context initiation and web api channel info logger
    options.Filters.Add<InterchangeContextFactoryFilter>();
    options.Filters.Add<WebApiControllerChannelInfoFilter>();
});
```

## Mediatr
Mediatr pipeline behaviors is the backbone for content logging and interchange context property enrichment. Mediatr request type has to be marked with IDataExchangeRequest marker interface to trigger data content sanitization and logging for both mediatr request and response.

```
builder.Services.AddMediatR(c =>
{
    //order is important!!
    c.AddInterchangeContextFactoryBehavior();
    c.AddInterchangeContextPopulateRequestPropertiesBehavior();
    c.AddDataExchangeLoggerBehavior();
    c.AddInterchangeContextPopulateResponsePropertiesBehavior();

    //additional code...

});
```

## Serilog configuration
Most of the configuration is read from config file when using provided extension method. Sanitization mapper functions has to be defined during dependency injection configuration to take effect:

```
builder.AddSerilogLoggers((ctx, lc) =>
{
    lc
    .Destructure.ByTransformingWrapped<DataExchangeLogWrapper<WeatherForecastDto[]>, WeatherForecastDto[]>(x => x.ToDataExchangeLog())
    .Destructure.ByTransformingWrapped<DataExchangeLogWrapper<PostCommand>, PostCommand>(x => x.ToDataExchangeLog());
});
```

If a "ByTransformingWrapped" mapper function configuration is not provided for a mediatr request marked with IDataExchangeRequest, data content of both mediatr request and response is still logged but as-is, without sanitization.
