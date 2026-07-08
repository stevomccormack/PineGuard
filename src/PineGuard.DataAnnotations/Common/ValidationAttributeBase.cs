using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations.Common;

/// <summary>
/// Provides the base class for PineGuard validation attributes that target a single expected CLR type.
/// </summary>
/// <remarks>
/// <para>
/// Subclasses specify the expected <see cref="Type"/> via the constructor. The base class handles
/// <see langword="null"/> values (optionally skipping validation) and type-mismatch detection before
/// delegating to <see cref="ValidateValue"/>.
/// </para>
/// <para>
/// If <paramref name="allowNull"/> is <see langword="true"/> (the default) and the value is
/// <see langword="null"/>, validation succeeds immediately. If <see langword="false"/>, the
/// <see langword="null"/> value is forwarded to <see cref="ValidateValue"/> for further evaluation.
/// </para>
/// </remarks>
/// <param name="expectedType">The CLR type the attribute is designed to validate.</param>
/// <param name="allowNull">
/// When <see langword="true"/>, <see langword="null"/> values pass validation without invoking
/// <see cref="ValidateValue"/>; when <see langword="false"/>, <see langword="null"/> values are
/// forwarded to the subclass for evaluation.
/// </param>
/// <seealso cref="GenericDictionaryAttributeBase"/>
/// <seealso href="https://pineguard.ai/docs/annotations">Annotation documentation</seealso>
public abstract class ValidationAttributeBase(Type expectedType, bool allowNull = true) : ValidationAttribute
{
    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return allowNull ? ValidationResult.Success : ValidateValue(value, validationContext);

        if (!expectedType.IsInstanceOfType(value))
            throw new InvalidOperationException(
                $"[{GetType().Name}] can only be applied to properties of type {expectedType.Name}. " +
                $"Property '{validationContext.DisplayName}' is of type {value.GetType().Name}.");

        return ValidateValue(value, validationContext);
    }

    /// <summary>
    /// When overridden in a derived class, validates the non-<see langword="null"/>, type-checked value.
    /// </summary>
    /// <param name="value">The value to validate (already verified as an instance of the expected type).</param>
    /// <param name="validationContext">The validation context for the current member.</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    protected abstract ValidationResult? ValidateValue(object? value, ValidationContext validationContext);

    /// <summary>
    /// Converts a <see cref="MustResult{T}"/> to a <see cref="ValidationResult"/>, substituting the
    /// member display name into the error message template.
    /// </summary>
    /// <typeparam name="T">The parsed-value type carried by the <see cref="MustResult{T}"/>.</typeparam>
    /// <param name="result">The must-clause result to convert.</param>
    /// <param name="context">The validation context for the current member.</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> with the formatted error.
    /// </returns>
    protected ValidationResult? FromMustResult<T>(MustResult<T> result, ValidationContext context)
    {
        if (result.Success)
            return ValidationResult.Success;

        var errorTemplate = !string.IsNullOrWhiteSpace(ErrorMessage) || !string.IsNullOrWhiteSpace(ErrorMessageResourceName)
            ? FormatErrorMessage(context.DisplayName)
            : result.Message.Replace("{paramName}", context.DisplayName);

        return new ValidationResult(errorTemplate, [context.MemberName!]);
    }

    /// <summary>
    /// Builds an argument array suitable for reflective invocation of a must-clause method, filling
    /// positional parameters and defaulting trailing optional parameters.
    /// </summary>
    /// <param name="method">The <see cref="MethodInfo"/> to build arguments for.</param>
    /// <param name="value">The value being validated (placed at index 1).</param>
    /// <param name="args">Additional arguments to pass after the value.</param>
    /// <returns>An object array ready for <see cref="MethodBase.Invoke(object, object[])"/>.</returns>
    protected static object?[] BuildInvokeArgs(MethodInfo method, object? value, object?[] args)
    {
        var parameters = method.GetParameters();
        var invokeArgs = new object?[parameters.Length];

        if (parameters.Length > 0) invokeArgs[0] = null;
        if (parameters.Length > 1) invokeArgs[1] = value;

        var argIndex = 2;
        for (var i = 0; i < args.Length && argIndex < parameters.Length; i++)
            invokeArgs[argIndex++] = args[i];

        for (; argIndex < parameters.Length; argIndex++)
            invokeArgs[argIndex] = parameters[argIndex].DefaultValue;

        return invokeArgs;
    }

    /// <summary>
    /// Invokes the specified must-clause method via reflection and maps the <see cref="MustResult{T}"/>
    /// result to a <see cref="ValidationResult"/>, itself via reflection rather than <see langword="dynamic"/>.
    /// </summary>
    /// <param name="method">The must-clause <see cref="MethodInfo"/> to invoke.</param>
    /// <param name="invokeArgs">The argument array to pass to the method.</param>
    /// <param name="ctx">The validation context for the current member.</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    /// <remarks>
    /// Reflection (rather than <see langword="dynamic"/>/the DLR) is used because the DLR fails to bind
    /// members on a <see cref="MustResult{T}"/> parameterized with a non-public type argument.
    /// </remarks>
    protected ValidationResult? InvokeAndMapResult(MethodInfo method, object?[] invokeArgs, ValidationContext ctx)
    {
        var resultObj = method.Invoke(null, invokeArgs)!;
        var resultType = resultObj.GetType();

        var success = (bool)resultType.GetProperty(nameof(MustResult<object>.Success))!.GetValue(resultObj)!;
        if (success) return ValidationResult.Success;

        var msg = (string)resultType.GetProperty(nameof(MustResult<object>.Message))!.GetValue(resultObj)!;
        var errorTemplate = !string.IsNullOrWhiteSpace(ErrorMessage) || !string.IsNullOrWhiteSpace(ErrorMessageResourceName)
            ? FormatErrorMessage(ctx.DisplayName)
            : msg.Replace("{paramName}", ctx.DisplayName);

        return new ValidationResult(errorTemplate, [ctx.MemberName!]);
    }
}
