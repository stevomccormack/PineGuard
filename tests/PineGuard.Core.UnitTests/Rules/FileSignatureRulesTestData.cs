using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class FileSignatureRulesTestData
{
    public static class HasSignature
    {
        public static TheoryData<RuleCase<(byte[]? value, string extension)>> Cases => F.HasSignature.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new(nameof(F.IsKnownExtension.NullExtension), (F.Png, F.IsKnownExtension.NullExtension!), new ExpectedException(typeof(ArgumentNullException), "extension")),
            new(nameof(F.IsKnownExtension.EmptyExtension), (F.Png, F.IsKnownExtension.EmptyExtension!), new ExpectedException(typeof(ArgumentException), "extension")),
            new(nameof(F.IsKnownExtension.WhitespaceExtension), (F.Png, F.IsKnownExtension.WhitespaceExtension!), new ExpectedException(typeof(ArgumentException), "extension")),
            new(nameof(F.IsKnownExtension.Unregistered), (F.Png, F.IsKnownExtension.Unregistered!), new ExpectedException(typeof(ArgumentException), "extension")),
            new(nameof(F.IsKnownExtension.DotOnly), (F.Png, F.IsKnownExtension.DotOnly!), new ExpectedException(typeof(ArgumentException), "extension"))
        ];

        public sealed record InvalidCase(string Name, (byte[]? Value, string Extension) Input, ExpectedException ExpectedException)
            : ThrowsCase<(byte[]? Value, string Extension)>(Name, Input, ExpectedException);
    }

    public static class HasKnownSignature
    {
        public static TheoryData<RuleCase<byte[]?>> Cases => F.HasKnownSignature.AllScenarios.ToRuleCases();
    }
}
