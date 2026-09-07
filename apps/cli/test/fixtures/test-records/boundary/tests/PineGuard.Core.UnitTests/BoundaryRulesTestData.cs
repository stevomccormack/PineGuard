using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

/// <summary>
/// test-records VIBE boundary/ case: real repo precedent
/// (tests/PineGuard.Core.UnitTests/GuardClauses/GuardExceptionPolicyTestData.cs)
/// — a Core case record that inherits the raw <c>BaseCase</c> infrastructure
/// type directly, rather than <c>ReturnCase&lt;,&gt;</c>/<c>ThrowsCase&lt;&gt;</c>.
/// This used to be flagged ("wrong-base") until P4.3's spec-conformance
/// review found no spec text supports the ban (plan
/// docs/ai/plans/audit-cli-rebuild.md §9.2 row P4.3, §7.4): unit-test.md
/// §2.2 lists <c>BaseCase</c> and <c>ValueCase&lt;TValue&gt;</c> as current,
/// first-class case types, not superseded. This fixture now proves the
/// opposite of its original purpose: a direct <c>BaseCase</c> base has a
/// real base clause (so it is not the invalid/ fixture's "missing base
/// entirely" case) and must produce ZERO findings.
/// </summary>
public static class BoundaryRulesTestData
{
    public static class InstallExceptionMap
    {
        public static TheoryData<Case> Cases =>
        [
            new("installed", true, true),
        ];

        public sealed record Case(string Name, bool InstallMap, bool Expected) : BaseCase(Name);
    }
}
