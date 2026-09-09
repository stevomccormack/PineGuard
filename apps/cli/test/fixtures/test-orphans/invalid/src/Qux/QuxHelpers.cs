namespace Sample.Qux;

// The same-named source project exists, but nothing in it is called "Qux" —
// this represents a type renamed or moved without leaving a trace anywhere
// the rule can find, which is real drift QuxTests.cs must still be flagged
// for, not a false positive to be fixed away.
public static class QuxHelpers
{
    public static string Normalize(string value) => value.Trim();
}
