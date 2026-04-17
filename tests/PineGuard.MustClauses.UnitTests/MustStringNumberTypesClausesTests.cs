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
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32InRange.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32InRange))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32InRange.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32InRange))]
    public void Int32InRange_BehavesAsExpected(MustCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.Int32InRange(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32InRange.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32InRange))]
    public void Int32InRange_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int32InRange.NullCase tc)
    {
        var result = Must.Be.Int32InRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32InRange.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32InRange))]
    public void Int32InRange_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int32InRange.RangeCase tc)
    {
        var result = Must.Be.Int32InRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32OutOfRange.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32OutOfRange))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32OutOfRange.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32OutOfRange))]
    public void Int32OutOfRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int32OutOfRange.ValidCase tc)
    {
        var result = Must.Be.Int32OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32OutOfRange.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32OutOfRange))]
    public void Int32OutOfRange_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int32OutOfRange.NullCase tc)
    {
        var result = Must.Be.Int32OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int32OutOfRange.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int32OutOfRange))]
    public void Int32OutOfRange_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int32OutOfRange.RangeCase tc)
    {
        var result = Must.Be.Int32OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64InRange.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64InRange))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64InRange.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64InRange))]
    public void Int64InRange_BehavesAsExpected(MustCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var result = Must.Be.Int64InRange(tc.Value.text, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64InRange.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64InRange))]
    public void Int64InRange_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int64InRange.NullCase tc)
    {
        var result = Must.Be.Int64InRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64InRange.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64InRange))]
    public void Int64InRange_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int64InRange.RangeCase tc)
    {
        var result = Must.Be.Int64InRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64OutOfRange.ValidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64OutOfRange))]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64OutOfRange.InvalidCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64OutOfRange))]
    public void Int64OutOfRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int64OutOfRange.ValidCase tc)
    {
        var result = Must.Be.Int64OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64OutOfRange.NullCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64OutOfRange))]
    public void Int64OutOfRange_Null_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int64OutOfRange.NullCase tc)
    {
        var result = Must.Be.Int64OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumberTypesClausesTestData.Int64OutOfRange.RangeCases), MemberType = typeof(MustStringNumberTypesClausesTestData.Int64OutOfRange))]
    public void Int64OutOfRange_InvalidRange_BehavesAsExpected(MustStringNumberTypesClausesTestData.Int64OutOfRange.RangeCase tc)
    {
        var result = Must.Be.Int64OutOfRange(tc.Value.text, tc.Value.min, tc.Value.max, paramName: "value");
        Assert.Equal(tc.Expected, result.Success);
        Assert.Equal(tc.ParamName, result.ParamName);
    }
}
