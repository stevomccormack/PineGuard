using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.ObjectRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentObjectExtensionsTestData
{
    public static class EqualTo
    {
        public static TheoryData<FluentCase<(string? value, string? other)>> Cases => F.IsEqualTo.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be equal to the expected value.")
        });
    }

    public static class NotEqualTo
    {
        public static TheoryData<FluentCase<(string? value, string? other)>> Cases => F.IsEqualTo.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(false, "Value must not be equal to the expected value."),
            _ => new FluentExpected(true)
        });
    }

    public static class OfType
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsOfType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be of the expected type.")
        });
    }

    public static class NotOfType
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsOfType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(false, "Value must not be of the expected type."),
            _ => new FluentExpected(true)
        });
    }

    public static class AssignableToType
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsAssignableToType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be assignable to the expected type.")
        });
    }

    public static class NotAssignableToType
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsAssignableToType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(false, "Value must not be assignable to the expected type."),
            _ => new FluentExpected(true)
        });
    }

    public static class SameReferenceAs
    {
        public static TheoryData<FluentCase<(object? a, object? b)>> Cases => F.IsSameReferenceAs.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must reference the same instance.")
        });
    }

    public static class NotSameReferenceAs
    {
        public static TheoryData<FluentCase<(object? a, object? b)>> Cases => F.IsSameReferenceAs.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(false, "Value must not reference the same instance."),
            _ => new FluentExpected(true)
        });
    }
}
