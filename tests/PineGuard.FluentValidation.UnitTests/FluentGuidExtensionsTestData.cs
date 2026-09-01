using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentGuidExtensionsTestData
{
    public static class NotEmpty
    {
        public static TheoryData<FluentCase<Guid>> Cases => F.NotEmpty.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(true)
            : new FluentExpected(false, "Value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty));
    }

    public static class NotEmptyNullable
    {
        public static TheoryData<FluentCase<Guid?>> Cases => F.IsNotEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotEmpty.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be an empty GUID.")
        });
    }

    public static class HasGuidVersion
    {
        public static TheoryData<FluentCase<(Guid value, int version)>> Cases =>
            F.HasVersion.AllScenarios
                .Except(nameof(F.HasVersion.NullValue))
                .Project(v => (value: v.value!.Value, v.version))
                .ToFluentCases(s => s.Name switch
                {
                    nameof(F.HasVersion.VersionBelowMin) or nameof(F.HasVersion.VersionAboveMax) or nameof(F.HasVersion.NegativeVersion) =>
                        new FluentExpected(false, "version requires a value between 1 and 8.", Code: MustCodes.Guid.Version.Mismatch),
                    _ when s.IsValid => new FluentExpected(true),
                    _ => new FluentExpected(false, "Value must have the specified GUID version.", Code: MustCodes.Guid.Version.Mismatch)
                });
    }

    public static class HasGuidVersionNullable
    {
        public static TheoryData<FluentCase<(Guid? value, int version)>> Cases => F.HasVersion.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasVersion.NullValue) => new FluentExpected(true),
            nameof(F.HasVersion.VersionBelowMin) or nameof(F.HasVersion.VersionAboveMax) or nameof(F.HasVersion.NegativeVersion) =>
                new FluentExpected(false, "version requires a value between 1 and 8.", Code: MustCodes.Guid.Version.Mismatch),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the specified GUID version.", Code: MustCodes.Guid.Version.Mismatch)
        });
    }
}
