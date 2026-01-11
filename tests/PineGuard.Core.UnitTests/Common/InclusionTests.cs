using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class InclusionTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(InclusionTestData.DefinedValues.ValidCases), MemberType = typeof(InclusionTestData.DefinedValues))]
    public void DefinedValues_AreStable(InclusionTestData.DefinedValues.Case testCase)
    {
        // Act
        var intValue = (int)testCase.Inclusion;

        // Assert
        Assert.Equal(testCase.ExpectedIntValue, intValue);
    }

    [Theory]
    [MemberData(nameof(InclusionTestData.UndefinedValues.EdgeCases), MemberType = typeof(InclusionTestData.UndefinedValues))]
    public void UndefinedValues_AreNotEqualToDefinedMembers(InclusionTestData.UndefinedValues.Case testCase)
    {
        // Act
        var equalsInclusive = testCase.Inclusion == Inclusion.Inclusive;
        var equalsExclusive = testCase.Inclusion == Inclusion.Exclusive;

        // Assert
        Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
        Assert.False(equalsInclusive || equalsExclusive);
    }

    [Theory]
    [MemberData(nameof(InclusionTestData.DefinedValues.ValidCases), MemberType = typeof(InclusionTestData.DefinedValues))]
    public void ToString_ReturnsMemberName_ForDefinedValues(InclusionTestData.DefinedValues.Case testCase)
    {
        // Act
        var s = testCase.Inclusion.ToString();

        // Assert
        Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
        Assert.False(string.IsNullOrWhiteSpace(s));
    }

    [Theory]
    [MemberData(nameof(InclusionTestData.UndefinedValues.EdgeCases), MemberType = typeof(InclusionTestData.UndefinedValues))]
    public void ToString_ReturnsNumeric_ForUndefinedValues(InclusionTestData.UndefinedValues.Case testCase)
    {
        // Act
        var s = testCase.Inclusion.ToString();

        // Assert
        Assert.Equal(testCase.ExpectedIntValue, (int)testCase.Inclusion);
        Assert.Equal(testCase.ExpectedIntValue.ToString(), s);
    }
}
