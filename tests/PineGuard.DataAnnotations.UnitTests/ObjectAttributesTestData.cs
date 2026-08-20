using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
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

    // Covers ValidationAttributeBase.BuildInvokeArgs: a null value inferred to a non-nullable value-type
    // parameter (int, via the int ComparisonValue) must throw rather than silently coerce to default(int).
    public static class EqualToNullValueType
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "null-value-int-comparison",
                () => new EqualToAttribute(0).GetValidationResult(null, new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException), null, "non-nullable value-type parameter"))
        ];

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    // Covers ObjectAttributeBase.CheckArgCompatibility: a ValidationContext with a MemberName set must
    // report it in ValidationResult.MemberNames rather than always building a member-less result.
    public static class EqualToWithMemberName
    {
        public static TheoryData<ValidCase> Cases => [new("int", 1, false)];
    }
}
