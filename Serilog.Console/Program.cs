// See https://aka.ms/new-console-template for more information

using Serilog;
using Serilog.Console;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.WrapAndTransform.Destructuring;

var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} "+
    "[{Level:u3}] {Properties:lj} {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Logger(c =>c.Filter.ByIncludingOnly(Matching.WithProperty("IsAuditLog"))
        .WriteTo.Console(formatter: new CompactJsonFormatter(new JsonValueFormatter(null))))
    .WriteTo.Logger(c => c.Filter.ByIncludingOnly(Matching.WithProperty("IsArchiveLog"))
        .WriteTo.File(new CompactJsonFormatter(new JsonValueFormatter(null)), "compact_logs.json"))
    .Destructure.ByTransformingWrapped<AuditLogWrapper<SampleMessage>, SampleMessage>(x => x.ToAuditLog())
    .Destructure.ByTransformingWrapped<ArchiveLogWrapper<SampleMessage>, SampleMessage>(x => x.ToArchiveLog())
    .CreateLogger();


var logContent = new SampleMessage() { Name = "Hello", Description = "Clean this!" };
for (var  i = 0; i < 10; i++)
{
    logContent.Items.Add(new SampleMessageItem() { Id = i, Name = $"Name{i:00}" });
}

var propertyBag = new List<SampleMessageItem>();
for (var i = 0; i < 10; i++)
{
    propertyBag.Add(new SampleMessageItem() { Id = i, Name = $"Data{i:00}" });
}

//using (LogContext.PushProperty("IsAuditLog", true))
//{
//    using (LogContext.PushProperty("Namespace", typeof(LogHelper).FullName))
//    {
//        using (LogContext.PushProperty("PropertyBag", propertyBag, true))
//        {
//            Log.Information("{@LogContent}", new AuditLogWrapper<SampleMessage>(logContent));
//        }
//    }
//}

(new AuditLogger(propertyBag)).LogInformation(logContent);

//using (LogContext.PushProperty("IsArchiveLog", true))
//{
//    using (LogContext.PushProperty("Namespace", typeof(LogHelper).FullName))
//    {
//        using (LogContext.PushProperty("PropertyBag", propertyBag, true))
//        {
//            Log.Information("{@LogContent}", new ArchiveLogWrapper<SampleMessage>(logContent));
//        }
//    }
//}