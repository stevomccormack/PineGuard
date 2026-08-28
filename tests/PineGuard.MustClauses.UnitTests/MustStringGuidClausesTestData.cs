using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringGuidClausesTestData
{
    public static class Guid
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.GuidIsGuid.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.GuidIsGuid.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.GuidIsGuid.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid GUID.", Code: MustCodes.Guid.Format.Invalid)
        });
    }

    public static class NotEmptyGuid
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.GuidIsNotEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.GuidIsNotEmpty.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.GuidIsNotEmpty.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty)
        });
    }
}
