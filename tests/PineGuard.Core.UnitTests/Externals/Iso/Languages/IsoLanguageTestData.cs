using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public static class IsoLanguageTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("English", "en", "eng", "English", "en", "eng"),
            new("French", "fr", "fra", "French", "fr", "fra"),
            new("Spanish", "es", "spa", "Spanish", "es", "spa"),
            new("German", "de", "deu", "German", "de", "deu"),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("alpha2 too short", "e", "eng", "English", new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("alpha3 too short", "en", "en", "English", new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("alpha3 non-alpha", "en", "en1", "English", new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("alpha2 non-alpha", "e1", "eng", "English", new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("name whitespace", "en", "eng", " ", new ExpectedException(typeof(ArgumentException), "name")),
        ];

        public sealed record ValidCase(string Name, string Alpha2Code, string Alpha3Code, string NameValue, string ExpectedAlpha2, string ExpectedAlpha3)
            : ReturnCase<(string Alpha2Code, string Alpha3Code, string NameValue), (string ExpectedAlpha2, string ExpectedAlpha3)>(
                Name,
                (Alpha2Code, Alpha3Code, NameValue),
                (ExpectedAlpha2, ExpectedAlpha3));

        public sealed record InvalidCase(string Name, string Alpha2Code, string Alpha3Code, string NameValue, ExpectedException ExpectedException)
            : ThrowsCase<(string Alpha2Code, string Alpha3Code, string NameValue)>(Name, (Alpha2Code, Alpha3Code, NameValue), ExpectedException);
    }

    public static class TryParse
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Alpha2", "en", true, "en"),
            new("Alpha3", "spa", true, "es"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Trim alpha2", " EN ", true, "en"),
            new("Trim alpha3", "\tFRA\t", true, "fr"),
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Tab", "\t", false, null),
            new("Not a code", "not-a-code", false, null),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Alpha2", "en", "en"),
            new("Alpha3", "spa", "es"),
            new("Trim alpha2", " EN ", "en"),
            new("Trim alpha3", "\tFRA\t", "fr"),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Space", " ", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Tab", "\t", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Not a code", "not-a-code", new ExpectedException(typeof(FormatException), null, "ISO")),
        ];

        public sealed record ValidCase(string Name, string Value, string ExpectedAlpha2)
            : ReturnCase<string, string>(Name, Value, ExpectedAlpha2);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }
}
