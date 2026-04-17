namespace PineGuard.GuardClauses;

/// <summary>
/// Configuration options for <see cref="GuardExceptionPolicy"/> scopes.
/// </summary>
/// <seealso cref="GuardExceptionPolicy"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public sealed class GuardExceptionPolicyOptions
{
    /// <summary>
    /// Gets or sets a factory that maps a default exception to a custom replacement exception.
    /// If <see langword="null"/>, the default exception is thrown unmodified (subject to <see cref="ReplaceDefaultExceptions"/>).
    /// </summary>
    public Func<Exception, Exception>? ExceptionReplacer { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the exception replacer should apply to
    /// <see cref="ArgumentException"/> and <see cref="ArgumentNullException"/> as well as other exception types.
    /// </summary>
    /// <remarks>
    /// When <see langword="false"/> (the default), the replacer is skipped for the built-in
    /// <see cref="ArgumentException"/> / <see cref="ArgumentNullException"/> types. Set to
    /// <see langword="true"/> to override all exception types uniformly.
    /// </remarks>
    public bool ReplaceDefaultExceptions { get; set; }
}
