using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Runs a two-argument <c>RuleForEachAsync(x =&gt; x.Items, (instance, item, ct) =&gt; …)</c> cross-property
/// rule against every element, reporting failures at <c>Property[i]</c>. Skips a <see langword="null"/>
/// collection; enumerates exactly once; awaits each element in order.
/// </summary>
internal sealed class MustAsyncCollectionCrossPropertyRuleRunner<T, TItem, TResult>(
    string propertyPath,
    Func<T, IEnumerable<TItem>?> accessor,
    Func<T, TItem, CancellationToken, ValueTask<MustResult<TResult>>> check)
    : MustAsyncRuleRunnerBase<T>(propertyPath)
{
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
            var result = await check(instance, item, cancellationToken).ConfigureAwait(false);
            if (result.Failed)
                failures.Add(BuildFailure(result, PropertyPathUtility.Index(PropertyPath, index)));

            index++;
        }

        return failures;
    }
}
