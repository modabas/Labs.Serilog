namespace Serilog.WebApi.InterchangeContext.Services;

public class InterchangeContextAccessor : IInterchangeContextAccessor
{

    private static readonly AsyncLocal<ContextHolder> _contextCurrent = new AsyncLocal<ContextHolder>();

    public IInterchangeContext? InterchangeContext
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
                // Clear current context data trapped in the AsyncLocals, as its done.
                holder.WrappedObject = null;
            }

            if (value != null)
            {
                // Use an object indirection to hold the context data in the AsyncLocal,
                // so it can be cleared in all ExecutionContexts when its cleared.
                _contextCurrent.Value = new ContextHolder { WrappedObject = value };
            }
        }
    }

    public bool IsInitialized => InterchangeContext is not null;

    private class ContextHolder
    {
        public IInterchangeContext? WrappedObject;
    }
}
