namespace PineGuard.Testing.Common;

public interface IReturnsCase<out TExpected>
{
    TExpected Expected { get; }
}
