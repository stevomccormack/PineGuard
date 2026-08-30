using System.Text.Json;
using PineGuard.Codes;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustValidationOptionsTestData
{
    public sealed record OptionsExpected(
        JsonNamingPolicy? PropertyNamingPolicy,
        bool UseJsonNamingPolicy,
        bool IncludeCodes,
        MustValidationMode Mode,
        bool HandleGuardExceptions,
        string UnknownGuardCode,
        string Title,
        Type? LocalizationResourceType);

    public static class Defaults
    {
        public static TheoryData<Case> Cases =>
        [
            new("freshly-constructed-options-carry-the-documented-defaults", _ => { }, new OptionsExpected(null, true, true, MustValidationMode.Aggregate, false, MustCodes.Value.Argument.Invalid, "One or more validation errors occurred.", null))
        ];

        public sealed record Case(string Name, Action<MustValidationOptions> Value, OptionsExpected Expected)
            : ReturnCase<Action<MustValidationOptions>, OptionsExpected>(Name, Value, Expected);
    }

    public static class Configuration
    {
        public static TheoryData<Defaults.Case> Cases =>
        [
            new("every-setting-round-trips", ConfigureEverything, new OptionsExpected(JsonNamingPolicy.SnakeCaseLower, false, false, MustValidationMode.StopOnFirstFailure, true, "value.state.invalid", "Your request was rejected.", typeof(MustValidationOptionsTests)))
        ];

        private static void ConfigureEverything(MustValidationOptions options)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.UseJsonNamingPolicy = false;
            options.IncludeCodes = false;
            options.Mode = MustValidationMode.StopOnFirstFailure;
            options.HandleGuardExceptions = true;
            options.UnknownGuardCode = "value.state.invalid";
            options.Title = "Your request was rejected.";
            options.LocalizationResourceType = typeof(MustValidationOptionsTests);
        }
    }
}
