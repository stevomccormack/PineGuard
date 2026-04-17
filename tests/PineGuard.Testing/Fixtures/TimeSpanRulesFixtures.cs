using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class TimeSpanRulesFixtures
{
    public static class IsDurationBetween
    {
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) MiddleInclusive = (TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AtMinInclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AtMaxInclusive = (TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AtMinExclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) BelowMin = (TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AboveMax = (TimeSpan.FromMinutes(70), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);

        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleInclusive), MiddleInclusive, true),
            new(nameof(AtMinInclusive),  AtMinInclusive,  true),
            new(nameof(AtMaxInclusive),  AtMaxInclusive,  true)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] ValidEdgeScenarios =>
        [
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BelowMin),  BelowMin,  false),
            new(nameof(AboveMax),  AboveMax,  false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] InvalidEdgeScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsGreaterThan
    {
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) AboveExclusive = (TimeSpan.FromMinutes(11), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) EqualInclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) EqualExclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) BelowExclusive = (TimeSpan.FromMinutes(9), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) NullThreshold = (TimeSpan.FromMinutes(11), null, Inclusion.Exclusive);

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(AboveExclusive), AboveExclusive, true),
            new(nameof(EqualInclusive), EqualInclusive, true)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] ValidEdgeScenarios =>
        [
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BelowExclusive), BelowExclusive, false),
            new(nameof(NullValue),      NullValue,      false),
            new(nameof(NullThreshold),  NullThreshold,  false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] InvalidEdgeScenarios =>
        [
            new(nameof(EqualExclusive), EqualExclusive, false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLessThan
    {
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) BelowExclusive = (TimeSpan.FromMinutes(9), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) EqualInclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) EqualExclusive = (TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) AboveExclusive = (TimeSpan.FromMinutes(11), TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (TimeSpan? value, TimeSpan? threshold, Inclusion inclusion) NullThreshold = (TimeSpan.FromMinutes(9), null, Inclusion.Exclusive);

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(BelowExclusive), BelowExclusive, true),
            new(nameof(EqualInclusive), EqualInclusive, true)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] ValidEdgeScenarios =>
        [
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AboveExclusive), AboveExclusive, false),
            new(nameof(NullValue),      NullValue,      false),
            new(nameof(NullThreshold),  NullThreshold,  false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] InvalidEdgeScenarios =>
        [
            new(nameof(EqualExclusive), EqualExclusive, false)
        ];

        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
