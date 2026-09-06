using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.GuardClauses.UnitTests;

/// <summary>
/// test-records VIBE valid/ case (b): Guard is a "custom case records
/// forbidden" layer, and this file honours that — every Op Group uses the
/// shared <see cref="GuardCase{TValue}"/> directly. <c>Widget</c> is a bare
/// value-object record (no base at all, real repo precedent) used only as
/// the case's <c>Value</c> payload — it is not named with the <c>Case</c>
/// suffix and carries domain data, not scenario metadata, so the
/// base-record convention never applies to it. This is the direct proof
/// that the old blanket-scoping bug (plan §2.4) is fixed: a legitimately
/// bare record in a context where it is fine must not be flagged.
/// </summary>
public sealed record Widget(string Name, int Count);

public static class GuardValidatorClausesTestData
{
    public static class NotNullWidget
    {
        public static TheoryData<GuardCase<Widget?>> ValidCases =>
        [
            new("populated", new Widget("bolt", 3), new GuardExpected(true)),
        ];

        public static TheoryData<GuardCase<Widget?>> InvalidCases =>
        [
            new("null", null, new GuardExpected(false, typeof(ArgumentNullException), "widget")),
        ];
    }
}
