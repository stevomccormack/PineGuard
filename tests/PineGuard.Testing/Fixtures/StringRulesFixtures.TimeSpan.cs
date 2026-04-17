using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── TimeSpan ────────────────────────────────────────────────────

    public static class TimeSpanIsDurationBetween
    {
        public static readonly (string? value, TimeSpan min, TimeSpan max, Inclusion inclusion) InsideRange = ("00:30:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AtMinInclusive = ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan min, TimeSpan max, Inclusion inclusion) AtMinExclusive = ("00:10:00", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Exclusive);
        public static readonly (string? value, TimeSpan min, TimeSpan max, Inclusion inclusion) NotADuration = ("not-a-duration", TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan min, TimeSpan max, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(60), Inclusion.Inclusive);

        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(InsideRange), InsideRange, true)];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(AtMinInclusive), AtMinInclusive, true)];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotADuration), NotADuration, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(AtMinExclusive), AtMinExclusive, false)];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class TimeSpanIsGreaterThan
    {
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) GreaterExclusive = ("00:11:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) EqualInclusive = ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) EqualExclusive = ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) NotADuration = ("not-a-duration", TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), Inclusion.Inclusive);

        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] ValidScenarios => [new(nameof(GreaterExclusive), GreaterExclusive, true)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(EqualInclusive), EqualInclusive, true)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotADuration), NotADuration, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(EqualExclusive), EqualExclusive, false)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class TimeSpanIsLessThan
    {
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) LessExclusive = ("00:09:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) EqualInclusive = ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) EqualExclusive = ("00:10:00", TimeSpan.FromMinutes(10), Inclusion.Exclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) NotADuration = ("not-a-duration", TimeSpan.FromMinutes(10), Inclusion.Inclusive);
        public static readonly (string? value, TimeSpan threshold, Inclusion inclusion) NullValue = (null, TimeSpan.FromMinutes(10), Inclusion.Inclusive);

        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] ValidScenarios => [new(nameof(LessExclusive), LessExclusive, true)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(EqualInclusive), EqualInclusive, true)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotADuration), NotADuration, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(EqualExclusive), EqualExclusive, false)];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, TimeSpan threshold, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
