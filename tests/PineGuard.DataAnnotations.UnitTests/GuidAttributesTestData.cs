using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class GuidAttributesTestData
{
    public static class NotEmptyGuid
    {
        public static TheoryData<DataAnnotationCase> Cases
        {
            get
            {
                var td = F.NotEmpty.AllScenarios.ToDataAnnotationCases(s => s.IsValid
                    ? new DataAnnotationExpected(true)
                    : new DataAnnotationExpected(false, "Value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty));
                td.Add(new DataAnnotationCase("null-value", null, new DataAnnotationExpected(true)));
                return td;
            }
        }
    }
}
