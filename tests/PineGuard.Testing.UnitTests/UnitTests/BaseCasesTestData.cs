namespace PineGuard.Testing.UnitTests.UnitTests;

public static class BaseCasesTestData
{
    public sealed record ConcreteReturnCase(string Name, string? Value, bool Expected)
        : ReturnCase<string?, bool>(Name, Value, Expected);

    public sealed record ConcreteReturnOutCase(string Name, string? Value, bool Expected, string? ExpectedOutValue)
        : ReturnOutCase<string?, bool, string?>(Name, Value, Expected, ExpectedOutValue);

    public sealed record ConcreteIsCase(string Name, string? Value, bool Expected)
        : IsCase<string?>(Name, Value, Expected);

    public sealed record ConcreteHasCase(string Name, string? Value, bool Expected)
        : HasCase<string?>(Name, Value, Expected);

    public sealed record ConcreteTryCase(string Name, string? Value, bool Expected, string? ExpectedOutValue)
        : TryCase<string?, string?>(Name, Value, Expected, ExpectedOutValue);

    public static class BaseCaseOps
    {
        public sealed record Case(string Name, string Value, string Expected)
            : ReturnCase<string, string>(Name, Value, Expected);

        public static TheoryData<Case> ValidCases =>
        [
            new("non-empty name", "my-case", "my-case"),
            new("spaces in name", "hello world", "hello world")
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("empty name", string.Empty, string.Empty)
        ];

    }

    public static class ReturnCaseOps
    {
        public sealed record Case(string Name, (string? value, bool Expected) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("non-null value, true", (value: "hello", Expected: true)),
            new("non-null value, false", (value: "world", Expected: false))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null value", (value: null, Expected: false))
        ];

    }

    public static class ReturnOutCaseOps
    {
        public sealed record Case(string Name, (string? value, bool Expected, string? expectedOutValue) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("all non-null", (value: "in", Expected: true, expectedOutValue: "out")),
            new("false return", (value: "bad", Expected: false, expectedOutValue: null))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null value and out", (value: null, Expected: false, expectedOutValue: null))
        ];

    }

    public static class IsCaseOps
    {
        public sealed record Case(string Name, (string? value, bool Expected) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("true result", (value: "abc", Expected: true)),
            new("false result", (value: string.Empty, Expected: false))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null value", (value: null, Expected: false))
        ];

    }

    public static class HasCaseOps
    {
        public sealed record Case(string Name, (string? value, bool Expected) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("true result", (value: "abc", Expected: true)),
            new("false result", (value: string.Empty, Expected: false))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null value", (value: null, Expected: false))
        ];

    }

    public static class TryCaseOps
    {
        public sealed record Case(string Name, (string? value, bool Expected, string? expectedOutValue) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("success with out", (value: "123", Expected: true, expectedOutValue: "parsed")),
            new("failure with null out", (value: "bad", Expected: false, expectedOutValue: null))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null value", (value: null, Expected: false, expectedOutValue: null))
        ];

    }
}
