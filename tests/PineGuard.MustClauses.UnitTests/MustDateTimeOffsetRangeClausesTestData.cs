using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDateTimeOffsetRangeClausesTestData
{
    private static readonly DateTimeOffset Now = DateTimeOffset.Now;
    private static readonly DateTimeOffset Later = Now.AddDays(1);

    private static readonly DateTimeOffsetRange ValidRange = new(Now, Later);

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("chronological", (ValidRange, Inclusion.Exclusive), true)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffsetRange range, Inclusion inclusion) Value, bool Expected)
            : IsCase<(DateTimeOffsetRange range, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        private static readonly DateTimeOffsetRange R1 = new(Now, Later);
        private static readonly DateTimeOffsetRange R2 = new(Now.AddHours(1), Later.AddHours(1)); // Overlaps
        private static readonly DateTimeOffsetRange R3 = new(Later.AddHours(2), Later.AddHours(3)); // No overlap

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("overlaps", (R1, R2, Inclusion.Exclusive), true),
            new("not overlaps", (R1, R3, Inclusion.Exclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion) Value, bool Expected)
            : IsCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        private static readonly DateTimeOffsetRange R1 = new(Now, Later);
        private static readonly DateTimeOffsetRange R2 = new(Now.AddHours(1), Later.AddHours(1));
        private static readonly DateTimeOffsetRange R3 = new(Later.AddHours(2), Later.AddHours(3));

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not overlaps", (R1, R3, Inclusion.Exclusive), true),
            new("overlaps", (R1, R2, Inclusion.Exclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion) Value, bool Expected)
            : IsCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class Contains
    {
        private static readonly DateTimeOffsetRange R = new(Now, Later);
        private static readonly DateTimeOffset Inside = Now.AddHours(1);
        private static readonly DateTimeOffset Outside = Later.AddHours(1);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("contains", (R, Inside, Inclusion.Inclusive), true),
            new("not contains", (R, Outside, Inclusion.Inclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffsetRange range, DateTimeOffset target, Inclusion inclusion) Value, bool Expected)
            : IsCase<(DateTimeOffsetRange range, DateTimeOffset target, Inclusion inclusion)>(Name, Value, Expected);
    }

    public static class NotContains
    {
        private static readonly DateTimeOffsetRange R = new(Now, Later);
        private static readonly DateTimeOffset Inside = Now.AddHours(1);
        private static readonly DateTimeOffset Outside = Later.AddHours(1);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not contains", (R, Outside, Inclusion.Inclusive), true),
             new("contains", (R, Inside, Inclusion.Inclusive), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffsetRange range, DateTimeOffset target, Inclusion inclusion) Value, bool Expected)
            : IsCase<(DateTimeOffsetRange range, DateTimeOffset target, Inclusion inclusion)>(Name, Value, Expected);
    }
}
