using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeZoneRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeZoneRulesTestData.Iana_IsIanaTimeZoneId.ValidCases), MemberType = typeof(TimeZoneRulesTestData.Iana_IsIanaTimeZoneId))]
    [MemberData(nameof(TimeZoneRulesTestData.Iana_IsIanaTimeZoneId.EdgeCases), MemberType = typeof(TimeZoneRulesTestData.Iana_IsIanaTimeZoneId))]
    public void Iana_IsIanaTimeZoneId_ReturnsExpected(TimeZoneRulesTestData.Iana_IsIanaTimeZoneId.Case testCase)
    {
        var result = TimeZoneRules.Iana.IsIanaTimeZoneId(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(TimeZoneRulesTestData.Cldr_IsWindowsTimeZoneId.ValidCases), MemberType = typeof(TimeZoneRulesTestData.Cldr_IsWindowsTimeZoneId))]
    [MemberData(nameof(TimeZoneRulesTestData.Cldr_IsWindowsTimeZoneId.EdgeCases), MemberType = typeof(TimeZoneRulesTestData.Cldr_IsWindowsTimeZoneId))]
    public void Cldr_IsWindowsTimeZoneId_ReturnsExpected(TimeZoneRulesTestData.Cldr_IsWindowsTimeZoneId.Case testCase)
    {
        var result = TimeZoneRules.Cldr.IsWindowsTimeZoneId(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }
}
