using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class GuidRulesFixtures
{
    public static class IsEmpty
    {
        public static readonly Guid? Null = null;
        public static readonly Guid? Empty = Guid.Empty;
        public static readonly Guid? NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static RuleScenario<Guid?>[] ValidScenarios =>
        [
            new(nameof(Empty), Empty, true)
        ];

        public static RuleScenario<Guid?>[] InvalidScenarios =>
        [
            new(nameof(Null),  Null,  false),
            new(nameof(NonEmpty), NonEmpty, false)
        ];

        public static RuleScenario<Guid?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotEmpty
    {
        public static readonly Guid? Null = null;
        public static readonly Guid? Empty = Guid.Empty;
        public static readonly Guid? NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public static RuleScenario<Guid?>[] ValidScenarios =>
        [
            new(nameof(NonEmpty), NonEmpty, true)
        ];

        public static RuleScenario<Guid?>[] InvalidScenarios =>
        [
            new(nameof(Null),  Null,  false),
            new(nameof(Empty), Empty, false)
        ];

        public static RuleScenario<Guid?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasVersion
    {
        public static readonly (Guid? value, int version) Version1 = (Guid.Parse("d9428888-122b-11e1-b85c-61cd3cbb3210"), 1);
        public static readonly (Guid? value, int version) Version4 = (Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), 4);
        public static readonly (Guid? value, int version) Version5 = (Guid.Parse("74738ff5-5367-5958-9aee-98fffdcd1876"), 5);
        public static readonly (Guid? value, int version) Version7 = (Guid.Parse("017f22e2-79b0-7cc3-98c4-dc0c0c07398f"), 7);
        public static readonly (Guid? value, int version) AtMinVersion = (Guid.Parse("d9428888-122b-11e1-b85c-61cd3cbb3210"), GuidRules.MinVersion);
        public static readonly (Guid? value, int version) AtMaxVersion = (Guid.Parse("320c3d4d-cc00-875b-8ec9-32d5f69181c0"), GuidRules.MaxVersion);
        public static readonly (Guid? value, int version) Mismatch = (Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), 7);
        public static readonly (Guid? value, int version) EmptyGuid = (Guid.Empty, 4);
        public static readonly (Guid? value, int version) NullValue = (null, 4);
        public static readonly (Guid? value, int version) VersionBelowMin = (Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), GuidRules.MinVersion - 1);
        public static readonly (Guid? value, int version) VersionAboveMax = (Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), GuidRules.MaxVersion + 1);
        public static readonly (Guid? value, int version) NegativeVersion = (Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), -1);

        public static RuleScenario<(Guid? value, int version)>[] ValidScenarios => [new(nameof(Version1), Version1, true), new(nameof(Version4), Version4, true), new(nameof(Version5), Version5, true), new(nameof(Version7), Version7, true)];
        public static RuleScenario<(Guid? value, int version)>[] ValidEdgeScenarios => [new(nameof(AtMinVersion), AtMinVersion, true), new(nameof(AtMaxVersion), AtMaxVersion, true)];
        public static RuleScenario<(Guid? value, int version)>[] InvalidScenarios => [new(nameof(Mismatch), Mismatch, false), new(nameof(EmptyGuid), EmptyGuid, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(Guid? value, int version)>[] InvalidEdgeScenarios => [new(nameof(VersionBelowMin), VersionBelowMin, false), new(nameof(VersionAboveMax), VersionAboveMax, false), new(nameof(NegativeVersion), NegativeVersion, false)];
        public static RuleScenario<(Guid? value, int version)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(Guid? value, int version)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(Guid? value, int version)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class NotEmpty
    {
        public static readonly Guid NonEmpty = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid Empty = Guid.Empty;

        public static RuleScenario<Guid>[] ValidScenarios =>
        [
            new(nameof(NonEmpty), NonEmpty, true)
        ];

        public static RuleScenario<Guid>[] InvalidScenarios =>
        [
            new(nameof(Empty), Empty, false)
        ];

        public static RuleScenario<Guid>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
