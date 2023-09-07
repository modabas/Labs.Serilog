﻿using System.Diagnostics;

namespace Serilog.WebApi.InterchangeContext.Services;

public class InterchangeContextIdResolver : IInterchangeContextIdResolver
{
    public string ResolveId()
    {
        return Activity.Current?.Id ?? Guid.NewGuid().ToString();
    }
}
