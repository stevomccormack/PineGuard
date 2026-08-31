using PineGuard.Analyzers.CodeFixes;

namespace PineGuard.Analyzers.UnitTests;

public sealed class PreferGuardAnalyzerTests
{
    [Theory]
    [MemberData(nameof(PreferGuardAnalyzerTestData.PG1001.Cases), MemberType = typeof(PreferGuardAnalyzerTestData.PG1001))]
    public Task ReportsOrStaysSilentAsExpected(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc);

    [Theory]
    [MemberData(nameof(PreferGuardAnalyzerTestData.PG1001.WithoutPineGuardReferenceCases), MemberType = typeof(PreferGuardAnalyzerTestData.PG1001))]
    public Task StaysSilentWithoutAPineGuardReference(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, referencePineGuard: false);

    [Theory]
    [MemberData(nameof(PreferGuardAnalyzerTestData.PG1001.InsidePineGuardCases), MemberType = typeof(PreferGuardAnalyzerTestData.PG1001))]
    public Task StaysSilentInsidePineGuardItself(AnalyzerCase tc) =>
        AnalyzerVerifier.AnalyzeAsync<PreferGuardAnalyzer>(tc, assemblyName: AnalyzerVerifier.PineGuardAssemblyName);

    [Theory]
    [MemberData(nameof(PreferGuardAnalyzerTestData.PG1001.FixCases), MemberType = typeof(PreferGuardAnalyzerTestData.PG1001))]
    public Task FixesToTheExpectedSource(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(tc);

    [Theory]
    [MemberData(nameof(PreferGuardAnalyzerTestData.PG1001.FixAllCases), MemberType = typeof(PreferGuardAnalyzerTestData.PG1001))]
    public Task FixAllFixesEveryOccurrenceAndAddsTheUsingOnce(AnalyzerCase tc) =>
        AnalyzerVerifier.FixAsync<PreferGuardAnalyzer, PreferGuardCodeFixProvider>(
            tc,
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNull, 9, 9, "name"),
            AnalyzerVerifier.Diagnostic(DiagnosticDescriptors.UseGuardAgainstNull, 11, 9, "address"));
}
