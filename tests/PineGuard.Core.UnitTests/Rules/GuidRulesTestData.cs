using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class GuidRulesTestData
{
    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty", Guid.Empty, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Non-empty", Guid.NewGuid(), false),
        ];

        #region Case Records

        public sealed record Case(string Name, Guid? Value, bool ExpectedReturn)
            : IsCase<Guid?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsNullOrEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Null", null, true),
            new("Empty", Guid.Empty, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Non-empty", Guid.NewGuid(), false),
        ];

        #region Case Records

        public sealed record Case(string Name, Guid? Value, bool ExpectedReturn)
            : IsCase<Guid?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGuid
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("D format", Guid.NewGuid().ToString("D"), true),
            new("N format", Guid.NewGuid().ToString("N"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Not a GUID", "not-a-guid", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsGuidEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty GUID", Guid.Empty.ToString("D"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Not a GUID", "not-a-guid", false),
            new("Non-empty GUID", Guid.NewGuid().ToString("D"), false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
