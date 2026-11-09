using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringGraphemesExtensionsTestData
{
    public static class HasExactGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int count)>> Cases => F.GraphemesHasExactCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasExactCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasExactCount.NegativeCount) => new FluentExpected(false, "count requires a non-negative count.", Code: MustCodes.Text.Graphemes.Mismatch),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have the expected number of characters.", Code: MustCodes.Text.Graphemes.Mismatch)
        });
    }

    public static class NotHasExactGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int count)>> Cases => F.GraphemesHasExactCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasExactCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasExactCount.NegativeCount) => new FluentExpected(false, "count requires a non-negative count.", Code: MustCodes.Text.Graphemes.Match),
            _ when s.IsValid => new FluentExpected(false, "Value must not have the expected number of characters.", Code: MustCodes.Text.Graphemes.Match),
            _ => new FluentExpected(true)
        });
    }

    public static class HasMinGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int min)>> Cases => F.GraphemesHasMinCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasMinCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasMinCount.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.TooFew),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooFew)
        });
    }

    public static class NotHasMinGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int min)>> Cases => F.GraphemesHasMinCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasMinCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasMinCount.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.TooMany),
            _ when s.IsValid => new FluentExpected(false, "Value must not have at least the minimum number of characters.", Code: MustCodes.Text.Graphemes.TooMany),
            _ => new FluentExpected(true)
        });
    }

    public static class HasMaxGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int max)>> Cases => F.GraphemesHasMaxCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasMaxCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasMaxCount.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.TooMany),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooMany)
        });
    }

    public static class NotHasMaxGraphemeCount
    {
        public static TheoryData<FluentCase<(string? value, int max)>> Cases => F.GraphemesHasMaxCount.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasMaxCount.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasMaxCount.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.TooFew),
            _ when s.IsValid => new FluentExpected(false, "Value must not have at most the maximum number of characters.", Code: MustCodes.Text.Graphemes.TooFew),
            _ => new FluentExpected(true)
        });
    }

    public static class HasGraphemeCountInRange
    {
        public static TheoryData<FluentCase<(string? value, int min, int max, Inclusion inclusion)>> Cases
        {
            get
            {
                var data = F.GraphemesHasCountInRange.AllScenarios.ToFluentCases(s => s.Name switch
                {
                    nameof(F.GraphemesHasCountInRange.NullValue) => new FluentExpected(true),
                    nameof(F.GraphemesHasCountInRange.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
                    nameof(F.GraphemesHasCountInRange.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
                    nameof(F.GraphemesHasCountInRange.MinAboveMax) => new FluentExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.OutOfRange),
                    _ when s.IsValid => new FluentExpected(true),
                    _ => new FluentExpected(false, "Value must have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.OutOfRange)
                });
                data.Add(new("NullInvalidInclusion", (null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.")));
                data.Add(new("NullReversedBounds", (null, 2, 1, Inclusion.Inclusive), new(false, "min requires a valid count range.")));
                return data;
            }
        }
    }

    public static class HasGraphemeCountOutOfRange
    {
        public static TheoryData<FluentCase<(string? value, int min, int max, Inclusion inclusion)>> Cases
        {
            get
            {
                var data = F.GraphemesHasCountInRange.AllScenarios.ToFluentCases(s => s.Name switch
                {
                    nameof(F.GraphemesHasCountInRange.NullValue) => new FluentExpected(true),
                    nameof(F.GraphemesHasCountInRange.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.InRange),
                    nameof(F.GraphemesHasCountInRange.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.InRange),
                    nameof(F.GraphemesHasCountInRange.MinAboveMax) => new FluentExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.InRange),
                    _ when s.IsValid => new FluentExpected(false, "Value must not have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.InRange),
                    _ => new FluentExpected(true)
                });
                data.Add(new("NullInvalidInclusion", (null, 0, 1, (Inclusion)999), new(false, "inclusion must be a defined inclusion value.")));
                data.Add(new("NullReversedBounds", (null, 2, 1, Inclusion.Inclusive), new(false, "min requires a valid count range.")));
                return data;
            }
        }
    }
}
