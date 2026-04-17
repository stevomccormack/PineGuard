using PineGuard.GuardClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public sealed class GuardTests : BaseUnitTest
{
    public static class Against
    {
        [Theory]
        [MemberData(nameof(GuardTestData.Against.ValidCases), MemberType = typeof(GuardTestData.Against))]
        public static void ReturnsSingletonClause(GuardTestData.Against.ValidCase testCase)
        {
            // Act
            var first = Guard.Against;
            var second = Guard.Against;
            var result = ReferenceEquals(first, second);

            // Assert
            Assert.Equal(testCase.Expected, result);
        }
    }
}
