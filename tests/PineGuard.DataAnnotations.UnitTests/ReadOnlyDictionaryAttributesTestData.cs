using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.ReadOnlyDictionaryRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class ReadOnlyDictionaryAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new(nameof(F.IsEmpty.NullValue), F.IsEmpty.NullValue, true)
    ];

    public static TheoryData<ThrowsCase> TypeMismatchCases =>
    [
        new("string", "not a dict", new ExpectedException(typeof(InvalidOperationException), null, "can only be applied to properties implementing"))
    ];

    public static class EmptyReadOnlyDictionary
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsEmpty.EmptyValue), F.IsEmpty.EmptyValue, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsNotEmpty.PopulatedValue), F.IsNotEmpty.PopulatedValue, false)];
    }

    public static class NotEmptyReadOnlyDictionary
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsNotEmpty.PopulatedValue), F.IsNotEmpty.PopulatedValue, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsEmpty.EmptyValue), F.IsEmpty.EmptyValue, false)];
    }
}
