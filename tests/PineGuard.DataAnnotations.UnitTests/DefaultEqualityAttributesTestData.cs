using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DefaultEqualityAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    // NullOrDefault — valid when null or equal to default(runtime type)
    public static class NullOrDefault
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("zero int", 0, true),
            new("empty guid", Guid.Empty, true),
            new("false bool", false, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => [new("null", null, true)];
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("non-zero int", 5, false),
            new("non-empty string", "text", false)
        ];
    }

    // NotNullOrDefault — valid when neither null nor equal to default(runtime type)
    public static class NotNullOrDefault
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("non-zero int", 5, true),
            new("non-empty string", "text", true),
            new("true bool", true, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => [new("null", null, false)];
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("zero int", 0, false),
            new("empty guid", Guid.Empty, false)
        ];
    }
}
