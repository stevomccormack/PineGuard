namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class BaseCasesTests : BaseUnitTest
{
    public static class BaseCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.BaseCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.BaseCaseOps))]
        [MemberData(nameof(BaseCasesTestData.BaseCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.BaseCaseOps))]
        public static void ToString_ReturnsName(BaseCasesTestData.BaseCaseOps.Case testCase)
        {
            var record = new BaseCasesTestData.ConcreteReturnCase(testCase.Value, "x", false);

            Assert.Equal(testCase.Expected, record.ToString());
        }
    }

    public static class ReturnCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.ReturnCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.ReturnCaseOps))]
        [MemberData(nameof(BaseCasesTestData.ReturnCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.ReturnCaseOps))]
        public static void ShouldExposeValueAndExpected(BaseCasesTestData.ReturnCaseOps.Case testCase)
        {
            var (value, expected) = testCase.Value;
            var record = new BaseCasesTestData.ConcreteReturnCase("case", value, expected);

            Assert.Equal(value, record.Value);
            Assert.Equal(expected, record.Expected);
        }
    }

    public static class ReturnOutCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.ReturnOutCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.ReturnOutCaseOps))]
        [MemberData(nameof(BaseCasesTestData.ReturnOutCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.ReturnOutCaseOps))]
        public static void ShouldExposeValueReturnAndOutValue(BaseCasesTestData.ReturnOutCaseOps.Case testCase)
        {
            var (value, expected, expectedOutValue) = testCase.Value;
            var record = new BaseCasesTestData.ConcreteReturnOutCase("case", value, expected, expectedOutValue);

            Assert.Equal(value, record.Value);
            Assert.Equal(expected, record.Expected);
            Assert.Equal(expectedOutValue, record.ExpectedOutValue);
        }
    }

    public static class IsCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.IsCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.IsCaseOps))]
        [MemberData(nameof(BaseCasesTestData.IsCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.IsCaseOps))]
        public static void ShouldExposeValueAndExpected(BaseCasesTestData.IsCaseOps.Case testCase)
        {
            var (value, expected) = testCase.Value;
            var record = new BaseCasesTestData.ConcreteIsCase("case", value, expected);

            Assert.Equal(value, record.Value);
            Assert.Equal(expected, record.Expected);
        }
    }

    public static class HasCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.HasCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.HasCaseOps))]
        [MemberData(nameof(BaseCasesTestData.HasCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.HasCaseOps))]
        public static void ShouldExposeValueAndExpected(BaseCasesTestData.HasCaseOps.Case testCase)
        {
            var (value, expected) = testCase.Value;
            var record = new BaseCasesTestData.ConcreteHasCase("case", value, expected);

            Assert.Equal(value, record.Value);
            Assert.Equal(expected, record.Expected);
        }
    }

    public static class TryCaseOps
    {
        [Theory]
        [MemberData(nameof(BaseCasesTestData.TryCaseOps.ValidCases), MemberType = typeof(BaseCasesTestData.TryCaseOps))]
        [MemberData(nameof(BaseCasesTestData.TryCaseOps.EdgeCases), MemberType = typeof(BaseCasesTestData.TryCaseOps))]
        public static void ShouldExposeValueReturnAndOutValue(BaseCasesTestData.TryCaseOps.Case testCase)
        {
            var (value, expected, expectedOutValue) = testCase.Value;
            var record = new BaseCasesTestData.ConcreteTryCase("case", value, expected, expectedOutValue);

            Assert.Equal(value, record.Value);
            Assert.Equal(expected, record.Expected);
            Assert.Equal(expectedOutValue, record.ExpectedOutValue);
        }
    }
}
