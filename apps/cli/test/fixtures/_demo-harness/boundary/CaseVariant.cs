namespace Demo;

public static class Greeter
{
    // Boundary probe #2: the word appears in a comment below, but in
    // lowercase rather than the rule's fully-uppercase target token. The
    // demo rule's check (see test/support/runRule.test.ts) is
    // case-sensitive, so a lowercase occurrence is NOT flagged. Asserts the
    // rule does NOT flag this file — exercising the case-sensitivity edge
    // of the same naive text match.
    public static string Hello(string name) => $"Hello, {name}!"; // badword, lowercase, should not trip
}
