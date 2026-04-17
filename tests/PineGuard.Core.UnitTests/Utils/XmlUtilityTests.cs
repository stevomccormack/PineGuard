using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class XmlUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(XmlUtilityTestData.TryParse.ValidCases), MemberType = typeof(XmlUtilityTestData.TryParse))]
    [MemberData(nameof(XmlUtilityTestData.TryParse.EdgeCases), MemberType = typeof(XmlUtilityTestData.TryParse))]
    public void TryParse_ReturnsExpected(XmlUtilityTestData.TryParse.ValidCase testCase)
    {
        // Act
        var ok = XmlUtility.TryParse(testCase.Value, out var doc);

        // Assert
        Assert.Equal(testCase.Expected.ok, ok);
        Assert.Equal(testCase.Expected.hasDocument, doc is not null);
    }
}
