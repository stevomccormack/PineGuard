using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.BitWiseRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentBitWiseExtensionsTestData
{
    public static class HasAllBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasAllBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAllBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasAllBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Flags must contain all required bits.")
        });
    }

    public static class NotHasAllBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasAllBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAllBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasAllBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(false, "Flags must not contain all required bits."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasAnyBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasAnyBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAnyBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasAnyBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Flags must contain at least one required bit.")
        });
    }

    public static class NotHasAnyBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasAnyBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAnyBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasAnyBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(false, "Flags must not contain any of the specified bits."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasNoBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasNoBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasNoBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasNoBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Flags must contain none of the forbidden bits.")
        });
    }

    public static class NotHasNoBits
    {
        public static TheoryData<FluentCase<(int? value, int mask)>> Cases => F.HasNoBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasNoBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasNoBits.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(false, "Flags must contain at least one of the forbidden bits."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasOnlyBits
    {
        public static TheoryData<FluentCase<(int? value, int allowedMask)>> Cases => F.HasOnlyBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasOnlyBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasOnlyBits.ZeroMask) => new FluentExpected(false, "allowedMask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Flags must contain only allowed bits.")
        });
    }

    public static class NotHasOnlyBits
    {
        public static TheoryData<FluentCase<(int? value, int allowedMask)>> Cases => F.HasOnlyBits.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasOnlyBits.ValueNull) => new FluentExpected(true),
            nameof(F.HasOnlyBits.ZeroMask) => new FluentExpected(false, "allowedMask must be a valid bitwise mask."),
            _ when s.IsValid => new FluentExpected(false, "Flags must contain bits not allowed by the mask."),
            _ => new FluentExpected(true)
        });
    }

    public static class PowerOfTwo
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsPowerOfTwo.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPowerOfTwo.ValueNull) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Flags must be a power of two.")
        });
    }

    public static class NotPowerOfTwo
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsPowerOfTwo.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPowerOfTwo.ValueNull) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Flags must not be a power of two."),
            _ => new FluentExpected(true)
        });
    }

    public static class BitwiseEqualTo
    {
        public static TheoryData<FluentCase<(int? left, int? right, int mask)>> Cases =>
            F.IsBitwiseEqualTo.AllScenarios
                .Where(s => s.Name != nameof(F.IsBitwiseEqualTo.RightNull))
                .ToArray()
                .ToFluentCases(s => s.Name switch
                {
                    nameof(F.IsBitwiseEqualTo.LeftNull) => new FluentExpected(true),
                    nameof(F.IsBitwiseEqualTo.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
                    _ when s.IsValid => new FluentExpected(true),
                    _ => new FluentExpected(false, "Left must be bitwise equal to the expected value.")
                });
    }

    public static class NotBitwiseEqualTo
    {
        public static TheoryData<FluentCase<(int? left, int? right, int mask)>> Cases =>
            F.IsBitwiseEqualTo.AllScenarios
                .Where(s => s.Name != nameof(F.IsBitwiseEqualTo.RightNull))
                .ToArray()
                .ToFluentCases(s => s.Name switch
                {
                    nameof(F.IsBitwiseEqualTo.LeftNull) => new FluentExpected(true),
                    nameof(F.IsBitwiseEqualTo.ZeroMask) => new FluentExpected(false, "mask must be a valid bitwise mask."),
                    _ when s.IsValid => new FluentExpected(false, "Left must not be bitwise equal to the expected value."),
                    _ => new FluentExpected(true)
                });
    }
}
