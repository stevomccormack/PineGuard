using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentResults;

/// <summary>
/// A <c>FluentResults.Error</c> that keeps a PineGuard failure whole: the rule's stable
/// <see cref="Code"/> and the <see cref="PropertyPath"/> it was found at, alongside the rendered message.
/// </summary>
/// <remarks>
/// Both values are mirrored into <c>Metadata</c> under <see cref="CodeMetadataKey"/> and
/// <see cref="PropertyPathMetadataKey"/>, so error handling that only knows <c>IError</c> still reads
/// them without a cast.
/// </remarks>
/// <seealso cref="FluentResultsExtension"/>
/// <seealso cref="MustFailure"/>
public sealed class MustError : global::FluentResults.Error
{
    /// <summary>
    /// The metadata key <see cref="Code"/> is mirrored under.
    /// </summary>
    /// <remarks>
    /// camelCase because metadata bags are wire-shaped, matching every other PineGuard bridge.
    /// </remarks>
    public const string CodeMetadataKey = "code";

    /// <summary>
    /// The metadata key <see cref="PropertyPath"/> is mirrored under.
    /// </summary>
    /// <remarks>
    /// camelCase because metadata bags are wire-shaped, matching every other PineGuard bridge.
    /// </remarks>
    public const string PropertyPathMetadataKey = "propertyPath";

    /// <summary>
    /// Initializes a new instance of the <see cref="MustError"/> class.
    /// </summary>
    /// <param name="code">The stable, machine-readable identity of the rule that failed.</param>
    /// <param name="propertyPath">Where in the validated object the failure is, or <see cref="string.Empty"/> for the root.</param>
    /// <param name="message">The rendered, human-readable failure message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="code"/>, <paramref name="propertyPath"/> or <paramref name="message"/> is <see langword="null"/>.
    /// </exception>
    public MustError(string code, string propertyPath, string message)
        : base(message)
    {
        ThrowHelper.ThrowIfNull(code);
        ThrowHelper.ThrowIfNull(propertyPath);
        ThrowHelper.ThrowIfNull(message);

        Code = code;
        PropertyPath = propertyPath;

        WithMetadata(CodeMetadataKey, code);
        WithMetadata(PropertyPathMetadataKey, propertyPath);
    }

    /// <summary>
    /// Gets the stable, machine-readable identity of the rule that failed.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets where in the validated object the failure is (e.g. <c>Email</c>, <c>Address.City</c>,
    /// <c>Lines[2].Sku</c>), or <see cref="string.Empty"/> for the root.
    /// </summary>
    public string PropertyPath { get; }

    /// <summary>
    /// Builds a <see cref="MustError"/> from a failed <see cref="IMustResult"/>.
    /// </summary>
    /// <param name="result">The failed result to convert.</param>
    /// <returns>A <see cref="MustError"/> carrying <paramref name="result"/>'s code, message and parameter name.</returns>
    /// <remarks>
    /// Deliberately strict: a success has no code, no message and nothing to report, so converting one
    /// is a programmer error rather than an empty error.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="result"/> represents a success.</exception>
    public static MustError From(IMustResult result) => From(MustFailure.From(result));

    /// <summary>
    /// Builds a <see cref="MustError"/> from a single <see cref="MustFailure"/>.
    /// </summary>
    /// <param name="failure">The failure to convert.</param>
    /// <returns>A <see cref="MustError"/> carrying <paramref name="failure"/>'s code, property path and message.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public static MustError From(MustFailure failure)
    {
        ThrowHelper.ThrowIfNull(failure);

        return new MustError(failure.Code, failure.PropertyPath, failure.Message);
    }
}
