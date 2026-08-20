using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardObjectClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.NotEqualTo.ValidCases), MemberType = typeof(GuardObjectClausesTestData.NotEqualTo))]
    [MemberData(nameof(GuardObjectClausesTestData.NotEqualTo.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.NotEqualTo))]
    public void NotEqualTo_BehavesAsExpected(GuardCase<(string? value, string? other)> tc)
    {
        var value = tc.Value.value;
        var other = tc.Value.other;
        var result = AssertResult(tc, () => Guard.Against.NotEqualTo(value, other));
        AssertCustomMessage(tc, () => Guard.Against.NotEqualTo(value, other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.EqualTo.ValidCases), MemberType = typeof(GuardObjectClausesTestData.EqualTo))]
    [MemberData(nameof(GuardObjectClausesTestData.EqualTo.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.EqualTo))]
    public void EqualTo_BehavesAsExpected(GuardCase<(string? value, string? other)> tc)
    {
        var value = tc.Value.value;
        var other = tc.Value.other;
        var result = AssertResult(tc, () => Guard.Against.EqualTo(value, other));
        AssertCustomMessage(tc, () => Guard.Against.EqualTo(value, other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.NotOfType.ValidCases), MemberType = typeof(GuardObjectClausesTestData.NotOfType))]
    [MemberData(nameof(GuardObjectClausesTestData.NotOfType.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.NotOfType))]
    public void NotOfType_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotOfType<string>(value));
        AssertCustomMessage(tc, () => Guard.Against.NotOfType<string>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.OfType.ValidCases), MemberType = typeof(GuardObjectClausesTestData.OfType))]
    [MemberData(nameof(GuardObjectClausesTestData.OfType.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.OfType))]
    public void OfType_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.OfType<string>(value));
        AssertCustomMessage(tc, () => Guard.Against.OfType<string>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.NotAssignableToType.ValidCases), MemberType = typeof(GuardObjectClausesTestData.NotAssignableToType))]
    [MemberData(nameof(GuardObjectClausesTestData.NotAssignableToType.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.NotAssignableToType))]
    public void NotAssignableToType_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotAssignableToType<string>(value));
        AssertCustomMessage(tc, () => Guard.Against.NotAssignableToType<string>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.AssignableToType.ValidCases), MemberType = typeof(GuardObjectClausesTestData.AssignableToType))]
    [MemberData(nameof(GuardObjectClausesTestData.AssignableToType.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.AssignableToType))]
    public void AssignableToType_BehavesAsExpected(GuardCase<object?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.AssignableToType<string>(value));
        AssertCustomMessage(tc, () => Guard.Against.AssignableToType<string>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.NotSameReferenceAs.ValidCases), MemberType = typeof(GuardObjectClausesTestData.NotSameReferenceAs))]
    [MemberData(nameof(GuardObjectClausesTestData.NotSameReferenceAs.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.NotSameReferenceAs))]
    public void NotSameReferenceAs_BehavesAsExpected(GuardCase<(object? a, object? b)> tc)
    {
        var a = tc.Value.a;
        var b = tc.Value.b;
        var result = AssertResult(tc, () => Guard.Against.NotSameReferenceAs(a, b));
        AssertCustomMessage(tc, () => Guard.Against.NotSameReferenceAs(a, b, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(a, result);
    }

    [Theory]
    [MemberData(nameof(GuardObjectClausesTestData.SameReferenceAs.ValidCases), MemberType = typeof(GuardObjectClausesTestData.SameReferenceAs))]
    [MemberData(nameof(GuardObjectClausesTestData.SameReferenceAs.InvalidCases), MemberType = typeof(GuardObjectClausesTestData.SameReferenceAs))]
    public void SameReferenceAs_BehavesAsExpected(GuardCase<(object? a, object? b)> tc)
    {
        var a = tc.Value.a;
        var b = tc.Value.b;
        var result = AssertResult(tc, () => Guard.Against.SameReferenceAs(a, b));
        AssertCustomMessage(tc, () => Guard.Against.SameReferenceAs(a, b, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(a, result);
    }
}
