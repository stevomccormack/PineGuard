using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentHttpExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record StringModel { public string? Value { get; init; } }
    private sealed record IntModel { public int? Value { get; init; } }
    private sealed record HeadersModel { public IReadOnlyDictionary<string, IEnumerable<string>>? Value { get; init; } }

    private sealed class HeaderNameValidator : AbstractValidator<StringModel>
    {
        public HeaderNameValidator() => RuleFor(x => x.Value).HeaderName();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HeaderName.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HeaderName))]
    public void HeaderName_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HeaderNameValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHeaderNameValidator : AbstractValidator<StringModel>
    {
        public NotHeaderNameValidator() => RuleFor(x => x.Value).NotHeaderName();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHeaderName.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHeaderName))]
    public void NotHeaderName_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotHeaderNameValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HeaderValueValidator : AbstractValidator<StringModel>
    {
        public HeaderValueValidator() => RuleFor(x => x.Value).HeaderValue();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HeaderValue))]
    public void HeaderValue_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HeaderValueValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHeaderValueValidator : AbstractValidator<StringModel>
    {
        public NotHeaderValueValidator() => RuleFor(x => x.Value).NotHeaderValue();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHeaderValue))]
    public void NotHeaderValue_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotHeaderValueValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusCodeValidator : AbstractValidator<IntModel>
    {
        public HttpStatusCodeValidator() => RuleFor(x => x.Value).HttpStatusCode();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusCode.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusCode))]
    public void HttpStatusCode_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusCodeValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusCodeValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusCodeValidator() => RuleFor(x => x.Value).NotHttpStatusCode();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusCode.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusCode))]
    public void NotHttpStatusCode_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusCodeValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusInformationalValidator : AbstractValidator<IntModel>
    {
        public HttpStatusInformationalValidator() => RuleFor(x => x.Value).HttpStatusInformational();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusInformational.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusInformational))]
    public void HttpStatusInformational_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusInformationalValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusInformationalValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusInformationalValidator() => RuleFor(x => x.Value).NotHttpStatusInformational();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusInformational.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusInformational))]
    public void NotHttpStatusInformational_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusInformationalValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusSuccessValidator : AbstractValidator<IntModel>
    {
        public HttpStatusSuccessValidator() => RuleFor(x => x.Value).HttpStatusSuccess();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusSuccess.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusSuccess))]
    public void HttpStatusSuccess_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusSuccessValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusSuccessValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusSuccessValidator() => RuleFor(x => x.Value).NotHttpStatusSuccess();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusSuccess.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusSuccess))]
    public void NotHttpStatusSuccess_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusSuccessValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusRedirectValidator : AbstractValidator<IntModel>
    {
        public HttpStatusRedirectValidator() => RuleFor(x => x.Value).HttpStatusRedirect();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusRedirect.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusRedirect))]
    public void HttpStatusRedirect_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusRedirectValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusRedirectValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusRedirectValidator() => RuleFor(x => x.Value).NotHttpStatusRedirect();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusRedirect.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusRedirect))]
    public void NotHttpStatusRedirect_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusRedirectValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusClientErrorValidator : AbstractValidator<IntModel>
    {
        public HttpStatusClientErrorValidator() => RuleFor(x => x.Value).HttpStatusClientError();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusClientError.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusClientError))]
    public void HttpStatusClientError_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusClientErrorValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusClientErrorValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusClientErrorValidator() => RuleFor(x => x.Value).NotHttpStatusClientError();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusClientError.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusClientError))]
    public void NotHttpStatusClientError_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusClientErrorValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HttpStatusServerErrorValidator : AbstractValidator<IntModel>
    {
        public HttpStatusServerErrorValidator() => RuleFor(x => x.Value).HttpStatusServerError();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HttpStatusServerError.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HttpStatusServerError))]
    public void HttpStatusServerError_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new HttpStatusServerErrorValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHttpStatusServerErrorValidator : AbstractValidator<IntModel>
    {
        public NotHttpStatusServerErrorValidator() => RuleFor(x => x.Value).NotHttpStatusServerError();
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHttpStatusServerError.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHttpStatusServerError))]
    public void NotHttpStatusServerError_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotHttpStatusServerErrorValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HasHeaderValidator : AbstractValidator<HeadersModel>
    {
        public HasHeaderValidator(string name) => RuleFor(x => x.Value).HasHeader(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HasHeader.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HasHeader))]
    public void HasHeader_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new HasHeaderValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class NotHasHeaderValidator : AbstractValidator<HeadersModel>
    {
        public NotHasHeaderValidator(string name) => RuleFor(x => x.Value).NotHasHeader(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHasHeader.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHasHeader))]
    public void NotHasHeader_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new NotHasHeaderValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class HasHeaderValueValidator : AbstractValidator<HeadersModel>
    {
        public HasHeaderValueValidator(string name) => RuleFor(x => x.Value).HasHeaderValue(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HasHeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HasHeaderValue))]
    public void HasHeaderValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new HasHeaderValueValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class NotHasHeaderValueValidator : AbstractValidator<HeadersModel>
    {
        public NotHasHeaderValueValidator(string name) => RuleFor(x => x.Value).NotHasHeaderValue(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHasHeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHasHeaderValue))]
    public void NotHasHeaderValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new NotHasHeaderValueValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class HasHeaderValueEqualToValidator : AbstractValidator<HeadersModel>
    {
        public HasHeaderValueEqualToValidator(string name, string expectedValue) => RuleFor(x => x.Value).HasHeaderValueEqualTo(name, expectedValue).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HasHeaderValueEqualTo.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HasHeaderValueEqualTo))]
    public void HasHeaderValueEqualTo_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name, string expectedValue)> tc)
    {
        var result = new HasHeaderValueEqualToValidator(tc.Value.name, tc.Value.expectedValue).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class NotHasHeaderValueEqualToValidator : AbstractValidator<HeadersModel>
    {
        public NotHasHeaderValueEqualToValidator(string name, string expectedValue) => RuleFor(x => x.Value).NotHasHeaderValueEqualTo(name, expectedValue).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHasHeaderValueEqualTo.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHasHeaderValueEqualTo))]
    public void NotHasHeaderValueEqualTo_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name, string expectedValue)> tc)
    {
        var result = new NotHasHeaderValueEqualToValidator(tc.Value.name, tc.Value.expectedValue).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class HasSingleHeaderValueValidator : AbstractValidator<HeadersModel>
    {
        public HasSingleHeaderValueValidator(string name) => RuleFor(x => x.Value).HasSingleHeaderValue(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HasSingleHeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HasSingleHeaderValue))]
    public void HasSingleHeaderValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new HasSingleHeaderValueValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class NotHasSingleHeaderValueValidator : AbstractValidator<HeadersModel>
    {
        public NotHasSingleHeaderValueValidator(string name) => RuleFor(x => x.Value).NotHasSingleHeaderValue(name).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHasSingleHeaderValue.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHasSingleHeaderValue))]
    public void NotHasSingleHeaderValue_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)> tc)
    {
        var result = new NotHasSingleHeaderValueValidator(tc.Value.name).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class HasContentTypeValidator : AbstractValidator<HeadersModel>
    {
        public HasContentTypeValidator(string[] allowed) => RuleFor(x => x.Value).HasContentType(allowed).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.HasContentType.Cases), MemberType = typeof(FluentHttpExtensionsTestData.HasContentType))]
    public void HasContentType_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] allowed)> tc)
    {
        var result = new HasContentTypeValidator(tc.Value.allowed).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class NotHasContentTypeValidator : AbstractValidator<HeadersModel>
    {
        public NotHasContentTypeValidator(string[] allowed) => RuleFor(x => x.Value).NotHasContentType(allowed).WithName("Value");
    }

    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.NotHasContentType.Cases), MemberType = typeof(FluentHttpExtensionsTestData.NotHasContentType))]
    public void NotHasContentType_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] allowed)> tc)
    {
        var result = new NotHasContentTypeValidator(tc.Value.allowed).Validate(new HeadersModel { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class MediaTypeValidator : AbstractValidator<StringModel>
    {
        public MediaTypeValidator() => RuleFor(x => x.Value).MediaType();
    }

    // FluentHttpExtensions.MediaType
    [Theory]
    [MemberData(nameof(FluentHttpExtensionsTestData.MediaType.Cases), MemberType = typeof(FluentHttpExtensionsTestData.MediaType))]
    public void MediaType_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new MediaTypeValidator().Validate(new StringModel { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
