using System.Globalization;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityNumberTypesTestData
{
    public static class TryParseInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("trimmed", " 123 ", true, 123),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, 0),
            new("space", " ", false, 0),
            new("not a number", "not", false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, int ExpectedInt32)
            : TryCase<string, int>(Name, Value, ExpectedSuccess, ExpectedInt32);

        #endregion Cases
    }

    public static class TryParseInt64
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("trimmed", " 123 ", true, 123L),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, 0),
            new("tab", "\t", false, 0),
            new("not a number", "not", false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, long ExpectedInt64)
            : TryCase<string, long>(Name, Value, ExpectedSuccess, ExpectedInt64);

        #endregion Cases
    }

    public static class TryParseDecimal
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("trimmed", " 1.23 ", null, true, 1.23m),
            new("fr-FR comma", "1,23", "fr-FR", true, 1.23m),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,23", null, false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, string? CultureName, bool ExpectedSuccess, decimal ExpectedDecimal)
            : TryCase<string, decimal>(Name, Value, ExpectedSuccess, ExpectedDecimal);

        #endregion Cases
    }

    public static class TryParseSingle
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("trimmed", " 1.5 ", null, true, 1.5f),
            new("fr-FR comma", "1,5", "fr-FR", true, 1.5f),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,5", null, false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, string? CultureName, bool ExpectedSuccess, float ExpectedSingle)
            : TryCase<string, float>(Name, Value, ExpectedSuccess, ExpectedSingle);

        #endregion Cases
    }

    public static class TryParseDouble
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("trimmed", " 1.5 ", null, true, 1.5d),
            new("fr-FR comma", "1,5", "fr-FR", true, 1.5d),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, null, false, 0),
            new("space", " ", null, false, 0),
            new("comma w/o culture", "1,5", null, false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, string? CultureName, bool ExpectedSuccess, double ExpectedDouble)
            : TryCase<string, double>(Name, Value, ExpectedSuccess, ExpectedDouble);

        #endregion Cases
    }

    public static IFormatProvider? GetProvider(string? cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
            return null;

        return CultureInfo.GetCultureInfo(cultureName);
    }
}
