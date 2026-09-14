using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class XmlUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(XmlUtilityTestData.TryGetRootName.ValidCases), MemberType = typeof(XmlUtilityTestData.TryGetRootName))]
    [MemberData(nameof(XmlUtilityTestData.TryGetRootName.EdgeCases), MemberType = typeof(XmlUtilityTestData.TryGetRootName))]
    public void TryGetRootName_ReturnsExpected(XmlUtilityTestData.TryGetRootName.ValidCase testCase)
    {
        // Act
        var ok = XmlUtility.TryGetRootName(testCase.Value, out var rootName);

        // Assert
        Assert.Equal(testCase.Expected.ok, ok);

        if (testCase.Expected.ok)
        {
            Assert.NotNull(rootName);
            Assert.Equal(testCase.Expected.name, rootName!.Name);
            Assert.Equal(testCase.Expected.ns, rootName.Namespace);
        }
        else
        {
            Assert.Null(rootName);
        }
    }
}
