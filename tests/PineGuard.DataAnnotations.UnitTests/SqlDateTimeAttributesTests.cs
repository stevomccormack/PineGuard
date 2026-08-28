using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class SqlDateTimeAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(SqlDateTimeAttributesTestData.InSqlDateRangeTypeMismatch.Cases), MemberType = typeof(SqlDateTimeAttributesTestData.InSqlDateRangeTypeMismatch))]
    public void InSqlDateRange_TypeMismatch_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeAttributesTestData.InSqlDateRange.Cases), MemberType = typeof(SqlDateTimeAttributesTestData.InSqlDateRange))]
    public void InSqlDateRange_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new InSqlDateRangeAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeDateTime.Cases), MemberType = typeof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeDateTime))]
    public void InSqlDateTimeRange_DateTime_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new InSqlDateTimeRangeAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeDateTimeOffset.Cases), MemberType = typeof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeDateTimeOffset))]
    public void InSqlDateTimeRange_DateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new InSqlDateTimeRangeAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeWrongType.Cases), MemberType = typeof(SqlDateTimeAttributesTestData.InSqlDateTimeRangeWrongType))]
    public void InSqlDateTimeRange_WrongType_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
