using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardJsonClausesTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardJsonClausesTestData.NotJson.ValidCases), MemberType = typeof(GuardJsonClausesTestData.NotJson))]
    [MemberData(nameof(GuardJsonClausesTestData.NotJson.InvalidCases), MemberType = typeof(GuardJsonClausesTestData.NotJson))]
    public void NotJson_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotJson(tc.Value, paramName: "value"));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonObject.ValidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonObject))]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonObject.InvalidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonObject))]
    public void NotJsonObject_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotJsonObject(tc.Value, paramName: "value"));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonArray.ValidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonArray))]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonArray.InvalidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonArray))]
    public void NotJsonArray_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotJsonArray(tc.Value, paramName: "value"));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonContentType.ValidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonContentType))]
    [MemberData(nameof(GuardJsonClausesTestData.NotJsonContentType.InvalidCases), MemberType = typeof(GuardJsonClausesTestData.NotJsonContentType))]
    public void NotJsonContentType_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotJsonContentType(tc.Value, paramName: "value"));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }
}
