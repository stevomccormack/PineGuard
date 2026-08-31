using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PineGuard.Analyzers.CodeFixes;

/// <summary>
/// Builds the <c>Guard.Against.X(...)</c> syntax a code fix substitutes for a hand-rolled check,
/// and the <c>using PineGuard.GuardClauses;</c> that makes it bind.
/// </summary>
internal static class GuardSyntaxFactory
{
    /// <summary>
    /// The namespace a fixed document must import for <c>Guard</c> to bind.
    /// </summary>
    internal const string GuardClausesNamespace = "PineGuard.GuardClauses";

    /// <summary>
    /// The <c>Guard.Against.Null</c> clause name.
    /// </summary>
    internal const string NullClause = "Null";

    private const string GuardTypeName = "Guard";
    private const string AgainstPropertyName = "Against";

    /// <summary>
    /// Rewrites <paramref name="node"/> — one of the three shapes PG1001 reports — as the
    /// equivalent <c>Guard.Against.Null</c> call, preserving the original trivia.
    /// </summary>
    /// <param name="node">The reported node: an <c>if</c> statement, a coalesce-throw expression, or a <c>ThrowIfNull</c> invocation.</param>
    /// <param name="identifier">The name of the guarded identifier.</param>
    /// <returns>The replacement node.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="node"/> is not one of the three shapes PG1001 reports.
    /// </exception>
    internal static SyntaxNode CreateGuardAgainstNull(SyntaxNode node, string identifier)
    {
        var invocation = GuardInvocation(NullClause, IdentifierName(identifier));

        return node switch
        {
            IfStatementSyntax ifStatement => ExpressionStatement(invocation).WithTriviaFrom(ifStatement),
            InvocationExpressionSyntax throwIfNull => invocation.WithTriviaFrom(throwIfNull),
            BinaryExpressionSyntax coalesce => invocation.WithTriviaFrom(coalesce),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.Kind(), "PG1001 reports an if statement, a coalesce-throw expression or a ThrowIfNull invocation.")
        };
    }

    /// <summary>
    /// Adds <c>using PineGuard.GuardClauses;</c> to <paramref name="root"/> unless it is already imported.
    /// </summary>
    /// <param name="root">The compilation unit to import into.</param>
    /// <returns>The compilation unit, importing PineGuard.GuardClauses exactly once.</returns>
    internal static CompilationUnitSyntax AddGuardClausesUsing(CompilationUnitSyntax root)
    {
        foreach (var directive in root.Usings)
        {
            if (string.Equals(directive.Name?.ToString(), GuardClausesNamespace, StringComparison.Ordinal))
                return root;
        }

        var guardClauses = UsingDirective(ParseName(GuardClausesNamespace))
            .WithTrailingTrivia(ElasticCarriageReturnLineFeed)
            .WithAdditionalAnnotations(Formatter.Annotation);

        return root.AddUsings(guardClauses);
    }

    /// <summary>
    /// Builds a <c>Guard.Against.{clause}({arguments})</c> invocation.
    /// </summary>
    /// <param name="clause">The guard clause name, such as <c>Null</c>.</param>
    /// <param name="arguments">The arguments to pass to the clause.</param>
    /// <returns>The invocation expression.</returns>
    internal static InvocationExpressionSyntax GuardInvocation(string clause, params ExpressionSyntax[] arguments) =>
        InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(GuardTypeName),
                    IdentifierName(AgainstPropertyName)),
                IdentifierName(clause)),
            ArgumentList(SeparatedList(Array.ConvertAll(arguments, Argument))));
}
