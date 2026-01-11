using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class ObjectRulesTestData
{
    public static class IsOfType
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("string", "abc", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("int", 123, false)
        ];

        #region Case Records

        public sealed record Case(string Name, object? Value, bool ExpectedReturn)
            : IsCase<object?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsAssignableToType
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("string", "abc", true),
            new("empty", string.Empty, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("int", 123, false),
            new("string", "abc", false),
            new("object", new object(), false)
        ];

        #region Case Records

        public sealed record Case(string Name, object? Value, bool ExpectedReturn)
            : IsCase<object?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
