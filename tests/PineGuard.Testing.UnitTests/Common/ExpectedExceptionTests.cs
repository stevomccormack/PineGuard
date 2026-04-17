using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Common;

public sealed class ExpectedExceptionTests : BaseUnitTest
{
    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(ExpectedExceptionTestData.Constructor.ValidCases), MemberType = typeof(ExpectedExceptionTestData.Constructor))]
        public static void ShouldConstructWithExpectedValues(ExpectedExceptionTestData.Constructor.Case testCase)
        {
            var (type, paramName, messageContains) = testCase.Value;

            var result = new ExpectedException(type, paramName, messageContains);

            Assert.Equal(testCase.ExpectedType, result.Type);
            Assert.Equal(testCase.ExpectedParamName, result.ParamName);
            Assert.Equal(testCase.ExpectedMessageContains, result.MessageContains);
        }
    }
}
