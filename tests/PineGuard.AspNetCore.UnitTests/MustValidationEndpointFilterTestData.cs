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
}
