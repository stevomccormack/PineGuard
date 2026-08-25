#if !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// A netstandard2.1 polyfill of the BCL <c>System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes</c>,
/// used to describe which members of a type are dynamically accessed (e.g. via reflection) so trimming tools
/// can preserve them. Only compiled when targeting frameworks below net5.0 (the type is provided by the
/// runtime on net5.0 and later, including net8.0 and net10.0).
/// </summary>
[Flags]
internal enum DynamicallyAccessedMemberTypes
{
    None = 0,
    PublicParameterlessConstructor = 0x0001,
    PublicConstructors = 0x0002 | PublicParameterlessConstructor,
    NonPublicConstructors = 0x0004,
    PublicMethods = 0x0008,
    NonPublicMethods = 0x0010,
    PublicFields = 0x0020,
    NonPublicFields = 0x0040,
    PublicNestedTypes = 0x0080,
    NonPublicNestedTypes = 0x0100,
    PublicProperties = 0x0200,
    NonPublicProperties = 0x0400,
    PublicEvents = 0x0800,
    NonPublicEvents = 0x1000,
    Interfaces = 0x2000,
    All = ~None,
}

/// <summary>
/// A netstandard2.1 polyfill of the BCL <c>System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute</c>,
/// used to mark a type, generic parameter, field, property, parameter, or return value as being dynamically
/// accessed (e.g. via reflection) so trimming tools preserve the described members. Only compiled when
/// targeting frameworks below net5.0 (the attribute is provided by the runtime on net5.0 and later).
/// </summary>
/// <remarks>
/// Internal, matching the <see cref="System.Runtime.CompilerServices.IsExternalInit"/> and
/// <see cref="System.Runtime.CompilerServices.CallerArgumentExpressionAttribute"/> polyfills beside it: trim
/// tooling recognizes this well-known attribute by its fully-qualified name regardless of accessibility, so
/// it never needs to be part of this assembly's public API surface.
/// </remarks>
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter |
    AttributeTargets.Parameter | AttributeTargets.Property,
    Inherited = false)]
internal sealed class DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes) : Attribute
{
    public DynamicallyAccessedMemberTypes MemberTypes { get; } = memberTypes;
}
#endif
