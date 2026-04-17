using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringCasingClausesTestData
{
    public static class NotCaseStyle
    {
        public static TheoryData<GuardCase<(string? value, StringCasing style)>> ValidCases => F.IsCaseStyle.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(string? value, StringCasing style)>> InvalidCases => F.IsCaseStyle.InvalidScenarios
            .Except(nameof(F.IsCaseStyle.UnknownStyle))
            .ToGuardCases(s => s.Name switch
            {
                nameof(F.IsCaseStyle.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }

    public static class CaseStyle
    {
        public static TheoryData<GuardCase<(string? value, StringCasing style)>> ValidCases => F.IsCaseStyle.InvalidScenarios.Except(nameof(F.IsCaseStyle.NullValue), nameof(F.IsCaseStyle.UnknownStyle)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, StringCasing style)>> InvalidCases => F.IsCaseStyle.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
        public static TheoryData<GuardCase<(string? value, StringCasing style)>> NullCases => F.IsCaseStyle.InvalidScenarios.Only(nameof(F.IsCaseStyle.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }

    public static class NotCamelCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsCamelCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsCamelCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class CamelCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsCamelCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsCamelCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotPascalCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsPascalCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsPascalCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class PascalCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsPascalCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsPascalCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotSnakeCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsSnakeCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsSnakeCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class SnakeCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsSnakeCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsSnakeCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotUpperSnakeCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsUpperSnakeCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsUpperSnakeCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class UpperSnakeCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsUpperSnakeCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsUpperSnakeCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotKebabCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsKebabCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsKebabCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class KebabCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsKebabCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsKebabCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotTrainCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsTrainCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsTrainCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class TrainCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsTrainCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsTrainCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotDotCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsDotCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsDotCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class DotCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsDotCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsDotCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotSpaceCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsSpaceCase.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsSpaceCase.InvalidScenarios.ToGuardCases("value");
    }

    public static class SpaceCase
    {
        public static TheoryData<GuardCase<string>> ValidCases => F.IsSpaceCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string>> InvalidCases => F.IsSpaceCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotUpperInvariant
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsUpperInvariant.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsUpperInvariant.InvalidScenarios.ToGuardCases("value");
    }

    public static class UpperInvariant
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsUpperInvariant.InvalidScenarios.Except(nameof(F.IsUpperInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsUpperInvariant.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
        public static TheoryData<GuardCase<string?>> NullCases => F.IsUpperInvariant.InvalidScenarios.Only(nameof(F.IsUpperInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }

    public static class NotLowerInvariant
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLowerInvariant.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLowerInvariant.InvalidScenarios.ToGuardCases("value");
    }

    public static class LowerInvariant
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLowerInvariant.InvalidScenarios.Except(nameof(F.IsLowerInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLowerInvariant.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
        public static TheoryData<GuardCase<string?>> NullCases => F.IsLowerInvariant.InvalidScenarios.Only(nameof(F.IsLowerInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }
}
