namespace PineGuard.Testing.UnitTests.Common;

public static class ExpectedExceptionTestData
{
    public static class Constructor
    {
        public sealed record Case(string Name, (Type type, string? paramName, string? messageContains) Value, Type ExpectedType, string? ExpectedParamName, string? ExpectedMessageContains)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("type only", (type: typeof(ArgumentException), paramName: null, messageContains: null), typeof(ArgumentException), null, null),
            new("type and paramName", (type: typeof(ArgumentNullException), paramName: "value", messageContains: null), typeof(ArgumentNullException), "value", null),
            new("type and messageContains", (type: typeof(InvalidOperationException), paramName: null, messageContains: "contains"), typeof(InvalidOperationException), null, "contains"),
            new("all", (type: typeof(ArgumentException), paramName: "p", messageContains: "msg"), typeof(ArgumentException), "p", "msg")
        ];

    }
}
