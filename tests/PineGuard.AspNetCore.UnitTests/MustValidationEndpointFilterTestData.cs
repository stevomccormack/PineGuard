using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationEndpointFilterTestData
{
    public static class InvokeAsync
    {
        public static TheoryData<Case> Cases =>
        [
            new("no-arguments-invoke-the-handler", ([], CamelCase), new ProblemDetailsExpected(true)),
            new("an-argument-without-a-validator-invokes-the-handler", ([new Customer()], CamelCase), new ProblemDetailsExpected(true)),
            new("a-valid-argument-invokes-the-handler", ([CreateOrder.Valid], CamelCase), new ProblemDetailsExpected(true)),
            new("a-null-argument-invokes-the-handler", ([null], CamelCase), new ProblemDetailsExpected(true)),
            new("an-invalid-argument-answers-the-story-two-body", ([CreateOrder.Invalid], CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred.")),
            new("without-a-naming-policy-the-declared-paths-are-published", ([CreateOrder.Invalid], NoNamingPolicy), new ProblemDetailsExpected(false, 400, ["Email", "Lines[1].Sku"], ["email.address.invalid", "text.content.blank"])),
            new("two-invalid-arguments-merge-into-one-body", ([CreateOrder.Invalid, SearchQuery.Invalid], CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku", "term"], ["email.address.invalid", "text.content.blank", "text.content.blank"])),
            new("stop-on-first-failure-answers-with-the-first-validator-only", ([CreateOrder.Invalid, SearchQuery.Invalid], StopOnFirstFailure), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"])),
            new("include-codes-disabled-omits-the-failures-extension", ([CreateOrder.Invalid], WithoutCodes), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"])),
            new("the-request-token-reaches-the-validator", ([new TokenProbe()], CamelCase), new ProblemDetailsExpected(true))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FuncThrowsCase("null-context", () => new MustValidationEndpointFilter().InvokeAsync(null!, static _ => ValueTask.FromResult<object?>(null)).AsTask(), new ExpectedException(typeof(ArgumentNullException), "context")),
            new FuncThrowsCase("null-next", () => new MustValidationEndpointFilter().InvokeAsync(new DefaultEndpointFilterInvocationContext(new DefaultHttpContext()), null!).AsTask(), new ExpectedException(typeof(ArgumentNullException), "next"))
        ];

        private static void CamelCase(IServiceCollection services)
        {
            AddValidators(services);
            services.Configure<HttpJsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }

        /// <summary>
        /// Opts out of the application's JSON policy, which is the only way to publish declared paths:
        /// Minimal API's own <c>JsonOptions</c> is seeded from <see cref="JsonSerializerDefaults.Web"/>, so
        /// leaving it alone still yields camelCase.
        /// </summary>
        private static void NoNamingPolicy(IServiceCollection services)
        {
            AddValidators(services);
            services.Configure<MustValidationOptions>(options => options.UseJsonNamingPolicy = false);
        }

        private static void StopOnFirstFailure(IServiceCollection services)
        {
            CamelCase(services);
            services.Configure<MustValidationOptions>(options => options.Mode = MustValidationMode.StopOnFirstFailure);
        }

        private static void WithoutCodes(IServiceCollection services)
        {
            CamelCase(services);
            services.Configure<MustValidationOptions>(options => options.IncludeCodes = false);
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<SearchQueryValidator>();
            services.AddMustValidator<TokenProbeValidator>();
        }

        public sealed record Case(string Name, (object?[] arguments, Action<IServiceCollection> configureServices) Value, ProblemDetailsExpected Expected)
            : ReturnCase<(object?[] arguments, Action<IServiceCollection> configureServices), ProblemDetailsExpected>(Name, Value, Expected);

        private sealed record FuncThrowsCase(string Name, Func<Task> Value, ExpectedException ExpectedException)
            : ThrowsCase<Func<Task>>(Name, Value, ExpectedException);
    }

    /// <summary>
    /// Plan 03's story 2 (and story 8) sent as real requests through <see cref="SampleMinimalApi"/>, so the
    /// published body is the one a client parses rather than the one the filter returned.
    /// </summary>
    public static class EndToEnd
    {
        private const string ValidOrderBody = """{"email":"buyer@example.test"}""";

        private const string InvalidOrderBody = """{"email":"not-an-email"}""";

        private const string CustomerBody = """{"name":"Ada"}""";

        public static TheoryData<Case> Cases =>
        [
            new("a-valid-body-reaches-the-handler", (HttpMethod.Post, "/orders", ValidOrderBody), new ResponseExpected(true, StatusCodes.Status200OK)),
            new("an-invalid-body-answers-the-story-two-body", (HttpMethod.Post, "/orders", InvalidOrderBody), new ResponseExpected(false, StatusCodes.Status400BadRequest, StoryTwoBody)),
            new("an-endpoint-with-nothing-to-validate-is-left-alone", (HttpMethod.Post, "/customers", CustomerBody), new ResponseExpected(true, StatusCodes.Status200OK)),
            new("a-valid-as-parameters-query-reaches-the-handler", (HttpMethod.Get, "/search?term=pine", null), new ResponseExpected(true, StatusCodes.Status200OK)),
            new("an-invalid-as-parameters-query-is-validated-like-a-body", (HttpMethod.Get, "/search?term=%20%20", null), new ResponseExpected(false, StatusCodes.Status400BadRequest, new ProblemDetailsExpected(false, 400, ["term"], ["text.content.blank"], ["term must not be null or whitespace."]))),
            new("a-group-wide-registration-validates-the-endpoints-in-it", (HttpMethod.Post, "/api/orders", InvalidOrderBody), new ResponseExpected(false, StatusCodes.Status400BadRequest, StoryTwoBody))
        ];

        /// <summary>
        /// The body Plan 03's story 2 publishes verbatim — camel-cased keys, messages naming the field the
        /// same way the keys do, and one <c>failures</c> entry per failure carrying its stable code.
        /// </summary>
        private static ProblemDetailsExpected StoryTwoBody =>
            new(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred.");

        /// <param name="IsValid">Whether the request reached its handler.</param>
        /// <param name="Status">The status code the client read.</param>
        /// <param name="Body">What the response says, when PineGuard answered it instead of the handler.</param>
        public sealed record ResponseExpected(bool IsValid, int Status, ProblemDetailsExpected? Body = null) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (HttpMethod method, string requestUri, string? json) Value, ResponseExpected Expected)
            : ReturnCase<(HttpMethod method, string requestUri, string? json), ResponseExpected>(Name, Value, Expected);
    }
}
