using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringGeoLocationAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    // LatitudeString — valid range [-90, 90]
    public static class LatitudeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("valid", "45.0", true), new("negative", "-89.5", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("out of range", "100.0", false), new("unparseable", "abc", false)];
    }

    // LongitudeString — valid range [-180, 180]
    public static class LongitudeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("valid", "120.0", true), new("negative", "-179.5", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("out of range", "200.0", false), new("unparseable", "abc", false)];
    }
}
