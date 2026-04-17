using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringDateTimeOffsetClausesTestData
{
    // Guard.Against.FutureOrPresent — valid when Must.Be.PastDateTimeOffset succeeds (past)
    public static class FutureOrPresent
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateTimeOffsetIsInPast.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateTimeOffsetIsInPast.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.FutureDateTimeOffset — valid when Must.Be.PastOrPresentDateTimeOffset succeeds
    public static class Future
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateTimeOffsetIsInPast.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateTimeOffsetIsInPast.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.PastOrPresent — valid when Must.Be.FutureDateTimeOffset succeeds (future)
    public static class PastOrPresent
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateTimeOffsetIsInFuture.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateTimeOffsetIsInFuture.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.PastDateTimeOffset — valid when Must.Be.FutureOrPresentDateTimeOffset succeeds
    public static class Past
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateTimeOffsetIsInFuture.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateTimeOffsetIsInFuture.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotBetween — passes when between range; throws when not between / null / unparseable
    public static class NotBetween
    {
        public static TheoryData<GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)>> ValidCases =>
            F.DateTimeOffsetIsBetween.AllValid.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)>> InvalidCases =>
            F.DateTimeOffsetIsBetween.AllInvalid.ToGuardCases(s => s.Inputs.value is null
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Between — passes when not between (parseable); throws when between / null / unparseable
    public static class Between
    {
        public static TheoryData<GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.DateTimeOffsetIsBetween.MinExclusive), F.DateTimeOffsetIsBetween.MinExclusive, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, DateTimeOffset min, DateTimeOffset max, Common.Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.DateTimeOffsetIsBetween.InsideRange), F.DateTimeOffsetIsBetween.InsideRange, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.DateTimeOffsetIsBetween.NotADate),    F.DateTimeOffsetIsBetween.NotADate,    new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.DateTimeOffsetIsBetween.NullValue),   F.DateTimeOffsetIsBetween.NullValue,   new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotWithinDateTimeOffset — forbids not-within (passes when within)
    public static class NotWithin
    {
        private static readonly DateTimeOffset Ref = new(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> ValidCases =>
        [
            new("within", ("2024-01-01T00:00:05+00:00", Ref, TimeSpan.FromSeconds(10)), new GuardExpected(true)),
            new("exact",  ("2024-01-01T00:00:00+00:00", Ref, TimeSpan.Zero),            new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> InvalidCases =>
        [
            new("outside", ("2024-01-01T00:00:11+00:00", Ref, TimeSpan.FromSeconds(10)), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null",    (null,                         Ref, TimeSpan.FromSeconds(10)), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.WithinDateTimeOffset — forbids within (passes when not-within)
    public static class Within
    {
        private static readonly DateTimeOffset Ref = new(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> ValidCases =>
        [
            new("outside", ("2024-01-01T00:00:11+00:00", Ref, TimeSpan.FromSeconds(10)), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> InvalidCases =>
        [
            new("within", ("2024-01-01T00:00:05+00:00", Ref, TimeSpan.FromSeconds(10)), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null",   (null,                         Ref, TimeSpan.FromSeconds(10)), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotWithinCalendarMonthsDateTimeOffset — forbids not-within-months (passes when within months)
    public static class NotWithinCalendarMonths
    {
        private static readonly DateTimeOffset Ref = new(2024, 02, 15, 0, 0, 0, TimeSpan.Zero);

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, int months)>> ValidCases =>
        [
            new("within", ("2024-03-01T00:00:00+00:00", Ref, 1), new GuardExpected(true)),
            new("same",   ("2024-02-15T10:00:00+00:00", Ref, 0), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, int months)>> InvalidCases =>
        [
            new("outside", ("2024-04-01T00:00:00+00:00", Ref, 1), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null",    (null,                         Ref, 1), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.WithinCalendarMonthsDateTimeOffset — forbids within-months (passes when not-within months)
    public static class WithinCalendarMonths
    {
        private static readonly DateTimeOffset Ref = new(2024, 02, 15, 0, 0, 0, TimeSpan.Zero);

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, int months)>> ValidCases =>
        [
            new("outside", ("2024-04-01T00:00:00+00:00", Ref, 1), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, DateTimeOffset? reference, int months)>> InvalidCases =>
        [
            new("within", ("2024-03-01T00:00:00+00:00", Ref, 1), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null",   (null,                         Ref, 1), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }
}
