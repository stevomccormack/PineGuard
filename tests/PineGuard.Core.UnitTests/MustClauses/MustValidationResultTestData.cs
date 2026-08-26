using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustValidationResultTestData
{
    private static readonly MustFailure RootFailure = new(string.Empty, "must.validation.a", "message a", "value-a");
    private static readonly MustFailure NestedFailure = new("Email", "must.validation.b", "message b", "value-b");
    private static readonly IMustResult SuccessResult = MustResult<int>.Ok(1, "v", "p1");
    private static readonly IMustResult FailureWithParam = MustResult<int>.Fail("must.validation.c", "{paramName} must be valid.", "age", -1);
    private static readonly IMustResult FailureWithoutParam = MustResult<int>.Fail("must.validation.d", "{paramName} must not be empty.", null, "");
    private static readonly MustValidationResult SuccessValidation = MustValidationResult.Ok();
    private static readonly MustValidationResult FirstFailureValidation = MustValidationResult.Fail(RootFailure);
    private static readonly MustValidationResult SecondFailureValidation = MustValidationResult.Fail(NestedFailure);

    public static class Ok
    {
        public static TheoryData<Case> Cases =>
        [
            new("returns the shared successful singleton")
        ];

        public sealed record Case(string Name)
            : BaseCase(Name);
    }

    public static class Fail
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single failure with no additional", RootFailure, [], [RootFailure]),
            new("failure with additional failures", RootFailure, [NestedFailure], [RootFailure, NestedFailure])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null failure", (null!, []), new ExpectedException(typeof(ArgumentNullException), "failure")),
            new InvalidCase("null additional", (RootFailure, null!), new ExpectedException(typeof(ArgumentNullException), "additional"))
        ];

        public sealed record ValidCase(string Name, MustFailure Failure, MustFailure[] Additional, MustFailure[] Expected)
            : ReturnCase<(MustFailure failure, MustFailure[] additional), MustFailure[]>(Name, (Failure, Additional), Expected);

        public sealed record InvalidCase(string Name, (MustFailure failure, MustFailure[] additional) Value, ExpectedException ExpectedException)
            : ThrowsCase<(MustFailure failure, MustFailure[] additional)>(Name, Value, ExpectedException);
    }

    public static class FailEnumerable
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single item sequence", [RootFailure], [RootFailure]),
            new("multi item sequence", [RootFailure, NestedFailure], [RootFailure, NestedFailure])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null sequence", null!, new ExpectedException(typeof(ArgumentNullException), "failures")),
            new InvalidCase("empty sequence", [], new ExpectedException(typeof(ArgumentException), "failures"))
        ];

        public sealed record ValidCase(string Name, MustFailure[] Failures, MustFailure[] Expected)
            : ReturnCase<IEnumerable<MustFailure>, MustFailure[]>(Name, Failures, Expected);

        public sealed record InvalidCase(string Name, IEnumerable<MustFailure> Value, ExpectedException ExpectedException)
            : ThrowsCase<IEnumerable<MustFailure>>(Name, Value, ExpectedException);
    }

    public static class From
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("all successful returns the shared Ok singleton", [SuccessResult], []),
            new("mixed results keep only failures with ParamName as the path", [SuccessResult, FailureWithParam, FailureWithoutParam], ["age", ""])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null array", null!, new ExpectedException(typeof(ArgumentNullException), "results"))
        ];

        public sealed record ValidCase(string Name, IMustResult[] Results, string[] ExpectedPropertyPaths)
            : ReturnCase<IMustResult[], string[]>(Name, Results, ExpectedPropertyPaths);

        public sealed record InvalidCase(string Name, IMustResult[] Value, ExpectedException ExpectedException)
            : ThrowsCase<IMustResult[]>(Name, Value, ExpectedException);
    }

    public static class FromEnumerable
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("keeps only failures from a lazy sequence", EnumerateResults(SuccessResult, FailureWithParam), ["age"])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null sequence", null!, new ExpectedException(typeof(ArgumentNullException), "results"))
        ];

        public sealed record ValidCase(string Name, IEnumerable<IMustResult> Results, string[] ExpectedPropertyPaths)
            : ReturnCase<IEnumerable<IMustResult>, string[]>(Name, Results, ExpectedPropertyPaths);

        public sealed record InvalidCase(string Name, IEnumerable<IMustResult> Value, ExpectedException ExpectedException)
            : ThrowsCase<IEnumerable<IMustResult>>(Name, Value, ExpectedException);
    }

    public static class Combine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("all successful returns the shared Ok singleton", [SuccessValidation], []),
            new("merges every failure from every input", [FirstFailureValidation, SuccessValidation, SecondFailureValidation], [RootFailure, NestedFailure])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null array", null!, new ExpectedException(typeof(ArgumentNullException), "results"))
        ];

        public sealed record ValidCase(string Name, MustValidationResult[] Results, MustFailure[] Expected)
            : ReturnCase<MustValidationResult[], MustFailure[]>(Name, Results, Expected);

        public sealed record InvalidCase(string Name, MustValidationResult[] Value, ExpectedException ExpectedException)
            : ThrowsCase<MustValidationResult[]>(Name, Value, ExpectedException);
    }

    public static class CombineEnumerable
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("merges failures from a lazy sequence", EnumerateValidations(FirstFailureValidation, SuccessValidation, SecondFailureValidation), [RootFailure, NestedFailure])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null sequence", null!, new ExpectedException(typeof(ArgumentNullException), "results"))
        ];

        public sealed record ValidCase(string Name, IEnumerable<MustValidationResult> Results, MustFailure[] Expected)
            : ReturnCase<IEnumerable<MustValidationResult>, MustFailure[]>(Name, Results, Expected);

        public sealed record InvalidCase(string Name, IEnumerable<MustValidationResult> Value, ExpectedException ExpectedException)
            : ThrowsCase<IEnumerable<MustValidationResult>>(Name, Value, ExpectedException);
    }

    public static class WithPropertyPathPrefix
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("root failure becomes exactly the prefix", MustValidationResult.Fail(RootFailure), "Order", ["Order"], false),
            new("nested failure becomes prefix.path", MustValidationResult.Fail(NestedFailure), "Order", ["Order.Email"], false),
            new("success returns the same instance", SuccessValidation, "Order", [], true)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null prefix", (MustValidationResult.Fail(RootFailure), null!), new ExpectedException(typeof(ArgumentNullException), "prefix"))
        ];

        public sealed record ValidCase(string Name, MustValidationResult Result, string Prefix, string[] ExpectedPropertyPaths, bool ExpectedSameInstance)
            : ReturnCase<(MustValidationResult result, string prefix), (string[] ExpectedPropertyPaths, bool ExpectedSameInstance)>(Name, (Result, Prefix), (ExpectedPropertyPaths, ExpectedSameInstance));

        public sealed record InvalidCase(string Name, (MustValidationResult result, string prefix) Value, ExpectedException ExpectedException)
            : ThrowsCase<(MustValidationResult result, string prefix)>(Name, Value, ExpectedException);
    }

    public static class ThrowIfFailed
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("success does not throw", SuccessValidation)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("failure throws MustValidationException carrying Result", FirstFailureValidation)
        ];

        public sealed record ValidCase(string Name, MustValidationResult Value)
            : ValueCase<MustValidationResult>(Name, Value);

        public sealed record InvalidCase(string Name, MustValidationResult Value)
            : ValueCase<MustValidationResult>(Name, Value);
    }

    public static class ImplicitBool
    {
        public static TheoryData<Case> Cases =>
        [
            new("null reference converts to false", null, false),
            new("successful instance converts to true", SuccessValidation, true),
            new("failed instance converts to false", FirstFailureValidation, false)
        ];

        public sealed record Case(string Name, MustValidationResult? Value, bool Expected)
            : ReturnCase<MustValidationResult?, bool>(Name, Value, Expected);
    }

    public static class MessageFormatting
    {
        public static TheoryData<Case> Cases =>
        [
            new("success has an empty message", SuccessValidation, string.Empty),
            new("root failure omits the path", FirstFailureValidation, RootFailure.Message),
            new("nested failure includes the path", SecondFailureValidation, $"{NestedFailure.PropertyPath}: {NestedFailure.Message}"),
            new("multiple failures joined by semicolon", MustValidationResult.Fail(RootFailure, NestedFailure), $"{RootFailure.Message}; {NestedFailure.PropertyPath}: {NestedFailure.Message}")
        ];

        public sealed record Case(string Name, MustValidationResult Value, string Expected)
            : ReturnCase<MustValidationResult, string>(Name, Value, Expected);
    }

    private static IEnumerable<IMustResult> EnumerateResults(params IMustResult[] results)
    {
        foreach (var result in results)
            yield return result;
    }

    private static IEnumerable<MustValidationResult> EnumerateValidations(params MustValidationResult[] results)
    {
        foreach (var result in results)
            yield return result;
    }
}
