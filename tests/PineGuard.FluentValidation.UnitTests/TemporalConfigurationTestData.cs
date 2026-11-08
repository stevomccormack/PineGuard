using System.Globalization;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class TemporalConfigurationTestData
{
    public sealed record Case(string Name, (string? Input, Inclusion Inclusion, DateTimeStyles Styles) Value, bool Expected)
        : ReturnCase<(string? Input, Inclusion Inclusion, DateTimeStyles Styles), bool>(Name, Value, Expected);

    public static class Configuration
    {
        public static TheoryData<Case> Cases =>
        [
            new("invalid-inclusion-valid-input", ("2024-01-15", (Inclusion)999, DateTimeStyles.None), false),
            new("invalid-inclusion-malformed-input", ("invalid", (Inclusion)999, DateTimeStyles.None), false),
            new("invalid-styles-valid-input", ("2024-01-15", Inclusion.Inclusive, DateTimeStyles.AssumeUniversal), false),
            new("invalid-styles-malformed-input", ("invalid", Inclusion.Inclusive, DateTimeStyles.AssumeUniversal), false),
            new("optional-null-rejects-configuration", (null, (Inclusion)999, (DateTimeStyles)999), false),
            new("optional-null-valid-configuration", (null, Inclusion.Inclusive, DateTimeStyles.None), true),
            new("valid-configuration", ("2024-01-15", Inclusion.Inclusive, DateTimeStyles.None), true)
        ];
    }
}
