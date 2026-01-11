namespace PineGuard.Testing.Common;

public interface IReturnsCase<out TResult>
{
    TResult ExpectedReturn { get; }
}
