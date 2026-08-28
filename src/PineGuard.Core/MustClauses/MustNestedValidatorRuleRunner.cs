using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Runs a <c>RuleFor(x =&gt; x.Property, nestedValidator)</c> rule: skips a <see langword="null"/>
/// property (presence is a separate rule) and re-roots the nested result under the property path.
/// </summary>
internal sealed class MustNestedValidatorRuleRunner<T, TProperty>(
    string propertyPath,
    Func<T, TProperty?> accessor,
    IMustValidator<TProperty> validator)
    : MustRuleRunnerBase<T>(propertyPath)
    where TProperty : notnull
{
    public override IEnumerable<MustFailure> Run(T instance)
    {
        if (!ConditionsPass(instance))
            return [];

        if (accessor(instance) is not TProperty property)
            return [];

        var result = validator.Validate(property);
        return result.Success ? [] : result.WithPropertyPathPrefix(PropertyPath).Failures;
    }

    public override async ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken)
    {
        if (!ConditionsPass(instance))
            return [];

        if (accessor(instance) is not TProperty property)
            return [];

        var result = await validator.ValidateAsync(property, cancellationToken).ConfigureAwait(false);
        return result.Success ? [] : result.WithPropertyPathPrefix(PropertyPath).Failures;
    }
}
