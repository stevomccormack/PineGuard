using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringNumbersClausesTestData
{
    // ── Simple string? ops (DIRECT fixture mapping) ─────────────────

    public static class ZeroOrNegative
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsPositive.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsPositive.InvalidScenarios.ToGuardCases("value");
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsNegative.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsNegative.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotZero
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsZero.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsZero.InvalidScenarios.ToGuardCases("value");
    }

    public static class Zero
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsNotZero.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsNotZero.InvalidScenarios.ToGuardCases("value");
    }

    public static class Negative
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsZeroOrPositive.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsZeroOrPositive.InvalidScenarios.ToGuardCases("value");
    }

    public static class Positive
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsZeroOrNegative.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsZeroOrNegative.InvalidScenarios.ToGuardCases("value");
    }

    public static class Odd
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsEven.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsEven.InvalidScenarios.ToGuardCases("value");
    }

    public static class Even
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsOdd.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsOdd.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotFinite
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.NumbersIsFinite.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.NumbersIsFinite.InvalidScenarios.ToGuardCases("value");
    }

    // ── Simple string? ops (INVERTED — unparseable values stay in InvalidCases) ──

    public static class Finite
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new("Infinity", "Infinity", new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.NumbersIsFinite.Finite), F.NumbersIsFinite.Finite, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsFinite.Letters), F.NumbersIsFinite.Letters, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NaN
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(F.NumbersIsNaN.Finite), F.NumbersIsNaN.Finite, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.NumbersIsNaN.NaN), F.NumbersIsNaN.NaN, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsNaN.Letters), F.NumbersIsNaN.Letters, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // ── Tuple ops (DIRECT fixture mapping) ──────────────────────────

    public static class LessThanOrEqual
    {
        public static TheoryData<GuardCase<(string? text, decimal min)>> ValidCases => F.NumbersIsGreaterThan.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal min)>> InvalidCases => F.NumbersIsGreaterThan.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThan.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class LessThan
    {
        public static TheoryData<GuardCase<(string? text, decimal min)>> ValidCases => F.NumbersIsGreaterThanOrEqual.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal min)>> InvalidCases => F.NumbersIsGreaterThanOrEqual.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThanOrEqual.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class GreaterThanOrEqual
    {
        public static TheoryData<GuardCase<(string? text, decimal max)>> ValidCases => F.NumbersIsLessThan.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal max)>> InvalidCases => F.NumbersIsLessThan.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThan.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class GreaterThan
    {
        public static TheoryData<GuardCase<(string? text, decimal max)>> ValidCases => F.NumbersIsLessThanOrEqual.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal max)>> InvalidCases => F.NumbersIsLessThanOrEqual.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThanOrEqual.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.OutOfRange calls Must.Be.OutOfRange, which checks "min > max" as its own precondition
    // before inspecting the value at all — an inverted range therefore attributes to "min", not "value"
    // (see MustStringNumbersClauses.OutOfRange).
    public static class OutOfRange
    {
        public static TheoryData<GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> ValidCases => F.NumbersIsInRange.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> InvalidCases => F.NumbersIsInRange.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsInRange.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            nameof(F.NumbersIsInRange.InvalidRange) => new GuardExpected(false, typeof(ArgumentException), "min"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.NotApproximately calls Must.Be.Approximately, which checks "tolerance" for null as its
    // own precondition before inspecting the value at all — a null tolerance therefore attributes to
    // "tolerance" (and is a null-reference failure, not a range failure), not "value" (see MustStringNumbersClauses.Approximately).
    public static class NotApproximately
    {
        public static TheoryData<GuardCase<(string? text, decimal target, decimal? tolerance)>> ValidCases => F.NumbersIsApproximately.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal target, decimal? tolerance)>> InvalidCases => F.NumbersIsApproximately.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsApproximately.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            nameof(F.NumbersIsApproximately.NullTolerance) => new GuardExpected(false, typeof(ArgumentNullException), "tolerance"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.NotMultipleOf calls Must.Be.MultipleOf, which checks "factor == 0" as its own
    // precondition before inspecting the value at all — a zero factor therefore attributes to "factor",
    // not "value" (see MustStringNumbersClauses.MultipleOf).
    public static class NotMultipleOf
    {
        public static TheoryData<GuardCase<(string? text, decimal factor)>> ValidCases => F.NumbersIsMultipleOf.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<(string? text, decimal factor)>> InvalidCases => F.NumbersIsMultipleOf.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.NumbersIsMultipleOf.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            nameof(F.NumbersIsMultipleOf.ZeroFactor) => new GuardExpected(false, typeof(ArgumentException), "factor"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // ── Tuple ops (INVERTED — unparseable values stay in InvalidCases) ──

    public static class InRange
    {
        public static TheoryData<GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> ValidCases =>
        [
            new(nameof(F.NumbersIsInRange.AtMinExclusive), F.NumbersIsInRange.AtMinExclusive, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> InvalidCases =>
        [
            new(nameof(F.NumbersIsInRange.BetweenInclusive), F.NumbersIsInRange.BetweenInclusive, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsInRange.Letters), F.NumbersIsInRange.Letters, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Approximately calls Must.Be.NotApproximately, which also checks "tolerance" for null as
    // its own precondition — a null tolerance attributes to "tolerance" here too (see MustStringNumbersClauses.NotApproximately).
    public static class Approximately
    {
        public static TheoryData<GuardCase<(string? text, decimal target, decimal? tolerance)>> ValidCases =>
        [
            new(nameof(F.NumbersIsApproximately.OutsideTolerance), F.NumbersIsApproximately.OutsideTolerance, new GuardExpected(true)),
            new(nameof(F.NumbersIsApproximately.NegativeTolerance), F.NumbersIsApproximately.NegativeTolerance, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(string? text, decimal target, decimal? tolerance)>> InvalidCases =>
        [
            new(nameof(F.NumbersIsApproximately.WithinTolerance), F.NumbersIsApproximately.WithinTolerance, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsApproximately.Letters), F.NumbersIsApproximately.Letters, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsApproximately.NullTolerance), F.NumbersIsApproximately.NullTolerance, new GuardExpected(false, typeof(ArgumentNullException), "tolerance"))
        ];
    }

    public static class MultipleOf
    {
        public static TheoryData<GuardCase<(string? text, decimal factor)>> ValidCases =>
        [
            new(nameof(F.NumbersIsMultipleOf.NotMultiple), F.NumbersIsMultipleOf.NotMultiple, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(string? text, decimal factor)>> InvalidCases =>
        [
            new(nameof(F.NumbersIsMultipleOf.Multiple), F.NumbersIsMultipleOf.Multiple, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.NumbersIsMultipleOf.Letters), F.NumbersIsMultipleOf.Letters, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
