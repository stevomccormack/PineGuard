using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringGraphemesClausesTestData
{
    // Guard.Against.NotHasExactGraphemeCount — throws when the character count does NOT match (calls Must.Be.HasExactGraphemeCount)
    public static class NotHasExactGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int count)>> ValidCases => F.GraphemesHasExactCount.AllValid.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int count)>> InvalidCases => F.GraphemesHasExactCount.AllInvalid.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.Mismatch) : s.Inputs.count < 0 ? new GuardExpected(false, typeof(ArgumentException), "count", Code: MustCodes.Text.Graphemes.Mismatch) : new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.Mismatch));
    }

    // Guard.Against.HasExactGraphemeCount — throws when the character count DOES match (calls Must.Be.NotHasExactGraphemeCount)
    public static class HasExactGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int count)>> ValidCases => F.GraphemesHasExactCount.AllInvalid.Except(nameof(F.GraphemesHasExactCount.NullValue), nameof(F.GraphemesHasExactCount.NegativeCount)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int count)>> InvalidCases => [.. F.GraphemesHasExactCount.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.Match)), .. F.GraphemesHasExactCount.AllInvalid.Only(nameof(F.GraphemesHasExactCount.NullValue), nameof(F.GraphemesHasExactCount.NegativeCount)).ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.Match) : new GuardExpected(false, typeof(ArgumentException), "count", Code: MustCodes.Text.Graphemes.Match))];
    }

    // Guard.Against.NotHasMinGraphemeCount — throws when the character count is below the minimum (calls Must.Be.HasMinGraphemeCount)
    public static class NotHasMinGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int min)>> ValidCases => F.GraphemesHasMinCount.AllValid.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int min)>> InvalidCases => F.GraphemesHasMinCount.AllInvalid.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.TooFew) : s.Inputs.min < 0 ? new GuardExpected(false, typeof(ArgumentException), "min", Code: MustCodes.Text.Graphemes.TooFew) : new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.TooFew));
    }

    // Guard.Against.HasMinGraphemeCount — throws when the character count reaches the minimum (calls Must.Be.NotHasMinGraphemeCount)
    public static class HasMinGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int min)>> ValidCases => F.GraphemesHasMinCount.AllInvalid.Except(nameof(F.GraphemesHasMinCount.NullValue), nameof(F.GraphemesHasMinCount.NegativeMin)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int min)>> InvalidCases => [.. F.GraphemesHasMinCount.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.TooMany)), .. F.GraphemesHasMinCount.AllInvalid.Only(nameof(F.GraphemesHasMinCount.NullValue), nameof(F.GraphemesHasMinCount.NegativeMin)).ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.TooMany) : new GuardExpected(false, typeof(ArgumentException), "min", Code: MustCodes.Text.Graphemes.TooMany))];
    }

    // Guard.Against.NotHasMaxGraphemeCount — throws when the character count is above the maximum (calls Must.Be.HasMaxGraphemeCount)
    public static class NotHasMaxGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int max)>> ValidCases => F.GraphemesHasMaxCount.AllValid.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int max)>> InvalidCases => F.GraphemesHasMaxCount.AllInvalid.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.TooMany) : s.Inputs.max < 0 ? new GuardExpected(false, typeof(ArgumentException), "max", Code: MustCodes.Text.Graphemes.TooMany) : new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.TooMany));
    }

    // Guard.Against.HasMaxGraphemeCount — throws when the character count respects the maximum (calls Must.Be.NotHasMaxGraphemeCount)
    public static class HasMaxGraphemeCount
    {
        public static TheoryData<GuardCase<(string? value, int max)>> ValidCases => F.GraphemesHasMaxCount.AllInvalid.Except(nameof(F.GraphemesHasMaxCount.NullValue), nameof(F.GraphemesHasMaxCount.NegativeMax)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int max)>> InvalidCases => [.. F.GraphemesHasMaxCount.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.TooFew)), .. F.GraphemesHasMaxCount.AllInvalid.Only(nameof(F.GraphemesHasMaxCount.NullValue), nameof(F.GraphemesHasMaxCount.NegativeMax)).ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Text.Graphemes.TooFew) : new GuardExpected(false, typeof(ArgumentException), "max", Code: MustCodes.Text.Graphemes.TooFew))];
    }

    // Guard.Against.NotHasGraphemeCountBetween — throws when the character count is outside the range (calls Must.Be.HasGraphemeCountBetween)
    public static class NotHasGraphemeCountBetween
    {
        public static TheoryData<GuardCase<(string? value, int min, int max, Inclusion inclusion)>> ValidCases => F.GraphemesHasCountBetween.AllValid.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidCases => F.GraphemesHasCountBetween.AllInvalid.ToGuardCases(s => Expected(s.Inputs, MustCodes.Text.Graphemes.OutOfRange));
    }

    // Guard.Against.HasGraphemeCountBetween — throws when the character count is inside the range (calls Must.Be.NotHasGraphemeCountBetween)
    public static class HasGraphemeCountBetween
    {
        public static TheoryData<GuardCase<(string? value, int min, int max, Inclusion inclusion)>> ValidCases => F.GraphemesHasCountBetween.AllInvalid.Except(nameof(F.GraphemesHasCountBetween.NullValue), nameof(F.GraphemesHasCountBetween.NegativeMin), nameof(F.GraphemesHasCountBetween.NegativeMax), nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(string? value, int min, int max, Inclusion inclusion)>> InvalidCases => [.. F.GraphemesHasCountBetween.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Text.Graphemes.InRange)), .. F.GraphemesHasCountBetween.AllInvalid.Only(nameof(F.GraphemesHasCountBetween.NullValue), nameof(F.GraphemesHasCountBetween.NegativeMin), nameof(F.GraphemesHasCountBetween.NegativeMax), nameof(F.GraphemesHasCountBetween.MinAboveMax)).ToGuardCases(s => Expected(s.Inputs, MustCodes.Text.Graphemes.InRange))];
    }

    private static GuardExpected Expected((string? value, int min, int max, Inclusion inclusion) inputs, string code) =>
        inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value", Code: code)
        : inputs.min < 0 ? new GuardExpected(false, typeof(ArgumentException), "min", Code: code)
        : inputs.max < 0 ? new GuardExpected(false, typeof(ArgumentException), "max", Code: code)
        : inputs.min > inputs.max ? new GuardExpected(false, typeof(ArgumentException), "min", Code: code)
        : new GuardExpected(false, typeof(ArgumentException), "value", Code: code);
}
