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
            new("0 with nulls", 0, null, null)
        ];

        public static TheoryData<StringValidCase> StringValidCases =>
        [
            new("abc", "abc", "original", "value"),
            new("null allowed", null, 42, "value")
        ];

        public sealed record IntValidCase(string Name, int Result, object? InputValue, string? ParamName)
            : ReturnCase<(int Result, object? Value, string? ParamName), (bool Success, string Message, string? ParamName, object? Value, int Result)>(Name, (Result, InputValue, ParamName), (true, string.Empty, ParamName, InputValue, Result));

        public sealed record StringValidCase(string Name, string? Result, object? InputValue, string? ParamName)
            : ReturnCase<(string? Result, object? Value, string? ParamName), (bool Success, string Message, string? ParamName, object? Value, string? Result)>(Name, (Result, InputValue, ParamName), (true, string.Empty, ParamName, InputValue, Result));
    }

    public static class Fail
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("template with param", "{paramName} must be valid.", "value", 123, "value must be valid."),
            new("no param name", "No param name.", null, 123, "No param name.")
        ];

        public sealed record ValidCase(string Name, string Template, string? ParamName, object? InputValue, string ExpectedMessage)
            : ReturnCase<(string Template, string? ParamName, object? Value), (bool Success, string Message, string? ParamName, object? Value)>(Name, (Template, ParamName, InputValue), (false, ExpectedMessage, ParamName, InputValue));
    }

    public static class FailCoded
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("template with param", "test.code.a", "{paramName} must be valid.", "value", 123, "value must be valid."),
            new("no param name", "test.code.b", "No param name.", null, 123, "No param name.")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("empty code", string.Empty, "{paramName} must be valid.", "value", 123, new ExpectedException(typeof(ArgumentException), "code")),
            new InvalidCase("whitespace code", "   ", "{paramName} must be valid.", "value", 123, new ExpectedException(typeof(ArgumentException), "code")),
            new InvalidCase("null code", null!, "{paramName} must be valid.", "value", 123, new ExpectedException(typeof(ArgumentNullException), "code"))
        ];

        public sealed record ValidCase(string Name, string Code, string Template, string? ParamName, object? InputValue, string ExpectedMessage)
            : ReturnCase<(string Code, string Template, string? ParamName, object? Value), (bool Success, string Code, string Message, string MessageTemplate, string? ParamName, object? Value)>(Name, (Code, Template, ParamName, InputValue), (false, Code, ExpectedMessage, Template, ParamName, InputValue));

        public sealed record InvalidCase(string Name, string Code, string Template, string? ParamName, object? InputValue, ExpectedException ExpectedException)
            : ThrowsCase<(string Code, string Template, string? ParamName, object? Value)>(Name, (Code, Template, ParamName, InputValue), ExpectedException);
    }

    public static class FromBoolWithResult
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", true, "{paramName} must be valid.", "value", 123, 7, true, string.Empty),
            new("fail", false, "{paramName} must be valid.", "value", 123, 7, false, "value must be valid.")
        ];

        public sealed record ValidCase(string Name, bool IsOk, string Template, string? ParamName, object? InputValue, int Result, bool IsSuccess, string ExpectedMessage)
            : ReturnCase<(bool Ok, string Template, string? ParamName, object? Value, int Result), (bool IsSuccess, string ExpectedMessage, string? ExpectedParamName, object? ExpectedValue, int ExpectedResult)>(Name, (IsOk, Template, ParamName, InputValue, Result), (IsSuccess, ExpectedMessage, ParamName, InputValue, IsSuccess ? Result : 0));
    }

    public static class FromBoolWithoutResult
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", true, "{paramName} must be valid.", "value", 123, true, string.Empty),
            new("fail", false, "{paramName} must be valid.", "value", 123, false, "value must be valid.")
        ];

        public sealed record ValidCase(string Name, bool IsOk, string Template, string? ParamName, object? InputValue, bool IsSuccess, string ExpectedMessage)
            : ReturnCase<(bool Ok, string Template, string? ParamName, object? Value), (bool IsSuccess, string ExpectedMessage, string? ExpectedParamName, object? ExpectedValue)>(Name, (IsOk, Template, ParamName, InputValue), (IsSuccess, ExpectedMessage, ParamName, InputValue));
    }

    public static class FromBoolCoded
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", true, "test.code", "{paramName} must be valid.", "value", 123, 7, true, string.Empty),
            new("fail", false, "test.code", "{paramName} must be valid.", "value", 123, 7, false, "value must be valid.")
        ];

        public sealed record ValidCase(string Name, bool IsOk, string Code, string Template, string? ParamName, object? InputValue, int Result, bool IsSuccess, string ExpectedMessage)
            : ReturnCase<(bool Ok, string Code, string Template, string? ParamName, object? Value, int Result), (bool IsSuccess, string ExpectedCode, string ExpectedMessage, string? ExpectedParamName, object? ExpectedValue, int ExpectedResult)>(Name, (IsOk, Code, Template, ParamName, InputValue, Result), (IsSuccess, IsSuccess ? string.Empty : Code, ExpectedMessage, ParamName, InputValue, IsSuccess ? Result : 0));
    }

    public static class ThrowIfFailed
    {
        public static TheoryData<InvalidCase> ThrowIfFailedInvalidCases =>
        [
            new("fails with param", MustResult<int>.Fail("{paramName} must be valid.", "value", 123), new ExpectedException(typeof(ArgumentException), "value", "value must be valid.")),
            new("fails with null param", MustResult<int>.Fail("No param name.", null, 123), new ExpectedException(typeof(ArgumentException), null, "No param name."))
        ];

        public static TheoryData<InvalidCase> ThrowNullIfFailedInvalidCases =>
        [
            new("fails with param", MustResult<int>.Fail("{paramName} must be valid.", "value", 123), new ExpectedException(typeof(ArgumentNullException), "value")),
            new("fails with null param", MustResult<int>.Fail("No param name.", null, 123), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public static TheoryData<InvalidCase> ThrowIfFailedGenericInvalidCases =>
        [
            new("fails with param", MustResult<int>.Fail("{paramName} must be valid.", "value", 123), new ExpectedException(typeof(InvalidOperationException), null, "value must be valid.")),
            new("fails with null param", MustResult<int>.Fail("No param name.", null, 123), new ExpectedException(typeof(InvalidOperationException), null, "No param name."))
        ];

        public static TheoryData<InvalidCase> ThrowIfFailedResultInvalidCases =>
        [
            new("fails with code", MustResult<int>.Fail("test.code", "{paramName} must be valid.", "value", 123), new ExpectedException(typeof(InvalidOperationException), null, "test.code:value must be valid."))
        ];

        public static TheoryData<DataStampingCase> DataStampingCases =>
        [
            new("stamps code and property path when param name is known", MustResult<int>.Fail("test.code", "{paramName} must be valid.", "value", 123), "test.code", "value"),
            new("stamps code only when param name is unknown", MustResult<int>.Fail("test.other-code", "No param name.", null, 123), "test.other-code", string.Empty)
        ];

        public sealed record DataStampingCase(string Name, MustResult<int> MustResult, string ExpectedCode, string ExpectedPropertyPath)
            : ValueCase<MustResult<int>>(Name, MustResult);

        public static TheoryData<ImplicitBoolCase> ImplicitBoolCases =>
        [
            new("null reference converts to false", null, false),
            new("non-null successful reference converts to true", MustResult<int>.Ok(1), true),
            new("non-null failed reference converts to false", MustResult<int>.Fail("{paramName} failed.", "p", 1), false)
        ];

        public sealed record ImplicitBoolCase(string Name, MustResult<int>? MustResult, bool Expected)
            : ValueCase<MustResult<int>?>(Name, MustResult);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok with param", MustResult<int>.Ok(1, "original", "value")),
            new("ok with nulls", MustResult<int>.Ok(0))
        ];

        public static TheoryData<ValidCase> NullFactoryCases =>
        [
            new("successful result", MustResult<int>.Ok(1, "original", "value")),
            new("failed result", MustResult<int>.Fail("{paramName} must be valid.", "value", 123))
        ];

        public sealed record ValidCase(string Name, MustResult<int> MustResult)
            : ValueCase<MustResult<int>>(Name, MustResult);

        public sealed record InvalidCase(string Name, MustResult<int> MustResult, ExpectedException ExpectedException)
            : ThrowsCase<MustResult<int>>(Name, MustResult, ExpectedException);
    }

    public static class OrThrow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("successful returns result", MustResult<int>.Ok(7, value: "original", paramName: "value"), 7)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "failed throws",
                MustResult<int>.Fail("{paramName} must be valid.", "value", 123),
                new ExpectedException(typeof(ArgumentException), "value"))
        ];

        public static TheoryData<FallbackValidCase> FallbackValidCases =>
        [
            new("result null returns fallback", (MustResult<string?>.Ok(null, "original", "value"), "fallback"), "fallback"),
            new("result not null returns result", (MustResult<string?>.Ok("result", "original", "value"), "fallback"), "result")
        ];

        public static TheoryData<NullResultCase> NullResultCases =>
        [
            new("successful with null result returns null", MustResult<string?>.Ok(null, "original", "value"))
        ];

        public sealed record ValidCase(string Name, MustResult<int> Value, int Expected)
            : ReturnCase<MustResult<int>, int>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, MustResult<int> Value, ExpectedException ExpectedException)
            : ThrowsCase<MustResult<int>>(Name, Value, ExpectedException);

        public sealed record FallbackValidCase(string Name, (MustResult<string?> MustResult, string Fallback) Value, string? Expected)
            : ReturnCase<(MustResult<string?> MustResult, string Fallback), string?>(Name, Value, Expected);

        public sealed record NullResultCase(string Name, MustResult<string?> Value)
            : ValueCase<MustResult<string?>>(Name, Value);
    }

    public static class Combine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(
                "returns first successful when no failures",
                [
                    MustResult<int>.Ok(1, value: "v1", paramName: "p1"),
                    MustResult<int>.Ok(2, value: "v2", paramName: "p2")
                ],
                (
                    Success: true,
                    Result: 1,
                    Value: "v1",
                    ParamName: "p1",
                    ExpectedMessage: string.Empty,
                    MessageContains: [])),

            new(
                "returns default ok when empty",
                [],
                (
                    Success: true,
                    Result: 0,
                    Value: null,
                    ParamName: null,
                    ExpectedMessage: string.Empty,
                    MessageContains: [])),

            new(
                "joins failure messages and uses first failure param name",
                [
                    MustResult<int>.Ok(1, value: "v1", paramName: "p1"),
                    MustResult<int>.Fail("{paramName} failed.", "a", 1),
                    MustResult<int>.Fail("{paramName} failed.", "b", 2)
                ],
                (
                    Success: false,
                    Result: 0,
                    Value: 1,
                    ParamName: "a",
                    ExpectedMessage: null,
                    MessageContains: ["a failed.", "b failed.", "; "])),

            new(
                "does not re-substitute a leftover placeholder from a failure with no param name",
                [
                    MustResult<int>.Fail("{paramName} must be positive.", "age", -1),
                    MustResult<int>.Fail("{paramName} must not be empty.", null, "")
                ],
                (
                    Success: false,
                    Result: 0,
                    Value: -1,
                    ParamName: "age",
                    ExpectedMessage: null,
                    MessageContains: ["age must be positive.", "{paramName} must not be empty.", "; "]))
        ];

        public static TheoryData<NullCase> NullCases =>
        [
            new("results null", null, false)
        ];

        public sealed record ValidCase(string Name, MustResult<int>[] Value, (bool Success, int Result, object? Value, string? ParamName, string? ExpectedMessage, string[] MessageContains) Expected)
            : ReturnCase<MustResult<int>[], (bool Success, int Result, object? Value, string? ParamName, string? ExpectedMessage, string[] MessageContains)>(Name, Value, Expected);

        public sealed record NullCase(string Name, IEnumerable<MustResult<int>>? Results, bool Expected)
            : IsCase<IEnumerable<MustResult<int>>?>(Name, Results, Expected);
    }

    public static class CombineCoded
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(
                "carries first failure's code and message template",
                [
                    MustResult<int>.Fail("first.code", "{paramName} first failed.", "a", 1),
                    MustResult<int>.Fail("second.code", "{paramName} second failed.", "b", 2)
                ],
                ("first.code", "{paramName} first failed."))
        ];

        public sealed record ValidCase(string Name, MustResult<int>[] Value, (string ExpectedCode, string ExpectedMessageTemplate) Expected)
            : ReturnCase<MustResult<int>[], (string ExpectedCode, string ExpectedMessageTemplate)>(Name, Value, Expected);
    }

    public static class ThrowIfAnyFailed
    {
        public static TheoryData<Case> Cases =>
        [
            new("any failed", true),
            new("none failed", false)
        ];

        public sealed record Case(string Name, bool AnyFailed)
            : ValueCase<bool>(Name, AnyFailed);
    }

    public static class AndThen
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("success chains to next", MustResult<int>.Ok(3, "v", "p"), (true, "6", "", "", null, null)),
            new("failure propagates without calling next", MustResult<int>.Fail("source.code", "{paramName} failed.", "p", 1), (false, null, "source.code", "{paramName} failed.", "p", 1))
        ];

        public sealed record ValidCase(string Name, MustResult<int> Value, (bool Success, string? Result, string ExpectedCode, string ExpectedMessageTemplate, string? ExpectedParamName, object? ExpectedValue) Expected)
            : ReturnCase<MustResult<int>, (bool Success, string? Result, string ExpectedCode, string ExpectedMessageTemplate, string? ExpectedParamName, object? ExpectedValue)>(Name, Value, Expected);
    }

    public static class When
    {
        public static TheoryData<Case> Cases =>
        [
            new("condition true keeps failure", MustResult<int>.Fail("source.code", "{paramName} failed.", "p", 1), true, false),
            new("condition false becomes success", MustResult<int>.Fail("source.code", "{paramName} failed.", "p", 1), false, true)
        ];

        public sealed record Case(string Name, MustResult<int> Value, bool Condition, bool ExpectedSuccess)
            : ValueCase<MustResult<int>>(Name, Value);
    }

    public static class Unless
    {
        public static TheoryData<Case> Cases =>
        [
            new("condition true becomes success", MustResult<int>.Fail("source.code", "{paramName} failed.", "p", 1), true, true),
            new("condition false keeps failure", MustResult<int>.Fail("source.code", "{paramName} failed.", "p", 1), false, false)
        ];

        public sealed record Case(string Name, MustResult<int> Value, bool Condition, bool ExpectedSuccess)
            : ValueCase<MustResult<int>>(Name, Value);
    }

    public static class ToMustValidationResult
    {
        public static TheoryData<Case> Cases =>
        [
            new("success lifts to Ok", MustResult<int>.Ok(1), null, true, 0, null, null),
            new("failure without path uses ParamName", MustResult<int>.Fail("source.code", "{paramName} must be valid.", "p", 1), null, false, 1, "p", "p must be valid."),
            new("failure with path re-renders template", MustResult<int>.Fail("source.code", "{paramName} must be valid.", "p", 1), "Order.Email", false, 1, "Order.Email", "Order.Email must be valid.")
        ];

        public sealed record Case(string Name, MustResult<int> Value, string? PropertyPath, bool ExpectedSuccess, int ExpectedFailureCount, string? ExpectedPropertyPath, string? ExpectedMessage)
            : ValueCase<MustResult<int>>(Name, Value);
    }

    public static class NullArgumentGuards
    {
        public static TheoryData<Case> Cases =>
        [
            new("AndThen null source", NullSource_AndThen, new ExpectedException(typeof(ArgumentNullException), "result")),
            new("AndThen null next", NullNext_AndThen, new ExpectedException(typeof(ArgumentNullException), "next")),
            new("ToMustValidationResult null source", NullSource_ToMustValidationResult, new ExpectedException(typeof(ArgumentNullException), "result"))
        ];

        private static void NullSource_AndThen()
        {
            MustResult<int>? source = null;
            source!.AndThen(v => MustResult<int>.Ok(v));
        }

        private static void NullNext_AndThen() => MustResult<int>.Ok(1).AndThen<int, int>(null!);

        private static void NullSource_ToMustValidationResult()
        {
            MustResult<int>? source = null;
            source!.ToMustValidationResult();
        }

        public sealed record Case(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
