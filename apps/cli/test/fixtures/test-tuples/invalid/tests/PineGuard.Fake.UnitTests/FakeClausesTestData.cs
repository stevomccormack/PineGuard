namespace PineGuard.Fake.UnitTests;

public static class FakeClausesTestData
{
    // Every element of the Value tuple is PascalCase — the exact "mandatory
    // fail fixture" this rule must never go silently vacuous on (the same
    // failure mode that let the old tool's Rules 03/04/05/07 pass for
    // months). This fixture has no paired src/ tree at all, so the
    // exact-parameter-name check is unresolvable and gracefully skipped;
    // only the camelCase check applies, giving one finding per element.
    public static class LengthBetween
    {
        public sealed record ValidCase(string Name, (string? Value, int Min, int Max) Value, bool Expected);
    }
}
