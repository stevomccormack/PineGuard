using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class AttributeNullTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class Model
    {
        [True] public bool? TrueProp { get; set; }
        [False] public bool? FalseProp { get; set; }
        [Latitude] public double? LatProp { get; set; }
        [Longitude] public double? LonProp { get; set; }
        [NotEmptyGuid] public Guid? GuidProp { get; set; }
        [SafeFileName] public string? FileProp { get; set; }
        [OddNumber] public object? OddProp { get; set; }
        [EvenNumber] public object? EvenProp { get; set; }
    }

    [Theory]
    [MemberData(nameof(AttributeNullTestData.AttributesWithNullValue.ValidCases), MemberType = typeof(AttributeNullTestData.AttributesWithNullValue))]
    [MemberData(nameof(AttributeNullTestData.AttributesWithNullValue.EdgeCases), MemberType = typeof(AttributeNullTestData.AttributesWithNullValue))]
    public void Attributes_WithNullValue_ShouldReturnExpected(AttributeNullTestData.AttributesWithNullValue.ValidCase testCase)
    {
        var context = new ValidationContext(testCase.Value);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(testCase.Value, context, results, validateAllProperties: true);
        Assert.Equal(testCase.Expected, isValid);
        if (testCase.Expected) Assert.Empty(results);
    }

    [Theory]
    [MemberData(nameof(AttributeNullTestData.OddNumberUnsupportedType.InvalidCases), MemberType = typeof(AttributeNullTestData.OddNumberUnsupportedType))]
    public void OddNumber_WithUnsupportedType_ThrowsExpected(IThrowsCase testCase)
    {
        var action = ((ValueCase<Action>)testCase).Value;
        var ex = Assert.Throws(testCase.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(AttributeNullTestData.EvenNumberUnsupportedType.InvalidCases), MemberType = typeof(AttributeNullTestData.EvenNumberUnsupportedType))]
    public void EvenNumber_WithUnsupportedType_ThrowsExpected(IThrowsCase testCase)
    {
        var action = ((ValueCase<Action>)testCase).Value;
        var ex = Assert.Throws(testCase.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
