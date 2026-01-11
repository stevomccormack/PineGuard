using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class IsoPaymentCardBrandTestData
{
    public static class Constructor
    {
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new(
                "brand null",
                (BrandName: null, ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentNullException), "brandName")),
            new(
                "brand space",
                (BrandName: " ", ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "brandName")),
            new(
                "lengths null",
                (BrandName: "X", ValidPanLengths: null, IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentNullException), "validPanLengths")),
            new(
                "lengths empty",
                (BrandName: "X", ValidPanLengths: [], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "validPanLengths")),
            new(
                "lengths 0",
                (BrandName: "X", ValidPanLengths: [0], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentOutOfRangeException), "validPanLengths")),
            new(
                "prefixes null",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: null, DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentNullException), "iinPrefixes")),
            new(
                "prefixes empty",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: [], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "iinPrefixes")),
            new(
                "prefixes space",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: [" "], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "iinPrefixes")),
            new(
                "prefixes non-digit",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: ["4a"], DisplayFormatPattern: [4, 4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "iinPrefixes")),
            new(
                "pattern null",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: null),
                new ExpectedException(typeof(ArgumentNullException), "displayFormatPattern")),
            new(
                "pattern empty",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: []),
                new ExpectedException(typeof(ArgumentException), "displayFormatPattern")),
            new(
                "pattern contains 0",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 0, 4]),
                new ExpectedException(typeof(ArgumentOutOfRangeException), "displayFormatPattern")),
            new(
                "pattern sum mismatch",
                (BrandName: "X", ValidPanLengths: [16], IinPrefixes: ["4"], DisplayFormatPattern: [4, 4, 4]),
                new ExpectedException(typeof(ArgumentException), "displayFormatPattern")),
        ];

        #region Case Records

        public sealed record InvalidCase(
            string Name,
            (string? BrandName, int[]? ValidPanLengths, string[]? IinPrefixes, int[]? DisplayFormatPattern) Value,
            ExpectedException ExpectedException)
            : ThrowsCase<(string? BrandName, int[]? ValidPanLengths, string[]? IinPrefixes, int[]? DisplayFormatPattern)>(Name, Value, ExpectedException);

        #endregion
    }

    public static class IsoCardNumberWithSeparatorsRegex
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("spaces", "1234 5678 9012 3456", true),
            new("dashes", "1234-5678-9012-3456", true),
            new("single digit", "0", true),
            new("digit-dash-digit", "9-9", true),
            new("space", " ", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("empty", "", false),
            new("underscores", "1234_5678", false),
            new("letters", "abcd", false),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string Value, bool ExpectedReturn)
            : IsCase<string>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class MatchesIinRange
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("51", "5100000000000000", true),
            new("55", "5500000000000000", true),
            new("2221", "2221000000000000", true),
            new("2720", "2720000000000000", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("empty", "", false),
            new("length < 4", "123", false),
            new("50", "5000000000000000", false),
            new("56", "5600000000000000", false),
            new("2220", "2220000000000000", false),
            new("2721", "2721000000000000", false),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string? SanitizedPan, bool ExpectedReturn)
            : IsCase<string?>(Name, SanitizedPan, ExpectedReturn);

        #endregion
    }
}
