#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
#if !NET11_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif
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

#if NET11_0_OR_GREATER
        var context = BuildContext(provider, validationOptions()!, currentValidationPath);
#else
        var context = BuildContext(value, provider, validationOptions()!, currentValidationPath);
#endif

        // Act
        await info.ValidateAsync(value, context, CancellationToken.None);

        // Assert
        AssertExpectedErrors(context, tc.Expected);
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

#if NET11_0_OR_GREATER
    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.Validate.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.Validate))]
    public async Task Validate_BehavesAsExpected(MustValidatableInfoResolverTestData.ValidateAsync.Case tc)
    {
        // Arrange
        var (value, validatedType, configureServices, currentValidationPath, validationOptions) = tc.Value;
        await using var provider = SampleServices.Build(configureServices);

        Assert.True(new MustValidatableInfoResolver().TryGetValidatableTypeInfo(validatedType, out var info));

        var context = BuildContext(provider, validationOptions()!, currentValidationPath);

        // Act
        info.Validate(value, context);

        // Assert
        AssertExpectedErrors(context, tc.Expected);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.Validate.InvalidCases), MemberType = typeof(MustValidatableInfoResolverTestData.Validate))]
    public void Validate_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.ValidateAsyncWithoutServices.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.ValidateAsyncWithoutServices))]
    public async Task ValidateAsync_WithoutServices_BehavesAsExpected(MustValidatableInfoResolverTestData.ValidateAsync.Case tc)
    {
        // Arrange
        var (value, validatedType, _, currentValidationPath, validationOptions) = tc.Value;

        Assert.True(new MustValidatableInfoResolver().TryGetValidatableTypeInfo(validatedType, out var info));

        var context = new ValidateContext
        {
            ValidationOptions = validationOptions()!,
            CurrentValidationPath = currentValidationPath
        };

        // Act
        await info.ValidateAsync(value, context, CancellationToken.None);

        // Assert
        AssertExpectedErrors(context, tc.Expected);
    }

    [Theory]
    [MemberData(nameof(MustValidatableInfoResolverTestData.TryFindProperty.Cases), MemberType = typeof(MustValidatableInfoResolverTestData.TryFindProperty))]
    public void TryFindProperty_BehavesAsExpected(MustValidatableInfoResolverTestData.TryFindProperty.Case tc)
    {
        // Arrange
        var (validationOptions, propertyName) = tc.Value;

        Assert.True(new MustValidatableInfoResolver().TryGetValidatableTypeInfo(typeof(CreateOrder), out var info));

        // Act
        var found = info.TryFindProperty(propertyName, validationOptions()!, out var propertyInfo);

        // Assert
        Assert.Equal(tc.Expected.IsValid, found);
        Assert.Equal(tc.Expected.IsValid, propertyInfo is not null);
    }
#endif

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

#if NET11_0_OR_GREATER
    private static ValidateContext BuildContext(IServiceProvider provider, ValidationOptions options, string currentValidationPath) =>
        new()
        {
            ServiceProvider = provider,
            ValidationOptions = options,
            CurrentValidationPath = currentValidationPath
        };
#else
    private static ValidateContext BuildContext(object? value, IServiceProvider provider, ValidationOptions options, string currentValidationPath) =>
        new()
        {
            ValidationContext = new ValidationContext(value ?? new object(), provider, items: null),
            ValidationOptions = options,
            CurrentValidationPath = currentValidationPath
        };
#endif

    private static void AssertExpectedErrors(ValidateContext context, MustValidatableInfoResolverTestData.ValidateAsync.ValidationErrorsExpected expected)
    {
        var errors = context.ValidationErrors;

        Assert.Equal(expected.IsValid, errors is null || errors.Count == 0);

        if (expected.Keys is not null)
            Assert.Equal(expected.Keys, errors!.Keys);

        if (expected.Messages is not null)
#if NET11_0_OR_GREATER
            Assert.Equal(expected.Messages, errors!.Values.SelectMany(list => list).Select(error => error.ErrorMessage));
#else
            Assert.Equal(expected.Messages, errors!.Values.SelectMany(messages => messages));
#endif
    }
}
#pragma warning restore ASP0029
#endif
