using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class BoolRulesTestData
{
    public static class IsTrue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("true", true, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("false", false, false),
        ];

        #region Case Records

        public sealed record Case(string Name, bool? Value, bool ExpectedReturn)
            : IsCase<bool?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFalse
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("false", false, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("true", true, false),
        ];

        #region Case Records

        public sealed record Case(string Name, bool? Value, bool ExpectedReturn)
            : IsCase<bool?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrTrue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("true", true, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("false", false, false),
        ];

        #region Case Records

        public sealed record Case(string Name, bool? Value, bool ExpectedReturn)
            : IsCase<bool?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrFalse
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null", null, true),
            new("false", false, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("true", true, false),
        ];

        #region Case Records

        public sealed record Case(string Name, bool? Value, bool ExpectedReturn)
            : IsCase<bool?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
