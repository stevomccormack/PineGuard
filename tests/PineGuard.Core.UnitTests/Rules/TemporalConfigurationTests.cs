using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TemporalConfigurationTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.InvalidInclusion.Cases), MemberType = typeof(TemporalConfigurationTestData.InvalidInclusion))]
    public void InvalidInclusion_ThrowsBeforeInputValidation(TemporalConfigurationTestData.InvalidInclusion.Case tc)
    {
        // Act
        var exception = Assert.Throws(tc.ExpectedException.Type, tc.Value);

        // Assert
        ThrowsCaseAssert.Expected(exception, tc);
    }
    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.InvalidPrecision.Cases), MemberType = typeof(TemporalConfigurationTestData.InvalidPrecision))]
    public void InvalidPrecision_RejectsEvenEqualOrAbsentPeers(RuleCase<Func<bool>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }
}
