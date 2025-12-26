namespace PineGuard.MustClauses;

public sealed class MustResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public string Message { get; }
    public string? ParamName { get; }
    public object? Value { get; }

    private MustResult(bool isSuccess, string message, string? paramName, object? value)
    {
        IsSuccess = isSuccess;
        Message = message;
        ParamName = paramName;
        Value = value;
    }

    public static MustResult Ok()
        => new(true, string.Empty, null, null);

    public static MustResult Fail(
        string messageTemplate,
        string? paramName,
        object? value)
        => new(
            false,
            FormatMessage(messageTemplate, paramName),
            paramName,
            value);

    public static MustResult FromBool(
        bool ok,
        string messageTemplate,
        string? paramName,
        object? value)
        => ok
            ? Ok()
            : Fail(messageTemplate, paramName, value);

    public void ThrowIfFailed()
    {
        if (IsFailure)
            throw new ArgumentException(Message, ParamName);
    }

    public void ThrowNullIfFailed()
    {
        if (IsFailure)
            throw new ArgumentNullException(ParamName, Message);
    }

    public void ThrowIfFailed<TException>(Func<string, string?, TException> exceptionFactory)
        where TException : Exception
    {
        if (IsFailure)
            throw exceptionFactory(Message, ParamName);
    }

    public T OrThrow<T>(T value)
    {
        ThrowIfFailed();
        return value;
    }

    private static string FormatMessage(string template, string? paramName)
        => string.IsNullOrWhiteSpace(paramName)
            ? template.Replace("", "Value")
            : template.Replace("", $"'{paramName}'", StringComparison.OrdinalIgnoreCase);
}

public static class MustResultExtensions
{
    public static MustResult Combine(this IEnumerable<MustResult> results)
    {
        var failures = results.Where(r => r.IsFailure).ToList();

        if (failures.Count == 0)
            return MustResult.Ok();

        var message = string.Join("; ", failures.Select(f => f.Message));

        return MustResult.Fail(message, failures.First().ParamName, failures.First().Value);
    }

    public static void ThrowIfAnyFailed(this IEnumerable<MustResult> results)
        => results.Combine().ThrowIfFailed();
}
