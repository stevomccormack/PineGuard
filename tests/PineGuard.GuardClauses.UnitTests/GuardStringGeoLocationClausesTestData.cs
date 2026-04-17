using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringGeoLocationClausesTestData
{
    public static class NotLatitude
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLatitude.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLatitude.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotLongitude
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLongitude.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLongitude.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotGeoLocation
    {
        public static TheoryData<GuardCase<(string? latitude, string? longitude)>> ValidCases => F.IsGeoLocation.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? latitude, string? longitude)>> InvalidCases => F.IsGeoLocation.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "latitude"));
    }
}
