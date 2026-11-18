using System.Globalization;
using F = PineGuard.Testing.Fixtures.StringPastDateOnlyRuleFixtures;

namespace PineGuard.Testing.UnitTests.Rules.Conformance;

public static class StringPastDateOnlyRuleTestData
{
    private const string NotPastCode = "date.relative.not-past";
    private const string FormatInvalidCode = "date.format.invalid";
    private const string ReferenceOutOfRangeCode = "date.reference.out-of-range";

    private const string StylesParamName = "styles";
    private const string TimeProviderParamName = "timeProvider";

    public const string ParamNameToken = "{paramName}";

    private const string NotPastTemplate = "{paramName} must be a date in the past.";
    private const string InvalidStylesTemplate = "{paramName} must be a valid combination of parsing styles.";
    private const string NullTemplate = "{paramName} must not be null.";
    private const string ReferenceOutOfRangeTemplate = "{paramName} requires a reference date within the supported range.";

    public static class Evaluate
    {
        public static readonly string ParamName = "birthDate";
        public static readonly string MemberName = "Value";

        public static TheoryData<ConformanceCase<(string? value, DateTimeStyles styles, TimeProvider? timeProvider)>> Cases =>
            F.Evaluate.AllScenarios
                .Except(nameof(F.Evaluate.PastDateAllowNull), nameof(F.Evaluate.TodayAllowNull), nameof(F.Evaluate.InvalidStylesAllowNull), nameof(F.Evaluate.NullValueInvalidStylesAllowNull), nameof(F.Evaluate.NullValueAllowNull))
                .Except(nameof(F.Evaluate.NotADateAllowNull), nameof(F.Evaluate.EmptyAllowNull), nameof(F.Evaluate.MaxValueProjectionAllowNull), nameof(F.Evaluate.NullValueAllowNullBeforeProjection))
                .Project(inputs => (inputs.value, inputs.styles, timeProvider: (TimeProvider?)inputs.timeProvider))
                .Concat(F.EvaluateSystemClock.AllScenarios.Project(inputs => (inputs.value, inputs.styles, inputs.timeProvider)))
                .ToArray()
                .ToConformanceCases(s => s.Name switch
                {
                    nameof(F.Evaluate.PastDate) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PastDate.value, NotPastCode),
                    nameof(F.Evaluate.Yesterday) => new ConformanceExpected(true, true, new DateOnly(2026, 6, 14), ParamName, F.Evaluate.Yesterday.value, NotPastCode),
                    nameof(F.Evaluate.AheadOfUtcPastDate) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.AheadOfUtcPastDate.value, NotPastCode),
                    nameof(F.Evaluate.AheadOfUtcYesterday) => new ConformanceExpected(true, true, new DateOnly(2026, 6, 15), ParamName, F.Evaluate.AheadOfUtcYesterday.value, NotPastCode),
                    nameof(F.Evaluate.BehindUtcPastDate) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.BehindUtcPastDate.value, NotPastCode),
                    nameof(F.Evaluate.BehindUtcYesterday) => new ConformanceExpected(true, true, new DateOnly(2026, 6, 13), ParamName, F.Evaluate.BehindUtcYesterday.value, NotPastCode),
                    nameof(F.Evaluate.PaddedPastDate) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PaddedPastDate.value, NotPastCode),
                    nameof(F.Evaluate.PaddedPastDateNoneStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PaddedPastDateNoneStyles.value, NotPastCode),
                    nameof(F.Evaluate.PastDateNoneStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PastDateNoneStyles.value, NotPastCode),
                    nameof(F.Evaluate.InnerSpacedPastDateNoneStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.InnerSpacedPastDateNoneStyles.value, NotPastCode),
                    nameof(F.Evaluate.PaddedPastDateLeadingWhiteStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PaddedPastDateLeadingWhiteStyles.value, NotPastCode),
                    nameof(F.Evaluate.PaddedPastDateTrailingWhiteStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PaddedPastDateTrailingWhiteStyles.value, NotPastCode),
                    nameof(F.Evaluate.PaddedPastDateInnerWhiteStyles) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.Evaluate.PaddedPastDateInnerWhiteStyles.value, NotPastCode),
                    nameof(F.Evaluate.Today) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.Today.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.Tomorrow) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.Tomorrow.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.AheadOfUtcToday) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.AheadOfUtcToday.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.AheadOfUtcTomorrow) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.AheadOfUtcTomorrow.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.BehindUtcToday) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.BehindUtcToday.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.BehindUtcTomorrow) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.BehindUtcTomorrow.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.InvalidStyles) => new ConformanceExpected(false, false, default, StylesParamName, F.Evaluate.InvalidStyles.styles, NotPastCode, InvalidStylesTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.InvalidStylesCombination) => new ConformanceExpected(false, false, default, StylesParamName, F.Evaluate.InvalidStylesCombination.styles, NotPastCode, InvalidStylesTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.NullValueInvalidStyles) => new ConformanceExpected(false, false, default, StylesParamName, F.Evaluate.NullValueInvalidStyles.styles, NotPastCode, InvalidStylesTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.NullValue) => new ConformanceExpected(false, true, default, ParamName, F.Evaluate.NullValue.value, NotPastCode, NullTemplate, NotPastCode, typeof(ArgumentNullException)),
                    nameof(F.Evaluate.NotADate) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.NotADate.value, NotPastCode, NotPastTemplate, FormatInvalidCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.Empty) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.Empty.value, NotPastCode, NotPastTemplate, FormatInvalidCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.WhitespaceAllowNull) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.WhitespaceAllowNull.value, NotPastCode, NotPastTemplate, FormatInvalidCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.SpaceInsideComponent) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.SpaceInsideComponent.value, NotPastCode, NotPastTemplate, FormatInvalidCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.MinValueProjection) => new ConformanceExpected(false, false, default, TimeProviderParamName, F.Evaluate.MinValueProjection.timeProvider, NotPastCode, ReferenceOutOfRangeTemplate, ReferenceOutOfRangeCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.MaxValueProjection) => new ConformanceExpected(false, false, default, TimeProviderParamName, F.Evaluate.MaxValueProjection.timeProvider, NotPastCode, ReferenceOutOfRangeTemplate, ReferenceOutOfRangeCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.InvalidStylesBeforeProjection) => new ConformanceExpected(false, false, default, StylesParamName, F.Evaluate.InvalidStylesBeforeProjection.styles, NotPastCode, InvalidStylesTemplate, NotPastCode, typeof(ArgumentException)),
                    nameof(F.Evaluate.NullValueBeforeProjection) => new ConformanceExpected(false, true, default, ParamName, F.Evaluate.NullValueBeforeProjection.value, NotPastCode, NullTemplate, NotPastCode, typeof(ArgumentNullException)),
                    nameof(F.Evaluate.NotADateBeforeProjection) => new ConformanceExpected(false, false, default, ParamName, F.Evaluate.NotADateBeforeProjection.value, NotPastCode, NotPastTemplate, FormatInvalidCode, typeof(ArgumentException)),
                    nameof(F.EvaluateSystemClock.SystemClockFarPast) => new ConformanceExpected(true, true, new DateOnly(2000, 1, 1), ParamName, F.EvaluateSystemClock.SystemClockFarPast.value, NotPastCode),
                    nameof(F.EvaluateSystemClock.SystemClockFarFuture) => new ConformanceExpected(false, false, default, ParamName, F.EvaluateSystemClock.SystemClockFarFuture.value, NotPastCode, NotPastTemplate, NotPastCode, typeof(ArgumentException)),
                    _ => throw new ArgumentOutOfRangeException(nameof(s), s.Name, null)
                });
    }
}
