namespace PineGuard.Testing.Common;

public interface IReturnsOutCase<out TExpected, out TOut> : IReturnsCase<TExpected>
{
    TOut? ExpectedOutValue { get; }
}
