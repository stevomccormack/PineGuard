using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public static class IanaTimeZoneTestData
{
    public static class Constructor
    {
        private static ValidCase V(string name, string id, string[] countryAlpha2Codes, string coordinates, string? comment) =>
            new(
                Name: name,
                Id: id,
                CountryAlpha2Codes: countryAlpha2Codes,
                Coordinates: coordinates,
                Comment: comment,
                ExpectedId: id.Trim(),
                ExpectedCountryAlpha2Codes: Array.ConvertAll(countryAlpha2Codes, c => c.Trim().ToUpperInvariant()),
                ExpectedCoordinates: coordinates.Trim(),
                ExpectedComment: string.IsNullOrWhiteSpace(comment) ? null : comment.Trim());

        private static InvalidCase I(string name, string? id, string[]? countryAlpha2Codes, string? coordinates, string? comment, Type exceptionType, string? paramName) =>
            new(Name: name, Id: id, CountryAlpha2Codes: countryAlpha2Codes, Coordinates: coordinates, Comment: comment, ExpectedException: new ExpectedException(exceptionType, paramName));

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("trims + normalizes all fields", " Europe/London ", ["gb"], " +5130-00007 ", "  comment  ") },
            { V("no normalization needed", "America/New_York", ["US", "CA"], "+404251-0740023", null) },
            { V("comment empty becomes null", "Asia/Tokyo", [" jp "], " +3539+13944 ", "") },
            { V("multiple countries mixed casing", "Europe/Paris", ["fr", " Mc"], "+4852+00220", " Note ") },
            { V("lowercase id preserved (only trimmed)", "  europe/london  ", ["GB"], "+5130-00007", null) },
            { V("id with underscores", "America/Argentina/Buenos_Aires", ["ar"], "-3436-05827", null) },
            { V("Etc style id", "Etc/GMT+1", ["de"], "+0000+00000", "  ") },
            { V("country code trims each element", "Pacific/Auckland", [" nz ", "  ck"], "-3652+17446", null) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("minimal id accepted", "A", ["ZZ"], "0", null) },
            { V("comment whitespace becomes null", "Etc/UTC", ["  us  "], "-0000+00000", "\t\r\n") },
            { V("coordinates trimmed but not validated", "X", ["GB"], "   not-a-coordinate   ", " comment ") },
            { V("multiple countries include duplicates", "Europe/London", ["gb", "GB"], "+5130-00007", null) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("id null", null, ["US"], "+0000+00000", null, typeof(ArgumentNullException), "id") },
            { I("id whitespace", " ", ["US"], "+0000+00000", null, typeof(ArgumentException), "id") },
            { I("id tabs/newlines", "\r\n\t", ["US"], "+0000+00000", null, typeof(ArgumentException), "id") },

            { I("country list null", "Europe/London", null, "+0000+00000", null, typeof(ArgumentNullException), "countryAlpha2Codes") },
            { I("country list empty", "Europe/London", Array.Empty<string>(), "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country contains whitespace element", "Europe/London", ["  "], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country contains null element", "Europe/London", [(string)null!], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country code length 1", "Europe/London", ["U"], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country code length 3", "Europe/London", ["USA"], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country code not alphabetic", "Europe/London", ["U1"], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country code symbols", "Europe/London", ["1@"], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country code internal space", "Europe/London", ["U S"], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },

            { I("coordinates null", "Europe/London", ["US"], null, null, typeof(ArgumentNullException), "coordinates") },
            { I("coordinates whitespace", "Europe/London", ["US"], " ", null, typeof(ArgumentException), "coordinates") },
            { I("id tab", "\t", ["US"], "+0000+00000", null, typeof(ArgumentException), "id") },
            { I("coordinates newlines", "Europe/London", ["US"], "\r\n", null, typeof(ArgumentException), "coordinates") },
            { I("country trims to length 1", "Europe/London", [" U "], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
            { I("country trims to non-alpha", "Europe/London", [" 1@ "], "+0000+00000", null, typeof(ArgumentException), "countryAlpha2Codes") },
        };

        #region Cases

        public record Case(string Name, string? Id, string[]? CountryAlpha2Codes, string? Coordinates, string? Comment);

        public sealed record ValidCase(string Name, string Id, string[] CountryAlpha2Codes, string Coordinates, string? Comment, string ExpectedId, string[] ExpectedCountryAlpha2Codes, string ExpectedCoordinates, string? ExpectedComment)
            : Case(Name, Id, CountryAlpha2Codes, Coordinates, Comment);

        public record InvalidCase(string Name, string? Id, string[]? CountryAlpha2Codes, string? Coordinates, string? Comment, ExpectedException ExpectedException)
            : Case(Name, Id, CountryAlpha2Codes, Coordinates, Comment);

        #endregion
    }
}
