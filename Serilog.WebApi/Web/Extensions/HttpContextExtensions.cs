using Microsoft.Extensions.Primitives;

namespace Serilog.WebApi.Web.Extensions;

public static class HttpContextExtensions
{

    public static string GetRequestIp(this HttpContext httpContext, bool tryUseXForwardHeader = true)
    {
        string? ip = null;

        // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

        // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
        // for 99% of cases however it has been suggested that a better (although tedious)
        // approach might be to read each IP from right to left and use the first public IP.
        // http://stackoverflow.com/a/43554000/538763
        //
        if (tryUseXForwardHeader)
            ip = httpContext.GetHeaderValueAs<string>("X-Forwarded-For")?.SplitCsv().FirstOrDefault();

        // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
        if (string.IsNullOrWhiteSpace(ip) && httpContext.Connection?.RemoteIpAddress != null)
            ip = httpContext.Connection.RemoteIpAddress.ToString();

        if (string.IsNullOrWhiteSpace(ip))
            ip = httpContext.GetHeaderValueAs<string>("REMOTE_ADDR");

        if (string.IsNullOrWhiteSpace(ip))
            return string.Empty;

        return ip;
    }

    public static T? GetHeaderValueAs<T>(this HttpContext httpContext, string headerName)
    {
        StringValues values;

        if (httpContext.Request.Headers.TryGetValue(headerName, out values) == true)
        {
            var rawValues = values.ToString();   // writes out as Csv when there are multiple.

            if (!string.IsNullOrWhiteSpace(rawValues))
                return (T)Convert.ChangeType(values.ToString(), typeof(T));
        }
        return default;
    }

    private static IEnumerable<string> SplitCsv(this string csvList)
    {
        if (string.IsNullOrWhiteSpace(csvList))
            return Array.Empty<string>();

        return csvList
            .TrimEnd(',')
            .Split(',')
            .Select(s => s.Trim())
            .ToList().AsReadOnly();
    }
}
