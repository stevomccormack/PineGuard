using PineGuard.GuardClauses;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class GuardXmlSchemaClausesTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardXmlSchemaClausesTestData.InvalidXml.ValidCases), MemberType = typeof(GuardXmlSchemaClausesTestData.InvalidXml))]
    [MemberData(nameof(GuardXmlSchemaClausesTestData.InvalidXml.InvalidCases), MemberType = typeof(GuardXmlSchemaClausesTestData.InvalidXml))]
    public void InvalidXml_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.InvalidXml(value, TestSchemas.DocumentSet));
        AssertCustomMessage(tc, () => Guard.Against.InvalidXml(value, TestSchemas.DocumentSet, message: CustomMessage));

        if (tc.Expected.IsValid)
            Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardXmlSchemaClausesTestData.InvalidXmlExceptionCreator.InvalidCases), MemberType = typeof(GuardXmlSchemaClausesTestData.InvalidXmlExceptionCreator))]
    public void InvalidXmlExceptionCreator_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        AssertResult(tc, () => Guard.Against.InvalidXml(value, TestSchemas.DocumentSet, exceptionCreator: () => new InvalidOperationException("custom")));
    }
}
