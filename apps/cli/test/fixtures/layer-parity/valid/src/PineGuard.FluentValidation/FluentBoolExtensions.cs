namespace PineGuard.FluentValidation;

/// <summary>Fluent extensions for boolean values (layer-parity valid/ fixture).</summary>
public static class FluentBoolExtensions
{
    public static IRuleBuilderOptions<TModel, bool> True<TModel>(
        this IRuleBuilder<TModel, bool> ruleBuilder)
    {
        return ruleBuilder.Must(value => value);
    }
}
