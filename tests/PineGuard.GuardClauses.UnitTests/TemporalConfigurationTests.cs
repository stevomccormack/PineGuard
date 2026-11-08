using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class TemporalConfigurationTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.Configuration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.Configuration))]
    public void Configuration_IsAttributedBeforeInput(GuardCase<Action> tc)
    {
        // Act / Assert
        AssertResult(tc, () => { tc.Value(); return true; });
    }
}
