using PineGuard.Iso.Payments.Cards;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public sealed class IsoPaymentCardBrandTests : BaseUnitTest
{
    private sealed class TestBrand : IsoPaymentCardBrand
    {
        public TestBrand(string brandName, int[] validPanLengths, string[] iinPrefixes, int[] displayFormatPattern)
            : base(brandName, validPanLengths, iinPrefixes, displayFormatPattern)
        {
        }

        public bool CallMatchesIinRange(string sanitizedPan, int rangeStart, int rangeEnd) =>
            MatchesIinRange(sanitizedPan, rangeStart, rangeEnd);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardBrandTestData.Constructor.InvalidCases), MemberType = typeof(IsoPaymentCardBrandTestData.Constructor))]
    public void Constructor_Throws_ForInvalidInputs(IsoPaymentCardBrandTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => new TestBrand(testCase.BrandName!, testCase.ValidPanLengths!, testCase.IinPrefixes!, testCase.DisplayFormatPattern!));

        // Assert
        Assert.NotNull(ex);
        Assert.IsType(testCase.ExpectedException.Type, ex);

        if (testCase.ExpectedException.ParamName is not null)
        {
            var actualParam = ex switch
            {
                ArgumentNullException ane => ane.ParamName,
                ArgumentException ae => ae.ParamName,
                _ => null
            };

            Assert.Equal(testCase.ExpectedException.ParamName, actualParam);
        }
    }

    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange
        var validPanLengths = new[] { 16 };
        var iinPrefixes = new[] { "4" };
        var displayFormatPattern = new[] { 4, 4, 4, 4 };

        // Act
        var brand = new TestBrand("Test", validPanLengths, iinPrefixes, displayFormatPattern);

        // Assert
        Assert.Equal("Test", brand.BrandName);
        Assert.Same(validPanLengths, brand.ValidPanLengths);
        Assert.Same(iinPrefixes, brand.IinPrefixes);
        Assert.Same(displayFormatPattern, brand.DisplayFormatPattern);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardBrandTestData.IsoCardNumberWithSeparatorsRegex.ValidCases), MemberType = typeof(IsoPaymentCardBrandTestData.IsoCardNumberWithSeparatorsRegex))]
    [MemberData(nameof(IsoPaymentCardBrandTestData.IsoCardNumberWithSeparatorsRegex.EdgeCases), MemberType = typeof(IsoPaymentCardBrandTestData.IsoCardNumberWithSeparatorsRegex))]
    public void IsoCardNumberWithSeparatorsRegex_ReturnsExpected(IsoPaymentCardBrandTestData.IsoCardNumberWithSeparatorsRegex.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = IsoPaymentCardBrand.IsoCardNumberWithSeparatorsRegex().IsMatch(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardBrandTestData.MatchesIinRange.ValidCases), MemberType = typeof(IsoPaymentCardBrandTestData.MatchesIinRange))]
    [MemberData(nameof(IsoPaymentCardBrandTestData.MatchesIinRange.EdgeCases), MemberType = typeof(IsoPaymentCardBrandTestData.MatchesIinRange))]
    public void MatchesIinRange_ReturnsExpected(IsoPaymentCardBrandTestData.MatchesIinRange.ValidCase testCase)
    {
        // Arrange
        var brand = new TestBrand("Test", [16], ["5"], [4, 4, 4, 4]);

        // Act
        var pan = testCase.SanitizedPan ?? string.Empty;
        var result = brand.CallMatchesIinRange(pan, 51, 55)
            || brand.CallMatchesIinRange(pan, 2221, 2720);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void MatchesIinRange_CoversGuardBranches()
    {
        // Arrange
        var brand = new TestBrand("Test", [16], ["5"], [4, 4, 4, 4]);

        // Act / Assert
        Assert.False(brand.CallMatchesIinRange("123", 51, 55)); // length < 4
        Assert.False(brand.CallMatchesIinRange("1234", 12345, 12346)); // prefixLength > pan length
        Assert.False(brand.CallMatchesIinRange("ABCD", 12, 34)); // parse fails
    }
}
