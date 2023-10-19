using Serilog.Formatting.Json;

namespace Serilog.WrapAndTransform.Formatters;

public class JsonValueFormatterWithNoTypeTag : JsonValueFormatter
{
    public JsonValueFormatterWithNoTypeTag() : base(null)
    {

    }
}
