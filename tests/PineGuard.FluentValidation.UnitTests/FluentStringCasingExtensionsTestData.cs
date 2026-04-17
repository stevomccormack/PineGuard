using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringCasingExtensionsTestData
{
    public static class CaseStyle
    {
        public static TheoryData<FluentCase<(string? value, StringCasing style)>> Cases => F.IsCaseStyle.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCaseStyle.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the specified casing style.")
        });
    }

    public static class NotCaseStyle
    {
        public static TheoryData<FluentCase<(string? value, StringCasing style)>> Cases => F.IsCaseStyle.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCaseStyle.NullValue) => new FluentExpected(true),
            nameof(F.IsCaseStyle.UnknownStyle) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be in the specified casing style.")
        });
    }

    public static class CamelCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsCamelCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be camelCase."));
    }

    public static class NotCamelCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsCamelCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be camelCase."));
    }

    public static class PascalCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsPascalCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be PascalCase."));
    }

    public static class NotPascalCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsPascalCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be PascalCase."));
    }

    public static class SnakeCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsSnakeCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be snake_case."));
    }

    public static class NotSnakeCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsSnakeCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be snake_case."));
    }

    public static class UpperSnakeCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsUpperSnakeCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be UPPER_SNAKE_CASE."));
    }

    public static class NotUpperSnakeCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsUpperSnakeCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be UPPER_SNAKE_CASE."));
    }

    public static class KebabCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsKebabCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be kebab-case."));
    }

    public static class NotKebabCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsKebabCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be kebab-case."));
    }

    public static class TrainCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsTrainCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be Train-Case."));
    }

    public static class NotTrainCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsTrainCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be Train-Case."));
    }

    public static class DotCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsDotCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be dot.case."));
    }

    public static class NotDotCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsDotCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be dot.case."));
    }

    public static class SpaceCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsSpaceCase.AllScenarios.ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be space case."));
    }

    public static class NotSpaceCase
    {
        public static TheoryData<FluentCase<string>> Cases => F.IsSpaceCase.AllScenarios.ToFluentCases(s => !s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be space case."));
    }

    public static class UpperInvariant
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsUpperInvariant.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUpperInvariant.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be upper invariant.")
        });
    }

    public static class NotUpperInvariant
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsUpperInvariant.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUpperInvariant.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be upper invariant.")
        });
    }

    public static class LowerInvariant
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLowerInvariant.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLowerInvariant.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be lower invariant.")
        });
    }

    public static class NotLowerInvariant
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLowerInvariant.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLowerInvariant.NullValue) => new FluentExpected(true),
            _ when !s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be lower invariant.")
        });
    }
}
