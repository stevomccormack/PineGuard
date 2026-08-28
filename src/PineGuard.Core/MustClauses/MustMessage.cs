namespace PineGuard.MustClauses;

/// <summary>
/// Shared <c>{paramName}</c> placeholder substitution used by <see cref="MustResult{T}"/>,
/// <see cref="MustFailure"/> and <see cref="MustValidator{T}"/>, so the rendering rule lives in
/// exactly one place regardless of which type is re-rendering a stored <c>MessageTemplate</c>.
/// </summary>
internal static class MustMessage
{
    private const string ParamNameToken = "{paramName}";

    /// <summary>
    /// Substitutes <paramref name="paramName"/> into <paramref name="messageTemplate"/>, or returns the
    /// template unchanged when <paramref name="paramName"/> is <see langword="null"/> or empty.
    /// </summary>
    public static string Format(string messageTemplate, string? paramName) =>
        string.IsNullOrEmpty(paramName)
            ? messageTemplate
            : messageTemplate.Replace(ParamNameToken, paramName, StringComparison.Ordinal);
}
