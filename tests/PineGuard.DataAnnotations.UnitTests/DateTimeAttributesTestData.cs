using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DateTimeAttributesTestData
{
    public sealed record ValidCase(string Name, Func<object?> Value, bool Expected)
        : ReturnCase<Func<object?>, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", () => null, true)
    ];

    // Past
    public static class PastDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", () => F.IsPast.PastDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", () => F.IsPast.FutureDate!.Value, false)];
    }

    // PastOrPresent
    public static class PastOrPresentDateTime
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", () => F.IsPast.PastDate!.Value, true),
            new("present", () => DateTime.UtcNow, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", () => F.IsPast.FutureDate!.Value, false)];
    }

    // Future
    public static class FutureDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", () => F.IsPast.FutureDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", () => F.IsPast.PastDate!.Value, false)];
    }

    // FutureOrPresent
    public static class FutureOrPresentDateTime
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", () => F.IsPast.FutureDate!.Value, true),
            // True "present" cannot be tested reliably without a controllable clock.
            // We use a small buffer to ensure the generated value is still >= "now" when evaluated.
            new("present", () => DateTime.UtcNow.AddSeconds(1), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", () => F.IsPast.PastDate!.Value, false)];
    }

    // Utc
    public static class UtcDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("utc", () => F.IsUtc.Utc!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("local", () => F.IsLocal.Local!.Value, false),
            new("unspecified", () => F.IsUnspecified.Unspecified!.Value, false)
        ];
    }

    // Local
    public static class LocalDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("local", () => F.IsLocal.Local!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("utc", () => F.IsLocal.Utc!.Value, false),
            new("unspecified", () => F.IsUnspecified.Unspecified!.Value, false)
        ];
    }

    // Unspecified
    public static class UnspecifiedDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("unspecified", () => F.IsUnspecified.Unspecified!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("utc", () => F.IsUnspecified.Utc!.Value, false),
            new("local", () => F.IsLocal.Local!.Value, false)
        ];
    }
}
