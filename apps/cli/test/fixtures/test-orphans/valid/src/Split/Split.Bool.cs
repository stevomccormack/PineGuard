namespace Sample.Split;

// Dotted partial-file split, the shape fixture.md §10 documents for
// StringRules.Bool.cs. The paired test file in this repo is named after the
// *file* with the dots removed (StringRulesBoolTests.cs), not after the type
// (which is the whole `StringRules` partial), so the rule must resolve
// SplitBoolTests.cs via the dot-stripped source file name.
public static partial class Split
{
    public static bool IsTrue(bool value) => value;
}
