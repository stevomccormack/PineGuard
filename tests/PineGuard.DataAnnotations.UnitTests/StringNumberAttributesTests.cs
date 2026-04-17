using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringNumberAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, StringNumberAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.PositiveString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.PositiveString))]
    [MemberData(nameof(StringNumberAttributesTestData.PositiveString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.PositiveString))]
    [MemberData(nameof(StringNumberAttributesTestData.PositiveString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.PositiveString))]
    public void PositiveString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new PositiveStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NegativeString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NegativeString))]
    [MemberData(nameof(StringNumberAttributesTestData.NegativeString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NegativeString))]
    [MemberData(nameof(StringNumberAttributesTestData.NegativeString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NegativeString))]
    public void NegativeString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NegativeStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.ZeroString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroString))]
    public void ZeroString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new ZeroStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NotZeroString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NotZeroString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotZeroString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NotZeroString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotZeroString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NotZeroString))]
    public void NotZeroString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NotZeroStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.EvenString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.EvenString))]
    [MemberData(nameof(StringNumberAttributesTestData.EvenString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.EvenString))]
    [MemberData(nameof(StringNumberAttributesTestData.EvenString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.EvenString))]
    public void EvenString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new EvenStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.OddString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.OddString))]
    [MemberData(nameof(StringNumberAttributesTestData.OddString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.OddString))]
    [MemberData(nameof(StringNumberAttributesTestData.OddString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.OddString))]
    public void OddString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new OddStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrPositiveString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrPositiveString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrPositiveString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrPositiveString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrPositiveString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrPositiveString))]
    public void ZeroOrPositiveString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new ZeroOrPositiveStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrNegativeString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrNegativeString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrNegativeString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrNegativeString))]
    [MemberData(nameof(StringNumberAttributesTestData.ZeroOrNegativeString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.ZeroOrNegativeString))]
    public void ZeroOrNegativeString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new ZeroOrNegativeStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.GreaterThanOrEqualString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.GreaterThanOrEqualString))]
    [MemberData(nameof(StringNumberAttributesTestData.GreaterThanOrEqualString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.GreaterThanOrEqualString))]
    [MemberData(nameof(StringNumberAttributesTestData.GreaterThanOrEqualString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.GreaterThanOrEqualString))]
    public void GreaterThanOrEqualString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new GreaterThanOrEqualStringAttribute(10), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.LessThanOrEqualString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.LessThanOrEqualString))]
    [MemberData(nameof(StringNumberAttributesTestData.LessThanOrEqualString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.LessThanOrEqualString))]
    [MemberData(nameof(StringNumberAttributesTestData.LessThanOrEqualString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.LessThanOrEqualString))]
    public void LessThanOrEqualString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new LessThanOrEqualStringAttribute(10), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.InRangeString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.InRangeString))]
    [MemberData(nameof(StringNumberAttributesTestData.InRangeString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.InRangeString))]
    [MemberData(nameof(StringNumberAttributesTestData.InRangeString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.InRangeString))]
    public void InRangeString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new InRangeStringAttribute(10, 20), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.OutOfRangeString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.OutOfRangeString))]
    [MemberData(nameof(StringNumberAttributesTestData.OutOfRangeString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.OutOfRangeString))]
    [MemberData(nameof(StringNumberAttributesTestData.OutOfRangeString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.OutOfRangeString))]
    public void OutOfRangeString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new OutOfRangeStringAttribute(10, 20), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.MultipleOfString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.MultipleOfString))]
    [MemberData(nameof(StringNumberAttributesTestData.MultipleOfString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.MultipleOfString))]
    [MemberData(nameof(StringNumberAttributesTestData.MultipleOfString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.MultipleOfString))]
    public void MultipleOfString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new MultipleOfStringAttribute(5), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NotMultipleOfString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NotMultipleOfString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotMultipleOfString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NotMultipleOfString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotMultipleOfString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NotMultipleOfString))]
    public void NotMultipleOfString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NotMultipleOfStringAttribute(5), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.ApproximatelyString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.ApproximatelyString))]
    [MemberData(nameof(StringNumberAttributesTestData.ApproximatelyString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.ApproximatelyString))]
    [MemberData(nameof(StringNumberAttributesTestData.ApproximatelyString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.ApproximatelyString))]
    public void ApproximatelyString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new ApproximatelyStringAttribute(10) { Tolerance = 1 }, testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NotApproximatelyString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NotApproximatelyString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotApproximatelyString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NotApproximatelyString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotApproximatelyString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NotApproximatelyString))]
    public void NotApproximatelyString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NotApproximatelyStringAttribute(10) { Tolerance = 1 }, testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.FiniteString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.FiniteString))]
    [MemberData(nameof(StringNumberAttributesTestData.FiniteString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.FiniteString))]
    [MemberData(nameof(StringNumberAttributesTestData.FiniteString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.FiniteString))]
    public void FiniteString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new FiniteStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NotFiniteString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NotFiniteString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotFiniteString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NotFiniteString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotFiniteString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NotFiniteString))]
    public void NotFiniteString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NotFiniteStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumberAttributesTestData.NotNaNString.ValidCases), MemberType = typeof(StringNumberAttributesTestData.NotNaNString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotNaNString.EdgeCases), MemberType = typeof(StringNumberAttributesTestData.NotNaNString))]
    [MemberData(nameof(StringNumberAttributesTestData.NotNaNString.InvalidCases), MemberType = typeof(StringNumberAttributesTestData.NotNaNString))]
    public void NotNaNString_ShouldReturnExpected(StringNumberAttributesTestData.ValidCase testCase)
        => Verify(new NotNaNStringAttribute(), testCase);
}
