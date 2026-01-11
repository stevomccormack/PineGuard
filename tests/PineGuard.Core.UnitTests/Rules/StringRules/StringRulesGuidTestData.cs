using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesGuidTestData
{
    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Guid.Empty => true", Guid.Empty.ToString("D"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Guid.NewGuid => false", Guid.NewGuid().ToString("D"), false),
            new("not-a-guid => false", "not-a-guid", false),
            new("null => false", null, false),
            new("space => false", " ", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null => true", null, true),
            new("Guid.Empty => true", Guid.Empty.ToString("D"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Guid.NewGuid => false", Guid.NewGuid().ToString("D"), false),
            new("not-a-guid => false", "not-a-guid", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
