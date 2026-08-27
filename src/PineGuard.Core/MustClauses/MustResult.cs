using PineGuard.Common;
using PineGuard.GuardClauses;

namespace PineGuard.MustClauses;

/// <summary>
/// Represents the outcome of a <c>Must.Be.*</c> validation or parsing operation.
/// </summary>
/// <typeparam name="T">The type of the validated or parsed result value.</typeparam>
/// <remarks>
/// A <see cref="MustResult{T}"/> is produced by every MustClause method. Callers inspect
/// <see cref="Success"/> (or <see cref="Failed"/>) and, on failure, read <see cref="Message"/>
/// for a human-readable description, <see cref="Code"/> for a stable machine-readable identity,
/// and <see cref="ParamName"/> for the failing parameter name.
/// </remarks>
/// <seealso cref="Must"/>
/// <seealso cref="IMustClause"/>
/// <seealso href="https://pineguard.ai/docs/must">Must Clauses documentation</seealso>
public sealed class MustResult<T> : IMustResult
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
    /// Gets the stable, machine-readable identity of the rule that failed, or <see cref="string.Empty"/> on success.
    /// </summary>
    /// <remarks>
    /// A three-segment <c>&lt;domain&gt;.&lt;aspect&gt;.&lt;condition&gt;</c> address (e.g. <c>email.address.invalid</c>)
    /// drawn from the <c>MustCodes</c> catalogue. Never <see cref="string.Empty"/> when <see cref="Failed"/> is <see langword="true"/>.
    /// </remarks>
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable failure message, or <see cref="string.Empty"/> on success.
    /// </summary>
    /// <remarks>
    /// The message is produced by substituting <see cref="ParamName"/> into the message
    /// template via <c>{paramName}</c> placeholder replacement.
    /// </remarks>
    public string Message { get; }

    /// <summary>
    /// Gets the raw message template with the <c>{paramName}</c> placeholder still present,
    /// or <see cref="string.Empty"/> on success.
    /// </summary>
    /// <remarks>
    /// Lets a caller re-render the failure against a different name than <see cref="ParamName"/> —
    /// e.g. a property path inside an object validator — without leaking a lambda parameter name.
    /// </remarks>
    public string MessageTemplate { get; }

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

    object? IMustResult.Result => Result;

    private MustResult(bool success, string code, string message, string messageTemplate, string? paramName, object? value, T? result)
    {
        Success = success;
        Code = code;
        Message = message;
        MessageTemplate = messageTemplate;
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
        new(true, string.Empty, string.Empty, string.Empty, paramName, value, result);

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
        new(false, string.Empty, MustMessage.Format(messageTemplate, paramName), messageTemplate, paramName, value, default);

    /// <summary>
    /// Creates a failed <see cref="MustResult{T}"/> carrying a stable machine-readable <paramref name="code"/>.
    /// </summary>
    /// <param name="code">The <c>MustCodes</c> catalogue constant identifying the failed rule. Must not be empty.</param>
    /// <param name="messageTemplate">
    /// The message template. Use <c>{paramName}</c> as a placeholder for the parameter name.
    /// </param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original untyped value that failed validation.</param>
    /// <returns>A <see cref="MustResult{T}"/> with <see cref="Failed"/> set to <see langword="true"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is <see langword="null"/> or empty.</exception>
    public static MustResult<T> Fail(string code, string messageTemplate, string? paramName, object? value)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(code);

        return new(false, code, MustMessage.Format(messageTemplate, paramName), messageTemplate, paramName, value, default);
    }

    /// <summary>
    /// Creates a failed <see cref="MustResult{T}"/> from a message that is already fully formatted
    /// and must not be passed through <c>{paramName}</c> placeholder substitution again — e.g. when
    /// joining the already-formatted <see cref="Message"/> values of several other results, as in
    /// <see cref="MustResultExtension.Combine{T}"/>.
    /// </summary>
    /// <param name="code">The code carried onto the combined result (typically the first failure's <see cref="Code"/>).</param>
    /// <param name="message">The final, already-formatted failure message.</param>
    /// <param name="messageTemplate">The raw template carried onto the combined result (typically the first failure's <see cref="MessageTemplate"/>).</param>
    /// <param name="paramName">The name of the parameter that failed validation.</param>
    /// <param name="value">The original untyped value that failed validation.</param>
    /// <returns>A <see cref="MustResult{T}"/> with <see cref="Failed"/> set to <see langword="true"/>.</returns>
    internal static MustResult<T> FailPreformatted(string code, string message, string messageTemplate, string? paramName, object? value) =>
        new(false, code, message, messageTemplate, paramName, value, default);

    /// <summary>
    /// Implicitly converts a <see cref="MustResult{T}"/> to <see cref="bool"/> by returning <see cref="Success"/>.
    /// </summary>
    /// <param name="mustResult">The result to convert. A <see langword="null"/> reference converts to <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the result represents success; otherwise, <see langword="false"/>.</returns>
    public static implicit operator bool(MustResult<T>? mustResult) => mustResult?.Success ?? false;

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

    /// <summary>
    /// Creates a <see cref="MustResult{T}"/> carrying a stable machine-readable <paramref name="code"/> from a nullable boolean flag.
    /// </summary>
    /// <param name="ok">
    /// If <see langword="true"/>, returns a successful result; if <see langword="false"/> or
    /// <see langword="null"/>, returns a failed result.
    /// </param>
    /// <param name="code">The <c>MustCodes</c> catalogue constant identifying the rule on failure. Must not be empty.</param>
    /// <param name="messageTemplate">The failure message template. Use <c>{paramName}</c> as a placeholder.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <param name="value">The original value being validated.</param>
    /// <param name="result">The typed result to carry on success.</param>
    /// <returns>A success or failure <see cref="MustResult{T}"/> based on <paramref name="ok"/>.</returns>
    public static MustResult<T> FromBool(bool? ok, string code, string messageTemplate, string? paramName, object? value, T? result) =>
        ok ?? false ? Ok(result!, value, paramName) : Fail(code, messageTemplate, paramName, value);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the result represents a failure.
    /// </summary>
    /// <remarks>
    /// Stamps <see cref="Code"/> and <see cref="ParamName"/> onto the thrown exception's
    /// <see cref="Exception.Data"/> — read them back via <see cref="PineGuard.GuardClauses.ExceptionExtension"/>.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public void ThrowIfFailed()
    {
        if (!Failed) return;

        throw Stamp(new ArgumentException(Message, ParamName));
    }

    /// <summary>
    /// Throws a custom exception if the result represents a failure.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <param name="exceptionFactory">
    /// A factory that creates the exception from the failure <see cref="Message"/> and <see cref="ParamName"/>.
    /// </param>
    /// <remarks>
    /// Stamps <see cref="Code"/> and <see cref="ParamName"/> onto the thrown exception's
    /// <see cref="Exception.Data"/> — read them back via <see cref="PineGuard.GuardClauses.ExceptionExtension"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception">Thrown as <typeparamref name="TException"/> when <see cref="Failed"/> is <see langword="true"/>.</exception>
    public void ThrowIfFailed<TException>(Func<string, string?, TException> exceptionFactory)
        where TException : Exception
    {
        ThrowHelper.ThrowIfNull(exceptionFactory);

        if (!Failed) return;

        throw Stamp(exceptionFactory(Message, ParamName));
    }

    /// <summary>
    /// Throws a custom exception built from the full <see cref="IMustResult"/> if the result represents a failure.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw.</typeparam>
    /// <param name="exceptionFactory">
    /// A factory that creates the exception from the failed result, so it can read <see cref="Code"/> in
    /// addition to <see cref="Message"/> and <see cref="ParamName"/> — e.g. to build a coded domain exception.
    /// </param>
    /// <remarks>
    /// Stamps <see cref="Code"/> and <see cref="ParamName"/> onto the thrown exception's
    /// <see cref="Exception.Data"/> — read them back via <see cref="PineGuard.GuardClauses.ExceptionExtension"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exceptionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception">Thrown as <typeparamref name="TException"/> when <see cref="Failed"/> is <see langword="true"/>.</exception>
    public void ThrowIfFailed<TException>(Func<IMustResult, TException> exceptionFactory)
        where TException : Exception
    {
        ThrowHelper.ThrowIfNull(exceptionFactory);

        if (!Failed) return;

        throw Stamp(exceptionFactory(this));
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the result represents a failure.
    /// </summary>
    /// <remarks>
    /// Stamps <see cref="Code"/> and <see cref="ParamName"/> onto the thrown exception's
    /// <see cref="Exception.Data"/> — read them back via <see cref="PineGuard.GuardClauses.ExceptionExtension"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <see cref="Failed"/> is <see langword="true"/>.
    /// </exception>
    public void ThrowNullIfFailed()
    {
        if (!Failed) return;

        throw Stamp(new ArgumentNullException(ParamName, Message));
    }

    private TException Stamp<TException>(TException exception)
        where TException : Exception
    {
        exception.Data[GuardFailure.CodeDataKey] = Code;
        exception.Data[GuardFailure.PropertyPathDataKey] = ParamName ?? string.Empty;

        return exception;
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
/// Extension methods for working with <see cref="MustResult{T}"/> instances: chaining, conditional
/// gating, and lifting into the object-level <see cref="MustValidationResult"/>.
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
    /// <see cref="MustResult{T}.Message"/> concatenates all failure messages separated by <c>"; "</c>,
    /// carrying the first failure's <see cref="MustResult{T}.Code"/> and <see cref="MustResult{T}.MessageTemplate"/>
    /// — a <see cref="MustResult{T}"/> has one slot for each, so only the first failure's identity survives.
    /// Prefer <see cref="MustValidationResult.From(IMustResult[])"/> or <see cref="ToMustValidationResult{T}"/>
    /// when every failure's code and path must be preserved.
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

        return MustResult<T>.FailPreformatted(first.Code, message, first.MessageTemplate, first.ParamName, first.Value);
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

    /// <summary>
    /// Chains a second check onto a successful result, propagating failure untouched.
    /// </summary>
    /// <typeparam name="T">The source result type.</typeparam>
    /// <typeparam name="TNext">The result type produced by <paramref name="next"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="next">The check to run against <see cref="MustResult{T}.Result"/> when <paramref name="result"/> succeeded.</param>
    /// <returns>
    /// <paramref name="result"/>'s failure (with its <see cref="MustResult{T}.Code"/>, <see cref="MustResult{T}.Message"/>,
    /// <see cref="MustResult{T}.MessageTemplate"/>, <see cref="MustResult{T}.ParamName"/> and <see cref="MustResult{T}.Value"/>
    /// carried across unchanged) when <paramref name="result"/> failed; otherwise, the result of <paramref name="next"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> or <paramref name="next"/> is <see langword="null"/>.</exception>
    public static MustResult<TNext> AndThen<T, TNext>(this MustResult<T> result, Func<T, MustResult<TNext>> next)
    {
        ThrowHelper.ThrowIfNull(result);
        ThrowHelper.ThrowIfNull(next);

        return result.Failed
            ? MustResult<TNext>.FailPreformatted(result.Code, result.Message, result.MessageTemplate, result.ParamName, result.Value)
            : next(result.Result!);
    }

    /// <summary>
    /// Keeps <paramref name="result"/> when <paramref name="condition"/> is <see langword="true"/>;
    /// otherwise returns a successful result carrying the same value.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="condition">Evaluated eagerly by the caller before this method runs.</param>
    /// <returns><paramref name="result"/> when <paramref name="condition"/> is <see langword="true"/>; otherwise a success.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static MustResult<T> When<T>(this MustResult<T> result, bool condition)
    {
        ThrowHelper.ThrowIfNull(result);

        return condition ? result : MustResult<T>.Ok(result.Result, result.Value, result.ParamName);
    }

    /// <summary>
    /// Keeps <paramref name="result"/> unless <paramref name="condition"/> is <see langword="true"/>;
    /// otherwise returns a successful result carrying the same value.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="condition">Evaluated eagerly by the caller before this method runs.</param>
    /// <returns>The complement of <see cref="When{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static MustResult<T> Unless<T>(this MustResult<T> result, bool condition) =>
        result.When(!condition);

    /// <summary>
    /// Losslessly lifts a single <see cref="MustResult{T}"/> into the object-level <see cref="MustValidationResult"/>.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="result">The result to lift.</param>
    /// <param name="propertyPath">
    /// The property path to attribute the failure to. When <see langword="null"/>, the failure is
    /// attributed to <see cref="IMustResult.ParamName"/> (or the root, <c>""</c>, when that is also <see langword="null"/>)
    /// and <see cref="MustFailure.Message"/> is <see cref="IMustResult.Message"/> as-is; when given, the message is
    /// <see cref="IMustResult.MessageTemplate"/> re-rendered against <paramref name="propertyPath"/>.
    /// </param>
    /// <returns><see cref="MustValidationResult.Ok"/> when <paramref name="result"/> succeeded; otherwise a single-failure result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public static MustValidationResult ToMustValidationResult<T>(this MustResult<T> result, string? propertyPath = null)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Success
            ? MustValidationResult.Ok()
            : MustValidationResult.Fail(MustFailure.From(result, propertyPath));
    }
}
