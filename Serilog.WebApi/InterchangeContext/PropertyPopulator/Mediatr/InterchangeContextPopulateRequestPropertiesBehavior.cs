using MediatR;
using Serilog.WebApi.InterchangeContext.Accessor.Services;
using Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;

namespace Serilog.WebApi.InterchangeContext.PropertyPopulator.Mediatr;

public class InterchangeContextPopulateRequestPropertiesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;
    private readonly IInterchangeContextPropertyPopulator<TRequest>? _propertyPopulator;

    public InterchangeContextPopulateRequestPropertiesBehavior(IInterchangeContextAccessor interchangeContextAccessor,
        IInterchangeContextPropertyPopulator<TRequest>? propertyPopulator = null)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
        _propertyPopulator = propertyPopulator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        //if (interchangeContext is not null)
        //{
        //    await PopulateRequestProperties(interchangeContext, request, cancellationToken);
        //}

        var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        if (interchangeContext is not null && _propertyPopulator is not null)
        {
            await _propertyPopulator.SetProperties(request, cancellationToken);
            foreach (var property in await _propertyPopulator.GetProperties(cancellationToken))
            {
                await interchangeContext.SetProperty(property, cancellationToken);
            }
        }

        return await next();
    }

    //private async Task PopulateRequestProperties(IInterchangeContext interchangeContext, TRequest instance, CancellationToken cancellationToken)
    //{
    //    if (instance is null)
    //        return;
    //    var instanceType = instance.GetType();
    //    var populatorType = typeof(IInterchangeContextPropertyPopulator<>).MakeGenericType(instanceType);
    //    if (populatorType is null)
    //    {
    //        return;
    //    }
    //    var populator = interchangeContext.Services.GetService(populatorType);
    //    if (populator is null)
    //    {
    //        return;
    //    }

    //    var createPropertiesTask = (Task?)populatorType
    //        .GetTypeInfo()
    //        .GetMethod("SetProperties")?
    //        .Invoke(populator, new object[] { instance, cancellationToken });
    //    if (createPropertiesTask is null)
    //    {
    //        return;
    //    }
    //    await createPropertiesTask;

    //    var getPropertiesTask = (Task<IEnumerable<ContextProperty>>?)populatorType
    //        .GetTypeInfo()
    //        .GetMethod("GetProperties")?
    //        .Invoke(populator, new object[] { cancellationToken });
    //    if (getPropertiesTask is null)
    //    {
    //        return;
    //    }
    //    foreach (var property in await getPropertiesTask)
    //    {
    //        await interchangeContext.SetProperty(property, cancellationToken);
    //    }
    //}
}