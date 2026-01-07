using FluentValidation;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation.Common;

public static class FluentExtension
{
    private const string ParamNameToken = "{paramName}";
    private const string DefaultNotNullMessageTemplate = "{paramName} must not be null.";

    public static IRuleBuilderOptions<T, TProp?> MustBe<T, TProp>(
        this IRuleBuilder<T, TProp?> ruleBuilder,
        Func<TProp?, MustResult<TProp?>> check,
        string? message)
    {
        ArgumentNullException.ThrowIfNull(ruleBuilder);
        ArgumentNullException.ThrowIfNull(check);

        return ruleBuilder
            .Must((_, value, context) =>
            {
                var result = check(value);
                if (result.Success)
                    return true;

                var propertyName = GetPropertyName(context);
                var errorMessage = FormatMessage(message ?? result.Message, propertyName);
                context.MessageFormatter.AppendArgument("ErrorMessage", errorMessage);
                return false;
            })
            .WithMessage("{ErrorMessage}");
    }

    private static string FormatMessage(string template, string paramName) =>
        template.Replace(ParamNameToken, paramName, StringComparison.Ordinal);

    private static string GetPropertyName<T>(ValidationContext<T> context)
    {
        var displayName = context.DisplayName;
        if (!string.IsNullOrWhiteSpace(displayName))
            return displayName;

        return context.PropertyPath ?? "value";
    }
}
