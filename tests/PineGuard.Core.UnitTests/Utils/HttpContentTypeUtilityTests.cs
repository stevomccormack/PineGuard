using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class HttpContentTypeUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(HttpContentTypeUtilityTestData.TryGetMediaType.ValidCases), MemberType = typeof(HttpContentTypeUtilityTestData.TryGetMediaType))]
    [MemberData(nameof(HttpContentTypeUtilityTestData.TryGetMediaType.EdgeCases), MemberType = typeof(HttpContentTypeUtilityTestData.TryGetMediaType))]
    public void TryGetMediaType_ReturnsExpected(HttpContentTypeUtilityTestData.TryGetMediaType.ValidCase testCase)
    {
        // Act
        var ok = HttpContentTypeUtility.TryGetMediaType(testCase.Value, out var mediaType);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, mediaType);
    }

    [Theory]
    [MemberData(nameof(HttpContentTypeUtilityTestData.TryGetContentTypeMediaTypes.ValidCases), MemberType = typeof(HttpContentTypeUtilityTestData.TryGetContentTypeMediaTypes))]
    [MemberData(nameof(HttpContentTypeUtilityTestData.TryGetContentTypeMediaTypes.EdgeCases), MemberType = typeof(HttpContentTypeUtilityTestData.TryGetContentTypeMediaTypes))]
    public void TryGetContentTypeMediaTypes_ReturnsExpected(HttpContentTypeUtilityTestData.TryGetContentTypeMediaTypes.ValidCase testCase)
    {
        // Act
        var ok = HttpContentTypeUtility.TryGetContentTypeMediaTypes(testCase.Value, out var mediaTypes);

        // Assert
        Assert.Equal(testCase.Expected, ok);

        if (testCase.ExpectedOutValue is null)
        {
            Assert.Null(mediaTypes);
            return;
        }

        Assert.NotNull(mediaTypes);
        Assert.Equal(testCase.ExpectedOutValue, mediaTypes);
    }
}
