using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentBitWiseExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record FlagsModel { public int? Flags { get; init; } }
    private sealed record BitwiseModel { public int? Left { get; init; } }

    private sealed class HasAllBitsValidator : AbstractValidator<FlagsModel>
    {
        public HasAllBitsValidator(string mask) => RuleFor(x => x.Flags).HasAllBits(mask);
    }

    private sealed class NotHasAllBitsValidator : AbstractValidator<FlagsModel>
    {
        public NotHasAllBitsValidator(string mask) => RuleFor(x => x.Flags).NotHasAllBits(mask);
    }

    private sealed class HasAnyBitsValidator : AbstractValidator<FlagsModel>
    {
        public HasAnyBitsValidator(string mask) => RuleFor(x => x.Flags).HasAnyBits(mask);
    }

    private sealed class NotHasAnyBitsValidator : AbstractValidator<FlagsModel>
    {
        public NotHasAnyBitsValidator(string mask) => RuleFor(x => x.Flags).NotHasAnyBits(mask);
    }

    private sealed class HasNoBitsValidator : AbstractValidator<FlagsModel>
    {
        public HasNoBitsValidator(string mask) => RuleFor(x => x.Flags).HasNoBits(mask);
    }

    private sealed class NotHasNoBitsValidator : AbstractValidator<FlagsModel>
    {
        public NotHasNoBitsValidator(string mask) => RuleFor(x => x.Flags).NotHasNoBits(mask);
    }

    private sealed class HasOnlyBitsValidator : AbstractValidator<FlagsModel>
    {
        public HasOnlyBitsValidator(string allowedMask) => RuleFor(x => x.Flags).HasOnlyBits(allowedMask);
    }

    private sealed class NotHasOnlyBitsValidator : AbstractValidator<FlagsModel>
    {
        public NotHasOnlyBitsValidator(string allowedMask) => RuleFor(x => x.Flags).NotHasOnlyBits(allowedMask);
    }

    private sealed class PowerOfTwoValidator : AbstractValidator<FlagsModel>
    {
        public PowerOfTwoValidator() => RuleFor(x => x.Flags).PowerOfTwo();
    }

    private sealed class NotPowerOfTwoValidator : AbstractValidator<FlagsModel>
    {
        public NotPowerOfTwoValidator() => RuleFor(x => x.Flags).NotPowerOfTwo();
    }

    private sealed class BitwiseEqualToValidator : AbstractValidator<BitwiseModel>
    {
        public BitwiseEqualToValidator(int other, string mask) => RuleFor(x => x.Left).BitwiseEqualTo(other, mask);
    }

    private sealed class NotBitwiseEqualToValidator : AbstractValidator<BitwiseModel>
    {
        public NotBitwiseEqualToValidator(int other, string mask) => RuleFor(x => x.Left).NotBitwiseEqualTo(other, mask);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.HasAllBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.HasAllBits))]
    public void HasAllBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new HasAllBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotHasAllBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotHasAllBits))]
    public void NotHasAllBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new NotHasAllBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.HasAnyBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.HasAnyBits))]
    public void HasAnyBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new HasAnyBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotHasAnyBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotHasAnyBits))]
    public void NotHasAnyBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new NotHasAnyBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.HasNoBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.HasNoBits))]
    public void HasNoBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new HasNoBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotHasNoBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotHasNoBits))]
    public void NotHasNoBits_BehavesAsExpected(FluentCase<(int? value, int mask)> tc)
    {
        var result = new NotHasNoBitsValidator(tc.Value.mask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.HasOnlyBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.HasOnlyBits))]
    public void HasOnlyBits_BehavesAsExpected(FluentCase<(int? value, int allowedMask)> tc)
    {
        var result = new HasOnlyBitsValidator(tc.Value.allowedMask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotHasOnlyBits.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotHasOnlyBits))]
    public void NotHasOnlyBits_BehavesAsExpected(FluentCase<(int? value, int allowedMask)> tc)
    {
        var result = new NotHasOnlyBitsValidator(tc.Value.allowedMask.ToString()).Validate(new FlagsModel { Flags = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.PowerOfTwo.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.PowerOfTwo))]
    public void PowerOfTwo_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new PowerOfTwoValidator().Validate(new FlagsModel { Flags = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotPowerOfTwo.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotPowerOfTwo))]
    public void NotPowerOfTwo_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotPowerOfTwoValidator().Validate(new FlagsModel { Flags = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.BitwiseEqualTo.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.BitwiseEqualTo))]
    public void BitwiseEqualTo_BehavesAsExpected(FluentCase<(int? left, int? right, int mask)> tc)
    {
        var result = new BitwiseEqualToValidator(tc.Value.right ?? 0, tc.Value.mask.ToString()).Validate(new BitwiseModel { Left = tc.Value.left });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBitWiseExtensionsTestData.NotBitwiseEqualTo.Cases), MemberType = typeof(FluentBitWiseExtensionsTestData.NotBitwiseEqualTo))]
    public void NotBitwiseEqualTo_BehavesAsExpected(FluentCase<(int? left, int? right, int mask)> tc)
    {
        var result = new NotBitwiseEqualToValidator(tc.Value.right ?? 0, tc.Value.mask.ToString()).Validate(new BitwiseModel { Left = tc.Value.left });
        AssertResult(tc, result);
    }
}
