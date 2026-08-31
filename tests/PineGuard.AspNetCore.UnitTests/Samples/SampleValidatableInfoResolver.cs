#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Validation;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Stands in for whatever resolver sits behind PineGuard's in <see cref="ValidationOptions.Resolvers"/> —
/// the source generator's, or the data-annotation walk — so a test can prove PineGuard only ever adds
/// validation and never replaces it.
/// </summary>
/// <param name="errorKey">The key this resolver records a failure under, or <see langword="null"/> to decline every type.</param>
public sealed class SampleValidatableInfoResolver(string? errorKey) : IValidatableInfoResolver
{
    /// <summary>
    /// The message a claiming resolver records, so a test can tell its failure from PineGuard's.
    /// </summary>
    public const string Message = "The next resolver in the chain also ran.";

    public bool TryGetValidatableTypeInfo(Type type, [NotNullWhen(true)] out IValidatableInfo? validatableInfo)
    {
        validatableInfo = errorKey is null ? null : new SampleValidatableInfo(errorKey);

        return validatableInfo is not null;
    }

    public bool TryGetValidatableParameterInfo(ParameterInfo parameterInfo, [NotNullWhen(true)] out IValidatableInfo? validatableInfo)
    {
        validatableInfo = null;

        return false;
    }

    private sealed class SampleValidatableInfo(string errorKey) : IValidatableInfo
    {
        public Task ValidateAsync(object? value, ValidateContext context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var errors = context.ValidationErrors ??= [];
            errors[errorKey] = [Message];

            return Task.CompletedTask;
        }
    }
}
#pragma warning restore ASP0029
#endif
