using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardFileSignatureClausesTestData
{
    // Guard.Against.NotFileSignature — throws when the header does NOT match the declared extension (delegates to Must.Be.FileSignature)
    public static class NotFileSignature
    {
        public static TheoryData<GuardCase<(byte[]? value, string extension)>> ValidCases => F.HasSignature.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<(byte[]? value, string extension)>> InvalidCases =>
        [
            .. F.HasSignature.InvalidScenarios.ToGuardCases(s => new GuardExpected(false, s.Inputs.value is null ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.File.Signature.Mismatch)),
            .. F.IsKnownExtension.InvalidScenarios.Project<string?, (byte[]? value, string extension)>(extension => (F.Png, extension!)).ToGuardCases(s => new GuardExpected(false, s.Inputs.extension is null ? typeof(ArgumentNullException) : typeof(ArgumentException), "extension", Code: MustCodes.File.Signature.Unknown))
        ];
    }

    // Guard.Against.NotKnownFileSignature — throws when the header matches NO registered signature (delegates to Must.Be.KnownFileSignature)
    public static class NotKnownFileSignature
    {
        public static TheoryData<GuardCase<byte[]?>> ValidCases => F.HasKnownSignature.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<byte[]?>> InvalidCases => F.HasKnownSignature.InvalidScenarios.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.File.Signature.Unknown));
    }
}
