using PineGuard.Analyzers.CodeFixes;
using PG2001Data = PineGuard.Analyzers.UnitTests.DiscardedMustResultAnalyzerTestData.PG2001;
using PG2002Data = PineGuard.Analyzers.UnitTests.DiscardedMustResultAnalyzerTestData.PG2002;

namespace PineGuard.Analyzers.UnitTests;

public sealed class DiscardedMustResultAnalyzerTests
{
    [Theory]
    [MemberData(nameof(PG2001Data.Cases), MemberType = typeof(PG2001Data))]
    public Task PG2001_ReportsOrStaysSilentAsExpected(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc);

    [Theory]
    [MemberData(nameof(PG2001Data.WithoutPineGuardReferenceCases), MemberType = typeof(PG2001Data))]
    public Task PG2001_StaysSilentWithoutAPineGuardReference(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc, referencePineGuard: false);

    [Theory]
    [MemberData(nameof(PG2001Data.InsidePineGuardCases), MemberType = typeof(PG2001Data))]
    public Task PG2001_StaysSilentInsidePineGuardItself(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc, assemblyName: AnalyzerVerifier.PineGuardAssemblyName);

    [Theory]
    [MemberData(nameof(PG2001Data.ThrowIfFailedFixCases), MemberType = typeof(PG2001Data))]
    public Task PG2001_ThrowIfFailedFixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.ThrowIfFailedEquivalenceKey);

    [Theory]
    [MemberData(nameof(PG2001Data.AssignResultFixCases), MemberType = typeof(PG2001Data))]
    public Task PG2001_AssignResultFixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.AssignResultEquivalenceKey);

    [Theory]
    [MemberData(nameof(PG2001Data.ThrowIfFailedFixAllCases), MemberType = typeof(PG2001Data))]
    public Task PG2001_FixAllChainsThrowIfFailedOntoEveryOccurrence(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.ThrowIfFailedEquivalenceKey,
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.DiscardedMustResult, 9, 9, "NotNull"),
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.DiscardedMustResult, 10, 9, "NotNull"));

    [Theory]
    [MemberData(nameof(PG2002Data.Cases), MemberType = typeof(PG2002Data))]
    public Task PG2002_ReportsOrStaysSilentAsExpected(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc);

    [Theory]
    [MemberData(nameof(PG2002Data.WithoutPineGuardReferenceCases), MemberType = typeof(PG2002Data))]
    public Task PG2002_StaysSilentWithoutAPineGuardReference(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc, referencePineGuard: false);

    [Theory]
    [MemberData(nameof(PG2002Data.InsidePineGuardCases), MemberType = typeof(PG2002Data))]
    public Task PG2002_StaysSilentInsidePineGuardItself(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<DiscardedMustResultAnalyzer>(tc, assemblyName: AnalyzerVerifier.PineGuardAssemblyName);

    [Theory]
    [MemberData(nameof(PG2002Data.ThrowIfFailedFixCases), MemberType = typeof(PG2002Data))]
    public Task PG2002_ThrowIfFailedFixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.ThrowIfFailedEquivalenceKey);

    [Theory]
    [MemberData(nameof(PG2002Data.AssignResultFixCases), MemberType = typeof(PG2002Data))]
    public Task PG2002_AssignResultFixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.AssignResultEquivalenceKey);

    [Theory]
    [MemberData(nameof(PG2002Data.ThrowIfFailedFixAllCases), MemberType = typeof(PG2002Data))]
    public Task PG2002_FixAllChainsThrowIfFailedOntoEveryOccurrence(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<DiscardedMustResultAnalyzer, DiscardedMustResultCodeFixProvider>(
            tc,
            DiscardedMustResultCodeFixProvider.ThrowIfFailedEquivalenceKey,
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.DiscardedMustValidationResult, 13, 9, "Validate"),
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.DiscardedMustValidationResult, 14, 9, "Validate"));
}
