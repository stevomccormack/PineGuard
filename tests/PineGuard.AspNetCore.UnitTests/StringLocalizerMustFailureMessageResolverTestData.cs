using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class StringLocalizerMustFailureMessageResolverTestData
{
    public static readonly Dictionary<string, string> FrenchResources = new(StringComparer.Ordinal)
    {
        ["email.address.invalid"] = "{paramName} doit être une adresse e-mail valide.",
        ["text.content.blank"] = "{paramName} ne doit pas être vide.",
        ["value.state.invalid"] = "La commande est incohérente."
    };

    public sealed record ResolveExpected(bool IsValid, string Message, Type? RequestedResourceSource = null);

    public static class Resolve
    {
        public static TheoryData<Case> Cases =>
        [
            new("localized-template-is-rendered-with-the-property-path", (SampleFailures.Email, true, null), new ResolveExpected(true, "Email doit être une adresse e-mail valide.", typeof(MustValidationOptions))),
            new("indexed-property-path-is-rendered-into-the-template", (SampleFailures.LineSku, true, null), new ResolveExpected(true, "Lines[1].Sku ne doit pas être vide.", typeof(MustValidationOptions))),
            new("template-without-a-placeholder-is-published-as-is", (SampleFailures.Root, true, null), new ResolveExpected(true, "La commande est incohérente.", typeof(MustValidationOptions))),
            new("configured-resource-type-is-the-one-asked-for", (SampleFailures.Email, true, typeof(StringLocalizerMustFailureMessageResolverTests)), new ResolveExpected(true, "Email doit être une adresse e-mail valide.", typeof(StringLocalizerMustFailureMessageResolverTests))),
            new("unknown-code-falls-back-to-the-rendered-message", (SampleFailures.EmailTooLong, true, null), new ResolveExpected(false, "Email must be at most 256 characters.", typeof(MustValidationOptions))),
            new("no-localizer-factory-falls-back-to-the-rendered-message", (SampleFailures.Email, false, null), new ResolveExpected(false, "Email must be a valid email address."))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new FailureThrowsCase("null-failure", null!, new ExpectedException(typeof(ArgumentNullException), "failure"))
        ];

        public sealed record Case(string Name, (MustFailure failure, bool hasLocalizerFactory, Type? localizationResourceType) Value, ResolveExpected Expected)
            : ReturnCase<(MustFailure failure, bool hasLocalizerFactory, Type? localizationResourceType), ResolveExpected>(Name, Value, Expected);

        private sealed record FailureThrowsCase(string Name, MustFailure Value, ExpectedException ExpectedException)
            : ThrowsCase<MustFailure>(Name, Value, ExpectedException);
    }
}
