using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentXmlExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }
    private sealed record HeaderModel { public IReadOnlyDictionary<string, IEnumerable<string>>? Value { get; init; } }

    private sealed class XmlValidator : AbstractValidator<Model>
    {
        public XmlValidator() => RuleFor(x => x.Value).Xml();
    }

    private sealed class XmlContentTypeValidator : AbstractValidator<HeaderModel>
    {
        public XmlContentTypeValidator() => RuleFor(x => x.Value).XmlContentType();
    }

    private sealed class HasXmlRootValidator : AbstractValidator<Model>
    {
        public HasXmlRootValidator() => RuleFor(x => x.Value).HasXmlRoot(F.HasXmlRoot.LocalName, F.HasXmlRoot.Namespace);
    }

    private sealed class HasXmlRootAnyNamespaceValidator : AbstractValidator<Model>
    {
        public HasXmlRootAnyNamespaceValidator() => RuleFor(x => x.Value).HasXmlRoot(F.HasXmlRoot.LocalName);
    }

    [Theory]
    [MemberData(nameof(FluentXmlExtensionsTestData.Xml.Cases), MemberType = typeof(FluentXmlExtensionsTestData.Xml))]
    public void Xml_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new XmlValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentXmlExtensionsTestData.XmlContentType.Cases), MemberType = typeof(FluentXmlExtensionsTestData.XmlContentType))]
    public void XmlContentType_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new XmlContentTypeValidator().Validate(new HeaderModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentXmlExtensionsTestData.HasXmlRoot.Cases), MemberType = typeof(FluentXmlExtensionsTestData.HasXmlRoot))]
    public void HasXmlRoot_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HasXmlRootValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentXmlExtensionsTestData.HasXmlRootAnyNamespace.Cases), MemberType = typeof(FluentXmlExtensionsTestData.HasXmlRootAnyNamespace))]
    public void HasXmlRootAnyNamespace_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HasXmlRootAnyNamespaceValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
