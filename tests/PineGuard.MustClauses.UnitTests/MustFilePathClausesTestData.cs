using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.FilePathRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustFilePathClausesTestData
{
    public static class SafeFileName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsSafeFileName.Normal),    F.IsSafeFileName.Normal,    true),
            new(nameof(F.IsSafeFileName.Slash),     F.IsSafeFileName.Slash,     false),
            new(nameof(F.IsSafeFileName.Backslash), F.IsSafeFileName.Backslash, false),
            new(nameof(F.IsSafeFileName.Colon),     F.IsSafeFileName.Colon,     false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new(nameof(F.IsSafeFileName.Null), F.IsSafeFileName.Null, false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, string? Value, bool Expected) : IsCase<string?>(Name, Value, Expected);
    }

    public static class HasFileExtension
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.HasFileExtension.MatchesWithDot),  (F.HasFileExtension.MatchesWithDot.path,  [".txt"]), true),
            new(nameof(F.HasFileExtension.NoMatch),         (F.HasFileExtension.NoMatch.path,         [".jpg"]), false),
            new(nameof(F.HasFileExtension.CaseInsensitive), (F.HasFileExtension.CaseInsensitive.path, [".txt"]), true)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new(nameof(F.HasFileExtension.NullPath), (F.HasFileExtension.NullPath.path, [".txt"]), false)
        ];

        public sealed record ValidCase(string Name, (string? value, string[] extensions) Value, bool Expected)
            : IsCase<(string? value, string[] extensions)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (string? value, string[] extensions) Value, bool Expected)
            : IsCase<(string? value, string[] extensions)>(Name, Value, Expected);
    }
}
