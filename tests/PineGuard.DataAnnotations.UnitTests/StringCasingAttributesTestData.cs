using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringCasingAttributesTestData
{
    public static class CaseStyle
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCamelCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotCaseStyle
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCamelCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class CamelCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCamelCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotCamelCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCamelCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class PascalCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPascalCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotPascalCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPascalCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class SnakeCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSnakeCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotSnakeCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSnakeCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class UpperSnakeCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUpperSnakeCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotUpperSnakeCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUpperSnakeCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class KebabCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsKebabCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotKebabCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsKebabCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class TrainCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsTrainCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotTrainCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsTrainCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class DotCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsDotCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotDotCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsDotCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class SpaceCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSpaceCase.AllScenarios.ToDataAnnotationCases(v => v, s => s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class NotSpaceCase
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSpaceCase.AllScenarios.ToDataAnnotationCases(v => v, s => !s.IsValid
            ? new DataAnnotationExpected(true)
            : new DataAnnotationExpected(false));
    }

    public static class UpperInvariant
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUpperInvariant.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsUpperInvariant.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class NotUpperInvariant
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUpperInvariant.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsUpperInvariant.NullValue) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class LowerInvariant
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLowerInvariant.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsLowerInvariant.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class NotLowerInvariant
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLowerInvariant.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsLowerInvariant.NullValue) => new DataAnnotationExpected(true),
            _ when !s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class UppercaseString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("null", null, new DataAnnotationExpected(true)),
            new("upper with numbers", "ABC 123", new DataAnnotationExpected(true)),
            new("lower", "abc", new DataAnnotationExpected(false))
        ];
    }

    public static class NotUppercaseString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("null", null, new DataAnnotationExpected(true)),
            new("lower", "abc", new DataAnnotationExpected(true)),
            new("upper", "ABC", new DataAnnotationExpected(false))
        ];
    }

    public static class LowercaseString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("null", null, new DataAnnotationExpected(true)),
            new("lower with numbers", "abc 123", new DataAnnotationExpected(true)),
            new("upper", "ABC", new DataAnnotationExpected(false))
        ];
    }

    public static class NotLowercaseString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("null", null, new DataAnnotationExpected(true)),
            new("upper", "ABC", new DataAnnotationExpected(true)),
            new("lower", "abc", new DataAnnotationExpected(false))
        ];
    }
}
