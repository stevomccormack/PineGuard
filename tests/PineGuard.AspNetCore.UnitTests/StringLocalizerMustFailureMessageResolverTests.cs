using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class StringLocalizerMustFailureMessageResolverTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringLocalizerMustFailureMessageResolverTestData.Resolve.Cases), MemberType = typeof(StringLocalizerMustFailureMessageResolverTestData.Resolve))]
    public void Resolve_BehavesAsExpected(StringLocalizerMustFailureMessageResolverTestData.Resolve.Case tc)
    {
        // Arrange
        var (failure, hasLocalizerFactory, localizationResourceType) = tc.Value;
        var factory = hasLocalizerFactory ? new FakeStringLocalizerFactory(StringLocalizerMustFailureMessageResolverTestData.FrenchResources) : null;
        var options = Options.Create(new MustValidationOptions { LocalizationResourceType = localizationResourceType });
        var resolver = new StringLocalizerMustFailureMessageResolver(options, factory);

        // Act
        var message = resolver.Resolve(failure, new DefaultHttpContext());

        // Assert
        Assert.Equal(tc.Expected.Message, message);
        Assert.Equal(tc.Expected.IsValid, !string.Equals(message, failure.Message, StringComparison.Ordinal));
        Assert.Equal(tc.Expected.RequestedResourceSource, factory?.RequestedResourceSource);
    }

    [Theory]
    [MemberData(nameof(StringLocalizerMustFailureMessageResolverTestData.Resolve.InvalidCases), MemberType = typeof(StringLocalizerMustFailureMessageResolverTestData.Resolve))]
    public void Resolve_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var failure = ((ValueCase<MustFailure>)tc).Value;
        var resolver = new StringLocalizerMustFailureMessageResolver(Options.Create(new MustValidationOptions()));

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => resolver.Resolve(failure, new DefaultHttpContext()));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
