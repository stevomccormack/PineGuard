using PineGuard.Common;

namespace PineGuard.GuardClauses;

/// <summary>
/// Configures how <c>Guard.Against.*</c> failures are mapped onto the exception your application throws.
/// </summary>
/// <remarks>
/// <para>
/// By default, guards throw the standard <see cref="ArgumentException"/> / <see cref="ArgumentNullException"/>
/// family. Call <see cref="Map"/> once, at the composition root, to route every guard failure through your
/// own <see cref="Func{GuardFailure,Exception}"/> — switching on <see cref="GuardFailure.Code"/> lets one
/// expression map by code, by code family, or by exception type. <see cref="BeginScope"/> overrides the map
/// for a bounded ambient scope (e.g. a test, or a per-request scope), restoring the previous map on disposal.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// using PineGuard.Codes;
/// using PineGuard.GuardClauses;
///
/// GuardExceptionPolicy.Map(failure => failure.Code switch
/// {
///     MustCodes.Value.State.Null => new MissingRequiredValueException(failure.ParamName, failure.Exception),
///     var c when c.StartsWith(MustCodes.Owasp.Prefix + '.', StringComparison.Ordinal) => new SecurityViolationException(c, failure.Exception),
///     var c => new DomainValidationException(c, failure.Message, failure.Exception),
/// });
/// </code>
/// </example>
/// <seealso cref="GuardFailure"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public static class GuardExceptionPolicy
{
    private static Func<GuardFailure, Exception>? _globalMap;
    private static readonly AsyncLocal<ScopeFrame?> CurrentScope = new();

    /// <summary>
    /// Installs an app-wide map from a <see cref="GuardFailure"/> to the exception guards should throw.
    /// </summary>
    /// <param name="map">The mapping function. Called once per guard failure while no scope overrides it.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="map"/> is <see langword="null"/>.</exception>
    public static void Map(Func<GuardFailure, Exception> map)
    {
        ThrowHelper.ThrowIfNull(map);
        SetMap(map);
    }

    /// <summary>
    /// Begins an ambient scope that overrides the current map. Restores the previous map when the
    /// returned <see cref="IDisposable"/> is disposed.
    /// </summary>
    /// <param name="map">The mapping function for the scope.</param>
    /// <returns>An <see cref="IDisposable"/> that restores the previous map on disposal.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="map"/> is <see langword="null"/>.</exception>
    public static IDisposable BeginScope(Func<GuardFailure, Exception> map)
    {
        ThrowHelper.ThrowIfNull(map);

        var scope = new ScopeFrame(map, ActiveScope());
        CurrentScope.Value = scope;

        return new ScopeLease(scope);
    }

    /// <summary>
    /// Removes the current map. Guards fall back to the standard <see cref="ArgumentException"/> family.
    /// </summary>
    public static void Clear() => SetMap(null);

    /// <summary>
    /// Gets a value indicating whether a map is currently installed (globally, or by an active scope).
    /// </summary>
    public static bool HasMap => GetEffectiveMap() is not null;

    private static void SetMap(Func<GuardFailure, Exception>? map)
    {
        var currentScope = ActiveScope();
        if (currentScope is null)
        {
            Volatile.Write(ref _globalMap, map);
            return;
        }

        CurrentScope.Value = ScopeFrame.WithOverride(currentScope, map);
    }

    /// <summary>
    /// Resolves the map currently in effect: the innermost active scope's map, or the global map when
    /// no scope is active.
    /// </summary>
    internal static Func<GuardFailure, Exception>? GetEffectiveMap()
    {
        var currentScope = ActiveScope();
        return currentScope is not null ? currentScope.Map : Volatile.Read(ref _globalMap);
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
        // the original scope's lease disposed must also invalidate every frame later derived from it via
        // SetMap, even though those frames are distinct object instances.
        private readonly int[] _disposedCell;

        public ScopeFrame(Func<GuardFailure, Exception>? map, ScopeFrame? previous)
            : this(map, previous, new int[1])
        {
        }

        private ScopeFrame(Func<GuardFailure, Exception>? map, ScopeFrame? previous, int[] disposedCell)
        {
            Map = map;
            Previous = previous;
            _disposedCell = disposedCell;
        }

        public Func<GuardFailure, Exception>? Map { get; }
        public ScopeFrame? Previous { get; }

        /// <summary>
        /// Gets a value indicating whether the lease for this frame has been disposed. Disposed frames stay
        /// linked in the chain but are skipped when resolving the ambient map, so out-of-order disposal
        /// never restores a dead frame's map.
        /// </summary>
        public bool IsDisposed => Volatile.Read(ref _disposedCell[0]) != 0;

        public void MarkDisposed() => Volatile.Write(ref _disposedCell[0], 1);

        /// <summary>
        /// Creates a copy-on-write frame carrying <paramref name="map"/> in place of <paramref name="source"/>'s
        /// map. The new frame shares <paramref name="source"/>'s disposal cell and <see cref="Previous"/> link,
        /// so disposing the scope's original lease invalidates it too.
        /// </summary>
        public static ScopeFrame WithOverride(ScopeFrame source, Func<GuardFailure, Exception>? map) =>
            new(map, source.Previous, source._disposedCell);
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
