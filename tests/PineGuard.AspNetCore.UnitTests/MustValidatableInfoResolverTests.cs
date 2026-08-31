#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Validation;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidatableInfoResolverTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.TryGetValidatableTypeInfo.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.TryGetValidatableTypeInfo))]
    public void TryGetValidatableTypeInfo_BehavesAsExpected(MustValidatableInfoResolverTestData.TryGetValidatableTypeInfo.Case tc)
    {
        // Arrange
        var resolver = new MustValidatableInfoResolver();

        // Act
        var claimed = resolver.TryGetValidatableTypeInfo(tc.Value, out var info);

        // Assert
        Assert.Equal(tc.Expected.IsValid, claimed);
        Assert.Equal(tc.Expected.IsValid, info is not null);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.TryGetValidatableTypeInfo.InvalidCases), MemberType = typeof(MustValidatableInfoResolverTestData.TryGetValidatableTypeInfo))]
    public void TryGetValidatableTypeInfo_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.TryGetValidatableParameterInfo.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.TryGetValidatableParameterInfo))]
    public void TryGetValidatableParameterInfo_BehavesAsExpected(MustValidatableInfoResolverTestData.TryGetValidatableParameterInfo.Case tc)
    {
        // Arrange
        var resolver = new MustValidatableInfoResolver();

        // Act
        var claimed = resolver.TryGetValidatableParameterInfo(tc.Value, out var info);

        // Assert
        Assert.Equal(tc.Expected.IsValid, claimed);
        Assert.Null(info);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.ValidateAsync.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.ValidateAsync))]
    public async Task ValidateAsync_BehavesAsExpected(MustValidatableInfoResolverTestData.ValidateAsync.Case tc)
    {
        // Arrange
        var (value, validatedType, configureServices, currentValidationPath, validationOptions) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);

        Assert.True(new MustValidatableInfoResolver().TryGetValidatableTypeInfo(validatedType, out var info));

        var context = new ValidateContext
        {
            ValidationContext = new ValidationContext(value ?? new object(), provider, items: null),
            ValidationOptions = validationOptions()!,
            CurrentValidationPath = currentValidationPath
        };

        // Act
        await info.ValidateAsync(value, context, CancellationToken.None);

        // Assert
        var errors = context.ValidationErrors;

        Assert.Equal(tc.Expected.IsValid, errors is null || errors.Count == 0);

        if (tc.Expected.Keys is not null)
            Assert.Equal(tc.Expected.Keys, errors!.Keys);

        if (tc.Expected.Messages is not null)
            Assert.Equal(tc.Expected.Messages, errors!.Values.SelectMany(messages => messages));
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.ValidateAsync.InvalidCases), MemberType = typeof(MustValidatableInfoResolverTestData.ValidateAsync))]
    public async Task ValidateAsync_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Func<Task>>)tc).Value;

        // Act & Assert
        var ex = await Assert.ThrowsAsync(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.EndToEnd.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.EndToEnd))]
    public async Task EndToEnd_BehavesAsExpected(MustValidatableInfoResolverTestData.EndToEnd.Case tc)
    {
        // Arrange
        var (requestUri, json) = tc.Value;
        await using var app = await SampleHost.StartAsync(SampleBuiltInValidationApi.ConfigureServices, SampleBuiltInValidationApi.Map);
        using var client = app.GetTestClient();
        using var request = SampleHost.Request(HttpMethod.Post, requestUri, json);

        // Act
        using var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(tc.Expected.IsValid, response.IsSuccessStatusCode);
        Assert.Equal(tc.Expected.Status, (int)response.StatusCode);

        var body = await SampleResponses.ReadJsonAsync(response);

        if (tc.Expected.Echo is not null)
            Assert.Equal(tc.Expected.Echo, body.GetString());

        if (tc.Expected.Body is not { } problem)
            return;

        var errors = body.GetProperty("errors");

        Assert.Equal(problem.Title, body.GetProperty("title").GetString());
        Assert.Equal(problem.ErrorKeys, errors.EnumerateObject().Select(error => error.Name));
        Assert.Equal(problem.Messages, errors.EnumerateObject().SelectMany(error => error.Value.EnumerateArray().Select(message => message.GetString())));
        Assert.False(body.TryGetProperty(ProblemDetailsExtension.FailuresExtensionKey, out _));
    }
}
#pragma warning restore ASP0029
#endif
