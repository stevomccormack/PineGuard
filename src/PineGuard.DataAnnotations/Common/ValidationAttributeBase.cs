using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.Common;
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
/// <param name="code">
/// The <c>MustCodes</c> catalogue constant identifying the clause this attribute adapts. Every attribute
/// passes the constant of the clause it invokes — PineGuard never subclasses a framework result type, so
/// this is where the code lives for the DataAnnotations adapter (see <see cref="Code"/>).
/// </param>
/// <param name="allowNull">
/// When <see langword="true"/>, <see langword="null"/> values pass validation without invoking
/// <see cref="ValidateValue"/>; when <see langword="false"/>, <see langword="null"/> values are
/// forwarded to the subclass for evaluation.
/// </param>
/// <seealso cref="GenericDictionaryAttributeBase"/>
/// <seealso href="https://pineguard.ai/docs/annotations">Annotation documentation</seealso>
public abstract class ValidationAttributeBase(Type expectedType, string code, bool allowNull = true) : ValidationAttribute
{
    private const string ParamNameToken = "{paramName}";

    /// <summary>
    /// Gets the <c>MustCodes</c> catalogue constant identifying the clause this attribute adapts.
    /// </summary>
    /// <remarks>
    /// The natural resource key for a later DataAnnotations localisation hook
    /// (<see cref="ValidationAttribute.ErrorMessageResourceType"/>/<see cref="ValidationAttribute.ErrorMessageResourceName"/>),
    /// and design-time metadata a form generator or OpenAPI enricher can read before any validation runs.
    /// </remarks>
    public string Code { get; } = RequireCode(code);

    private static string RequireCode(string code)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(code);
        return code;
    }

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
    /// Formats the attribute's error message, substituting the <c>{paramName}</c> token used by every
    /// PineGuard message template when present, per <c>docs/ai/specs/data-annotations/project.md</c> §2.
    /// </summary>
    /// <param name="name">The display name to substitute into the error message.</param>
    /// <returns>The formatted error message.</returns>
    /// <remarks>
    /// When <see cref="ValidationAttribute.ErrorMessageString"/> contains the <c>{paramName}</c> token
    /// (e.g. a user-supplied <see cref="ValidationAttribute.ErrorMessage"/> reusing the library's
    /// convention), it is replaced directly rather than passed through <see cref="string.Format(string, object?)"/>,
    /// which would otherwise throw <see cref="FormatException"/> on the non-numeric token. Otherwise, this
    /// falls back to the base <see cref="ValidationAttribute.FormatErrorMessage"/> behavior.
    /// </remarks>
    public override string FormatErrorMessage(string name) =>
        ErrorMessageString.Contains(ParamNameToken, StringComparison.Ordinal)
            ? ErrorMessageString.Replace(ParamNameToken, name, StringComparison.Ordinal)
            : base.FormatErrorMessage(name);

    /// <summary>
    /// Builds the <see cref="ValidationResult"/> for a failed validation, applying the same
    /// custom-message-vs-must-message priority and <c>{paramName}</c> substitution used by both
    /// <see cref="FromMustResult{T}"/> and <see cref="InvokeAndMapResult"/>.
    /// </summary>
    /// <param name="mustMessage">The message template returned by the must-clause on failure.</param>
    /// <param name="context">The validation context for the current member.</param>
    /// <returns>A <see cref="ValidationResult"/> describing the failure.</returns>
    private ValidationResult BuildFailureResult(string mustMessage, ValidationContext context)
    {
        var errorTemplate = !string.IsNullOrWhiteSpace(ErrorMessage) || !string.IsNullOrWhiteSpace(ErrorMessageResourceName)
            ? FormatErrorMessage(context.DisplayName)
            : mustMessage.Replace(ParamNameToken, context.DisplayName, StringComparison.Ordinal);

        return context.MemberName is { } memberName
            ? new ValidationResult(errorTemplate, [memberName])
            : new ValidationResult(errorTemplate);
    }

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
    protected ValidationResult? FromMustResult<T>(MustResult<T> result, ValidationContext context) =>
        result.Success ? ValidationResult.Success : BuildFailureResult(result.Message, context);

    /// <summary>
    /// Resolves the <see cref="TimeProvider"/> a clock-reading attribute should validate against from the
    /// validation context's service provider.
    /// </summary>
    /// <param name="validationContext">The validation context for the current member.</param>
    /// <returns>
    /// The registered <see cref="TimeProvider"/>, or <see langword="null"/> when the context resolves no
    /// service — which every clock-reading must-clause already reads as "use the system clock".
    /// </returns>
    /// <remarks>
    /// <para>
    /// Every other PineGuard layer takes the clock as a <c>TimeProvider? timeProvider = null</c> parameter.
    /// An attribute cannot: its arguments must be compile-time constants, and a <see cref="TimeProvider"/>
    /// is not one. Service resolution is the substitute seam — a test (or a host) registers the provider on
    /// the <see cref="ValidationContext"/>, and the attribute reads it at validation time.
    /// </para>
    /// <para>
    /// <see cref="ValidationContext"/> implements <see cref="IServiceProvider"/> over the
    /// <c>serviceProvider</c> passed to its constructor, so a <c>new ValidationContext(instance)</c> with no
    /// provider simply yields <see langword="null"/> here and the system clock applies.
    /// </para>
    /// </remarks>
    protected static TimeProvider? ResolveTimeProvider(ValidationContext validationContext) =>
        validationContext.GetService(typeof(TimeProvider)) as TimeProvider;

    /// <summary>
    /// Builds an argument array suitable for reflective invocation of a must-clause method, filling
    /// positional parameters and defaulting trailing optional parameters.
    /// </summary>
    /// <param name="method">The <see cref="MethodInfo"/> to build arguments for.</param>
    /// <param name="value">The value being validated (placed at index 1).</param>
    /// <param name="args">Additional arguments to pass after the value.</param>
    /// <returns>An object array ready for <see cref="MethodBase.Invoke(object, object[])"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/> is <see langword="null"/> and the method's value parameter is a
    /// non-nullable value type. <see cref="MethodBase.Invoke(object, object[])"/> would otherwise
    /// silently coerce the <see langword="null"/> argument to <c>default</c>, flipping the validation
    /// verdict instead of reporting the mismatch.
    /// </exception>
    protected static object?[] BuildInvokeArgs(MethodInfo method, object? value, object?[] args)
    {
        var parameters = method.GetParameters();
        var invokeArgs = new object?[parameters.Length];

        if (parameters.Length > 0) invokeArgs[0] = null;

        if (parameters.Length > 1)
        {
            if (value is null && parameters[1].ParameterType.IsValueType && Nullable.GetUnderlyingType(parameters[1].ParameterType) is null)
                throw new InvalidOperationException(
                    $"Cannot invoke '{method.Name}' with a null value for its non-nullable value-type parameter " +
                    $"'{parameters[1].Name}' ({parameters[1].ParameterType.Name}); reflective invocation would silently " +
                    $"coerce null to default({parameters[1].ParameterType.Name}), producing an incorrect validation result.");

            invokeArgs[1] = value;
        }

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
        return BuildFailureResult(msg, ctx);
    }
}
