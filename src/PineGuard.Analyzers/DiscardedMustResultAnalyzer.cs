using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace PineGuard.Analyzers;

/// <summary>
/// Reports a PineGuard validation call whose result is thrown away, which checks nothing.
/// </summary>
/// <remarks>
/// Neither a Must clause nor a validator throws on its own — each hands back a result to inspect.
/// Calling one as a statement is therefore a silent no-op, and the compiler has nothing to say about
/// it. The analyzer stays silent unless the compilation references PineGuard's Must clauses, and
/// never reports inside PineGuard's own assemblies.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/analyzers">PineGuard analyzers documentation</seealso>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DiscardedMustResultAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.DiscardedMustResult, DiagnosticDescriptors.DiscardedMustValidationResult);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(OnCompilationStart);
    }

    private static void OnCompilationStart(CompilationStartAnalysisContext context)
    {
        if (PineGuardTypes.IsPineGuardAssembly(context.Compilation))
            return;

        var types = PineGuardTypes.From(context.Compilation);
        if (!types.CanReportDiscardedResults)
            return;

        context.RegisterOperationAction(operation => AnalyzeExpressionStatement(operation, types), OperationKind.ExpressionStatement);
    }

    /// <summary>
    /// Reports a call statement whose return value is one of PineGuard's result types.
    /// </summary>
    /// <remarks>
    /// Only a bare call is reported. Assigning the result, discarding it with <c>_ =</c>, returning
    /// it or awaiting it all produce a different operation, and each of those is a deliberate act
    /// the analyzer has no business second-guessing.
    /// </remarks>
    private static void AnalyzeExpressionStatement(OperationAnalysisContext context, PineGuardTypes types)
    {
        var statement = (IExpressionStatementOperation)context.Operation;
        if (statement.Operation is not IInvocationOperation invocation)
            return;

        var descriptor = GetDiscardedResultDescriptor(invocation.TargetMethod.ReturnType.OriginalDefinition, types);
        if (descriptor is null)
            return;

        context.ReportDiagnostic(Diagnostic.Create(descriptor, statement.Syntax.GetLocation(), invocation.TargetMethod.Name));
    }

    /// <summary>
    /// Maps the return type of a discarded call onto the diagnostic it warrants.
    /// </summary>
    /// <param name="returnType">The original definition of the called method's return type.</param>
    /// <param name="types">The well-known types resolved for this compilation.</param>
    /// <returns>The descriptor to report, or <see langword="null"/> when the call returns something PineGuard does not own.</returns>
    /// <remarks>
    /// A well-known type the compilation could not resolve is <see langword="null"/>, which no return
    /// type equals — so a compilation that references only one of the two result types can only ever
    /// be reported for that one.
    /// </remarks>
    private static DiagnosticDescriptor? GetDiscardedResultDescriptor(ITypeSymbol returnType, PineGuardTypes types)
    {
        if (SymbolEqualityComparer.Default.Equals(returnType, types.MustResult))
            return DiagnosticDescriptors.DiscardedMustResult;

        if (SymbolEqualityComparer.Default.Equals(returnType, types.MustValidationResult))
            return DiagnosticDescriptors.DiscardedMustValidationResult;

        return null;
    }
}
