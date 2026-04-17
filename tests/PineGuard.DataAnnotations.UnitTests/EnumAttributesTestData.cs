using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class EnumAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    private enum SimpleEnum { A = 1, B = 2 }

    [Flags]
    private enum FlagsEnum { None = 0, A = 1, B = 2, C = 4 }

    public static class Defined
    {
        public static TheoryData<ValidCase> ValidCases => [new("defined", SimpleEnum.A, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("undefined", (SimpleEnum)99, false)];
    }

    public static class FlagsEnumCombination
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single flag", FlagsEnum.A, true),
            new("combined flags", FlagsEnum.A | FlagsEnum.B, true),
            new("none", FlagsEnum.None, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("undefined flag", (FlagsEnum)99, false)];
    }

    // HasFlag("A")
    public static class HasFlag
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("has flag", SimpleEnum.A, true),
            // Flags enum
            new("flags has flag", FlagsEnum.A | FlagsEnum.B, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("missing flag", SimpleEnum.B, false),
            new("flags missing flag", FlagsEnum.B | FlagsEnum.C, false)
        ];
    }

    // NotHasFlag("A")
    public static class NotHasFlag
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("missing flag", SimpleEnum.B, true),
            new("flags missing flag", FlagsEnum.B | FlagsEnum.C, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("has flag", SimpleEnum.A, false),
            new("flags has flag", FlagsEnum.A | FlagsEnum.B, false)
        ];
    }

    public static class DefinedNonEnum
    {
        private sealed record InvalidCase(string Name, object Value, ExpectedException ExpectedException)
            : ThrowsCase<object>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new InvalidCase("string value", "not an enum", new ExpectedException(typeof(InvalidOperationException), null, "can only be applied to Enum types"))
        ];
    }

    public static class FlagsEnumCombinationNonEnum
    {
        private sealed record InvalidCase(string Name, object Value, ExpectedException ExpectedException)
            : ThrowsCase<object>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new InvalidCase("int value", 42, new ExpectedException(typeof(InvalidOperationException), null, "can only be applied to Enum types"))
        ];
    }

    public static class HasFlagInvalidFlagName
    {
        private sealed record InvalidCase(string Name, object Value, ExpectedException ExpectedException)
            : ThrowsCase<object>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new InvalidCase("valid enum invalid flag", SimpleEnum.A, new ExpectedException(typeof(InvalidOperationException), null, "Flag 'NonExistentFlag' not found"))
        ];
    }

    public static class NotHasFlagInvalidFlagName
    {
        private sealed record InvalidCase(string Name, object Value, ExpectedException ExpectedException)
            : ThrowsCase<object>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new InvalidCase("valid enum invalid flag", SimpleEnum.A, new ExpectedException(typeof(InvalidOperationException), null, "Flag 'NonExistentFlag' not found"))
        ];
    }

    public static class DefinedWithErrorMessage
    {
        public static TheoryData<ValidCase> Cases =>
        [
            new("defined valid", SimpleEnum.A, true),
            new("undefined invalid", (SimpleEnum)99, false)
        ];
    }
}
