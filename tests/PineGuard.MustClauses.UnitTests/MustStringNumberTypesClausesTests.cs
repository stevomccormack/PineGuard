using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringNumberTypesClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Decimal.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Decimal))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Decimal.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Decimal))]
    public void Decimal_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Decimal(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.DecimalNegativePlaces.Cases), MemberType = typeof(MustStringNumberTypesClausesTestData.DecimalNegativePlaces))]
    public void Decimal_NegativePlaces_BehavesAsExpected(MustStringNumberTypesClausesTestData.DecimalNegativePlaces.Case tc)
    {
        var result = Must.Be.Decimal(tc.Value, decimalPlaces: tc.DecimalPlaces, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ExactDecimal.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.ExactDecimal))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ExactDecimal.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.ExactDecimal))]
    public void ExactDecimal_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.ExactDecimal(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ExactDecimalNegativePlaces.Cases), MemberType = typeof(MustStringNumberTypesClausesTestData.ExactDecimalNegativePlaces))]
    public void ExactDecimal_NegativePlaces_BehavesAsExpected(MustStringNumberTypesClausesTestData.ExactDecimalNegativePlaces.Case tc)
    {
        var result = Must.Be.ExactDecimal(tc.Value, exactDecimalPlaces: tc.ExactDecimalPlaces, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32))]
    public void Int32_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Int32(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64))]
    public void Int64_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Int64(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt32.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt32))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt32.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt32))]
    public void InRangeInt32_BehavesAsExpected(MustCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.InRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt32.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt32))]
    public void InRangeInt32_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.InRangeInt32.NullCase tc)
    {
        var result = Must.Be.InRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt32.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt32))]
    public void InRangeInt32_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.InRangeInt32.RangeCase tc)
    {
        var result = Must.Be.InRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt32.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt32))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt32.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt32))]
    public void OutOfRangeInt32_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt32.ValidCase tc)
    {
        var result = Must.Be.OutOfRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt32.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt32))]
    public void OutOfRangeInt32_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt32.NullCase tc)
    {
        var result = Must.Be.OutOfRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt32.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt32))]
    public void OutOfRangeInt32_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt32.RangeCase tc)
    {
        var result = Must.Be.OutOfRangeInt32(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt64.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt64))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt64.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt64))]
    public void InRangeInt64_BehavesAsExpected(MustCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.InRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt64.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt64))]
    public void InRangeInt64_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.InRangeInt64.NullCase tc)
    {
        var result = Must.Be.InRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.InRangeInt64.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.InRangeInt64))]
    public void InRangeInt64_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.InRangeInt64.RangeCase tc)
    {
        var result = Must.Be.InRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt64.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt64))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt64.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt64))]
    public void OutOfRangeInt64_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt64.ValidCase tc)
    {
        var result = Must.Be.OutOfRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt64.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt64))]
    public void OutOfRangeInt64_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt64.NullCase tc)
    {
        var result = Must.Be.OutOfRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.OutOfRangeInt64.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.OutOfRangeInt64))]
    public void OutOfRangeInt64_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.OutOfRangeInt64.RangeCase tc)
    {
        var result = Must.Be.OutOfRangeInt64(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }
    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence.Cases), MemberType = typeof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence))]
    public void ConfigurationPrecedence_RejectsInvalidConfiguration(MustCase<Func<MustResult<int>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence.Int64Cases), MemberType = typeof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence))]
    public void Int64ConfigurationPrecedence_RejectsInvalidConfiguration(MustCase<Func<MustResult<long>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence.DecimalCases), MemberType = typeof(MustStringNumberTypesClausesTestData.ConfigurationPrecedence))]
    public void DecimalConfigurationPrecedence_RejectsInvalidConfiguration(MustCase<Func<MustResult<decimal>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }
}
