using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Numbers ─────────────────────────────────────────────────────

    public static class NumbersIsPositive
    {
        public static readonly string? Positive = "1";
        public static readonly string? Zero = "0";
        public static readonly string? Negative = "-1";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Positive), Positive, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Zero), Zero, false), new(nameof(Negative), Negative, false), new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsNegative
    {
        public static readonly string? Negative = "-1";
        public static readonly string? Zero = "0";
        public static readonly string? Positive = "1";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Negative), Negative, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Zero), Zero, false), new(nameof(Positive), Positive, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsZero
    {
        public static readonly string? Zero = "0";
        public static readonly string? NonZero = "1";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Zero), Zero, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NonZero), NonZero, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsNotZero
    {
        public static readonly string? Positive = "1";
        public static readonly string? Zero = "0";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Positive), Positive, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Zero), Zero, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsZeroOrPositive
    {
        public static readonly string? Zero = "0";
        public static readonly string? Positive = "1";
        public static readonly string? Negative = "-1";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Zero), Zero, true), new(nameof(Positive), Positive, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Negative), Negative, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsZeroOrNegative
    {
        public static readonly string? Zero = "0";
        public static readonly string? Negative = "-1";
        public static readonly string? Positive = "1";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Zero), Zero, true), new(nameof(Negative), Negative, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Positive), Positive, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsGreaterThan
    {
        public static readonly (string? text, decimal min) Greater = ("2", 1m);
        public static readonly (string? text, decimal min) Equal = ("1", 1m);
        public static readonly (string? text, decimal min) NullValue = (null, 1m);
        public static readonly (string? text, decimal min) Letters = ("abc", 1m);

        public static RuleScenario<(string? text, decimal min)>[] ValidScenarios => [new(nameof(Greater), Greater, true)];
        public static RuleScenario<(string? text, decimal min)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal min)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal min)>[] InvalidEdgeScenarios => [new(nameof(Equal), Equal, false)];
        public static RuleScenario<(string? text, decimal min)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsGreaterThanOrEqual
    {
        public static readonly (string? text, decimal min) Greater = ("2", 1m);
        public static readonly (string? text, decimal min) Equal = ("1", 1m);
        public static readonly (string? text, decimal min) Less = ("0", 1m);
        public static readonly (string? text, decimal min) NullValue = (null, 1m);
        public static readonly (string? text, decimal min) Letters = ("abc", 1m);

        public static RuleScenario<(string? text, decimal min)>[] ValidScenarios => [new(nameof(Greater), Greater, true)];
        public static RuleScenario<(string? text, decimal min)>[] ValidEdgeScenarios => [new(nameof(Equal), Equal, true)];
        public static RuleScenario<(string? text, decimal min)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal min)>[] InvalidEdgeScenarios => [new(nameof(Less), Less, false)];
        public static RuleScenario<(string? text, decimal min)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsLessThan
    {
        public static readonly (string? text, decimal max) Less = ("0", 1m);
        public static readonly (string? text, decimal max) Equal = ("1", 1m);
        public static readonly (string? text, decimal max) NullValue = (null, 1m);
        public static readonly (string? text, decimal max) Letters = ("abc", 1m);

        public static RuleScenario<(string? text, decimal max)>[] ValidScenarios => [new(nameof(Less), Less, true)];
        public static RuleScenario<(string? text, decimal max)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal max)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal max)>[] InvalidEdgeScenarios => [new(nameof(Equal), Equal, false)];
        public static RuleScenario<(string? text, decimal max)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal max)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal max)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsLessThanOrEqual
    {
        public static readonly (string? text, decimal max) Less = ("0", 1m);
        public static readonly (string? text, decimal max) Equal = ("1", 1m);
        public static readonly (string? text, decimal max) Greater = ("2", 1m);
        public static readonly (string? text, decimal max) NullValue = (null, 1m);
        public static readonly (string? text, decimal max) Letters = ("abc", 1m);

        public static RuleScenario<(string? text, decimal max)>[] ValidScenarios => [new(nameof(Less), Less, true)];
        public static RuleScenario<(string? text, decimal max)>[] ValidEdgeScenarios => [new(nameof(Equal), Equal, true)];
        public static RuleScenario<(string? text, decimal max)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal max)>[] InvalidEdgeScenarios => [new(nameof(Greater), Greater, false)];
        public static RuleScenario<(string? text, decimal max)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal max)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal max)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsInRange
    {
        public static readonly (string? text, decimal min, decimal max, Inclusion inclusion) BetweenInclusive = ("5", 1m, 10m, Inclusion.Inclusive);
        public static readonly (string? text, decimal min, decimal max, Inclusion inclusion) AtMinExclusive = ("1", 1m, 10m, Inclusion.Exclusive);
        public static readonly (string? text, decimal min, decimal max, Inclusion inclusion) NullValue = (null, 1m, 10m, Inclusion.Inclusive);
        public static readonly (string? text, decimal min, decimal max, Inclusion inclusion) InvalidRange = ("5", 10m, 1m, Inclusion.Inclusive);
        public static readonly (string? text, decimal min, decimal max, Inclusion inclusion) Letters = ("abc", 1m, 10m, Inclusion.Inclusive);

        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(BetweenInclusive), BetweenInclusive, true)];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(InvalidRange), InvalidRange, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(AtMinExclusive), AtMinExclusive, false)];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal min, decimal max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsApproximately
    {
        public static readonly (string? text, decimal target, decimal? tolerance) WithinTolerance = ("10.0", 10.1m, 0.2m);
        public static readonly (string? text, decimal target, decimal? tolerance) OutsideTolerance = ("10.0", 10.3m, 0.2m);
        public static readonly (string? text, decimal target, decimal? tolerance) NullValue = (null, 10.0m, 0.2m);
        public static readonly (string? text, decimal target, decimal? tolerance) Letters = ("abc", 10.0m, 0.2m);
        public static readonly (string? text, decimal target, decimal? tolerance) NullTolerance = ("10.0", 10.0m, null);
        public static readonly (string? text, decimal target, decimal? tolerance) NegativeTolerance = ("10.0", 10.0m, -0.1m);

        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] ValidScenarios => [new(nameof(WithinTolerance), WithinTolerance, true)];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] InvalidScenarios => [new(nameof(OutsideTolerance), OutsideTolerance, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] InvalidEdgeScenarios => [new(nameof(NullTolerance), NullTolerance, false), new(nameof(NegativeTolerance), NegativeTolerance, false)];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal target, decimal? tolerance)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsMultipleOf
    {
        public static readonly (string? text, decimal factor) Multiple = ("4", 2m);
        public static readonly (string? text, decimal factor) NotMultiple = ("5", 2m);
        public static readonly (string? text, decimal factor) NullValue = (null, 2m);
        public static readonly (string? text, decimal factor) ZeroFactor = ("4", 0m);
        public static readonly (string? text, decimal factor) Letters = ("abc", 2m);

        public static RuleScenario<(string? text, decimal factor)>[] ValidScenarios => [new(nameof(Multiple), Multiple, true)];
        public static RuleScenario<(string? text, decimal factor)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal factor)>[] InvalidScenarios => [new(nameof(NotMultiple), NotMultiple, false), new(nameof(NullValue), NullValue, false), new(nameof(ZeroFactor), ZeroFactor, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<(string? text, decimal factor)>[] InvalidEdgeScenarios => [];
        public static RuleScenario<(string? text, decimal factor)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? text, decimal factor)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? text, decimal factor)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NumbersIsEven
    {
        public static readonly string? Even = "4";
        public static readonly string? Odd = "5";
        public static readonly string? Decimal = "4.0";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Even), Even, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Odd), Odd, false), new(nameof(Decimal), Decimal, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsOdd
    {
        public static readonly string? Odd = "5";
        public static readonly string? Even = "4";
        public static readonly string? Decimal = "5.0";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Odd), Odd, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Even), Even, false), new(nameof(Decimal), Decimal, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsFinite
    {
        public static readonly string? Finite = "1.23";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Finite), Finite, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NumbersIsNaN
    {
        public static readonly string? NaN = "NaN";
        public static readonly string? Finite = "1.23";
        public static readonly string? NullValue = null;
        public static readonly string? Letters = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(NaN), NaN, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Finite), Finite, false), new(nameof(NullValue), NullValue, false), new(nameof(Letters), Letters, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
