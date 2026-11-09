using PineGuard.Codes;
using PineGuard.Common;
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

    public static class HasGraphemeCountInRange
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasCountInRange.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasCountInRange.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasCountInRange.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountInRange.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountInRange.MinAboveMax) => new DataAnnotationExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.OutOfRange),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.OutOfRange)
        });
    }

    public static class HasGraphemeCountOutOfRange
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GraphemesHasCountInRange.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GraphemesHasCountInRange.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GraphemesHasCountInRange.NegativeMin) => new DataAnnotationExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountInRange.NegativeMax) => new DataAnnotationExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountInRange.MinAboveMax) => new DataAnnotationExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.InRange),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.InRange),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class NullConfiguration
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("UndefinedInclusion", new HasGraphemeCountInRangeAttribute(0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.")),
            new("NegativeUndefinedInclusion", new HasGraphemeCountOutOfRangeAttribute(0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.")),
            new("NegativeCount", new HasExactGraphemeCountAttribute(-1), new(false, "count requires a non-negative count.")),
            new("NegativeNotCount", new NotHasExactGraphemeCountAttribute(-1), new(false, "count requires a non-negative count.")),
            new("NegativeMinimum", new HasMinGraphemeCountAttribute(-1), new(false, "min requires a non-negative minimum count.")),
            new("NegativeNotMinimum", new NotHasMinGraphemeCountAttribute(-1), new(false, "min requires a non-negative minimum count.")),
            new("NegativeMaximum", new HasMaxGraphemeCountAttribute(-1), new(false, "max requires a non-negative maximum count.")),
            new("NegativeNotMaximum", new NotHasMaxGraphemeCountAttribute(-1), new(false, "max requires a non-negative maximum count.")),
            new("OptionalValue", new HasGraphemeCountInRangeAttribute(0, 1), new(true))
        ];
    }
}
