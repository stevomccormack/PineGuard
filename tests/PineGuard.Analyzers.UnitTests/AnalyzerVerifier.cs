using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace PineGuard.Analyzers.UnitTests;

/// <summary>
/// Builds the Roslyn analyzer and code-fix test harnesses with the PineGuard reference set every
/// snippet compiles against.
/// </summary>
public static class AnalyzerVerifier
{
    /// <summary>
    /// An assembly name that trips the "never report inside PineGuard itself" suppression.
    /// </summary>
    public const string PineGuardAssemblyName = "PineGuard.Core";

    private static readonly MetadataReference[] PineGuardReferences =
    [
        MetadataReference.CreateFromFile(typeof(GuardClauses.Guard).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(GuardClauses.GuardNullClauses).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(MustClauses.MustNullClauses).Assembly.Location)
    ];

    // The snippets are compiled against the same framework the test host runs on, so the PineGuard
    // assemblies loaded above (net8.0 on one leg, net10.0 on the other) never out-version the
    // reference assemblies they are placed beside.
    private static ReferenceAssemblies TargetFrameworkReferences =>
#if NET10_0_OR_GREATER
        ReferenceAssemblies.Net.Net100;
#else
        ReferenceAssemblies.Net.Net80;
#endif

    /// <summary>
    /// Runs <typeparamref name="TAnalyzer"/> over <paramref name="tc"/> and asserts it reports
    /// exactly what the case expects.
    /// </summary>
    /// <typeparam name="TAnalyzer">The analyzer under test.</typeparam>
    /// <param name="tc">The case holding the source and its expectation.</param>
    /// <param name="referencePineGuard">Whether the compilation may resolve PineGuard's types.</param>
    /// <param name="assemblyName">The assembly name to compile the snippet under.</param>
    public static async Task AnalyzeAsync<TAnalyzer>(AnalyzerCase tc, bool referencePineGuard = true, string? assemblyName = null)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
        {
            TestCode = Normalize(tc.Source),
            ReferenceAssemblies = TargetFrameworkReferences
        };

        Configure(test, test.TestState, referencePineGuard, assemblyName);
        Expect(test.TestState.ExpectedDiagnostics, tc.Expected);

        await test.RunAsync();
    }

    /// <summary>
    /// Runs <typeparamref name="TCodeFix"/> over <paramref name="tc"/> and asserts the fixed source
    /// matches <see cref="AnalyzerExpected.FixedSource"/>.
    /// </summary>
    /// <typeparam name="TAnalyzer">The analyzer that reports the diagnostic.</typeparam>
    /// <typeparam name="TCodeFix">The code-fix provider under test.</typeparam>
    /// <param name="tc">The case holding the source, the diagnostic and the fixed source.</param>
    /// <param name="expectedDiagnostics">Every diagnostic the source reports, when the case reports more than one.</param>
    public static async Task FixAsync<TAnalyzer, TCodeFix>(AnalyzerCase tc, params DiagnosticResult[] expectedDiagnostics)
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        var test = new CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
        {
            TestCode = Normalize(tc.Source),
            FixedCode = Normalize(tc.Expected.FixedSource!),
            ReferenceAssemblies = TargetFrameworkReferences
        };

        Configure(test, test.TestState, referencePineGuard: true, assemblyName: null);

        if (expectedDiagnostics.Length == 0)
            Expect(test.TestState.ExpectedDiagnostics, tc.Expected);
        else
            test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);

        await test.RunAsync();
    }

    /// <summary>
    /// Builds the expected <c>PG</c> diagnostic for a case that reports one.
    /// </summary>
    /// <param name="descriptor">The descriptor the analyzer reports.</param>
    /// <param name="line">The one-based line of the report.</param>
    /// <param name="column">The one-based column of the report.</param>
    /// <param name="identifier">The guarded identifier named in the message.</param>
    /// <returns>The expected diagnostic.</returns>
    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor, int line, int column, string identifier) =>
        new DiagnosticResult(descriptor).WithLocation(line, column).WithArguments(identifier);

    private static void Configure(AnalyzerTest<DefaultVerifier> test, SolutionState state, bool referencePineGuard, string? assemblyName)
    {
        if (referencePineGuard)
            state.AdditionalReferences.AddRange(PineGuardReferences);

        if (assemblyName is not null)
            test.SolutionTransforms.Add((solution, projectId) => solution.WithProjectAssemblyName(projectId, assemblyName));
    }

    private static void Expect(List<DiagnosticResult> expected, AnalyzerExpected analyzerExpected)
    {
        if (analyzerExpected.IsValid)
            return;

        var diagnostic = new DiagnosticResult(analyzerExpected.DiagnosticId!, DiagnosticSeverity.Info)
            .WithLocation(analyzerExpected.Line!.Value, analyzerExpected.Column!.Value)
            .WithMessage(analyzerExpected.Message);

        expected.Add(diagnostic);
    }

    // Snippets are written as raw string literals, so their line endings are whatever git checked
    // the test file out with. The code fix emits Environment.NewLine, so both sides are pinned to
    // it and the comparison holds on Windows and Linux alike.
    private static string Normalize(string source) =>
        source.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
}
