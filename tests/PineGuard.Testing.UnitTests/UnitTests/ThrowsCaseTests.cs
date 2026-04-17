namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class ThrowsCaseTests : BaseUnitTest
{
    public static class ConstructorWithExpectedException
    {
        [Theory]
        [MemberData(nameof(ThrowsCaseTestData.ConstructorWithExpectedException.ValidCases), MemberType = typeof(ThrowsCaseTestData.ConstructorWithExpectedException))]
        public static void ShouldStoreExpectedException(ThrowsCaseTestData.ConstructorWithExpectedException.Case testCase)
        {
            var (value, expectedException) = testCase.Value;
            var record = new ThrowsCaseTestData.ConcreteThrowsCase("case", value, expectedException);

            Assert.Equal(value, record.Value);
            Assert.Equal(expectedException, record.ExpectedException);
        }
    }

    public static class ConstructorTypeOnly
    {
        [Theory]
        [MemberData(nameof(ThrowsCaseTestData.ConstructorTypeOnly.ValidCases), MemberType = typeof(ThrowsCaseTestData.ConstructorTypeOnly))]
        public static void ShouldCreateExpectedExceptionFromType(ThrowsCaseTestData.ConstructorTypeOnly.Case testCase)
        {
            var (value, exType) = testCase.Value;
            var record = new ThrowsCaseTestData.ConcreteThrowsCaseTypeOnly("case", value, exType);

            Assert.Equal(exType, record.ExpectedException.Type);
            Assert.Null(record.ExpectedException.ParamName);
            Assert.Null(record.ExpectedException.MessageContains);
        }
    }

    public static class ConstructorTypeAndParam
    {
        [Theory]
        [MemberData(nameof(ThrowsCaseTestData.ConstructorTypeAndParam.ValidCases), MemberType = typeof(ThrowsCaseTestData.ConstructorTypeAndParam))]
        public static void ShouldCreateExpectedExceptionWithParamName(ThrowsCaseTestData.ConstructorTypeAndParam.Case testCase)
        {
            var (value, exType, paramName) = testCase.Value;
            var record = new ThrowsCaseTestData.ConcreteThrowsCaseTypeParam("case", value, exType, paramName);

            Assert.Equal(exType, record.ExpectedException.Type);
            Assert.Equal(paramName, record.ExpectedException.ParamName);
            Assert.Null(record.ExpectedException.MessageContains);
        }
    }

    public static class ConstructorTypeFull
    {
        [Theory]
        [MemberData(nameof(ThrowsCaseTestData.ConstructorTypeFull.ValidCases), MemberType = typeof(ThrowsCaseTestData.ConstructorTypeFull))]
        public static void ShouldCreateExpectedExceptionWithAllFields(ThrowsCaseTestData.ConstructorTypeFull.Case testCase)
        {
            var (value, exType, paramName, messageContains) = testCase.Value;
            var record = new ThrowsCaseTestData.ConcreteThrowsCaseTypeFull("case", value, exType, paramName, messageContains);

            Assert.Equal(exType, record.ExpectedException.Type);
            Assert.Equal(paramName, record.ExpectedException.ParamName);
            Assert.Equal(messageContains, record.ExpectedException.MessageContains);
        }
    }
}
