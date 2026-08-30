using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.Core.UnitTests.Utils;

public static class TokenUtilityTestData
{
    public static class TryParseJwt
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsJwt.Canonical), F.IsJwt.Canonical, true, F.IsJwt.HeaderSegment, F.IsJwt.PayloadSegment, F.IsJwt.SignatureSegment),
            new(nameof(F.IsJwt.MinimalSegments), F.IsJwt.MinimalSegments, true, F.IsJwt.JsonObjectSegment, F.IsJwt.JsonObjectSegment, F.IsJwt.PlainTextSegment),
            new(nameof(F.IsJwt.UrlSafeSignature), F.IsJwt.UrlSafeSignature, true, F.IsJwt.HeaderSegment, F.IsJwt.PayloadSegment, F.IsJwt.UrlSafeSegment)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.IsJwt.Padded), F.IsJwt.Padded, true, F.IsJwt.HeaderSegment, F.IsJwt.PayloadSegment, F.IsJwt.SignatureSegment),
            new(nameof(F.IsJwt.NullValue), F.IsJwt.NullValue, false, string.Empty, string.Empty, string.Empty),
            new(nameof(F.IsJwt.TwoSegments), F.IsJwt.TwoSegments, false, string.Empty, string.Empty, string.Empty),
            new(nameof(F.IsJwt.PayloadNotJson), F.IsJwt.PayloadNotJson, false, string.Empty, string.Empty, string.Empty)
        ];

        public sealed record ValidCase : ReturnCase<string?, (bool ok, string header, string payload, string signature)>
        {
            public ValidCase(string name, string? value, bool expectedOk, string expectedHeader, string expectedPayload, string expectedSignature)
                : base(name, value, (expectedOk, expectedHeader, expectedPayload, expectedSignature)) { }
        }
    }
}
