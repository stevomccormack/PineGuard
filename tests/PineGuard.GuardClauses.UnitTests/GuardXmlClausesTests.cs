using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardXmlClausesTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardXmlClausesTestData.NotXml.ValidCases), MemberType = typeof(GuardXmlClausesTestData.NotXml))]
    [MemberData(nameof(GuardXmlClausesTestData.NotXml.InvalidCases), MemberType = typeof(GuardXmlClausesTestData.NotXml))]
    public void NotXml_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXml(value));
        AssertCustomMessage(tc, () => Guard.Against.NotXml(value, message: CustomMessage));

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardXmlClausesTestData.NotHasXmlRoot.ValidCases), MemberType = typeof(GuardXmlClausesTestData.NotHasXmlRoot))]
    [MemberData(nameof(GuardXmlClausesTestData.NotHasXmlRoot.InvalidCases), MemberType = typeof(GuardXmlClausesTestData.NotHasXmlRoot))]
    public void NotHasXmlRoot_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasXmlRoot(value, F.HasXmlRoot.LocalName, F.HasXmlRoot.Namespace));
        AssertCustomMessage(tc, () => Guard.Against.NotHasXmlRoot(value, F.HasXmlRoot.LocalName, F.HasXmlRoot.Namespace, message: CustomMessage));

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardXmlClausesTestData.NotHasXmlRootAnyNamespace.ValidCases), MemberType = typeof(GuardXmlClausesTestData.NotHasXmlRootAnyNamespace))]
    [MemberData(nameof(GuardXmlClausesTestData.NotHasXmlRootAnyNamespace.InvalidCases), MemberType = typeof(GuardXmlClausesTestData.NotHasXmlRootAnyNamespace))]
    public void NotHasXmlRoot_AnyNamespace_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasXmlRoot(value, F.HasXmlRoot.LocalName));
        AssertCustomMessage(tc, () => Guard.Against.NotHasXmlRoot(value, F.HasXmlRoot.LocalName, message: CustomMessage));

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardXmlClausesTestData.NotXmlContentType.ValidCases), MemberType = typeof(GuardXmlClausesTestData.NotXmlContentType))]
    [MemberData(nameof(GuardXmlClausesTestData.NotXmlContentType.InvalidCases), MemberType = typeof(GuardXmlClausesTestData.NotXmlContentType))]
    public void NotXmlContentType_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXmlContentType(headers));
        AssertCustomMessage(tc, () => Guard.Against.NotXmlContentType(headers, message: CustomMessage));

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }
}
