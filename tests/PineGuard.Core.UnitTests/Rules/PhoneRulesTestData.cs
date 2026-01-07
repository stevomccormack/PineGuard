using Xunit;

namespace PineGuard.Core.UnitTests.Rules;

public static class PhoneRulesTestData
{
    public static class IsPhoneNumber
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "+1(425)555-0123 => true", Value: "+1(425)555-0123", Expected: true) },
            { new Case(Name: "4255550123 => true", Value: "4255550123", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "123 => false", Value: "123", Expected: false) },
            { new Case(Name: "extension suffix => false", Value: "425-555-0123x", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
