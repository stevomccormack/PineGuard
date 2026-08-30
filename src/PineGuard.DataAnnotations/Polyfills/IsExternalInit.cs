#if !NET8_0_OR_GREATER
#pragma warning disable IDE0130
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130

/// <summary>
/// A netstandard2.1 polyfill of the BCL <c>System.Runtime.CompilerServices.IsExternalInit</c>, the marker type
/// the C# compiler requires to emit an <c>init</c> accessor. Only compiled when targeting frameworks below
/// net8.0 (the type is provided by the runtime on net8.0 and later).
/// </summary>
/// <remarks>
/// Internal, for the same reason as the identical polyfill in <c>PineGuard.Core</c>: the compiler recognizes
/// this type by its fully-qualified name regardless of accessibility, and keeping it internal prevents it from
/// being exported as part of this assembly's public API surface, where it would collide with the real BCL type
/// for any netcoreapp3.0-net7.0 consumer resolving the netstandard2.1 asset. This project needs its own copy
/// because Core's is internal and grants friend access only to <c>PineGuard.MustClauses</c> and
/// <c>PineGuard.GuardClauses</c>.
/// </remarks>
internal static class IsExternalInit;
#endif
