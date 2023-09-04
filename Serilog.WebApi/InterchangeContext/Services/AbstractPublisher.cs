namespace Serilog.WebApi.InterchangeContext.Services;

public abstract class AbstractPublisher<T> : IContextPublisher<T>
{
    private readonly List<ContextProperty> _promotions = new List<ContextProperty>();

    public abstract Task CreatePromotions(T instance, CancellationToken cancellationToken);

    public Task<IEnumerable<ContextProperty>> GetPromotions(T instance, CancellationToken cancellationToken)
    {
        return Task.FromResult(_promotions.AsEnumerable());
    }

    public ContextProperty CreatePromotion(string name, object? value)
    {
        return CreatePromotion(name, value, true);
    }

    public ContextProperty CreatePromotion(string name, object? value, bool writeToContentLog)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "The value of 'name' should not be null or whitespace.");

        var promotion = new ContextProperty() { Name = name, Value = value, WriteToContentLog = writeToContentLog };
        _promotions.Add(promotion);
        return promotion;
    }
}
