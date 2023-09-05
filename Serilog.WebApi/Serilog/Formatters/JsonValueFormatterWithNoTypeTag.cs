using Serilog.Formatting.Json;

namespace Serilog.WebApi.Serilog.Formatters;

public class JsonValueFormatterWithNoTypeTag : JsonValueFormatter
{
    public JsonValueFormatterWithNoTypeTag() : base(null)
    {

    }
}
