using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentJsonExtensionsTests(ITestOutputHelper output)
    : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }
    private sealed record HeaderModel { public IReadOnlyDictionary<string, IEnumerable<string>>? Value { get; init; } }

    private sealed class JsonValidator : AbstractValidator<Model>
    {
        public JsonValidator() => RuleFor(x => x.Value).Json();
    }

    private sealed class JsonObjectValidator : AbstractValidator<Model>
    {
        public JsonObjectValidator() => RuleFor(x => x.Value).JsonObject();
    }

    private sealed class JsonArrayValidator : AbstractValidator<Model>
    {
        public JsonArrayValidator() => RuleFor(x => x.Value).JsonArray();
    }

    private sealed class JsonContentTypeValidator : AbstractValidator<HeaderModel>
    {
        public JsonContentTypeValidator() => RuleFor(x => x.Value).JsonContentType();
    }

    [Theory]
    [MemberData(nameof(FluentJsonExtensionsTestData.Json.Cases), MemberType = typeof(FluentJsonExtensionsTestData.Json))]
    public void Json_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new JsonValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentJsonExtensionsTestData.JsonObject.Cases), MemberType = typeof(FluentJsonExtensionsTestData.JsonObject))]
    public void JsonObject_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new JsonObjectValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentJsonExtensionsTestData.JsonArray.Cases), MemberType = typeof(FluentJsonExtensionsTestData.JsonArray))]
    public void JsonArray_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new JsonArrayValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentJsonExtensionsTestData.JsonContentType.Cases), MemberType = typeof(FluentJsonExtensionsTestData.JsonContentType))]
    public void JsonContentType_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = new JsonContentTypeValidator().Validate(new HeaderModel { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
