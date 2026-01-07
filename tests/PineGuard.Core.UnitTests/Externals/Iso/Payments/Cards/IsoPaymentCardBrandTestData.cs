using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class IsoPaymentCardBrandTestData
{
    public static class Constructor
    {
        private static InvalidCase I(string name, string? brandName, int[]? validPanLengths, string[]? iinPrefixes, int[]? displayFormatPattern, Type exceptionType, string? paramName) =>
            new(Name: name, BrandName: brandName, ValidPanLengths: validPanLengths, IinPrefixes: iinPrefixes, DisplayFormatPattern: displayFormatPattern, ExpectedException: new ExpectedException(exceptionType, paramName));

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("brand null", null, [16], ["4"], [4, 4, 4, 4], typeof(ArgumentNullException), "brandName") },
            { I("brand space", " ", [16], ["4"], [4, 4, 4, 4], typeof(ArgumentException), "brandName") },
            { I("lengths null", "X", null, ["4"], [4, 4, 4, 4], typeof(ArgumentNullException), "validPanLengths") },
            { I("lengths empty", "X", Array.Empty<int>(), ["4"], [4, 4, 4, 4], typeof(ArgumentException), "validPanLengths") },
            { I("lengths 0", "X", [0], ["4"], [4, 4, 4, 4], typeof(ArgumentOutOfRangeException), "validPanLengths") },
            { I("prefixes null", "X", [16], null, [4, 4, 4, 4], typeof(ArgumentNullException), "iinPrefixes") },
            { I("prefixes empty", "X", [16], Array.Empty<string>(), [4, 4, 4, 4], typeof(ArgumentException), "iinPrefixes") },
            { I("prefixes space", "X", [16], [" "], [4, 4, 4, 4], typeof(ArgumentException), "iinPrefixes") },
            { I("prefixes non-digit", "X", [16], ["4a"], [4, 4, 4, 4], typeof(ArgumentException), "iinPrefixes") },
            { I("pattern null", "X", [16], ["4"], null, typeof(ArgumentNullException), "displayFormatPattern") },
            { I("pattern empty", "X", [16], ["4"], Array.Empty<int>(), typeof(ArgumentException), "displayFormatPattern") },
            { I("pattern contains 0", "X", [16], ["4"], [4, 4, 0, 4], typeof(ArgumentOutOfRangeException), "displayFormatPattern") },
            { I("pattern sum mismatch", "X", [16], ["4"], [4, 4, 4], typeof(ArgumentException), "displayFormatPattern") },
        };

        #region Cases

        public record Case(string Name, string? BrandName, int[]? ValidPanLengths, string[]? IinPrefixes, int[]? DisplayFormatPattern);

        public record InvalidCase(string Name, string? BrandName, int[]? ValidPanLengths, string[]? IinPrefixes, int[]? DisplayFormatPattern, ExpectedException ExpectedException)
            : Case(Name, BrandName, ValidPanLengths, IinPrefixes, DisplayFormatPattern);

        #endregion
    }

    public static class IsoCardNumberWithSeparatorsRegex
    {
        private static ValidCase V(string name, string value, bool expected) => new(Name: name, Value: value, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("spaces", "1234 5678 9012 3456", expected: true) },
            { V("dashes", "1234-5678-9012-3456", expected: true) },
            { V("single digit", "0", expected: true) },
            { V("digit-dash-digit", "9-9", expected: true) },
            { V("space", " ", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("empty", "", expected: false) },
            { V("underscores", "1234_5678", expected: false) },
            { V("letters", "abcd", expected: false) },
        };

        #region Cases

        public record Case(string Name, string Value);

        public sealed record ValidCase(string Name, string Value, bool Expected)
            : Case(Name, Value);

        #endregion
    }

    public static class MatchesIinRange
    {
        private static ValidCase V(string name, string? sanitizedPan, bool expected) => new(Name: name, SanitizedPan: sanitizedPan, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("51", "5100000000000000", expected: true) },
            { V("55", "5500000000000000", expected: true) },
            { V("2221", "2221000000000000", expected: true) },
            { V("2720", "2720000000000000", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("empty", "", expected: false) },
            { V("length < 4", "123", expected: false) },
            { V("50", "5000000000000000", expected: false) },
            { V("56", "5600000000000000", expected: false) },
            { V("2220", "2220000000000000", expected: false) },
            { V("2721", "2721000000000000", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? SanitizedPan);

        public sealed record ValidCase(string Name, string? SanitizedPan, bool Expected)
            : Case(Name, SanitizedPan);

        #endregion
    }
}
