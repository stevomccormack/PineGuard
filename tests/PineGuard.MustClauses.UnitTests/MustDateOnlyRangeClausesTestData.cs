using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDateOnlyRangeClausesTestData
{
    private static readonly DateOnly D1 = new(2023, 1, 1);
    private static readonly DateOnly D2 = new(2023, 1, 2);
    private static readonly DateOnly D3 = new(2023, 1, 3);
    private static readonly DateOnly D4 = new(2023, 1, 4);
    private static readonly DateOnly D5 = new(2023, 1, 5);

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("strict", (D1, D3, Inclusion.Exclusive), true),
            new("equal fail strict", (D1, D1, Inclusion.Exclusive), false),
            new("inclusive", (D1, D3, Inclusion.Inclusive), true),
            new("equal pass inclusive", (D1, D1, Inclusion.Inclusive), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly Start, DateOnly End, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(DateOnly Start, DateOnly End, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("overlap", (D1, D3, D2, D4, Inclusion.Exclusive), true),
            new("contained", (D1, D4, D2, D3, Inclusion.Exclusive), true),
            new("no overlap", (D1, D2, D3, D4, Inclusion.Exclusive), false),
            new("touching exclusive", (D1, D2, D2, D3, Inclusion.Exclusive), false),
            new("touching inclusive", (D1, D2, D2, D3, Inclusion.Inclusive), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly S1, DateOnly E1, DateOnly S2, DateOnly E2, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(DateOnly S1, DateOnly E1, DateOnly S2, DateOnly E2, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("no overlap", (D1, D2, D3, D4, Inclusion.Exclusive), true),
            new("overlap", (D1, D3, D2, D4, Inclusion.Exclusive), false),
            new("touching exclusive", (D1, D2, D2, D3, Inclusion.Exclusive), true),
            new("touching inclusive", (D1, D2, D2, D3, Inclusion.Inclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly S1, DateOnly E1, DateOnly S2, DateOnly E2, Inclusion Inclusion) Value, bool Expected)
            : IsCase<(DateOnly S1, DateOnly E1, DateOnly S2, DateOnly E2, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class Contains
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("middle", (D1, D3, D2, Inclusion.Inclusive), true),
             new("outside", (D1, D3, D4, Inclusion.Inclusive), false),
             new("start inclusive", (D1, D3, D1, Inclusion.Inclusive), true),
             new("end inclusive", (D1, D3, D3, Inclusion.Inclusive), true),
             new("start exclusive", (D1, D3, D1, Inclusion.Exclusive), false),
             new("end exclusive", (D1, D3, D3, Inclusion.Exclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly Start, DateOnly End, DateOnly Target, Inclusion Inclusion) Value, bool Expected)
           : IsCase<(DateOnly Start, DateOnly End, DateOnly Target, Inclusion Inclusion)>(Name, Value, Expected);
    }

    public static class NotContains
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("outside", (D1, D3, D4, Inclusion.Inclusive), true),
             new("middle", (D1, D3, D2, Inclusion.Inclusive), false),
             new("start inclusive", (D1, D3, D1, Inclusion.Inclusive), false),
             new("start exclusive", (D1, D3, D1, Inclusion.Exclusive), true),
             new("D5 beyond end is outside", (D1, D3, D5, Inclusion.Inclusive), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly Start, DateOnly End, DateOnly Target, Inclusion Inclusion) Value, bool Expected)
           : IsCase<(DateOnly Start, DateOnly End, DateOnly Target, Inclusion Inclusion)>(Name, Value, Expected);
    }
}
