using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class GuardFailureTestData
{
    public static class NullResultGuard
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class DefaultException
    {
        public static TheoryData<Case> Cases =>
        [
            new("null value falls back to ArgumentNullException", Value: null, ExpectedExceptionType: typeof(ArgumentNullException)),
            new("non-null value falls back to ArgumentException", Value: "x", ExpectedExceptionType: typeof(ArgumentException))
        ];

        public sealed record Case(string Name, object? Value, Type ExpectedExceptionType)
            : ReturnCase<object?, Type>(Name, Value, ExpectedExceptionType);
    }

    public static class MessageOverride
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ExceptionCreatorPrecedence
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class ExceptionCreatorReturnsNull
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class MapReceivesFailure
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class MapReturnsNull
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class PrintMembers
    {
        public static TheoryData<bool> Cases => [true];
    }
}
