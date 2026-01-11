using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesDateTimeOffsetTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInPast.ValidCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInPast))]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInPast.EdgeCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInPast))]
    public void IsInPast_ReturnsExpected(StringRulesDateTimeOffsetTestData.IsInPast.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInPast(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInFuture.ValidCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInFuture))]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInFuture.EdgeCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInFuture))]
    public void IsInFuture_ReturnsExpected(StringRulesDateTimeOffsetTestData.IsInFuture.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInFuture(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsBetween.ValidCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsBetween))]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsBetween.EdgeCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsBetween))]
    public void IsBetween_RespectsInclusion(StringRulesDateTimeOffsetTestData.IsBetween.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsBetween(
            testCase.Value.Text,
            testCase.Value.Min,
            testCase.Value.Max,
            testCase.Value.Inclusion);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
