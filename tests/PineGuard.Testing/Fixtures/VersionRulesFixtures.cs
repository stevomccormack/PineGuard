using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class VersionRulesFixtures
{
    public static class IsSemVer
    {
        public static readonly string? Simple = "1.0.0";
        public static readonly string? ZeroVersion = "0.0.0";
        public static readonly string? LargeNumbers = "10.20.30";
        public static readonly string? PreRelease = "1.0.0-alpha";
        public static readonly string? PreReleaseNumeric = "1.0.0-alpha.1";
        public static readonly string? PreReleaseZero = "1.0.0-0.3.7";
        public static readonly string? PreReleaseHyphens = "1.0.0-x-y-z.--";
        public static readonly string? BuildMetadata = "1.0.0+20130313144700";
        public static readonly string? PreReleaseAndBuild = "1.0.0-beta+exp.sha.5114f85";
        public static readonly string? Padded = "  1.2.3  ";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? WhiteSpace = "   ";
        public static readonly string? MajorOnly = "1";
        public static readonly string? MissingPatch = "1.0";
        public static readonly string? LeadingV = "v1.0.0";
        public static readonly string? NegativeMajor = "-1.0.0";
        public static readonly string? LeadingZeroMajor = "01.0.0";
        public static readonly string? LeadingZeroMinor = "1.01.0";
        public static readonly string? LeadingZeroPatch = "1.0.01";
        public static readonly string? LeadingZeroPreRelease = "1.0.0-01";
        public static readonly string? EmptyPreRelease = "1.0.0-";
        public static readonly string? EmptyBuildMetadata = "1.0.0+";
        public static readonly string? NonAsciiDigit = "1.٢.0";
        public static readonly string? EmbeddedSpace = "1.0.0 beta";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Simple), Simple, true), new(nameof(ZeroVersion), ZeroVersion, true), new(nameof(LargeNumbers), LargeNumbers, true), new(nameof(PreRelease), PreRelease, true), new(nameof(PreReleaseNumeric), PreReleaseNumeric, true), new(nameof(PreReleaseZero), PreReleaseZero, true), new(nameof(PreReleaseHyphens), PreReleaseHyphens, true), new(nameof(BuildMetadata), BuildMetadata, true), new(nameof(PreReleaseAndBuild), PreReleaseAndBuild, true), new(nameof(Padded), Padded, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(EmptyString), EmptyString, false), new(nameof(WhiteSpace), WhiteSpace, false), new(nameof(MajorOnly), MajorOnly, false), new(nameof(MissingPatch), MissingPatch, false), new(nameof(LeadingV), LeadingV, false), new(nameof(NegativeMajor), NegativeMajor, false), new(nameof(LeadingZeroMajor), LeadingZeroMajor, false), new(nameof(LeadingZeroMinor), LeadingZeroMinor, false), new(nameof(LeadingZeroPatch), LeadingZeroPatch, false), new(nameof(LeadingZeroPreRelease), LeadingZeroPreRelease, false), new(nameof(EmptyPreRelease), EmptyPreRelease, false), new(nameof(EmptyBuildMetadata), EmptyBuildMetadata, false), new(nameof(NonAsciiDigit), NonAsciiDigit, false), new(nameof(EmbeddedSpace), EmbeddedSpace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
