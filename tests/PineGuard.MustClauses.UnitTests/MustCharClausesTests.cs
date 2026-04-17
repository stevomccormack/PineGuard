using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustCharClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Letter.ValidCases), MemberType = typeof(MustCharClausesTestData.Letter))]
    [MemberData(nameof(MustCharClausesTestData.Letter.InvalidCases), MemberType = typeof(MustCharClausesTestData.Letter))]
    public void Letter_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Letter(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotLetter.ValidCases), MemberType = typeof(MustCharClausesTestData.NotLetter))]
    [MemberData(nameof(MustCharClausesTestData.NotLetter.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotLetter))]
    public void NotLetter_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotLetter(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Digit.ValidCases), MemberType = typeof(MustCharClausesTestData.Digit))]
    [MemberData(nameof(MustCharClausesTestData.Digit.InvalidCases), MemberType = typeof(MustCharClausesTestData.Digit))]
    public void Digit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Digit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotDigit.ValidCases), MemberType = typeof(MustCharClausesTestData.NotDigit))]
    [MemberData(nameof(MustCharClausesTestData.NotDigit.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotDigit))]
    public void NotDigit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotDigit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.LetterOrDigit.ValidCases), MemberType = typeof(MustCharClausesTestData.LetterOrDigit))]
    [MemberData(nameof(MustCharClausesTestData.LetterOrDigit.InvalidCases), MemberType = typeof(MustCharClausesTestData.LetterOrDigit))]
    public void LetterOrDigit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.LetterOrDigit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotLetterOrDigit.ValidCases), MemberType = typeof(MustCharClausesTestData.NotLetterOrDigit))]
    [MemberData(nameof(MustCharClausesTestData.NotLetterOrDigit.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotLetterOrDigit))]
    public void NotLetterOrDigit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotLetterOrDigit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Ascii.ValidCases), MemberType = typeof(MustCharClausesTestData.Ascii))]
    [MemberData(nameof(MustCharClausesTestData.Ascii.InvalidCases), MemberType = typeof(MustCharClausesTestData.Ascii))]
    public void Ascii_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Ascii(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotAscii.ValidCases), MemberType = typeof(MustCharClausesTestData.NotAscii))]
    [MemberData(nameof(MustCharClausesTestData.NotAscii.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotAscii))]
    public void NotAscii_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotAscii(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.PrintableAscii.ValidCases), MemberType = typeof(MustCharClausesTestData.PrintableAscii))]
    [MemberData(nameof(MustCharClausesTestData.PrintableAscii.InvalidCases), MemberType = typeof(MustCharClausesTestData.PrintableAscii))]
    public void PrintableAscii_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.PrintableAscii(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotPrintableAscii.ValidCases), MemberType = typeof(MustCharClausesTestData.NotPrintableAscii))]
    [MemberData(nameof(MustCharClausesTestData.NotPrintableAscii.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotPrintableAscii))]
    public void NotPrintableAscii_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotPrintableAscii(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotWhitespace.ValidCases), MemberType = typeof(MustCharClausesTestData.NotWhitespace))]
    [MemberData(nameof(MustCharClausesTestData.NotWhitespace.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotWhitespace))]
    public void NotWhitespace_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotWhitespace(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Control.ValidCases), MemberType = typeof(MustCharClausesTestData.Control))]
    [MemberData(nameof(MustCharClausesTestData.Control.InvalidCases), MemberType = typeof(MustCharClausesTestData.Control))]
    public void Control_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Control(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotControl.ValidCases), MemberType = typeof(MustCharClausesTestData.NotControl))]
    [MemberData(nameof(MustCharClausesTestData.NotControl.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotControl))]
    public void NotControl_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotControl(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Uppercase.ValidCases), MemberType = typeof(MustCharClausesTestData.Uppercase))]
    [MemberData(nameof(MustCharClausesTestData.Uppercase.InvalidCases), MemberType = typeof(MustCharClausesTestData.Uppercase))]
    public void Uppercase_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Uppercase(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.Lowercase.ValidCases), MemberType = typeof(MustCharClausesTestData.Lowercase))]
    [MemberData(nameof(MustCharClausesTestData.Lowercase.InvalidCases), MemberType = typeof(MustCharClausesTestData.Lowercase))]
    public void Lowercase_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.Lowercase(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.HexDigit.ValidCases), MemberType = typeof(MustCharClausesTestData.HexDigit))]
    [MemberData(nameof(MustCharClausesTestData.HexDigit.InvalidCases), MemberType = typeof(MustCharClausesTestData.HexDigit))]
    public void HexDigit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.HexDigit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCharClausesTestData.NotHexDigit.ValidCases), MemberType = typeof(MustCharClausesTestData.NotHexDigit))]
    [MemberData(nameof(MustCharClausesTestData.NotHexDigit.InvalidCases), MemberType = typeof(MustCharClausesTestData.NotHexDigit))]
    public void NotHexDigit_BehavesAsExpected(MustCase<char?> tc)
    {
        var result = Must.Be.NotHexDigit(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
