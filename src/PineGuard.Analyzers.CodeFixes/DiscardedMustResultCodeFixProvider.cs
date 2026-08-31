using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PineGuard.Analyzers.CodeFixes;

/// <summary>
/// Offers the two ways to stop discarding the result reported by
/// <see cref="DiscardedMustResultAnalyzer"/>: throw on failure, or keep the result and inspect it.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/analyzers">PineGuard analyzers documentation</seealso>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscardedMustResultCodeFixProvider))]
[Shared]
public sealed class DiscardedMustResultCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Identifies the "throw if failed" fix, so a fix-all applies that one everywhere.
    /// </summary>
    internal const string ThrowIfFailedEquivalenceKey = "PineGuard.ThrowIfFailed";

    /// <summary>
    /// Identifies the "assign the result" fix, so a fix-all applies that one everywhere.
    /// </summary>
    internal const string AssignResultEquivalenceKey = "PineGuard.AssignResult";

    private const string ThrowIfFailedTitle = "Throw if failed";
    private const string AssignResultTitle = "Assign the result";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticIds.DiscardedMustResult);

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    ThrowIfFailedTitle,
                    cancellationToken => RewriteAsync(context.Document, diagnostic, MustResultSyntaxFactory.ThrowIfFailed, cancellationToken),
                    equivalenceKey: ThrowIfFailedEquivalenceKey),
                diagnostic);

            context.RegisterCodeFix(
                CodeAction.Create(
                    AssignResultTitle,
                    cancellationToken => RewriteAsync(context.Document, diagnostic, MustResultSyntaxFactory.AssignResult, cancellationToken),
                    equivalenceKey: AssignResultEquivalenceKey),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Replaces the reported call statement with whatever <paramref name="rewrite"/> makes of it.
    /// </summary>
    /// <param name="document">The document holding the reported statement.</param>
    /// <param name="diagnostic">The reported diagnostic, locating the statement.</param>
    /// <param name="rewrite">The rewrite to apply to that statement.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The document with the discarded result put to use.</returns>
    private static async Task<Document> RewriteAsync(
        Document document,
        Diagnostic diagnostic,
        Func<ExpressionStatementSyntax, StatementSyntax> rewrite,
        CancellationToken cancellationToken)
    {
        var root = (await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;

        // The analyzer reports the whole statement, so the span names it exactly.
        var discarded = (ExpressionStatementSyntax)root.FindNode(diagnostic.Location.SourceSpan);

        return document.WithSyntaxRoot(root.ReplaceNode(discarded, rewrite(discarded)));
    }
}
