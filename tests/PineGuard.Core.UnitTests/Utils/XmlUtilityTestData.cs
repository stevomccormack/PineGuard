using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class XmlUtilityTestData
{
    public static class TryGetRootName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "<root />", true, "root", ""),
            new("namespaced", "<d:Document xmlns:d=\"urn:test:doc\"><d:Id>1</d:Id></d:Document>", true, "Document", "urn:test:doc"),
            new("with declaration", "<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>", true, "root", "")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null, null),
            new("whitespace", "   ", false, null, null),
            new("invalid xml", "<root>", false, null, null),
            new("multiple roots", "<a/><b/>", false, null, null),
            new("doctype prohibited", "<!DOCTYPE root [<!ELEMENT root ANY>]><root />", false, null, null)
        ];

        public sealed record ValidCase : ReturnCase<string?, (bool ok, string? name, string? ns)>
        {
            public ValidCase(string name, string? value, bool expectedOk, string? expectedName, string? expectedNamespace)
                : base(name, value, (expectedOk, expectedName, expectedNamespace)) { }
        }
    }
}
