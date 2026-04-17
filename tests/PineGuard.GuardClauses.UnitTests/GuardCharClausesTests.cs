using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardCharClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotLetter.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotLetter))]
    [MemberData(nameof(GuardCharClausesTestData.NotLetter.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotLetter))]
    public void NotLetter_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotLetter(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotDigit.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotDigit))]
    [MemberData(nameof(GuardCharClausesTestData.NotDigit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotDigit))]
    public void NotDigit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotDigit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Digit.ValidCases), MemberType = typeof(GuardCharClausesTestData.Digit))]
    [MemberData(nameof(GuardCharClausesTestData.Digit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Digit))]
    public void Digit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Digit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotLetterOrDigit.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotLetterOrDigit))]
    [MemberData(nameof(GuardCharClausesTestData.NotLetterOrDigit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotLetterOrDigit))]
    public void NotLetterOrDigit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotLetterOrDigit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.LetterOrDigit.ValidCases), MemberType = typeof(GuardCharClausesTestData.LetterOrDigit))]
    [MemberData(nameof(GuardCharClausesTestData.LetterOrDigit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.LetterOrDigit))]
    public void LetterOrDigit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.LetterOrDigit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotAscii.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotAscii))]
    [MemberData(nameof(GuardCharClausesTestData.NotAscii.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotAscii))]
    public void NotAscii_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotAscii(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Ascii.ValidCases), MemberType = typeof(GuardCharClausesTestData.Ascii))]
    [MemberData(nameof(GuardCharClausesTestData.Ascii.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Ascii))]
    public void Ascii_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Ascii(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotPrintableAscii.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotPrintableAscii))]
    [MemberData(nameof(GuardCharClausesTestData.NotPrintableAscii.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotPrintableAscii))]
    public void NotPrintableAscii_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotPrintableAscii(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.PrintableAscii.ValidCases), MemberType = typeof(GuardCharClausesTestData.PrintableAscii))]
    [MemberData(nameof(GuardCharClausesTestData.PrintableAscii.InvalidCases), MemberType = typeof(GuardCharClausesTestData.PrintableAscii))]
    public void PrintableAscii_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.PrintableAscii(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Whitespace.ValidCases), MemberType = typeof(GuardCharClausesTestData.Whitespace))]
    [MemberData(nameof(GuardCharClausesTestData.Whitespace.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Whitespace))]
    public void Whitespace_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Whitespace(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotControl.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotControl))]
    [MemberData(nameof(GuardCharClausesTestData.NotControl.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotControl))]
    public void NotControl_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotControl(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Control.ValidCases), MemberType = typeof(GuardCharClausesTestData.Control))]
    [MemberData(nameof(GuardCharClausesTestData.Control.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Control))]
    public void Control_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Control(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Letter.ValidCases), MemberType = typeof(GuardCharClausesTestData.Letter))]
    [MemberData(nameof(GuardCharClausesTestData.Letter.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Letter))]
    public void Letter_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Letter(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.HexDigit.ValidCases), MemberType = typeof(GuardCharClausesTestData.HexDigit))]
    [MemberData(nameof(GuardCharClausesTestData.HexDigit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.HexDigit))]
    public void HexDigit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.HexDigit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.NotHexDigit.ValidCases), MemberType = typeof(GuardCharClausesTestData.NotHexDigit))]
    [MemberData(nameof(GuardCharClausesTestData.NotHexDigit.InvalidCases), MemberType = typeof(GuardCharClausesTestData.NotHexDigit))]
    public void NotHexDigit_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.NotHexDigit(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Lowercase.ValidCases), MemberType = typeof(GuardCharClausesTestData.Lowercase))]
    [MemberData(nameof(GuardCharClausesTestData.Lowercase.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Lowercase))]
    public void Lowercase_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Lowercase(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCharClausesTestData.Uppercase.ValidCases), MemberType = typeof(GuardCharClausesTestData.Uppercase))]
    [MemberData(nameof(GuardCharClausesTestData.Uppercase.InvalidCases), MemberType = typeof(GuardCharClausesTestData.Uppercase))]
    public void Uppercase_BehavesAsExpected(GuardCase<char?> tc)
    {
        var result = AssertResult(tc, () => Guard.Against.Uppercase(tc.Value!.Value, paramName: "value"));
        if (tc.Expected.IsValid) Assert.Equal(tc.Value!.Value, result);
    }
}
