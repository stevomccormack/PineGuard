using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardFilePathClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardFilePathClausesTestData.NotSafeFileName.ValidCases), MemberType = typeof(GuardFilePathClausesTestData.NotSafeFileName))]
    [MemberData(nameof(GuardFilePathClausesTestData.NotSafeFileName.InvalidCases), MemberType = typeof(GuardFilePathClausesTestData.NotSafeFileName))]
    public void NotSafeFileName_BehavesAsExpected(GuardCase<string?> tc)
    {
        var fileName = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotSafeFileName(fileName!));
        if (tc.Expected.IsValid) Assert.Equal(fileName, result);
    }

    [Theory]
    [MemberData(nameof(GuardFilePathClausesTestData.NotHasFileExtension.ValidCases), MemberType = typeof(GuardFilePathClausesTestData.NotHasFileExtension))]
    [MemberData(nameof(GuardFilePathClausesTestData.NotHasFileExtension.InvalidCases), MemberType = typeof(GuardFilePathClausesTestData.NotHasFileExtension))]
    public void NotHasFileExtension_BehavesAsExpected(GuardCase<(string? path, string[]? allowed)> tc)
    {
        var path = tc.Value.path;
        var result = AssertResult(tc, () => Guard.Against.NotHasFileExtension(path, tc.Value.allowed));
        if (tc.Expected.IsValid) Assert.Equal(path, result);
    }
}
