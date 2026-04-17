using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public static class MustResultTestData
{
    public static class Ok
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("valid", (123, "val", "param"), true)
        ];
        public sealed record ValidCase(string Name, (int Result, string Value, string ParamName) Value, bool Expected)
            : IsCase<(int Result, string Value, string ParamName)>(Name, Value, Expected);
    }

    public static class Fail
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("standard", ("Error {paramName}", "param", "val"), false),
            new("null param", ("Error {paramName}", null, "val"), false)
        ];
        public sealed record ValidCase(string Name, (string Msg, string? ParamName, string Value) Value, bool Expected)
            : IsCase<(string Msg, string? ParamName, string Value)>(Name, Value, Expected);
    }

    public static class FromBool
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("true with res", (true, "err", "p", "v", 1), true),
            new("false with res", (false, "err {paramName}", "p", "v", 0), false),
            new("true no res", (true, "err", "p", "v", 0), true) // 0 is default int
        ];
        public sealed record ValidCase(string Name, (bool Success, string Msg, string ParamName, string Value, int Result) Value, bool Expected)
            : IsCase<(bool Success, string Msg, string ParamName, string Value, int Result)>(Name, Value, Expected);
    }

    public static class Combine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("all ok", [MustResult<int>.Ok(1), MustResult<int>.Ok(2)], true),
            new("any fail", [MustResult<int>.Ok(1), MustResult<int>.Fail("F", "p", "v")], false),
            new("empty", [], true),
            new("null", null, false)
        ];
        public sealed record ValidCase(string Name, IEnumerable<MustResult<int>>? Value, bool Expected)
            : IsCase<IEnumerable<MustResult<int>>?>(Name, Value, Expected);
    }

    public static class ThrowIfAnyFailed
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("all ok", [MustResult<int>.Ok(1)], true),
            new("empty", [], true)
        ];
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("any fail", [MustResult<int>.Ok(1), MustResult<int>.Fail("F", "p", "v")], new ExpectedException(typeof(ArgumentException)))
        ];
        public sealed record ValidCase(string Name, IEnumerable<MustResult<int>> Value, bool Expected) : IsCase<IEnumerable<MustResult<int>>>(Name, Value, Expected);
        public sealed record InvalidCase(string Name, IEnumerable<MustResult<int>> Value, ExpectedException ExpectedException) : ThrowsCase<IEnumerable<MustResult<int>>>(Name, Value, ExpectedException);
    }

    public static class Deconstruct
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", MustResult<int>.Ok(1, "val", "param"), true)
        ];
        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
    }

    public static class ImplicitBool
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok is true", MustResult<int>.Ok(1), true),
            new("fail is false", MustResult<int>.Fail("E", "p", "v"), false)
        ];
        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
    }

    public static class ThrowIfFailed
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", MustResult<int>.Ok(1), true)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
             new("fail", MustResult<int>.Fail("Error", "param", "val"), new ExpectedException(typeof(ArgumentException), "param")),
             new("fail custom", MustResult<int>.Fail("Error", "param", "val"), new ExpectedException(typeof(InvalidOperationException))) { Name = "fail custom" }
        ];

        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
        public sealed record InvalidCase(string Name, MustResult<int> Value, ExpectedException ExpectedException) : ThrowsCase<MustResult<int>>(Name, Value, ExpectedException);
    }

    public static class ThrowIfFailedCustom
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok no throw", MustResult<int>.Ok(1), true)
        ];
        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
    }

    public static class ThrowNullIfFailed
    {
        public static TheoryData<ValidCase> ValidCases => [new("ok", MustResult<int>.Ok(1), true)];
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("fail", MustResult<int>.Fail("E", "p", "v"), new ExpectedException(typeof(ArgumentNullException), "p"))
        ];
        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
        public sealed record InvalidCase(string Name, MustResult<int> Value, ExpectedException ExpectedException) : ThrowsCase<MustResult<int>>(Name, Value, ExpectedException);
    }

    public static class OrThrow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", MustResult<int>.Ok(123), true)
        ];
        public static TheoryData<InvalidCase> InvalidCases =>
      [
          new("fail", MustResult<int>.Fail("E", "p", "v"), new ExpectedException(typeof(ArgumentException)))
      ];
        public sealed record ValidCase(string Name, MustResult<int> Value, bool Expected) : IsCase<MustResult<int>>(Name, Value, Expected);
        public sealed record InvalidCase(string Name, MustResult<int> Value, ExpectedException ExpectedException) : ThrowsCase<MustResult<int>>(Name, Value, ExpectedException);
    }

    public static class OrThrowWithFallback
    {
        // T is int
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", (MustResult<int>.Ok(123), 456), true) // Returns 123
        ];

        // T is string?
        public static TheoryData<ValidCaseNullable> ValidCasesNullable =>
        [
            new("ok", (MustResult<string?>.Ok("ok", "val"), "fallback"), true), // Returns "ok"
            new("ok null result", (MustResult<string?>.Ok(null, "val"), "fallback"), false) // Returns "fallback" (expected "false" logic in test interpretation or custom expected value)
        ];

        public sealed record ValidCase(string Name, (MustResult<int> Result, int Fallback) Value, bool Expected) : IsCase<(MustResult<int> Result, int Fallback)>(Name, Value, Expected);
        public sealed record ValidCaseNullable(string Name, (MustResult<string?> Result, string Fallback) Value, bool UseResult) : IsCase<(MustResult<string?> Result, string Fallback)>(Name, Value, UseResult);
    }
}
