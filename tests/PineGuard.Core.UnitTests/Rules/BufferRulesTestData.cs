using PineGuard.Core.UnitTests.Utils;

namespace PineGuard.Core.UnitTests.Rules;

public static class BufferRulesTestData
{
    public static class IsHex
    {
        public static TheoryData<BufferUtilityTestData.IsHexString.Case> ValidCases => BufferUtilityTestData.IsHexString.ValidCases;

        public static TheoryData<BufferUtilityTestData.IsHexString.Case> EdgeCases => BufferUtilityTestData.IsHexString.EdgeCases;
    }

    public static class IsBase64
    {
        public static TheoryData<BufferUtilityTestData.IsBase64String.Case> ValidCases => BufferUtilityTestData.IsBase64String.ValidCases;

        public static TheoryData<BufferUtilityTestData.IsBase64String.Case> EdgeCases => BufferUtilityTestData.IsBase64String.EdgeCases;
    }
}
