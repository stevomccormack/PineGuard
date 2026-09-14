using System.Threading;
using System.Threading.Tasks;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Xml;
using PineGuard.Xml.UnitTests.Schemas;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class XmlSchemaMustValidatorTests(ITestOutputHelper output)
    : BaseMustValidationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.Validate.Cases), MemberType = typeof(XmlSchemaMustValidatorTestData.Validate))]
    public void Validate_BehavesAsExpected(MustValidationCase<(string? xml, XmlSchemaValidationOptions? options)> tc)
    {
        // Arrange
        var (xml, options) = tc.Value;
        var validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet, options);

        // Act
        var result = validator.Validate(xml!);

        // Assert
        AssertResult(tc, result);

        foreach (var failure in result.Failures)
            Assert.Null(failure.Value);

        if (tc.Name == nameof(TestSchemas.MsgIdTooLong))
            Assert.False(string.IsNullOrEmpty(result.Failures[0].Message));
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.ValidateAsync.Cases), MemberType = typeof(XmlSchemaMustValidatorTestData.ValidateAsync))]
    public async Task ValidateAsync_BehavesAsExpected(MustValidationCase<(string? xml, XmlSchemaValidationOptions? options)> tc)
    {
        // Arrange
        var (xml, options) = tc.Value;
        var validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet, options);

        // Act
        var result = await validator.ValidateAsync(xml!, CancellationToken.None);

        // Assert
        AssertResult(tc, result);

        foreach (var failure in result.Failures)
            Assert.Null(failure.Value);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.ValidatedType.ValidCases), MemberType = typeof(XmlSchemaMustValidatorTestData.ValidatedType))]
    public void ValidatedType_BehavesAsExpected(XmlSchemaMustValidatorTestData.ValidatedType.Case tc)
    {
        // Arrange
        IMustValidator validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet);

        // Act
        var actual = validator.ValidatedType;

        // Assert
        Assert.Equal(tc.Expected, actual);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.NonGenericValidate.Cases), MemberType = typeof(XmlSchemaMustValidatorTestData.NonGenericValidate))]
    public void NonGenericValidate_BehavesAsExpected(MustValidationCase<object?> tc)
    {
        // Arrange
        IMustValidator validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet);

        // Act
        var result = validator.Validate(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.NonGenericValidateAsync.Cases), MemberType = typeof(XmlSchemaMustValidatorTestData.NonGenericValidateAsync))]
    public async Task NonGenericValidateAsync_BehavesAsExpected(MustValidationCase<(object? value, MustValidationMode? mode)> tc)
    {
        // Arrange
        IMustValidator validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet);
        var (value, mode) = tc.Value;

        // Act
        var result = mode is { } m
            ? await validator.ValidateAsync(value, m, CancellationToken.None)
            : await validator.ValidateAsync(value, CancellationToken.None);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.NonGenericValidateCastThrows.InvalidCases), MemberType = typeof(XmlSchemaMustValidatorTestData.NonGenericValidateCastThrows))]
    public void NonGenericValidateCastThrows_BehavesAsExpected(XmlSchemaMustValidatorTestData.NonGenericValidateCastThrows.ThrowCase tc)
    {
        // Arrange
        IMustValidator validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet);

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => validator.Validate(tc.Value));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.Constructor.ValidCases), MemberType = typeof(XmlSchemaMustValidatorTestData.Constructor))]
    public void Constructor_BehavesAsExpected(MustValidationCase<XmlSchemaValidationOptions?> tc)
    {
        // Arrange
        var validator = new XmlSchemaMustValidator(TestSchemas.DocumentSet, tc.Value);

        // Act
        var result = validator.Validate(TestSchemas.ValidWithSupplementary);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(XmlSchemaMustValidatorTestData.Constructor.InvalidCases), MemberType = typeof(XmlSchemaMustValidatorTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(XmlSchemaMustValidatorTestData.Constructor.ThrowCase tc)
    {
        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => new XmlSchemaMustValidator(tc.Value!, null));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
