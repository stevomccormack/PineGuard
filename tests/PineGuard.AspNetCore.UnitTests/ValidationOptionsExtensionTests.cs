#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class ValidationOptionsExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ValidationOptionsExtensionTestData.AddMustValidatorResolver.Cases), MemberType = typeof(ValidationOptionsExtensionTestData.AddMustValidatorResolver))]
    public void AddMustValidatorResolver_BehavesAsExpected(ValidationOptionsExtensionTestData.AddMustValidatorResolver.Case tc)
    {
        // Arrange
        var options = tc.Value();

        // Act
        for (var call = 0; call < tc.Expected.Calls; call++)
            Assert.Same(options, options.AddMustValidatorResolver());

        // Assert
        Assert.Equal(tc.Expected.ResolverCount, options.Resolvers.Count);
        Assert.IsType<MustValidatableInfoResolver>(options.Resolvers[tc.Expected.PineGuardIndex]);
    }

    [Theory]
    [MemberData(nameof(ValidationOptionsExtensionTestData.AddMustValidatorResolver.InvalidCases), MemberType = typeof(ValidationOptionsExtensionTestData.AddMustValidatorResolver))]
    public void AddMustValidatorResolver_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
#pragma warning restore ASP0029
#endif
