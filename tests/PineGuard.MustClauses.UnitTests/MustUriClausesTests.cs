using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustUriClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustUriClausesTestData.AbsoluteUri.ValidCases), MemberType = typeof(MustUriClausesTestData.AbsoluteUri))]
    [MemberData(nameof(MustUriClausesTestData.AbsoluteUri.InvalidCases), MemberType = typeof(MustUriClausesTestData.AbsoluteUri))]
    public void AbsoluteUri_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.AbsoluteUri(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.RelativeUri.ValidCases), MemberType = typeof(MustUriClausesTestData.RelativeUri))]
    [MemberData(nameof(MustUriClausesTestData.RelativeUri.InvalidCases), MemberType = typeof(MustUriClausesTestData.RelativeUri))]
    public void RelativeUri_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.RelativeUri(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.Url.ValidCases), MemberType = typeof(MustUriClausesTestData.Url))]
    [MemberData(nameof(MustUriClausesTestData.Url.InvalidCases), MemberType = typeof(MustUriClausesTestData.Url))]
    public void Url_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Url(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.HttpsUrl.ValidCases), MemberType = typeof(MustUriClausesTestData.HttpsUrl))]
    [MemberData(nameof(MustUriClausesTestData.HttpsUrl.InvalidCases), MemberType = typeof(MustUriClausesTestData.HttpsUrl))]
    public void HttpsUrl_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.HttpsUrl(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.HttpUrl.ValidCases), MemberType = typeof(MustUriClausesTestData.HttpUrl))]
    [MemberData(nameof(MustUriClausesTestData.HttpUrl.InvalidCases), MemberType = typeof(MustUriClausesTestData.HttpUrl))]
    public void HttpUrl_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.HttpUrl(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.FileUri.ValidCases), MemberType = typeof(MustUriClausesTestData.FileUri))]
    [MemberData(nameof(MustUriClausesTestData.FileUri.InvalidCases), MemberType = typeof(MustUriClausesTestData.FileUri))]
    public void FileUri_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.FileUri(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.FilePath.ValidCases), MemberType = typeof(MustUriClausesTestData.FilePath))]
    [MemberData(nameof(MustUriClausesTestData.FilePath.InvalidCases), MemberType = typeof(MustUriClausesTestData.FilePath))]
    public void FilePath_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.FilePath(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.NotFilePath.ValidCases), MemberType = typeof(MustUriClausesTestData.NotFilePath))]
    [MemberData(nameof(MustUriClausesTestData.NotFilePath.InvalidCases), MemberType = typeof(MustUriClausesTestData.NotFilePath))]
    public void NotFilePath_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotFilePath(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.HasScheme.ValidCases), MemberType = typeof(MustUriClausesTestData.HasScheme))]
    [MemberData(nameof(MustUriClausesTestData.HasScheme.InvalidCases), MemberType = typeof(MustUriClausesTestData.HasScheme))]
    public void HasScheme_BehavesAsExpected(MustCase<(string? value, string scheme)> tc)
    {
        var result = Must.Be.HasScheme(tc.Value.value, tc.Value.scheme, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustUriClausesTestData.NotHasScheme.ValidCases), MemberType = typeof(MustUriClausesTestData.NotHasScheme))]
    [MemberData(nameof(MustUriClausesTestData.NotHasScheme.InvalidCases), MemberType = typeof(MustUriClausesTestData.NotHasScheme))]
    public void NotHasScheme_BehavesAsExpected(MustCase<(string? value, string scheme)> tc)
    {
        var result = Must.Be.NotHasScheme(tc.Value.value, tc.Value.scheme, paramName: "value");
        AssertResult(tc, result);
    }
}
