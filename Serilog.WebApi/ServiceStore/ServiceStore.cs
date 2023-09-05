namespace Serilog.WebApi.ServiceStore;

public class ServiceStore : IServiceStore, IDisposable, IAsyncDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IServiceProvider? _requestServices;
    private IServiceScope? _scope;
    private bool _requestServicesSet;

    public ServiceStore(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IServiceProvider ServiceProvider
    {
        get
        {
            if (!_requestServicesSet && _scopeFactory != null)
            {
                _scope = _scopeFactory.CreateScope();
                _requestServices = _scope.ServiceProvider;
                _requestServicesSet = true;
            }
            return _requestServices!;
        }

        set
        {
            _requestServices = value;
            _requestServicesSet = true;
        }
    }

    public ValueTask DisposeAsync()
    {
        switch (_scope)
        {
            case IAsyncDisposable asyncDisposable:
                var vt = asyncDisposable.DisposeAsync();
                if (!vt.IsCompletedSuccessfully)
                {
                    return Awaited(this, vt);
                }
                // If its a IValueTaskSource backed ValueTask,
                // inform it its result has been read so it can reset
                vt.GetAwaiter().GetResult();
                break;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }

        _scope = null;
        _requestServices = null;

        return default;

        static async ValueTask Awaited(ServiceStore servicesFeature, ValueTask vt)
        {
            await vt;
            servicesFeature._scope = null;
            servicesFeature._requestServices = null;
        }
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
