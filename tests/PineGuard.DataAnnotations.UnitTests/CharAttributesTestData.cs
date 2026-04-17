using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.CharRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class CharAttributesTestData
{
    public static class CharAscii
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsAscii.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsAscii.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNonAscii
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsAscii.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsAscii.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsDigit.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNotDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsDigit.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharLetter
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLetter.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLetter.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNotLetter
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLetter.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLetter.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharLetterOrDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLetterOrDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLetterOrDigit.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNonLetterOrDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLetterOrDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLetterOrDigit.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharLowercase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLowercase.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLowercase.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharUppercase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUppercase.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsUppercase.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharHexDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHexDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsHexDigit.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNotHexDigit
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHexDigit.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsHexDigit.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharPrintableAscii
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPrintableAscii.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsPrintableAscii.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNonPrintableAscii
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPrintableAscii.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsPrintableAscii.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNonWhitespace
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWhitespace.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsWhitespace.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharControl
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsControl.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsControl.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class CharNotControl
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsControl.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsControl.Null) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }
}
