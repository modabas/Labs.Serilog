namespace Serilog.WrapAndTransform;

public interface ILogWrapper<T>
{
    public T? Log { get; set; }

}
