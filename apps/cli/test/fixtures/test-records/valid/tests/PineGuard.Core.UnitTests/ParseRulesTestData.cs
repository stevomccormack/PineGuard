using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

/// <summary>
/// test-records VIBE valid/ case (a): a Core "Value/Result Test" (converter,
/// not a predicate) per unit-test.md §4.2/§8.2's <c>FooRulesTestData.Parse</c>
/// canonical example — the base-record convention correctly applies here,
/// and both case records properly inherit it, so the rule must report zero
/// findings.
/// </summary>
public static class ParseRulesTestData
{
    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("SingleDigit", "5", 5),
            new("Negative", "-3", -3),
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null", null, new ExpectedException(typeof(ArgumentNullException), "input")),
            new InvalidCase("NonNumeric", "abc", new ExpectedException(typeof(FormatException))),
        ];

        public sealed record ValidCase(string Name, string? Value, int Expected)
            : ReturnCase<string?, int>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }
}
