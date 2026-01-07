using PineGuard.Externals.Iso.Payments.Cards;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public sealed class CardBrandOverrideTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CardBrandOverrideTestData.MastercardMatchesPan.ValidCases), MemberType = typeof(CardBrandOverrideTestData.MastercardMatchesPan))]
    [MemberData(nameof(CardBrandOverrideTestData.MastercardMatchesPan.EdgeCases), MemberType = typeof(CardBrandOverrideTestData.MastercardMatchesPan))]
    public void MastercardCard_MatchesPan_ReturnsExpected(CardBrandOverrideTestData.MastercardMatchesPan.ValidCase testCase)
    {
        var card = new MastercardCard();

        var result = card.MatchesPan(testCase.Pan);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(CardBrandOverrideTestData.JcbMatchesPan.ValidCases), MemberType = typeof(CardBrandOverrideTestData.JcbMatchesPan))]
    [MemberData(nameof(CardBrandOverrideTestData.JcbMatchesPan.EdgeCases), MemberType = typeof(CardBrandOverrideTestData.JcbMatchesPan))]
    public void JcbCard_MatchesPan_ReturnsExpected(CardBrandOverrideTestData.JcbMatchesPan.ValidCase testCase)
    {
        var card = new JcbCard();

        var result = card.MatchesPan(testCase.Pan);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(CardBrandOverrideTestData.DiscoverMatchesPan.ValidCases), MemberType = typeof(CardBrandOverrideTestData.DiscoverMatchesPan))]
    [MemberData(nameof(CardBrandOverrideTestData.DiscoverMatchesPan.EdgeCases), MemberType = typeof(CardBrandOverrideTestData.DiscoverMatchesPan))]
    public void DiscoverCard_MatchesPan_ReturnsExpected(CardBrandOverrideTestData.DiscoverMatchesPan.ValidCase testCase)
    {
        var card = new DiscoverCard();

        var result = card.MatchesPan(testCase.Pan);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(CardBrandOverrideTestData.DinersClubMatchesPan.ValidCases), MemberType = typeof(CardBrandOverrideTestData.DinersClubMatchesPan))]
    [MemberData(nameof(CardBrandOverrideTestData.DinersClubMatchesPan.EdgeCases), MemberType = typeof(CardBrandOverrideTestData.DinersClubMatchesPan))]
    public void DinersClubCard_MatchesPan_ReturnsExpected(CardBrandOverrideTestData.DinersClubMatchesPan.ValidCase testCase)
    {
        var card = new DinersClubCard();

        var result = card.MatchesPan(testCase.Pan);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(CardBrandOverrideTestData.VisaMatchesPan.EdgeCases), MemberType = typeof(CardBrandOverrideTestData.VisaMatchesPan))]
    public void VisaCard_MatchesPan_ReturnsExpected(CardBrandOverrideTestData.VisaMatchesPan.ValidCase testCase)
    {
        var card = new VisaCard();

        var result = card.MatchesPan(testCase.Pan);

        Assert.Equal(testCase.Expected, result);
    }
}
