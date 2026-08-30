using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentFileSignatureExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public byte[]? Value { get; init; } }

    private sealed class FileSignatureValidator : AbstractValidator<Model>
    {
        public FileSignatureValidator(string extension) => RuleFor(x => x.Value).FileSignature(extension);
    }

    private sealed class KnownFileSignatureValidator : AbstractValidator<Model>
    {
        public KnownFileSignatureValidator() => RuleFor(x => x.Value).KnownFileSignature();
    }

    // FluentFileSignatureExtensions.FileSignature
    [Theory]
    [MemberData(nameof(FluentFileSignatureExtensionsTestData.FileSignature.Cases), MemberType = typeof(FluentFileSignatureExtensionsTestData.FileSignature))]
    public void FileSignature_BehavesAsExpected(FluentCase<(byte[]? value, string extension)> tc)
    {
        // Act
        var result = new FileSignatureValidator(tc.Value.extension).Validate(new Model { Value = tc.Value.value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentFileSignatureExtensions.KnownFileSignature
    [Theory]
    [MemberData(nameof(FluentFileSignatureExtensionsTestData.KnownFileSignature.Cases), MemberType = typeof(FluentFileSignatureExtensionsTestData.KnownFileSignature))]
    public void KnownFileSignature_BehavesAsExpected(FluentCase<byte[]?> tc)
    {
        // Act
        var result = new KnownFileSignatureValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
