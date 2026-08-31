using PineGuard.Analyzers.CodeFixes;
using PG1001Data = PineGuard.Analyzers.UnitTests.PreferGuardAnalyzerTestData.PG1001;
using PG1002Data = PineGuard.Analyzers.UnitTests.PreferGuardAnalyzerTestData.PG1002;

namespace PineGuard.Analyzers.UnitTests;

public sealed class PreferGuardAnalyzerTests
{
    [Theory]
    [MemberData(nameof(PG1001Data.Cases), MemberType = typeof(PG1001Data))]
    public Task PG1001_ReportsOrStaysSilentAsExpected(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc);

    [Theory]
    [MemberData(nameof(PG1001Data.WithoutPineGuardReferenceCases), MemberType = typeof(PG1001Data))]
    public Task PG1001_StaysSilentWithoutAPineGuardReference(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, referencePineGuard: false);

    [Theory]
    [MemberData(nameof(PG1001Data.InsidePineGuardCases), MemberType = typeof(PG1001Data))]
    public Task PG1001_StaysSilentInsidePineGuardItself(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, assemblyName: AnalyzerVerifier.PineGuardAssemblyName);

    [Theory]
    [MemberData(nameof(PG1001Data.FixCases), MemberType = typeof(PG1001Data))]
    public Task PG1001_FixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(tc);

    [Theory]
    [MemberData(nameof(PG1001Data.FixAllCases), MemberType = typeof(PG1001Data))]
    public Task PG1001_FixAllFixesEveryOccurrenceAndAddsTheUsingOnce(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(
            tc,
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNull, 9, 9, "name"),
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNull, 11, 9, "address"));

    [Theory]
    [MemberData(nameof(PG1002Data.Cases), MemberType = typeof(PG1002Data))]
    public Task PG1002_ReportsOrStaysSilentAsExpected(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc);

    [Theory]
    [MemberData(nameof(PG1002Data.WithoutPineGuardReferenceCases), MemberType = typeof(PG1002Data))]
    public Task PG1002_StaysSilentWithoutAPineGuardReference(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, referencePineGuard: false);

    [Theory]
    [MemberData(nameof(PG1002Data.InsidePineGuardCases), MemberType = typeof(PG1002Data))]
    public Task PG1002_StaysSilentInsidePineGuardItself(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, assemblyName: AnalyzerVerifier.PineGuardAssemblyName);

    [Theory]
    [MemberData(nameof(PG1002Data.FixCases), MemberType = typeof(PG1002Data))]
    public Task PG1002_FixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(tc);

    [Theory]
    [MemberData(nameof(PG1002Data.FixAllCases), MemberType = typeof(PG1002Data))]
    public Task PG1002_FixAllFixesEveryOccurrenceAndAddsTheUsingOnce(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(
            tc,
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNullOrWhiteSpace, 9, 9, "name"),
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNullOrWhiteSpace, 11, 9, "address"));
}
