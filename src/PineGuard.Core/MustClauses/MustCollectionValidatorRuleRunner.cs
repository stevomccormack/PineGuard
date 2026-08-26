using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Runs a <c>RuleForEach(x =&gt; x.Items, nestedValidator)</c> rule against every element, re-rooting
/// each nested result under <c>Property[i]</c>. Skips a <see langword="null"/> collection and any
/// <see langword="null"/> element; enumerates exactly once.
/// </summary>
internal sealed class MustCollectionValidatorRuleRunner<T, TItem>(
    string propertyPath,
    Func<T, IEnumerable<TItem>?> accessor,
    IMustValidator<TItem> validator)
    : MustRuleRunnerBase<T>(propertyPath)
    where TItem : notnull
{
    public override IEnumerable<MustFailure> Run(T instance)
    {
        if (!ConditionsPass(instance))
            return [];

        var items = accessor(instance);
        if (items is null)
            return [];

        var failures = new List<MustFailure>();
        var index = 0;
        foreach (var item in items)
        {
            if (item is not null)
            {
                var result = validator.Validate(item);
                if (result.Failed)
                    failures.AddRange(result.WithPropertyPathPrefix(PropertyPathUtility.Index(PropertyPath, index)).Failures);
            }

            index++;
        }

        return failures;
    }

    public override async ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken)
    {
        if (!ConditionsPass(instance))
            return [];

        var items = accessor(instance);
        if (items is null)
            return [];

        var failures = new List<MustFailure>();
        var index = 0;
        foreach (var item in items)
        {
            if (item is not null)
            {
                var result = await validator.ValidateAsync(item, cancellationToken).ConfigureAwait(false);
                if (result.Failed)
                    failures.AddRange(result.WithPropertyPathPrefix(PropertyPathUtility.Index(PropertyPath, index)).Failures);
            }

            index++;
        }

        return failures;
    }
}
