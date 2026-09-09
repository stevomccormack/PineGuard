namespace PineGuard.Fake.UnitTests;

public static class FakeClausesTestData
{
    // Boundary A ("resolvable but renamed"): "val" is valid camelCase, but is
    // a renamed/abbreviated form of FakeClauses.IsBetween's real "value"
    // parameter. The source method IS resolvable here, so this must produce
    // exactly one finding — the exact-name mismatch — and must NOT also
    // produce a camelCase finding, since "val" is camelCase.
    public static class IsBetween
    {
        public sealed record ValidCase(string Name, (int val, int min, int max) Value, bool Expected);
    }

    // Boundary B ("camelCase, no resolvable source"): every element is
    // camelCase, but no method named "NoSuchMethod" exists anywhere under
    // src/ (only FakeClauses.IsBetween does). The exact-name check must be
    // skipped gracefully (best-effort, not a crash and not a false
    // positive), leaving zero findings — proving the two checks are
    // independent and the second one degrades gracefully when unresolvable.
    public static class NoSuchMethod
    {
        public sealed record ValidCase(string Name, (int value, int min, int max) Value, bool Expected);
    }
}
