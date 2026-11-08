namespace PineGuard.FluentValidation.UnitTests;

public static class FluentNumberComparisonExtensionsTestData
{
    public static TheoryData<int?, int, bool, bool, bool, bool> NullableCases => new()
    {
        { null, 10, true, true, true, true },
        { 9, 10, false, false, true, true },
        { 10, 10, false, true, false, true },
        { 11, 10, true, true, false, false }
    };

    public static TheoryData<int, int, bool, bool, bool, bool> Cases => new()
    {
        { 9, 10, false, false, true, true },
        { 10, 10, false, true, false, true },
        { 11, 10, true, true, false, false }
    };
}
