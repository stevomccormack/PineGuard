using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.DefaultEqualityRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDefaultEqualityClausesTestData
{
    // Guard.Against.Default throws when value IS default (IsDefaultInt32.ValidScenarios = IS default)
    public static class Default
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsDefaultInt32.InvalidScenarios   // NOT default → doesn't throw
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsDefaultInt32.ValidScenarios     // IS default → throws AE
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotDefault throws when value is NOT default
    public static class NotDefault
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsDefaultInt32.ValidScenarios     // IS default → doesn't throw
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsDefaultInt32.InvalidScenarios   // NOT default → throws AE
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NullOrDefault throws when value IS null or default
    public static class NullOrDefault
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsNullOrDefaultString.InvalidScenarios   // NOT null/default → doesn't throw
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
            F.IsNullOrDefaultString.ValidScenarios     // IS null → throws ANE
            .ToGuardCases(s => s.IsNull
                ? new GuardExpected(false, typeof(ArgumentNullException), "value")
                : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotNullOrDefault throws when value is NOT null/default
    public static class NotNullOrDefault
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsNullOrDefaultString.ValidScenarios     // IS null/default → doesn't throw
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
            F.IsNullOrDefaultString.InvalidScenarios   // NOT null/default → throws AE
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
