using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CldrTimeZoneUtilityTestData
{
    public static class ToIanaTimeZone
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null) },
            { new Case("empty", string.Empty) },
            { new Case("space", " ") },
            { new Case("whitespace", "\t\r\n") },
            { new Case("not a zone", "Not/AZone") },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? WindowsTimeZoneId)
            : ValueCase<string?>(Name, WindowsTimeZoneId);

        #endregion Cases
    }

    public static class TryGetSystemTimeZone
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null) },
            { new Case("empty", string.Empty) },
            { new Case("space", " ") },
            { new Case("whitespace", "\t\r\n") },
            { new Case("not a zone", "Not/AZone") },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? WindowsOrIanaTimeZoneId)
            : ValueCase<string?>(Name, WindowsOrIanaTimeZoneId);

        #endregion Cases
    }

    public static class ToWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null) },
            { new Case("space", " ") },
            { new Case("whitespace", "\t\r\n") },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? IanaTimeZoneId)
            : ValueCase<string?>(Name, IanaTimeZoneId);

        #endregion Cases
    }

    public static class TryParseWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("UTC", "UTC", true, "UTC") },
            { new Case("trimmed", "  UTC  ", true, "UTC") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false, string.Empty) },
            { new Case("empty", string.Empty, false, string.Empty) },
            { new Case("space", " ", false, string.Empty) },
            { new Case("whitespace", "\t\r\n", false, string.Empty) },
            { new Case("not a zone", "Not/AZone", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected, string ExpectedWindowsId)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedWindowsId);

        #endregion Cases
    }

    public static class TryGetSystemTimeZoneSystemId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("UTC => UTC", "UTC", "UTC") },
        };

        public static TheoryData<Case> EdgeCases => [];

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string IanaTimeZoneId, string? ExpectedSystemId)
            : ReturnCase<string, string?>(Name, IanaTimeZoneId, ExpectedSystemId);

        #endregion Cases
    }
}
