using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class DefaultEqualityRulesTestData
{
    public static class IsDefaultInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("default", 0, true),
            new("positive", 1, false),
            new("negative", -1, false),
        ];

        #region Case Records

        public sealed record Case(string Name, int Value, bool ExpectedReturn)
            : IsCase<int>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsDefaultNullableInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("zero", 0, false),
            new("one", 1, false),
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsDefaultString
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("empty", string.Empty, false),
            new("whitespace", " ", false),
            new("text", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrDefaultInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("default", 0, true),
            new("non-default", 1, false),
        ];

        #region Case Records

        public sealed record Case(string Name, int Value, bool ExpectedReturn)
            : IsCase<int>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrDefaultNullableInt32
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("zero", 0, false),
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrDefaultString
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("empty", string.Empty, false),
            new("text", "abc", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
