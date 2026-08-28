using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class ExceptionExtensionTestData
{
    public static class RoundTrip
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class Unstamped
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class HasMustCode
    {
        public static TheoryData<Case> Cases =>
        [
            new("matching code returns true", CodeMatches: true, Expected: true),
            new("mismatched code returns false", CodeMatches: false, Expected: false)
        ];

        public sealed record Case(string Name, bool CodeMatches, bool Expected) : BaseCase(Name);
    }

    public static class NullArgumentGuards
    {
        public static TheoryData<bool> Cases => [true];
    }
}
