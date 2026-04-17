using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class CsvColumnSchemaTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CsvColumnSchemaTestData.Init.Cases), MemberType = typeof(CsvColumnSchemaTestData.Init))]
    public void WithExpression_ExecutesInitAccessors(CsvColumnSchemaTestData.Init.Case testCase)
    {
        // Act
        var updated = new CsvColumnSchema(
            Name: testCase.Value.Name,
            Type: testCase.Value.Type,
            IsRequired: testCase.Value.IsRequired,
            MaxLength: testCase.Value.MaxLength);

        // Assert
        Assert.Equal(testCase.Value.Name, updated.Name);
        Assert.Equal(testCase.Value.Type, updated.Type);
        Assert.Equal(testCase.Value.IsRequired, updated.IsRequired);
        Assert.Equal(testCase.Value.MaxLength, updated.MaxLength);
    }

    [Theory]
    [MemberData(nameof(CsvColumnSchemaTestData.WithExpression.Cases), MemberType = typeof(CsvColumnSchemaTestData.WithExpression))]
    public void WithExpression_MutatesAllFields(CsvColumnSchemaTestData.WithExpression.Case testCase)
    {
        // Arrange
        var original = new CsvColumnSchema(
            Name: testCase.Value.Name,
            Type: testCase.Value.Type,
            IsRequired: testCase.Value.IsRequired,
            MaxLength: testCase.Value.MaxLength);

        // Act
        // ReSharper disable once WithExpressionModifiesAllMembers
        var mutated = original with
        {
            Name = testCase.Mutated.Name,
            Type = testCase.Mutated.Type,
            IsRequired = testCase.Mutated.IsRequired,
            MaxLength = testCase.Mutated.MaxLength
        };

        // Assert
        Assert.Equal(testCase.Mutated.Name, mutated.Name);
        Assert.Equal(testCase.Mutated.Type, mutated.Type);
        Assert.Equal(testCase.Mutated.IsRequired, mutated.IsRequired);
        Assert.Equal(testCase.Mutated.MaxLength, mutated.MaxLength);
    }

    [Theory]
    [MemberData(nameof(CsvColumnSchemaTestData.CtorStringType.ValidCases), MemberType = typeof(CsvColumnSchemaTestData.CtorStringType))]
    public void CtorStringType_MapsKnownTypes(CsvColumnSchemaTestData.CtorStringType.ValidCase testCase)
    {
        // Act
        var schema = new CsvColumnSchema(testCase.Value.Name, testCase.Value.Type);

        // Assert
        Assert.Equal(testCase.Expected, schema.Type);
    }

    [Theory]
    [MemberData(nameof(CsvColumnSchemaTestData.CtorStringType.InvalidCases), MemberType = typeof(CsvColumnSchemaTestData.CtorStringType))]
    public void CtorStringType_Throws_ForInvalidType(CsvColumnSchemaTestData.CtorStringType.InvalidCase testCase)
    {
        // Act & Assert
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => new CsvColumnSchema(testCase.Value.Name, testCase.Value.Type!));
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
