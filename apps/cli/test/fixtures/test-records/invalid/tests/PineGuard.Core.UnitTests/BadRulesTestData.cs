using PineGuard.Testing.Common;

namespace PineGuard.Core.UnitTests.Rules;

/// <summary>
/// test-records VIBE invalid/ case: Core is a layer where the base-record
/// convention correctly applies to a custom converter-style case record
/// (unit-test.md §4.2's "Value/Result Tests"), but this <c>ValidCase</c> has
/// no base clause at all — it must be caught (missing-base finding).
/// </summary>
public static class BadRulesTestData
{
    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("SingleDigit", "5", 5),
        ];

        public sealed record ValidCase(string Name, string? Value, int Expected);
    }
}
