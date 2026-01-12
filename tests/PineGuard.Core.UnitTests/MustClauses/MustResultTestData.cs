using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustResultTestData
{
    public static class Ok
    {
        public static TheoryData<IntValidCase> IntValidCases =>
        [
            new("123 with value+param", 123, "original", "value"),
            new("0 with nulls", 0, null, null),
        ];

        public static TheoryData<StringValidCase> StringValidCases =>
        [
            new("abc", "abc", "original", "value"),
            new("null allowed", null, 42, "value"),
        ];

        public sealed record IntValidCase(string Name, int Result, object? InputValue, string? ParamName)
            : ReturnCase<(int Result, object? Value, string? ParamName), (bool Success, string Message, string? ParamName, object? Value, int Result)>(
                Name,
            (Result, InputValue, ParamName),
            (true, string.Empty, ParamName, InputValue, Result));

        public sealed record StringValidCase(string Name, string? Result, object? InputValue, string? ParamName)
            : ReturnCase<(string? Result, object? Value, string? ParamName), (bool Success, string Message, string? ParamName, object? Value, string? Result)>(
                Name,
            (Result, InputValue, ParamName),
            (true, string.Empty, ParamName, InputValue, Result));
    }

    public static class Fail
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("template with param", "{paramName} must be valid.", "value", 123, "value must be valid."),
            new("no param name", "No param name.", null, 123, "No param name."),
        ];

        public sealed record ValidCase(string Name, string Template, string? ParamName, object? InputValue, string ExpectedMessage)
            : ReturnCase<(string Template, string? ParamName, object? Value), (bool Success, string Message, string? ParamName, object? Value)>(
                Name,
            (Template, ParamName, InputValue),
            (false, ExpectedMessage, ParamName, InputValue));
    }

    public static class FromBoolWithResult
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", true, "{paramName} must be valid.", "value", 123, 7, true, string.Empty),
            new("fail", false, "{paramName} must be valid.", "value", 123, 7, false, "value must be valid."),
        ];

        public sealed record ValidCase(
            string Name,
            bool IsOk,
            string Template,
            string? ParamName,
            object? InputValue,
            int Result,
            bool ExpectedSuccess,
            string ExpectedMessage)
            : ReturnCase<(bool Ok, string Template, string? ParamName, object? Value, int Result), (bool ExpectedSuccess, string ExpectedMessage, string? ExpectedParamName, object? ExpectedValue, int ExpectedResult)>(
                Name,
                (IsOk, Template, ParamName, InputValue, Result),
                (ExpectedSuccess, ExpectedMessage, ParamName, InputValue, ExpectedSuccess ? Result : 0));
    }

    public static class FromBoolWithoutResult
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", true, "{paramName} must be valid.", "value", 123, true, string.Empty),
            new("fail", false, "{paramName} must be valid.", "value", 123, false, "value must be valid."),
        ];

        public sealed record ValidCase(
            string Name,
            bool IsOk,
            string Template,
            string? ParamName,
            object? InputValue,
            bool ExpectedSuccess,
            string ExpectedMessage)
            : ReturnCase<(bool Ok, string Template, string? ParamName, object? Value), (bool ExpectedSuccess, string ExpectedMessage, string? ExpectedParamName, object? ExpectedValue)>(
                Name,
                (IsOk, Template, ParamName, InputValue),
                (ExpectedSuccess, ExpectedMessage, ParamName, InputValue));
    }

    public static class ThrowIfFailed
    {
        public static TheoryData<InvalidCase> ThrowIfFailedInvalidCases =>
        [
            new(
                "fails with param",
                MustResult<int>.Fail("{paramName} must be valid.", "value", 123),
                new ExpectedException(typeof(ArgumentException), "value", "value must be valid.")),
            new(
                "fails with null param",
                MustResult<int>.Fail("No param name.", null, 123),
                new ExpectedException(typeof(ArgumentException), null, "No param name.")),
        ];

        public static TheoryData<InvalidCase> ThrowNullIfFailedInvalidCases =>
        [
            new(
                "fails with param",
                MustResult<int>.Fail("{paramName} must be valid.", "value", 123),
                new ExpectedException(typeof(ArgumentNullException), "value")),
            new(
                "fails with null param",
                MustResult<int>.Fail("No param name.", null, 123),
                new ExpectedException(typeof(ArgumentNullException))),
        ];

        public static TheoryData<InvalidCase> ThrowIfFailedGenericInvalidCases =>
        [
            new(
                "fails with param",
                MustResult<int>.Fail("{paramName} must be valid.", "value", 123),
                new ExpectedException(typeof(InvalidOperationException), null, "value must be valid.")),
            new(
                "fails with null param",
                MustResult<int>.Fail("No param name.", null, 123),
                new ExpectedException(typeof(InvalidOperationException), null, "No param name.")),
        ];

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok with param", MustResult<int>.Ok(1, "original", "value")),
            new("ok with nulls", MustResult<int>.Ok(0)),
        ];

        public sealed record ValidCase(string Name, MustResult<int> MustResult)
            : ValueCase<MustResult<int>>(Name, MustResult);

        public sealed record InvalidCase(string Name, MustResult<int> MustResult, ExpectedException ExpectedException)
            : ThrowsCase<MustResult<int>>(Name, MustResult, ExpectedException);
    }

    public static class OrThrow
    {
        public static TheoryData<FallbackValidCase> FallbackValidCases =>
        [
            new(
                "result null returns fallback",
                (MustResult<string?>.Ok(null, "original", "value"), "fallback"),
                "fallback"),
            new(
                "result not null returns result",
                (MustResult<string?>.Ok("result", "original", "value"), "fallback"),
                "result"),
        ];

        public sealed record FallbackValidCase(
            string Name,
            (MustResult<string?> MustResult, string Fallback) Value,
            string? ExpectedReturn)
            : ReturnCase<(MustResult<string?> MustResult, string Fallback), string?>(Name, Value, ExpectedReturn);
    }

    public static class Combine
    {
        public static TheoryData<NullCase> NullCases =>
        [
            new("results null", null, false),
        ];

        public sealed record NullCase(string Name, IEnumerable<MustResult<int>>? Results, bool ExpectedSuccess)
            : IsCase<IEnumerable<MustResult<int>>?>(Name, Results, ExpectedSuccess);
    }

    public static class ThrowIfAnyFailed
    {
        public static TheoryData<Case> Cases =>
        [
            new("any failed", true),
            new("none failed", false),
        ];

        public sealed record Case(string Name, bool AnyFailed)
            : ValueCase<bool>(Name, AnyFailed);
    }
}
