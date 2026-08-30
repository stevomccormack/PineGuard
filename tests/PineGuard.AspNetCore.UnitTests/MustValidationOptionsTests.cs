using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationOptionsTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationOptionsTestData.Defaults.Cases), MemberType = typeof(MustValidationOptionsTestData.Defaults))]
    public void Defaults_BehaveAsExpected(MustValidationOptionsTestData.Defaults.Case tc) => AssertOptions(tc);

    [Theory]
    [MemberData(nameof(MustValidationOptionsTestData.Configuration.Cases), MemberType = typeof(MustValidationOptionsTestData.Configuration))]
    public void Configuration_BehavesAsExpected(MustValidationOptionsTestData.Defaults.Case tc) => AssertOptions(tc);

    private static void AssertOptions(MustValidationOptionsTestData.Defaults.Case tc)
    {
        // Arrange
        var options = new MustValidationOptions();

        // Act
        tc.Value(options);

        // Assert
        Assert.Equal(tc.Expected.PropertyNamingPolicy, options.PropertyNamingPolicy);
        Assert.Equal(tc.Expected.UseJsonNamingPolicy, options.UseJsonNamingPolicy);
        Assert.Equal(tc.Expected.IncludeCodes, options.IncludeCodes);
        Assert.Equal(tc.Expected.Mode, options.Mode);
        Assert.Equal(tc.Expected.HandleGuardExceptions, options.HandleGuardExceptions);
        Assert.Equal(tc.Expected.UnknownGuardCode, options.UnknownGuardCode);
        Assert.Equal(tc.Expected.Title, options.Title);
        Assert.Equal(tc.Expected.LocalizationResourceType, options.LocalizationResourceType);
    }
}
