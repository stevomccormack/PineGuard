using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

/// <summary>
/// test-records VIBE boundary/ case: real repo precedent
/// (tests/PineGuard.Core.UnitTests/GuardClauses/GuardExceptionPolicyTestData.cs)
/// — a Core case record that DOES have a base clause, so it is not the
/// "missing base entirely" failure mode, but the base is the raw
/// <c>BaseCase</c> infrastructure type rather than
/// <c>ReturnCase&lt;,&gt;</c>/<c>ThrowsCase&lt;&gt;</c>. A different failure
/// mode from the invalid/ fixture's fully-bare record — still wrong, but for
/// a different reason (skips the Expected-carrying shape rather than
/// omitting a base outright).
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
