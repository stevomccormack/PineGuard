using PineGuard.Common;

namespace PineGuard.GuardClauses;

/// <summary>
/// Configures the global and ambient exception-replacement policy for all <c>Guard.Against.*</c> guards.
/// </summary>
/// <remarks>
/// <para>
/// By default, guards throw <see cref="ArgumentException"/> or <see cref="ArgumentNullException"/>.
/// Use <see cref="ExceptionReplacer"/> together with <see cref="ReplaceDefaultExceptions"/> set to
/// <see langword="true"/> to redirect these default guard failures to a different exception type
/// (e.g., a custom domain exception). When <see cref="ReplaceDefaultExceptions"/> is <see langword="false"/>
/// (the default), <see cref="ExceptionReplacer"/> has no effect on the built-in
/// <see cref="ArgumentException"/> / <see cref="ArgumentNullException"/> failures.
/// </para>
/// <para>
/// Use <see cref="BeginScope"/> to override the policy within a bounded ambient scope (e.g., per-request
/// in an ASP.NET Core application), restoring the previous policy automatically on disposal.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// using (GuardExceptionPolicy.BeginScope(o =>
/// {
///     o.ExceptionReplacer = ex => new DomainException(ex.Message);
///     o.ReplaceDefaultExceptions = true;
/// }))
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
    /// When set, and <see cref="ReplaceDefaultExceptions"/> is <see langword="true"/>, the factory maps
    /// the default <see cref="ArgumentException"/> / <see cref="ArgumentNullException"/> to a custom exception.
    /// If an ambient scope is active, the scope-local value is used instead — including a scope that
    /// explicitly sets it back to <see langword="null"/> to disable an inherited replacer for that scope.
    /// If <see langword="null"/> and no scope is active, no replacement occurs.
    /// </remarks>
    public static Func<Exception, Exception>? ExceptionReplacer
    {
        get
        {
            var currentScope = ActiveScope();
            return currentScope is not null ? currentScope.Options.ExceptionReplacer : Volatile.Read(ref field);
        }
        set
        {
            var currentScope = ActiveScope();
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
        get => ActiveScope()?.Options.ReplaceDefaultExceptions ?? Volatile.Read(ref field);
        set
        {
            var currentScope = ActiveScope();
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
            ActiveScope());

        configure(scope.Options);
        CurrentScope.Value = scope;

        return new ScopeLease(scope);
    }

    internal static bool ShouldReplace(Exception exception) =>
        ReplaceDefaultExceptions || exception is not ArgumentException;

    /// <summary>
    /// Resolves the innermost scope frame that is still active, skipping any frames already disposed
    /// out of order. Returns <see langword="null"/> when no active scope remains.
    /// </summary>
    private static ScopeFrame? ActiveScope()
    {
        var frame = CurrentScope.Value;
        while (frame is not null && frame.IsDisposed)
            frame = frame.Previous;

        return frame;
    }

    private sealed class ScopeFrame(GuardExceptionPolicyOptions options, ScopeFrame? previous)
    {
        private int _disposed;

        public GuardExceptionPolicyOptions Options { get; } = options;
        public ScopeFrame? Previous { get; } = previous;

        /// <summary>
        /// Gets a value indicating whether the lease for this frame has been disposed. Disposed frames stay
        /// linked in the chain but are skipped when resolving the ambient policy, so out-of-order disposal
        /// never restores a dead frame's options.
        /// </summary>
        public bool IsDisposed => Volatile.Read(ref _disposed) != 0;

        public void MarkDisposed() => Volatile.Write(ref _disposed, 1);
    }

    private sealed class ScopeLease(ScopeFrame scope) : IDisposable
    {
        private ScopeFrame? _scope = scope;

        public void Dispose()
        {
            var currentScope = Interlocked.Exchange(ref _scope, null);
            if (currentScope is null)
                return;

            // Frames are never unlinked — the chain is shared across execution contexts via AsyncLocal, so
            // mutating it would race. Marking the frame instead makes every stale reference to it inert, and
            // resolution simply walks past disposed frames regardless of the order leases are disposed in.
            currentScope.MarkDisposed();

            CurrentScope.Value = ActiveScope();
        }
    }
}
