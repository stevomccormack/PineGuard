using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.Extensions.Options.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace PineGuard.Extensions.Options.UnitTests;

public static class OptionsBuilderExtensionTestData
{
    private static readonly SmtpOptions ValidSmtpOptions = new() { Host = "smtp.example.com", Port = 25, From = "noreply@example.com", UseTls = false };
    private static readonly SmtpOptions InvalidHostSmtpOptions = new() { Host = "not a valid host", Port = 25, From = "noreply@example.com", UseTls = false };

    public sealed record ResolveExpected(Type? ExceptionType = null, string? MessageContains = null);

    public static class ValidateMustRules
    {
        public static TheoryData<Case> Cases =>
        [
            new("valid-options-resolves-successfully", (ValidSmtpOptions, true), new ResolveExpected()),
            new("invalid-options-throws-validation-exception", (InvalidHostSmtpOptions, true), new ResolveExpected(typeof(OptionsValidationException), "SmtpOptions.Host")),
            new("validator-never-registered-defers-failure-to-resolution", (ValidSmtpOptions, false), new ResolveExpected(typeof(InvalidOperationException), nameof(IMustValidator<SmtpOptions>)))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-builder", () => OptionsBuilderExtension.ValidateMustRules<SmtpOptions>(null!), new ExpectedException(typeof(ArgumentNullException), "builder"))
        ];

        public sealed record Case(string Name, (SmtpOptions options, bool registerValidator) Value, ResolveExpected Expected)
            : ReturnCase<(SmtpOptions options, bool registerValidator), ResolveExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ValidateMustRulesInstance
    {
        public static TheoryData<Case> Cases =>
        [
            new("valid-options-resolves-successfully", ValidSmtpOptions, new ResolveExpected()),
            new("invalid-options-throws-validation-exception", InvalidHostSmtpOptions, new ResolveExpected(typeof(OptionsValidationException), "SmtpOptions.Host"))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-builder", () => OptionsBuilderExtension.ValidateMustRules(null!, new SmtpOptionsValidator()), new ExpectedException(typeof(ArgumentNullException), "builder")),
            new ActionThrowsCase("null-validator", () => OptionsBuilderExtension.ValidateMustRules(NewSmtpBuilder(), (IMustValidator<SmtpOptions>)null!), new ExpectedException(typeof(ArgumentNullException), "validator"))
        ];

        public sealed record Case(string Name, SmtpOptions Value, ResolveExpected Expected)
            : ReturnCase<SmtpOptions, ResolveExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class ValidateMustRulesInline
    {
        public static TheoryData<Case> Cases =>
        [
            new("valid-ttl-seconds-rules-honoured", 60, new ResolveExpected()),
            new("invalid-ttl-seconds-rules-honoured", 0, new ResolveExpected(typeof(OptionsValidationException), "TtlSeconds"))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-builder", () => OptionsBuilderExtension.ValidateMustRules((OptionsBuilder<CacheOptions>)null!, _ => { }), new ExpectedException(typeof(ArgumentNullException), "builder")),
            new ActionThrowsCase("null-configure", () => OptionsBuilderExtension.ValidateMustRules(NewCacheBuilder(), (Action<InlineMustValidator<CacheOptions>>)null!), new ExpectedException(typeof(ArgumentNullException), "configure"))
        ];

        public sealed record Case(string Name, int Value, ResolveExpected Expected)
            : ReturnCase<int, ResolveExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    private static OptionsBuilder<SmtpOptions> NewSmtpBuilder() => new(new ServiceCollection(), MicrosoftOptions.DefaultName);

    private static OptionsBuilder<CacheOptions> NewCacheBuilder() => new(new ServiceCollection(), MicrosoftOptions.DefaultName);
}
