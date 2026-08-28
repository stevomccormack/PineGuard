using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustFailureTestData
{
    public static class From
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("propertyPath null uses ParamName as-is", MustResult<string>.Fail("email.address.invalid", "{paramName} must be a valid email address.", "email", "bad-value"), null, "email", "email must be a valid email address."),
            new("propertyPath null with no ParamName defaults to empty path", MustResult<string>.Fail("email.address.invalid", "{paramName} must be a valid email address.", null, "bad-value"), null, string.Empty, "{paramName} must be a valid email address."),
            new("propertyPath given re-renders MessageTemplate against it", MustResult<string>.Fail("email.address.invalid", "{paramName} must be a valid email address.", "email", "bad-value"), "Order.Email", "Order.Email", "Order.Email must be a valid email address.")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null result", null!, new ExpectedException(typeof(ArgumentNullException), "result")),
            new InvalidCase("successful result", MustResult<string>.Ok("value", "original", "paramName"), new ExpectedException(typeof(ArgumentException), "result"))
        ];

        public sealed record ValidCase(string Name, IMustResult Result, string? PropertyPath, string ExpectedPropertyPath, string ExpectedMessage)
            : ReturnCase<(IMustResult result, string? propertyPath), (string ExpectedPropertyPath, string ExpectedCode, string ExpectedMessage, object? ExpectedValue)>(Name, (Result, PropertyPath), (ExpectedPropertyPath, Result.Code, ExpectedMessage, Result.Value));

        public sealed record InvalidCase(string Name, IMustResult Value, ExpectedException ExpectedException)
            : ThrowsCase<IMustResult>(Name, Value, ExpectedException);
    }

    public static class Properties
    {
        public static TheoryData<Case> Cases =>
        [
            new("Value round trips onto the record and is carried by the record's default ToString", "Order.Email", "email.address.invalid", "Order.Email must be a valid email address.", "SECRET")
        ];

        public sealed record Case(string Name, string PropertyPath, string Code, string Message, string SentinelValue)
            : ValueCase<MustFailure>(Name, new MustFailure(PropertyPath, Code, Message, SentinelValue));
    }
}
