﻿using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace Serilog.WebApi.DataExchangeLogger.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSerilogLoggers(this WebApplicationBuilder builder, Action<HostBuilderContext, LoggerConfiguration> additionalLogConfiguraton)
    {
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(ctx.Configuration);
            //.Enrich.FromLogContext()
            //.Enrich.WithMachineName()
            //.Enrich.WithEnvironmentName()
            //.WriteTo.Logger(c => c.Filter.ByIncludingOnly(Matching.WithProperty("IsDataExchangeLog"))
            //    .WriteTo.Console(formatter: new CompactJsonFormatter(new JsonValueFormatter(null))));
            if (additionalLogConfiguraton is not null)
            {
                additionalLogConfiguraton(ctx, lc);
            }
        });
        return builder;
    }
}
