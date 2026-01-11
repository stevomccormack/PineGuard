using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class PhoneRulesTestData
{
    public static class IsPhoneNumber
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("+1(425)555-0123 => true", "+1(425)555-0123", true),
            new("4255550123 => true", "4255550123", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("123 => false", "123", false),
            new("extension suffix => false", "425-555-0123x", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
