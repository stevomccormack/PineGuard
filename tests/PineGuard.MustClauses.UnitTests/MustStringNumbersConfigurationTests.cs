using System.Globalization;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringNumbersConfigurationTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringNumbersConfigurationTestData.InvalidStyles), MemberType = typeof(MustStringNumbersConfigurationTestData))]
    public void InvalidStyles_AreAttributedBeforeNullInputAcrossNumericClauses(NumberStyles styles)
    {
        // Act
        IMustResult[] results =
        [
            Must.Be.PositiveDecimal(null, styles), Must.Be.NegativeDecimal(null, styles),
            Must.Be.ZeroDecimal(null, styles), Must.Be.NotZeroDecimal(null, styles),
            Must.Be.ZeroOrPositiveDecimal(null, styles), Must.Be.ZeroOrNegativeDecimal(null, styles),
            Must.Be.GreaterThanDecimal(null, 0, styles), Must.Be.GreaterThanOrEqualDecimal(null, 0, styles),
            Must.Be.LessThanDecimal(null, 0, styles), Must.Be.LessThanOrEqualDecimal(null, 0, styles),
            Must.Be.InRangeDecimal(null, 0, 1, styles: styles), Must.Be.OutOfRangeDecimal(null, 0, 1, styles: styles),
            Must.Be.PercentageDecimal(null, styles), Must.Be.ApproximatelyDecimal(null, 0, 1, styles),
            Must.Be.NotApproximatelyDecimal(null, 0, 1, styles), Must.Be.MultipleOfDecimal(null, 1, styles),
            Must.Be.NotMultipleOfDecimal(null, 1, styles), Must.Be.EvenInt32(null, styles),
            Must.Be.OddInt32(null, styles), Must.Be.FiniteDouble(null, styles),
            Must.Be.NotFiniteDouble(null, styles), Must.Be.NotNaNDouble(null, styles)
        ];

        // Assert
        Assert.All(results, result =>
        {
            Assert.False(result.Success);
            Assert.Equal("styles", result.ParamName);
            Assert.Equal(styles, result.Value);
            Assert.NotEmpty(result.Code);
        });
    }

    [Theory]
    [MemberData(nameof(MustStringNumbersConfigurationTestData.InvalidInclusions), MemberType = typeof(MustStringNumbersConfigurationTestData))]
    public void UndefinedInclusion_IsAttributedBeforeNullInput(Inclusion inclusion)
    {
        // Act
        var inside = Must.Be.InRangeDecimal(null, 0, 1, inclusion);
        var outside = Must.Be.OutOfRangeDecimal(null, 0, 1, inclusion);

        // Assert
        Assert.False(inside.Success);
        Assert.False(outside.Success);
        Assert.Equal("inclusion", inside.ParamName);
        Assert.Equal("inclusion", outside.ParamName);
        Assert.Equal(inclusion, inside.Value);
        Assert.Equal(inclusion, outside.Value);
    }

    [Theory]
    [MemberData(nameof(MustStringNumbersConfigurationTestData.NegativeTolerances), MemberType = typeof(MustStringNumbersConfigurationTestData))]
    public void NegativeTolerance_CannotSucceedByNegatingApproximation(decimal tolerance)
    {
        // Act
        var approximate = Must.Be.ApproximatelyDecimal(null, 0, tolerance);
        var notApproximate = Must.Be.NotApproximatelyDecimal("100", 0, tolerance);

        // Assert
        Assert.False(approximate.Success);
        Assert.False(notApproximate.Success);
        Assert.Equal("tolerance", approximate.ParamName);
        Assert.Equal("tolerance", notApproximate.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringNumbersConfigurationTestData.HexadecimalPairs), MemberType = typeof(MustStringNumbersConfigurationTestData))]
    public void IntegerParity_PreservesHexadecimalParsingAndInt32Result(string evenText, string oddText, int expectedEven, int expectedOdd)
    {
        // Act
        var even = Must.Be.EvenInt32(evenText, NumberStyles.HexNumber);
        var odd = Must.Be.OddInt32(oddText, NumberStyles.HexNumber);

        // Assert
        Assert.True(even.Success);
        Assert.True(odd.Success);
        Assert.Equal(expectedEven, even.Result);
        Assert.Equal(expectedOdd, odd.Result);
        Assert.Equal(evenText, even.Value);
        Assert.Equal(oddText, odd.Value);
    }
}
