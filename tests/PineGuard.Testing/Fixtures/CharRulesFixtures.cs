using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class CharRulesFixtures
{
    public static class IsLetter
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, true),
            new(nameof(LowercaseLetter), LowercaseLetter, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(Digit), Digit, false),
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false),
            new(nameof(C1Control), C1Control, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsDigit
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(Digit), Digit, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, false),
            new(nameof(LowercaseLetter), LowercaseLetter, false),
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(C1Control), C1Control, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLetterOrDigit
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, true),
            new(nameof(LowercaseLetter), LowercaseLetter, true),
            new(nameof(Digit), Digit, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(C1Control), C1Control, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsAscii
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, true),
            new(nameof(LowercaseLetter), LowercaseLetter, true),
            new(nameof(Digit), Digit, true),
            new(nameof(PrintableMin), PrintableMin, true),
            new(nameof(PrintableMax), PrintableMax, true),
            new(nameof(UnitSeparator), UnitSeparator, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, true),
            new(nameof(AsciiMax), AsciiMax, true)
        ];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(C1Control), C1Control, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsPrintableAscii
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, true),
            new(nameof(LowercaseLetter), LowercaseLetter, true),
            new(nameof(Digit), Digit, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios =>
        [
            new(nameof(PrintableMin), PrintableMin, true),
            new(nameof(PrintableMax), PrintableMax, true)
        ];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(UnitSeparator), UnitSeparator, false),
            new(nameof(C1Control), C1Control, false),
            new(nameof(AsciiMax), AsciiMax, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsWhitespace
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(PrintableMin), PrintableMin, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, false),
            new(nameof(LowercaseLetter), LowercaseLetter, false),
            new(nameof(Digit), Digit, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(C1Control), C1Control, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsControl
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, true),
            new(nameof(UnitSeparator), UnitSeparator, true),
            new(nameof(AsciiMax), AsciiMax, true),
            new(nameof(C1Control), C1Control, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, false),
            new(nameof(LowercaseLetter), LowercaseLetter, false),
            new(nameof(Digit), Digit, false),
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsUppercase
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(LowercaseLetter), LowercaseLetter, false),
            new(nameof(Digit), Digit, false),
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false),
            new(nameof(C1Control), C1Control, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLowercase
    {
        public static readonly char? UppercaseLetter = 'A';
        public static readonly char? LowercaseLetter = 'a';
        public static readonly char? Digit = '0';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? PrintableMin = ' ';
        public static readonly char? PrintableMax = '~';
        public static readonly char? UnitSeparator = '\u001F';
        public static readonly char? C1Control = '\u0080';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(LowercaseLetter), LowercaseLetter, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios => [];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(UppercaseLetter), UppercaseLetter, false),
            new(nameof(Digit), Digit, false),
            new(nameof(PrintableMin), PrintableMin, false),
            new(nameof(PrintableMax), PrintableMax, false),
            new(nameof(UnitSeparator), UnitSeparator, false),
            new(nameof(C1Control), C1Control, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsHexDigit
    {
        public static readonly char? Numeric = '9';
        public static readonly char? LowercaseF = 'f';
        public static readonly char? LowercaseA = 'a';
        public static readonly char? UppercaseA = 'A';
        public static readonly char? UppercaseB = 'B';
        public static readonly char? UppercaseE = 'E';
        public static readonly char? MinHexDigit = '0';
        public static readonly char? MaxHexDigit = 'F';
        public static readonly char? Before0 = '/';
        public static readonly char? After9 = ':';
        public static readonly char? LowercaseG = 'g';
        public static readonly char? UppercaseG = 'G';
        public static readonly char? AsciiMin = '\0';
        public static readonly char? AsciiMax = '\u007F';
        public static readonly char? Null = null;

        public static RuleScenario<char?>[] ValidScenarios =>
        [
            new(nameof(Numeric), Numeric, true),
            new(nameof(LowercaseF), LowercaseF, true),
            new(nameof(LowercaseA), LowercaseA, true),
            new(nameof(UppercaseA), UppercaseA, true),
            new(nameof(UppercaseB), UppercaseB, true),
            new(nameof(UppercaseE), UppercaseE, true)
        ];

        public static RuleScenario<char?>[] ValidEdgeScenarios =>
        [
            new(nameof(MinHexDigit), MinHexDigit, true),
            new(nameof(MaxHexDigit), MaxHexDigit, true)
        ];

        public static RuleScenario<char?>[] InvalidScenarios =>
        [
            new(nameof(Before0), Before0, false),
            new(nameof(After9), After9, false),
            new(nameof(LowercaseG), LowercaseG, false),
            new(nameof(UppercaseG), UppercaseG, false)
        ];

        public static RuleScenario<char?>[] InvalidEdgeScenarios =>
        [
            new(nameof(AsciiMin), AsciiMin, false),
            new(nameof(AsciiMax), AsciiMax, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<char?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<char?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<char?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
