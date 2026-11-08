using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PineGuard.DataAnnotations;
using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class XmlImportConsumerTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlImportConsumerTestData.Cases), MemberType = typeof(XmlImportConsumerTestData))]
    public void CombinedImports_PreserveWellFormednessAndSchemaValidity(RuleCase<(string xml, bool wellFormed)> tc)
    {
        // Arrange
        var (xml, wellFormed) = tc.Value;
        var names = new NameTable();
        var frameworkValidator = new XmlSchemaValidator(names, TestSchemas.DocumentSet,
            new XmlNamespaceManager(names), XmlSchemaValidationFlags.None);
        var serializer = new XmlSerializer(typeof(string));
        var mustValidator = new XmlSchemaMustValidator(TestSchemas.DocumentSet);
        var syntaxAttribute = new XmlStringAttribute();
        var schemaAttribute = new ValidXmlAttribute();
        using var textReader = new System.IO.StringReader("<string>value</string>");
        using var xmlReader = XmlReader.Create(textReader);

        // Act
        var syntax = Must.Be.Xml(xml);
        var schema = Must.Be.ValidXml(xml, TestSchemas.DocumentSet);
        var validation = mustValidator.Validate(xml);
        frameworkValidator.Initialize();

        // Assert
        Assert.Equal(wellFormed, XmlRules.IsXml(xml));
        Assert.Equal(wellFormed, syntax.Success);
        Assert.Equal(wellFormed, syntaxAttribute.GetValidationResult(xml,
            new System.ComponentModel.DataAnnotations.ValidationContext(new object())) is null);
        Assert.Equal(tc.Expected.IsValid, XmlSchemaRules.IsValidXml(xml, TestSchemas.DocumentSet));
        Assert.Equal(tc.Expected.IsValid, schema.Success);
        Assert.Equal(tc.Expected.IsValid, validation.Success);
        Assert.IsType<ValidXmlAttribute>(schemaAttribute);
        Assert.True(serializer.CanDeserialize(xmlReader));
        if (tc.Expected.IsValid)
            Assert.Equal(xml, Guard.Against.InvalidXml(xml, TestSchemas.DocumentSet));
        else
            Assert.Throws<ArgumentException>(() => Guard.Against.InvalidXml(xml, TestSchemas.DocumentSet));
    }
}
