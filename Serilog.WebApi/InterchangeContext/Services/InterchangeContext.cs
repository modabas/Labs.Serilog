namespace Serilog.WebApi.InterchangeContext.Services;

public class InterchangeContext : IInterchangeContext
{

    private static readonly AsyncLocal<ContextHolder> _contextCurrent = new AsyncLocal<ContextHolder>();

    private InterchangeContextData? Context
    {
        get
        {
            return _contextCurrent.Value?.WrappedObject;
        }
        set
        {
            var holder = _contextCurrent.Value;
            if (holder != null)
            {
                // Clear current DbTransaction trapped in the AsyncLocals, as its done.
                holder.WrappedObject = null;
            }

            if (value != null)
            {
                // Use an object indirection to hold the DbTransaction in the AsyncLocal,
                // so it can be cleared in all ExecutionContexts when its cleared.
                _contextCurrent.Value = new ContextHolder { WrappedObject = value };
            }
        }
    }

    private class ContextHolder
    {
        public InterchangeContextData? WrappedObject;
    }

    private class InterchangeContextData
    {
        public string Id { get; set; } = string.Empty;
        public string OpType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, ContextProperty> PropertyBag = new();
    }

    public string Id
    {
        get
        {
            CheckContextCreated();
            //Null checked in CheckMessageContextCreated
            return Context!.Id;
        }
    }

    public string OpType
    {
        get
        {
            CheckContextCreated();
            //Null checked in CheckMessageContextCreated
            return Context!.OpType;
        }
    }

    public DateTime CreatedAt
    {
        get
        {
            CheckContextCreated();
            //Null checked in CheckMessageContextCreated
            return Context!.CreatedAt;
        }
    }

    public bool IsCreated => Context is not null;

    public Task SetProperty(ContextProperty property, CancellationToken cancellationToken)
    {
        CheckContextCreated();
        //Null checked in CheckMessageContextCreated
        Context!.PropertyBag[property.Name] = property;
        return Task.CompletedTask;
    }

    public Task Create(string id, string opType, CancellationToken cancellationToken)
    {
        CheckContextIsNotCreated();
        Context = new InterchangeContextData() { Id = id, OpType = opType };
        return Task.CompletedTask;
    }

    public Task<ContextProperty?> GetPropertyWithName(string propertyName, CancellationToken cancellationToken)
    {
        CheckContextCreated();
        //Null checked in CheckMessageContextCreated
        var ret = Context!.PropertyBag.GetValueOrDefault(propertyName);
        return Task.FromResult(ret);
    }

    public Task<IEnumerable<ContextProperty>> GetPropertiesForContentLog(CancellationToken cancellationToken)
    {
        CheckContextCreated();
        //Null checked in CheckMessageContextCreated
        var ret = Context!.PropertyBag.Where(p => p.Value.WriteToContentLog == true).Select(p => p.Value).ToList().AsReadOnly();
        return Task.FromResult<IEnumerable<ContextProperty>>(ret);
    }

    private void CheckContextCreated()
    {
        if (!IsCreated)
            throw new ApplicationException("MessageContext is not created.");
    }

    private void CheckContextIsNotCreated()
    {
        if (IsCreated)
            throw new ApplicationException("MessageContext has already been created.");
    }

    public async Task PublishProperty(string name, object? value, CancellationToken cancellationToken)
    {
        await SetProperty(new ContextProperty() { Name = name, Value = value, WriteToContentLog = true }, cancellationToken);
    }
}
