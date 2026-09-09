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

    // The same violation in the shape the scenario architecture actually
    // uses — the tuple as a *Case generic's first type argument, with no case
    // record at all. Before P4.3 this shape was invisible to the rule, so a
    // PR introducing it was never flagged even though it is the form every
    // §4.3 example is written in.
    public static class ExactLength
    {
        public static TheoryData<RuleCase<(string? Value, int Length)>> Cases => [];
    }
}
