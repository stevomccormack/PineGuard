using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class ChecksumUtilityTestData
{
    public static class IsLuhn
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("digits satisfy the Luhn checksum", "79927398713", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("empty span returns false", "", false),
            new("character above the digit range returns false", "4539a", false),
            new("character below the digit range returns false", "4539/", false),
            new("digits fail the Luhn checksum", "79927398711", false)
        ];

        public sealed record ValidCase(string Name, string Value, bool Expected)
            : ReturnCase<string, bool>(Name, Value, Expected);
    }
}
