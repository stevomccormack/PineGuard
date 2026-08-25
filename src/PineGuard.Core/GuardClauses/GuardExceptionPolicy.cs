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

            CurrentScope.Value = ScopeFrame.WithOverride(
                currentScope,
                new GuardExceptionPolicyOptions
                {
                    ExceptionReplacer = value,
                    ReplaceDefaultExceptions = currentScope.Options.ReplaceDefaultExceptions
                });
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

            CurrentScope.Value = ScopeFrame.WithOverride(
                currentScope,
                new GuardExceptionPolicyOptions
                {
                    ExceptionReplacer = currentScope.Options.ExceptionReplacer,
                    ReplaceDefaultExceptions = value
                });
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
        ShouldReplace(ReplaceDefaultExceptions, exception);

    /// <summary>
    /// Evaluates the replacement gate against an already-resolved <see cref="ReplaceDefaultExceptions"/>
    /// value, so callers that need to combine it with an <see cref="ExceptionReplacer"/> resolved from the
    /// same snapshot (see <see cref="GetEffectivePolicy"/>) never trigger a second, independently-resolved
    /// ambient lookup.
    /// </summary>
    internal static bool ShouldReplace(bool replaceDefaultExceptions, Exception exception) =>
        replaceDefaultExceptions || exception is not ArgumentException;

    /// <summary>
    /// Resolves <see cref="ExceptionReplacer"/> and <see cref="ReplaceDefaultExceptions"/> from a single
    /// ambient-scope resolution, so the pair is always read from the same frame. Reading the two properties
    /// independently can observe a torn combination if a scope is disposed by another execution context
    /// between the reads; this method walks <see cref="ActiveScope"/> exactly once and returns both values
    /// from that one result.
    /// </summary>
    internal static (Func<Exception, Exception>? ExceptionReplacer, bool ReplaceDefaultExceptions) GetEffectivePolicy()
    {
        var currentScope = ActiveScope();
        return currentScope is not null
            ? (currentScope.Options.ExceptionReplacer, currentScope.Options.ReplaceDefaultExceptions)
            : (ExceptionReplacer, ReplaceDefaultExceptions);
    }

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

    private sealed class ScopeFrame
    {
        // A single-element array, rather than a plain field, so that copy-on-write frames created by
        // WithOverride can share the exact same disposal cell as the frame they were derived from: marking
        // the original scope's lease disposed must also invalidate every frame later derived from it via a
        // property setter, even though those frames are distinct object instances.
        private readonly int[] _disposedCell;

        public ScopeFrame(GuardExceptionPolicyOptions options, ScopeFrame? previous)
            : this(options, previous, new int[1])
        {
        }

        private ScopeFrame(GuardExceptionPolicyOptions options, ScopeFrame? previous, int[] disposedCell)
        {
            Options = options;
            Previous = previous;
            _disposedCell = disposedCell;
        }

        public GuardExceptionPolicyOptions Options { get; }
        public ScopeFrame? Previous { get; }

        /// <summary>
        /// Gets a value indicating whether the lease for this frame has been disposed. Disposed frames stay
        /// linked in the chain but are skipped when resolving the ambient policy, so out-of-order disposal
        /// never restores a dead frame's options.
        /// </summary>
        public bool IsDisposed => Volatile.Read(ref _disposedCell[0]) != 0;

        public void MarkDisposed() => Volatile.Write(ref _disposedCell[0], 1);

        /// <summary>
        /// Creates a copy-on-write frame carrying <paramref name="options"/> in place of
        /// <paramref name="source"/>'s options. The new frame shares <paramref name="source"/>'s disposal
        /// cell and <see cref="Previous"/> link, so disposing the scope's original lease invalidates it too.
        /// Assigning the result to <see cref="CurrentScope"/> only changes the ambient policy for the
        /// current execution context and its descendants (normal <see cref="AsyncLocal{T}"/> semantics) —
        /// unlike mutating <paramref name="source"/>.Options in place, it can never be observed by the
        /// parent context or by sibling contexts that already captured a reference to <paramref name="source"/>.
        /// </summary>
        public static ScopeFrame WithOverride(ScopeFrame source, GuardExceptionPolicyOptions options) =>
            new(options, source.Previous, source._disposedCell);
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
