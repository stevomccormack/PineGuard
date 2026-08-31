using Microsoft.CodeAnalysis;

namespace PineGuard.Analyzers;

/// <summary>
/// The <see cref="DiagnosticDescriptor"/> instances behind every <see cref="DiagnosticIds"/> entry.
/// </summary>
internal static class DiagnosticDescriptors
{
    /// <summary>
    /// The category reported for a <c>PG1xxx</c> "prefer a guard clause" suggestion.
    /// </summary>
    internal const string UsageCategory = "Usage";

    private const string HelpLinkPrefix = "https://pineguard.ai/docs/analyzers/";

    /// <summary>
    /// PG1001 — suggests <c>Guard.Against.Null</c> in place of a hand-rolled null check.
    /// </summary>
    internal static readonly DiagnosticDescriptor UseGuardAgainstNull = new(
        DiagnosticIds.UseGuardAgainstNull,
        "Use Guard.Against.Null",
        "Replace this null check with Guard.Against.Null({0})",
        UsageCategory,
        DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "A hand-rolled null check throws the ArgumentNullException that Guard.Against.Null already throws, and the guard captures the parameter name for you.",
        helpLinkUri: HelpLinkPrefix + DiagnosticIds.UseGuardAgainstNull);

    /// <summary>
    /// PG1002 — suggests <c>Guard.Against.NullOrWhiteSpace</c> in place of a hand-rolled
    /// null-or-whitespace check.
    /// </summary>
    internal static readonly DiagnosticDescriptor UseGuardAgainstNullOrWhiteSpace = new(
        DiagnosticIds.UseGuardAgainstNullOrWhiteSpace,
        "Use Guard.Against.NullOrWhiteSpace",
        "Replace this null-or-whitespace check with Guard.Against.NullOrWhiteSpace({0})",
        UsageCategory,
        DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "A hand-rolled string.IsNullOrWhiteSpace check throws the ArgumentException that Guard.Against.NullOrWhiteSpace already throws, and the guard captures the parameter name for you.",
        helpLinkUri: HelpLinkPrefix + DiagnosticIds.UseGuardAgainstNullOrWhiteSpace);
}
