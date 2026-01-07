using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NullRulesTests : BaseUnitTest
{
    [Fact]
    public void IsNull_And_IsNotNull_ReturnExpected()
    {
        object? value = null;
        Assert.True(NullRules.IsNull(value));
        Assert.False(NullRules.IsNotNull(value));

        value = new object();
        Assert.False(NullRules.IsNull(value));
        Assert.True(NullRules.IsNotNull(value));
    }
}
