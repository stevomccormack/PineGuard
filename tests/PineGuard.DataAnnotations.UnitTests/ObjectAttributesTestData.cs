using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.ObjectRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class ObjectAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases(bool expected = true) =>
    [
        new("null", null, expected)
    ];

    public static class Null
    {
        public static TheoryData<ValidCase> ValidCases => [new("null", null, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, false)];
    }

    public static class NotNull
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases(false);
        public static TheoryData<ValidCase> InvalidCases => [new("null", null, false)];
    }

    public static class IsDefault
    {
        public static TheoryData<ValidCase> ValidCases => [new("null", null, true), new("default int", 0, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, false), new("int", 1, false)];
    }

    public static class NotDefault
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, true), new("int", 1, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases(false);
        public static TheoryData<ValidCase> InvalidCases => [new("null", null, false), new("default int", 0, false)];
    }

    // ComparisonValue="abc"
    public static class EqualTo
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsEqualTo.EqualStrings), F.IsEqualTo.EqualStrings.value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases(false);
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsEqualTo.NotEqualStrings), F.IsEqualTo.NotEqualStrings.other, false), new("int", 1, false)];
    }

    // ComparisonValue="abc"
    public static class NotEqualTo
    {
        // "int" mismatch -> Validation Failure -> Success=False.
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsEqualTo.NotEqualStrings), F.IsEqualTo.NotEqualStrings.other, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsEqualTo.EqualStrings), F.IsEqualTo.EqualStrings.value, false), new("int", 1, false)];
    }

    // TargetType=typeof(string)
    public static class OfType
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases(false);
        public static TheoryData<ValidCase> InvalidCases => [new("int", 1, false), new(nameof(F.IsAssignableToType.ObjectValue), F.IsAssignableToType.ObjectValue, false)];
    }

    // TargetType=typeof(string)
    public static class NotOfType
    {
        public static TheoryData<ValidCase> ValidCases => [new("int", 1, true), new(nameof(F.IsAssignableToType.ObjectValue), F.IsAssignableToType.ObjectValue, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsOfType.StringValue), F.IsOfType.StringValue, false)];
    }
}
