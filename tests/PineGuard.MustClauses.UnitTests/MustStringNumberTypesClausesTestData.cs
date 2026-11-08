using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

#pragma warning disable CS0618
public static class MustStringNumberTypesClausesTestData
{
    public static class Decimal
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsDecimal.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsDecimal.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsDecimal.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Format.NotDecimal),
            _ => new MustExpected(false, "value must be a decimal number.", Code: MustCodes.Number.Format.NotDecimal)
        });
    }

    public static class DecimalNegativePlaces
    {
        public static TheoryData<Case> Cases =>
        [
            new("neg decimalPlaces", "1.23", false, "decimalPlaces", -1)
        ];

        public sealed record Case(string Name, string? Value, bool Expected, string ParamName, int DecimalPlaces)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class ExactDecimal
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsExactDecimal.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsExactDecimal.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsExactDecimal.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Scale.Mismatch),
            _ => new MustExpected(false, "value must be an exact decimal number.", Code: MustCodes.Number.Scale.Mismatch)
        });
    }

    public static class ExactDecimalNegativePlaces
    {
        public static TheoryData<Case> Cases =>
        [
            new("neg exactDecimalPlaces", "1.23", false, "exactDecimalPlaces", -1)
        ];

        public sealed record Case(string Name, string? Value, bool Expected, string ParamName, int ExactDecimalPlaces)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class Int32
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsInt32.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsInt32.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsInt32.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Format.NotInt32),
            _ => new MustExpected(false, "value must be a 32-bit integer.", Code: MustCodes.Number.Format.NotInt32)
        });
    }

    public static class Int64
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsInt64.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsInt64.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsInt64.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Format.NotInt64),
            _ => new MustExpected(false, "value must be a 64-bit integer.", Code: MustCodes.Number.Format.NotInt64)
        });
    }

    public static class InRangeInt32
    {
        public static TheoryData<MustCase<(string text, int min, int max, Inclusion inclusion)>> ValidCases =>
            F.IsInRangeInt32.AllValid.ToMustCases();

        public static TheoryData<MustCase<(string text, int min, int max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRangeInt32.AllInvalid.ToMustCases(_ => new MustExpected(false, "value must be a 32-bit integer within the expected range.", Code: MustCodes.Number.Range.OutOfRange));

        public static TheoryData<NullCase> NullCases =>
        [
            new("null value", (null!, 1, 10, Inclusion.Inclusive), false, "value")
        ];

        public static TheoryData<RangeCase> RangeCases =>
        [
            new("min > max", ("5", 10, 1, Inclusion.Inclusive), false, "min")
        ];

        public sealed record NullCase(string Name, (string? text, int min, int max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string? text, int min, int max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record RangeCase(string Name, (string text, int min, int max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string text, int min, int max, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class OutOfRangeInt32
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("out of range", ("15", 1, 10, Inclusion.Inclusive), true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("in range", ("5", 1, 10, Inclusion.Inclusive), false),
            new("not numeric", ("not", 1, 10, Inclusion.Inclusive), false)
        ];

        public static TheoryData<NullCase> NullCases =>
        [
            new("null value", (null!, 1, 10, Inclusion.Inclusive), false, "value")
        ];

        public static TheoryData<RangeCase> RangeCases =>
        [
            new("min > max", ("5", 10, 1, Inclusion.Inclusive), false, "min")
        ];

        public sealed record ValidCase(string Name, (string? text, int min, int max, Inclusion inclusion) Value, bool Expected)
            : IsCase<(string? text, int min, int max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record NullCase(string Name, (string? text, int min, int max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string? text, int min, int max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record RangeCase(string Name, (string text, int min, int max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string text, int min, int max, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class InRangeInt64
    {
        public static TheoryData<MustCase<(string text, long min, long max, Inclusion inclusion)>> ValidCases =>
            F.IsInRangeInt64.AllValid.ToMustCases();

        public static TheoryData<MustCase<(string text, long min, long max, Inclusion inclusion)>> InvalidCases =>
            F.IsInRangeInt64.AllInvalid.ToMustCases(_ => new MustExpected(false, "value must be a 64-bit integer within the expected range.", Code: MustCodes.Number.Range.OutOfRange));

        public static TheoryData<NullCase> NullCases =>
        [
            new("null value", (null!, 1L, 10L, Inclusion.Inclusive), false, "value")
        ];

        public static TheoryData<RangeCase> RangeCases =>
        [
            new("min > max", ("5", 10L, 1L, Inclusion.Inclusive), false, "min")
        ];

        public sealed record NullCase(string Name, (string? text, long min, long max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string? text, long min, long max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record RangeCase(string Name, (string text, long min, long max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string text, long min, long max, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class OutOfRangeInt64
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("out of range", ("15", 1L, 10L, Inclusion.Inclusive), true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("in range", ("5", 1L, 10L, Inclusion.Inclusive), false),
            new("not numeric", ("not", 1L, 10L, Inclusion.Inclusive), false)
        ];

        public static TheoryData<NullCase> NullCases =>
        [
            new("null value", (null!, 1L, 10L, Inclusion.Inclusive), false, "value")
        ];

        public static TheoryData<RangeCase> RangeCases =>
        [
            new("min > max", ("5", 10L, 1L, Inclusion.Inclusive), false, "min")
        ];

        public sealed record ValidCase(string Name, (string? text, long min, long max, Inclusion inclusion) Value, bool Expected)
            : IsCase<(string? text, long min, long max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record NullCase(string Name, (string? text, long min, long max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string? text, long min, long max, Inclusion inclusion)>(Name, Value, Expected);

        public sealed record RangeCase(string Name, (string text, long min, long max, Inclusion inclusion) Value, bool Expected, string ParamName)
            : IsCase<(string text, long min, long max, Inclusion inclusion)>(Name, Value, Expected);
    }
    public static class ConfigurationPrecedence
    {
        public static TheoryData<MustCase<Func<MustResult<decimal>>>> DecimalCases =>
        [
            new("DecimalStyleBeforeNull", () => Must.Be.Decimal(null, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Format.NotDecimal)),
            new("ExactDecimalStyleBeforeNull", () => Must.Be.ExactDecimal(null, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Scale.Mismatch))
        ];

        public static TheoryData<MustCase<Func<MustResult<long>>>> Int64Cases =>
        [
            new("Int64StyleBeforeNull", () => Must.Be.Int64(null, (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Format.NotInt64)),
            new("InclusionBeforeNull", () => Must.Be.InRangeInt64(null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.", "inclusion", MustCodes.Number.Range.OutOfRange)),
            new("OutInclusionBeforeNull", () => Must.Be.OutOfRangeInt64(null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.", "inclusion", MustCodes.Number.Range.InRange)),
            new("RangeStyleBeforeNull", () => Must.Be.InRangeInt64(null, 0, 1, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Range.OutOfRange)),
            new("OutRangeStyleBeforeNull", () => Must.Be.OutOfRangeInt64(null, 0, 1, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Range.InRange))
        ];

        public static TheoryData<MustCase<Func<MustResult<int>>>> Cases =>
        [
            new("RangeStyleBeforeNull", () => Must.Be.InRangeInt32(null, 0, 1, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Range.OutOfRange)),
            new("OutRangeStyleBeforeNull", () => Must.Be.OutOfRangeInt32(null, 0, 1, styles: (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Range.InRange)),
            new("InvalidStyleBeforeNull", () => Must.Be.Int32(null, (NumberStyles)(-1)), new(false, "styles must be a valid number style.", "styles", MustCodes.Number.Format.NotInt32)),
            new("InclusionBeforeNull", () => Must.Be.InRangeInt32(null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.", "inclusion", MustCodes.Number.Range.OutOfRange)),
            new("OutInclusionBeforeNull", () => Must.Be.OutOfRangeInt32(null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.", "inclusion", MustCodes.Number.Range.InRange)),
            new("BoundsBeforeNull", () => Must.Be.InRangeInt32(null, 2, 1), new(false, "min requires a valid range.", "min", MustCodes.Number.Range.Invalid)),
            new("NegativeBoundsBeforeNull", () => Must.Be.OutOfRangeInt32(null, 2, 1), new(false, "min requires a valid range.", "min", MustCodes.Number.Range.Invalid))
        ];
    }
}
