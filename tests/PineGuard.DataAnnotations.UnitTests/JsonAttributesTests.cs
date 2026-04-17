using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class JsonAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(JsonAttributesTestData.Json.Cases), MemberType = typeof(JsonAttributesTestData.Json))]
    public void Json_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new JsonAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(JsonAttributesTestData.JsonObject.Cases), MemberType = typeof(JsonAttributesTestData.JsonObject))]
    public void JsonObject_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new JsonObjectAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(JsonAttributesTestData.JsonArray.Cases), MemberType = typeof(JsonAttributesTestData.JsonArray))]
    public void JsonArray_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new JsonArrayAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
