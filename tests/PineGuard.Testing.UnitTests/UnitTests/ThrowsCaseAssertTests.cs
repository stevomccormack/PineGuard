using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class ThrowsCaseAssertTests : BaseUnitTest
{
    public static class Expected
    {
        [Theory]
        [MemberData(nameof(ThrowsCaseAssertTestData.Expected.ValidCases), MemberType = typeof(ThrowsCaseAssertTestData.Expected))]
        [MemberData(nameof(ThrowsCaseAssertTestData.Expected.EdgeCases), MemberType = typeof(ThrowsCaseAssertTestData.Expected))]
        public static void ShouldPassWhenExpectationsMet(ThrowsCaseAssertTestData.Expected.ValidCase testCase)
        {
            var (ex, throwsCase) = testCase.Value;

            ThrowsCaseAssert.Expected(ex, throwsCase);
        }

        [Theory]
        [MemberData(nameof(ThrowsCaseAssertTestData.Expected.InvalidCases), MemberType = typeof(ThrowsCaseAssertTestData.Expected))]
        public static void ShouldThrowWhenExpectationsViolated(IThrowsCase testCase)
        {
            var t = (ThrowsCaseAssertTestData.Expected.InvalidCase)testCase;
            var ex = Assert.Throws(t.ExpectedException.Type, t.Value);
            ThrowsCaseAssert.Expected(ex, t);
        }
    }
}
