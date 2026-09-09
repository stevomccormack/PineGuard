using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class VariantRulesTestData
{
    // Underscore-variant pairing: this group is consumed by
    // `IsEven_NonNullable_BehavesAsExpected`, which is the *exact* shape the
    // FluentValidation addendum documents under "Nullable vs Non-Nullable
    // Variants" (Op Group `EvenNonNullable` -> method
    // `Even_NonNullable_BehavesAsExpected`) and which the repo follows in 35
    // places (IsDefaultInt32 -> IsDefault_Int32_BehavesAsExpected, ...).
    // Neither a "missing method" nor an "orphan method" finding is correct.
    public static class IsEvenNonNullable
    {
        public static TheoryData<RuleCase<int>> Cases =>
        [
            new("even", 2, new RuleExpected(true)),
        ];
    }

    // Throws-only group: its single test method is
    // `Parse_ThrowsAsExpected`, not `Parse_BehavesAsExpected`. Sanctioned by
    // v11 §8.3 ("`Parse` ... splits into a `_BehavesAsExpected` /
    // `_ThrowsAsExpected` pair") and by the DataAnnotations addendum's
    // "Pattern E — TypeMismatch Throws", whose group's only method is
    // `<Attr>_TypeMismatch_ThrowsExpected`.
    public static class Parse
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null input", null, new ExpectedException(typeof(ArgumentNullException), "input")),
        ];

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }
}
