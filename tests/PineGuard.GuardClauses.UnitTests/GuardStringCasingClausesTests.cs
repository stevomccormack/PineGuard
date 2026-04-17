using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringCasingClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotCaseStyle.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotCaseStyle))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotCaseStyle.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotCaseStyle))]
    public void NotCaseStyle_BehavesAsExpected(GuardCase<(string? value, StringCasing style)> tc)
    {
        var value = tc.Value.value;
        var style = tc.Value.style;
        var result = AssertResult(tc, () => Guard.Against.NotCaseStyle(value, style));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.CaseStyle.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.CaseStyle))]
    [MemberData(nameof(GuardStringCasingClausesTestData.CaseStyle.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.CaseStyle))]
    [MemberData(nameof(GuardStringCasingClausesTestData.CaseStyle.NullCases), MemberType = typeof(GuardStringCasingClausesTestData.CaseStyle))]
    public void CaseStyle_BehavesAsExpected(GuardCase<(string? value, StringCasing style)> tc)
    {
        var value = tc.Value.value;
        var style = tc.Value.style;
        var result = AssertResult(tc, () => Guard.Against.CaseStyle(value, style));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotCamelCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotCamelCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotCamelCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotCamelCase))]
    public void NotCamelCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotCamelCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.CamelCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.CamelCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.CamelCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.CamelCase))]
    public void CamelCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.CamelCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotPascalCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotPascalCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotPascalCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotPascalCase))]
    public void NotPascalCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPascalCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.PascalCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.PascalCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.PascalCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.PascalCase))]
    public void PascalCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PascalCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotSnakeCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotSnakeCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotSnakeCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotSnakeCase))]
    public void NotSnakeCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotSnakeCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.SnakeCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.SnakeCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.SnakeCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.SnakeCase))]
    public void SnakeCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.SnakeCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotUpperSnakeCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotUpperSnakeCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotUpperSnakeCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotUpperSnakeCase))]
    public void NotUpperSnakeCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotUpperSnakeCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.UpperSnakeCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.UpperSnakeCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.UpperSnakeCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.UpperSnakeCase))]
    public void UpperSnakeCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.UpperSnakeCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotKebabCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotKebabCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotKebabCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotKebabCase))]
    public void NotKebabCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotKebabCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.KebabCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.KebabCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.KebabCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.KebabCase))]
    public void KebabCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.KebabCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotTrainCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotTrainCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotTrainCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotTrainCase))]
    public void NotTrainCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotTrainCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.TrainCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.TrainCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.TrainCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.TrainCase))]
    public void TrainCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.TrainCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotDotCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotDotCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotDotCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotDotCase))]
    public void NotDotCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDotCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.DotCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.DotCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.DotCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.DotCase))]
    public void DotCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.DotCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotSpaceCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotSpaceCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotSpaceCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotSpaceCase))]
    public void NotSpaceCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotSpaceCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.SpaceCase.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.SpaceCase))]
    [MemberData(nameof(GuardStringCasingClausesTestData.SpaceCase.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.SpaceCase))]
    public void SpaceCase_BehavesAsExpected(GuardCase<string> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.SpaceCase(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotUpperInvariant.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotUpperInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotUpperInvariant.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotUpperInvariant))]
    public void NotUpperInvariant_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotUpperInvariant(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.UpperInvariant.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.UpperInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.UpperInvariant.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.UpperInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.UpperInvariant.NullCases), MemberType = typeof(GuardStringCasingClausesTestData.UpperInvariant))]
    public void UpperInvariant_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.UpperInvariant(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotLowerInvariant.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotLowerInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.NotLowerInvariant.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.NotLowerInvariant))]
    public void NotLowerInvariant_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotLowerInvariant(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardStringCasingClausesTestData.LowerInvariant.ValidCases), MemberType = typeof(GuardStringCasingClausesTestData.LowerInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.LowerInvariant.InvalidCases), MemberType = typeof(GuardStringCasingClausesTestData.LowerInvariant))]
    [MemberData(nameof(GuardStringCasingClausesTestData.LowerInvariant.NullCases), MemberType = typeof(GuardStringCasingClausesTestData.LowerInvariant))]
    public void LowerInvariant_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.LowerInvariant(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
