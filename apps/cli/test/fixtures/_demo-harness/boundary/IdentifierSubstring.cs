namespace Demo;

public static class Greeter
{
    // Boundary probe #1: the rule's target token (see
    // test/support/runRule.test.ts) appears below only as a substring
    // inside a longer identifier, never standing alone. The demo rule's
    // check is a plain substring search with no word-boundary awareness, so
    // it still trips on this identifier even though a human reading it
    // would not call it a standalone occurrence of the target word. Asserts
    // the rule DOES still flag this file — exercising the
    // substring-vs-identifier edge of a naive text match.
    private const string NOTABADWORDPLACEHOLDER = "safe-token";

    public static string Hello(string name) => $"Hello, {name}!";
}
