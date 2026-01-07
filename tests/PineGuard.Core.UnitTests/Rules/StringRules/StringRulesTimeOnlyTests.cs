using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTimeOnlyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsBetween.ValidCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsBetween))]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsBetween.EdgeCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsBetween))]
    public void IsBetween_ReturnsExpected(StringRulesTimeOnlyTestData.IsBetween.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeOnly.IsBetween(
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsChronological.ValidCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsChronological))]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsChronological.EdgeCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsChronological))]
    public void IsChronological_ReturnsExpected(StringRulesTimeOnlyTestData.IsChronological.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeOnly.IsChronological(
            testCase.Start,
            testCase.End,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsOverlapping.ValidCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsOverlapping))]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsOverlapping.EdgeCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected(StringRulesTimeOnlyTestData.IsOverlapping.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeOnly.IsOverlapping(
            testCase.Start1,
            testCase.End1,
            testCase.Start2,
            testCase.End2,
            testCase.Inclusion);

        Assert.Equal(testCase.Expected, result);
    }
}
