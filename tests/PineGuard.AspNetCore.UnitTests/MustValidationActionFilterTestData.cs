using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationActionFilterTestData
{
    public static class Constructor
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-options", () => _ = new MustValidationActionFilter(null!, new DefaultMustFailureMessageResolver()), new ExpectedException(typeof(ArgumentNullException), "options")),
            new ActionThrowsCase("null-resolver", () => _ = new MustValidationActionFilter(Options.Create(new MustValidationOptions()), null!), new ExpectedException(typeof(ArgumentNullException), "resolver"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class OnActionExecutionAsync
    {
        public static TheoryData<Case> Cases =>
        [
            new("no-action-arguments-run-the-action", (Arguments(), CamelCase), new ProblemDetailsExpected(true)),
            new("an-argument-without-a-validator-runs-the-action", (Arguments(("customer", new Customer())), CamelCase), new ProblemDetailsExpected(true)),
            new("a-valid-argument-runs-the-action", (Arguments(("order", CreateOrder.Valid)), CamelCase), new ProblemDetailsExpected(true)),
            new("a-null-argument-runs-the-action", (Arguments(("order", null)), CamelCase), new ProblemDetailsExpected(true)),
            new("an-invalid-argument-answers-the-story-three-body", (Arguments(("order", CreateOrder.Invalid)), CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred.")),
            new("without-a-naming-policy-the-declared-paths-are-published", (Arguments(("order", CreateOrder.Invalid)), NoNamingPolicy), new ProblemDetailsExpected(false, 400, ["Email", "Lines[1].Sku"], ["email.address.invalid", "text.content.blank"])),
            new("two-invalid-arguments-merge-into-one-body", (Arguments(("order", CreateOrder.Invalid), ("query", SearchQuery.Invalid)), CamelCase), new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku", "term"], ["email.address.invalid", "text.content.blank", "text.content.blank"]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FuncThrowsCase("null-context", () => Filter().OnActionExecutionAsync(null!, static () => Task.FromResult<ActionExecutedContext>(null!)), new ExpectedException(typeof(ArgumentNullException), "context")),
            new FuncThrowsCase("null-next", () => Filter().OnActionExecutionAsync(SampleActions.Executing(new DefaultHttpContext(), Arguments()), null!), new ExpectedException(typeof(ArgumentNullException), "next"))
        ];

        private static MustValidationActionFilter Filter() =>
            new(Options.Create(new MustValidationOptions()), new DefaultMustFailureMessageResolver());

        private static Dictionary<string, object?> Arguments(params (string name, object? value)[] arguments)
        {
            var actionArguments = new Dictionary<string, object?>(StringComparer.Ordinal);

            foreach (var (name, value) in arguments)
                actionArguments[name] = value;

            return actionArguments;
        }

        /// <summary>
        /// Lets MVC's own JSON options supply the policy: the Minimal API options are silenced first,
        /// because their <c>JsonSerializerDefaults.Web</c> baseline is already camelCase and would otherwise
        /// answer before MVC is ever asked.
        /// </summary>
        private static void CamelCase(IServiceCollection services)
        {
            AddValidators(services);
            services.Configure<HttpJsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = null);
            services.Configure<MvcJsonOptions>(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        }

        /// <summary>
        /// Opts out of the application's JSON policy, which is the only way to publish declared paths in a
        /// host that has the options infrastructure registered.
        /// </summary>
        private static void NoNamingPolicy(IServiceCollection services)
        {
            AddValidators(services);
            services.Configure<MustValidationOptions>(options => options.UseJsonNamingPolicy = false);
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddMustValidator<CreateOrderValidator>();
            services.AddMustValidator<SearchQueryValidator>();
        }

        public sealed record Case(string Name, (Dictionary<string, object?> actionArguments, Action<IServiceCollection> configureServices) Value, ProblemDetailsExpected Expected)
            : ReturnCase<(Dictionary<string, object?> actionArguments, Action<IServiceCollection> configureServices), ProblemDetailsExpected>(Name, Value, Expected);

        private sealed record FuncThrowsCase(string Name, Func<Task> Value, ExpectedException ExpectedException)
            : ThrowsCase<Func<Task>>(Name, Value, ExpectedException);
    }

    /// <summary>
    /// Plan 03's story 3 sent as real requests through <see cref="SampleMvcApi"/>, asserting the body MVC
    /// actually serialised.
    /// </summary>
    /// <remarks>
    /// The expected body is the same one
    /// <see cref="MustValidationEndpointFilterTestData.EndToEnd"/> asserts for story 2, which is the promise
    /// story 3 makes: an MVC action and a Minimal API endpoint answer the same bad request identically, with
    /// neither application configuring a JSON naming policy for itself.
    /// </remarks>
    public static class EndToEnd
    {
        private const string ValidOrderBody = """{"email":"buyer@example.test"}""";

        private const string InvalidOrderBody = """{"email":"not-an-email"}""";

        private const string CustomerBody = """{"name":"Ada"}""";

        public static TheoryData<Case> Cases =>
        [
            new("a-valid-body-runs-the-action", ("/mvc/orders", ValidOrderBody), new ResponseExpected(true, StatusCodes.Status200OK)),
            new("an-invalid-body-answers-the-story-three-body", ("/mvc/orders", InvalidOrderBody), new ResponseExpected(false, StatusCodes.Status400BadRequest, StoryThreeBody)),
            new("an-action-with-nothing-to-validate-is-left-alone", ("/mvc/customers", CustomerBody), new ResponseExpected(true, StatusCodes.Status200OK))
        ];

        /// <summary>
        /// The body story 3 publishes — byte-for-byte the one story 2 publishes, modulo whitespace.
        /// </summary>
        private static ProblemDetailsExpected StoryThreeBody =>
            new(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred.");

        /// <param name="IsValid">Whether the request reached its action.</param>
        /// <param name="Status">The status code the client read.</param>
        /// <param name="Body">What the response says, when PineGuard answered it instead of the action.</param>
        public sealed record ResponseExpected(bool IsValid, int Status, ProblemDetailsExpected? Body = null) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (string requestUri, string json) Value, ResponseExpected Expected)
            : ReturnCase<(string requestUri, string json), ResponseExpected>(Name, Value, Expected);
    }
}
