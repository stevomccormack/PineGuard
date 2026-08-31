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
    private const string TitlePrefix = "Use Guard.Against.";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(
            DiagnosticIds.UseGuardAgainstNull,
            DiagnosticIds.UseGuardAgainstNullOrWhiteSpace,
            DiagnosticIds.UseGuardAgainstNullOrEmpty);

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        foreach (var diagnostic in context.Diagnostics)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    TitlePrefix + diagnostic.Properties[DiagnosticProperties.Clause],
                    cancellationToken => UseGuardAsync(context.Document, diagnostic, cancellationToken),
                    equivalenceKey: diagnostic.Id),
                diagnostic);
        }

        return Task.CompletedTask;
    }

    private static async Task<Document> UseGuardAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        var root = (CompilationUnitSyntax)(await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;
        var reported = root.FindNode(diagnostic.Location.SourceSpan);

        var guarded = root.ReplaceNode(reported, GuardSyntaxFactory.CreateGuard(reported, diagnostic));

        return document.WithSyntaxRoot(GuardSyntaxFactory.AddGuardClausesUsing(guarded));
    }
}
