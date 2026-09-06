namespace PineGuard.Fake.UnitTests;

public static class FooTestData
{
    public static TheoryData<Case> ValidCases =>
    [
        new("empty", string.Empty, 0),
        new("short", "hi", 2),
    ];

    public sealed record Case(string Name, string Value, int Expected);
}
