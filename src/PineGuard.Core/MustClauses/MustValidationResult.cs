using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Everything a <see cref="MustValidator{T}"/> found: the object-level counterpart of <see cref="MustResult{T}"/>.
/// </summary>
/// <seealso cref="MustFailure"/>
/// <seealso cref="MustValidator{T}"/>
/// <seealso cref="MustValidationException"/>
public sealed class MustValidationResult
{
    private static readonly MustValidationResult OkInstance = new([]);

    /// <summary>
    /// Gets a value indicating whether validation succeeded (<see cref="Failures"/> is empty).
    /// </summary>
    public bool Success => Failures.Count == 0;

    /// <summary>
    /// Gets a value indicating whether validation failed (<see cref="Failures"/> is non-empty).
    /// </summary>
    public bool Failed => !Success;

    /// <summary>
    /// Gets every failure found, in rule registration order (then element order for collection rules).
    /// </summary>
    public IReadOnlyList<MustFailure> Failures { get; }

    /// <summary>
    /// Gets <see cref="string.Empty"/> on success, or every failure's <c>"{PropertyPath}: {Message}"</c>
    /// (path omitted at the root) joined by <c>"; "</c>.
    /// </summary>
    public string Message { get; }

    private MustValidationResult(IReadOnlyList<MustFailure> failures)
    {
        Failures = failures;
        Message = failures.Count == 0 ? string.Empty : string.Join("; ", failures.Select(FormatFailure));
    }

    private static string FormatFailure(MustFailure failure) =>
        string.IsNullOrEmpty(failure.PropertyPath) ? failure.Message : $"{failure.PropertyPath}: {failure.Message}";

    /// <summary>
    /// Gets the shared successful <see cref="MustValidationResult"/> instance.
    /// </summary>
    public static MustValidationResult Ok() => OkInstance;

    /// <summary>
    /// Creates a failed <see cref="MustValidationResult"/> from one or more failures.
    /// </summary>
    /// <param name="failure">The first failure. Required so an empty failure list is unrepresentable.</param>
    /// <param name="additional">Any further failures.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> or <paramref name="additional"/> is <see langword="null"/>.</exception>
    public static MustValidationResult Fail(MustFailure failure, params MustFailure[] additional)
    {
        ThrowHelper.ThrowIfNull(failure);
        ThrowHelper.ThrowIfNull(additional);

        var failures = new List<MustFailure>(additional.Length + 1) { failure };
        failures.AddRange(additional);
        return new MustValidationResult(failures);
    }

    /// <summary>
    /// Creates a failed <see cref="MustValidationResult"/> from a sequence of failures.
    /// </summary>
    /// <param name="failures">The failures. Must contain at least one element.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failures"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="failures"/> is empty.</exception>
    public static MustValidationResult Fail(IEnumerable<MustFailure> failures)
    {
        ThrowHelper.ThrowIfNull(failures);

        var list = failures.ToList();
        if (list.Count == 0)
            throw new ArgumentException("At least one failure is required.", nameof(failures));

        return new MustValidationResult(list);
    }

    /// <summary>
    /// Builds a <see cref="MustValidationResult"/> from zero or more <see cref="IMustResult"/> instances,
    /// keeping only the failures. <see cref="MustFailure.PropertyPath"/> is each result's <c>ParamName</c>.
    /// </summary>
    /// <param name="results">The results to inspect.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    public static MustValidationResult From(params IMustResult[] results) =>
        From((IEnumerable<IMustResult>)results);

    /// <summary>
    /// Builds a <see cref="MustValidationResult"/> from a sequence of <see cref="IMustResult"/> instances,
    /// keeping only the failures. <see cref="MustFailure.PropertyPath"/> is each result's <c>ParamName</c>.
    /// </summary>
    /// <param name="results">The results to inspect.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    public static MustValidationResult From(IEnumerable<IMustResult> results)
    {
        ThrowHelper.ThrowIfNull(results);

        var failures = results.Where(r => r.Failed).Select(r => MustFailure.From(r)).ToList();
        return failures.Count == 0 ? OkInstance : new MustValidationResult(failures);
    }

    /// <summary>
    /// Combines zero or more <see cref="MustValidationResult"/> instances, keeping every failure.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    public static MustValidationResult Combine(params MustValidationResult[] results) =>
        Combine((IEnumerable<MustValidationResult>)results);

    /// <summary>
    /// Combines a sequence of <see cref="MustValidationResult"/> instances, keeping every failure.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="results"/> is <see langword="null"/>.</exception>
    public static MustValidationResult Combine(IEnumerable<MustValidationResult> results)
    {
        ThrowHelper.ThrowIfNull(results);

        var failures = results.SelectMany(r => r.Failures).ToList();
        return failures.Count == 0 ? OkInstance : new MustValidationResult(failures);
    }

    /// <summary>
    /// Re-roots every failure's <see cref="MustFailure.PropertyPath"/> under <paramref name="prefix"/>.
    /// </summary>
    /// <param name="prefix">The path segment every failure is nested under.</param>
    /// <returns><see langword="this"/> on success (nothing to re-root); otherwise a new result with re-rooted failures.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="prefix"/> is <see langword="null"/>.</exception>
    public MustValidationResult WithPropertyPathPrefix(string prefix)
    {
        ThrowHelper.ThrowIfNull(prefix);

        if (Success)
            return this;

        var failures = Failures
            .Select(f => f with { PropertyPath = string.IsNullOrEmpty(f.PropertyPath) ? prefix : PropertyPathUtility.Combine(prefix, f.PropertyPath) })
            .ToList();

        return new MustValidationResult(failures);
    }

    /// <summary>
    /// Throws a <see cref="MustValidationException"/> if <see cref="Failed"/> is <see langword="true"/>.
    /// </summary>
    /// <exception cref="MustValidationException">Thrown when <see cref="Failed"/> is <see langword="true"/>.</exception>
    public void ThrowIfFailed()
    {
        if (Failed) throw new MustValidationException(this);
    }

    /// <summary>
    /// Implicitly converts a <see cref="MustValidationResult"/> to <see cref="bool"/> by returning <see cref="Success"/>.
    /// </summary>
    /// <param name="result">The result to convert. A <see langword="null"/> reference converts to <see langword="false"/>.</param>
    public static implicit operator bool(MustValidationResult? result) => result?.Success ?? false;

    /// <inheritdoc/>
    public override string ToString() => Message;
}
