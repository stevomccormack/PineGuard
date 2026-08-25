using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class FilePathUtilityTestData
{
    public static class ContainsInvalidFileNameChars
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("empty => false", "", false),
            new("normal => false", "file.txt", false),
            new("invalid char => true", "fi|le.txt", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null => false", null, false),
            new("colon invalid => true", "file:name.txt", true),
            new("question mark invalid => true", "file?.txt", true),
            new("asterisk invalid => true", "file*.txt", true),
            new("backslash invalid => true", "file\\name.txt", true),
            new("forward slash invalid => true", "file/name.txt", true),
            new("unicode filename => false", "über-file.txt", false),
            new("spaces in name => false", "file name.txt", false),
            new("embedded control char (BEL) invalid => true", "report\u0007.txt", true),
            new("embedded tab invalid => true", "a\tb.txt", true)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class IsWindowsReservedDeviceName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("con", "CON", true),
            new("con with ext", "con.txt", true),
            new("prn", "PRN", true),
            new("aux", "AUX", true),
            new("nul", "NUL", true),
            new("com1", "COM1", true),
            new("lpt9", "LPT9", true),
            new("com0 not reserved", "COM0", false),
            new("lpt0 not reserved", "LPT0", false),
            new("com10 not reserved", "COM10", false),
            new("lpt10 not reserved", "LPT10", false),
            new("normal", "file", false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("whitespace", "  ", false),
            new("reserved with surrounding whitespace", "  con  ", true),
            new("leading dot produces empty base", ".con", false),
            new("trailing space before extension is reserved", "CON .txt", true)
        ];

        public sealed record ValidCase(string Name, string Value, bool Expected)
            : IsCase<string>(Name, Value, Expected);
    }
}
