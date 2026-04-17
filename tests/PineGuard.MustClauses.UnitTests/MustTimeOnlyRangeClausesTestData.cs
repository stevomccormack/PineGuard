using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTimeOnlyRangeClausesTestData
{
    private static readonly TimeOnly T10 = F.IsKnownTimes.T1000!.Value;
    private static readonly TimeOnly T11 = F.IsKnownTimes.T1100!.Value;
    private static readonly TimeOnly T12 = F.IsKnownTimes.T1200!.Value;
    private static readonly TimeOnly T13 = F.IsKnownTimes.T1300!.Value;
    private static readonly TimeOnly T14 = F.IsKnownTimes.T1400!.Value;

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("strict", (T10, T12, Inclusion.Exclusive), true),
            new("equal fail strict", (T10, T10, Inclusion.Exclusive), false),
            new("inclusive", (T10, T12, Inclusion.Inclusive), true),
            new("equal pass inclusive", (T10, T10, Inclusion.Inclusive), true)
        ];
        public sealed record ValidCase(string Name, (TimeOnly Start, TimeOnly End, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(TimeOnly Start, TimeOnly End, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("overlap", (T10, T12, T11, T13, Inclusion.Exclusive), true),
            new("contained", (T10, T13, T11, T12, Inclusion.Exclusive), true),
            new("no overlap", (T10, T11, T12, T13, Inclusion.Exclusive), false),
            new("touching exclusive", (T10, T11, T11, T12, Inclusion.Exclusive), false),
            new("touching inclusive", (T10, T11, T11, T12, Inclusion.Inclusive), true)
        ];

        public sealed record ValidCase(string Name, (TimeOnly S1, TimeOnly E1, TimeOnly S2, TimeOnly E2, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(TimeOnly S1, TimeOnly E1, TimeOnly S2, TimeOnly E2, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("no overlap", (T10, T11, T12, T13, Inclusion.Exclusive), true),
            new("overlap", (T10, T12, T11, T13, Inclusion.Exclusive), false),
            new("touching exclusive", (T10, T11, T11, T12, Inclusion.Exclusive), true),
            new("touching inclusive", (T10, T11, T11, T12, Inclusion.Inclusive), false)
        ];

        public sealed record ValidCase(string Name, (TimeOnly S1, TimeOnly E1, TimeOnly S2, TimeOnly E2, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(TimeOnly S1, TimeOnly E1, TimeOnly S2, TimeOnly E2, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class Contains
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
            new("middle", (T10, T12, T11, Inclusion.Inclusive), true),
             new("outside", (T10, T12, T13, Inclusion.Inclusive), false),
             new("start inclusive", (T10, T12, T10, Inclusion.Inclusive), true),
             new("end inclusive", (T10, T12, T12, Inclusion.Inclusive), true),
             new("start exclusive", (T10, T12, T10, Inclusion.Exclusive), false),
             new("end exclusive", (T10, T12, T12, Inclusion.Exclusive), false)
       ];

        public sealed record ValidCase(string Name, (TimeOnly Start, TimeOnly End, TimeOnly Target, Inclusion Inclusion) Value, bool Expected)
           : IsCase<(TimeOnly Start, TimeOnly End, TimeOnly Target, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class NotContains
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
            new("outside", (T10, T12, T13, Inclusion.Inclusive), true),
             new("middle", (T10, T12, T11, Inclusion.Inclusive), false),
             new("start inclusive", (T10, T12, T10, Inclusion.Inclusive), false),
             new("start exclusive", (T10, T12, T10, Inclusion.Exclusive), true),
             new("T14 beyond end is outside", (T10, T12, T14, Inclusion.Inclusive), true)
       ];

        public sealed record ValidCase(string Name, (TimeOnly Start, TimeOnly End, TimeOnly Target, Inclusion Inclusion) Value, bool Expected)
           : IsCase<(TimeOnly Start, TimeOnly End, TimeOnly Target, Inclusion Inclusion)>(Name, Value, Expected);
    }
}
