using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class MustTestData
{
    public static class Be
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("singleton", null)
        ];

        public sealed record Case(string Name, object? Value)
            : ValueCase<object?>(Name, Value);
    }
}
