#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using System.ComponentModel.DataAnnotations;
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
}
#pragma warning restore ASP0029
#endif
