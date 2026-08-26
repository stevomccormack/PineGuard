using PineGuard.Core.UnitTests.MustClauses.Samples;

namespace PineGuard.Core.UnitTests.MustClauses;

public static class InlineMustValidatorTestData
{
    public sealed record OrderWithSingleLine(OrderLine? Line);

    public static class RuleForSingle
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RuleForCrossProperty
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RuleForValidator
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RuleForEachSingle
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RuleForEachCrossProperty
    {
        public static TheoryData<bool> Cases => [true];
    }

    public static class RuleForEachValidator
    {
        public static TheoryData<bool> Cases => [true];
    }
}
