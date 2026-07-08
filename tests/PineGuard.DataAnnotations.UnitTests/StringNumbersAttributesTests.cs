using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringNumbersAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, StringNumbersAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.PositiveString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.PositiveString))]
    [MemberData(nameof(StringNumbersAttributesTestData.PositiveString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.PositiveString))]
    [MemberData(nameof(StringNumbersAttributesTestData.PositiveString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.PositiveString))]
    public void PositiveString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new PositiveStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NegativeString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NegativeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NegativeString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NegativeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NegativeString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NegativeString))]
    public void NegativeString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NegativeStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroString))]
    public void ZeroString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new ZeroStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NotZeroString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NotZeroString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotZeroString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NotZeroString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotZeroString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NotZeroString))]
    public void NotZeroString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NotZeroStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.EvenString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.EvenString))]
    [MemberData(nameof(StringNumbersAttributesTestData.EvenString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.EvenString))]
    [MemberData(nameof(StringNumbersAttributesTestData.EvenString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.EvenString))]
    public void EvenString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new EvenStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.OddString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.OddString))]
    [MemberData(nameof(StringNumbersAttributesTestData.OddString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.OddString))]
    [MemberData(nameof(StringNumbersAttributesTestData.OddString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.OddString))]
    public void OddString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new OddStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrPositiveString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrPositiveString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrPositiveString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrPositiveString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrPositiveString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrPositiveString))]
    public void ZeroOrPositiveString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new ZeroOrPositiveStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrNegativeString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrNegativeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrNegativeString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrNegativeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ZeroOrNegativeString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.ZeroOrNegativeString))]
    public void ZeroOrNegativeString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new ZeroOrNegativeStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.GreaterThanOrEqualString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.GreaterThanOrEqualString))]
    [MemberData(nameof(StringNumbersAttributesTestData.GreaterThanOrEqualString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.GreaterThanOrEqualString))]
    [MemberData(nameof(StringNumbersAttributesTestData.GreaterThanOrEqualString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.GreaterThanOrEqualString))]
    public void GreaterThanOrEqualString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new GreaterThanOrEqualStringAttribute(10), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.LessThanOrEqualString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.LessThanOrEqualString))]
    [MemberData(nameof(StringNumbersAttributesTestData.LessThanOrEqualString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.LessThanOrEqualString))]
    [MemberData(nameof(StringNumbersAttributesTestData.LessThanOrEqualString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.LessThanOrEqualString))]
    public void LessThanOrEqualString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new LessThanOrEqualStringAttribute(10), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.InRangeString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.InRangeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.InRangeString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.InRangeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.InRangeString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.InRangeString))]
    public void InRangeString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new InRangeStringAttribute(10, 20), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.OutOfRangeString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.OutOfRangeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.OutOfRangeString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.OutOfRangeString))]
    [MemberData(nameof(StringNumbersAttributesTestData.OutOfRangeString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.OutOfRangeString))]
    public void OutOfRangeString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new OutOfRangeStringAttribute(10, 20), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.MultipleOfString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.MultipleOfString))]
    [MemberData(nameof(StringNumbersAttributesTestData.MultipleOfString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.MultipleOfString))]
    [MemberData(nameof(StringNumbersAttributesTestData.MultipleOfString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.MultipleOfString))]
    public void MultipleOfString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new MultipleOfStringAttribute(5), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NotMultipleOfString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NotMultipleOfString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotMultipleOfString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NotMultipleOfString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotMultipleOfString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NotMultipleOfString))]
    public void NotMultipleOfString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NotMultipleOfStringAttribute(5), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.ApproximatelyString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.ApproximatelyString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ApproximatelyString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.ApproximatelyString))]
    [MemberData(nameof(StringNumbersAttributesTestData.ApproximatelyString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.ApproximatelyString))]
    public void ApproximatelyString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new ApproximatelyStringAttribute(10) { Tolerance = 1 }, testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NotApproximatelyString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NotApproximatelyString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotApproximatelyString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NotApproximatelyString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotApproximatelyString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NotApproximatelyString))]
    public void NotApproximatelyString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NotApproximatelyStringAttribute(10) { Tolerance = 1 }, testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.FiniteString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.FiniteString))]
    [MemberData(nameof(StringNumbersAttributesTestData.FiniteString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.FiniteString))]
    [MemberData(nameof(StringNumbersAttributesTestData.FiniteString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.FiniteString))]
    public void FiniteString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new FiniteStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NotFiniteString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NotFiniteString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotFiniteString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NotFiniteString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotFiniteString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NotFiniteString))]
    public void NotFiniteString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NotFiniteStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringNumbersAttributesTestData.NotNaNString.ValidCases), MemberType = typeof(StringNumbersAttributesTestData.NotNaNString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotNaNString.EdgeCases), MemberType = typeof(StringNumbersAttributesTestData.NotNaNString))]
    [MemberData(nameof(StringNumbersAttributesTestData.NotNaNString.InvalidCases), MemberType = typeof(StringNumbersAttributesTestData.NotNaNString))]
    public void NotNaNString_ShouldReturnExpected(StringNumbersAttributesTestData.ValidCase testCase)
        => Verify(new NotNaNStringAttribute(), testCase);
}
