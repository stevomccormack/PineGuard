using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class ThrowHelperTests : BaseUnitTest
{
    public static class ThrowIfNull
    {
        [Theory]
        [MemberData(nameof(ThrowHelperTestData.ThrowIfNull.ValidCases), MemberType = typeof(ThrowHelperTestData.ThrowIfNull))]
        public static void ValidAndEdge_BehavesAsExpected(ThrowHelperTestData.ThrowIfNull.ValidCase testCase)
        {
            // Arrange
            var argument = testCase.Value;

            // Act
            var exception = Record.Exception(() => ThrowHelper.ThrowIfNull(argument));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [MemberData(nameof(ThrowHelperTestData.ThrowIfNull.InvalidCases), MemberType = typeof(ThrowHelperTestData.ThrowIfNull))]
        public static void Invalid_ThrowsAsExpected(IThrowsCase testCase)
        {
            // Arrange
            var invalidCase = (ThrowHelperTestData.ThrowIfNull.InvalidCase)testCase;
            var argument = invalidCase.Value;

            // Act
            var exception = Assert.Throws(testCase.ExpectedException.Type, () => ThrowHelper.ThrowIfNull(argument));

            // Assert
            ThrowsCaseAssert.Expected(exception, testCase);
        }
    }

    public static class ThrowIfNullExplicitParamName
    {
        [Theory]
        [MemberData(nameof(ThrowHelperTestData.ThrowIfNullExplicitParamName.ValidCases), MemberType = typeof(ThrowHelperTestData.ThrowIfNullExplicitParamName))]
        public static void ValidAndEdge_BehavesAsExpected(ThrowHelperTestData.ThrowIfNullExplicitParamName.ValidCase testCase)
        {
            // Arrange
            var (argument, paramName) = testCase.Value;

            // Act
            var exception = Record.Exception(() => ThrowHelper.ThrowIfNull(argument, paramName));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [MemberData(nameof(ThrowHelperTestData.ThrowIfNullExplicitParamName.InvalidCases), MemberType = typeof(ThrowHelperTestData.ThrowIfNullExplicitParamName))]
        public static void Invalid_ThrowsAsExpected(IThrowsCase testCase)
        {
            // Arrange
            var invalidCase = (ThrowHelperTestData.ThrowIfNullExplicitParamName.InvalidCase)testCase;
            var (argument, paramName) = invalidCase.Value;

            // Act
            var exception = Assert.Throws(testCase.ExpectedException.Type, () => ThrowHelper.ThrowIfNull(argument, paramName));

            // Assert
            ThrowsCaseAssert.Expected(exception, testCase);
        }
    }
}
