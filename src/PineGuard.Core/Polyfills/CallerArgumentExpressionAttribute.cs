#if !NET8_0_OR_GREATER
namespace System.Runtime.CompilerServices;

/// <summary>
/// A netstandard2.1 polyfill of the BCL <c>System.Runtime.CompilerServices.CallerArgumentExpressionAttribute</c>,
/// used to capture the source-code expression passed for another parameter. Only compiled when targeting
/// frameworks below net8.0 (the attribute is provided by the runtime on net8.0 and later).
/// </summary>
/// <param name="parameterName">The name of the parameter whose caller expression should be captured.</param>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    /// <summary>
    /// Gets the name of the parameter whose caller expression is captured.
    /// </summary>
    public string ParameterName { get; } = parameterName;
}
#endif
