#if !NET8_0_OR_GREATER
namespace System.Runtime.CompilerServices;

/// <summary>
/// A netstandard2.1 polyfill of the BCL <c>System.Runtime.CompilerServices.CallerArgumentExpressionAttribute</c>,
/// used to capture the source-code expression passed for another parameter. Only compiled when targeting
/// frameworks below net8.0 (the attribute is provided by the runtime on net8.0 and later).
/// </summary>
/// <remarks>
/// Internal, matching the <see cref="IsExternalInit"/> polyfill beside it: the C# compiler recognizes this
/// well-known attribute by its fully-qualified name regardless of accessibility, so callers never need to see
/// the type itself. Keeping it internal (rather than public) prevents it from being exported as part of this
/// assembly's public API surface, where it would collide with the real BCL type for any netcoreapp3.0-net7.0
/// consumer resolving the netstandard2.1 asset. <c>PineGuard.MustClauses</c> and <c>PineGuard.GuardClauses</c>
/// apply <c>[CallerArgumentExpression]</c> on their own netstandard2.1 builds and are granted friend access via
/// <c>InternalsVisibleTo</c> in this project's csproj so they can still see this type at compile time.
/// </remarks>
/// <param name="parameterName">The name of the parameter whose caller expression should be captured.</param>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    /// <summary>
    /// Gets the name of the parameter whose caller expression is captured.
    /// </summary>
    public string ParameterName { get; } = parameterName;
}
#endif
