using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class XmlRulesTestData
{
    public static class IsXml
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsXml.AllScenarios.ToRuleCases();
    }

    public static class IsXmlContentType
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.IsXmlContentType.AllScenarios.ToRuleCases();
    }

    public static class HasXmlRoot
    {
        public static TheoryData<RuleCase<(string? value, string localName, string? namespaceUri)>> Cases =>
            F.HasXmlRoot.AllScenarios.Project(v => (value: v, localName: F.HasXmlRoot.LocalName, namespaceUri: (string?)F.HasXmlRoot.Namespace)).ToRuleCases();

        public static TheoryData<RuleCase<(string? value, string localName, string? namespaceUri)>> AnyNamespaceCases =>
            F.HasXmlRoot.AnyNamespaceAllScenarios.Project(v => (value: v, localName: F.HasXmlRoot.LocalName, namespaceUri: (string?)null)).ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new($"{nameof(F.HasXmlRoot.Matching)}-BlankLocalName", (F.HasXmlRoot.Matching, ""), new ExpectedException(typeof(ArgumentException), "localName")),
            new($"{nameof(F.HasXmlRoot.Matching)}-WhitespaceLocalName", (F.HasXmlRoot.Matching, "   "), new ExpectedException(typeof(ArgumentException), "localName"))
        ];

        public sealed record InvalidCase(string Name, (string? Value, string LocalName) Input, ExpectedException ExpectedException)
            : ThrowsCase<(string? Value, string LocalName)>(Name, Input, ExpectedException);
    }
}
