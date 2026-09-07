namespace PineGuard.Fake.UnitTests;

public static class FakeClausesTestData
{
    // The Value tuple's element names are camelCase AND match
    // FakeClauses.IsBetween's real parameter names exactly (value, min, max)
    // — the clean case this rule must accept.
    public static class IsBetween
    {
        // The dominant shape in the repo (~1,533 sites) and the one every
        // §4.3 example is written in: the tuple is the first type argument of
        // a *Case generic, with no case record anywhere. Also camelCase and
        // parameter-name-exact, so it must produce zero findings.
        public static TheoryData<RuleCase<(int value, int min, int max)>> Cases => [];

        public sealed record ValidCase(string Name, (int value, int min, int max) Value, bool Expected);
    }

    // An ordinary record with a plain positional parameter list — NOT a
    // tuple. This is the direct regression fixture for the old Rule54
    // regex bug (plan §2.4): the old regex was not anchored to a tuple's own
    // parentheses, so it read this record's own parameter list — including
    // its legitimately PascalCase "Value" and "Expected" properties — as if
    // it were a tuple's element list. The AST-based rule must see this
    // record's Value type as `nullable_type` (`string?`), never `tuple_type`,
    // and skip it entirely.
    public sealed record PlainCase(string Name, string? Value, bool Expected);
}
