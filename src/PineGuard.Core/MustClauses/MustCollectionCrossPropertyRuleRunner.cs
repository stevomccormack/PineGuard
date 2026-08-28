using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Runs a two-argument <c>RuleForEach(x =&gt; x.Items, (instance, item) =&gt; Must.Be.X(item, instance.Other))</c>
/// cross-property rule against every element, reporting failures at <c>Property[i]</c>. Skips a
/// <see langword="null"/> collection; enumerates exactly once.
/// </summary>
internal sealed class MustCollectionCrossPropertyRuleRunner<T, TItem, TResult>(
    string propertyPath,
    Func<T, IEnumerable<TItem>?> accessor,
    Func<T, TItem, MustResult<TResult>> check)
    : MustRuleRunnerBase<T>(propertyPath)
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
            var result = check(instance, item);
            if (result.Failed)
                failures.Add(BuildFailure(result, PropertyPathUtility.Index(PropertyPath, index)));

            index++;
        }

        return failures;
    }

    public override ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken) =>
        new(Run(instance));
}
