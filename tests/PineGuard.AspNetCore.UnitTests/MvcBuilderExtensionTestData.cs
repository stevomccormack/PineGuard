using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MvcBuilderExtensionTestData
{
    public static class AddMustValidation
    {
        public static TheoryData<Case> Cases =>
        [
            new("one-call-adds-the-action-filter", 1, new FilterCountExpected(true, 1)),
            new("a-second-call-adds-it-again", 2, new FilterCountExpected(true, 2))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase("null-builder", () => MvcBuilderExtension.AddMustValidation(null!), new ExpectedException(typeof(ArgumentNullException), "builder"))
        ];

        public sealed record FilterCountExpected(bool IsValid, int FilterCount) : ReturnExpected(IsValid);

        public sealed record Case(string Name, int Value, FilterCountExpected Expected)
            : ReturnCase<int, FilterCountExpected>(Name, Value, Expected);

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
