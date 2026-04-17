using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace PineGuard.AuditCli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Any(a => a is "--help" or "-h" or "/?"))
        {
            PrintHelp();
            return 0;
        }

        if (args.Any(a => a is "--version"))
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown";
            Console.WriteLine($"PineGuard.AuditCli {version}");
            return 0;
        }

        var options = CliOptions.Parse(args);

        if (options.Audit.Equals("ordering", StringComparison.OrdinalIgnoreCase))
        {
            var resolvedRepoRoot = options.RepoRoot ?? FindRepoRoot(AppContext.BaseDirectory);
            return await MethodOrderingAudit.RunAsync(resolvedRepoRoot, options.ReportPath, options.AllowViolations);
        }

        if (options.CreateSpecTemplate)
        {
            var template = NamingSpec.CreateTemplate();
            var specDir = Path.GetDirectoryName(options.SpecPath);
            if (!string.IsNullOrWhiteSpace(specDir))
                Directory.CreateDirectory(specDir);

            await File.WriteAllTextAsync(options.SpecPath, Serialize(template, indented: true));
            Console.WriteLine($"Wrote spec template: {options.SpecPath}");
            return 0;
        }

        var specText = await File.ReadAllTextAsync(options.SpecPath);
        var spec = JsonSerializer.Deserialize<NamingSpec>(specText, NamingJson.Options);
        if (spec is null)
        {
            await Console.Error.WriteLineAsync($"Failed to read spec: {options.SpecPath}");
            return 2;
        }

        if (!spec.Projects.TryGetValue(options.Project, out var projectSpec))
        {
            await Console.Error.WriteLineAsync($"Project '{options.Project}' not found in spec. Known: {string.Join(", ", spec.Projects.Keys)}");
            return 2;
        }

        var repoRoot = options.RepoRoot ?? FindRepoRoot(AppContext.BaseDirectory);
        var resolvedProjectPath = ResolvePath(repoRoot, projectSpec.ProjectPath);

        if (!File.Exists(resolvedProjectPath))
        {
            await Console.Error.WriteLineAsync($"Project path not found: {resolvedProjectPath}");
            return 2;
        }

        if (!MSBuildLocator.IsRegistered)
            MSBuildLocator.RegisterDefaults();

        using var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += (_, e) =>
        {
            Console.Error.WriteLine($"[MSBuildWorkspace] {e.Diagnostic.Kind}: {e.Diagnostic.Message}");
        };

        var project = await workspace.OpenProjectAsync(resolvedProjectPath);
        var compilation = await project.GetCompilationAsync();

        if (compilation is null)
        {
            await Console.Error.WriteLineAsync("Failed to create compilation.");
            return 2;
        }

        var receiverTypeExpected = projectSpec.ExtensionReceiverFullyQualifiedType;
        var containingTypeRegex = new Regex(projectSpec.ContainingTypeNameRegex, RegexOptions.Compiled);

        var methods = new List<MethodRecord>();

        foreach (var document in project.Documents)
        {
            var tree = await document.GetSyntaxTreeAsync();
            if (tree is null)
                continue;

            var root = await tree.GetRootAsync();
            var semanticModel = compilation.GetSemanticModel(tree, ignoreAccessibility: true);

            foreach (var methodDecl in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                if (!methodDecl.Modifiers.Any(m => m.Text is "public") ||
                    !methodDecl.Modifiers.Any(m => m.Text is "static"))
                    continue;

                var symbol = semanticModel.GetDeclaredSymbol(methodDecl) as IMethodSymbol;
                if (symbol is null)
                    continue;

                if (!symbol.IsExtensionMethod)
                    continue;

                if (!StringEquals(symbol.ContainingNamespace.ToDisplayString(), projectSpec.Namespace))
                    continue;

                if (!containingTypeRegex.IsMatch(symbol.ContainingType.Name))
                    continue;

                var receiverParam = symbol.Parameters.FirstOrDefault();
                if (receiverParam is null)
                    continue;

                var receiverType = receiverParam.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!StringEquals(receiverType, receiverTypeExpected))
                    continue;

                if (symbol.Parameters.Length < 2)
                    continue;

                var primaryParam = symbol.Parameters[1];
                var primary = PrimaryParamInfo.From(primaryParam);

                var expected = ExpectedNullability.Compute(
                    methodId: MethodIds.Build(symbol),
                    primary: primary,
                    policy: spec.Policies.MustClauses);

                var record = MethodRecord.From(
                    method: symbol,
                    sourceFile: document.FilePath ?? string.Empty,
                    primary: primary,
                    expected: expected);

                methods.Add(record);
            }
        }

        var collision = CollisionAnalysis.Analyze(methods, compilation, spec.Policies.MustClauses.CollisionChecks);

        var report = new AuditReport
        {
            Metadata = AuditMetadata.Create(options, spec, resolvedProjectPath),
            Methods = methods
                .OrderBy(m => m.MethodId, StringComparer.Ordinal)
                .ThenBy(m => m.PrimaryParam.TypeDisplay, StringComparer.Ordinal)
                .ToList(),
            Summary = AuditSummary.From(methods, collision),
            Collisions = collision
        };

        var reportDir = Path.GetDirectoryName(options.ReportPath);
        if (!string.IsNullOrWhiteSpace(reportDir))
            Directory.CreateDirectory(reportDir);

        await File.WriteAllTextAsync(options.ReportPath, Serialize(report, indented: true));
        Console.WriteLine($"Wrote report: {options.ReportPath}");

        if (!string.IsNullOrWhiteSpace(options.SnapshotPath) && options.CreateSnapshot)
        {
            var snapDir = Path.GetDirectoryName(options.SnapshotPath);
            if (!string.IsNullOrWhiteSpace(snapDir))
                Directory.CreateDirectory(snapDir);

            await File.WriteAllTextAsync(options.SnapshotPath, Serialize(report.Methods, indented: true));
            Console.WriteLine($"Wrote snapshot: {options.SnapshotPath}");
        }

        if (!options.AllowViolations && report.Summary.ViolationsCount > 0)
        {
            await Console.Error.WriteLineAsync($"Violations: {report.Summary.ViolationsCount}");
            return 1;
        }

        return 0;
    }

    private static void PrintHelp()
    {
        Console.WriteLine("PineGuard.AuditCli");
        Console.WriteLine("\nAudits used by tools/audit-cli/**/*.ps1 wrappers.");
        Console.WriteLine("\nUsage:");
        Console.WriteLine("  dotnet run --project tools/PineGuard.AuditCli/PineGuard.AuditCli.csproj -c Release -- [options]");
        Console.WriteLine("\nAudits:");
        Console.WriteLine("  --audit naming      (default) MustClauses naming/nullability/collision audit");
        Console.WriteLine("  --audit ordering               Cross-layer method ordering parity audit");
        Console.WriteLine("\nCommon options:");
        Console.WriteLine("  --repoRoot <path>              Repository root (defaults to auto-detected) ");
        Console.WriteLine("  --allowViolations true|false   Exit 0 even when violations exist (default: false)");
        Console.WriteLine("\nNaming audit options:");
        Console.WriteLine("  --project <name>               e.g. MustClauses");
        Console.WriteLine("  --spec <path>                  Naming spec JSON (default: artifacts/audit/naming-spec.json)");
        Console.WriteLine("  --report <path>                Report output path (default: artifacts/audit/naming-audit.json)");
        Console.WriteLine("  --createSpecTemplate true|false");
        Console.WriteLine("  --createSnapshot true|false    If true, also write --snapshot");
        Console.WriteLine("  --snapshot <path>");
        Console.WriteLine("\nOrdering audit options:");
        Console.WriteLine("  --report <path>                Report output path");
        Console.WriteLine("\nExamples:");
        Console.WriteLine("  dotnet run --project tools/PineGuard.AuditCli/PineGuard.AuditCli.csproj -c Release -- --audit ordering --report artifacts/audit/Rule08-method-ordering-parity.txt");
        Console.WriteLine("  dotnet run --project tools/PineGuard.AuditCli/PineGuard.AuditCli.csproj -c Release -- --project MustClauses --spec artifacts/audit/naming-spec.json --report artifacts/audit/Rule01.json");
    }

    private static string Serialize<T>(T value, bool indented)
        => JsonSerializer.Serialize(value, new JsonSerializerOptions(NamingJson.Options)
        {
            WriteIndented = indented
        });

    private static string ResolvePath(string repoRoot, string path)
        => Path.GetFullPath(Path.Combine(repoRoot, path.Replace('/', Path.DirectorySeparatorChar)));

    private static bool StringEquals(string a, string b)
        => string.Equals(a, b, StringComparison.Ordinal);

    private static string FindRepoRoot(string start)
    {
        var dir = new DirectoryInfo(start);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "AGENTS.md")))
                return dir.FullName;

            dir = dir.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}

internal static class NamingJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
}

internal static class MethodIds
{
    public static string Build(IMethodSymbol method)
        => $"{method.ContainingNamespace.ToDisplayString()}.{method.ContainingType.Name}.{method.Name}";
}

internal sealed record CliOptions(
    string Audit,
    string Project,
    string SpecPath,
    string ReportPath,
    string? SnapshotPath,
    bool CreateSnapshot,
    bool CreateSpecTemplate,
    bool AllowViolations,
    string? RepoRoot)
{
    public static CliOptions Parse(string[] args)
    {
        var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            var key = args[i];
            if (!key.StartsWith("--", StringComparison.Ordinal))
                continue;

            var val = (i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal)) ? args[++i] : "true";
            dict[key] = val;
        }

        dict.TryGetValue("--project", out var project);
        dict.TryGetValue("--spec", out var spec);
        dict.TryGetValue("--report", out var report);
        dict.TryGetValue("--snapshot", out var snapshot);
        dict.TryGetValue("--repoRoot", out var repoRoot);
        dict.TryGetValue("--audit", out var audit);

        var createSnapshot = dict.TryGetValue("--createSnapshot", out var cs) && bool.TryParse(cs, out var b1) && b1;
        var createSpecTemplate = dict.TryGetValue("--createSpecTemplate", out var ct) && bool.TryParse(ct, out var b2) && b2;
        var allowViolations = dict.TryGetValue("--allowViolations", out var av) && bool.TryParse(av, out var b3) && b3;

        return new CliOptions(
            Audit: audit ?? "naming",
            Project: project ?? "MustClauses",
            SpecPath: spec ?? "artifacts/audit/naming-spec.json",
            ReportPath: report ?? "artifacts/audit/naming-audit.json",
            SnapshotPath: snapshot,
            CreateSnapshot: createSnapshot,
            CreateSpecTemplate: createSpecTemplate,
            AllowViolations: allowViolations,
            RepoRoot: repoRoot);
    }
}

internal sealed record NamingSpec
{
    public int Version { get; init; } = 1;

    public Dictionary<string, ProjectSpec> Projects { get; init; } = new(StringComparer.Ordinal);

    public PoliciesSpec Policies { get; init; } = new();

    public static NamingSpec CreateTemplate()
    {
        return new NamingSpec
        {
            Version = 1,
            Projects = new Dictionary<string, ProjectSpec>(StringComparer.Ordinal)
            {
                ["MustClauses"] = new ProjectSpec
                {
                    ProjectPath = "src/PineGuard.MustClauses/PineGuard.MustClauses.csproj",
                    Namespace = "PineGuard.MustClauses",
                    ContainingTypeNameRegex = "^Must.+Clauses$",
                    ExtensionReceiverFullyQualifiedType = "global::PineGuard.MustClauses.IMustClause"
                }
            },
            Policies = new PoliciesSpec
            {
                MustClauses = new MustClausesPolicySpec
                {
                    PrimaryParamPolicy = new PrimaryParamPolicySpec
                    {
                        RequireNullableReferenceTypePrimaryParams = true,
                        RequireNonNullableValueTypePrimaryParams = true
                    },
                    CollisionChecks = new CollisionChecksSpec
                    {
                        NullLiteralAmbiguity = new NullLiteralAmbiguityCheckSpec
                        {
                            Enabled = true,
                            ConsiderOptionalTrailingParametersCallableWithNullOnly = true
                        }
                    },
                    Exemptions = []
                }
            }
        };
    }
}

internal sealed record ProjectSpec
{
    public string ProjectPath { get; init; } = string.Empty;
    public string Namespace { get; init; } = string.Empty;
    public string ContainingTypeNameRegex { get; init; } = string.Empty;
    public string ExtensionReceiverFullyQualifiedType { get; init; } = string.Empty;
}

internal sealed record PoliciesSpec
{
    public MustClausesPolicySpec MustClauses { get; init; } = new();
}

internal sealed record MustClausesPolicySpec
{
    public PrimaryParamPolicySpec PrimaryParamPolicy { get; init; } = new();
    public CollisionChecksSpec CollisionChecks { get; init; } = new();
    public List<ExemptionSpec> Exemptions { get; init; } = [];
}

internal sealed record PrimaryParamPolicySpec
{
    public bool RequireNullableReferenceTypePrimaryParams { get; init; } = true;
    public bool RequireNonNullableValueTypePrimaryParams { get; init; } = true;
}

internal sealed record CollisionChecksSpec
{
    public NullLiteralAmbiguityCheckSpec NullLiteralAmbiguity { get; init; } = new();
}

internal sealed record NullLiteralAmbiguityCheckSpec
{
    public bool Enabled { get; init; } = true;
    public bool ConsiderOptionalTrailingParametersCallableWithNullOnly { get; init; } = true;
}

internal sealed record ExemptionSpec
{
    public string MethodId { get; init; } = string.Empty;
    public string? PrimaryParamTypeDisplay { get; init; }
    public bool AllowNullablePrimaryParam { get; init; }
    public string Reason { get; init; } = string.Empty;
}

internal sealed record AuditReport
{
    public AuditMetadata Metadata { get; init; } = new();
    public AuditSummary Summary { get; init; } = new();
    public List<MethodRecord> Methods { get; init; } = [];
    public CollisionReport Collisions { get; init; } = new();
}

internal sealed record AuditMetadata
{
    public int SpecVersion { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public string ProjectPath { get; init; } = string.Empty;
    public string SpecPath { get; init; } = string.Empty;
    public string GeneratedAtUtc { get; init; } = string.Empty;

    public static AuditMetadata Create(CliOptions options, NamingSpec spec, string resolvedProjectPath)
        => new()
        {
            SpecVersion = spec.Version,
            ProjectName = options.Project,
            ProjectPath = resolvedProjectPath,
            SpecPath = options.SpecPath,
            GeneratedAtUtc = DateTime.UtcNow.ToString("O")
        };
}

internal sealed record AuditSummary
{
    public int MethodsCount { get; init; }
    public int ViolationsCount { get; init; }
    public int NullLiteralAmbiguityGroupsCount { get; init; }

    public static AuditSummary From(List<MethodRecord> methods, CollisionReport collision)
        => new()
        {
            MethodsCount = methods.Count,
            ViolationsCount = methods.Count(m => !m.Compliance.IsCompliant),
            NullLiteralAmbiguityGroupsCount = collision.NullLiteralAmbiguityGroups.Count
        };
}

internal sealed record MethodRecord
{
    public string MethodId { get; init; } = string.Empty;
    public string ContainingType { get; init; } = string.Empty;
    public string Namespace { get; init; } = string.Empty;
    public string MethodName { get; init; } = string.Empty;
    public string SourceFile { get; init; } = string.Empty;
    public string SignatureDisplay { get; init; } = string.Empty;

    public bool IsGenericMethod { get; init; }
    public int GenericArity { get; init; }

    public PrimaryParamRecord PrimaryParam { get; init; } = new();
    public MethodCompliance Compliance { get; init; } = new();

    public int TotalParameterCount { get; init; }
    public int NonReceiverParameterCount { get; init; }
    public bool CallableWithOnlyPrimaryParam { get; init; }

    public static MethodRecord From(IMethodSymbol method, string sourceFile, PrimaryParamInfo primary, ExpectedNullability expected)
    {
        var nonReceiverParameters = method.Parameters.Skip(1).ToArray();
        var callableWithOnlyPrimaryParam = nonReceiverParameters.Length == 1 || nonReceiverParameters.Skip(1).All(p => p.IsOptional);

        var methodId = MethodIds.Build(method);

        return new MethodRecord
        {
            MethodId = methodId,
            ContainingType = method.ContainingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            Namespace = method.ContainingNamespace.ToDisplayString(),
            MethodName = method.Name,
            SourceFile = sourceFile,
            SignatureDisplay = method.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            IsGenericMethod = method.IsGenericMethod,
            GenericArity = method.TypeParameters.Length,
            TotalParameterCount = method.Parameters.Length,
            NonReceiverParameterCount = nonReceiverParameters.Length,
            CallableWithOnlyPrimaryParam = callableWithOnlyPrimaryParam,
            PrimaryParam = PrimaryParamRecord.From(primary),
            Compliance = MethodCompliance.From(primary, expected)
        };
    }
}

internal sealed record PrimaryParamRecord
{
    public string Name { get; init; } = string.Empty;
    public string TypeDisplay { get; init; } = string.Empty;
    public string TypeKind { get; init; } = string.Empty;

    public bool IsReferenceType { get; init; }
    public bool IsValueType { get; init; }
    public bool IsTypeParameter { get; init; }

    public bool IsStruct { get; init; }
    public bool IsClass { get; init; }
    public bool IsInterface { get; init; }
    public bool IsEnum { get; init; }

    public bool IsNullableValueType { get; init; }
    public string NullableAnnotation { get; init; } = string.Empty;

    public bool AllowsNullLiteral { get; init; }

    public static PrimaryParamRecord From(PrimaryParamInfo p)
        => new()
        {
            Name = p.Name,
            TypeDisplay = p.TypeDisplay,
            TypeKind = p.TypeKind,
            IsReferenceType = p.IsReferenceType,
            IsValueType = p.IsValueType,
            IsTypeParameter = p.IsTypeParameter,
            IsStruct = p.IsStruct,
            IsClass = p.IsClass,
            IsInterface = p.IsInterface,
            IsEnum = p.IsEnum,
            IsNullableValueType = p.IsNullableValueType,
            NullableAnnotation = p.NullableAnnotation,
            AllowsNullLiteral = p.AllowsNullLiteral
        };
}

internal sealed record MethodCompliance
{
    public bool ExpectedPrimaryParamNullable { get; init; }
    public bool ActualPrimaryParamNullable { get; init; }
    public bool IsCompliant { get; init; }
    public string Rule { get; init; } = string.Empty;
    public bool IsExempted { get; init; }
    public string? ExemptionReason { get; init; }

    public static MethodCompliance From(PrimaryParamInfo primary, ExpectedNullability expected)
    {
        var actualNullable = primary.IsNullableAnnotated || primary.IsNullableValueType;

        if (expected.Exempted)
        {
            return new MethodCompliance
            {
                ExpectedPrimaryParamNullable = expected.ExpectedNullable,
                ActualPrimaryParamNullable = actualNullable,
                IsCompliant = true,
                Rule = expected.Rule,
                IsExempted = true,
                ExemptionReason = expected.ExemptionReason
            };
        }

        return new MethodCompliance
        {
            ExpectedPrimaryParamNullable = expected.ExpectedNullable,
            ActualPrimaryParamNullable = actualNullable,
            IsCompliant = expected.ExpectedNullable == actualNullable,
            Rule = expected.Rule,
            IsExempted = false
        };
    }
}

internal sealed record CollisionReport
{
    public List<NullLiteralAmbiguityGroup> NullLiteralAmbiguityGroups { get; init; } = [];
}

internal sealed record NullLiteralAmbiguityGroup
{
    public string MethodName { get; init; } = string.Empty;
    public List<string> Candidates { get; init; } = [];
    public string Reason { get; init; } = string.Empty;
}

internal static class CollisionAnalysis
{
    public static CollisionReport Analyze(List<MethodRecord> methods, Compilation compilation, CollisionChecksSpec checks)
    {
        if (!checks.NullLiteralAmbiguity.Enabled)
            return new CollisionReport();

        var candidates = methods
            // Generic methods are typically NOT callable as `X(null)` because type inference cannot infer
            // type arguments from a null literal. Callers must specify type arguments explicitly.
            // Since this audit is targeting Must.Be.X(null) ambiguity, exclude generic methods here.
            .Where(m => !m.IsGenericMethod)
            .Where(m => m.PrimaryParam.AllowsNullLiteral)
            .Where(m => !checks.NullLiteralAmbiguity.ConsiderOptionalTrailingParametersCallableWithNullOnly || m.CallableWithOnlyPrimaryParam)
            .ToList();

        var byName = candidates.GroupBy(m => m.MethodName, StringComparer.Ordinal);
        var groups = new List<NullLiteralAmbiguityGroup>();

        foreach (var group in byName)
        {
            var list = group.ToList();
            if (list.Count < 2)
                continue;

            // A "real" ambiguity risk for null is when there exists at least one pair of primary param types
            // that are unrelated (no implicit conversion either direction). If all candidates form a chain
            // (e.g., string? vs object?), overload resolution tends to pick the most specific.
            var anyUnrelatedPair = false;

            for (var i = 0; i < list.Count && !anyUnrelatedPair; i++)
            {
                for (var j = i + 1; j < list.Count && !anyUnrelatedPair; j++)
                {
                    var t1 = list[i].PrimaryParam.TypeDisplay;
                    var t2 = list[j].PrimaryParam.TypeDisplay;

                    // We only have display strings in MethodRecord; treat different displays as different types.
                    // This is a conservative check: if types differ AND neither is clearly "object", flag.
                    if (!string.Equals(t1, t2, StringComparison.Ordinal) &&
                        !string.Equals(t1, "object?", StringComparison.Ordinal) &&
                        !string.Equals(t2, "object?", StringComparison.Ordinal))
                    {
                        anyUnrelatedPair = true;
                    }
                }
            }

            if (!anyUnrelatedPair)
                continue;

            groups.Add(new NullLiteralAmbiguityGroup
            {
                MethodName = group.Key,
                Candidates = list
                    .Select(m => $"{m.MethodId}({m.PrimaryParam.TypeDisplay})")
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToList(),
                Reason = "Multiple overloads appear callable with null and have differing primary param types. Review for Must.Be.X(null) ambiguity."
            });
        }

        return new CollisionReport
        {
            NullLiteralAmbiguityGroups = groups.OrderBy(g => g.MethodName, StringComparer.Ordinal).ToList()
        };
    }
}

internal sealed record ExpectedNullability(bool ExpectedNullable, string Rule, bool Exempted, string? ExemptionReason)
{
    public static ExpectedNullability Compute(string methodId, PrimaryParamInfo primary, MustClausesPolicySpec policy)
    {
        var exemption = policy.Exemptions.FirstOrDefault(e =>
            string.Equals(e.MethodId, methodId, StringComparison.Ordinal) &&
            (e.PrimaryParamTypeDisplay is null || string.Equals(e.PrimaryParamTypeDisplay, primary.TypeDisplay, StringComparison.Ordinal)));

        if (exemption is not null)
        {
            return new ExpectedNullability(
                ExpectedNullable: exemption.AllowNullablePrimaryParam,
                Rule: "exemption",
                Exempted: true,
                ExemptionReason: exemption.Reason);
        }

        if (primary.IsReferenceType && policy.PrimaryParamPolicy.RequireNullableReferenceTypePrimaryParams)
        {
            return new ExpectedNullability(
                ExpectedNullable: true,
                Rule: "mustclauses.primaryParam.refType.mustBeNullable",
                Exempted: false,
                ExemptionReason: null);
        }

        if (primary.IsValueType && policy.PrimaryParamPolicy.RequireNonNullableValueTypePrimaryParams)
        {
            return new ExpectedNullability(
                ExpectedNullable: false,
                Rule: "mustclauses.primaryParam.valueType.mustBeNonNullable",
                Exempted: false,
                ExemptionReason: null);
        }

        // Type-parameter fallback: no strong expectation.
        var actualNullable = primary.IsNullableAnnotated || primary.IsNullableValueType;
        return new ExpectedNullability(
            ExpectedNullable: actualNullable,
            Rule: "no-policy",
            Exempted: false,
            ExemptionReason: null);
    }
}

internal sealed record PrimaryParamInfo(
    string Name,
    string TypeDisplay,
    string TypeKind,
    bool IsReferenceType,
    bool IsValueType,
    bool IsTypeParameter,
    bool IsStruct,
    bool IsClass,
    bool IsInterface,
    bool IsEnum,
    bool IsNullableValueType,
    bool IsNullableAnnotated,
    string NullableAnnotation,
    bool AllowsNullLiteral)
{
    public static PrimaryParamInfo From(IParameterSymbol p)
    {
        var type = p.Type;

        var isNullableValueType = type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
        var isNullableAnnotated = p.NullableAnnotation == global::Microsoft.CodeAnalysis.NullableAnnotation.Annotated;

        var isStruct = type.TypeKind == global::Microsoft.CodeAnalysis.TypeKind.Struct;
        var isClass = type.TypeKind == global::Microsoft.CodeAnalysis.TypeKind.Class;
        var isInterface = type.TypeKind == global::Microsoft.CodeAnalysis.TypeKind.Interface;
        var isEnum = type.TypeKind == global::Microsoft.CodeAnalysis.TypeKind.Enum;

        var isTypeParameter = type.TypeKind == global::Microsoft.CodeAnalysis.TypeKind.TypeParameter;

        var allowsNullLiteral = type.IsReferenceType || isNullableValueType;

        return new PrimaryParamInfo(
            Name: p.Name,
            TypeDisplay: type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            TypeKind: type.TypeKind.ToString(),
            IsReferenceType: type.IsReferenceType,
            IsValueType: type.IsValueType,
            IsTypeParameter: isTypeParameter,
            IsStruct: isStruct,
            IsClass: isClass,
            IsInterface: isInterface,
            IsEnum: isEnum,
            IsNullableValueType: isNullableValueType,
            IsNullableAnnotated: isNullableAnnotated,
            NullableAnnotation: p.NullableAnnotation.ToString(),
            AllowsNullLiteral: allowsNullLiteral);
    }
}
