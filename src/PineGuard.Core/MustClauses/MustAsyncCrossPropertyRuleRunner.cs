namespace PineGuard.MustClauses;

/// <summary>
/// Runs a two-argument <c>RuleForAsync(x =&gt; x.Property, (instance, value, ct) =&gt; …)</c> cross-property rule.
/// </summary>
internal sealed class MustAsyncCrossPropertyRuleRunner<T, TProperty, TResult>(
    string propertyPath,
    Func<T, TProperty> accessor,
    Func<T, TProperty, CancellationToken, ValueTask<MustResult<TResult>>> check)
    : MustAsyncRuleRunnerBase<T>(propertyPath)
{
    public override async ValueTask<IEnumerable<MustFailure>> RunAsync(T instance, CancellationToken cancellationToken)
    {
        if (!ConditionsPass(instance))
            return [];

        var result = await check(instance, accessor(instance), cancellationToken).ConfigureAwait(false);
        return result.Failed ? [BuildFailure(result, PropertyPath)] : [];
    }
}
