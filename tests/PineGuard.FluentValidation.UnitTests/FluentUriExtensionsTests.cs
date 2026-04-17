using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentUriExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class AbsoluteUriValidator : AbstractValidator<Model>
    {
        public AbsoluteUriValidator() => RuleFor(x => x.Value).AbsoluteUri();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.AbsoluteUri.Cases), MemberType = typeof(FluentUriExtensionsTestData.AbsoluteUri))]
    public void AbsoluteUri_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new AbsoluteUriValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class RelativeUriValidator : AbstractValidator<Model>
    {
        public RelativeUriValidator() => RuleFor(x => x.Value).RelativeUri();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.RelativeUri.Cases), MemberType = typeof(FluentUriExtensionsTestData.RelativeUri))]
    public void RelativeUri_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new RelativeUriValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class WebUrlValidator : AbstractValidator<Model>
    {
        public WebUrlValidator() => RuleFor(x => x.Value).WebUrl();
    }

    [Theory]
    [SuppressMessage("xUnit", "xUnit1026:Theory methods should use all of their parameters", Justification = "Parameter is consumed by shared assertion helper.")]
    [MemberData(nameof(FluentUriExtensionsTestData.WebUrl.Cases), MemberType = typeof(FluentUriExtensionsTestData.WebUrl))]
    public void WebUrl_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new WebUrlValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class UrlValidator : AbstractValidator<Model>
    {
        public UrlValidator() => RuleFor(x => x.Value).Url();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.Url.Cases), MemberType = typeof(FluentUriExtensionsTestData.Url))]
    public void Url_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new UrlValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpsUrlValidator : AbstractValidator<Model>
    {
        public HttpsUrlValidator() => RuleFor(x => x.Value).HttpsUrl();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.HttpsUrl.Cases), MemberType = typeof(FluentUriExtensionsTestData.HttpsUrl))]
    public void HttpsUrl_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HttpsUrlValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpUrlValidator : AbstractValidator<Model>
    {
        public HttpUrlValidator() => RuleFor(x => x.Value).HttpUrl();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.HttpUrl.Cases), MemberType = typeof(FluentUriExtensionsTestData.HttpUrl))]
    public void HttpUrl_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HttpUrlValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class FileUriValidator : AbstractValidator<Model>
    {
        public FileUriValidator() => RuleFor(x => x.Value).FileUri();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.FileUri.Cases), MemberType = typeof(FluentUriExtensionsTestData.FileUri))]
    public void FileUri_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new FileUriValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class FilePathValidator : AbstractValidator<Model>
    {
        public FilePathValidator() => RuleFor(x => x.Value).FilePath();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.FilePath.Cases), MemberType = typeof(FluentUriExtensionsTestData.FilePath))]
    public void FilePath_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new FilePathValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotFilePathValidator : AbstractValidator<Model>
    {
        public NotFilePathValidator() => RuleFor(x => x.Value).NotFilePath();
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.NotFilePath.Cases), MemberType = typeof(FluentUriExtensionsTestData.NotFilePath))]
    public void NotFilePath_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotFilePathValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HasSchemeValidator : AbstractValidator<Model>
    {
        public HasSchemeValidator(string scheme) => RuleFor(x => x.Value).HasScheme(scheme);
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.HasScheme.Cases), MemberType = typeof(FluentUriExtensionsTestData.HasScheme))]
    public void HasScheme_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HasSchemeValidator(FluentUriExtensionsTestData.HasScheme.Scheme).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHasSchemeValidator : AbstractValidator<Model>
    {
        public NotHasSchemeValidator(string scheme) => RuleFor(x => x.Value).NotHasScheme(scheme);
    }

    [Theory]
    [MemberData(nameof(FluentUriExtensionsTestData.NotHasScheme.Cases), MemberType = typeof(FluentUriExtensionsTestData.NotHasScheme))]
    public void NotHasScheme_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotHasSchemeValidator(FluentUriExtensionsTestData.NotHasScheme.Scheme).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
