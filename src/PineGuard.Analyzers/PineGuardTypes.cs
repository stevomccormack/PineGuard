using Microsoft.CodeAnalysis;

namespace PineGuard.Analyzers;

/// <summary>
/// The well-known types a PineGuard analyzer needs, resolved once per compilation.
/// </summary>
/// <remarks>
/// Resolving through <see cref="Compilation.GetTypeByMetadataName(string)"/> is what keeps the
/// package quiet: a project that has not referenced PineGuard cannot resolve
/// <see cref="GuardMetadataName"/>, so no <c>PG1xxx</c> diagnostic is ever registered.
/// </remarks>
internal sealed class PineGuardTypes
{
    /// <summary>
    /// The metadata name of the <c>Guard.Against</c> entry point.
    /// </summary>
    internal const string GuardMetadataName = "PineGuard.GuardClauses.Guard";

    /// <summary>
    /// The metadata name of the exception every <c>PG1001</c> shape throws.
    /// </summary>
    internal const string ArgumentNullExceptionMetadataName = "System.ArgumentNullException";

    /// <summary>
    /// The metadata name of the exception the <c>PG1002</c> and <c>PG1003</c> shapes throw.
    /// </summary>
    internal const string ArgumentExceptionMetadataName = "System.ArgumentException";

    private const string PineGuardAssemblyNamePrefix = "PineGuard.";

    private PineGuardTypes(INamedTypeSymbol? guard, INamedTypeSymbol? argumentNullException, INamedTypeSymbol? argumentException)
    {
        Guard = guard;
        ArgumentNullException = argumentNullException;
        ArgumentException = argumentException;
    }

    /// <summary>
    /// Gets <c>PineGuard.GuardClauses.Guard</c>, or <see langword="null"/> when the compilation
    /// does not reference PineGuard.GuardClauses.
    /// </summary>
    internal INamedTypeSymbol? Guard { get; }

    /// <summary>
    /// Gets <see cref="System.ArgumentNullException"/> as the compilation sees it.
    /// </summary>
    internal INamedTypeSymbol? ArgumentNullException { get; }

    /// <summary>
    /// Gets <see cref="System.ArgumentException"/> as the compilation sees it.
    /// </summary>
    internal INamedTypeSymbol? ArgumentException { get; }

    /// <summary>
    /// Gets a value indicating whether guard-clause suggestions may be reported at all.
    /// </summary>
    internal bool CanSuggestGuardClauses => Guard is not null;

    /// <summary>
    /// Resolves the well-known types for <paramref name="compilation"/>.
    /// </summary>
    /// <param name="compilation">The compilation under analysis.</param>
    /// <returns>The resolved well-known types.</returns>
    internal static PineGuardTypes From(Compilation compilation) => new(
        compilation.GetTypeByMetadataName(GuardMetadataName),
        compilation.GetTypeByMetadataName(ArgumentNullExceptionMetadataName),
        compilation.GetTypeByMetadataName(ArgumentExceptionMetadataName));

    /// <summary>
    /// Determines whether <paramref name="compilation"/> is one of PineGuard's own assemblies.
    /// </summary>
    /// <param name="compilation">The compilation under analysis.</param>
    /// <returns><see langword="true"/> when the assembly is part of PineGuard itself.</returns>
    /// <remarks>
    /// PineGuard.Core's own <c>ThrowHelper</c> is precisely the pattern <c>PG1001</c> targets, and
    /// it has to stay — the library cannot guard itself with the guard it defines.
    /// </remarks>
    internal static bool IsPineGuardAssembly(Compilation compilation) =>
        compilation.Assembly.Name.StartsWith(PineGuardAssemblyNamePrefix, StringComparison.Ordinal);
}
