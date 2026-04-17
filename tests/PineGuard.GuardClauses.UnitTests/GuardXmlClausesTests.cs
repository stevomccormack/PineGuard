using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

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

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardXmlClausesTestData.NotXmlDocument.ValidCases), MemberType = typeof(GuardXmlClausesTestData.NotXmlDocument))]
    [MemberData(nameof(GuardXmlClausesTestData.NotXmlDocument.InvalidCases), MemberType = typeof(GuardXmlClausesTestData.NotXmlDocument))]
    public void NotXmlDocument_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXmlDocument(value));

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

        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }
}
