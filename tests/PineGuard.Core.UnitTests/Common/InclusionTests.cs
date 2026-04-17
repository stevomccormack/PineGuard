using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class InclusionTests : BaseUnitTest
{
    public sealed class DefinedValues : BaseUnitTest
    {
        [Theory]
        [MemberData(nameof(InclusionTestData.DefinedValues.ValidCases), MemberType = typeof(InclusionTestData.DefinedValues))]
        public void Valid_BehavesAsExpected(InclusionTestData.DefinedValues.Case testCase)
        {
            // Act
            var intValue = (int)testCase.Inclusion;

            // Assert
            Assert.Equal(testCase.ExpectedIntValue, intValue);
        }

        [Theory]
        [MemberData(nameof(InclusionTestData.DefinedValues.ValidCases), MemberType = typeof(InclusionTestData.DefinedValues))]
        public void ToString_ReturnsMemberName(InclusionTestData.DefinedValues.Case testCase)
        {
            // Act
            var s = testCase.Inclusion.ToString();

            // Assert
            Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
            Assert.False(string.IsNullOrWhiteSpace(s));
        }
    }

    public sealed class UndefinedValues : BaseUnitTest
    {
        [Theory]
        [MemberData(nameof(InclusionTestData.UndefinedValues.EdgeCases), MemberType = typeof(InclusionTestData.UndefinedValues))]
        public void Edge_BehavesAsExpected(InclusionTestData.UndefinedValues.Case testCase)
        {
            // Act
            var equalsInclusive = testCase.Inclusion == Inclusion.Inclusive;
            var equalsExclusive = testCase.Inclusion == Inclusion.Exclusive;

            // Assert
            Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
            Assert.False(equalsInclusive || equalsExclusive);
        }

        [Theory]
        [MemberData(nameof(InclusionTestData.UndefinedValues.EdgeCases), MemberType = typeof(InclusionTestData.UndefinedValues))]
        public void ToString_ReturnsNumeric(InclusionTestData.UndefinedValues.Case testCase)
        {
            // Act
            var s = testCase.Inclusion.ToString();

            // Assert
            Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
            Assert.Equal(testCase.ExpectedIntValue.ToString(), s);
        }
    }
}
