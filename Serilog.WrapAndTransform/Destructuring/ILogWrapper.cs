namespace Serilog.WrapAndTransform.Destructuring;

public interface ILogWrapper<T>
{
    public T? Log { get; set; }

}
