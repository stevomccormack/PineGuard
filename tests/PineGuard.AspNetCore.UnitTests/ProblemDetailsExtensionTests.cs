using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class ProblemDetailsExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private const string SampleIdentifier = "SkuCode";

    [Theory]
    [MemberData(nameof(ProblemDetailsExtensionTestData.ToValidationProblemDetails.Cases), MemberType = typeof(ProblemDetailsExtensionTestData.ToValidationProblemDetails))]
    public void ToValidationProblemDetails_BehavesAsExpected(ProblemDetailsExtensionTestData.ToValidationProblemDetails.Case tc)
    {
        // Arrange
        var (result, options, namingPolicy) = tc.Value;

        // Act
        var problemDetails = result.ToValidationProblemDetails(options, namingPolicy, new DefaultMustFailureMessageResolver(), new DefaultHttpContext());

        // Assert
        ProblemDetailsAssert.Expected(tc.Expected, problemDetails);
    }

    [Theory]
    [MemberData(nameof(ProblemDetailsExtensionTestData.ToValidationProblemDetails.InvalidCases), MemberType = typeof(ProblemDetailsExtensionTestData.ToValidationProblemDetails))]
    public void ToValidationProblemDetails_ThrowsAsExpected(IThrowsCase tc) => AssertThrows(tc);

    [Theory]
    [MemberData(nameof(ProblemDetailsExtensionTestData.ToValidationProblemDetailsFromServices.Cases), MemberType = typeof(ProblemDetailsExtensionTestData.ToValidationProblemDetailsFromServices))]
    public void ToValidationProblemDetailsFromServices_BehavesAsExpected(ProblemDetailsExtensionTestData.ToValidationProblemDetailsFromServices.Case tc)
    {
        // Arrange
        var (result, configureServices) = tc.Value;
        using var provider = BuildProvider(configureServices);
        var httpContext = new DefaultHttpContext { RequestServices = provider };

        // Act
        var problemDetails = result.ToValidationProblemDetails(httpContext);

        // Assert
        ProblemDetailsAssert.Expected(tc.Expected, problemDetails);
    }

    [Theory]
    [MemberData(nameof(ProblemDetailsExtensionTestData.ToValidationProblemDetailsFromServices.InvalidCases), MemberType = typeof(ProblemDetailsExtensionTestData.ToValidationProblemDetailsFromServices))]
    public void ToValidationProblemDetailsFromServices_ThrowsAsExpected(IThrowsCase tc) => AssertThrows(tc);

    [Theory]
    [MemberData(nameof(ProblemDetailsExtensionTestData.ResolveNamingPolicy.Cases), MemberType = typeof(ProblemDetailsExtensionTestData.ResolveNamingPolicy))]
    public void ResolveNamingPolicy_BehavesAsExpected(ProblemDetailsExtensionTestData.ResolveNamingPolicy.Case tc)
    {
        // Arrange
        var (options, configureServices) = tc.Value;
        using var provider = BuildProvider(configureServices);
        var httpContext = new DefaultHttpContext { RequestServices = provider };

        // Act
        var namingPolicy = ProblemDetailsExtension.ResolveNamingPolicy(httpContext, options);

        // Assert
        Assert.Equal(tc.Expected.IsValid, namingPolicy is not null);
        Assert.Equal(tc.Expected.ConvertedName, namingPolicy?.ConvertName(SampleIdentifier));
    }

    private static ServiceProvider BuildProvider(Action<IServiceCollection> configureServices)
    {
        var services = new ServiceCollection();
        configureServices(services);
        return services.BuildServiceProvider();
    }

    private static void AssertThrows(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
