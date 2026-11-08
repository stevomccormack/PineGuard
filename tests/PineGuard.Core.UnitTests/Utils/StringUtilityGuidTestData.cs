using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityGuidTestData
{
    public static class TryParse
    {
        public static TheoryData<RuleCase<(string? value, Guid parsed)>> Cases =>
        [
            new(nameof(F.GuidIsGuid.ValidGuid), (F.GuidIsGuid.ValidGuid, Guid.Parse(F.GuidIsGuid.ValidGuid!)), new(true)),
            new(nameof(F.GuidIsGuid.ValidGuidN), (F.GuidIsGuid.ValidGuidN, Guid.Parse(F.GuidIsGuid.ValidGuid!)), new(true)),
            new(nameof(F.GuidIsEmpty.BracedEmptyGuid), (F.GuidIsEmpty.BracedEmptyGuid, Guid.Empty), new(true)),
            new(nameof(F.GuidIsEmpty.PaddedEmptyGuid), (F.GuidIsEmpty.PaddedEmptyGuid, Guid.Empty), new(true)),
            new(nameof(F.GuidIsGuid.NotAGuid), (F.GuidIsGuid.NotAGuid, Guid.Empty), new(false)),
            new(nameof(F.GuidIsGuid.NullValue), (F.GuidIsGuid.NullValue, Guid.Empty), new(false)),
            new(nameof(F.GuidIsGuid.Space), (F.GuidIsGuid.Space, Guid.Empty), new(false)),
            new(nameof(F.GuidIsGuid.Empty), (F.GuidIsGuid.Empty, Guid.Empty), new(false))
        ];
    }
}
