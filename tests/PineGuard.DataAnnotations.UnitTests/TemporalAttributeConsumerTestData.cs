using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TemporalAttributeConsumerTestData
{
    public sealed record Case(string Name, bool Value, bool Expected)
        : ReturnCase<bool, bool>(Name, Value, Expected);

    public static class AttributeSyntax
    {
        public static TheoryData<Case> Cases =>
        [
            new("valid-boundaries", true, true),
            new("invalid-values", false, false)
        ];
    }
}
