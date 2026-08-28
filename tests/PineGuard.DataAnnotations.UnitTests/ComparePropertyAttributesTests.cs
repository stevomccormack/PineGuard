using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class ComparePropertyAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterDateOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterDateOnly))]
    public void AfterDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterPropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterTimeOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterTimeOnly))]
    public void AfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterPropertyAttribute(nameof(TemporalCompareModel.TimeOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterDateTime.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterDateTime))]
    public void AfterDateTime_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterPropertyAttribute(nameof(TemporalCompareModel.DateTimeOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterDateTimeOffset.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterDateTimeOffset))]
    public void AfterDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterPropertyAttribute(nameof(TemporalCompareModel.DateTimeOffsetOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterFieldOther.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterFieldOther))]
    public void AfterFieldOther_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterPropertyAttribute(nameof(TemporalCompareModel.FieldDateTimeOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterUnsupportedComparison))]
    public void After_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.AfterMissingMember.Cases), MemberType = typeof(ComparePropertyAttributesTestData.AfterMissingMember))]
    public void After_MissingMember_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrAfterDateOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrAfterDateOnly))]
    public void OnOrAfterDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterPropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrAfterTimeOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrAfterTimeOnly))]
    public void OnOrAfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterPropertyAttribute(nameof(TemporalCompareModel.TimeOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrAfter.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterPropertyAttribute(nameof(TemporalCompareModel.DateTimeOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrAfterDateTimeOffset.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrAfterDateTimeOffset))]
    public void OnOrAfterDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterPropertyAttribute(nameof(TemporalCompareModel.DateTimeOffsetOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrAfterUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrAfterUnsupportedComparison))]
    public void OnOrAfter_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.Before.Cases), MemberType = typeof(ComparePropertyAttributesTestData.Before))]
    public void Before_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforePropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.BeforeTimeOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.BeforeTimeOnly))]
    public void BeforeTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforePropertyAttribute(nameof(TemporalCompareModel.TimeOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.BeforeDateTime.Cases), MemberType = typeof(ComparePropertyAttributesTestData.BeforeDateTime))]
    public void BeforeDateTime_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforePropertyAttribute(nameof(TemporalCompareModel.DateTimeOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.BeforeDateTimeOffset.Cases), MemberType = typeof(ComparePropertyAttributesTestData.BeforeDateTimeOffset))]
    public void BeforeDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforePropertyAttribute(nameof(TemporalCompareModel.DateTimeOffsetOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.BeforeUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.BeforeUnsupportedComparison))]
    public void Before_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrBeforeDateOnly.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrBeforeDateOnly))]
    public void OnOrBeforeDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforePropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrBefore.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforePropertyAttribute(nameof(TemporalCompareModel.TimeOnlyOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrBeforeDateTime.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrBeforeDateTime))]
    public void OnOrBeforeDateTime_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforePropertyAttribute(nameof(TemporalCompareModel.DateTimeOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrBeforeDateTimeOffset.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrBeforeDateTimeOffset))]
    public void OnOrBeforeDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforePropertyAttribute(nameof(TemporalCompareModel.DateTimeOffsetOther));
        var ctx = new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.OnOrBeforeUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.OnOrBeforeUnsupportedComparison))]
    public void OnOrBefore_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.GreaterThan.Cases), MemberType = typeof(ComparePropertyAttributesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new GreaterThanPropertyAttribute(nameof(NumericCompareModel.IntOther));
        var ctx = new ValidationContext(new NumericCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.GreaterThanUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.GreaterThanUnsupportedComparison))]
    public void GreaterThan_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.GreaterThanOrEqual.Cases), MemberType = typeof(ComparePropertyAttributesTestData.GreaterThanOrEqual))]
    public void GreaterThanOrEqual_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new GreaterThanOrEqualPropertyAttribute(nameof(NumericCompareModel.IntOther));
        var ctx = new ValidationContext(new NumericCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.GreaterThanOrEqualUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.GreaterThanOrEqualUnsupportedComparison))]
    public void GreaterThanOrEqual_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.LessThan.Cases), MemberType = typeof(ComparePropertyAttributesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LessThanPropertyAttribute(nameof(NumericCompareModel.IntOther));
        var ctx = new ValidationContext(new NumericCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.LessThanUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.LessThanUnsupportedComparison))]
    public void LessThan_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.LessThanOrEqual.Cases), MemberType = typeof(ComparePropertyAttributesTestData.LessThanOrEqual))]
    public void LessThanOrEqual_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LessThanOrEqualPropertyAttribute(nameof(NumericCompareModel.IntOther));
        var ctx = new ValidationContext(new NumericCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.LessThanOrEqualUnsupportedComparison.Cases), MemberType = typeof(ComparePropertyAttributesTestData.LessThanOrEqualUnsupportedComparison))]
    public void LessThanOrEqual_UnsupportedComparison_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.EqualTo.Cases), MemberType = typeof(ComparePropertyAttributesTestData.EqualTo))]
    public void EqualTo_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new EqualToPropertyAttribute(nameof(EqualityCompareModel.StringOther));
        var ctx = new ValidationContext(new EqualityCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.EqualToTypeMismatch.Cases), MemberType = typeof(ComparePropertyAttributesTestData.EqualToTypeMismatch))]
    public void EqualTo_TypeMismatch_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.NotEqualTo.Cases), MemberType = typeof(ComparePropertyAttributesTestData.NotEqualTo))]
    public void NotEqualTo_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotEqualToPropertyAttribute(nameof(EqualityCompareModel.StringOther));
        var ctx = new ValidationContext(new EqualityCompareModel()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ComparePropertyAttributesTestData.NotEqualToTypeMismatch.Cases), MemberType = typeof(ComparePropertyAttributesTestData.NotEqualToTypeMismatch))]
    public void NotEqualTo_TypeMismatch_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ThrowsCase<Action>)tc).Value;

        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, action);

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
