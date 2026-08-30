using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringGraphemesClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasExactGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasExactGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasExactGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasExactGraphemeCount.NegativeCountCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasExactGraphemeCount))]
    public void HasExactGraphemeCount_BehavesAsExpected(MustCase<(string? value, int count)> tc)
    {
        // Arrange
        var (value, count) = tc.Value;

        // Act
        var result = Must.Be.HasExactGraphemeCount(value, count, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount.NegativeCountCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasExactGraphemeCount))]
    public void NotHasExactGraphemeCount_BehavesAsExpected(MustCase<(string? value, int count)> tc)
    {
        // Arrange
        var (value, count) = tc.Value;

        // Act
        var result = Must.Be.NotHasExactGraphemeCount(value, count, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMinGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMinGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMinGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMinGraphemeCount.NegativeMinCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMinGraphemeCount))]
    public void HasMinGraphemeCount_BehavesAsExpected(MustCase<(string? value, int min)> tc)
    {
        // Arrange
        var (value, min) = tc.Value;

        // Act
        var result = Must.Be.HasMinGraphemeCount(value, min, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount.NegativeMinCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMinGraphemeCount))]
    public void NotHasMinGraphemeCount_BehavesAsExpected(MustCase<(string? value, int min)> tc)
    {
        // Arrange
        var (value, min) = tc.Value;

        // Act
        var result = Must.Be.NotHasMinGraphemeCount(value, min, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount.NegativeMaxCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasMaxGraphemeCount))]
    public void HasMaxGraphemeCount_BehavesAsExpected(MustCase<(string? value, int max)> tc)
    {
        // Arrange
        var (value, max) = tc.Value;

        // Act
        var result = Must.Be.HasMaxGraphemeCount(value, max, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount.NegativeMaxCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasMaxGraphemeCount))]
    public void NotHasMaxGraphemeCount_BehavesAsExpected(MustCase<(string? value, int max)> tc)
    {
        // Arrange
        var (value, max) = tc.Value;

        // Act
        var result = Must.Be.NotHasMaxGraphemeCount(value, max, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.NegativeMinCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.NegativeMaxCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween.InvalidRangeCases), MemberType = typeof(MustStringGraphemesClausesTestData.HasGraphemeCountBetween))]
    public void HasGraphemeCountBetween_BehavesAsExpected(MustCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = Must.Be.HasGraphemeCountBetween(value, min, max, inclusion, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.ValidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.InvalidCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.NullCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.NegativeMinCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.NegativeMaxCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    [MemberData(nameof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween.InvalidRangeCases), MemberType = typeof(MustStringGraphemesClausesTestData.NotHasGraphemeCountBetween))]
    public void NotHasGraphemeCountBetween_BehavesAsExpected(MustCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = Must.Be.NotHasGraphemeCountBetween(value, min, max, inclusion, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
