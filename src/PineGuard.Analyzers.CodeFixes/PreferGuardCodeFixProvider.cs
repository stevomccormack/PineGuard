using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PineGuard.Analyzers.CodeFixes;

/// <summary>
/// Replaces the hand-rolled argument checks reported by <see cref="PreferGuardAnalyzer"/> with the
/// equivalent PineGuard guard clause.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/analyzers">PineGuard analyzers documentation</seealso>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PreferGuardCodeFixProvider))]
[Shared]
public sealed class PreferGuardCodeFixProvider : CodeFixProvider
{
    private const string UseGuardAgainstNullTitle = "Use Guard.Against.Null";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticIds.UseGuardAgainstNull);

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    UseGuardAgainstNullTitle,
                    cancellationToken => UseGuardAgainstNullAsync(context.Document, diagnostic, cancellationToken),
                    equivalenceKey: DiagnosticIds.UseGuardAgainstNull),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> UseGuardAgainstNullAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        var root = (CompilationUnitSyntax)(await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;
        var reported = root.FindNode(diagnostic.Location.SourceSpan);
        var identifier = diagnostic.Properties[DiagnosticProperties.Identifier]!;

        var guarded = root.ReplaceNode(reported, GuardSyntaxFactory.CreateGuardAgainstNull(reported, identifier));

        return document.WithSyntaxRoot(GuardSyntaxFactory.AddGuardClausesUsing(guarded));
    }
}
