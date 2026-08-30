namespace PineGuard.MustClauses;

/// <summary>
/// Base for every <c>RuleForAsync</c>/<c>RuleForEachAsync</c> runner: an async rule has no
/// synchronous form, so <see cref="Run"/> is sealed here to one guard and each runner implements
/// only <see cref="MustRuleRunnerBase{T}.RunAsync"/>.
/// </summary>
/// <typeparam name="T">The type the owning <see cref="MustValidator{T}"/> validates.</typeparam>
internal abstract class MustAsyncRuleRunnerBase<T>(string propertyPath) : MustRuleRunnerBase<T>(propertyPath)
{
    /// <summary>
    /// Builds the exception both this runner and <see cref="MustValidator{T}.Validate"/> throw when a
    /// validator carrying async rules is asked for a synchronous answer, so the wording is written once.
    /// </summary>
    internal static InvalidOperationException AsyncRulesRequireValidateAsync() =>
        new($"{typeof(T).Name} has async rules; call ValidateAsync.");

    /// <summary>
    /// Never produces failures: an async rule cannot run synchronously.
    /// <see cref="MustValidator{T}.Validate"/> rejects the validator before any runner is reached, so
    /// this is the guard for a caller that reaches a runner another way.
    /// </summary>
    /// <exception cref="InvalidOperationException">Always thrown.</exception>
    public sealed override IEnumerable<MustFailure> Run(T instance) =>
        throw AsyncRulesRequireValidateAsync();
}
