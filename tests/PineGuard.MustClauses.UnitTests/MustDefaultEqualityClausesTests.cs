using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDefaultEqualityClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.DefaultInt32.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.DefaultInt32))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.DefaultInt32.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.DefaultInt32))]
    public void Default_Int32_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.Default(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.DefaultString.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.DefaultString))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.DefaultString.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.DefaultString))]
    public void Default_String_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Default(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotDefaultInt32.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotDefaultInt32))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotDefaultInt32.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotDefaultInt32))]
    public void NotDefault_Int32_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.NotDefault(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotDefaultString.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotDefaultString))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotDefaultString.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotDefaultString))]
    public void NotDefault_String_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.NotDefault(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NullOrDefaultNullableInt32.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NullOrDefaultNullableInt32))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NullOrDefaultNullableInt32.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NullOrDefaultNullableInt32))]
    public void NullOrDefault_NullableInt32_BehavesAsExpected(MustCase<int?> tc)
    {
        // Act
        var result = Must.Be.NullOrDefault(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotNullOrDefaultNullableInt32.ValidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotNullOrDefaultNullableInt32))]
    [MemberData(nameof(MustDefaultEqualityClausesTestData.NotNullOrDefaultNullableInt32.InvalidCases), MemberType = typeof(MustDefaultEqualityClausesTestData.NotNullOrDefaultNullableInt32))]
    public void NotNullOrDefault_NullableInt32_BehavesAsExpected(MustCase<int?> tc)
    {
        // Act
        var result = Must.Be.NotNullOrDefault(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
