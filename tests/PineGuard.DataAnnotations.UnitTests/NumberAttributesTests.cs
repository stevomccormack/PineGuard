using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

#pragma warning disable CS0618
public sealed class NumberAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private static void Verify<TAttribute>(TAttribute attribute, NumberAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Positive.ValidCases), MemberType = typeof(NumberAttributesTestData.Positive))]
    [MemberData(nameof(NumberAttributesTestData.Positive.EdgeCases), MemberType = typeof(NumberAttributesTestData.Positive))]
    [MemberData(nameof(NumberAttributesTestData.Positive.InvalidCases), MemberType = typeof(NumberAttributesTestData.Positive))]
    public void PositiveNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new PositiveNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.NotPositive, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Negative.ValidCases), MemberType = typeof(NumberAttributesTestData.Negative))]
    [MemberData(nameof(NumberAttributesTestData.Negative.EdgeCases), MemberType = typeof(NumberAttributesTestData.Negative))]
    [MemberData(nameof(NumberAttributesTestData.Negative.InvalidCases), MemberType = typeof(NumberAttributesTestData.Negative))]
    public void NegativeNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NegativeNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.NotNegative, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Zero.ValidCases), MemberType = typeof(NumberAttributesTestData.Zero))]
    [MemberData(nameof(NumberAttributesTestData.Zero.EdgeCases), MemberType = typeof(NumberAttributesTestData.Zero))]
    [MemberData(nameof(NumberAttributesTestData.Zero.InvalidCases), MemberType = typeof(NumberAttributesTestData.Zero))]
    public void ZeroNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new ZeroNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.NotZero, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotZero.ValidCases), MemberType = typeof(NumberAttributesTestData.NotZero))]
    [MemberData(nameof(NumberAttributesTestData.NotZero.EdgeCases), MemberType = typeof(NumberAttributesTestData.NotZero))]
    [MemberData(nameof(NumberAttributesTestData.NotZero.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotZero))]
    public void NotZeroNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotZeroNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.Zero, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrPositive.ValidCases), MemberType = typeof(NumberAttributesTestData.ZeroOrPositive))]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrPositive.EdgeCases), MemberType = typeof(NumberAttributesTestData.ZeroOrPositive))]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrPositive.InvalidCases), MemberType = typeof(NumberAttributesTestData.ZeroOrPositive))]
    public void ZeroOrPositiveNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new ZeroOrPositiveNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.Negative, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrNegative.ValidCases), MemberType = typeof(NumberAttributesTestData.ZeroOrNegative))]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrNegative.EdgeCases), MemberType = typeof(NumberAttributesTestData.ZeroOrNegative))]
    [MemberData(nameof(NumberAttributesTestData.ZeroOrNegative.InvalidCases), MemberType = typeof(NumberAttributesTestData.ZeroOrNegative))]
    public void ZeroOrNegativeNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new ZeroOrNegativeNumberAttribute();
        Assert.Equal(MustCodes.Number.Sign.Positive, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Even.ValidCases), MemberType = typeof(NumberAttributesTestData.Even))]
    [MemberData(nameof(NumberAttributesTestData.Even.EdgeCases), MemberType = typeof(NumberAttributesTestData.Even))]
    [MemberData(nameof(NumberAttributesTestData.Even.InvalidCases), MemberType = typeof(NumberAttributesTestData.Even))]
    public void EvenNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new EvenNumberAttribute();
        Assert.Equal(MustCodes.Number.Parity.Odd, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Odd.ValidCases), MemberType = typeof(NumberAttributesTestData.Odd))]
    [MemberData(nameof(NumberAttributesTestData.Odd.EdgeCases), MemberType = typeof(NumberAttributesTestData.Odd))]
    [MemberData(nameof(NumberAttributesTestData.Odd.InvalidCases), MemberType = typeof(NumberAttributesTestData.Odd))]
    public void OddNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new OddNumberAttribute();
        Assert.Equal(MustCodes.Number.Parity.Even, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Finite.ValidCases), MemberType = typeof(NumberAttributesTestData.Finite))]
    [MemberData(nameof(NumberAttributesTestData.Finite.EdgeCases), MemberType = typeof(NumberAttributesTestData.Finite))]
    [MemberData(nameof(NumberAttributesTestData.Finite.InvalidCases), MemberType = typeof(NumberAttributesTestData.Finite))]
    public void FiniteNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new FiniteNumberAttribute();
        Assert.Equal(MustCodes.Number.Form.NotFinite, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotFinite.ValidCases), MemberType = typeof(NumberAttributesTestData.NotFinite))]
    [MemberData(nameof(NumberAttributesTestData.NotFinite.EdgeCases), MemberType = typeof(NumberAttributesTestData.NotFinite))]
    [MemberData(nameof(NumberAttributesTestData.NotFinite.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotFinite))]
    public void NotFiniteNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotFiniteNumberAttribute();
        Assert.Equal(MustCodes.Number.Form.Finite, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NaN.ValidCases), MemberType = typeof(NumberAttributesTestData.NaN))]
    [MemberData(nameof(NumberAttributesTestData.NaN.EdgeCases), MemberType = typeof(NumberAttributesTestData.NaN))]
    [MemberData(nameof(NumberAttributesTestData.NaN.InvalidCases), MemberType = typeof(NumberAttributesTestData.NaN))]
    public void NaNNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NaNNumberAttribute();
        Assert.Equal(MustCodes.Number.Form.NotNan, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotNaN.ValidCases), MemberType = typeof(NumberAttributesTestData.NotNaN))]
    [MemberData(nameof(NumberAttributesTestData.NotNaN.EdgeCases), MemberType = typeof(NumberAttributesTestData.NotNaN))]
    [MemberData(nameof(NumberAttributesTestData.NotNaN.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotNaN))]
    public void NotNaNNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotNaNNumberAttribute();
        Assert.Equal(MustCodes.Number.Form.Nan, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.GreaterThanOrEqual.ValidCases), MemberType = typeof(NumberAttributesTestData.GreaterThanOrEqual))]
    [MemberData(nameof(NumberAttributesTestData.GreaterThanOrEqual.EdgeCases), MemberType = typeof(NumberAttributesTestData.GreaterThanOrEqual))]
    [MemberData(nameof(NumberAttributesTestData.GreaterThanOrEqual.InvalidCases), MemberType = typeof(NumberAttributesTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqualNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new GreaterThanOrEqualNumberAttribute(10);
        Assert.Equal(MustCodes.Number.Range.BelowMinimum, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.LessThanOrEqual.ValidCases), MemberType = typeof(NumberAttributesTestData.LessThanOrEqual))]
    [MemberData(nameof(NumberAttributesTestData.LessThanOrEqual.EdgeCases), MemberType = typeof(NumberAttributesTestData.LessThanOrEqual))]
    [MemberData(nameof(NumberAttributesTestData.LessThanOrEqual.InvalidCases), MemberType = typeof(NumberAttributesTestData.LessThanOrEqual))]
    public void LessThanOrEqualNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new LessThanOrEqualNumberAttribute(10);
        Assert.Equal(MustCodes.Number.Range.Exceeded, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.InRange.ValidCases), MemberType = typeof(NumberAttributesTestData.InRange))]
    [MemberData(nameof(NumberAttributesTestData.InRange.EdgeCases), MemberType = typeof(NumberAttributesTestData.InRange))]
    [MemberData(nameof(NumberAttributesTestData.InRange.InvalidCases), MemberType = typeof(NumberAttributesTestData.InRange))]
    public void InRangeNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new InRangeNumberAttribute(10, 20);
        Assert.Equal(MustCodes.Number.Range.OutOfRange, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.OutOfRange.ValidCases), MemberType = typeof(NumberAttributesTestData.OutOfRange))]
    [MemberData(nameof(NumberAttributesTestData.OutOfRange.EdgeCases), MemberType = typeof(NumberAttributesTestData.OutOfRange))]
    [MemberData(nameof(NumberAttributesTestData.OutOfRange.InvalidCases), MemberType = typeof(NumberAttributesTestData.OutOfRange))]
    public void OutOfRangeNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new OutOfRangeNumberAttribute(10, 20);
        Assert.Equal(MustCodes.Number.Range.InRange, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.MultipleOf.ValidCases), MemberType = typeof(NumberAttributesTestData.MultipleOf))]
    [MemberData(nameof(NumberAttributesTestData.MultipleOf.EdgeCases), MemberType = typeof(NumberAttributesTestData.MultipleOf))]
    [MemberData(nameof(NumberAttributesTestData.MultipleOf.InvalidCases), MemberType = typeof(NumberAttributesTestData.MultipleOf))]
    public void MultipleOfNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new MultipleOfNumberAttribute(5);
        Assert.Equal(MustCodes.Number.Divisibility.NotMultiple, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotMultipleOf.ValidCases), MemberType = typeof(NumberAttributesTestData.NotMultipleOf))]
    [MemberData(nameof(NumberAttributesTestData.NotMultipleOf.EdgeCases), MemberType = typeof(NumberAttributesTestData.NotMultipleOf))]
    [MemberData(nameof(NumberAttributesTestData.NotMultipleOf.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotMultipleOf))]
    public void NotMultipleOfNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotMultipleOfNumberAttribute(5);
        Assert.Equal(MustCodes.Number.Divisibility.Multiple, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.Approximately.ValidCases), MemberType = typeof(NumberAttributesTestData.Approximately))]
    [MemberData(nameof(NumberAttributesTestData.Approximately.EdgeCases), MemberType = typeof(NumberAttributesTestData.Approximately))]
    [MemberData(nameof(NumberAttributesTestData.Approximately.InvalidCases), MemberType = typeof(NumberAttributesTestData.Approximately))]
    public void ApproximatelyNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new ApproximatelyNumberAttribute(10) { Tolerance = 1 };
        Assert.Equal(MustCodes.Number.Proximity.NotApproximate, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotApproximately.ValidCases), MemberType = typeof(NumberAttributesTestData.NotApproximately))]
    [MemberData(nameof(NumberAttributesTestData.NotApproximately.EdgeCases), MemberType = typeof(NumberAttributesTestData.NotApproximately))]
    [MemberData(nameof(NumberAttributesTestData.NotApproximately.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotApproximately))]
    public void NotApproximatelyNumber_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotApproximatelyNumberAttribute(10) { Tolerance = 1 };
        Assert.Equal(MustCodes.Number.Proximity.Approximate, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.ApproximatelyNoTolerance.InvalidCases), MemberType = typeof(NumberAttributesTestData.ApproximatelyNoTolerance))]
    public void ApproximatelyNumber_NoTolerance_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
        => Verify(new ApproximatelyNumberAttribute(10), testCase);

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.NotApproximatelyNoTolerance.InvalidCases), MemberType = typeof(NumberAttributesTestData.NotApproximatelyNoTolerance))]
    public void NotApproximatelyNumber_NoTolerance_BehavesAsExpected(NumberAttributesTestData.ValidCase testCase)
        => Verify(new NotApproximatelyNumberAttribute(10), testCase);

    [Theory]
    [InlineData(typeof(PositiveNumberAttribute))]
    public void NumberAttribute_WithUnsupportedType_ShouldThrow(Type attributeType)
    {
        var attribute = (ValidationAttribute)Activator.CreateInstance(attributeType)!;
        var ctx = new ValidationContext(new object());
        Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult(DateTime.Now, ctx));
    }

    [Theory]
    [MemberData(nameof(NumberAttributesTestData.BoundMismatch.Cases), MemberType = typeof(NumberAttributesTestData.BoundMismatch))]
    public void NumberAttribute_WithNonExactBound_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
