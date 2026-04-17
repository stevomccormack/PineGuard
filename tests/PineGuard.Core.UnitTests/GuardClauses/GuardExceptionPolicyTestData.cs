using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class GuardExceptionPolicyTestData
{
    public static class ExceptionReplacer
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("non-null replacer can be set and retrieved", ex => new InvalidOperationException("wrapped", ex)),
            new("null replacer can be set and retrieved", null)
        ];

        public sealed record Case(string Name, Func<Exception, Exception>? Value)
            : ValueCase<Func<Exception, Exception>?>(Name, Value);
    }

    public static class ReplaceDefaultExceptions
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("false can be set and retrieved", false),
            new("true can be set and retrieved", true)
        ];

        public sealed record Case(string Name, bool Value)
            : ValueCase<bool>(Name, Value);
    }

    public static class ShouldReplace
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("ReplaceDefaultExceptions=false: ArgumentException => false", ReplaceDefaultExceptions: false, Value: new ArgumentException("bad"), Expected: false),
            new("ReplaceDefaultExceptions=false: InvalidOperationException => true", ReplaceDefaultExceptions: false, Value: new InvalidOperationException("bad"), Expected: true),
            new("ReplaceDefaultExceptions=true: ArgumentException => true", ReplaceDefaultExceptions: true, Value: new ArgumentException("bad"), Expected: true),
            new("ReplaceDefaultExceptions=true: InvalidOperationException => true", ReplaceDefaultExceptions: true, Value: new InvalidOperationException("bad"), Expected: true)
        ];

        public sealed record Case(string Name, bool ReplaceDefaultExceptions, Exception Value, bool Expected)
            : ReturnCase<Exception, bool>(Name, Value, Expected);
    }

    public static class BeginScope
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("scope can enable default replacement over a disabled global policy", GlobalReplaceDefaultExceptions: false, ScopedReplaceDefaultExceptions: true),
            new("scope can disable default replacement over an enabled global policy", GlobalReplaceDefaultExceptions: true, ScopedReplaceDefaultExceptions: false)
        ];

        public sealed record Case(string Name, bool GlobalReplaceDefaultExceptions, bool ScopedReplaceDefaultExceptions)
            : BaseCase(Name);
    }

    public static class NestedScope
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("inner scope overrides outer policy and outer policy is restored after inner dispose", GlobalReplaceDefaultExceptions: false, OuterReplaceDefaultExceptions: true, InnerReplaceDefaultExceptions: false)
        ];

        public sealed record Case(string Name, bool GlobalReplaceDefaultExceptions, bool OuterReplaceDefaultExceptions, bool InnerReplaceDefaultExceptions)
            : BaseCase(Name);
    }

    public static class SetPropertyInsideScope
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("setting ExceptionReplacer inside scope updates scope not global", SetExceptionReplacer: true, SetReplaceDefaultExceptions: false),
            new("setting ReplaceDefaultExceptions inside scope updates scope not global", SetExceptionReplacer: false, SetReplaceDefaultExceptions: true),
            new("setting both properties inside scope updates scope not global", SetExceptionReplacer: true, SetReplaceDefaultExceptions: true)
        ];

        public sealed record Case(string Name, bool SetExceptionReplacer, bool SetReplaceDefaultExceptions)
            : BaseCase(Name);
    }

    public static class DoubleDispose
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("disposing a scope twice is idempotent")
        ];

        public sealed record Case(string Name)
            : BaseCase(Name);
    }

    public static class StaleDispose
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("disposing a stale scope does not affect current scope")
        ];

        public sealed record Case(string Name)
            : BaseCase(Name);
    }
}
