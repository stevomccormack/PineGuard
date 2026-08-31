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
    private const string ThrowIfNullOrWhiteSpaceMethodName = "ThrowIfNullOrWhiteSpace";
    private const string ThrowIfNullOrEmptyMethodName = "ThrowIfNullOrEmpty";
    private const string IsNullOrWhiteSpaceMethodName = "IsNullOrWhiteSpace";
    private const string IsNullOrEmptyMethodName = "IsNullOrEmpty";

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(
            DiagnosticDescriptors.UseGuardAgainstNull,
            DiagnosticDescriptors.UseGuardAgainstNullOrWhiteSpace,
            DiagnosticDescriptors.UseGuardAgainstNullOrEmpty);

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
    /// Handles the <c>if (check) throw new ArgumentXException(...);</c> shapes.
    /// </summary>
    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        var ifStatement = (IfStatementSyntax)context.Node;
        if (ifStatement.Else is not null)
            return;

        var throwStatement = GetOnlyThrow(ifStatement.Statement);
        if (throwStatement is null)
            return;

        var thrown = GetCreatedExceptionType(throwStatement.Expression, context);
        if (thrown is null)
            return;

        if (SymbolEqualityComparer.Default.Equals(thrown, types.ArgumentNullException))
        {
            ReportNullCheck(context, ifStatement);
            return;
        }

        if (SymbolEqualityComparer.Default.Equals(thrown, types.ArgumentException))
            ReportEmptinessCheck(context, ifStatement);
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

        var thrown = GetCreatedExceptionType(throwExpression.Expression, context);
        if (thrown is null)
            return;

        if (!SymbolEqualityComparer.Default.Equals(thrown, types.ArgumentNullException))
            return;

        Report(context, GuardSuggestion.Null, coalesce.GetLocation(), identifierName.Identifier.ValueText);
    }

    /// <summary>
    /// Handles the framework throw helpers — <c>ArgumentNullException.ThrowIfNull(x)</c> and
    /// <c>ArgumentException.ThrowIfNullOrWhiteSpace(x)</c>.
    /// </summary>
    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context, PineGuardTypes types)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var identifier = GetSingleIdentifierArgument(invocation);
        if (identifier is null)
            return;

        if (context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol is not IMethodSymbol method)
            return;

        var suggestion = GetThrowHelperSuggestion(method, types);
        if (suggestion is null)
            return;

        Report(context, suggestion, invocation.GetLocation(), identifier);
    }

    /// <summary>
    /// Reports <c>if (x is null) throw new ArgumentNullException(nameof(x));</c>.
    /// </summary>
    private static void ReportNullCheck(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement)
    {
        var identifier = GetIdentifierCheckedAgainstNull(ifStatement.Condition);
        if (identifier is null)
            return;

        Report(context, GuardSuggestion.Null, ifStatement.GetLocation(), identifier);
    }

    /// <summary>
    /// Reports <c>if (string.IsNullOrWhiteSpace(x)) throw new ArgumentException(...);</c>.
    /// </summary>
    private static void ReportEmptinessCheck(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement)
    {
        if (ifStatement.Condition is not InvocationExpressionSyntax invocation)
            return;

        var identifier = GetSingleIdentifierArgument(invocation);
        if (identifier is null)
            return;

        if (context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol
            is not IMethodSymbol { ContainingType.SpecialType: SpecialType.System_String } predicate)
            return;

        var suggestion = GetEmptinessSuggestion(predicate.Name);
        if (suggestion is null)
            return;

        Report(context, suggestion, ifStatement.GetLocation(), identifier);
    }

    private static void Report(SyntaxNodeAnalysisContext context, GuardSuggestion suggestion, Location location, string identifier) =>
        context.ReportDiagnostic(Diagnostic.Create(
            suggestion.Descriptor,
            location,
            DiagnosticProperties.ForGuard(suggestion.Clause, identifier),
            identifier));

    /// <summary>
    /// Maps a <see cref="string"/> predicate name onto the guard clause that replaces it.
    /// </summary>
    private static GuardSuggestion? GetEmptinessSuggestion(string methodName)
    {
        if (string.Equals(methodName, IsNullOrWhiteSpaceMethodName, StringComparison.Ordinal))
            return GuardSuggestion.NullOrWhiteSpace;

        return string.Equals(methodName, IsNullOrEmptyMethodName, StringComparison.Ordinal)
            ? GuardSuggestion.NullOrEmpty
            : null;
    }

    /// <summary>
    /// Maps a framework <c>ThrowIfX</c> helper onto the guard clause that replaces it.
    /// </summary>
    private static GuardSuggestion? GetThrowHelperSuggestion(IMethodSymbol method, PineGuardTypes types)
    {
        if (string.Equals(method.Name, ThrowIfNullMethodName, StringComparison.Ordinal))
        {
            return SymbolEqualityComparer.Default.Equals(method.ContainingType, types.ArgumentNullException)
                ? GuardSuggestion.Null
                : null;
        }

        var suggestion = GetEmptinessThrowHelperSuggestion(method.Name);
        if (suggestion is null)
            return null;

        return SymbolEqualityComparer.Default.Equals(method.ContainingType, types.ArgumentException)
            ? suggestion
            : null;
    }

    /// <summary>
    /// Maps an <see cref="System.ArgumentException"/> emptiness helper name onto the guard clause
    /// that replaces it.
    /// </summary>
    private static GuardSuggestion? GetEmptinessThrowHelperSuggestion(string methodName)
    {
        if (string.Equals(methodName, ThrowIfNullOrWhiteSpaceMethodName, StringComparison.Ordinal))
            return GuardSuggestion.NullOrWhiteSpace;

        return string.Equals(methodName, ThrowIfNullOrEmptyMethodName, StringComparison.Ordinal)
            ? GuardSuggestion.NullOrEmpty
            : null;
    }

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

    /// <summary>
    /// Returns the name of the only argument of <paramref name="invocation"/> when it is a plain
    /// identifier, and <see langword="null"/> for every other argument list.
    /// </summary>
    private static string? GetSingleIdentifierArgument(InvocationExpressionSyntax invocation)
    {
        var arguments = invocation.ArgumentList.Arguments;
        if (arguments.Count != 1)
            return null;

        return arguments[0].Expression is IdentifierNameSyntax identifierName
            ? identifierName.Identifier.ValueText
            : null;
    }

    private static ThrowStatementSyntax? GetOnlyThrow(StatementSyntax statement)
    {
        if (statement is not BlockSyntax block)
            return statement as ThrowStatementSyntax;

        return block.Statements.Count == 1
            ? block.Statements[0] as ThrowStatementSyntax
            : null;
    }

    /// <summary>
    /// Returns the type of the exception <paramref name="expression"/> constructs, or
    /// <see langword="null"/> when it does not construct one.
    /// </summary>
    private static ITypeSymbol? GetCreatedExceptionType(ExpressionSyntax? expression, SyntaxNodeAnalysisContext context) =>
        expression is ObjectCreationExpressionSyntax creation
            ? context.SemanticModel.GetTypeInfo(creation, context.CancellationToken).Type
            : null;
}
