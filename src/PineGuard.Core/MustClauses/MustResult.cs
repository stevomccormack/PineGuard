using PineGuard.Common;

namespace PineGuard.MustClauses;

/// <summary>
/// Represents the outcome of a <c>Must.Be.*</c> validation or parsing operation.
/// </summary>
/// <typeparam name="T">The type of the validated or parsed result value.</typeparam>
/// <remarks>
/// A <see cref="MustResult{T}"/> is produced by every MustClause method. Callers inspect
/// <see cref="Success"/> (or <see cref="Failed"/>) and, on failure, read <see cref="Message"/>
/// for a human-readable description and <see cref="ParamName"/> for the failing parameter name.
/// </remarks>
/// <seealso cref="Must"/>
/// <seealso cref="IMustClause"/>
/// <seealso href="https://pineguard.ai/docs/must">Must Clauses documentation</seealso>
public sealed class MustResult<T>
{
    /// <summary>
    /// Gets a value indicating whether the validation or parsing operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets a value indicating whether the validation or parsing operation failed.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="Success"/> is <see langword="false"/>; otherwise, <see langword="false"/>.</returns>
    public bool Failed => !Success;

    /// <summary>
    /// Gets the human-readable failure message, or <see cref="string.Empty"/> on success.
    /// </summary>
    /// <remarks>
    /// The message is produced by substituting <see cref="ParamName"/> into the message
    /// template via <c>{paramName}</c> placeholder replacement.
    /// </remarks>
    public string Message { get; }

    /// <summary>
    /// Gets the name of the parameter that failed validation, or <see langword="null"/> if unknown.
    /// </summary>
    public string? ParamName { get; }

    /// <summary>
    /// The original value that was validated/parsed.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// The typed result produced by the operation when <see cref="Success"/> is <see langword="true"/>.
    /// </summary>
    public T? Result { get; }

    private MustResult(bool success, string message, string? paramName, object? value, T? result)
    {
        Success = success;
        Message = message;
        ParamName = paramName;
        Value = value;
        Result = result;
    }

    /// <summary>
    /// Creates a successful <see cref="MustResult{T}"/> with the given typed result.
    /// </summary>
    /// <param name="result">The typed result value.</param>
    /// <param name="value">The original untyped value that was validated.</param>
    /// <param name="paramName">The optional parameter name associated with this result.</param>
    /// <returns>A <see cref="MustResult{T}"/> with <see cref="Success"/> set to <see langword="true"/>.</returns>
    public static MustResult<T> Ok(T? result, object? value = null, string? paramName = null) =>
        new(true, string.Empty, paramName, value, result);

    /// <summary>
    /// Creates a failed <see cref="MustResult{T}"/> with a formatted failure message.
    /// </summary>
    /// <param name="messageTemplate">
    /// The message template. Use <c>{paramName}</c> as a placeholder for the parameter name.
    /// </param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original untyped value that failed validation.</param>
    /// <returns>A <see cref="MustResult{T}"/> with <see cref="Failed"/> set to <see langword="true"/>.</returns>
    public static MustResult<T> Fail(string messageTemplate, string? paramName, object? value) =>
        new(false, FormatMessage(messageTemplate, paramName), paramName, value, default);

    /// <summary>
    /// Creates a failed <see cref="MustResult{T}"/> from a message that is already fully formatted
    /// and must not be passed through <c>{paramName}</c> placeholder substitution again — e.g. when
    /// joining the already-formatted <see cref="Message"/> values of several other results, as in
    /// <see cref="MustResultExtension.Combine{T}"/>.
    /// </summary>
    /// <param name="message">The final, already-formatted failure message.</param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original untyped value that failed validation.</param>
    /// <returns>A <see cref="MustResult{T}"/> with <see cref="Failed"/> set to <see langword="true"/>.</returns>
    internal static MustResult<T> FailPreformatted(string message, string? paramName, object? value) =>
        new(false, message, paramName, value, default);

    /// <summary>
    /// Implicitly converts a <see cref="MustResult{T}"/> to <see cref="bool"/> by returning <see cref="Success"/>.
    /// </summary>
    /// <param name="mustResult">The result to convert.</param>
    /// <returns><see langword="true"/> if the result represents success; otherwise, <see langword="false"/>.</returns>
    public static implicit operator bool(MustResult<T> mustResult) => mustResult.Success;

    /// <summary>
    /// Deconstructs the result into its component parts.
    /// </summary>
    /// <param name="success">Set to <see cref="Success"/>.</param>
    /// <param name="message">Set to <see cref="Message"/>.</param>
    /// <param name="paramName">Set to <see cref="ParamName"/>.</param>
    /// <param name="value">Set to <see cref="Value"/>.</param>
    /// <param name="result">Set to <see cref="Result"/>.</param>
    public void Deconstruct(out bool success, out string message, out string? paramName, out object? value, out T? result)
    {
        success = Success;
        message = Message;
        paramName = ParamName;
        value = Value;
        result = Result;
    }

    /// <summary>
    /// Creates a <see cref="MustResult{T}"/> from a nullable boolean flag.
    /// </summary>
    /// <param name="ok">
    /// If <see langword="true"/>, returns a successful result; if <see langword="false"/> or
    /// <see langword="null"/>, returns a failed result.
    /// </param>
    /// <param name="messageTemplate">The failure message template. Use <c>{paramName}</c> as a placeholder.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <param name="value">The original value being validated.</param>
    /// <param name="result">The typed result to carry on success.</param>
    /// <returns>A success or failure <see cref="MustResult{T}"/> based on <paramref name="ok"/>.</returns>
    public static MustResult<T> FromBool(bool? ok, string messageTemplate, string? paramName, object? value, T? result) =>
        ok ?? false ? Ok(result!, value, paramName) : Fail(messageTemplate, paramName, value);

    /// <summary>
    /// Creates a <see cref="MustResult{T}"/> from a nullable boolean flag with no typed result.
    /// </summary>
    /// <param name="ok">
    /// If <see langword="true"/>, returns a successful result; if <see langword="false"/> or
    /// <see langword="null"/>, returns a failed result.
    /// </param>
    /// <param name="messageTemplate">The failure message template. Use <c>{paramName}</c> as a placeholder.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <param name="value">The original value being validated.</param>
    /// <returns>A success or failure <see cref="MustResult{T}"/> based on <paramref name="ok"/>.</returns>
    public static MustResult<T> FromBool(bool? ok, string messageTemplate, string? paramName, object? value) =>
        FromBool(ok, messageTemplate, paramName, value, result: default);

    internal static string FormatMessage(string messageTemplate, string? paramName) =>
        string.IsNullOrEmpty(paramName)
            ? messageTemplate
            : messageTemplate.Replace("{paramName}", paramName, StringComparison.Ordinal);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the result represents a failure.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public void ThrowIfFailed()
    {
        if (Failed) throw new ArgumentException(Message, ParamName);
    }

    /// <summary>
    /// Throws a custom exception if the result represents a failure.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <param name="exceptionFactory">
    /// A factory that creates the exception from the failure <see cref="Message"/> and <see cref="ParamName"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception">Thrown as <typeparamref name="TException"/> when <see cref="Failed"/> is <see langword="true"/>.</exception>
    public void ThrowIfFailed<TException>(Func<string, string?, TException> exceptionFactory)
        where TException : Exception
    {
        ThrowHelper.ThrowIfNull(exceptionFactory);

        if (Failed) throw exceptionFactory(Message, ParamName);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the result represents a failure.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public void ThrowNullIfFailed()
    {
        if (Failed) throw new ArgumentNullException(ParamName, Message);
    }

    /// <summary>
    /// Returns <see cref="Result"/> if the validation succeeded, or throws if it failed.
    /// </summary>
    /// <returns>
    /// The typed <see cref="Result"/> value, which may be <see langword="null"/> when the operation
    /// succeeded but carries no result (e.g. <see cref="MustResultExtension.Combine{T}"/> on an empty
    /// sequence, or a successful result explicitly constructed with a <see langword="null"/> result).
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public T? OrThrow()
    {
        ThrowIfFailed();
        return Result;
    }

    /// <summary>
    /// Returns <see cref="Result"/> if the validation succeeded and non-<see langword="null"/>,
    /// or <paramref name="fallback"/> if <see cref="Result"/> is <see langword="null"/>, or throws if it failed.
    /// </summary>
    /// <param name="fallback">The value to return when <see cref="Result"/> is <see langword="null"/>.</param>
    /// <returns>The typed result or <paramref name="fallback"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public T OrThrow(T fallback)
    {
        ThrowIfFailed();
        return Result is null ? fallback : Result;
    }
}

/// <summary>
/// Extension methods for working with sequences of <see cref="MustResult{T}"/> instances.
/// </summary>
public static class MustResultExtension
{
    /// <summary>
    /// Combines a sequence of <see cref="MustResult{T}"/> instances into a single result.
    /// </summary>
    /// <typeparam name="T">The validated result type.</typeparam>
    /// <param name="results">
    /// The sequence of results to combine. If <see langword="null"/>, returns a failure result.
    /// </param>
    /// <returns>
    /// The first successful result if all results succeeded; otherwise, a failed result whose
    /// <see cref="MustResult{T}.Message"/> concatenates all failure messages separated by <c>"; "</c>.
    /// </returns>
    public static MustResult<T> Combine<T>(this IEnumerable<MustResult<T>>? results)
    {
        if (results is null)
            return MustResult<T>.Fail("{paramName} must not be null.", nameof(results), results);

        var all = results.ToList();
        if (all.Count == 0) return MustResult<T>.Ok(result: default!, value: null);

        var failures = all.Where(r => r.Failed).ToList();
        if (failures.Count == 0) return all[0];

        var first = failures[0];
        var message = string.Join("; ", failures.Select(f => f.Message));

        return MustResult<T>.FailPreformatted(message, first.ParamName, first.Value);
    }

    /// <summary>
    /// Throws if any result in the sequence represents a failure.
    /// </summary>
    /// <typeparam name="T">The validated result type.</typeparam>
    /// <param name="results">The sequence of results to inspect.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when at least one result in <paramref name="results"/> has <see cref="MustResult{T}.Failed"/> set to <see langword="true"/>.
    /// </exception>
    public static void ThrowIfAnyFailed<T>(this IEnumerable<MustResult<T>> results) =>
        results.Combine().ThrowIfFailed();
}
