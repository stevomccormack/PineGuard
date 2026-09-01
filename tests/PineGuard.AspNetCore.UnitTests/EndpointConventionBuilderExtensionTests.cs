using Microsoft.AspNetCore.Http;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class EndpointConventionBuilderExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private static ValueTask<object?> Next(EndpointFilterInvocationContext context) => ValueTask.FromResult<object?>(null);

    [Theory]
    [MemberData(nameof(EndpointConventionBuilderExtensionTestData.CreateFilter.Cases), MemberType = typeof(EndpointConventionBuilderExtensionTestData.CreateFilter))]
    public void CreateFilter_BehavesAsExpected(EndpointConventionBuilderExtensionTestData.CreateFilter.Case tc)
    {
        // Arrange
        var (methodInfo, configureServices) = tc.Value;
        using var provider = SampleServices.Build(configureServices);
        var context = new EndpointFilterFactoryContext { MethodInfo = methodInfo, ApplicationServices = provider };
        EndpointFilterDelegate next = Next;

        // Act
        var filter = EndpointConventionBuilderExtension.CreateFilter(context, next);

        // Assert
        Assert.Equal(tc.Expected.IsValid, !ReferenceEquals(filter, next));
    }

    [Theory]
    [MemberData(nameof(EndpointConventionBuilderExtensionTestData.CreateFilterWithoutServiceProbe.Cases), MemberType = typeof(EndpointConventionBuilderExtensionTestData.CreateFilterWithoutServiceProbe))]
    public void CreateFilterWithoutServiceProbe_BehavesAsExpected(EndpointConventionBuilderExtensionTestData.CreateFilterWithoutServiceProbe.Case tc)
    {
        // Arrange
        var context = new EndpointFilterFactoryContext { MethodInfo = tc.Value, ApplicationServices = new EmptyServiceProvider() };
        EndpointFilterDelegate next = Next;

        // Act
        var filter = EndpointConventionBuilderExtension.CreateFilter(context, next);

        // Assert
        Assert.Equal(tc.Expected.IsValid, !ReferenceEquals(filter, next));
    }

    [Theory]
    [MemberData(nameof(EndpointConventionBuilderExtensionTestData.AddMustValidation.Cases), MemberType = typeof(EndpointConventionBuilderExtensionTestData.AddMustValidation))]
    public void AddMustValidation_BehavesAsExpected(EndpointConventionBuilderExtensionTestData.AddMustValidation.Case tc)
    {
        // Arrange
        var builder = new SampleEndpointConventionBuilder();

        // Act
        for (var call = 0; call < tc.Value; call++)
            Assert.Same(builder, builder.AddMustValidation());

        // Assert
        Assert.Equal(tc.Expected.FilterFactoryCount, builder.Build().FilterFactories.Count);
    }

    [Theory]
    [MemberData(nameof(EndpointConventionBuilderExtensionTestData.AddMustValidation.InvalidCases), MemberType = typeof(EndpointConventionBuilderExtensionTestData.AddMustValidation))]
    public void AddMustValidation_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
