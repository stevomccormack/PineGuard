using PineGuard.Extensions.Options.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace PineGuard.Extensions.Options.UnitTests;

public static class MustRulesValidateOptionsTestData
{
    private static readonly SmtpOptionsValidator Validator = new();
    private static readonly SmtpOptions ValidOptions = new() { Host = "smtp.example.com", Port = 25, From = "noreply@example.com", UseTls = false };
    private static readonly SmtpOptions OnlyHostInvalid = new() { Host = "not a valid host", Port = 25, From = "noreply@example.com", UseTls = false };
    private static readonly SmtpOptions Unconfigured = new();

    public static class Constructor
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-validator", () => new MustRulesValidateOptions<SmtpOptions>(null, null!), new ExpectedException(typeof(ArgumentNullException), "validator"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class Validate
    {
        public static TheoryData<ValidateOptionsCase<(string? registeredName, string? name, SmtpOptions options)>> Cases =>
        [
            new("default-name-match", (MicrosoftOptions.DefaultName, MicrosoftOptions.DefaultName, ValidOptions), new ValidateOptionsExpected(true)),
            new("named-match", ("Marketing", "Marketing", ValidOptions), new ValidateOptionsExpected(true)),
            new("named-mismatch-skips", ("Marketing", "Other", ValidOptions), new ValidateOptionsExpected(false, null, true)),
            new("null-registered-name-matches-any", (null, "AnythingAtAll", ValidOptions), new ValidateOptionsExpected(true)),
            new("one-failure", (MicrosoftOptions.DefaultName, MicrosoftOptions.DefaultName, OnlyHostInvalid), new ValidateOptionsExpected(false, OneFailureMessage, false, [OneFailureMessage])),
            new("three-failures-in-order", (MicrosoftOptions.DefaultName, MicrosoftOptions.DefaultName, Unconfigured), new ValidateOptionsExpected(false, null, false, [HostNullFailureMessage, PortInvalidFailureMessage, FromNullFailureMessage]))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-options", () => new MustRulesValidateOptions<SmtpOptions>(MicrosoftOptions.DefaultName, Validator).Validate(MicrosoftOptions.DefaultName, null!), new ExpectedException(typeof(ArgumentNullException), "options"))
        ];

        private const string OneFailureMessage = "SmtpOptions.Host: Host must be a valid hostname. [network.hostname.invalid]";
        private const string HostNullFailureMessage = "SmtpOptions.Host: Host must not be null. [network.hostname.invalid]";
        private const string PortInvalidFailureMessage = "SmtpOptions.Port: Port must be a valid port number. [network.port.invalid]";
        private const string FromNullFailureMessage = "SmtpOptions.From: From must not be null. [email.address.invalid]";

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class FormatFailure
    {
        public static TheoryData<Case> Cases =>
        [
            new("with-property-path", new MustFailure("Host", "network.hostname.invalid", "Host must be a valid hostname.", null), "SmtpOptions.Host: Host must be a valid hostname. [network.hostname.invalid]"),
            new("without-property-path", new MustFailure("", "value.state.null", "SmtpOptions must not be null.", null), "SmtpOptions: SmtpOptions must not be null. [value.state.null]")
        ];

        public sealed record Case(string Name, MustFailure Value, string Expected)
            : ReturnCase<MustFailure, string>(Name, Value, Expected);
    }
}
