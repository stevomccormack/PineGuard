using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public static class IanaTimeZoneTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trims + normalizes all fields", " Europe/London ", ["gb"], " +5130-00007 ", "  comment  ", "Europe/London", ["GB"], "+5130-00007", "comment"),
            new("no normalization needed", "America/New_York", ["US", "CA"], "+404251-0740023", null, "America/New_York", ["US", "CA"], "+404251-0740023", null),
            new("comment empty becomes null", "Asia/Tokyo", [" jp "], " +3539+13944 ", "", "Asia/Tokyo", ["JP"], "+3539+13944", null),
            new("multiple countries mixed casing", "Europe/Paris", ["fr", " Mc"], "+4852+00220", " Note ", "Europe/Paris", ["FR", "MC"], "+4852+00220", "Note"),
            new("lowercase id preserved (only trimmed)", "  europe/london  ", ["GB"], "+5130-00007", null, "europe/london", ["GB"], "+5130-00007", null),
            new("id with underscores", "America/Argentina/Buenos_Aires", ["ar"], "-3436-05827", null, "America/Argentina/Buenos_Aires", ["AR"], "-3436-05827", null),
            new("Etc style id", "Etc/GMT+1", ["de"], "+0000+00000", "  ", "Etc/GMT+1", ["DE"], "+0000+00000", null),
            new("country code trims each element", "Pacific/Auckland", [" nz ", "  ck"], "-3652+17446", null, "Pacific/Auckland", ["NZ", "CK"], "-3652+17446", null),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("minimal id accepted", "A", ["ZZ"], "0", null, "A", ["ZZ"], "0", null),
            new("comment whitespace becomes null", "Etc/UTC", ["  us  "], "-0000+00000", "\t\r\n", "Etc/UTC", ["US"], "-0000+00000", null),
            new("coordinates trimmed but not validated", "X", ["GB"], "   not-a-coordinate   ", " comment ", "X", ["GB"], "not-a-coordinate", "comment"),
            new("multiple countries include duplicates", "Europe/London", ["gb", "GB"], "+5130-00007", null, "Europe/London", ["GB", "GB"], "+5130-00007", null),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("id null", null, ["US"], "+0000+00000", null, new ExpectedException(typeof(ArgumentNullException), "id")),
            new("id whitespace", " ", ["US"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "id")),
            new("id tabs/newlines", "\r\n\t", ["US"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "id")),

            new("country list null", "Europe/London", null, "+0000+00000", null, new ExpectedException(typeof(ArgumentNullException), "countryAlpha2Codes")),
            new("country list empty", "Europe/London", [], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country contains whitespace element", "Europe/London", ["  "], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country contains null element", "Europe/London", [null!], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country code length 1", "Europe/London", ["U"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country code length 3", "Europe/London", ["USA"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country code not alphabetic", "Europe/London", ["U1"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country code symbols", "Europe/London", ["1@"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country code internal space", "Europe/London", ["U S"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),

            new("coordinates null", "Europe/London", ["US"], null, null, new ExpectedException(typeof(ArgumentNullException), "coordinates")),
            new("coordinates whitespace", "Europe/London", ["US"], " ", null, new ExpectedException(typeof(ArgumentException), "coordinates")),
            new("id tab", "\t", ["US"], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "id")),
            new("coordinates newlines", "Europe/London", ["US"], "\r\n", null, new ExpectedException(typeof(ArgumentException), "coordinates")),
            new("country trims to length 1", "Europe/London", [" U "], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
            new("country trims to non-alpha", "Europe/London", [" 1@ "], "+0000+00000", null, new ExpectedException(typeof(ArgumentException), "countryAlpha2Codes")),
        ];

        public sealed record ValidCase(
            string Name,
            string Id,
            string[] CountryAlpha2Codes,
            string Coordinates,
            string? Comment,
            string ExpectedId,
            string[] ExpectedCountryAlpha2Codes,
            string ExpectedCoordinates,
            string? ExpectedComment)
            : ReturnCase<(string Id, string[] CountryAlpha2Codes, string Coordinates, string? Comment), (string ExpectedId, string[] ExpectedCountryAlpha2Codes, string ExpectedCoordinates, string? ExpectedComment)>(
                Name,
                (Id, CountryAlpha2Codes, Coordinates, Comment),
                (ExpectedId, ExpectedCountryAlpha2Codes, ExpectedCoordinates, ExpectedComment));

        public sealed record InvalidCase(
            string Name,
            string? Id,
            string[]? CountryAlpha2Codes,
            string? Coordinates,
            string? Comment,
            ExpectedException ExpectedException)
            : ThrowsCase<(string? Id, string[]? CountryAlpha2Codes, string? Coordinates, string? Comment)>(Name, (Id, CountryAlpha2Codes, Coordinates, Comment), ExpectedException);
    }
}
