using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardUriClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardUriClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.RelativeUri.ValidCases), MemberType = typeof(TD.RelativeUri))]
    [MemberData(nameof(TD.RelativeUri.InvalidCases), MemberType = typeof(TD.RelativeUri))]
    public void RelativeUri_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.RelativeUri(value!));
        AssertCustomMessage(tc, () => Guard.Against.RelativeUri(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.AbsoluteUri.ValidCases), MemberType = typeof(TD.AbsoluteUri))]
    [MemberData(nameof(TD.AbsoluteUri.InvalidCases), MemberType = typeof(TD.AbsoluteUri))]
    public void AbsoluteUri_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.AbsoluteUri(value!));
        AssertCustomMessage(tc, () => Guard.Against.AbsoluteUri(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotUrl.ValidCases), MemberType = typeof(TD.NotUrl))]
    [MemberData(nameof(TD.NotUrl.InvalidCases), MemberType = typeof(TD.NotUrl))]
    public void NotUrl_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotUrl(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotUrl(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpsUrl.ValidCases), MemberType = typeof(TD.NotHttpsUrl))]
    [MemberData(nameof(TD.NotHttpsUrl.InvalidCases), MemberType = typeof(TD.NotHttpsUrl))]
    public void NotHttpsUrl_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpsUrl(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpsUrl(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpUrl.ValidCases), MemberType = typeof(TD.NotHttpUrl))]
    [MemberData(nameof(TD.NotHttpUrl.InvalidCases), MemberType = typeof(TD.NotHttpUrl))]
    public void NotHttpUrl_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpUrl(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpUrl(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotFileUri.ValidCases), MemberType = typeof(TD.NotFileUri))]
    [MemberData(nameof(TD.NotFileUri.InvalidCases), MemberType = typeof(TD.NotFileUri))]
    public void NotFileUri_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotFileUri(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotFileUri(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotFilePath.ValidCases), MemberType = typeof(TD.NotFilePath))]
    [MemberData(nameof(TD.NotFilePath.InvalidCases), MemberType = typeof(TD.NotFilePath))]
    public void NotFilePath_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotFilePath(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotFilePath(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.FilePath.ValidCases), MemberType = typeof(TD.FilePath))]
    [MemberData(nameof(TD.FilePath.InvalidCases), MemberType = typeof(TD.FilePath))]
    public void FilePath_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.FilePath(value!));
        AssertCustomMessage(tc, () => Guard.Against.FilePath(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasScheme.ValidCases), MemberType = typeof(TD.NotHasScheme))]
    [MemberData(nameof(TD.NotHasScheme.InvalidCases), MemberType = typeof(TD.NotHasScheme))]
    public void NotHasScheme_BehavesAsExpected(GuardCase<(string? value, string scheme)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotHasScheme(value!, tc.Value.scheme));
        AssertCustomMessage(tc, () => Guard.Against.NotHasScheme(value!, tc.Value.scheme, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    [Theory]
    [MemberData(nameof(TD.HasScheme.ValidCases), MemberType = typeof(TD.HasScheme))]
    [MemberData(nameof(TD.HasScheme.InvalidCases), MemberType = typeof(TD.HasScheme))]
    public void HasScheme_BehavesAsExpected(GuardCase<(string? value, string scheme)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.HasScheme(value!, tc.Value.scheme));
        AssertCustomMessage(tc, () => Guard.Against.HasScheme(value!, tc.Value.scheme, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }
}
