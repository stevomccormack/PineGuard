using Microsoft.AspNetCore.Http;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class DefaultMustFailureMessageResolverTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DefaultMustFailureMessageResolverTestData.Resolve.Cases), MemberType = typeof(DefaultMustFailureMessageResolverTestData.Resolve))]
    public void Resolve_BehavesAsExpected(DefaultMustFailureMessageResolverTestData.Resolve.Case tc)
    {
        // Arrange
        var resolver = new DefaultMustFailureMessageResolver();

        // Act
        var message = resolver.Resolve(tc.Value, new DefaultHttpContext());

        // Assert
        Assert.Equal(tc.Expected, message);
    }

    [Theory]
    [MemberData(nameof(DefaultMustFailureMessageResolverTestData.Resolve.InvalidCases), MemberType = typeof(DefaultMustFailureMessageResolverTestData.Resolve))]
    public void Resolve_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var resolver = new DefaultMustFailureMessageResolver();
        var failure = ((ValueCase<MustFailure>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => resolver.Resolve(failure, new DefaultHttpContext()));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
