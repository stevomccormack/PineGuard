using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringGraphemesClausesTestData
{
    public static class HasExactGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int count)>> ValidCases => F.GraphemesHasExactCount.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, int count)>> InvalidCases => F.GraphemesHasExactCount.AllInvalid.Except(nameof(F.GraphemesHasExactCount.NullValue), nameof(F.GraphemesHasExactCount.NegativeCount)).ToMustCases(_ => new MustExpected(false, "value must have the expected number of characters.", Code: MustCodes.Text.Graphemes.Mismatch));
        public static TheoryData<MustCase<(string? value, int count)>> NullCases => F.GraphemesHasExactCount.AllInvalid.Only(nameof(F.GraphemesHasExactCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.Mismatch));
        public static TheoryData<MustCase<(string? value, int count)>> NegativeCountCases => F.GraphemesHasExactCount.AllInvalid.Only(nameof(F.GraphemesHasExactCount.NegativeCount)).ToMustCases(_ => new MustExpected(false, "count requires a non-negative count.", "count", MustCodes.Text.Graphemes.Mismatch));
    }

    public static class NotHasExactGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int count)>> ValidCases => F.GraphemesHasExactCount.AllInvalid.Except(nameof(F.GraphemesHasExactCount.NullValue), nameof(F.GraphemesHasExactCount.NegativeCount)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, int count)>> InvalidCases => F.GraphemesHasExactCount.AllValid.ToMustCases(_ => new MustExpected(false, "value must not have the expected number of characters.", Code: MustCodes.Text.Graphemes.Match));
        public static TheoryData<MustCase<(string? value, int count)>> NullCases => F.GraphemesHasExactCount.AllInvalid.Only(nameof(F.GraphemesHasExactCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.Match));
        public static TheoryData<MustCase<(string? value, int count)>> NegativeCountCases => F.GraphemesHasExactCount.AllInvalid.Only(nameof(F.GraphemesHasExactCount.NegativeCount)).ToMustCases(_ => new MustExpected(false, "count requires a non-negative count.", "count", MustCodes.Text.Graphemes.Match));
    }

    public static class HasMinGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int min)>> ValidCases => F.GraphemesHasMinCount.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, int min)>> InvalidCases => F.GraphemesHasMinCount.AllInvalid.Except(nameof(F.GraphemesHasMinCount.NullValue), nameof(F.GraphemesHasMinCount.NegativeMin)).ToMustCases(_ => new MustExpected(false, "value must have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooFew));
        public static TheoryData<MustCase<(string? value, int min)>> NullCases => F.GraphemesHasMinCount.AllInvalid.Only(nameof(F.GraphemesHasMinCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.TooFew));
        public static TheoryData<MustCase<(string? value, int min)>> NegativeMinCases => F.GraphemesHasMinCount.AllInvalid.Only(nameof(F.GraphemesHasMinCount.NegativeMin)).ToMustCases(_ => new MustExpected(false, "min requires a non-negative minimum count.", "min", MustCodes.Text.Graphemes.TooFew));
    }

    public static class NotHasMinGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int min)>> ValidCases => F.GraphemesHasMinCount.AllInvalid.Except(nameof(F.GraphemesHasMinCount.NullValue), nameof(F.GraphemesHasMinCount.NegativeMin)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, int min)>> InvalidCases => F.GraphemesHasMinCount.AllValid.ToMustCases(_ => new MustExpected(false, "value must not have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooMany));
        public static TheoryData<MustCase<(string? value, int min)>> NullCases => F.GraphemesHasMinCount.AllInvalid.Only(nameof(F.GraphemesHasMinCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.TooMany));
        public static TheoryData<MustCase<(string? value, int min)>> NegativeMinCases => F.GraphemesHasMinCount.AllInvalid.Only(nameof(F.GraphemesHasMinCount.NegativeMin)).ToMustCases(_ => new MustExpected(false, "min requires a non-negative minimum count.", "min", MustCodes.Text.Graphemes.TooMany));
    }

    public static class HasMaxGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int max)>> ValidCases => F.GraphemesHasMaxCount.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, int max)>> InvalidCases => F.GraphemesHasMaxCount.AllInvalid.Except(nameof(F.GraphemesHasMaxCount.NullValue), nameof(F.GraphemesHasMaxCount.NegativeMax)).ToMustCases(_ => new MustExpected(false, "value must have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooMany));
        public static TheoryData<MustCase<(string? value, int max)>> NullCases => F.GraphemesHasMaxCount.AllInvalid.Only(nameof(F.GraphemesHasMaxCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.TooMany));
        public static TheoryData<MustCase<(string? value, int max)>> NegativeMaxCases => F.GraphemesHasMaxCount.AllInvalid.Only(nameof(F.GraphemesHasMaxCount.NegativeMax)).ToMustCases(_ => new MustExpected(false, "max requires a non-negative maximum count.", "max", MustCodes.Text.Graphemes.TooMany));
    }

    public static class NotHasMaxGraphemeCount
    {
        public static TheoryData<MustCase<(string? value, int max)>> ValidCases => F.GraphemesHasMaxCount.AllInvalid.Except(nameof(F.GraphemesHasMaxCount.NullValue), nameof(F.GraphemesHasMaxCount.NegativeMax)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, int max)>> InvalidCases => F.GraphemesHasMaxCount.AllValid.ToMustCases(_ => new MustExpected(false, "value must not have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooFew));
        public static TheoryData<MustCase<(string? value, int max)>> NullCases => F.GraphemesHasMaxCount.AllInvalid.Only(nameof(F.GraphemesHasMaxCount.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.TooFew));
        public static TheoryData<MustCase<(string? value, int max)>> NegativeMaxCases => F.GraphemesHasMaxCount.AllInvalid.Only(nameof(F.GraphemesHasMaxCount.NegativeMax)).ToMustCases(_ => new MustExpected(false, "max requires a non-negative maximum count.", "max", MustCodes.Text.Graphemes.TooFew));
    }

    public static class HasGraphemeCountBetween
    {
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> ValidCases => F.GraphemesHasCountBetween.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidCases => F.GraphemesHasCountBetween.AllInvalid.Except(nameof(F.GraphemesHasCountBetween.NullValue), nameof(F.GraphemesHasCountBetween.NegativeMin), nameof(F.GraphemesHasCountBetween.NegativeMax), nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToMustCases(_ => new MustExpected(false, "value must have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.OutOfRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NullCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.OutOfRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NegativeMinCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NegativeMin)).ToMustCases(_ => new MustExpected(false, "min requires a non-negative minimum count.", "min", MustCodes.Text.Graphemes.OutOfRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NegativeMaxCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NegativeMax)).ToMustCases(_ => new MustExpected(false, "max requires a non-negative maximum count.", "max", MustCodes.Text.Graphemes.OutOfRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidRangeCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToMustCases(_ => new MustExpected(false, "min requires a valid count range.", "min", MustCodes.Text.Graphemes.OutOfRange));
    }

    public static class NotHasGraphemeCountBetween
    {
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> ValidCases => F.GraphemesHasCountBetween.AllInvalid.Except(nameof(F.GraphemesHasCountBetween.NullValue), nameof(F.GraphemesHasCountBetween.NegativeMin), nameof(F.GraphemesHasCountBetween.NegativeMax), nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidCases => F.GraphemesHasCountBetween.AllValid.ToMustCases(_ => new MustExpected(false, "value must not have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.InRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NullCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", MustCodes.Text.Graphemes.InRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NegativeMinCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NegativeMin)).ToMustCases(_ => new MustExpected(false, "min requires a non-negative minimum count.", "min", MustCodes.Text.Graphemes.InRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> NegativeMaxCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NegativeMax)).ToMustCases(_ => new MustExpected(false, "max requires a non-negative maximum count.", "max", MustCodes.Text.Graphemes.InRange));
        public static TheoryData<MustCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidRangeCases => F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToMustCases(_ => new MustExpected(false, "min requires a valid count range.", "min", MustCodes.Text.Graphemes.InRange));
    }
}
