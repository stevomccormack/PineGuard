using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Codes;
using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationExceptionHandlerTestData
{
    public static class Constructor
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-options", static () => _ = new MustValidationExceptionHandler(null!, new DefaultMustFailureMessageResolver()), new ExpectedException(typeof(ArgumentNullException), "options")),
            new ActionThrowsCase("null-resolver", static () => _ = new MustValidationExceptionHandler(Options.Create(new MustValidationOptions()), null!), new ExpectedException(typeof(ArgumentNullException), "resolver"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class TryHandleAsync
    {
        public static TheoryData<Case> Cases =>
        [
            new("a-validation-exception-answers-the-story-five-body", (Validation(), Defaults), new HandledExpected(true, new ProblemDetailsExpected(false, 400, ["email", "lines[1].sku"], ["email.address.invalid", "text.content.blank"], ["email must be a valid email address.", "lines[1].sku must not be null or whitespace."], "One or more validation errors occurred."))),
            new("an-unrelated-exception-is-left-to-the-pipeline", (new InvalidOperationException("the order store is offline"), Defaults), new HandledExpected(false)),
            new("a-guard-exception-is-left-to-the-pipeline-by-default", (new ArgumentNullException("email"), Defaults), new HandledExpected(false)),
            new("a-guard-exception-becomes-a-bad-request-when-handled", (new ArgumentNullException("email"), HandleGuards), new HandledExpected(true, new ProblemDetailsExpected(false, 400, ["email"], [MustCodes.Value.Argument.Invalid]))),
            new("an-out-of-range-guard-exception-becomes-a-bad-request-when-handled", (new ArgumentOutOfRangeException("count"), HandleGuards), new HandledExpected(true, new ProblemDetailsExpected(false, 400, ["count"], [MustCodes.Value.Argument.Invalid]))),
            new("a-stamped-guard-exception-publishes-its-own-code-and-path", (Stamped(new ArgumentException("Order.Email must not be null or whitespace.", "email"), MustCodes.Text.Content.Blank, "Order.Email"), HandleGuards), new HandledExpected(true, new ProblemDetailsExpected(false, 400, ["order.email"], [MustCodes.Text.Content.Blank]))),
            new("a-guard-exception-without-a-parameter-name-reports-the-root-path", (new ArgumentException("the order is not consistent"), HandleGuards), new HandledExpected(true, new ProblemDetailsExpected(false, 400, [""], [MustCodes.Value.Argument.Invalid])))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FuncThrowsCase("null-http-context", static () => Handler().TryHandleAsync(null!, new InvalidOperationException("boom"), CancellationToken.None).AsTask(), new ExpectedException(typeof(ArgumentNullException), "httpContext")),
            new FuncThrowsCase("null-exception", static () => Handler().TryHandleAsync(new DefaultHttpContext(), null!, CancellationToken.None).AsTask(), new ExpectedException(typeof(ArgumentNullException), "exception"))
        ];

        private static MustValidationExceptionHandler Handler() =>
            new(Options.Create(new MustValidationOptions()), new DefaultMustFailureMessageResolver());

        private static MustValidationException Validation() =>
            new(MustValidationResult.Fail(SampleFailures.Email, SampleFailures.LineSku));

        /// <summary>
        /// Stamps <paramref name="exception"/> the way <see cref="GuardFailure"/> does, so the handler reads
        /// the code and path a real guard would have left behind.
        /// </summary>
        /// <param name="exception">The exception to stamp.</param>
        /// <param name="code">The catalogue code the guard failed on.</param>
        /// <param name="propertyPath">The path the guard failed on.</param>
        private static ArgumentException Stamped(ArgumentException exception, string code, string propertyPath)
        {
            exception.Data[GuardFailure.CodeDataKey] = code;
            exception.Data[GuardFailure.PropertyPathDataKey] = propertyPath;

            return exception;
        }

        private static void Defaults(IServiceCollection services) => _ = services;

        private static void HandleGuards(IServiceCollection services) =>
            services.Configure<MustValidationOptions>(static options => options.HandleGuardExceptions = true);

        /// <param name="IsValid">Whether the handler answered the exception itself.</param>
        /// <param name="Body">What the answer says, when there is one.</param>
        public sealed record HandledExpected(bool IsValid, ProblemDetailsExpected? Body = null) : ReturnExpected(IsValid);

        public sealed record Case(string Name, (Exception exception, Action<IServiceCollection> configureServices) Value, HandledExpected Expected)
            : ReturnCase<(Exception exception, Action<IServiceCollection> configureServices), HandledExpected>(Name, Value, Expected);

        private sealed record FuncThrowsCase(string Name, Func<Task> Value, ExpectedException ExpectedException)
            : ThrowsCase<Func<Task>>(Name, Value, ExpectedException);
    }
}
