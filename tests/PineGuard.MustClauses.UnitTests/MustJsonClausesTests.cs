using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustJsonClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustJsonClausesTestData.Json.ValidCases), MemberType = typeof(MustJsonClausesTestData.Json))]
    [MemberData(nameof(MustJsonClausesTestData.Json.InvalidCases), MemberType = typeof(MustJsonClausesTestData.Json))]
    public void Json_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Json(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustJsonClausesTestData.JsonObject.ValidCases), MemberType = typeof(MustJsonClausesTestData.JsonObject))]
    [MemberData(nameof(MustJsonClausesTestData.JsonObject.InvalidCases), MemberType = typeof(MustJsonClausesTestData.JsonObject))]
    public void JsonObject_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.JsonObject(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustJsonClausesTestData.JsonArray.ValidCases), MemberType = typeof(MustJsonClausesTestData.JsonArray))]
    [MemberData(nameof(MustJsonClausesTestData.JsonArray.InvalidCases), MemberType = typeof(MustJsonClausesTestData.JsonArray))]
    public void JsonArray_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.JsonArray(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustJsonClausesTestData.JsonContentType.ValidCases), MemberType = typeof(MustJsonClausesTestData.JsonContentType))]
    [MemberData(nameof(MustJsonClausesTestData.JsonContentType.InvalidCases), MemberType = typeof(MustJsonClausesTestData.JsonContentType))]
    public void JsonContentType_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.JsonContentType(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
