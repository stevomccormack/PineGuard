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

#if NET11_0_OR_GREATER
    /// <summary>
    /// The one property name this resolver's info answers <c>TryFindProperty</c> for, so a test can prove
    /// the lookup was delegated rather than invented.
    /// </summary>
    public const string KnownPropertyName = "SampleProperty";

    public bool TryGetValidatableTypeInfo(Type type, [NotNullWhen(true)] out IValidatableTypeInfo? validatableTypeInfo)
    {
        validatableTypeInfo = errorKey is null ? null : new SampleValidatableInfo(errorKey);

        return validatableTypeInfo is not null;
    }

    public bool TryGetValidatableParameterInfo(ParameterInfo parameterInfo, [NotNullWhen(true)] out IValidatableParameterInfo? validatableParameterInfo)
    {
        validatableParameterInfo = null;

        return false;
    }

    private sealed class SampleValidatableInfo(string errorKey) : IValidatableTypeInfo
    {
        public Task ValidateAsync(object? value, ValidateContext context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            context.AddValidationError(new ValidationError
            {
                Name = errorKey,
                Path = errorKey,
                ErrorMessage = Message,
                Container = value
            });

            return Task.CompletedTask;
        }

        public void Validate(object? value, ValidateContext context) =>
            ValidateAsync(value, context, CancellationToken.None).GetAwaiter().GetResult();

        public bool TryFindProperty(string propertyName, ValidationOptions validationOptions, [NotNullWhen(true)] out IValidatablePropertyInfo? validatablePropertyInfo)
        {
            validatablePropertyInfo = string.Equals(propertyName, KnownPropertyName, StringComparison.Ordinal)
                ? new SampleValidatablePropertyInfo()
                : null;

            return validatablePropertyInfo is not null;
        }
    }

    private sealed class SampleValidatablePropertyInfo : IValidatablePropertyInfo
    {
        public void Validate(object containingObject, ValidateContext context)
        {
        }

        public Task ValidateAsync(object containingObject, ValidateContext context, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
#else
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
#endif
}
#pragma warning restore ASP0029
#endif
