using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class CronRulesFixtures
{
    public static class IsCronExpression
    {
        public static readonly (string? value, CronFormat format) EveryMinute = ("* * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) Hourly = ("0 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) StepMinutes = ("*/15 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) HourRange = ("0 9-17 * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) MinuteList = ("0,15,30,45 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) RangeWithStep = ("0 0-23/2 * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) ValueWithStep = ("5/10 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) FirstMonthName = ("0 0 1 JAN *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) LastMonthName = ("0 0 1 DEC *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) MonthNameRange = ("0 0 1 JAN-MAR *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) FirstDayName = ("0 0 * * SUN", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) LastDayName = ("0 0 * * SAT", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) DayNameRange = ("0 0 * * MON-FRI", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) LowerCaseNames = ("0 0 * * mon-fri", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) MixedList = ("0 0 1,15 JAN,JUL MON", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) NamedValueWithStep = ("0 0 1 JAN/2 *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) LeadingZeroValues = ("00 09 * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) TabSeparated = ("0\t0\t*\t*\t*", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) RepeatedSpaces = ("0  0  *  *  *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) Padded = ("  * * * * *  ", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) SecondsEveryMinute = ("0 * * * * *", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) SecondsWithStep = ("*/30 * * * * *", CronFormat.WithSeconds);

        public static readonly (string? value, CronFormat format) AtMinSecond = ($"{CronRules.MinSecond} * * * * *", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) AtMaxSecond = ($"{CronRules.MaxSecond} * * * * *", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) AtMinMinute = ($"{CronRules.MinMinute} * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMaxMinute = ($"{CronRules.MaxMinute} * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMinHour = ($"* {CronRules.MinHour} * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMaxHour = ($"* {CronRules.MaxHour} * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMinDayOfMonth = ($"* * {CronRules.MinDayOfMonth} * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMaxDayOfMonth = ($"* * {CronRules.MaxDayOfMonth} * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMinMonth = ($"* * * {CronRules.MinMonth} *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMaxMonth = ($"* * * {CronRules.MaxMonth} *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMinDayOfWeek = ($"* * * * {CronRules.MinDayOfWeek}", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMaxDayOfWeek = ($"* * * * {CronRules.MaxDayOfWeek}", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AtMinStep = ($"*/{CronRules.MinStep} * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) FullMinuteRange = ($"{CronRules.MinMinute}-{CronRules.MaxMinute} * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) SingleValueRange = ("5-5 * * * *", CronFormat.Standard);

        public static readonly (string? value, CronFormat format) NullValue = (null, CronFormat.Standard);
        public static readonly (string? value, CronFormat format) EmptyString = ("", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) WhiteSpace = ("   ", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) UnknownFormat = ("* * * * *", (CronFormat)99);
        public static readonly (string? value, CronFormat format) TooFewFields = ("* * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) TooManyFields = ("* * * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) SecondsFieldsMissing = ("* * * * *", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) Macro = ("@daily", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) QuartzNoValue = ("0 0 12 * * ?", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) QuartzLastDay = ("* * L * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) QuartzNearestWeekday = ("* * 15W * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) QuartzNthDayOfWeek = ("* * * * 6#3", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) NonNumericField = ("abc * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) UnknownMonthName = ("* * * FOO *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) UnknownDayName = ("* * * * FOO", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) MonthNameInMinuteField = ("JAN * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) DayNameInMonthField = ("* * * MON *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) EmptyListItem = ("1,,2 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) TrailingListSeparator = ("1,2, * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) SpacedListItem = ("1, 2 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) EmptyRangeStart = ("-5 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) EmptyRangeEnd = ("5- * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) DescendingRange = ("5-1 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) DoubleRange = ("1-2-3 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AnyValueInRange = ("*-5 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) EmptyStep = ("*/ * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) DoubleStep = ("*/2/3 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) NonNumericStep = ("*/x * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) NamedStep = ("* * * JAN/JAN *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) NegativeStep = ("*/-1 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) SignedValue = ("+5 * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) OverflowingValue = ("99999999999 * * * *", CronFormat.Standard);

        public static readonly (string? value, CronFormat format) AboveMaxSecond = ($"{CronRules.MaxSecond + 1} * * * * *", CronFormat.WithSeconds);
        public static readonly (string? value, CronFormat format) AboveMaxMinute = ($"{CronRules.MaxMinute + 1} * * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AboveMaxHour = ($"* {CronRules.MaxHour + 1} * * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) BelowMinDayOfMonth = ($"* * {CronRules.MinDayOfMonth - 1} * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AboveMaxDayOfMonth = ($"* * {CronRules.MaxDayOfMonth + 1} * *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) BelowMinMonth = ($"* * * {CronRules.MinMonth - 1} *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AboveMaxMonth = ($"* * * {CronRules.MaxMonth + 1} *", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) AboveMaxDayOfWeek = ($"* * * * {CronRules.MaxDayOfWeek + 1}", CronFormat.Standard);
        public static readonly (string? value, CronFormat format) BelowMinStep = ($"*/{CronRules.MinStep - 1} * * * *", CronFormat.Standard);

        public static RuleScenario<(string? value, CronFormat format)>[] ValidScenarios => [new(nameof(EveryMinute), EveryMinute, true), new(nameof(Hourly), Hourly, true), new(nameof(StepMinutes), StepMinutes, true), new(nameof(HourRange), HourRange, true), new(nameof(MinuteList), MinuteList, true), new(nameof(RangeWithStep), RangeWithStep, true), new(nameof(ValueWithStep), ValueWithStep, true), new(nameof(FirstMonthName), FirstMonthName, true), new(nameof(LastMonthName), LastMonthName, true), new(nameof(MonthNameRange), MonthNameRange, true), new(nameof(FirstDayName), FirstDayName, true), new(nameof(LastDayName), LastDayName, true), new(nameof(DayNameRange), DayNameRange, true), new(nameof(LowerCaseNames), LowerCaseNames, true), new(nameof(MixedList), MixedList, true), new(nameof(NamedValueWithStep), NamedValueWithStep, true), new(nameof(LeadingZeroValues), LeadingZeroValues, true), new(nameof(TabSeparated), TabSeparated, true), new(nameof(RepeatedSpaces), RepeatedSpaces, true), new(nameof(Padded), Padded, true), new(nameof(SecondsEveryMinute), SecondsEveryMinute, true), new(nameof(SecondsWithStep), SecondsWithStep, true)];
        public static RuleScenario<(string? value, CronFormat format)>[] ValidEdgeScenarios => [new(nameof(AtMinSecond), AtMinSecond, true), new(nameof(AtMaxSecond), AtMaxSecond, true), new(nameof(AtMinMinute), AtMinMinute, true), new(nameof(AtMaxMinute), AtMaxMinute, true), new(nameof(AtMinHour), AtMinHour, true), new(nameof(AtMaxHour), AtMaxHour, true), new(nameof(AtMinDayOfMonth), AtMinDayOfMonth, true), new(nameof(AtMaxDayOfMonth), AtMaxDayOfMonth, true), new(nameof(AtMinMonth), AtMinMonth, true), new(nameof(AtMaxMonth), AtMaxMonth, true), new(nameof(AtMinDayOfWeek), AtMinDayOfWeek, true), new(nameof(AtMaxDayOfWeek), AtMaxDayOfWeek, true), new(nameof(AtMinStep), AtMinStep, true), new(nameof(FullMinuteRange), FullMinuteRange, true), new(nameof(SingleValueRange), SingleValueRange, true)];
        public static RuleScenario<(string? value, CronFormat format)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(EmptyString), EmptyString, false), new(nameof(WhiteSpace), WhiteSpace, false), new(nameof(UnknownFormat), UnknownFormat, false), new(nameof(TooFewFields), TooFewFields, false), new(nameof(TooManyFields), TooManyFields, false), new(nameof(SecondsFieldsMissing), SecondsFieldsMissing, false), new(nameof(Macro), Macro, false), new(nameof(QuartzNoValue), QuartzNoValue, false), new(nameof(QuartzLastDay), QuartzLastDay, false), new(nameof(QuartzNearestWeekday), QuartzNearestWeekday, false), new(nameof(QuartzNthDayOfWeek), QuartzNthDayOfWeek, false), new(nameof(NonNumericField), NonNumericField, false), new(nameof(UnknownMonthName), UnknownMonthName, false), new(nameof(UnknownDayName), UnknownDayName, false), new(nameof(MonthNameInMinuteField), MonthNameInMinuteField, false), new(nameof(DayNameInMonthField), DayNameInMonthField, false), new(nameof(EmptyListItem), EmptyListItem, false), new(nameof(TrailingListSeparator), TrailingListSeparator, false), new(nameof(SpacedListItem), SpacedListItem, false), new(nameof(EmptyRangeStart), EmptyRangeStart, false), new(nameof(EmptyRangeEnd), EmptyRangeEnd, false), new(nameof(DescendingRange), DescendingRange, false), new(nameof(DoubleRange), DoubleRange, false), new(nameof(AnyValueInRange), AnyValueInRange, false), new(nameof(EmptyStep), EmptyStep, false), new(nameof(DoubleStep), DoubleStep, false), new(nameof(NonNumericStep), NonNumericStep, false), new(nameof(NamedStep), NamedStep, false), new(nameof(NegativeStep), NegativeStep, false), new(nameof(SignedValue), SignedValue, false), new(nameof(OverflowingValue), OverflowingValue, false)];
        public static RuleScenario<(string? value, CronFormat format)>[] InvalidEdgeScenarios => [new(nameof(AboveMaxSecond), AboveMaxSecond, false), new(nameof(AboveMaxMinute), AboveMaxMinute, false), new(nameof(AboveMaxHour), AboveMaxHour, false), new(nameof(BelowMinDayOfMonth), BelowMinDayOfMonth, false), new(nameof(AboveMaxDayOfMonth), AboveMaxDayOfMonth, false), new(nameof(BelowMinMonth), BelowMinMonth, false), new(nameof(AboveMaxMonth), AboveMaxMonth, false), new(nameof(AboveMaxDayOfWeek), AboveMaxDayOfWeek, false), new(nameof(BelowMinStep), BelowMinStep, false)];
        public static RuleScenario<(string? value, CronFormat format)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, CronFormat format)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, CronFormat format)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class TryParse
    {
        public static readonly (string? value, CronFormat format, string[]? fields) EveryMinute = ("* * * * *", CronFormat.Standard, ["*", "*", "*", "*", "*"]);
        public static readonly (string? value, CronFormat format, string[]? fields) Padded = ("  0  9-17  *  *  MON-FRI  ", CronFormat.Standard, ["0", "9-17", "*", "*", "MON-FRI"]);
        public static readonly (string? value, CronFormat format, string[]? fields) TabSeparated = ("0\t0\t*\t*\t*", CronFormat.Standard, ["0", "0", "*", "*", "*"]);
        public static readonly (string? value, CronFormat format, string[]? fields) Seconds = ("*/30 0 12 1 JAN MON", CronFormat.WithSeconds, ["*/30", "0", "12", "1", "JAN", "MON"]);
        public static readonly (string? value, CronFormat format, string[]? fields) UnnormalizedNames = ("0 0 * * mon-fri", CronFormat.Standard, ["0", "0", "*", "*", "mon-fri"]);
        public static readonly (string? value, CronFormat format, string[]? fields) NullValue = (null, CronFormat.Standard, null);
        public static readonly (string? value, CronFormat format, string[]? fields) UnknownFormat = ("* * * * *", (CronFormat)99, null);
        public static readonly (string? value, CronFormat format, string[]? fields) WrongFieldCount = ("* * * *", CronFormat.Standard, null);
        public static readonly (string? value, CronFormat format, string[]? fields) OutOfRangeValue = ("60 * * * *", CronFormat.Standard, null);

        public static RuleScenario<(string? value, CronFormat format, string[]? fields)>[] ValidScenarios => [new(nameof(EveryMinute), EveryMinute, true), new(nameof(Padded), Padded, true), new(nameof(TabSeparated), TabSeparated, true), new(nameof(Seconds), Seconds, true), new(nameof(UnnormalizedNames), UnnormalizedNames, true)];
        public static RuleScenario<(string? value, CronFormat format, string[]? fields)>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(UnknownFormat), UnknownFormat, false), new(nameof(WrongFieldCount), WrongFieldCount, false), new(nameof(OutOfRangeValue), OutOfRangeValue, false)];
        public static RuleScenario<(string? value, CronFormat format, string[]? fields)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
