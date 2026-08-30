using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringGraphemesAttributesTestData
{
    public static class HasExactGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasExactCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasExactCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasExactCount.NegativeCount) => new DataAnnotationExpected(false, "count requires a non-negative count.", Code: MustCodes.Text.Graphemes.Mismatch),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have the expected number of characters.", Code: MustCodes.Text.Graphemes.Mismatch)
        });
    }

    public static class NotHasExactGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasExactCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasExactCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasExactCount.NegativeCount) => new DataAnnotationExpected(false, "count requires a non-negative count.", Code: MustCodes.Text.Graphemes.Match),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have the expected number of characters.", Code: MustCodes.Text.Graphemes.Match),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class HasMinGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasMinCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasMinCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasMinCount.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.TooFew),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooFew)
        });
    }

    public static class NotHasMinGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasMinCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasMinCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasMinCount.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.TooMany),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooMany),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class HasMaxGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasMaxCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasMaxCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasMaxCount.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.TooMany),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooMany)
        });
    }

    public static class NotHasMaxGraphemeCount
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasMaxCount.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasMaxCount.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasMaxCount.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.TooFew),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooFew),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class HasGraphemeCountBetween
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasCountBetween.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasCountBetween.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasCountBetween.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountBetween.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountBetween.MinAboveMax) => new DataAnnotationExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.OutOfRange),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.OutOfRange)
        });
    }

    public static class NotHasGraphemeCountBetween
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasCountBetween.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasCountBetween.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasCountBetween.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountBetween.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountBetween.MinAboveMax) => new DataAnnotationExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.InRange),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.InRange),
            _ => new DataAnnotationExpected(true)
        });
    }
}
