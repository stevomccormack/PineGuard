using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public static class IsoLanguageTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("English", alpha2Code: "en", alpha3Code: "eng", nameValue: "English", expectedAlpha2: "en", expectedAlpha3: "eng"),
            V("French", alpha2Code: "fr", alpha3Code: "fra", nameValue: "French", expectedAlpha2: "fr", expectedAlpha3: "fra"),
            V("Spanish", alpha2Code: "es", alpha3Code: "spa", nameValue: "Spanish", expectedAlpha2: "es", expectedAlpha3: "spa"),
            V("German", alpha2Code: "de", alpha3Code: "deu", nameValue: "German", expectedAlpha2: "de", expectedAlpha3: "deu"),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("alpha2 too short", alpha2Code: "e", alpha3Code: "eng", nameValue: "English", paramName: "alpha2Code"),
            I("alpha3 too short", alpha2Code: "en", alpha3Code: "en", nameValue: "English", paramName: "alpha3Code"),
            I("alpha3 non-alpha", alpha2Code: "en", alpha3Code: "en1", nameValue: "English", paramName: "alpha3Code"),
            I("alpha2 non-alpha", alpha2Code: "e1", alpha3Code: "eng", nameValue: "English", paramName: "alpha2Code"),
            I("name whitespace", alpha2Code: "en", alpha3Code: "eng", nameValue: " ", paramName: "name"),
        };

        private static ValidCase V(string name, string alpha2Code, string alpha3Code, string nameValue, string expectedAlpha2, string expectedAlpha3)
            => new(name, alpha2Code, alpha3Code, nameValue, expectedAlpha2, expectedAlpha3);

        private static InvalidCase I(string name, string alpha2Code, string alpha3Code, string nameValue, string paramName)
            => new(name, alpha2Code, alpha3Code, nameValue, ExpectedException: new ExpectedException(typeof(ArgumentException), ParamName: paramName));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Alpha2Code, string Alpha3Code, string NameValue, string ExpectedAlpha2, string ExpectedAlpha3) : Case(Name);

        public sealed record InvalidCase(string Name, string Alpha2Code, string Alpha3Code, string NameValue, ExpectedException ExpectedException) : Case(Name);

        #endregion
    }

    public static class TryParse
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("Alpha2", value: "en", expected: true, expectedAlpha2: "en"),
            V("Alpha3", value: "spa", expected: true, expectedAlpha2: "es"),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Trim alpha2", value: " EN ", expected: true, expectedAlpha2: "en"),
            V("Trim alpha3", value: "\tFRA\t", expected: true, expectedAlpha2: "fr"),
            V("Null", value: null, expected: false, expectedAlpha2: null),
            V("Empty", value: string.Empty, expected: false, expectedAlpha2: null),
            V("Space", value: " ", expected: false, expectedAlpha2: null),
            V("Tab", value: "\t", expected: false, expectedAlpha2: null),
            V("Not a code", value: "not-a-code", expected: false, expectedAlpha2: null),
        };

        private static Case V(string name, string? value, bool expected, string? expectedAlpha2) => new(name, value, expected, expectedAlpha2);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected, string? ExpectedAlpha2);

        #endregion
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Alpha2", value: "en", expectedAlpha2: "en"),
            V("Alpha3", value: "spa", expectedAlpha2: "es"),
            V("Trim alpha2", value: " EN ", expectedAlpha2: "en"),
            V("Trim alpha3", value: "\tFRA\t", expectedAlpha2: "fr"),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Null", value: null),
            I("Empty", value: string.Empty),
            I("Space", value: " "),
            I("Tab", value: "\t"),
            I("Not a code", value: "not-a-code"),
        };

        private static ValidCase V(string name, string value, string expectedAlpha2) => new(name, value, expectedAlpha2);

        private static InvalidCase I(string name, string? value)
            => new(name, value, ExpectedException: new ExpectedException(typeof(FormatException), MessageContains: "ISO"));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Value, string ExpectedAlpha2) : Case(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException) : Case(Name);

        #endregion
    }
}
