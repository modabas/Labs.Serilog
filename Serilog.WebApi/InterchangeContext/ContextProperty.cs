﻿namespace Serilog.WebApi.InterchangeContext;

public class ContextProperty
{
    public string Name { get; set; } = string.Empty;
    public object? Value { get; set; }
    public bool WriteToContentLog { get; set; } = false;
}