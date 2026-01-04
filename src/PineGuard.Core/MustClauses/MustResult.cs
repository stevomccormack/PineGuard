namespace PineGuard.MustClauses;

public sealed class MustResult<T>
{
    public bool Success { get; }
    public bool Failed => !Success;

    public string Message { get; }
    public string? ParamName { get; }

    /// <summary>
    /// The original value that was validated/parsed.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// The typed result produced by the operation when <see cref="Success"/> is true.
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

    public static MustResult<T> Ok(T result, object? value = null, string? paramName = null) =>
        new(true, string.Empty, paramName, value, result);

    public static MustResult<T> Fail(string messageTemplate, string? paramName, object? value) =>
        new(false, FormatMessage(messageTemplate, paramName), paramName, value, default);

    public static implicit operator bool(MustResult<T> mustResult) => mustResult.Success;

    public void Deconstruct(out bool success, out string message, out string? paramName, out object? value, out T? result)
    {
        success = Success;
        message = Message;
        paramName = ParamName;
        value = Value;
        result = Result;
    }

    public static MustResult<T> FromBool(bool ok, string messageTemplate, string? paramName, object? value, T? result) =>
        ok ? Ok(result!, value, paramName) : Fail(messageTemplate, paramName, value);

    public static MustResult<T> FromBool(bool ok, string messageTemplate, string? paramName, object? value) =>
        FromBool(ok, messageTemplate, paramName, value, result: default);

    internal static string FormatMessage(string messageTemplate, string? paramName) =>
        string.IsNullOrEmpty(paramName)
            ? messageTemplate
            : messageTemplate.Replace("{paramName}", paramName, StringComparison.Ordinal);

    public void ThrowIfFailed()
    {
        if (Failed) throw new ArgumentException(Message, ParamName);
    }

    public void ThrowNullIfFailed()
    {
        if (Failed) throw new ArgumentNullException(ParamName, Message);
    }

    public void ThrowIfFailed<TException>(Func<string, string?, TException> exceptionFactory)
        where TException : Exception
    {
        if (Failed) throw exceptionFactory(Message, ParamName);
    }

    public T OrThrow()
    {
        ThrowIfFailed();
        return Result!;
    }

    public T OrThrow(T fallback)
    {
        ThrowIfFailed();
        return Result is null ? fallback : Result;
    }
}

public static class MustResultExtension
{
    public static MustResult<T> Combine<T>(this IEnumerable<MustResult<T>> results)
    {
        if (results is null)
            return MustResult<T>.Fail("{paramName} must not be null.", nameof(results), results);

        var failures = results.Where(r => r.Failed).ToList();
        if (failures.Count == 0)
        {
            // No failures; return the first successful result if present, otherwise default.
            foreach (var r in results)
            {
                if (r.Success) return r;
            }

            return MustResult<T>.Ok(result: default!, value: null);
        }

        var first = failures[0];
        var message = string.Join("; ", failures.Select(f => f.Message));

        return MustResult<T>.Fail(message, first.ParamName, first.Value);
    }

    public static void ThrowIfAnyFailed<T>(this IEnumerable<MustResult<T>> results) =>
        results.Combine().ThrowIfFailed();
}
