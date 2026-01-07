using System.Globalization;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityNumberTypesTestData
{
    public static class TryParseInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("trimmed", " 123 ", true, 123) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false, default) },
            { new Case("space", " ", false, default) },
            { new Case("not a number", "not", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, int ExpectedInt32)
            : TryCase<string, int>(Name, Input, ExpectedSuccess, ExpectedInt32);

        #endregion Cases
    }

    public static class TryParseInt64
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("trimmed", " 123 ", true, 123L) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false, default) },
            { new Case("tab", "\t", false, default) },
            { new Case("not a number", "not", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, long ExpectedInt64)
            : TryCase<string, long>(Name, Input, ExpectedSuccess, ExpectedInt64);

        #endregion Cases
    }

    public static class TryParseDecimal
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("trimmed", " 1.23 ", null, true, 1.23m) },
            { new Case("fr-FR comma", "1,23", "fr-FR", true, 1.23m) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, null, false, default) },
            { new Case("space", " ", null, false, default) },
            { new Case("comma w/o culture", "1,23", null, false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, string? CultureName, bool ExpectedSuccess, decimal ExpectedDecimal)
            : TryCase<string, decimal>(Name, Input, ExpectedSuccess, ExpectedDecimal);

        #endregion Cases
    }

    public static class TryParseSingle
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("trimmed", " 1.5 ", null, true, 1.5f) },
            { new Case("fr-FR comma", "1,5", "fr-FR", true, 1.5f) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, null, false, default) },
            { new Case("space", " ", null, false, default) },
            { new Case("comma w/o culture", "1,5", null, false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, string? CultureName, bool ExpectedSuccess, float ExpectedSingle)
            : TryCase<string, float>(Name, Input, ExpectedSuccess, ExpectedSingle);

        #endregion Cases
    }

    public static class TryParseDouble
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("trimmed", " 1.5 ", null, true, 1.5d) },
            { new Case("fr-FR comma", "1,5", "fr-FR", true, 1.5d) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, null, false, default) },
            { new Case("space", " ", null, false, default) },
            { new Case("comma w/o culture", "1,5", null, false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, string? CultureName, bool ExpectedSuccess, double ExpectedDouble)
            : TryCase<string, double>(Name, Input, ExpectedSuccess, ExpectedDouble);

        #endregion Cases
    }

    public static IFormatProvider? GetProvider(string? cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
            return null;

        return CultureInfo.GetCultureInfo(cultureName);
    }
}
