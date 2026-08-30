using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardStringGraphemesClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringGraphemesClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotHasExactGraphemeCount.ValidCases), MemberType = typeof(TD.NotHasExactGraphemeCount))]
    [MemberData(nameof(TD.NotHasExactGraphemeCount.InvalidCases), MemberType = typeof(TD.NotHasExactGraphemeCount))]
    public void NotHasExactGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int count)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var count = tc.Value.count;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasExactGraphemeCount(value, count));
        AssertCustomMessage(tc, () => Guard.Against.NotHasExactGraphemeCount(value, count, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasExactGraphemeCount.ValidCases), MemberType = typeof(TD.HasExactGraphemeCount))]
    [MemberData(nameof(TD.HasExactGraphemeCount.InvalidCases), MemberType = typeof(TD.HasExactGraphemeCount))]
    public void HasExactGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int count)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var count = tc.Value.count;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.HasExactGraphemeCount(value, count));
        AssertCustomMessage(tc, () => Guard.Against.HasExactGraphemeCount(value, count, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasMinGraphemeCount.ValidCases), MemberType = typeof(TD.NotHasMinGraphemeCount))]
    [MemberData(nameof(TD.NotHasMinGraphemeCount.InvalidCases), MemberType = typeof(TD.NotHasMinGraphemeCount))]
    public void NotHasMinGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int min)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var min = tc.Value.min;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasMinGraphemeCount(value, min));
        AssertCustomMessage(tc, () => Guard.Against.NotHasMinGraphemeCount(value, min, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasMinGraphemeCount.ValidCases), MemberType = typeof(TD.HasMinGraphemeCount))]
    [MemberData(nameof(TD.HasMinGraphemeCount.InvalidCases), MemberType = typeof(TD.HasMinGraphemeCount))]
    public void HasMinGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int min)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var min = tc.Value.min;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.HasMinGraphemeCount(value, min));
        AssertCustomMessage(tc, () => Guard.Against.HasMinGraphemeCount(value, min, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasMaxGraphemeCount.ValidCases), MemberType = typeof(TD.NotHasMaxGraphemeCount))]
    [MemberData(nameof(TD.NotHasMaxGraphemeCount.InvalidCases), MemberType = typeof(TD.NotHasMaxGraphemeCount))]
    public void NotHasMaxGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int max)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var max = tc.Value.max;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasMaxGraphemeCount(value, max));
        AssertCustomMessage(tc, () => Guard.Against.NotHasMaxGraphemeCount(value, max, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasMaxGraphemeCount.ValidCases), MemberType = typeof(TD.HasMaxGraphemeCount))]
    [MemberData(nameof(TD.HasMaxGraphemeCount.InvalidCases), MemberType = typeof(TD.HasMaxGraphemeCount))]
    public void HasMaxGraphemeCount_BehavesAsExpected(GuardCase<(string? value, int max)> tc)
    {
        // Arrange
        var value = tc.Value.value;
        var max = tc.Value.max;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.HasMaxGraphemeCount(value, max));
        AssertCustomMessage(tc, () => Guard.Against.HasMaxGraphemeCount(value, max, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasGraphemeCountBetween.ValidCases), MemberType = typeof(TD.NotHasGraphemeCountBetween))]
    [MemberData(nameof(TD.NotHasGraphemeCountBetween.InvalidCases), MemberType = typeof(TD.NotHasGraphemeCountBetween))]
    public void NotHasGraphemeCountBetween_BehavesAsExpected(GuardCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotHasGraphemeCountBetween(value, min, max, inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotHasGraphemeCountBetween(value, min, max, inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasGraphemeCountBetween.ValidCases), MemberType = typeof(TD.HasGraphemeCountBetween))]
    [MemberData(nameof(TD.HasGraphemeCountBetween.InvalidCases), MemberType = typeof(TD.HasGraphemeCountBetween))]
    public void HasGraphemeCountBetween_BehavesAsExpected(GuardCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.HasGraphemeCountBetween(value, min, max, inclusion));
        AssertCustomMessage(tc, () => Guard.Against.HasGraphemeCountBetween(value, min, max, inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
