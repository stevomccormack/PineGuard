using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.FilePathRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class FilePathAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class SafeFileName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsSafeFileName.Normal), F.IsSafeFileName.Normal, true),
            new("safe2", "file_name.png", true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsSafeFileName.Slash),     F.IsSafeFileName.Slash,     false),
            new(nameof(F.IsSafeFileName.InvalidChar),F.IsSafeFileName.InvalidChar,false)
        ];
    }

    // HasFileExtension("txt", ".png")
    public static class HasFileExtension
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasFileExtension.MatchesWithDot), F.HasFileExtension.MatchesWithDot.path, true),
            new("png", "image.png", true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("jpg", "image.jpg", false),
            new(nameof(F.HasFileExtension.NoExtension), F.HasFileExtension.NoExtension.path, false)
        ];
    }
}
