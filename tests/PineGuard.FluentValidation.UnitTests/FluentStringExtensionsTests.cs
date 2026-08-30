using System.Text.RegularExpressions;
using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record StringModel { public string? Value { get; init; } }

    private sealed class NotNullOrEmptyValidator : AbstractValidator<StringModel>
    {
        public NotNullOrEmptyValidator() => RuleFor(x => x.Value).NotNullOrEmpty();
    }

    private sealed class NullOrEmptyValidator : AbstractValidator<StringModel>
    {
        public NullOrEmptyValidator() => RuleFor(x => x.Value).NullOrEmpty();
    }

    private sealed class NotNullOrWhiteSpaceValidator : AbstractValidator<StringModel>
    {
        public NotNullOrWhiteSpaceValidator() => RuleFor(x => x.Value).NotNullOrWhiteSpace();
    }

    private sealed class NullOrWhiteSpaceValidator : AbstractValidator<StringModel>
    {
        public NullOrWhiteSpaceValidator() => RuleFor(x => x.Value).NullOrWhiteSpace();
    }

    private sealed class ExactLengthValidator : AbstractValidator<StringModel>
    {
        public ExactLengthValidator(int length) => RuleFor(x => x.Value).ExactLength(length);
    }

    private sealed class LengthBetweenValidator : AbstractValidator<StringModel>
    {
        public LengthBetweenValidator(int min, int max) => RuleFor(x => x.Value).LengthBetween(min, max);
    }

    private sealed class LongerThanValidator : AbstractValidator<StringModel>
    {
        public LongerThanValidator(int length) => RuleFor(x => x.Value).LongerThan(length);
    }

    private sealed class LongerThanOrEqualValidator : AbstractValidator<StringModel>
    {
        public LongerThanOrEqualValidator(int length) => RuleFor(x => x.Value).LongerThanOrEqual(length);
    }

    private sealed class ShorterThanValidator : AbstractValidator<StringModel>
    {
        public ShorterThanValidator(int length) => RuleFor(x => x.Value).ShorterThan(length);
    }

    private sealed class ShorterThanOrEqualValidator : AbstractValidator<StringModel>
    {
        public ShorterThanOrEqualValidator(int length) => RuleFor(x => x.Value).ShorterThanOrEqual(length);
    }

    private sealed class DigitsOnlyValidator : AbstractValidator<StringModel>
    {
        public DigitsOnlyValidator() => RuleFor(x => x.Value).DigitsOnly();
    }

    private sealed class NotDigitsOnlyValidator : AbstractValidator<StringModel>
    {
        public NotDigitsOnlyValidator() => RuleFor(x => x.Value).NotDigitsOnly();
    }

    private sealed class DigitsOnlyWithAllowedValidator : AbstractValidator<StringModel>
    {
        public DigitsOnlyWithAllowedValidator(char[] allowedNonDigitChars) => RuleFor(x => x.Value).DigitsOnly(allowedNonDigitChars);
    }

    private sealed class NotDigitsOnlyWithAllowedValidator : AbstractValidator<StringModel>
    {
        public NotDigitsOnlyWithAllowedValidator(char[] allowedNonDigitChars) => RuleFor(x => x.Value).NotDigitsOnly(allowedNonDigitChars);
    }

    private sealed class UppercaseValidator : AbstractValidator<StringModel>
    {
        public UppercaseValidator(bool lettersOnly) => RuleFor(x => x.Value).Uppercase(lettersOnly);
    }

    private sealed class NotUppercaseValidator : AbstractValidator<StringModel>
    {
        public NotUppercaseValidator(bool lettersOnly) => RuleFor(x => x.Value).NotUppercase(lettersOnly);
    }

    private sealed class LowercaseValidator : AbstractValidator<StringModel>
    {
        public LowercaseValidator(bool lettersOnly) => RuleFor(x => x.Value).Lowercase(lettersOnly);
    }

    private sealed class NotLowercaseValidator : AbstractValidator<StringModel>
    {
        public NotLowercaseValidator(bool lettersOnly) => RuleFor(x => x.Value).NotLowercase(lettersOnly);
    }

    private sealed class AlphabeticValidator : AbstractValidator<StringModel>
    {
        public AlphabeticValidator() => RuleFor(x => x.Value).Alphabetic();
    }

    private sealed class NotAlphabeticValidator : AbstractValidator<StringModel>
    {
        public NotAlphabeticValidator() => RuleFor(x => x.Value).NotAlphabetic();
    }

    private sealed class NumericValidator : AbstractValidator<StringModel>
    {
        public NumericValidator() => RuleFor(x => x.Value).Numeric();
    }

    private sealed class NotNumericValidator : AbstractValidator<StringModel>
    {
        public NotNumericValidator() => RuleFor(x => x.Value).NotNumeric();
    }

    private sealed class AlphanumericValidator : AbstractValidator<StringModel>
    {
        public AlphanumericValidator() => RuleFor(x => x.Value).Alphanumeric();
    }

    private sealed class NotAlphanumericValidator : AbstractValidator<StringModel>
    {
        public NotAlphanumericValidator() => RuleFor(x => x.Value).NotAlphanumeric();
    }

    private sealed class ContainsAnyValidator : AbstractValidator<StringModel>
    {
        public ContainsAnyValidator(char[] anyOf) => RuleFor(x => x.Value).ContainsAny(anyOf);
    }

    private sealed class AsciiValidator : AbstractValidator<StringModel>
    {
        public AsciiValidator() => RuleFor(x => x.Value).Ascii();
    }

    private sealed class NotAsciiValidator : AbstractValidator<StringModel>
    {
        public NotAsciiValidator() => RuleFor(x => x.Value).NotAscii();
    }

    private sealed class MatchValidator : AbstractValidator<StringModel>
    {
        public MatchValidator(Regex pattern) => RuleFor(x => x.Value).Match(pattern);
    }

    private sealed class NotMatchValidator : AbstractValidator<StringModel>
    {
        public NotMatchValidator(Regex pattern) => RuleFor(x => x.Value).NotMatch(pattern);
    }

    private sealed class RegexPatternValidator : AbstractValidator<StringModel>
    {
        public RegexPatternValidator() => RuleFor(x => x.Value).RegexPattern();
    }

    private sealed class NotWhitespaceValidator : AbstractValidator<StringModel>
    {
        public NotWhitespaceValidator() => RuleFor(x => x.Value).NotWhitespace();
    }

    private sealed class ContainsWhitespaceValidator : AbstractValidator<StringModel>
    {
        public ContainsWhitespaceValidator() => RuleFor(x => x.Value).ContainsWhitespace();
    }

    private sealed class NotContainsWhitespaceValidator : AbstractValidator<StringModel>
    {
        public NotContainsWhitespaceValidator() => RuleFor(x => x.Value).NotContainsWhitespace();
    }

    private sealed class ContainsControlCharsValidator : AbstractValidator<StringModel>
    {
        public ContainsControlCharsValidator() => RuleFor(x => x.Value).ContainsControlChars();
    }

    private sealed class NotContainsControlCharsValidator : AbstractValidator<StringModel>
    {
        public NotContainsControlCharsValidator() => RuleFor(x => x.Value).NotContainsControlChars();
    }

    private sealed class PrintableAsciiValidator : AbstractValidator<StringModel>
    {
        public PrintableAsciiValidator(bool allowCommonWhitespace) => RuleFor(x => x.Value).PrintableAscii(allowCommonWhitespace);
    }

    private sealed class NotPrintableAsciiValidator : AbstractValidator<StringModel>
    {
        public NotPrintableAsciiValidator() => RuleFor(x => x.Value).NotPrintableAscii();
    }

    private sealed class ContainsAllowedOnlyValidator : AbstractValidator<StringModel>
    {
        public ContainsAllowedOnlyValidator(char[] allowedChars) => RuleFor(x => x.Value).ContainsAllowedOnly(allowedChars);
    }

    private sealed class NotContainsAllowedOnlyValidator : AbstractValidator<StringModel>
    {
        public NotContainsAllowedOnlyValidator(char[] allowedChars) => RuleFor(x => x.Value).NotContainsAllowedOnly(allowedChars);
    }

    private sealed class ContainsDisallowedValidator : AbstractValidator<StringModel>
    {
        public ContainsDisallowedValidator(char[] disallowedChars) => RuleFor(x => x.Value).ContainsDisallowed(disallowedChars);
    }

    private sealed class NotContainsDisallowedValidator : AbstractValidator<StringModel>
    {
        public NotContainsDisallowedValidator(char[] disallowedChars) => RuleFor(x => x.Value).NotContainsDisallowed(disallowedChars);
    }

    private sealed class ContainsValidator : AbstractValidator<StringModel>
    {
        public ContainsValidator(string substring, StringComparison comparison) => RuleFor(x => x.Value).Contains(substring, comparison);
    }

    private sealed class NotContainsValidator : AbstractValidator<StringModel>
    {
        public NotContainsValidator(string substring, StringComparison comparison) => RuleFor(x => x.Value).NotContains(substring, comparison);
    }

    private sealed class StartsWithValidator : AbstractValidator<StringModel>
    {
        public StartsWithValidator(string prefix, StringComparison comparison) => RuleFor(x => x.Value).StartsWith(prefix, comparison);
    }

    private sealed class NotStartsWithValidator : AbstractValidator<StringModel>
    {
        public NotStartsWithValidator(string prefix, StringComparison comparison) => RuleFor(x => x.Value).NotStartsWith(prefix, comparison);
    }

    private sealed class EndsWithValidator : AbstractValidator<StringModel>
    {
        public EndsWithValidator(string suffix, StringComparison comparison) => RuleFor(x => x.Value).EndsWith(suffix, comparison);
    }

    private sealed class NotEndsWithValidator : AbstractValidator<StringModel>
    {
        public NotEndsWithValidator(string suffix, StringComparison comparison) => RuleFor(x => x.Value).NotEndsWith(suffix, comparison);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotNullOrEmpty.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotNullOrEmpty))]
    public void NotNullOrEmpty_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotNullOrEmptyValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NullOrEmpty.Cases), MemberType = typeof(FluentStringExtensionsTestData.NullOrEmpty))]
    public void NullOrEmpty_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NullOrEmptyValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotNullOrWhiteSpace.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotNullOrWhiteSpace))]
    public void NotNullOrWhiteSpace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotNullOrWhiteSpaceValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NullOrWhiteSpace.Cases), MemberType = typeof(FluentStringExtensionsTestData.NullOrWhiteSpace))]
    public void NullOrWhiteSpace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NullOrWhiteSpaceValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ExactLength.Cases), MemberType = typeof(FluentStringExtensionsTestData.ExactLength))]
    public void ExactLength_BehavesAsExpected(FluentCase<(string? value, int length)> tc)
    {
        var result = new ExactLengthValidator(tc.Value.length).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.LengthBetween.Cases), MemberType = typeof(FluentStringExtensionsTestData.LengthBetween))]
    public void LengthBetween_BehavesAsExpected(FluentCase<(string? value, int min, int max)> tc)
    {
        var result = new LengthBetweenValidator(tc.Value.min, tc.Value.max).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.LongerThan.Cases), MemberType = typeof(FluentStringExtensionsTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(FluentCase<(string? value, int length)> tc)
    {
        var result = new LongerThanValidator(tc.Value.length).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.LongerThanOrEqual.Cases), MemberType = typeof(FluentStringExtensionsTestData.LongerThanOrEqual))]
    public void LongerThanOrEqual_BehavesAsExpected(FluentCase<(string? value, int length)> tc)
    {
        var result = new LongerThanOrEqualValidator(tc.Value.length).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ShorterThan.Cases), MemberType = typeof(FluentStringExtensionsTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(FluentCase<(string? value, int length)> tc)
    {
        var result = new ShorterThanValidator(tc.Value.length).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ShorterThanOrEqual.Cases), MemberType = typeof(FluentStringExtensionsTestData.ShorterThanOrEqual))]
    public void ShorterThanOrEqual_BehavesAsExpected(FluentCase<(string? value, int length)> tc)
    {
        var result = new ShorterThanOrEqualValidator(tc.Value.length).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.DigitsOnly.Cases), MemberType = typeof(FluentStringExtensionsTestData.DigitsOnly))]
    public void DigitsOnly_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new DigitsOnlyValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotDigitsOnly.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotDigitsOnly))]
    public void NotDigitsOnly_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotDigitsOnlyValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.DigitsOnlyWithAllowed.Cases), MemberType = typeof(FluentStringExtensionsTestData.DigitsOnlyWithAllowed))]
    public void DigitsOnlyWithAllowed_BehavesAsExpected(FluentCase<(string? value, char[] allowedNonDigitChars)> tc)
    {
        var result = new DigitsOnlyWithAllowedValidator(tc.Value.allowedNonDigitChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotDigitsOnlyWithAllowed.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotDigitsOnlyWithAllowed))]
    public void NotDigitsOnlyWithAllowed_BehavesAsExpected(FluentCase<(string? value, char[] allowedNonDigitChars)> tc)
    {
        var result = new NotDigitsOnlyWithAllowedValidator(tc.Value.allowedNonDigitChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Uppercase.Cases), MemberType = typeof(FluentStringExtensionsTestData.Uppercase))]
    public void Uppercase_BehavesAsExpected(FluentCase<(string? value, bool lettersOnly)> tc)
    {
        var result = new UppercaseValidator(tc.Value.lettersOnly).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotUppercase.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotUppercase))]
    public void NotUppercase_BehavesAsExpected(FluentCase<(string? value, bool lettersOnly)> tc)
    {
        var result = new NotUppercaseValidator(tc.Value.lettersOnly).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Lowercase.Cases), MemberType = typeof(FluentStringExtensionsTestData.Lowercase))]
    public void Lowercase_BehavesAsExpected(FluentCase<(string? value, bool lettersOnly)> tc)
    {
        var result = new LowercaseValidator(tc.Value.lettersOnly).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotLowercase.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotLowercase))]
    public void NotLowercase_BehavesAsExpected(FluentCase<(string? value, bool lettersOnly)> tc)
    {
        var result = new NotLowercaseValidator(tc.Value.lettersOnly).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Alphabetic.Cases), MemberType = typeof(FluentStringExtensionsTestData.Alphabetic))]
    public void Alphabetic_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new AlphabeticValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotAlphabetic.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotAlphabetic))]
    public void NotAlphabetic_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotAlphabeticValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Numeric.Cases), MemberType = typeof(FluentStringExtensionsTestData.Numeric))]
    public void Numeric_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NumericValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotNumeric.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotNumeric))]
    public void NotNumeric_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotNumericValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Alphanumeric.Cases), MemberType = typeof(FluentStringExtensionsTestData.Alphanumeric))]
    public void Alphanumeric_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new AlphanumericValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotAlphanumeric.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotAlphanumeric))]
    public void NotAlphanumeric_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotAlphanumericValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ContainsAny.Cases), MemberType = typeof(FluentStringExtensionsTestData.ContainsAny))]
    public void ContainsAny_BehavesAsExpected(FluentCase<(string? value, char[] anyOf)> tc)
    {
        var result = new ContainsAnyValidator(tc.Value.anyOf).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Ascii.Cases), MemberType = typeof(FluentStringExtensionsTestData.Ascii))]
    public void Ascii_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new AsciiValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotAscii.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotAscii))]
    public void NotAscii_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotAsciiValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Match.Cases), MemberType = typeof(FluentStringExtensionsTestData.Match))]
    public void Match_BehavesAsExpected(FluentCase<(string? value, Regex pattern)> tc)
    {
        var result = new MatchValidator(tc.Value.pattern).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotMatch.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotMatch))]
    public void NotMatch_BehavesAsExpected(FluentCase<(string? value, Regex pattern)> tc)
    {
        var result = new NotMatchValidator(tc.Value.pattern).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentStringExtensions.RegexPattern
    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.RegexPattern.Cases), MemberType = typeof(FluentStringExtensionsTestData.RegexPattern))]
    public void RegexPattern_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new RegexPatternValidator().Validate(new StringModel { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotWhitespace.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotWhitespace))]
    public void NotWhitespace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotWhitespaceValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ContainsWhitespace.Cases), MemberType = typeof(FluentStringExtensionsTestData.ContainsWhitespace))]
    public void ContainsWhitespace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new ContainsWhitespaceValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotContainsWhitespace.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotContainsWhitespace))]
    public void NotContainsWhitespace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotContainsWhitespaceValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ContainsControlChars.Cases), MemberType = typeof(FluentStringExtensionsTestData.ContainsControlChars))]
    public void ContainsControlChars_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new ContainsControlCharsValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotContainsControlChars.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotContainsControlChars))]
    public void NotContainsControlChars_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotContainsControlCharsValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.PrintableAscii.Cases), MemberType = typeof(FluentStringExtensionsTestData.PrintableAscii))]
    public void PrintableAscii_BehavesAsExpected(FluentCase<(string? value, bool allowCommonWhitespace)> tc)
    {
        var result = new PrintableAsciiValidator(tc.Value.allowCommonWhitespace).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotPrintableAscii.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotPrintableAscii))]
    public void NotPrintableAscii_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotPrintableAsciiValidator().Validate(new StringModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ContainsAllowedOnly.Cases), MemberType = typeof(FluentStringExtensionsTestData.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_BehavesAsExpected(FluentCase<(string? value, char[] allowedChars)> tc)
    {
        var result = new ContainsAllowedOnlyValidator(tc.Value.allowedChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotContainsAllowedOnly.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotContainsAllowedOnly))]
    public void NotContainsAllowedOnly_BehavesAsExpected(FluentCase<(string? value, char[] allowedChars)> tc)
    {
        var result = new NotContainsAllowedOnlyValidator(tc.Value.allowedChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.ContainsDisallowed.Cases), MemberType = typeof(FluentStringExtensionsTestData.ContainsDisallowed))]
    public void ContainsDisallowed_BehavesAsExpected(FluentCase<(string? value, char[] disallowedChars)> tc)
    {
        var result = new ContainsDisallowedValidator(tc.Value.disallowedChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotContainsDisallowed.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotContainsDisallowed))]
    public void NotContainsDisallowed_BehavesAsExpected(FluentCase<(string? value, char[] disallowedChars)> tc)
    {
        var result = new NotContainsDisallowedValidator(tc.Value.disallowedChars).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.Contains.Cases), MemberType = typeof(FluentStringExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(string? value, string substring, StringComparison comparison)> tc)
    {
        var result = new ContainsValidator(tc.Value.substring, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(string? value, string substring, StringComparison comparison)> tc)
    {
        var result = new NotContainsValidator(tc.Value.substring, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.StartsWith.Cases), MemberType = typeof(FluentStringExtensionsTestData.StartsWith))]
    public void StartsWith_BehavesAsExpected(FluentCase<(string? value, string prefix, StringComparison comparison)> tc)
    {
        var result = new StartsWithValidator(tc.Value.prefix, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotStartsWith.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotStartsWith))]
    public void NotStartsWith_BehavesAsExpected(FluentCase<(string? value, string prefix, StringComparison comparison)> tc)
    {
        var result = new NotStartsWithValidator(tc.Value.prefix, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.EndsWith.Cases), MemberType = typeof(FluentStringExtensionsTestData.EndsWith))]
    public void EndsWith_BehavesAsExpected(FluentCase<(string? value, string suffix, StringComparison comparison)> tc)
    {
        var result = new EndsWithValidator(tc.Value.suffix, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentStringExtensionsTestData.NotEndsWith.Cases), MemberType = typeof(FluentStringExtensionsTestData.NotEndsWith))]
    public void NotEndsWith_BehavesAsExpected(FluentCase<(string? value, string suffix, StringComparison comparison)> tc)
    {
        var result = new NotEndsWithValidator(tc.Value.suffix, tc.Value.comparison).Validate(new StringModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
