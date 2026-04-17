using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class HttpUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(HttpUtilityTestData.TryGetHeaderValues.ValidCases), MemberType = typeof(HttpUtilityTestData.TryGetHeaderValues))]
    [MemberData(nameof(HttpUtilityTestData.TryGetHeaderValues.EdgeCases), MemberType = typeof(HttpUtilityTestData.TryGetHeaderValues))]
    public void TryGetHeaderValues_ReturnsExpected(HttpUtilityTestData.TryGetHeaderValues.ValidCase testCase)
    {
        // Act
        var ok = HttpUtility.TryGetHeaderValues(testCase.Value.Headers, testCase.Value.Name, out var values);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (!ok)
        {
            Assert.Null(values);
            return;
        }

        Assert.NotNull(values);
    }

    [Theory]
    [MemberData(nameof(HttpUtilityTestData.TryGetSingleHeaderValue.ValidCases), MemberType = typeof(HttpUtilityTestData.TryGetSingleHeaderValue))]
    [MemberData(nameof(HttpUtilityTestData.TryGetSingleHeaderValue.EdgeCases), MemberType = typeof(HttpUtilityTestData.TryGetSingleHeaderValue))]
    public void TryGetSingleHeaderValue_ReturnsExpected(HttpUtilityTestData.TryGetSingleHeaderValue.ValidCase testCase)
    {
        // Act
        var ok = HttpUtility.TryGetSingleHeaderValue(testCase.Value.Headers, testCase.Value.Name, out var value);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, value);
    }
}
