using PineGuard.Common;

namespace PineGuard.GuardClauses;

/// <summary>
/// Configures the global and ambient exception-replacement policy for all <c>Guard.Against.*</c> guards.
/// </summary>
/// <remarks>
/// <para>
/// By default, guards throw <see cref="ArgumentException"/> or <see cref="ArgumentNullException"/>.
/// Use <see cref="ExceptionReplacer"/> to redirect all guard failures to a different exception type
/// (e.g., a custom domain exception).
/// </para>
/// <para>
/// Use <see cref="BeginScope"/> to override the policy within a bounded ambient scope (e.g., per-request
/// in an ASP.NET Core application), restoring the previous policy automatically on disposal.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// using (GuardExceptionPolicy.BeginScope(o =>
///     o.ExceptionReplacer = ex => new DomainException(ex.Message)))
/// {
///     Guard.Against.Null(value);  // throws DomainException instead of ArgumentNullException
/// }
/// </code>
/// </example>
/// <seealso cref="GuardExceptionPolicyOptions"/>
/// <seealso cref="GuardFailure"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public static class GuardExceptionPolicy
{
    private static readonly AsyncLocal<ScopeFrame?> CurrentScope = new();

    /// <summary>
    /// Gets or sets the global exception-replacement factory.
    /// </summary>
    /// <remarks>
    /// When set, the factory maps the default <see cref="ArgumentException"/> / <see cref="ArgumentNullException"/>
    /// to a custom exception. If an ambient scope is active, the scope-local value is used instead.
    /// If <see langword="null"/>, no replacement occurs.
    /// </remarks>
    public static Func<Exception, Exception>? ExceptionReplacer
    {
        get => CurrentScope.Value?.Options.ExceptionReplacer ?? Volatile.Read(ref field);
        set
        {
            var currentScope = CurrentScope.Value;
            if (currentScope is null)
            {
                Volatile.Write(ref field, value);
                return;
            }

            currentScope.Options.ExceptionReplacer = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the exception replacer applies to
    /// the built-in <see cref="ArgumentException"/> family as well as other types.
    /// </summary>
    /// <remarks>
    /// When <see langword="false"/> (the default), the replacer is not applied to
    /// <see cref="ArgumentException"/> or <see cref="ArgumentNullException"/>.
    /// Set to <see langword="true"/> to replace all exception types uniformly.
    /// </remarks>
    public static bool ReplaceDefaultExceptions
    {
        get => CurrentScope.Value?.Options.ReplaceDefaultExceptions ?? Volatile.Read(ref field);
        set
        {
            var currentScope = CurrentScope.Value;
            if (currentScope is null)
            {
                Volatile.Write(ref field, value);
                return;
            }

            currentScope.Options.ReplaceDefaultExceptions = value;
        }
    }

    /// <summary>
    /// Begins an ambient policy scope that overrides the current exception-replacement settings.
    /// Restores the previous settings when the returned <see cref="IDisposable"/> is disposed.
    /// </summary>
    /// <param name="configure">
    /// A delegate that configures the <see cref="GuardExceptionPolicyOptions"/> for the scope.
    /// </param>
    /// <returns>An <see cref="IDisposable"/> that restores the previous policy on disposal.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static IDisposable BeginScope(Action<GuardExceptionPolicyOptions> configure)
    {
        ThrowHelper.ThrowIfNull(configure);

        var scope = new ScopeFrame(
            new GuardExceptionPolicyOptions
            {
                ExceptionReplacer = ExceptionReplacer,
                ReplaceDefaultExceptions = ReplaceDefaultExceptions
            },
            CurrentScope.Value);

        configure(scope.Options);
        CurrentScope.Value = scope;

        return new ScopeLease(scope);
    }

    internal static bool ShouldReplace(Exception exception) =>
        ReplaceDefaultExceptions || exception is not ArgumentException;

    private sealed class ScopeFrame(GuardExceptionPolicyOptions options, ScopeFrame? previous)
    {
        public GuardExceptionPolicyOptions Options { get; } = options;
        public ScopeFrame? Previous { get; } = previous;
    }

    private sealed class ScopeLease(ScopeFrame scope) : IDisposable
    {
        private ScopeFrame? _scope = scope;

        public void Dispose()
        {
            var currentScope = Interlocked.Exchange(ref _scope, null);
            if (currentScope is null)
                return;

            if (ReferenceEquals(CurrentScope.Value, currentScope))
                CurrentScope.Value = currentScope.Previous;
        }
    }
}
