using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.AspNetCore.UnitTests;

public static class MustFailureDetailTestData
{
    public static class Serialization
    {
        public static TheoryData<Case> Cases =>
        [
            new("property-path-is-published-as-property", Detail(SampleFailures.Email), """{"property":"Email","code":"email.address.invalid","message":"Email must be a valid email address."}"""),
            new("indexed-path-survives-serialization", Detail(SampleFailures.LineSku), """{"property":"Lines[1].Sku","code":"text.content.blank","message":"Lines[1].Sku must not be null or whitespace."}"""),
            new("root-path-is-published-as-an-empty-property", Detail(SampleFailures.Root), """{"property":"","code":"value.state.invalid","message":"The order is not consistent."}"""),
            new("a-secret-attempted-value-never-reaches-the-wire", Detail(SampleFailures.Password), """{"property":"Password","code":"text.content.blank","message":"Password must not be null or whitespace."}""")
        ];

        private static MustFailureDetail Detail(MustFailure failure) => new(failure.PropertyPath, failure.Code, failure.Message);

        public sealed record Case(string Name, MustFailureDetail Value, string Expected)
            : ReturnCase<MustFailureDetail, string>(Name, Value, Expected);
    }
}
