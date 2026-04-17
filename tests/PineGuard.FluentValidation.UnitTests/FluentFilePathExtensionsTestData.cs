using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.FilePathRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentFilePathExtensionsTestData
{
    public static class SafeFileName
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsSafeFileName.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSafeFileName.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a safe file name.")
        });
    }

    public static class HasFileExtension
    {
        public static TheoryData<FluentCase<(string? path, string[]? allowed)>> Cases => F.HasFileExtension.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasFileExtension.NullPath) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have an allowed file extension.")
        });
    }
}
