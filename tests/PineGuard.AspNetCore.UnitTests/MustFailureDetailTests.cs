using System.Text.Json;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustFailureDetailTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private static readonly JsonSerializerOptions WebOptions = new(JsonSerializerDefaults.Web);

    [Theory]
    [MemberData(nameof(MustFailureDetailTestData.Serialization.Cases), MemberType = typeof(MustFailureDetailTestData.Serialization))]
    public void Serialization_BehavesAsExpected(MustFailureDetailTestData.Serialization.Case tc)
    {
        // Arrange
        var detail = tc.Value;

        // Act
        var json = JsonSerializer.Serialize(detail, WebOptions);

        // Assert
        Assert.Equal(tc.Expected, json);
        Assert.DoesNotContain(SampleFailures.SecretValue, json, StringComparison.Ordinal);
    }
}
