using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustObjectClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.EqualTo.ValidCases), MemberType = typeof(MustObjectClausesTestData.EqualTo))]
    [MemberData(nameof(MustObjectClausesTestData.EqualTo.InvalidCases), MemberType = typeof(MustObjectClausesTestData.EqualTo))]
    public void EqualTo_BehavesAsExpected(MustCase<(object value, object other)> tc)
    {
        // Arrange
        var (value, other) = tc.Value;

        // Act
        var result = Must.Be.EqualTo(value, other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.NotEqualTo.ValidCases), MemberType = typeof(MustObjectClausesTestData.NotEqualTo))]
    [MemberData(nameof(MustObjectClausesTestData.NotEqualTo.InvalidCases), MemberType = typeof(MustObjectClausesTestData.NotEqualTo))]
    public void NotEqualTo_BehavesAsExpected(MustCase<(object value, object other)> tc)
    {
        // Arrange
        var (value, other) = tc.Value;

        // Act
        var result = Must.Be.NotEqualTo(value, other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.OfType.ValidCases), MemberType = typeof(MustObjectClausesTestData.OfType))]
    [MemberData(nameof(MustObjectClausesTestData.OfType.InvalidCases), MemberType = typeof(MustObjectClausesTestData.OfType))]
    public void OfType_BehavesAsExpected(MustCase<(object value, Type type)> tc)
    {
        // Arrange
        var method = typeof(MustObjectClauses).GetMethod("OfType")?.MakeGenericMethod(tc.Value.type)
            ?? throw new InvalidOperationException("Could not find OfType generic method");

        // Act
        dynamic result = method.Invoke(null, [Must.Be, tc.Value.value, null])!;

        // Assert
        Assert.Equal(tc.Expected.IsValid, (bool)result.Success);
        if (tc.Expected.Message is not null)
            Assert.Equal(tc.Expected.Message, (string)result.Message);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.NotOfType.ValidCases), MemberType = typeof(MustObjectClausesTestData.NotOfType))]
    [MemberData(nameof(MustObjectClausesTestData.NotOfType.InvalidCases), MemberType = typeof(MustObjectClausesTestData.NotOfType))]
    public void NotOfType_BehavesAsExpected(MustCase<(object value, Type type)> tc)
    {
        // Arrange
        var method = typeof(MustObjectClauses).GetMethod("NotOfType")?.MakeGenericMethod(tc.Value.type)
            ?? throw new InvalidOperationException("Could not find NotOfType generic method");

        // Act
        dynamic result = method.Invoke(null, [Must.Be, tc.Value.value, null])!;

        // Assert
        Assert.Equal(tc.Expected.IsValid, (bool)result.Success);
        if (tc.Expected.Message is not null)
            Assert.Equal(tc.Expected.Message, (string)result.Message);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.AssignableToType.ValidCases), MemberType = typeof(MustObjectClausesTestData.AssignableToType))]
    [MemberData(nameof(MustObjectClausesTestData.AssignableToType.InvalidCases), MemberType = typeof(MustObjectClausesTestData.AssignableToType))]
    public void AssignableToType_BehavesAsExpected(MustCase<(object value, Type type)> tc)
    {
        // Arrange
        var method = typeof(MustObjectClauses).GetMethod("AssignableToType")?.MakeGenericMethod(tc.Value.type)
            ?? throw new InvalidOperationException("Could not find AssignableToType generic method");

        // Act
        dynamic result = method.Invoke(null, [Must.Be, tc.Value.value, null])!;

        // Assert
        Assert.Equal(tc.Expected.IsValid, (bool)result.Success);
        if (tc.Expected.Message is not null)
            Assert.Equal(tc.Expected.Message, (string)result.Message);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.NotAssignableToType.ValidCases), MemberType = typeof(MustObjectClausesTestData.NotAssignableToType))]
    [MemberData(nameof(MustObjectClausesTestData.NotAssignableToType.InvalidCases), MemberType = typeof(MustObjectClausesTestData.NotAssignableToType))]
    public void NotAssignableToType_BehavesAsExpected(MustCase<(object value, Type type)> tc)
    {
        // Arrange
        var method = typeof(MustObjectClauses).GetMethod("NotAssignableToType")?.MakeGenericMethod(tc.Value.type)
            ?? throw new InvalidOperationException("Could not find NotAssignableToType generic method");

        // Act
        dynamic result = method.Invoke(null, [Must.Be, tc.Value.value, null])!;

        // Assert
        Assert.Equal(tc.Expected.IsValid, (bool)result.Success);
        if (tc.Expected.Message is not null)
            Assert.Equal(tc.Expected.Message, (string)result.Message);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.SameReferenceAs.ValidCases), MemberType = typeof(MustObjectClausesTestData.SameReferenceAs))]
    [MemberData(nameof(MustObjectClausesTestData.SameReferenceAs.InvalidCases), MemberType = typeof(MustObjectClausesTestData.SameReferenceAs))]
    public void SameReferenceAs_BehavesAsExpected(MustCase<(object a, object b)> tc)
    {
        // Arrange
        var (a, b) = tc.Value;

        // Act
        var result = Must.Be.SameReferenceAs(a, b);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustObjectClausesTestData.NotSameReferenceAs.ValidCases), MemberType = typeof(MustObjectClausesTestData.NotSameReferenceAs))]
    [MemberData(nameof(MustObjectClausesTestData.NotSameReferenceAs.InvalidCases), MemberType = typeof(MustObjectClausesTestData.NotSameReferenceAs))]
    public void NotSameReferenceAs_BehavesAsExpected(MustCase<(object a, object b)> tc)
    {
        // Arrange
        var (a, b) = tc.Value;

        // Act
        var result = Must.Be.NotSameReferenceAs(a, b);

        // Assert
        AssertResult(tc, result);
    }
}
