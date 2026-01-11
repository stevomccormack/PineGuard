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
            testCase.Value.Text,
            testCase.Value.Min,
            testCase.Value.Max,
            testCase.Value.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsChronological.ValidCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsChronological))]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsChronological.EdgeCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsChronological))]
    public void IsChronological_ReturnsExpected(StringRulesTimeOnlyTestData.IsChronological.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeOnly.IsChronological(
            testCase.Value.Start,
            testCase.Value.End,
            testCase.Value.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsOverlapping.ValidCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsOverlapping))]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsOverlapping.EdgeCases), MemberType = typeof(StringRulesTimeOnlyTestData.IsOverlapping))]
    public void IsOverlapping_ReturnsExpected(StringRulesTimeOnlyTestData.IsOverlapping.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.TimeOnly.IsOverlapping(
            testCase.Value.Start1,
            testCase.Value.End1,
            testCase.Value.Start2,
            testCase.Value.End2,
            testCase.Value.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
