using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringNumberTypesClausesTestData
{
    public static class NotDecimal
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsDecimal.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsDecimal.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotExactDecimal
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsExactDecimal.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsExactDecimal.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotInt32
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsInt32.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsInt32.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotInt64
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsInt64.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsInt64.InvalidScenarios.ToGuardCases("value");
    }

    public static class OutOfRangeInt32
    {
        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> ValidCases =>
            F.IsInRangeInt32.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRangeInt32.AllInvalid.ToGuardCases("value");
    }

    public static class InRangeInt32
    {
        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.IsInRangeInt32.AtMinExclusive), F.IsInRangeInt32.AtMinExclusive, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.IsInRangeInt32.BetweenInclusive), F.IsInRangeInt32.BetweenInclusive, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInRangeInt32.AtMinInclusive),   F.IsInRangeInt32.AtMinInclusive,   new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInRangeInt32.NotNumeric),       F.IsInRangeInt32.NotNumeric,       new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class OutOfRangeInt64
    {
        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> ValidCases =>
            F.IsInRangeInt64.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRangeInt64.AllInvalid.ToGuardCases("value");
    }

    public static class InRangeInt64
    {
        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.IsInRangeInt64.AtMinExclusive), F.IsInRangeInt64.AtMinExclusive, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.IsInRangeInt64.BetweenInclusive), F.IsInRangeInt64.BetweenInclusive, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInRangeInt64.AtMinInclusive),   F.IsInRangeInt64.AtMinInclusive,   new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInRangeInt64.NotNumeric),       F.IsInRangeInt64.NotNumeric,       new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
