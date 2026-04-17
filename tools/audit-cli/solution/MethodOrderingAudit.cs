using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PineGuard.AuditCli;

internal static class MethodOrderingAudit
{
    private const string RuleId = "Rule08";
    private const string Title = "Cross-layer method ordering parity (Rules/Must/Guard/FV/DA)";

    public static async Task<int> RunAsync(string repoRoot, string reportPath, bool allowViolations)
    {
        var families = DiscoverMustFamilies(repoRoot);

        var reportDir = Path.GetDirectoryName(reportPath);
        if (!string.IsNullOrWhiteSpace(reportDir))
            Directory.CreateDirectory(reportDir);

        var violations = new List<string>();
        var warnings = new List<string>();

        var sb = new StringBuilder();
        sb.AppendLine($"{RuleId} - {Title}");
        sb.AppendLine($"GeneratedAtUtc: {DateTime.UtcNow:O}");
        sb.AppendLine($"RepoRoot: {repoRoot}");
        sb.AppendLine();

        foreach (var family in families.OrderBy(f => f.MustTypeName, StringComparer.Ordinal))
        {
            var expected = BuildGroupOrder(family.MustMemberNames, family.DomainName, layer: Layer.Must);
            var expectedSet = new HashSet<string>(expected, StringComparer.Ordinal);

            var familyHeader = $"Family: {family.MustTypeName}";

            // Guard
            if (TryGetGuardMembers(repoRoot, family, out var guardComparableMembers))
            {
                // Guard ordering is defined by the MustClause the Guard method calls.
                // Rationale: Guard methods are typically named for the forbidden state and implemented via Must's allowed-state clause.
                var guardGroupOrder = BuildGroupOrder(guardComparableMembers, family.DomainName, layer: Layer.Guard);
                var guardFiltered = FilterToIntersection(expected, expectedSet, guardGroupOrder);
                if (!SequenceEqual(guardFiltered.Expected, guardFiltered.Actual))
                {
                    var msg = FormatOrderMismatch(familyHeader, "GuardClauses", guardFiltered.Expected, guardFiltered.Actual);
                    violations.Add(msg);
                }

                var guardSetWarning = SummarizeConceptSetDiff(familyHeader, "GuardClauses", guardFiltered.MissingActual, guardFiltered.ExtraActual);
                if (!string.IsNullOrEmpty(guardSetWarning))
                    warnings.Add(guardSetWarning);
            }
            else
            {
                warnings.Add($"{familyHeader} - Missing GuardClauses sibling (skipped). Expected type: Guard{family.Tail}Clauses");
            }

            // FluentValidation
            if (TryGetFluentMembers(repoRoot, family, out var fluentMembers))
            {
                var fluentGroupOrder = BuildGroupOrder(fluentMembers, family.DomainName, layer: Layer.FluentValidation);
                var fluentFiltered = FilterToIntersection(expected, expectedSet, fluentGroupOrder);
                if (!SequenceEqual(fluentFiltered.Expected, fluentFiltered.Actual))
                {
                    var msg = FormatOrderMismatch(familyHeader, "FluentValidation", fluentFiltered.Expected, fluentFiltered.Actual);
                    violations.Add(msg);
                }

                var fluentSetWarning = SummarizeConceptSetDiff(familyHeader, "FluentValidation", fluentFiltered.MissingActual, fluentFiltered.ExtraActual);
                if (!string.IsNullOrEmpty(fluentSetWarning))
                    warnings.Add(fluentSetWarning);
            }
            else
            {
                warnings.Add($"{familyHeader} - Missing FluentValidation sibling (skipped). Expected type: Fluent{family.Tail}Extensions");
            }

            // DataAnnotations
            if (TryGetDataAnnotationsMembers(repoRoot, family, out var daMembers))
            {
                var daGroupOrder = BuildGroupOrder(daMembers, family.DomainName, layer: Layer.DataAnnotations);
                var daFiltered = FilterToIntersection(expected, expectedSet, daGroupOrder);
                if (!SequenceEqual(daFiltered.Expected, daFiltered.Actual))
                {
                    var msg = FormatOrderMismatch(familyHeader, "DataAnnotations", daFiltered.Expected, daFiltered.Actual);
                    violations.Add(msg);
                }

                var daSetWarning = SummarizeConceptSetDiff(familyHeader, "DataAnnotations", daFiltered.MissingActual, daFiltered.ExtraActual);
                if (!string.IsNullOrEmpty(daSetWarning))
                    warnings.Add(daSetWarning);
            }
            else
            {
                warnings.Add($"{familyHeader} - Missing DataAnnotations sibling (skipped). Expected file: {family.Tail}Attributes.cs");
            }

            // Rules
            if (TryGetRulesMembers(repoRoot, family, out var rulesMembers))
            {
                var rulesGroupOrder = BuildGroupOrder(rulesMembers, family.DomainName, layer: Layer.Rules);
                var rulesFiltered = FilterToIntersection(expected, expectedSet, rulesGroupOrder);
                if (!SequenceEqual(rulesFiltered.Expected, rulesFiltered.Actual))
                {
                    var msg = FormatOrderMismatch(familyHeader, "Rules", rulesFiltered.Expected, rulesFiltered.Actual);
                    violations.Add(msg);
                }

                var rulesSetWarning = SummarizeConceptSetDiff(familyHeader, "Rules", rulesFiltered.MissingActual, rulesFiltered.ExtraActual);
                if (!string.IsNullOrEmpty(rulesSetWarning))
                    warnings.Add(rulesSetWarning);
            }
            else
            {
                warnings.Add($"{familyHeader} - Missing Rules sibling (skipped). Expected: {(family.IsStringFamily ? $"StringRules.{GetStringRulesDomainCandidate(family.DomainName)}" : $"{family.Tail}Rules")}");
            }
        }

        sb.AppendLine($"Families: {families.Count}");
        sb.AppendLine($"Violations: {violations.Count}");
        sb.AppendLine($"Warnings: {warnings.Count}");
        sb.AppendLine();

        if (violations.Count > 0)
        {
            sb.AppendLine("-- Violations --");
            foreach (var v in violations.OrderBy(x => x, StringComparer.Ordinal))
            {
                sb.AppendLine(v);
                sb.AppendLine();
            }
        }

        if (warnings.Count > 0)
        {
            sb.AppendLine("-- Warnings --");
            foreach (var w in warnings.OrderBy(x => x, StringComparer.Ordinal))
                sb.AppendLine(w);
        }

        await File.WriteAllTextAsync(reportPath, sb.ToString());
        Console.WriteLine($"Wrote ordering audit report: {reportPath}");

        if (!allowViolations && violations.Count > 0)
            return 1;

        return 0;
    }

    private static bool SequenceEqual(IReadOnlyList<string> a, IReadOnlyList<string> b)
    {
        if (a.Count != b.Count)
            return false;

        for (var i = 0; i < a.Count; i++)
        {
            if (!string.Equals(a[i], b[i], StringComparison.Ordinal))
                return false;
        }

        return true;
    }

    private static string FormatOrderMismatch(string familyHeader, string layer, IReadOnlyList<string> expected, IReadOnlyList<string> actual)
    {
        var expectedText = string.Join(", ", expected);
        var actualText = string.Join(", ", actual);
        return $"{familyHeader} - {layer} order mismatch\nExpected: [{expectedText}]\nActual:   [{actualText}]";
    }

    private static IReadOnlyList<string> BuildGroupOrder(IReadOnlyList<string> memberNames, string domainName, Layer layer)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var ordered = new List<string>();

        foreach (var name in memberNames)
        {
            var op = NormalizeOperationName(name, domainName, layer);
            var baseKey = NormalizeBaseKey(op);

            if (seen.Add(baseKey))
                ordered.Add(baseKey);
        }

        return ordered;
    }

    private static string NormalizeOperationName(string memberName, string domainName, Layer layer)
    {
        var name = memberName;

        if (layer == Layer.Rules && name.StartsWith("Is", StringComparison.Ordinal))
            name = name[2..];

        // Some Rules use 'InXxx' to mean the 'Xxx' concept (e.g., InPast).
        if (layer == Layer.Rules && name.StartsWith("In", StringComparison.Ordinal) && name.Length > 2)
        {
            var remainder = name[2..];
            if (remainder is "Past" or "PastOrPresent" or "Future" or "FutureOrPresent")
                name = remainder;
        }

        if (layer == Layer.DataAnnotations)
        {
            if (name.EndsWith("StringAttribute", StringComparison.Ordinal))
                name = name[..^"StringAttribute".Length];
            else if (name.EndsWith("Attribute", StringComparison.Ordinal))
                name = name[..^"Attribute".Length];
        }

        // Remove domain name when it appears as a prefix (common in DataAnnotations, e.g., CharControl).
        if (!string.IsNullOrEmpty(domainName) && name.StartsWith(domainName, StringComparison.Ordinal) && name.Length > domainName.Length)
            name = name[domainName.Length..];

        if (!string.IsNullOrEmpty(domainName) && name.EndsWith(domainName, StringComparison.Ordinal))
            name = name[..^domainName.Length];

        return name;
    }

    private static string NormalizeBaseKey(string operationName)
    {
        var (isNegative, baseName) = TryStripNegativePrefix(operationName);
        return isNegative ? baseName : operationName;
    }

    private static bool IsNegativeVariant(string operationName)
        => TryStripNegativePrefix(operationName).IsNegative;

    private static (bool IsNegative, string BaseName) TryStripNegativePrefix(string operationName)
    {
        if (operationName.StartsWith("Not", StringComparison.Ordinal) && operationName.Length > 3)
            return (true, operationName[3..]);

        if (operationName.StartsWith("Non", StringComparison.Ordinal) && operationName.Length > 3)
            return (true, operationName[3..]);

        if (operationName.StartsWith("Invalid", StringComparison.Ordinal) && operationName.Length > 7)
            return (true, operationName[7..]);

        return (false, operationName);
    }

    private static (IReadOnlyList<string> Expected, IReadOnlyList<string> Actual, IReadOnlyList<string> ExtraActual, IReadOnlyList<string> MissingActual) FilterToIntersection(
        IReadOnlyList<string> expected,
        HashSet<string> expectedSet,
        IReadOnlyList<string> actual)
    {
        var actualSet = new HashSet<string>(actual, StringComparer.Ordinal);

        var expectedFiltered = expected.Where(k => actualSet.Contains(k)).ToList();
        var actualFiltered = actual.Where(k => expectedSet.Contains(k)).ToList();

        var extraActual = actual.Where(k => !expectedSet.Contains(k)).Distinct(StringComparer.Ordinal).ToList();
        var missingActual = expected.Where(k => !actualSet.Contains(k)).Distinct(StringComparer.Ordinal).ToList();

        return (expectedFiltered, actualFiltered, extraActual, missingActual);
    }

    private static string SummarizeConceptSetDiff(string familyHeader, string layer, IReadOnlyList<string> missing, IReadOnlyList<string> extra)
    {
        if (missing.Count == 0 && extra.Count == 0)
            return string.Empty;

        static string Sample(IReadOnlyList<string> items)
        {
            const int max = 6;
            var sample = items.OrderBy(x => x, StringComparer.Ordinal).Take(max).ToList();
            var suffix = items.Count > max ? $" (+{items.Count - max} more)" : string.Empty;
            return sample.Count == 0 ? "-" : $"[{string.Join(", ", sample)}]{suffix}";
        }

        return $"{familyHeader} - {layer} concept set differs from Must (missing: {missing.Count}; extra: {extra.Count})\nMissing: {Sample(missing)}\nExtra:   {Sample(extra)}";
    }

    private static List<Family> DiscoverMustFamilies(string repoRoot)
    {
        var mustDir = Path.Combine(repoRoot, "src", "PineGuard.MustClauses");
        if (!Directory.Exists(mustDir))
            return [];

        var files = Directory
            .EnumerateFiles(mustDir, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}archived{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var families = new List<Family>();

        foreach (var file in files)
        {
            var root = ParseFile(file);

            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (!cls.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) ||
                    !cls.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
                    continue;

                var typeName = cls.Identifier.Text;
                if (!typeName.StartsWith("Must", StringComparison.Ordinal) || !typeName.EndsWith("Clauses", StringComparison.Ordinal))
                    continue;

                var tail = typeName.Substring("Must".Length, typeName.Length - "Must".Length - "Clauses".Length);
                if (string.IsNullOrWhiteSpace(tail))
                    continue;

                var members = ExtractPublicStaticMethodNamesDistinctInOrder(cls);
                if (members.Count == 0)
                    continue;

                families.Add(new Family(
                    Tail: tail,
                    MustTypeName: typeName,
                    MustFilePath: file,
                    MustMemberNames: members));
            }
        }

        return families;
    }

    private static bool TryGetGuardMembers(string repoRoot, Family family, out IReadOnlyList<string> memberNames)
    {
        var guardDir = Path.Combine(repoRoot, "src", "PineGuard.GuardClauses");
        var candidateTypeName = $"Guard{family.Tail}Clauses";

        memberNames = [];

        var file = FindTypeFile(guardDir, candidateTypeName);
        if (file is null)
            return false;

        var root = ParseFile(file);
        var cls = FindStaticClass(root, candidateTypeName);
        if (cls is null)
            return false;

        // For Guard ordering parity, the comparable "member name" is the MustClause name the Guard method invokes.
        // This aligns ordering with Must's canonical sequence even when Guard method names represent forbidden complements.
        var methods = cls.Members.OfType<MethodDeclarationSyntax>()
            .Where(m => m.Modifiers.Any(x => x.IsKind(SyntaxKind.PublicKeyword))
                && m.Modifiers.Any(x => x.IsKind(SyntaxKind.StaticKeyword)))
            .ToList();

        var names = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var method in methods)
        {
            var comparable = TryGetGuardComparableName(method) ?? method.Identifier.Text;
            if (seen.Add(comparable))
                names.Add(comparable);
        }

        memberNames = names;
        return memberNames.Count > 0;
    }

    private static string? TryGetGuardComparableName(MethodDeclarationSyntax method)
    {
        var body = (SyntaxNode?)method.Body ?? method.ExpressionBody;
        if (body is null)
            return null;

        foreach (var invocation in body.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (!IsMustClauseInvocation(invocation))
                continue;

            return GetInvokedName(invocation);
        }

        return null;
    }

    private static bool IsMustClauseInvocation(InvocationExpressionSyntax invocation)
    {
        // Common patterns:
        // - Must.Be.Xxx(...)
        // - MustXxxClauses.Yyy(Must.Be, ...)

        var expressionContainsMust = invocation.Expression
            .DescendantNodesAndSelf()
            .OfType<IdentifierNameSyntax>()
            .Any(x => string.Equals(x.Identifier.Text, "Must", StringComparison.Ordinal));

        if (expressionContainsMust)
            return true;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (invocation.ArgumentList is null)
            return false;

        foreach (var arg in invocation.ArgumentList.Arguments)
        {
            if (arg.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax id } ma
                && string.Equals(id.Identifier.Text, "Must", StringComparison.Ordinal)
                && string.Equals(ma.Name.Identifier.Text, "Be", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static string? GetInvokedName(InvocationExpressionSyntax invocation)
    {
        return invocation.Expression switch
        {
            MemberAccessExpressionSyntax ma => ma.Name.Identifier.Text,
            IdentifierNameSyntax id => id.Identifier.Text,
            _ => null
        };
    }

    private static bool TryGetFluentMembers(string repoRoot, Family family, out IReadOnlyList<string> memberNames)
    {
        var fvDir = Path.Combine(repoRoot, "src", "PineGuard.FluentValidation");

        memberNames = [];

        foreach (var tailVariant in GetTailVariants(family.Tail))
        {
            var candidateTypeName = $"Fluent{tailVariant}Extensions";
            var file = FindTypeFile(fvDir, candidateTypeName);
            if (file is null)
                continue;

            var root = ParseFile(file);
            var cls = FindStaticClass(root, candidateTypeName);
            if (cls is null)
                continue;

            memberNames = ExtractPublicStaticMethodNamesDistinctInOrder(cls);
            return memberNames.Count > 0;
        }

        return false;
    }

    private static bool TryGetDataAnnotationsMembers(string repoRoot, Family family, out IReadOnlyList<string> memberNames)
    {
        var daDir = Path.Combine(repoRoot, "src", "PineGuard.DataAnnotations");

        memberNames = [];

        foreach (var fileName in GetDataAnnotationsFileNameCandidates(family))
        {
            var file = Path.Combine(daDir, fileName);
            if (!File.Exists(file))
                continue;

            var root = ParseFile(file);

            var names = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (cls.Parent is not BaseNamespaceDeclarationSyntax)
                    continue;

                if (!cls.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                    continue;

                var name = cls.Identifier.Text;
                if (!name.EndsWith("Attribute", StringComparison.Ordinal))
                    continue;

                if (seen.Add(name))
                    names.Add(name);
            }

            if (names.Count == 0)
                continue;

            memberNames = names;
            return true;
        }

        return false;
    }

    private static bool TryGetRulesMembers(string repoRoot, Family family, out IReadOnlyList<string> memberNames)
    {
        var rulesDir = Path.Combine(repoRoot, "src", "PineGuard.Core", "Rules");
        memberNames = [];

        if (family.IsStringFamily)
        {
            var domainCandidate = GetStringRulesDomainCandidate(family.DomainName);

            var stringRulesFiles = new[]
            {
                Path.Combine(rulesDir, $"StringRules.{domainCandidate}.cs"),
                Path.Combine(rulesDir, "StringRules.cs")
            };

            foreach (var file in stringRulesFiles.Where(File.Exists))
            {
                var root = ParseFile(file);

                if (string.IsNullOrEmpty(family.DomainName))
                {
                    var outer = FindStaticClass(root, "StringRules");
                    if (outer is null)
                        continue;

                    memberNames = ExtractPublicStaticMethodNamesDistinctInOrder(outer);
                    return memberNames.Count > 0;
                }

                var nested = FindNestedStaticClass(root, outerTypeName: "StringRules", nestedTypeName: domainCandidate);
                if (nested is null)
                    continue;

                memberNames = ExtractPublicStaticMethodNamesDistinctInOrder(nested);
                return memberNames.Count > 0;
            }

            return false;
        }

        var candidateTypeName = $"{family.Tail}Rules";
        var candidateFile = Path.Combine(rulesDir, $"{candidateTypeName}.cs");
        if (!File.Exists(candidateFile))
        {
            // Some rules types are split or nested; skip if we can't locate the obvious sibling.
            return false;
        }

        {
            var root = ParseFile(candidateFile);
            var cls = FindStaticClass(root, candidateTypeName);
            if (cls is null)
                return false;

            memberNames = ExtractPublicStaticMethodNamesDistinctInOrder(cls);
            return memberNames.Count > 0;
        }
    }

    private static IEnumerable<string> GetTailVariants(string tail)
    {
        yield return tail;

        if (tail.EndsWith("Number", StringComparison.Ordinal))
            yield return tail + "s";
    }

    private static IEnumerable<string> GetDataAnnotationsFileNameCandidates(Family family)
    {
        yield return $"{family.Tail}Attributes.cs";

        if (family.IsStringFamily && !string.IsNullOrEmpty(family.DomainName))
            yield return $"{family.DomainName}StringAttributes.cs";
    }

    private static string GetStringRulesDomainCandidate(string domainName)
    {
        // StringRules uses a small number of pluralized/namespaced domain type names.
        if (string.Equals(domainName, "Number", StringComparison.Ordinal))
            return "Numbers";

        return domainName;
    }

    private static CompilationUnitSyntax ParseFile(string filePath)
    {
        var text = File.ReadAllText(filePath);
        var tree = CSharpSyntaxTree.ParseText(text);
        return tree.GetCompilationUnitRoot();
    }

    private static string? FindTypeFile(string rootDir, string typeName)
    {
        if (!Directory.Exists(rootDir))
            return null;

        // Fast path: by convention, type name matches file name.
        var byName = Directory
            .EnumerateFiles(rootDir, $"{typeName}.cs", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (byName is not null)
            return byName;

        // Fallback: scan all .cs files for a matching type declaration.
        foreach (var file in Directory.EnumerateFiles(rootDir, "*.cs", SearchOption.AllDirectories))
        {
            if (file.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                file.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                continue;

            var root = ParseFile(file);
            if (FindStaticClass(root, typeName) is not null)
                return file;
        }

        return null;
    }

    private static ClassDeclarationSyntax? FindStaticClass(CompilationUnitSyntax root, string typeName)
        => root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(c =>
                string.Equals(c.Identifier.Text, typeName, StringComparison.Ordinal) &&
                c.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) &&
                c.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)));

    private static ClassDeclarationSyntax? FindNestedStaticClass(CompilationUnitSyntax root, string outerTypeName, string nestedTypeName)
        => root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(c =>
                string.Equals(c.Identifier.Text, nestedTypeName, StringComparison.Ordinal) &&
                c.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) &&
                c.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)) &&
                c.Parent is ClassDeclarationSyntax parent &&
                string.Equals(parent.Identifier.Text, outerTypeName, StringComparison.Ordinal));

    private static IReadOnlyList<string> ExtractPublicStaticMethodNamesDistinctInOrder(ClassDeclarationSyntax cls)
    {
        var names = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
        {
            if (!method.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) ||
                !method.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
                continue;

            var name = method.Identifier.Text;
            if (seen.Add(name))
                names.Add(name);
        }

        return names;
    }

    private enum Layer
    {
        Must,
        Guard,
        FluentValidation,
        DataAnnotations,
        Rules
    }

    private sealed record Family(
        string Tail,
        string MustTypeName,
        string MustFilePath,
        IReadOnlyList<string> MustMemberNames)
    {
        public bool IsStringFamily => Tail.StartsWith("String", StringComparison.Ordinal);

        public string DomainName
            => IsStringFamily
                ? Tail["String".Length..]
                : Tail;
    }
}
