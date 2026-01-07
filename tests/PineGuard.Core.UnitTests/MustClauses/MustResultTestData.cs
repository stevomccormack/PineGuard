using PineGuard.MustClauses;
using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustResultTestData
{
    public static class Ok
    {
        public static TheoryData<IntValidCase> IntValidCases => new()
        {
            V("123 with value+param", result: 123, value: "original", paramName: "value"),
            V("0 with nulls", result: 0, value: null, paramName: null),
        };

        public static TheoryData<StringValidCase> StringValidCases => new()
        {
            V("abc", result: "abc", value: "original", paramName: "value"),
            V("null allowed", result: null, value: 42, paramName: "value"),
        };

        private static IntValidCase V(string name, int result, object? value, string? paramName) => new(name, result, value, paramName);

        private static StringValidCase V(string name, string? result, object? value, string? paramName) => new(name, result, value, paramName);

        #region Cases

        public abstract record Case(string Name);

        public sealed record IntValidCase(string Name, int Result, object? Value, string? ParamName) : Case(Name);

        public sealed record StringValidCase(string Name, string? Result, object? Value, string? ParamName) : Case(Name);

        #endregion
    }

    public static class Fail
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("template with param", template: "{paramName} must be valid.", paramName: "value", value: 123, expectedMessage: "value must be valid."),
            V("no param name", template: "No param name.", paramName: null, value: 123, expectedMessage: "No param name."),
        };

        private static ValidCase V(string name, string template, string? paramName, object? value, string expectedMessage)
            => new(name, template, paramName, value, expectedMessage);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Template, string? ParamName, object? Value, string ExpectedMessage) : Case(Name);

        #endregion
    }

    public static class FromBoolWithResult
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("ok", ok: true, template: "{paramName} must be valid.", paramName: "value", value: 123, result: 7, expectedSuccess: true, expectedMessage: string.Empty),
            V("fail", ok: false, template: "{paramName} must be valid.", paramName: "value", value: 123, result: 7, expectedSuccess: false, expectedMessage: "value must be valid."),
        };

        private static ValidCase V(string name, bool ok, string template, string? paramName, object? value, int result, bool expectedSuccess, string expectedMessage)
            => new(name, ok, template, paramName, value, result, expectedSuccess, expectedMessage);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(
            string Name,
            bool Ok,
            string Template,
            string? ParamName,
            object? Value,
            int Result,
            bool ExpectedSuccess,
            string ExpectedMessage) : Case(Name);

        #endregion
    }

    public static class FromBoolWithoutResult
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("ok", ok: true, template: "{paramName} must be valid.", paramName: "value", value: 123, expectedSuccess: true, expectedMessage: string.Empty),
            V("fail", ok: false, template: "{paramName} must be valid.", paramName: "value", value: 123, expectedSuccess: false, expectedMessage: "value must be valid."),
        };

        private static ValidCase V(string name, bool ok, string template, string? paramName, object? value, bool expectedSuccess, string expectedMessage)
            => new(name, ok, template, paramName, value, expectedSuccess, expectedMessage);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(
            string Name,
            bool Ok,
            string Template,
            string? ParamName,
            object? Value,
            bool ExpectedSuccess,
            string ExpectedMessage) : Case(Name);

        #endregion
    }

    public static class ThrowIfFailed
    {
        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("fails with param", MustResult<int>.Fail("{paramName} must be valid.", "value", 123)),
            I("fails with null param", MustResult<int>.Fail("No param name.", null, 123)),
        };

        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("ok with param", MustResult<int>.Ok(1, value: "original", paramName: "value")),
            V("ok with nulls", MustResult<int>.Ok(0, value: null, paramName: null)),
        };

        private static InvalidCase I(string name, MustResult<int> mustResult) => new(name, mustResult);

        private static ValidCase V(string name, MustResult<int> mustResult) => new(name, mustResult);

        #region Cases

        public abstract record Case(string Name);

        public sealed record InvalidCase(string Name, MustResult<int> MustResult) : Case(Name);

        public sealed record ValidCase(string Name, MustResult<int> MustResult) : Case(Name);

        #endregion
    }

    public static class OrThrow
    {
        public static TheoryData<FallbackValidCase> FallbackValidCases => new()
        {
            V("result null returns fallback", MustResult<string?>.Ok(result: null, value: "original", paramName: "value"), fallback: "fallback", expected: "fallback"),
            V("result not null returns result", MustResult<string?>.Ok(result: "result", value: "original", paramName: "value"), fallback: "fallback", expected: "result"),
        };

        private static FallbackValidCase V(string name, MustResult<string?> mustResult, string fallback, string? expected)
            => new(name, mustResult, fallback, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record FallbackValidCase(string Name, MustResult<string?> MustResult, string Fallback, string? Expected) : Case(Name);

        #endregion
    }

    public static class Combine
    {
        public static TheoryData<NullCase> NullCases => new()
        {
            V("results null", results: null, expectedSuccess: false),
        };

        private static NullCase V(string name, IEnumerable<MustResult<int>>? results, bool expectedSuccess)
            => new(name, results, expectedSuccess);

        #region Cases

        public abstract record Case(string Name);

        public sealed record NullCase(string Name, IEnumerable<MustResult<int>>? Results, bool ExpectedSuccess) : Case(Name);

        #endregion
    }

    public static class ThrowIfAnyFailed
    {
        public static TheoryData<Case> Cases => new()
        {
            C("any failed", anyFailed: true),
            C("none failed", anyFailed: false),
        };

        private static Case C(string name, bool anyFailed) => new(name, anyFailed);

        #region Cases

        public sealed record Case(string Name, bool AnyFailed);

        #endregion
    }
}
