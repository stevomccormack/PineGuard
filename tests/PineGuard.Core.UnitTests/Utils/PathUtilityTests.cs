using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class FilePathUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(FilePathUtilityTestData.ContainsInvalidFileNameChars.ValidCases), MemberType = typeof(FilePathUtilityTestData.ContainsInvalidFileNameChars))]
    [MemberData(nameof(FilePathUtilityTestData.ContainsInvalidFileNameChars.EdgeCases), MemberType = typeof(FilePathUtilityTestData.ContainsInvalidFileNameChars))]
    public void ContainsInvalidFileNameChars_ReturnsExpected(FilePathUtilityTestData.ContainsInvalidFileNameChars.ValidCase testCase)
    {
        // Act
        var result = FilePathUtility.ContainsInvalidFileNameChars(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(FilePathUtilityTestData.IsWindowsReservedDeviceName.ValidCases), MemberType = typeof(FilePathUtilityTestData.IsWindowsReservedDeviceName))]
    [MemberData(nameof(FilePathUtilityTestData.IsWindowsReservedDeviceName.EdgeCases), MemberType = typeof(FilePathUtilityTestData.IsWindowsReservedDeviceName))]
    public void IsWindowsReservedDeviceName_ReturnsExpected(FilePathUtilityTestData.IsWindowsReservedDeviceName.ValidCase testCase)
    {
        // Act
        var result = FilePathUtility.IsWindowsReservedDeviceName(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
