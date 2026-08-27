using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringCasingClausesTestData
{
    public static class CaseStyle
    {
        public static TheoryData<MustCase<(string? value, StringCasing style)>> ValidCases => F.IsCaseStyle.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, StringCasing style)>> InvalidCases => F.IsCaseStyle.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Mismatch));
    }

    public static class CamelCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsCamelCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsCamelCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotCamel));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class PascalCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsPascalCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsPascalCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotPascal));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class SnakeCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsSnakeCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsSnakeCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotSnake));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class UpperSnakeCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsUpperSnakeCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsUpperSnakeCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotUpperSnake));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class KebabCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsKebabCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsKebabCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotKebab));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class TrainCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsTrainCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsTrainCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotTrain));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class DotCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsDotCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsDotCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotDot));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class SpaceCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsSpaceCase.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsSpaceCase.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotSpace));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class UpperInvariant
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsUpperInvariant.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsUpperInvariant.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotUpperInvariant));
    }

    public static class LowerInvariant
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLowerInvariant.ValidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLowerInvariant.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.NotLowerInvariant));
    }

    public static class NotCaseStyle
    {
        public static TheoryData<MustCase<(string? value, StringCasing style)>> ValidCases => F.IsCaseStyle.InvalidScenarios.Except(nameof(F.IsCaseStyle.NullValue), nameof(F.IsCaseStyle.UnknownStyle)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, StringCasing style)>> InvalidCases => F.IsCaseStyle.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Match));
        public static TheoryData<MustCase<(string? value, StringCasing style)>> NullCases => F.IsCaseStyle.InvalidScenarios.Only(nameof(F.IsCaseStyle.NullValue)).ToMustCases(_ => new MustExpected(false));
    }

    public static class NotCamelCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsCamelCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsCamelCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Camel));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotPascalCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsPascalCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsPascalCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Pascal));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotSnakeCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsSnakeCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsSnakeCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Snake));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotUpperSnakeCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsUpperSnakeCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsUpperSnakeCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.UpperSnake));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotKebabCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsKebabCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsKebabCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Kebab));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotTrainCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsTrainCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsTrainCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Train));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotDotCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsDotCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsDotCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Dot));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotSpaceCase
    {
        public static TheoryData<MustCase<string>> ValidCases => F.IsSpaceCase.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string>> InvalidCases => F.IsSpaceCase.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.Space));
        public static TheoryData<MustCase<string?>> NullCases => [new("NullValue", null, new MustExpected(false, "value must not be null.", "value"))];
    }

    public static class NotUpperInvariant
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsUpperInvariant.InvalidScenarios.Except(nameof(F.IsUpperInvariant.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsUpperInvariant.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.UpperInvariant));
        public static TheoryData<MustCase<string?>> NullCases => F.IsUpperInvariant.InvalidScenarios.Only(nameof(F.IsUpperInvariant.NullValue)).ToMustCases(_ => new MustExpected(false));
    }

    public static class NotLowerInvariant
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLowerInvariant.InvalidScenarios.Except(nameof(F.IsLowerInvariant.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLowerInvariant.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Text.Casing.LowerInvariant));
        public static TheoryData<MustCase<string?>> NullCases => F.IsLowerInvariant.InvalidScenarios.Only(nameof(F.IsLowerInvariant.NullValue)).ToMustCases(_ => new MustExpected(false));
    }
}
