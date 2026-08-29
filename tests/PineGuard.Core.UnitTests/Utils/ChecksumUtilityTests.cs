using PineGuard.Testing.UnitTests;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class ChecksumUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ChecksumUtilityTestData.IsLuhn.ValidCases), MemberType = typeof(ChecksumUtilityTestData.IsLuhn))]
    [MemberData(nameof(ChecksumUtilityTestData.IsLuhn.EdgeCases), MemberType = typeof(ChecksumUtilityTestData.IsLuhn))]
    public void IsLuhn_BehavesAsExpected(ChecksumUtilityTestData.IsLuhn.ValidCase testCase)
    {
        // Act
        var result = ChecksumUtility.IsLuhn(testCase.Value.AsSpan());

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
