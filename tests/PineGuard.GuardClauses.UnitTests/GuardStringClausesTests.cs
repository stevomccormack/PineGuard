using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardStringClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotNullOrEmpty.ValidCases), MemberType = typeof(TD.NotNullOrEmpty))]
    [MemberData(nameof(TD.NotNullOrEmpty.InvalidCases), MemberType = typeof(TD.NotNullOrEmpty))]
    public void NotNullOrEmpty_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNullOrEmpty(value));
        AssertCustomMessage(tc, () => Guard.Against.NotNullOrEmpty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NullOrEmpty.ValidCases), MemberType = typeof(TD.NullOrEmpty))]
    [MemberData(nameof(TD.NullOrEmpty.InvalidCases), MemberType = typeof(TD.NullOrEmpty))]
    public void NullOrEmpty_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NullOrEmpty(value));
        AssertCustomMessage(tc, () => Guard.Against.NullOrEmpty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotNullOrWhiteSpace.ValidCases), MemberType = typeof(TD.NotNullOrWhiteSpace))]
    [MemberData(nameof(TD.NotNullOrWhiteSpace.InvalidCases), MemberType = typeof(TD.NotNullOrWhiteSpace))]
    public void NotNullOrWhiteSpace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNullOrWhiteSpace(value));
        AssertCustomMessage(tc, () => Guard.Against.NotNullOrWhiteSpace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NullOrWhiteSpace.ValidCases), MemberType = typeof(TD.NullOrWhiteSpace))]
    [MemberData(nameof(TD.NullOrWhiteSpace.InvalidCases), MemberType = typeof(TD.NullOrWhiteSpace))]
    public void NullOrWhiteSpace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NullOrWhiteSpace(value));
        AssertCustomMessage(tc, () => Guard.Against.NullOrWhiteSpace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotExactLength.ValidCases), MemberType = typeof(TD.NotExactLength))]
    [MemberData(nameof(TD.NotExactLength.InvalidCases), MemberType = typeof(TD.NotExactLength))]
    public void NotExactLength_BehavesAsExpected(GuardCase<(string? value, int length)> tc)
    {
        var value = tc.Value.value;
        var length = tc.Value.length;
        var result = AssertResult(tc, () => Guard.Against.NotExactLength(value, length));
        AssertCustomMessage(tc, () => Guard.Against.NotExactLength(value, length, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotLengthBetween.ValidCases), MemberType = typeof(TD.NotLengthBetween))]
    [MemberData(nameof(TD.NotLengthBetween.InvalidCases), MemberType = typeof(TD.NotLengthBetween))]
    public void NotLengthBetween_BehavesAsExpected(GuardCase<(string? value, int min, int max)> tc)
    {
        var value = tc.Value.value;
        var min = tc.Value.min;
        var max = tc.Value.max;
        var result = AssertResult(tc, () => Guard.Against.NotLengthBetween(value, min, max));
        AssertCustomMessage(tc, () => Guard.Against.NotLengthBetween(value, min, max, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ShorterThanOrEqual.ValidCases), MemberType = typeof(TD.ShorterThanOrEqual))]
    [MemberData(nameof(TD.ShorterThanOrEqual.InvalidCases), MemberType = typeof(TD.ShorterThanOrEqual))]
    public void ShorterThanOrEqual_BehavesAsExpected(GuardCase<(string? value, int length)> tc)
    {
        var value = tc.Value.value;
        var length = tc.Value.length;
        var result = AssertResult(tc, () => Guard.Against.ShorterThanOrEqual(value, length));
        AssertCustomMessage(tc, () => Guard.Against.ShorterThanOrEqual(value, length, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ShorterThan.ValidCases), MemberType = typeof(TD.ShorterThan))]
    [MemberData(nameof(TD.ShorterThan.InvalidCases), MemberType = typeof(TD.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(GuardCase<(string? value, int length)> tc)
    {
        var value = tc.Value.value;
        var length = tc.Value.length;
        var result = AssertResult(tc, () => Guard.Against.ShorterThan(value, length));
        AssertCustomMessage(tc, () => Guard.Against.ShorterThan(value, length, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.LongerThanOrEqual.ValidCases), MemberType = typeof(TD.LongerThanOrEqual))]
    [MemberData(nameof(TD.LongerThanOrEqual.InvalidCases), MemberType = typeof(TD.LongerThanOrEqual))]
    public void LongerThanOrEqual_BehavesAsExpected(GuardCase<(string? value, int length)> tc)
    {
        var value = tc.Value.value;
        var length = tc.Value.length;
        var result = AssertResult(tc, () => Guard.Against.LongerThanOrEqual(value, length));
        AssertCustomMessage(tc, () => Guard.Against.LongerThanOrEqual(value, length, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.LongerThan.ValidCases), MemberType = typeof(TD.LongerThan))]
    [MemberData(nameof(TD.LongerThan.InvalidCases), MemberType = typeof(TD.LongerThan))]
    public void LongerThan_BehavesAsExpected(GuardCase<(string? value, int length)> tc)
    {
        var value = tc.Value.value;
        var length = tc.Value.length;
        var result = AssertResult(tc, () => Guard.Against.LongerThan(value, length));
        AssertCustomMessage(tc, () => Guard.Against.LongerThan(value, length, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotMatch.ValidCases), MemberType = typeof(TD.NotMatch))]
    [MemberData(nameof(TD.NotMatch.InvalidCases), MemberType = typeof(TD.NotMatch))]
    public void NotMatch_BehavesAsExpected(GuardCase<(string? value, System.Text.RegularExpressions.Regex pattern)> tc)
    {
        var value = tc.Value.value;
        var pattern = tc.Value.pattern;
        var result = AssertResult(tc, () => Guard.Against.NotMatch(value, pattern));
        AssertCustomMessage(tc, () => Guard.Against.NotMatch(value, pattern, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Match.ValidCases), MemberType = typeof(TD.Match))]
    [MemberData(nameof(TD.Match.InvalidCases), MemberType = typeof(TD.Match))]
    public void Match_BehavesAsExpected(GuardCase<(string? value, System.Text.RegularExpressions.Regex pattern)> tc)
    {
        var value = tc.Value.value;
        var pattern = tc.Value.pattern;
        var result = AssertResult(tc, () => Guard.Against.Match(value, pattern));
        AssertCustomMessage(tc, () => Guard.Against.Match(value, pattern, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotAlphabetic.ValidCases), MemberType = typeof(TD.NotAlphabetic))]
    [MemberData(nameof(TD.NotAlphabetic.InvalidCases), MemberType = typeof(TD.NotAlphabetic))]
    public void NotAlphabetic_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotAlphabetic(value));
        AssertCustomMessage(tc, () => Guard.Against.NotAlphabetic(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotNumeric.ValidCases), MemberType = typeof(TD.NotNumeric))]
    [MemberData(nameof(TD.NotNumeric.InvalidCases), MemberType = typeof(TD.NotNumeric))]
    public void NotNumeric_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotNumeric(value));
        AssertCustomMessage(tc, () => Guard.Against.NotNumeric(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotAlphanumeric.ValidCases), MemberType = typeof(TD.NotAlphanumeric))]
    [MemberData(nameof(TD.NotAlphanumeric.InvalidCases), MemberType = typeof(TD.NotAlphanumeric))]
    public void NotAlphanumeric_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotAlphanumeric(value));
        AssertCustomMessage(tc, () => Guard.Against.NotAlphanumeric(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotDigitsOnly.ValidCases), MemberType = typeof(TD.NotDigitsOnly))]
    [MemberData(nameof(TD.NotDigitsOnly.InvalidCases), MemberType = typeof(TD.NotDigitsOnly))]
    public void NotDigitsOnly_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDigitsOnly(value));
        AssertCustomMessage(tc, () => Guard.Against.NotDigitsOnly(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotDigitsOnlyWithAllowed.ValidCases), MemberType = typeof(TD.NotDigitsOnlyWithAllowed))]
    [MemberData(nameof(TD.NotDigitsOnlyWithAllowed.InvalidCases), MemberType = typeof(TD.NotDigitsOnlyWithAllowed))]
    public void NotDigitsOnlyWithAllowed_BehavesAsExpected(GuardCase<(string value, char[] allowedNonDigitChars)> tc)
    {
        var value = tc.Value.value;
        var allowedNonDigitChars = tc.Value.allowedNonDigitChars;
        var result = AssertResult(tc, () => Guard.Against.NotDigitsOnly(value, allowedNonDigitChars));
        AssertCustomMessage(tc, () => Guard.Against.NotDigitsOnly(value, allowedNonDigitChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Alphabetic.ValidCases), MemberType = typeof(TD.Alphabetic))]
    [MemberData(nameof(TD.Alphabetic.InvalidCases), MemberType = typeof(TD.Alphabetic))]
    public void Alphabetic_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Alphabetic(value));
        AssertCustomMessage(tc, () => Guard.Against.Alphabetic(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Numeric.ValidCases), MemberType = typeof(TD.Numeric))]
    [MemberData(nameof(TD.Numeric.InvalidCases), MemberType = typeof(TD.Numeric))]
    public void Numeric_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Numeric(value));
        AssertCustomMessage(tc, () => Guard.Against.Numeric(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Alphanumeric.ValidCases), MemberType = typeof(TD.Alphanumeric))]
    [MemberData(nameof(TD.Alphanumeric.InvalidCases), MemberType = typeof(TD.Alphanumeric))]
    public void Alphanumeric_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Alphanumeric(value));
        AssertCustomMessage(tc, () => Guard.Against.Alphanumeric(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.DigitsOnly.ValidCases), MemberType = typeof(TD.DigitsOnly))]
    [MemberData(nameof(TD.DigitsOnly.InvalidCases), MemberType = typeof(TD.DigitsOnly))]
    public void DigitsOnly_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.DigitsOnly(value));
        AssertCustomMessage(tc, () => Guard.Against.DigitsOnly(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.DigitsOnlyWithAllowed.ValidCases), MemberType = typeof(TD.DigitsOnlyWithAllowed))]
    [MemberData(nameof(TD.DigitsOnlyWithAllowed.InvalidCases), MemberType = typeof(TD.DigitsOnlyWithAllowed))]
    public void DigitsOnlyWithAllowed_BehavesAsExpected(GuardCase<(string? value, char[] allowedNonDigitChars)> tc)
    {
        var value = tc.Value.value;
        var allowedNonDigitChars = tc.Value.allowedNonDigitChars;
        var result = AssertResult(tc, () => Guard.Against.DigitsOnly(value, allowedNonDigitChars));
        AssertCustomMessage(tc, () => Guard.Against.DigitsOnly(value, allowedNonDigitChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Uppercase.ValidCases), MemberType = typeof(TD.Uppercase))]
    [MemberData(nameof(TD.Uppercase.InvalidCases), MemberType = typeof(TD.Uppercase))]
    public void Uppercase_BehavesAsExpected(GuardCase<(string? value, bool lettersOnly)> tc)
    {
        var value = tc.Value.value;
        var lettersOnly = tc.Value.lettersOnly;
        var result = AssertResult(tc, () => Guard.Against.Uppercase(value, lettersOnly));
        AssertCustomMessage(tc, () => Guard.Against.Uppercase(value, lettersOnly, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Lowercase.ValidCases), MemberType = typeof(TD.Lowercase))]
    [MemberData(nameof(TD.Lowercase.InvalidCases), MemberType = typeof(TD.Lowercase))]
    public void Lowercase_BehavesAsExpected(GuardCase<(string? value, bool lettersOnly)> tc)
    {
        var value = tc.Value.value;
        var lettersOnly = tc.Value.lettersOnly;
        var result = AssertResult(tc, () => Guard.Against.Lowercase(value, lettersOnly));
        AssertCustomMessage(tc, () => Guard.Against.Lowercase(value, lettersOnly, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotAscii.ValidCases), MemberType = typeof(TD.NotAscii))]
    [MemberData(nameof(TD.NotAscii.InvalidCases), MemberType = typeof(TD.NotAscii))]
    public void NotAscii_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotAscii(value));
        AssertCustomMessage(tc, () => Guard.Against.NotAscii(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Ascii.ValidCases), MemberType = typeof(TD.Ascii))]
    [MemberData(nameof(TD.Ascii.InvalidCases), MemberType = typeof(TD.Ascii))]
    public void Ascii_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Ascii(value));
        AssertCustomMessage(tc, () => Guard.Against.Ascii(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPrintableAscii.ValidCases), MemberType = typeof(TD.NotPrintableAscii))]
    [MemberData(nameof(TD.NotPrintableAscii.InvalidCases), MemberType = typeof(TD.NotPrintableAscii))]
    public void NotPrintableAscii_BehavesAsExpected(GuardCase<(string? value, bool allowCommonWhitespace)> tc)
    {
        var value = tc.Value.value;
        var allowCommonWhitespace = tc.Value.allowCommonWhitespace;
        var result = AssertResult(tc, () => Guard.Against.NotPrintableAscii(value, allowCommonWhitespace));
        AssertCustomMessage(tc, () => Guard.Against.NotPrintableAscii(value, allowCommonWhitespace, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.PrintableAscii.ValidCases), MemberType = typeof(TD.PrintableAscii))]
    [MemberData(nameof(TD.PrintableAscii.InvalidCases), MemberType = typeof(TD.PrintableAscii))]
    public void PrintableAscii_BehavesAsExpected(GuardCase<(string? value, bool allowCommonWhitespace)> tc)
    {
        var value = tc.Value.value;
        var allowCommonWhitespace = tc.Value.allowCommonWhitespace;
        var result = AssertResult(tc, () => Guard.Against.PrintableAscii(value, allowCommonWhitespace));
        AssertCustomMessage(tc, () => Guard.Against.PrintableAscii(value, allowCommonWhitespace, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.IsWhitespace.ValidCases), MemberType = typeof(TD.IsWhitespace))]
    [MemberData(nameof(TD.IsWhitespace.InvalidCases), MemberType = typeof(TD.IsWhitespace))]
    public void IsWhitespace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.IsWhitespace(value));
        AssertCustomMessage(tc, () => Guard.Against.IsWhitespace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Whitespace.ValidCases), MemberType = typeof(TD.Whitespace))]
    [MemberData(nameof(TD.Whitespace.InvalidCases), MemberType = typeof(TD.Whitespace))]
    public void Whitespace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Whitespace(value));
        AssertCustomMessage(tc, () => Guard.Against.Whitespace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContainsWhitespace.ValidCases), MemberType = typeof(TD.NotContainsWhitespace))]
    [MemberData(nameof(TD.NotContainsWhitespace.InvalidCases), MemberType = typeof(TD.NotContainsWhitespace))]
    public void NotContainsWhitespace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotContainsWhitespace(value));
        AssertCustomMessage(tc, () => Guard.Against.NotContainsWhitespace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContainsWhitespace.ValidCases), MemberType = typeof(TD.ContainsWhitespace))]
    [MemberData(nameof(TD.ContainsWhitespace.InvalidCases), MemberType = typeof(TD.ContainsWhitespace))]
    public void ContainsWhitespace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ContainsWhitespace(value));
        AssertCustomMessage(tc, () => Guard.Against.ContainsWhitespace(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContainsControlChars.ValidCases), MemberType = typeof(TD.NotContainsControlChars))]
    [MemberData(nameof(TD.NotContainsControlChars.InvalidCases), MemberType = typeof(TD.NotContainsControlChars))]
    public void NotContainsControlChars_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotContainsControlChars(value));
        AssertCustomMessage(tc, () => Guard.Against.NotContainsControlChars(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContainsControlChars.ValidCases), MemberType = typeof(TD.ContainsControlChars))]
    [MemberData(nameof(TD.ContainsControlChars.InvalidCases), MemberType = typeof(TD.ContainsControlChars))]
    public void ContainsControlChars_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ContainsControlChars(value));
        AssertCustomMessage(tc, () => Guard.Against.ContainsControlChars(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContainsAllowedOnly.ValidCases), MemberType = typeof(TD.NotContainsAllowedOnly))]
    [MemberData(nameof(TD.NotContainsAllowedOnly.InvalidCases), MemberType = typeof(TD.NotContainsAllowedOnly))]
    public void NotContainsAllowedOnly_BehavesAsExpected(GuardCase<(string? value, char[] allowedChars)> tc)
    {
        var value = tc.Value.value;
        var allowedChars = tc.Value.allowedChars;
        var result = AssertResult(tc, () => Guard.Against.NotContainsAllowedOnly(value, allowedChars));
        AssertCustomMessage(tc, () => Guard.Against.NotContainsAllowedOnly(value, allowedChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContainsAllowedOnly.ValidCases), MemberType = typeof(TD.ContainsAllowedOnly))]
    [MemberData(nameof(TD.ContainsAllowedOnly.InvalidCases), MemberType = typeof(TD.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_BehavesAsExpected(GuardCase<(string? value, char[] allowedChars)> tc)
    {
        var value = tc.Value.value;
        var allowedChars = tc.Value.allowedChars;
        var result = AssertResult(tc, () => Guard.Against.ContainsAllowedOnly(value, allowedChars));
        AssertCustomMessage(tc, () => Guard.Against.ContainsAllowedOnly(value, allowedChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContainsDisallowed.ValidCases), MemberType = typeof(TD.ContainsDisallowed))]
    [MemberData(nameof(TD.ContainsDisallowed.InvalidCases), MemberType = typeof(TD.ContainsDisallowed))]
    public void ContainsDisallowed_BehavesAsExpected(GuardCase<(string? value, char[] disallowedChars)> tc)
    {
        var value = tc.Value.value;
        var disallowedChars = tc.Value.disallowedChars;
        var result = AssertResult(tc, () => Guard.Against.ContainsDisallowed(value, disallowedChars));
        AssertCustomMessage(tc, () => Guard.Against.ContainsDisallowed(value, disallowedChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContainsAny.ValidCases), MemberType = typeof(TD.NotContainsAny))]
    [MemberData(nameof(TD.NotContainsAny.InvalidCases), MemberType = typeof(TD.NotContainsAny))]
    public void NotContainsAny_BehavesAsExpected(GuardCase<(string? value, char[] characters)> tc)
    {
        var value = tc.Value.value;
        var characters = tc.Value.characters;
        var result = AssertResult(tc, () => Guard.Against.NotContainsAny(value, characters));
        AssertCustomMessage(tc, () => Guard.Against.NotContainsAny(value, characters, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContainsDisallowed.ValidCases), MemberType = typeof(TD.NotContainsDisallowed))]
    [MemberData(nameof(TD.NotContainsDisallowed.InvalidCases), MemberType = typeof(TD.NotContainsDisallowed))]
    public void NotContainsDisallowed_BehavesAsExpected(GuardCase<(string? value, char[] disallowedChars)> tc)
    {
        var value = tc.Value.value;
        var disallowedChars = tc.Value.disallowedChars;
        var result = AssertResult(tc, () => Guard.Against.NotContainsDisallowed(value, disallowedChars));
        AssertCustomMessage(tc, () => Guard.Against.NotContainsDisallowed(value, disallowedChars, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
