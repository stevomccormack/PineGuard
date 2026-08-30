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

    public static class HasGraphemeCountBetween
    {
        public static TheoryData<FluentCase<(string? value, int min, int max, Inclusion inclusion)>> Cases => F.GraphemesHasCountBetween.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasCountBetween.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasCountBetween.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountBetween.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.OutOfRange),
            nameof(F.GraphemesHasCountBetween.MinAboveMax) => new FluentExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.OutOfRange),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.OutOfRange)
        });
    }

    public static class NotHasGraphemeCountBetween
    {
        public static TheoryData<FluentCase<(string? value, int min, int max, Inclusion inclusion)>> Cases => F.GraphemesHasCountBetween.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GraphemesHasCountBetween.NullValue) => new FluentExpected(true),
            nameof(F.GraphemesHasCountBetween.NegativeMin) => new FluentExpected(false, "min requires a non-negative minimum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountBetween.NegativeMax) => new FluentExpected(false, "max requires a non-negative maximum count.", Code: MustCodes.Text.Graphemes.InRange),
            nameof(F.GraphemesHasCountBetween.MinAboveMax) => new FluentExpected(false, "min requires a valid count range.", Code: MustCodes.Text.Graphemes.InRange),
            _ when s.IsValid => new FluentExpected(false, "Value must not have a number of characters within the expected range.", Code: MustCodes.Text.Graphemes.InRange),
            _ => new FluentExpected(true)
        });
    }
}
