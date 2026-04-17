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

    public static class Int32OutOfRange
    {
        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> ValidCases =>
            F.IsInt32InRange.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> InvalidCases =>
            F.IsInt32InRange.AllInvalid.ToGuardCases("value");
    }

    public static class Int32InRange
    {
        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.IsInt32InRange.AtMinExclusive), F.IsInt32InRange.AtMinExclusive, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string text, int min, int max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.IsInt32InRange.BetweenInclusive), F.IsInt32InRange.BetweenInclusive, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInt32InRange.AtMinInclusive),   F.IsInt32InRange.AtMinInclusive,   new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInt32InRange.NotNumeric),       F.IsInt32InRange.NotNumeric,       new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class Int64OutOfRange
    {
        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> ValidCases =>
            F.IsInt64InRange.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> InvalidCases =>
            F.IsInt64InRange.AllInvalid.ToGuardCases("value");
    }

    public static class Int64InRange
    {
        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.IsInt64InRange.AtMinExclusive), F.IsInt64InRange.AtMinExclusive, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string text, long min, long max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.IsInt64InRange.BetweenInclusive), F.IsInt64InRange.BetweenInclusive, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInt64InRange.AtMinInclusive),   F.IsInt64InRange.AtMinInclusive,   new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInt64InRange.NotNumeric),       F.IsInt64InRange.NotNumeric,       new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
