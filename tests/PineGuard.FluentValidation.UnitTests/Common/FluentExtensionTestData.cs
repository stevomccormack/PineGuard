using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests.Common;

public static class FluentExtensionTestData
{
    public static class MustBe
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Ok result returns valid", "test", MustResult<bool>.Ok(true), "Error", null, true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Fail uses result message template", "test", MustResult<bool>.Fail("test.code", "Failed {paramName}", null, "test"), null, null, false, "Failed Value"),
            new("Fail uses custom message template", "test", MustResult<bool>.Fail("test.code", "Failed", "param", "test"), "Custom {paramName}", null, false, "Custom Value"),
            new("Fail uses override property name", "test", MustResult<bool>.Fail("test.code", "Failed", "param", "test"), "Custom {paramName}", "CustomProp", false, "Custom Custom Prop"),
            new("Fail uses property path when display name blank", "test", MustResult<bool>.Fail("test.code", "Failed", "param", "test"), "Custom {paramName}", "", false, "Custom ")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null ruleBuilder throws", () =>
            {
                IRuleBuilder<object, string?>? builder = null;
                builder!.MustBe(_ => MustResult<bool>.Ok(true), "message");
            }, new ExpectedException(typeof(ArgumentNullException), "ruleBuilder")),
            new InvalidCase("Null check throws", () =>
            {
                var validator = new InlineValidator<string?>();
                var builder = validator.RuleFor(x => x);
                Func<string?, MustResult<bool>>? check = null;
                builder.MustBe(check!, "message");
            }, new ExpectedException(typeof(ArgumentNullException), "check")),
            new InvalidCase("Null model check throws", () =>
            {
                var validator = new InlineValidator<string?>();
                var builder = validator.RuleFor(x => x);
                Func<string?, string?, MustResult<bool>>? check = null;
                builder.MustBe(check!, "message");
            }, new ExpectedException(typeof(ArgumentNullException), "check"))
        ];

        public sealed record ValidCase(
            string Name,
            string? Value,
            MustResult<bool> Result,
            string? Message,
            string? PropertyNameOverride,
            bool Expected,
            string? ExpectedErrorMessage)
            : ReturnCase<string?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class MustBeModel
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Ok result returns valid", "test", MustResult<bool>.Ok(true), "Error", null, true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Fail uses result message template", "test", MustResult<bool>.Fail("test.code", "Failed {paramName}", null, "test"), null, null, false, "Failed Value"),
            new("Fail uses custom message template", "test", MustResult<bool>.Fail("test.code", "Failed", "param", "test"), "Custom {paramName}", null, false, "Custom Value")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Null ruleBuilder throws", () =>
            {
                IRuleBuilder<object, string?>? builder = null;
                builder!.MustBe((_, _) => MustResult<bool>.Ok(true), "message");
            }, new ExpectedException(typeof(ArgumentNullException), "ruleBuilder")),
            new InvalidCase("Null check throws", () =>
            {
                var validator = new InlineValidator<string?>();
                var builder = validator.RuleFor(x => x);
                Func<string?, string?, MustResult<bool>>? check = null;
                builder.MustBe(check!, "message");
            }, new ExpectedException(typeof(ArgumentNullException), "check"))
        ];

        public sealed record ValidCase(
            string Name,
            string? Value,
            MustResult<bool> Result,
            string? Message,
            string? PropertyNameOverride,
            bool Expected,
            string? ExpectedErrorMessage)
            : ReturnCase<string?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class MustBeStruct
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Ok result returns valid", 1, MustResult<int?>.Ok(1), "Error", null, true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Fail result returns invalid", null, MustResult<int?>.Fail("test.code", "Null {paramName}", null, null), null, null, false, "Null Id")
        ];

        public sealed record ValidCase(
            string Name,
            int? Value,
            MustResult<int?> Result,
            string? Message,
            string? PropertyNameOverride,
            bool Expected,
            string? ExpectedErrorMessage)
            : ReturnCase<int?, bool>(Name, Value, Expected);
    }

    public static class ErrorCode
    {
        public static TheoryData<bool> Cases => [true];
    }
}
