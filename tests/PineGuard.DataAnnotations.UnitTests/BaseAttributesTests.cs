using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class BaseAttributesTests : BaseUnitTest
{
    private sealed class TestImplementationObjectAttribute : ObjectAttributeBase
    {
        protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
            => InvokeGenericMust("Default", value, validationContext);

        public ValidationResult? TestInvokeGenericMust(string methodName, object? value, ValidationContext ctx, params object?[] args)
            => InvokeGenericMust(methodName, value, ctx, args);

        public static object?[] TestBuildInvokeArgs(MethodInfo method, object? value, params object?[] args)
            => BuildInvokeArgs(method, value, args);
    }

    private sealed class TestImplementationNumberAttribute : NumberAttributeBase
    {
        protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
            => ValidationResult.Success;

        public ValidationResult? TestInvokeAndMap(string methodName, object? value, ValidationContext ctx, params object?[] args)
            => InvokeAndMap(methodName, value, ctx, args);
    }

    private sealed class Model;

    [Theory]
    [MemberData(
        nameof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust.ValidCases),
        MemberType = typeof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust))]
    [MemberData(
        nameof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust.EdgeCases),
        MemberType = typeof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust))]
    public void ObjectAttributeBase_InvokeGenericMust_ShouldReturnExpected(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust.ValidCase testCase)
    {
        // Arrange
        var attribute = new TestImplementationObjectAttribute();
        var model = new Model();
        var context = new ValidationContext(model);

        if (testCase.Value.MemberName is not null)
        {
            context.MemberName = testCase.Value.MemberName;
        }

        // Act
        var result = attribute.TestInvokeGenericMust(testCase.Value.MethodName, testCase.Value.Value, context);

        // Assert
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(
        nameof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust.InvalidCases),
        MemberType = typeof(BaseAttributesTestData.ObjectAttributeBaseInvokeGenericMust))]
    public void ObjectAttributeBase_InvokeGenericMust_ShouldThrowExpected(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new TestImplementationObjectAttribute();
        var model = new Model();
        var data = ((ThrowsCase<(string MethodName, object? Value, string? MemberName)>)testCase).Value;
        var context = new ValidationContext(model);

        if (data.MemberName is not null)
        {
            context.MemberName = data.MemberName;
        }

        // Act
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => attribute.TestInvokeGenericMust(data.MethodName, data.Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(
        nameof(BaseAttributesTestData.ValidationAttributeBaseBuildInvokeArgs.ValidCases),
        MemberType = typeof(BaseAttributesTestData.ValidationAttributeBaseBuildInvokeArgs))]
    [MemberData(
        nameof(BaseAttributesTestData.ValidationAttributeBaseBuildInvokeArgs.EdgeCases),
        MemberType = typeof(BaseAttributesTestData.ValidationAttributeBaseBuildInvokeArgs))]
    public void ValidationAttributeBase_BuildInvokeArgs_ShouldReturnExpected(BaseAttributesTestData.ValidationAttributeBaseBuildInvokeArgs.ValidCase testCase)
    {
        // Arrange
        var value = new object();

        // Act
        var invokeArgs = TestImplementationObjectAttribute.TestBuildInvokeArgs(testCase.Value, value);

        // Assert
        Assert.Equal(testCase.Expected, invokeArgs.Length);
        if (testCase.Expected > 0) Assert.Null(invokeArgs[0]);
        if (testCase.Expected > 1) Assert.Same(value, invokeArgs[1]);
    }

    [Theory]
    [MemberData(
        nameof(BaseAttributesTestData.NumberAttributeBaseInvokeAndMap.InvalidCases),
        MemberType = typeof(BaseAttributesTestData.NumberAttributeBaseInvokeAndMap))]
    public void NumberAttributeBase_InvokeAndMap_ShouldThrowExpected(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new TestImplementationNumberAttribute();
        var context = new ValidationContext(new Model());
        var data = ((ThrowsCase<(string MethodName, object? Value)>)testCase).Value;

        // Act
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => attribute.TestInvokeAndMap(data.MethodName, data.Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
