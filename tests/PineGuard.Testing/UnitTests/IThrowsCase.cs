using PineGuard.Testing;

namespace PineGuard.Testing.UnitTests;

public interface IThrowsCase
{
    ExpectedException ExpectedException { get; }
}
