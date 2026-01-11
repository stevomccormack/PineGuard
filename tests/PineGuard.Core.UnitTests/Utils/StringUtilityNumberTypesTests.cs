using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityNumberTypesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt32.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt32))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt32.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt32))]
    public void TryParseInt32_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseInt32.Case testCase)
    {
        var ok = StringUtility.NumberTypes.TryParseInt32(testCase.Value, out var result);

        Assert.Equal(testCase.ExpectedSuccess, ok);
        Assert.Equal(testCase.ExpectedInt32, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt64.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt64))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt64.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt64))]
    public void TryParseInt64_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseInt64.Case testCase)
    {
        var ok = StringUtility.NumberTypes.TryParseInt64(testCase.Value, out var result);

        Assert.Equal(testCase.ExpectedSuccess, ok);
        Assert.Equal(testCase.ExpectedInt64, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimal.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimal))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimal.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimal))]
    public void TryParseDecimal_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseDecimal.Case testCase)
    {
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        var ok = StringUtility.NumberTypes.TryParseDecimal(testCase.Value, out var result, provider: provider);

        Assert.Equal(testCase.ExpectedSuccess, ok);
        Assert.Equal(testCase.ExpectedDecimal, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseSingle.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseSingle))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseSingle.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseSingle))]
    public void TryParseSingle_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseSingle.Case testCase)
    {
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        var ok = StringUtility.NumberTypes.TryParseSingle(testCase.Value, out var result, provider: provider);

        Assert.Equal(testCase.ExpectedSuccess, ok);
        Assert.Equal(testCase.ExpectedSingle, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDouble.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDouble))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDouble.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDouble))]
    public void TryParseDouble_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseDouble.Case testCase)
    {
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        var ok = StringUtility.NumberTypes.TryParseDouble(testCase.Value, out var result, provider: provider);

        Assert.Equal(testCase.ExpectedSuccess, ok);
        Assert.Equal(testCase.ExpectedDouble, result);
    }
}
