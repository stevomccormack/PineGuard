using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PineGuard.Analyzers.CodeFixes;

/// <summary>
/// Builds the two statements that stop a PineGuard result being discarded: the call with
/// <c>ThrowIfFailed()</c> chained onto it, and the call assigned to a local.
/// </summary>
internal static class MustResultSyntaxFactory
{
    /// <summary>
    /// The member every PineGuard result type exposes to turn a failure into an exception.
    /// </summary>
    internal const string ThrowIfFailedMethodName = "ThrowIfFailed";

    /// <summary>
    /// The name the assignment fix gives the local it introduces.
    /// </summary>
    internal const string ResultVariableName = "result";

    private static readonly TypeSyntax VarType = IdentifierName("var").WithTrailingTrivia(Space);

    /// <summary>
    /// Rewrites <paramref name="statement"/> as the same call with <c>ThrowIfFailed()</c> chained
    /// onto it, preserving the original trivia.
    /// </summary>
    /// <param name="statement">The discarded call statement.</param>
    /// <returns>The replacement statement.</returns>
    internal static StatementSyntax ThrowIfFailed(ExpressionStatementSyntax statement) =>
        ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    statement.Expression.WithoutTrivia(),
                    IdentifierName(ThrowIfFailedMethodName))))
            .WithTriviaFrom(statement);

    /// <summary>
    /// Rewrites <paramref name="statement"/> as a local declaration holding the result, preserving
    /// the original trivia.
    /// </summary>
    /// <param name="statement">The discarded call statement.</param>
    /// <returns>The replacement statement.</returns>
    internal static StatementSyntax AssignResult(ExpressionStatementSyntax statement) =>
        LocalDeclarationStatement(
            VariableDeclaration(
                VarType,
                SingletonSeparatedList(
                    VariableDeclarator(Identifier(UnusedResultName(statement)).WithTrailingTrivia(Space))
                        .WithInitializer(EqualsValueClause(
                            Token(SyntaxKind.EqualsToken).WithTrailingTrivia(Space),
                            statement.Expression.WithoutTrivia())))))
            .WithTriviaFrom(statement);

    /// <summary>
    /// Returns <see cref="ResultVariableName"/>, or the first numbered variation of it that the
    /// surrounding member does not already spell.
    /// </summary>
    /// <param name="statement">The statement the local is introduced beside.</param>
    /// <returns>A name the new local can safely take.</returns>
    /// <remarks>
    /// Every identifier in the member counts as taken, whether or not it is a local: a name that is
    /// merely unused is cheaper than a name that shadows or collides.
    /// </remarks>
    private static string UnusedResultName(SyntaxNode statement)
    {
        // A statement always sits inside a member declaration — a method body, or the global
        // statement that wraps a top-level statement.
        var member = statement.FirstAncestorOrSelf<MemberDeclarationSyntax>()!;

        var spelled = new HashSet<string>(
            member.DescendantTokens().Where(token => token.IsKind(SyntaxKind.IdentifierToken)).Select(token => token.ValueText),
            StringComparer.Ordinal);

        var name = ResultVariableName;
        for (var suffix = 2; spelled.Contains(name); suffix++)
            name = ResultVariableName + suffix.ToString(CultureInfo.InvariantCulture);

        return name;
    }
}
