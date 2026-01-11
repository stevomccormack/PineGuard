using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CldrTimeZoneUtilityTestData
{
    public static class ToIanaTimeZone
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null),
            new("empty", string.Empty),
            new("space", " "),
            new("whitespace", "\t\r\n"),
            new("not a zone", "Not/AZone"),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? WindowsTimeZoneId)
            : ReturnCase<string?, TimeZoneInfo?>(Name, WindowsTimeZoneId, null);

        #endregion Cases
    }

    public static class TryGetSystemTimeZone
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null),
            new("empty", string.Empty),
            new("space", " "),
            new("whitespace", "\t\r\n"),
            new("not a zone", "Not/AZone"),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? WindowsOrIanaTimeZoneId)
            : TryCase<string?, TimeZoneInfo?>(Name, WindowsOrIanaTimeZoneId, false, null);

        #endregion Cases
    }

    public static class ToWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases => [];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null),
            new("space", " "),
            new("whitespace", "\t\r\n"),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? IanaTimeZoneId)
            : ReturnCase<string?, string?>(Name, IanaTimeZoneId, null);

        #endregion Cases
    }

    public static class TryParseWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("UTC", "UTC", true, "UTC"),
            new("trimmed", "  UTC  ", true, "UTC"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("not a zone", "Not/AZone", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion Cases
    }

    public static class TryGetSystemTimeZoneSystemId
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("UTC => UTC", "UTC", "UTC"),
        ];

        public static TheoryData<Case> EdgeCases => [];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string IanaTimeZoneId, string? ExpectedSystemId)
            : ReturnCase<string, string?>(Name, IanaTimeZoneId, ExpectedSystemId);

        #endregion Cases
    }
}
