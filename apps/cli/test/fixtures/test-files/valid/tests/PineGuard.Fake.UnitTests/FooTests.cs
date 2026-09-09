namespace PineGuard.Fake.UnitTests;

public sealed class FooTests
{
    [Theory]
    [MemberData(nameof(FooTestData.ValidCases), MemberType = typeof(FooTestData))]
    public void Foo_BehavesAsExpected(FooTestData.Case tc)
    {
        // Arrange
        var value = tc.Value;

        // Act
        var result = value.Length;

        // Assert
        Assert.Equal(tc.Expected, result);
    }
}
