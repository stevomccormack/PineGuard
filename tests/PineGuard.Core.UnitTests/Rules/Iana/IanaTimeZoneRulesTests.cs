using PineGuard.Rules.Iana;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iana;

public sealed class IanaTimeZoneRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IanaTimeZoneRulesTestData.IsIanaTimeZoneId.ValidCases), MemberType = typeof(IanaTimeZoneRulesTestData.IsIanaTimeZoneId))]
    [MemberData(nameof(IanaTimeZoneRulesTestData.IsIanaTimeZoneId.EdgeCases), MemberType = typeof(IanaTimeZoneRulesTestData.IsIanaTimeZoneId))]
    public void IsIanaTimeZoneId_ReturnsExpected(IanaTimeZoneRulesTestData.IsIanaTimeZoneId.Case testCase)
    {
        var result = IanaTimeZoneRules.IsIanaTimeZoneId(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
