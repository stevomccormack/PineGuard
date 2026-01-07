using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class SqlDateTimeUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateTimeValidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateTime_ReturnsTrue_ForValidDateTime(DateTime? value)
    {
        Assert.True(SqlDateTimeUtility.TryCreateSqlDateTime(value, out var sqlDateTime));
        Assert.Equal(value!.Value, sqlDateTime);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateTimeInvalidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateTime_ReturnsFalse_ForInvalidDateTime(DateTime? value)
    {
        Assert.False(SqlDateTimeUtility.TryCreateSqlDateTime(value, out var sqlDateTime));
        Assert.Equal(default, sqlDateTime);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateTimeOffsetValidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateTime_ReturnsTrue_ForValidDateTimeOffset(DateTimeOffset? value)
    {
        Assert.True(SqlDateTimeUtility.TryCreateSqlDateTime(value, out var sqlDateTime));
        Assert.Equal(value!.Value, sqlDateTime);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateTimeOffsetInvalidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateTime_ReturnsFalse_ForInvalidDateTimeOffset(DateTimeOffset? value)
    {
        Assert.False(SqlDateTimeUtility.TryCreateSqlDateTime(value, out var sqlDateTime));
        Assert.Equal(default, sqlDateTime);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateOnlyValidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateOnly_ReturnsTrue_ForValidDateOnly(DateOnly? value)
    {
        Assert.True(SqlDateTimeUtility.TryCreateSqlDateOnly(value, out var sqlDate));
        Assert.Equal(value!.Value, sqlDate);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeUtilityTestData.SqlDateOnlyInvalidCases), MemberType = typeof(SqlDateTimeUtilityTestData))]
    public void TryCreateSqlDateOnly_ReturnsFalse_ForInvalidDateOnly(DateOnly? value)
    {
        Assert.False(SqlDateTimeUtility.TryCreateSqlDateOnly(value, out var sqlDate));
        Assert.Equal(default, sqlDate);
    }
}
