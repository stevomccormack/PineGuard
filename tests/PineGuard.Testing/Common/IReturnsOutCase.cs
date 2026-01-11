namespace PineGuard.Testing.Common;

public interface IReturnsOutCase<out TResult, out TOut> : IReturnsCase<TResult>
{
    TOut? ExpectedOutValue { get; }
}
