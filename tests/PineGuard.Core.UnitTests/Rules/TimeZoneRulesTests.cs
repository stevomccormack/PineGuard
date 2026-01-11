using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeZoneRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeZoneRulesTestData.Iana.IsTimeZoneId.ValidCases), MemberType = typeof(TimeZoneRulesTestData.Iana.IsTimeZoneId))]
    [MemberData(nameof(TimeZoneRulesTestData.Iana.IsTimeZoneId.EdgeCases), MemberType = typeof(TimeZoneRulesTestData.Iana.IsTimeZoneId))]
    public void Iana_IsIanaTimeZoneId_ReturnsExpected(TimeZoneRulesTestData.Iana.IsTimeZoneId.Case testCase)
    {
        var result = TimeZoneRules.Iana.IsIanaTimeZoneId(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(TimeZoneRulesTestData.Cldr.IsWindowsTimeZoneId.ValidCases), MemberType = typeof(TimeZoneRulesTestData.Cldr.IsWindowsTimeZoneId))]
    [MemberData(nameof(TimeZoneRulesTestData.Cldr.IsWindowsTimeZoneId.EdgeCases), MemberType = typeof(TimeZoneRulesTestData.Cldr.IsWindowsTimeZoneId))]
    public void Cldr_IsWindowsTimeZoneId_ReturnsExpected(TimeZoneRulesTestData.Cldr.IsWindowsTimeZoneId.Case testCase)
    {
        var result = TimeZoneRules.Cldr.IsWindowsTimeZoneId(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
