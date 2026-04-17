using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.FilePathRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardFilePathClausesTestData
{
    public static class NotSafeFileName
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsSafeFileName.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsSafeFileName.InvalidScenarios.ToGuardCases("fileName");
    }

    public static class NotHasFileExtension
    {
        public static TheoryData<GuardCase<(string? path, string[]? allowed)>> ValidCases =>
            F.HasFileExtension.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? path, string[]? allowed)>> InvalidCases =>
            F.HasFileExtension.InvalidScenarios.ToGuardCases(s => s.Name switch
            {
                nameof(F.HasFileExtension.NullPath) => new GuardExpected(false, typeof(ArgumentNullException), "path"),
                _ => new GuardExpected(false, typeof(ArgumentException), "path")
            });
    }
}
