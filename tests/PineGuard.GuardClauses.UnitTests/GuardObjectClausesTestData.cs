using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.ObjectRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardObjectClausesTestData
{
    private static readonly object ObjA = new();
    private static readonly object ObjB = new();

    // Guard.Against.NotEqualTo — throws when NOT equal (Must.Be.EqualTo fails)
    public static class NotEqualTo
    {
        public static TheoryData<GuardCase<(string? value, string? other)>> ValidCases =>
            F.IsEqualTo.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, string? other)>> InvalidCases =>
            F.IsEqualTo.InvalidScenarios.ToGuardCases(s => s.IsNull
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.EqualTo — throws when ARE equal (Must.Be.NotEqualTo fails)
    public static class EqualTo
    {
        public static TheoryData<GuardCase<(string? value, string? other)>> ValidCases =>
            F.IsEqualTo.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, string? other)>> InvalidCases =>
            F.IsEqualTo.ValidScenarios.ToGuardCases(s => s.Inputs.value is null
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotOfType<string> — throws when NOT of type string (Must.Be.OfType<string> fails)
    public static class NotOfType
    {
        public static TheoryData<GuardCase<object?>> ValidCases =>
            F.IsOfType.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<object?>> InvalidCases =>
            F.IsOfType.InvalidScenarios.ToGuardCases(s => s.IsNull
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OfType<string> — throws when IS of type string (Must.Be.NotOfType<string> fails)
    public static class OfType
    {
        public static TheoryData<GuardCase<object?>> ValidCases =>
            F.IsOfType.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<object?>> InvalidCases =>
            F.IsOfType.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotAssignableToType<string> — throws when NOT assignable to string (Must.Be.AssignableToType<string> fails)
    public static class NotAssignableToType
    {
        public static TheoryData<GuardCase<object?>> ValidCases =>
            F.IsAssignableToType.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<object?>> InvalidCases =>
            F.IsAssignableToType.InvalidScenarios.ToGuardCases(s => s.IsNull
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.AssignableToType<string> — throws when IS assignable to string (Must.Be.NotAssignableToType<string> fails)
    public static class AssignableToType
    {
        public static TheoryData<GuardCase<object?>> ValidCases =>
            F.IsAssignableToType.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<object?>> InvalidCases =>
            F.IsAssignableToType.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotSameReferenceAs — throws when NOT same reference (Must.Be.SameReferenceAs fails)
    public static class NotSameReferenceAs
    {
        public static TheoryData<GuardCase<(object? a, object? b)>> ValidCases =>
        [
            new("SameReference", (ObjA, ObjA), new GuardExpected(true)),
            new("BothNull", (null, null), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(object? a, object? b)>> InvalidCases =>
        [
            new("DifferentReference", (ObjA, ObjB), new GuardExpected(false, typeof(ArgumentException), "a"))
        ];
    }

    // Guard.Against.SameReferenceAs — throws when IS same reference (Must.Be.NotSameReferenceAs fails)
    public static class SameReferenceAs
    {
        public static TheoryData<GuardCase<(object? a, object? b)>> ValidCases =>
        [
            new("DifferentReference", (ObjA, ObjB), new GuardExpected(true)),
            new("ANull", (null, ObjB), new GuardExpected(true)),
            new("BNull", (ObjA, null), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(object? a, object? b)>> InvalidCases =>
        [
            new("SameReference", (ObjA, ObjA), new GuardExpected(false, typeof(ArgumentException), "a")),
            new("BothNull", (null, null), new GuardExpected(false, typeof(ArgumentNullException), "a"))
        ];
    }
}
