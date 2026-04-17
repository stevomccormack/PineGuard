using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public static class GuardTestData
{
    public static class Against
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Singleton clause", true)
        ];

        public sealed record ValidCase(string Name, bool Expected)
            : ReturnCase<object?, bool>(Name, null, Expected);
    }
}
