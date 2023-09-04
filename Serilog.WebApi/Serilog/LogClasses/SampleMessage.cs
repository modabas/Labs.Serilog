using Serilog.WrapAndTransform;

namespace Serilog.WebApi.Serilog.LogClasses;
public class SampleMessage
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IList<SampleMessageItem> Items { get; set; } = new List<SampleMessageItem>();
}

public class SampleMessageItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static class SampleMessageExtensions
{
    public static SampleMessage ToAuditLog(this SampleMessage content)
    {
        return new SampleMessage
        {
            Name = LogHelper.RedactedValue,
            Description = $"New:{content.Description}",
            Items = content.Items.Select(contentItem => new SampleMessageItem
            {
                Id = contentItem.Id,
                Name = $"**{contentItem.Name}**"
            }).ToList()
        };
    }

    public static SampleMessage ToArchiveLog(this SampleMessage content)
    {
        return new SampleMessage
        {
            Name = $"*{content.Name}*",
            Description = LogHelper.RedactedValue,
            Items = content.Items.Select(contentItem => new SampleMessageItem
            {
                Id = contentItem.Id,
                Name = $"2**{contentItem.Name}**2"
            }).ToList()
        };
    }
}
