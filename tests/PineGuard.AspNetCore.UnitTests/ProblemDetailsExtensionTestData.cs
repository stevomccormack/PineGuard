using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace PineGuard.AspNetCore.UnitTests;

public static class ProblemDetailsExtensionTestData
{
    private static readonly MustValidationOptions DefaultOptions = new();
    private static readonly MustValidationOptions WithoutCodes = new() { IncludeCodes = false };
    private static readonly MustValidationOptions WithCustomTitle = new() { Title = "Your request was rejected." };

    private static MustValidationResult OrderFailures => MustValidationResult.Fail(SampleFailures.Email, SampleFailures.LineSku);

    public static class ToValidationProblemDetails
    {
        public static TheoryData<Case> Cases =>
        [
            new("camel-case-policy-transforms-keys-and-messages", (OrderFailures, DefaultOptions, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred.")),
            new("no-naming-policy-leaves-paths-and-messages-unchanged", (OrderFailures, DefaultOptions, null), new ProblemDetailsExpected(false, 400, ["Email", "Lines[1].Sku"], ["email.address.invalid", "text.content.blank"], ["Email must be a valid email address.", "Lines[1].Sku must not be null or whitespace."])),
            new("root-failure-is-published-under-the-empty-key", (MustValidationResult.Fail(SampleFailures.Root), DefaultOptions, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(false, 400, [""], ["value.state.invalid"], ["The order is not consistent."])),
            new("include-codes-disabled-omits-the-failures-extension", (OrderFailures, WithoutCodes, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], null, ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."])),
            new("two-failures-on-one-property-share-one-errors-entry", (MustValidationResult.Fail(SampleFailures.Email, SampleFailures.EmailTooLong), DefaultOptions, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(false, 400, ["email"], ["email.address.invalid", "text.length.above-maximum"], ["email must be a valid email address.", "email must be at most 256 characters."])),
            new("failures-keep-the-order-the-validator-found-them-in", (MustValidationResult.Fail(SampleFailures.LineSku, SampleFailures.Email, SampleFailures.EmailTooLong), DefaultOptions, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(false, 400, ["lines[1].sku", "email"], ["text.content.blank", "email.address.invalid", "text.length.above-maximum"])),
            new("custom-title-is-published", (OrderFailures, WithCustomTitle, null), new ProblemDetailsExpected(false, 400, ["Email", "Lines[1].Sku"], ["email.address.invalid", "text.content.blank"], null, "Your request was rejected.")),
            new("successful-result-produces-an-empty-errors-dictionary", (MustValidationResult.Ok(), DefaultOptions, JsonNamingPolicy.CamelCase), new ProblemDetailsExpected(true, 400, [], []))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => ProblemDetailsExtension.ToValidationProblemDetails(null!, DefaultOptions, null, new DefaultMustFailureMessageResolver(), new DefaultHttpContext()), new ExpectedException(typeof(ArgumentNullException), "result")),
            new ActionThrowsCase("null-options", () => OrderFailures.ToValidationProblemDetails(null!, null, new DefaultMustFailureMessageResolver(), new DefaultHttpContext()), new ExpectedException(typeof(ArgumentNullException), "options")),
            new ActionThrowsCase("null-resolver", () => OrderFailures.ToValidationProblemDetails(DefaultOptions, null, null!, new DefaultHttpContext()), new ExpectedException(typeof(ArgumentNullException), "resolver")),
            new ActionThrowsCase("null-http-context", () => OrderFailures.ToValidationProblemDetails(DefaultOptions, null, new DefaultMustFailureMessageResolver(), null!), new ExpectedException(typeof(ArgumentNullException), "httpContext"))
        ];

        public sealed record Case(string Name, (MustValidationResult result, MustValidationOptions options, JsonNamingPolicy? namingPolicy) Value, ProblemDetailsExpected Expected)
            : ReturnCase<(MustValidationResult result, MustValidationOptions options, JsonNamingPolicy? namingPolicy), ProblemDetailsExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ToValidationProblemDetailsFromServices
    {
        public static TheoryData<Case> Cases =>
        [
            new("options-policy-and-resolver-come-from-the-request-services", (OrderFailures, ConfigureCamelCaseServices), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."])),
            new("configured-options-are-honoured", (OrderFailures, ConfigureWithoutCodes), new ProblemDetailsExpected(false, 400, ["Email", "Lines[1].Sku"], null, ["Email must be a valid email address.", "Lines[1].Sku must not be null or whitespace."], "Your request was rejected."))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-result", () => ProblemDetailsExtension.ToValidationProblemDetails(null!, new DefaultHttpContext()), new ExpectedException(typeof(ArgumentNullException), "result")),
            new ActionThrowsCase("null-http-context", () => OrderFailures.ToValidationProblemDetails(null!), new ExpectedException(typeof(ArgumentNullException), "httpContext"))
        ];

        private static void ConfigureCamelCaseServices(IServiceCollection services)
        {
            services.Configure<MustValidationOptions>(_ => { });
            services.Configure<HttpJsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            services.AddSingleton<IMustFailureMessageResolver, DefaultMustFailureMessageResolver>();
        }

        private static void ConfigureWithoutCodes(IServiceCollection services)
        {
            services.Configure<MustValidationOptions>(options =>
            {
                options.IncludeCodes = false;
                options.UseJsonNamingPolicy = false;
                options.Title = "Your request was rejected.";
            });
            services.AddSingleton<IMustFailureMessageResolver, DefaultMustFailureMessageResolver>();
        }

        public sealed record Case(string Name, (MustValidationResult result, Action<IServiceCollection> configureServices) Value, ProblemDetailsExpected Expected)
            : ReturnCase<(MustValidationResult result, Action<IServiceCollection> configureServices), ProblemDetailsExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ResolveNamingPolicy
    {
        public static TheoryData<Case> Cases =>
        [
            new("explicit-policy-wins-over-the-application-policy", (new MustValidationOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower }, ConfigureHttpCamelCase), new NamingPolicyExpected(true, "sku_code")),
            new("disabled-json-naming-policy-publishes-paths-unchanged", (new MustValidationOptions { UseJsonNamingPolicy = false }, ConfigureHttpCamelCase), new NamingPolicyExpected(false)),
            new("minimal-api-json-options-supply-the-policy", (new MustValidationOptions(), ConfigureHttpCamelCase), new NamingPolicyExpected(true, "skuCode")),
            new("mvc-json-options-supply-the-policy-when-minimal-api-has-none", (new MustValidationOptions(), ConfigureMvcCamelCaseOnly), new NamingPolicyExpected(true, "skuCode")),
            new("no-json-options-registered-publishes-paths-unchanged", (new MustValidationOptions(), _ => { }), new NamingPolicyExpected(false))
        ];

        private static void ConfigureHttpCamelCase(IServiceCollection services) =>
            services.Configure<HttpJsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

        private static void ConfigureMvcCamelCaseOnly(IServiceCollection services)
        {
            services.Configure<HttpJsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = null);
            services.Configure<MvcJsonOptions>(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }

        public sealed record NamingPolicyExpected(bool IsValid, string? ConvertedName = null) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (MustValidationOptions options, Action<IServiceCollection> configureServices) Value, NamingPolicyExpected Expected)
            : ReturnCase<(MustValidationOptions options, Action<IServiceCollection> configureServices), NamingPolicyExpected>(Name, Value, Expected);
    }
}
