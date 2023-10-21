using MediatR;
using Serilog.WebApi.InterchangeContext.Accessor.Services;
using Serilog.WebApi.InterchangeContext.PropertyPopulator.Services;

namespace Serilog.WebApi.InterchangeContext.PropertyPopulator.Mediatr;

public class InterchangeContextPopulateResponsePropertiesBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IInterchangeContextAccessor _interchangeContextAccessor;
    private readonly IInterchangeContextPropertyPopulator<TResponse>? _propertyPopulator;

    public InterchangeContextPopulateResponsePropertiesBehavior(IInterchangeContextAccessor interchangeContextAccessor,
        IInterchangeContextPropertyPopulator<TResponse>? propertyPopulator = null)
    {
        _interchangeContextAccessor = interchangeContextAccessor;
        _propertyPopulator = propertyPopulator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        //var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        //if (interchangeContext is not null)
        //{
        //    await PopulateResponseProperties(interchangeContext, response, cancellationToken);
        //}

        var interchangeContext = _interchangeContextAccessor.InterchangeContext;
        if (interchangeContext is not null && _propertyPopulator is not null)
        {
            await _propertyPopulator.SetProperties(response, cancellationToken);
            foreach (var property in await _propertyPopulator.GetProperties(cancellationToken))
            {
                await interchangeContext.SetProperty(property, cancellationToken);
            }
        }

        return response;
    }

    //private async Task PopulateResponseProperties(IInterchangeContext interchangeContext, TResponse instance, CancellationToken cancellationToken)
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
