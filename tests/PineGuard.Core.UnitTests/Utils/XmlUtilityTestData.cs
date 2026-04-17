using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class XmlUtilityTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "<root />", true, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, false),
            new("whitespace", "   ", false, false),
            new("invalid xml", "<root>", false, false),
            new("doctype prohibited", "<!DOCTYPE root [<!ELEMENT root ANY>]><root />", false, false)
        ];

        public sealed record ValidCase : ReturnCase<string?, (bool ok, bool hasDocument)>
        {
            public ValidCase(string name, string? value, bool expectedOk, bool expectedHasDocument)
                : base(name, value, (expectedOk, expectedHasDocument)) { }
        }
    }
}
