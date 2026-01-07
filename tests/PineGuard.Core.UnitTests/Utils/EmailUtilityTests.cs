using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class EmailUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(EmailUtilityTestData.TryCreate.ValidCases), MemberType = typeof(EmailUtilityTestData.TryCreate))]
    [MemberData(nameof(EmailUtilityTestData.TryCreate.EdgeCases), MemberType = typeof(EmailUtilityTestData.TryCreate))]
    public void TryCreate_ReturnsExpected(EmailUtilityTestData.TryCreate.ValidCase testCase)
    {
        var result = EmailUtility.TryCreate(testCase.Value, out var email);

        Assert.Equal(testCase.Expected, result);

        if (testCase.Expected)
        {
            Assert.NotNull(email);
        }
        else
        {
            Assert.Null(email);
        }
    }

    [Theory]
    [MemberData(nameof(EmailUtilityTestData.TryStrictCreate.ValidCases), MemberType = typeof(EmailUtilityTestData.TryStrictCreate))]
    [MemberData(nameof(EmailUtilityTestData.TryStrictCreate.EdgeCases), MemberType = typeof(EmailUtilityTestData.TryStrictCreate))]
    public void TryStrictCreate_ReturnsExpected(EmailUtilityTestData.TryStrictCreate.ValidCase testCase)
    {
        var result = EmailUtility.TryStrictCreate(testCase.Value, out var email);

        Assert.Equal(testCase.Expected, result);

        if (testCase.Expected)
        {
            Assert.NotNull(email);
            Assert.Equal(testCase.ExpectedAddress, email!.Address);
        }
        else
        {
            Assert.Null(email);
        }
    }

    [Fact]
    public void TryStrictCreate_ReturnsFalse_ForDisplayNameForm()
    {
        var result = EmailUtility.TryStrictCreate("User <user@example.com>", out var email);

        Assert.False(result);
        Assert.Null(email);
    }

    [Theory]
    [MemberData(nameof(EmailUtilityTestData.TryGetAlias.ValidCases), MemberType = typeof(EmailUtilityTestData.TryGetAlias))]
    [MemberData(nameof(EmailUtilityTestData.TryGetAlias.EdgeCases), MemberType = typeof(EmailUtilityTestData.TryGetAlias))]
    public void TryGetAlias_ReturnsExpected(EmailUtilityTestData.TryGetAlias.ValidCase testCase)
    {
        var result = EmailUtility.TryGetAlias(testCase.Value, out var alias);

        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedAlias, alias);
    }
}
