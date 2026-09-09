using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Runs every <see cref="ValidationAttribute"/> declared on an object's type and public instance
/// properties, and reports the outcome as a <see cref="MustValidationResult"/>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Validator"/> already does this, but only ever hands back a <see cref="ValidationResult"/> —
/// a shape with no slot for <see cref="MustFailure.Code"/>. This runner fills that gap for PineGuard's own
/// attributes: every failing <see cref="ValidationAttributeBase"/> contributes its
/// <see cref="ValidationAttributeBase.Code"/> to the corresponding <see cref="MustFailure"/>. A failing
/// attribute that is not a <see cref="ValidationAttributeBase"/> (e.g. the framework's own
/// <see cref="RequiredAttribute"/>) still contributes a failure, just with <see cref="MustFailure.Code"/>
/// set to <see cref="string.Empty"/> — there is no code to report.
/// </para>
/// <para>
/// Scope is deliberately limited to <see cref="ValidationAttribute"/>s: unlike <see cref="Validator"/>'s
/// own object validation, this never invokes <see cref="IValidatableObject.Validate"/> — that path never
/// runs through an attribute, so it could never carry a code either, and adding it would double the
/// surface for no benefit.
/// </para>
/// <para>
/// Every attribute on a property is evaluated independently rather than stopping at the first failure —
/// unlike <see cref="Validator"/>'s own property validation, which reports at most one failure per
/// property. A property decorated with both <see cref="RequiredAttribute"/> and a PineGuard attribute that
/// also fails reports both.
/// </para>
/// </remarks>
/// <seealso cref="MustValidationResult"/>
/// <seealso cref="MustFailure"/>
/// <seealso cref="ValidationAttributeBase"/>
public static class DataAnnotationsAttributeValidator
{
    /// <summary>
    /// Runs every <see cref="ValidationAttribute"/> declared on <paramref name="instance"/>'s type and
    /// public instance properties.
    /// </summary>
    /// <param name="instance">The object to validate.</param>
    /// <param name="serviceProvider">
    /// Resolves ambient services a validation attribute reads from its <see cref="ValidationContext"/> —
    /// for example the <see cref="TimeProvider"/> a clock-reading PineGuard attribute resolves via
    /// <see cref="ValidationAttributeBase"/>. <see langword="null"/> (the default) means no such service is
    /// available, which every clock-reading attribute already reads as "use the system clock".
    /// </param>
    /// <returns>
    /// <see cref="MustValidationResult.Ok()"/> when every attribute passes; otherwise a failed result
    /// carrying one <see cref="MustFailure"/> per failing attribute.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// public class ContactModel
    /// {
    ///     [Required]
    ///     public string Name { get; set; } = "";
    ///
    ///     [Email]
    ///     public string EmailAddress { get; set; } = "";
    /// }
    ///
    /// MustValidationResult result = DataAnnotationsAttributeValidator.Validate(
    ///     new ContactModel { EmailAddress = "not-an-email" });
    ///
    /// // result.Failures[0]: PropertyPath = "Name",         Code = "",                     Message = "The Name field is required."
    /// // result.Failures[1]: PropertyPath = "EmailAddress", Code = "email.address.invalid", Message = "EmailAddress must be a valid email address."
    /// </code>
    /// </example>
    public static MustValidationResult Validate(object instance, IServiceProvider? serviceProvider = null)
    {
        ThrowHelper.ThrowIfNull(instance);

        var failures = new List<MustFailure>();
        var type = instance.GetType();

        foreach (var attribute in type.GetCustomAttributes<ValidationAttribute>(inherit: true))
        {
            var context = new ValidationContext(instance, serviceProvider, items: null);
            TryAddFailure(failures, attribute, attribute.GetValidationResult(instance, context), propertyPath: string.Empty, value: null);
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
                continue;

            var attributes = property.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray();
            if (attributes.Length == 0)
                continue;

            var value = property.GetValue(instance);
            var context = new ValidationContext(instance, serviceProvider, items: null) { MemberName = property.Name };

            foreach (var attribute in attributes)
                TryAddFailure(failures, attribute, attribute.GetValidationResult(value, context), property.Name, value);
        }

        return failures.Count == 0 ? MustValidationResult.Ok() : MustValidationResult.Fail(failures);
    }

    private static void TryAddFailure(List<MustFailure> failures, ValidationAttribute attribute, ValidationResult? result, string propertyPath, object? value)
    {
        if (result is null)
            return;

        var code = attribute is ValidationAttributeBase coded ? coded.Code : string.Empty;

        // ValidationAttribute.GetValidationResult backfills a null ErrorMessage with
        // FormatErrorMessage(...) before returning, so it is never actually null here.
        failures.Add(new MustFailure(propertyPath, code, result.ErrorMessage!, value));
    }
}
