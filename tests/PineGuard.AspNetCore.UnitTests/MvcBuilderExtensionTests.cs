using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MvcBuilderExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MvcBuilderExtensionTestData.AddMustValidation.Cases), MemberType = typeof(MvcBuilderExtensionTestData.AddMustValidation))]
    public void AddMustValidation_BehavesAsExpected(MvcBuilderExtensionTestData.AddMustValidation.Case tc)
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = new SampleMvcBuilder(services);

        // Act
        for (var call = 0; call < tc.Value; call++)
            Assert.Same(builder, builder.AddMustValidation());

        // Assert
        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<MvcOptions>>().Value;

        Assert.Equal(tc.Expected.FilterCount, options.Filters.OfType<TypeFilterAttribute>().Count(filter => filter.ImplementationType == typeof(MustValidationActionFilter)));
    }

    [Theory]
    [MemberData(nameof(MvcBuilderExtensionTestData.AddMustValidation.InvalidCases), MemberType = typeof(MvcBuilderExtensionTestData.AddMustValidation))]
    public void AddMustValidation_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
