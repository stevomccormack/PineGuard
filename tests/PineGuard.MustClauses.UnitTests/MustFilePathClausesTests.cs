using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustFilePathClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustFilePathClausesTestData.SafeFileName.ValidCases), MemberType = typeof(MustFilePathClausesTestData.SafeFileName))]
    public void SafeFileName_Checks(MustFilePathClausesTestData.SafeFileName.ValidCase testCase)
    {
        var result = Must.Be.SafeFileName(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustFilePathClausesTestData.HasFileExtension.ValidCases), MemberType = typeof(MustFilePathClausesTestData.HasFileExtension))]
    public void HasFileExtension_Checks(MustFilePathClausesTestData.HasFileExtension.ValidCase testCase)
    {
        var result = Must.Be.HasFileExtension(testCase.Value.value, testCase.Value.extensions);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustFilePathClausesTestData.SafeFileName.EdgeCases), MemberType = typeof(MustFilePathClausesTestData.SafeFileName))]
    public void SafeFileName_EdgeChecks(MustFilePathClausesTestData.SafeFileName.EdgeCase testCase)
    {
        var result = Must.Be.SafeFileName(testCase.Value, paramName: "Input");
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustFilePathClausesTestData.HasFileExtension.EdgeCases), MemberType = typeof(MustFilePathClausesTestData.HasFileExtension))]
    public void HasFileExtension_EdgeChecks(MustFilePathClausesTestData.HasFileExtension.EdgeCase testCase)
    {
        var result = Must.Be.HasFileExtension(testCase.Value.value, testCase.Value.extensions, paramName: "Input");
        Assert.Equal(testCase.Expected, result.Success);
    }
}
