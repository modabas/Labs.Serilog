using Serilog.WebApi.DataExchangeLogger.Dto;
using System.Text.Json.Serialization;

namespace Serilog.WebApi.Web.DataExchangeLogger.Dto;

public class WebApiControllerChannelInfoForResponse : ChannelInfo
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Status { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StatusCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Headers { get; set; }

    public WebApiControllerChannelInfoForResponse() : base()
    {
        Name = "WebApiController";
        Step = "Response";
    }
}
