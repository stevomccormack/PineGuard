namespace PineGuard.Fake.UnitTests;

// Boundary: a genuinely pre-existing, legitimately-allowlisted orphan — no
// AllowlistedTestData.cs exists, mirroring the real repo's
// tests/PineGuard.DataAnnotations.UnitTests/ErrorMessageAttributesTests.cs
// entry in tools/audit-cli/test-audit-exceptions.json's Rule50.AllowMissingTestData.
//
// The rule itself has no knowledge of exceptions (that's engine-level, per
// applyExceptions in src/audit/engine.ts) — running this rule directly still
// flags [MissingTestData] here. test/rules/test-files.test.ts demonstrates
// that the engine's exceptions mechanism, given a matching entry for this
// file, suppresses that same finding — the still-invalid-at-the-rule,
// still-valid-after-exceptions boundary this fixture exists to probe.
public sealed class AllowlistedTests
{
    [Theory]
    [InlineData("x")]
    public void Allowlisted_BehavesAsExpected(string value)
    {
        Assert.NotEmpty(value);
    }
}
