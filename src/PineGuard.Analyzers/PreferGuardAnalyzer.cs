using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace PineGuard.Analyzers;

/// <summary>
/// Reports hand-rolled argument checks that a PineGuard guard clause already expresses.
/// </summary>
/// <remarks>
/// The analyzer stays silent unless the compilation references PineGuard.GuardClauses, and never
/// reports inside PineGuard's own assemblies.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/analyzers">PineGuard analyzers documentation</seealso>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class PreferGuardAnalyzer : DiagnosticAnalyzer
{
    private const string ThrowIfNullMethodName = "ThrowIfNull";

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.UseGuardAgainstNull);

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
        if (!types.CanSuggestGuardClauses)
            return;

        context.RegisterSyntaxNodeAction(node => AnalyzeIfStatement(node, types), SyntaxKind.IfStatement);
        context.RegisterSyntaxNodeAction(node => AnalyzeThrowExpression(node, types), SyntaxKind.ThrowExpression);
        context.RegisterSyntaxNodeAction(node => AnalyzeInvocation(node, types), SyntaxKind.InvocationExpression);
    }

    /// <summary>
    /// Handles <c>if (x is null) throw new ArgumentNullException(nameof(x));</c>.
    /// </summary>
    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        var ifStatement = (IfStatementSyntax)context.Node;
        if (ifStatement.Else is not null)
            return;

        var identifier = GetIdentifierCheckedAgainstNull(ifStatement.Condition);
        if (identifier is null)
            return;

        var throwStatement = GetOnlyThrow(ifStatement.Statement);
        if (throwStatement is null)
            return;

        if (!IsArgumentNullExceptionCreation(throwStatement.Expression, context, types))
            return;

        Report(context, ifStatement.GetLocation(), identifier);
    }

    /// <summary>
    /// Handles <c>x ?? throw new ArgumentNullException(nameof(x))</c>.
    /// </summary>
    private static void AnalyzeThrowExpression(SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        var throwExpression = (ThrowExpressionSyntax)context.Node;
        if (throwExpression.Parent is not BinaryExpressionSyntax coalesce || !coalesce.IsKind(SyntaxKind.CoalesceExpression))
            return;

        if (coalesce.Left is not IdentifierNameSyntax identifierName)
            return;

        if (!IsArgumentNullExceptionCreation(throwExpression.Expression, context, types))
            return;

        Report(context, coalesce.GetLocation(), identifierName.Identifier.ValueText);
    }

    /// <summary>
    /// Handles <c>ArgumentNullException.ThrowIfNull(x)</c>.
    /// </summary>
    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var arguments = invocation.ArgumentList.Arguments;
        if (arguments.Count != 1)
            return;

        if (arguments[0].Expression is not IdentifierNameSyntax identifierName)
            return;

        if (context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol is not IMethodSymbol method)
            return;

        if (!string.Equals(method.Name, ThrowIfNullMethodName, StringComparison.Ordinal))
            return;

        if (!SymbolEqualityComparer.Default.Equals(method.ContainingType, types.ArgumentNullException))
            return;

        Report(context, invocation.GetLocation(), identifierName.Identifier.ValueText);
    }

    private static void Report(SyntaxNodeAnalysisContext context, Location location, string identifier) =>
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.UseGuardAgainstNull,
            location,
            DiagnosticProperties.ForIdentifier(identifier),
            identifier));

    private static string? GetIdentifierCheckedAgainstNull(ExpressionSyntax condition) => condition switch
    {
        IsPatternExpressionSyntax pattern => GetIdentifierMatchedAgainstNull(pattern),
        BinaryExpressionSyntax binary when binary.IsKind(SyntaxKind.EqualsExpression) => GetIdentifierComparedToNull(binary),
        _ => null
    };

    private static string? GetIdentifierMatchedAgainstNull(IsPatternExpressionSyntax pattern)
    {
        if (pattern.Expression is not IdentifierNameSyntax identifierName)
            return null;

        return pattern.Pattern is ConstantPatternSyntax constant && IsNullLiteral(constant.Expression)
            ? identifierName.Identifier.ValueText
            : null;
    }

    private static string? GetIdentifierComparedToNull(BinaryExpressionSyntax binary)
    {
        if (binary.Left is IdentifierNameSyntax left && IsNullLiteral(binary.Right))
            return left.Identifier.ValueText;

        return binary.Right is IdentifierNameSyntax right && IsNullLiteral(binary.Left)
            ? right.Identifier.ValueText
            : null;
    }

    private static bool IsNullLiteral(ExpressionSyntax expression) =>
        expression.IsKind(SyntaxKind.NullLiteralExpression);

    private static ThrowStatementSyntax? GetOnlyThrow(StatementSyntax statement)
    {
        if (statement is not BlockSyntax block)
            return statement as ThrowStatementSyntax;

        return block.Statements.Count == 1
            ? block.Statements[0] as ThrowStatementSyntax
            : null;
    }

    private static bool IsArgumentNullExceptionCreation(ExpressionSyntax? expression, SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        if (expression is not ObjectCreationExpressionSyntax creation)
            return false;

        var type = context.SemanticModel.GetTypeInfo(creation, context.CancellationToken).Type;
        return SymbolEqualityComparer.Default.Equals(type, types.ArgumentNullException);
    }
}
