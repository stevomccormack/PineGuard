using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.UriRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardUriClausesTestData
{
    public static class RelativeUri
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsAbsoluteUri.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsAbsoluteUri.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class AbsoluteUri
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsRelativeUri.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsRelativeUri.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotUrl
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsUrl.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsUrl.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHttpsUrl
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHttpsUrl.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHttpsUrl.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHttpUrl
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHttpUrl.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHttpUrl.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotFileUri
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsFileUri.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsFileUri.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotFilePath
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsFilePath.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsFilePath.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class FilePath
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsFilePath.InvalidScenarios.Where(s => !s.IsNull).ToArray().ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
            [
                .. F.IsFilePath.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
                .. F.IsFilePath.InvalidScenarios.Where(s => s.IsNull).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))
            ];
    }

    public static class NotHasScheme
    {
        public static TheoryData<GuardCase<(string? value, string scheme)>> ValidCases =>
            F.HasScheme.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, string scheme)>> InvalidCases =>
            F.HasScheme.InvalidScenarios.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasScheme
    {
        public static TheoryData<GuardCase<(string? value, string scheme)>> ValidCases =>
            F.HasScheme.InvalidScenarios.Where(s => s.Inputs.value is not null).ToArray().ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, string scheme)>> InvalidCases =>
            [
                .. F.HasScheme.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
                .. F.HasScheme.InvalidScenarios.Where(s => s.Inputs.value is null).ToArray().ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"))
            ];
    }
}
