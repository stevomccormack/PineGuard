using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringCasingClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.CaseStyle.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.CaseStyle))]
    [MemberData(nameof(MustStringCasingClausesTestData.CaseStyle.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.CaseStyle))]
    public void CaseStyle_BehavesAsExpected(MustCase<(string? value, StringCasing style)> tc)
    { var value = tc.Value.value; var result = Must.Be.CaseStyle(value, tc.Value.style); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.CamelCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.CamelCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.CamelCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.CamelCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.CamelCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.CamelCase))]
    public void CamelCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.CamelCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.PascalCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.PascalCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.PascalCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.PascalCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.PascalCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.PascalCase))]
    public void PascalCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.PascalCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.SnakeCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.SnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.SnakeCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.SnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.SnakeCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.SnakeCase))]
    public void SnakeCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.SnakeCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.UpperSnakeCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.UpperSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.UpperSnakeCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.UpperSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.UpperSnakeCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.UpperSnakeCase))]
    public void UpperSnakeCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.UpperSnakeCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.KebabCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.KebabCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.KebabCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.KebabCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.KebabCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.KebabCase))]
    public void KebabCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.KebabCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.TrainCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.TrainCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.TrainCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.TrainCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.TrainCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.TrainCase))]
    public void TrainCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.TrainCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.DotCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.DotCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.DotCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.DotCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.DotCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.DotCase))]
    public void DotCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.DotCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.SpaceCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.SpaceCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.SpaceCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.SpaceCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.SpaceCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.SpaceCase))]
    public void SpaceCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.SpaceCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.UpperInvariant.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.UpperInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.UpperInvariant.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.UpperInvariant))]
    public void UpperInvariant_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.UpperInvariant(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.LowerInvariant.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.LowerInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.LowerInvariant.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.LowerInvariant))]
    public void LowerInvariant_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.LowerInvariant(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCaseStyle.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotCaseStyle))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCaseStyle.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotCaseStyle))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCaseStyle.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotCaseStyle))]
    public void NotCaseStyle_BehavesAsExpected(MustCase<(string? value, StringCasing style)> tc)
    { var value = tc.Value.value; var result = Must.Be.NotCaseStyle(value, tc.Value.style); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCamelCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotCamelCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCamelCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotCamelCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotCamelCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotCamelCase))]
    public void NotCamelCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotCamelCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotPascalCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotPascalCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotPascalCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotPascalCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotPascalCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotPascalCase))]
    public void NotPascalCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotPascalCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSnakeCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSnakeCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSnakeCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotSnakeCase))]
    public void NotSnakeCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotSnakeCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperSnakeCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperSnakeCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperSnakeCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperSnakeCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperSnakeCase))]
    public void NotUpperSnakeCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotUpperSnakeCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotKebabCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotKebabCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotKebabCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotKebabCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotKebabCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotKebabCase))]
    public void NotKebabCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotKebabCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotTrainCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotTrainCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotTrainCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotTrainCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotTrainCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotTrainCase))]
    public void NotTrainCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotTrainCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotDotCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotDotCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotDotCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotDotCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotDotCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotDotCase))]
    public void NotDotCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotDotCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSpaceCase.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotSpaceCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSpaceCase.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotSpaceCase))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotSpaceCase.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotSpaceCase))]
    public void NotSpaceCase_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotSpaceCase(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperInvariant.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperInvariant.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotUpperInvariant.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotUpperInvariant))]
    public void NotUpperInvariant_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotUpperInvariant(value); AssertResult(tc, result); }

    [Theory]
    [MemberData(nameof(MustStringCasingClausesTestData.NotLowerInvariant.ValidCases), MemberType = typeof(MustStringCasingClausesTestData.NotLowerInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotLowerInvariant.InvalidCases), MemberType = typeof(MustStringCasingClausesTestData.NotLowerInvariant))]
    [MemberData(nameof(MustStringCasingClausesTestData.NotLowerInvariant.NullCases), MemberType = typeof(MustStringCasingClausesTestData.NotLowerInvariant))]
    public void NotLowerInvariant_BehavesAsExpected(MustCase<string?> tc)
    { var value = tc.Value; var result = Must.Be.NotLowerInvariant(value); AssertResult(tc, result); }
}
