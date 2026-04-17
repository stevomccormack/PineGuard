using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class GuardFailureTestData
{
    public static class Throw
    {
        public static TheoryData<Case> Cases =>
        [
            new("exceptionCreator wins over configured global policy", true, true, "x", true, typeof(ApplicationException), null),
            new("null value falls back to ArgumentNullException when no policy applies", false, false, null, false, typeof(ArgumentNullException), "value"),
            new("non-null value falls back to ArgumentException when no policy applies", false, false, "x", false, typeof(ArgumentException), "value"),
            new("global replacer replaces the default exception when ReplaceDefaultExceptions is true", true, true, "x", false, typeof(InvalidOperationException), "wrapped"),
            new("global replacer does not replace ArgumentException when ReplaceDefaultExceptions is false", false, true, "x", false, typeof(ArgumentException), "value"),
            new("null exceptionCreator result falls back to the default exception", false, false, "x", true, typeof(ArgumentException), "value")
        ];

        public sealed record Case(string Name, bool ReplaceDefaultExceptions, bool ConfigureReplacer, string? Value, bool UseExceptionCreator, Type ExpectedExceptionType, string? ExpectedMessage)
            : ValueCase<string?>(Name, Value);
    }

    public static class ThrowScoped
    {
        public static TheoryData<Case> Cases =>
        [
            new("scoped policy overrides the global policy", GlobalReplaceDefaultExceptions: true, ScopedReplaceDefaultExceptions: true, ExpectedExceptionType: typeof(NotSupportedException), ExpectedMessage: "scoped"),
            new("scoped policy can disable replacement that is enabled globally", GlobalReplaceDefaultExceptions: true, ScopedReplaceDefaultExceptions: false, ExpectedExceptionType: typeof(ArgumentException), ExpectedMessage: "value")
        ];

        public sealed record Case(string Name, bool GlobalReplaceDefaultExceptions, bool ScopedReplaceDefaultExceptions, Type ExpectedExceptionType, string ExpectedMessage)
            : BaseCase(Name);
    }

    public static class ThrowCreatorPrecedence
    {
        public static TheoryData<Case> Cases =>
        [
            new("exceptionCreator beats both scoped and global policy", GlobalReplaceDefaultExceptions: true, ScopedReplaceDefaultExceptions: true, ExpectedExceptionType: typeof(ApplicationException), ExpectedMessage: "creator")
        ];

        public sealed record Case(string Name, bool GlobalReplaceDefaultExceptions, bool ScopedReplaceDefaultExceptions, Type ExpectedExceptionType, string ExpectedMessage)
            : BaseCase(Name);
    }

    public static class ThrowAndReplace
    {
        public static TheoryData<Case> Cases =>
        [
            new("explicit per-call replacer wins when ReplaceDefaultExceptions is false", false, typeof(InvalidOperationException)),
            new("explicit per-call replacer wins when ReplaceDefaultExceptions is true", true, typeof(InvalidOperationException))
        ];

        public sealed record Case(string Name, bool ReplaceDefaultExceptions, Type ExpectedExceptionType)
            : ValueCase<bool>(Name, ReplaceDefaultExceptions);
    }

    public static class ThrowAndReplaceWithScopedPolicy
    {
        public static TheoryData<Case> Cases =>
        [
            new("explicit per-call replacer beats both scoped and global policy", GlobalReplaceDefaultExceptions: true, ScopedReplaceDefaultExceptions: true, ExpectedExceptionType: typeof(ApplicationException))
        ];

        public sealed record Case(string Name, bool GlobalReplaceDefaultExceptions, bool ScopedReplaceDefaultExceptions, Type ExpectedExceptionType)
            : BaseCase(Name);
    }
}
