<!-- metadata_header
type: plan
id: new-surfaces-06-analyzers
version: 1.2
status: planned
last_updated: 2026-08-26
parent: new-surfaces-program
-->

# Plan 06 — Phase 6: `PineGuard.Analyzers`

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · **06 Analyzers**
<!-- /plan-nav -->

> **Status**: Planned | **Depends on**: a stable Guard surface (nothing structural; runs last by choice) and Phase 1's `MustResult<T>` shape | **Unblocks**: nothing — leaf phase
>
> **Worktree**: `.claude/worktrees/analyzers` on `feature/analyzers`.
>
> Read [Plan 00](new-surfaces-missing-validation-cases-00-program.md) first; onboarding per Plan 02 §3.4's log, with the TFM exceptions in §3.1 below.

## 1. Business plan

### 1.1 The idea

Serilog grew through its sinks; PineGuard's equivalent is an analyzer. Every codebase that installs `PineGuard.Analyzers` gets a continuous, in-editor suggestion — *this hand-rolled null check is `Guard.Against.Null(x)`* — with a one-click fix. It is a distribution channel as much as a feature, and no competitor ships one. A second diagnostic pays for itself on day one: a `Must.Be.X(...)` whose result is discarded is a silent no-op, and today nothing warns.

### 1.2 Why last

The analyzer advertises the Guard surface; it should advertise names that will not change. Phases 1–5 settle the vocabulary first.

### 1.3 Success metrics

- `PineGuard.Analyzers` installs as a development dependency into any C# project and reports nothing until `PineGuard.GuardClauses` / `PineGuard.MustClauses` are referenced (no noise for non-users).
- Six diagnostics with code fixes; fix-all supported; 100 %/100 % on both analyzer assemblies.
- The README shows a before/after for each diagnostic.

## 2. Functional plan

### 2.1 Diagnostics

| Id | Title | Category | Default severity | Fires on | Fix |
|---|---|---|---|---|---|
| `PG1001` | Use `Guard.Against.Null` | Usage | Info | `if (x is null) throw new ArgumentNullException(nameof(x));` (also `== null`, `x ?? throw new ArgumentNullException(…)`, `ArgumentNullException.ThrowIfNull(x)`) | `Guard.Against.Null(x);` (statement form) or `Guard.Against.Null(x)` (expression form replacing the `??` throw); adds `using PineGuard.GuardClauses;` |
| `PG1002` | Use `Guard.Against.NullOrWhiteSpace` | Usage | Info | `if (string.IsNullOrWhiteSpace(x)) throw new ArgumentException(…);`, `ArgumentException.ThrowIfNullOrWhiteSpace(x)` | `Guard.Against.NullOrWhiteSpace(x);` |
| `PG1003` | Use `Guard.Against.NullOrEmpty` | Usage | Info | `string.IsNullOrEmpty` throw forms, `ArgumentException.ThrowIfNullOrEmpty(x)` | `Guard.Against.NullOrEmpty(x);` |
| `PG1004` | Use `Guard.Against.OutOfRange` | Usage | Info | `if (x < min \|\| x > max) throw new ArgumentOutOfRangeException(nameof(x));` where all three operands are simple identifiers/literals and the guarded identifier is the same on both sides | `Guard.Against.OutOfRange(x, min, max);` (verify the exact Guard name at implementation — `guard-clauses/project.md` §5.4.5 prescribes `OutOfRange`) |
| `PG2001` | Must result is discarded | Reliability | Warning | An expression statement whose expression type is `PineGuard.MustClauses.MustResult<T>` (i.e. `Must.Be.X(...);` on its own line) | two fixes: *Throw if failed* (`….ThrowIfFailed();`) and *Assign the result* (`var result = …;`) |
| `PG2002` | Must validation result is discarded | Reliability | Warning | An expression statement whose expression type is `PineGuard.MustClauses.MustValidationResult` (`validator.Validate(order);` on its own line — the same silent no-op) | the same two fixes |

Rules:

- `PG1xxx` report only when the compilation can resolve `PineGuard.GuardClauses.Guard` (`Compilation.GetTypeByMetadataName`); `PG2001`/`PG2002` only when they can resolve ``PineGuard.MustClauses.MustResult`1`` / `PineGuard.MustClauses.MustValidationResult`. Otherwise the analyzers are silent — the package never nags a project that has not chosen PineGuard.
- Diagnostics never fire inside `PineGuard.*` assemblies themselves (assembly-name prefix check) — Core's `ThrowHelper` is exactly the pattern PG1001 targets and must stay.
- Fix-all provider: `WellKnownFixAllProviders.BatchFixer` for all fixes.
- The `PG` prefix is unclaimed by the well-known families (CA, CS, IDE, SA, S, MA, VSTHRD, xUnit); `1xxx` = "prefer a Guard", `2xxx` = "misuse".

### 2.2 Acceptance criteria

- [ ] Every row in §2.1 has analyzer + fix, positive and negative test cases, and a README before/after.
- [ ] `AnalyzerReleases.Shipped.md` / `AnalyzerReleases.Unshipped.md` maintained (RS2008 is an error under the repo's warning policy).
- [ ] Package layout verified by unpacking the `.nupkg`: both DLLs under `analyzers/dotnet/cs`, no `lib/` folder, `developmentDependency` true.
- [ ] Plan 00 §7; scope onboarded.

### 2.3 Not in this phase

Alias discoverability suggestions (`future-language.md` §7.2) — a later diagnostic family once that vocabulary is decided. Analyzers for FluentValidation/DataAnnotations usage. Source generators.

## 3. Technical plan

### 3.1 Projects

| Project | Path | TFM | Packable | References |
|---|---|---|---|---|
| `PineGuard.Analyzers` | `+ src/PineGuard.Analyzers/` | `netstandard2.0` (csproj override — Roslyn analyzers must target it) | yes — packs **both** assemblies | `Microsoft.CodeAnalysis.CSharp` (new `PackageVersion` 4.14.0 in the root `Directory.Packages.props` — Tier 1; it is **not** pinned today, only `Microsoft.CodeAnalysis.CSharp.Workspaces`/`Workspaces.MSBuild` are, and the existing comment there says to stay on the 4.x family), `Microsoft.CodeAnalysis.Analyzers` (`PrivateAssets="all"`) |
| `PineGuard.Analyzers.CodeFixes` | `+ src/PineGuard.Analyzers.CodeFixes/` | `netstandard2.0` | no (`IsPackable=false`; its DLL is packed by the analyzer project) | `PineGuard.Analyzers`, `Microsoft.CodeAnalysis.CSharp.Workspaces` |

Analyzer csproj essentials: `IsRoslynComponent=true`, `EnforceExtendedAnalyzerRules=true`, `IncludeBuildOutput=false`, `SuppressDependenciesWhenPacking=true`, `DevelopmentDependency=true`, and

```xml
<ItemGroup>
  <None Include="$(OutputPath)\$(AssemblyName).dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
  <None Include="$(OutputPath)\PineGuard.Analyzers.CodeFixes.dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
</ItemGroup>
```

(plus a `ProjectReference` to CodeFixes with `ReferenceOutputAssembly="false"` so it builds first — which also means the CodeFixes DLL is **not** copied into the analyzer's `197609OutputPath)`, so the second `<None Include>` above must point at `..\PineGuard.Analyzers.CodeFixes\197609OutputPath)\PineGuard.Analyzers.CodeFixes.dll`, and the package-layout assertion of §2.2 runs in W1's first build, not W5). `Directory.Build.props` still applies: nullable, warnings-as-errors, XML docs (analyzer types are public and documented), MinVer/SourceLink. Namespaces `PineGuard.Analyzers` and `PineGuard.Analyzers.CodeFixes`.

Files (`+ src/PineGuard.Analyzers/…`): `DiagnosticIds.cs` (`internal static class` of `const string`), `DiagnosticDescriptors.cs` (`internal`) — both names are universally owned, and neither needs to be public: consumers never reference an analyzer assembly (`docs/ai/specs/language/naming-collisions.md` §Type names vs vocabulary records the exception) — `PreferGuardAnalyzer.cs` (one `DiagnosticAnalyzer` owning `PG1001`–`PG1004`, registering syntax-node actions for `IfStatement`, `ThrowExpression`, `InvocationExpression`), `DiscardedMustResultAnalyzer.cs` (`PG2001`, operation action on `IExpressionStatementOperation`), `PineGuardTypes.cs` (internal well-known-type lookup, cached per compilation via `RegisterCompilationStartAction`). CodeFixes: `PreferGuardCodeFixProvider.cs`, `DiscardedMustResultCodeFixProvider.cs`, `GuardSyntaxFactory.cs` (internal: builds `Guard.Against.X(args)` invocations and the `using`). `README.md`, `AGENTS.md`, `AnalyzerReleases.Shipped.md`, `AnalyzerReleases.Unshipped.md` beside the analyzer csproj.

### 3.2 Onboarding differences

Plan 00 §8 applies with these deltas: two source projects map to one test project and one coverage scope (`Analyzers`). The coverage scripts resolve one source directory and one path regex per scope, so the registry entry is `SourceDir = src/PineGuard.Analyzers` (existence probe), `PathRegex = '(?i)(^|[\\/])(src[\\/]+)?PineGuard\.Analyzers(\.CodeFixes)?[\\/]'` and `IncludePattern = @('[PineGuard.Analyzers]*', '[PineGuard.Analyzers.CodeFixes]*')` — the `switch` blocks take the regex, not an array of directories; `ci.yml` filter covers both `src/` folders; Qodana slnx lists both; `tools/release/Run-GithubRelease.ps1` / `Run-NugetUnlist.ps1` list `PineGuard.Analyzers` only; the `.editorconfig` test brace list gains `PineGuard.Analyzers.UnitTests`; Rule53 maps `PineGuard.Analyzers.UnitTests` → `src/PineGuard.Analyzers` — code-fix assertions live inside the analyzer test classes as `Fix` operation groups so no orphan allowlist entry is needed.

### 3.3 Docs

`README.md` with a table of the five diagnostics and a before/after per row; root README *Analyzers* subsection and package table row; `docs/ai/specs/analyzers/` triad (the `project.md` states: analyzers are silent without the PineGuard reference; never fire in `PineGuard.*`; every diagnostic has a fix and a fix-all; release tracking files are mandatory).

## 4. Testing plan

Project `+ tests/PineGuard.Analyzers.UnitTests/` (`net8.0;net10.0`), test-only packages `Microsoft.CodeAnalysis.CSharp.Analyzer.Testing` and `Microsoft.CodeAnalysis.CSharp.CodeFix.Testing` (the current `Testing` line with `DefaultVerifier`), ordinary `ProjectReference`s (default `ReferenceOutputAssembly`) to **both** analyzer projects so both DLLs and PDBs land in the test output — verify with `Get-ChildItem tests/PineGuard.Analyzers.UnitTests/bin/Debug/net10.0 -Filter PineGuard.Analyzers*.pdb` before the first coverage run — **and** to `PineGuard.Core`, `PineGuard.MustClauses`, `PineGuard.GuardClauses` (their built assemblies are added to `TestState.AdditionalReferences` so test sources can resolve `Guard`/`Must`). Every `XxxTests.cs` ships with `XxxTestData.cs` (Rule50).

Base `BaseUnitTest`; project-local records:

```csharp
public sealed record AnalyzerExpected(bool IsValid, string? Message = null, string? DiagnosticId = null, int? Line = null, int? Column = null, string? FixedSource = null) : ReturnExpected(IsValid, Message);
public sealed record AnalyzerCase(string Name, string Source, AnalyzerExpected Expected) : ReturnCase<string, AnalyzerExpected>(Name, Source, Expected);   // the positional is the C# source, named so (Rule54 reads `Value` as the value under test)
```

Sources are C# snippets held as `const string` fields in the TestData shared-fields section (they are test data, not fixtures — no cross-layer reuse). Helper methods at the bottom of each TestData class build the `CSharpAnalyzerTest`/`CSharpCodeFixTest` with the reference set. Async test methods use the Phase 3 `public async Task` form.

| Tests | Groups |
|---|---|
| `PreferGuardAnalyzerTests` | `PG1001` (is-null / == null / `??` throw / `ThrowIfNull`; negatives: different exception, non-null check, inside `PineGuard.*` assembly name, no Guard reference), `PG1002`, `PG1003`, `PG1004` (positive, negative operand shapes); `Fix` groups per id (single fix, fix-all across two sites, `using` added once, existing `using` preserved) |
| `DiscardedMustResultAnalyzerTests` | `PG2001` and `PG2002` (statement discard; negatives: assigned, returned, awaited, `.ThrowIfFailed()` chained, `_ =` discard, no MustClauses reference); `Fix` (both fixes, both ids) |
| `DiagnosticDescriptorsTests` | every descriptor has id/title/category/help link/enabled-by-default; ids unique and in the `PG\d{4}` shape |

Coverage: `-Scope Analyzers` 100/100 (analyzers run in-process under the test host, so Coverlet sees them); `-Scope All` 100/100.

## 5. Playbook

**W0** Plan 00 §6 (`<slug> = analyzers`); read `docs/ai/specs/spec.md`, `docs/ai/specs/guard-clauses/project.md` (§5.4 vocabulary — the fixes must emit the real Guard names), Plan 02 §3.4 log; baseline gates.

**W1** Onboard the `Analyzers` scope with the §3.2 deltas; both csprojs (TFM overrides, packing); test csproj; `dotnet build` clean; commit `build(analyzers): add analyzer, code-fix and test projects`.

**W2** `DiagnosticIds`, `DiagnosticDescriptors`, `PineGuardTypes`, `PreferGuardAnalyzer` (`PG1001`–`PG1004`); tests; commit `feat(analyzers): suggest Guard clauses for hand-rolled argument checks`.

**W3** `PreferGuardCodeFixProvider` + `GuardSyntaxFactory`; fix tests incl. fix-all; commit `feat(analyzers): add code fixes for PG1001-PG1004`.

**W4** `DiscardedMustResultAnalyzer` + fix; tests; commit `feat(analyzers): warn when a Must result is discarded`.

**W5** `dotnet pack src/PineGuard.Analyzers -c Release -o artifacts/nupkg` and inspect the package layout (unzip; assert `analyzers/dotnet/cs/*.dll`, no `lib/`); release-tracking files; README; commit `build(analyzers): pack both assemblies as a development dependency`.

**W6** Brain/agents/README (Rule11/12); `-Scope Analyzers` and `-Scope All` 100/100; Plan 00 §7; PR; merge; cleanup.

## 6. Definition of Done

Plan 00 §7, plus §2.2 in full; the unpacked package layout checked; a scratch console project under `artifacts/analyzer-smoke/` inside the worktree (with a local `nuget.config` whose `<packageSources>` points at `artifacts/nupkg`; never outside the worktree — Plan 00 §6 and the output-dir hook) that references the packed `.nupkg` and `PineGuard.GuardClauses` shows `PG1001` in `dotnet build` output (recorded in the PR body).

## 7. Risks

| Risk | Mitigation |
|---|---|
| Analyzer package noise in projects that do not use PineGuard | Well-known-type gating; tested |
| Fixes emit a Guard name that no longer exists | Names are read from `guard-clauses/project.md`; the test project references the real `PineGuard.GuardClauses` assembly so a wrong name fails compilation of the fixed source |
| `TreatWarningsAsErrors` + `EnforceExtendedAnalyzerRules` friction (RS1xxx/RS2xxx) | Fix root causes (release tracking, `ConfigureGeneratedCodeAnalysis`, `EnableConcurrentExecution`); never `NoWarn` |
| Roslyn version skew with consumers' SDKs | 4.14 is conservative for .NET 10-era SDKs; the README states the minimum SDK |

## 8. Out of scope

Alias suggestions, FluentValidation/DataAnnotations analyzers, source generators, IDE-specific refactorings beyond code fixes.

<!-- footer
last_verified: 2026-08-26
-->

<!-- plan-nav -->
> [Parent](new-surfaces-missing-validation-cases.md) · [00 Program](new-surfaces-missing-validation-cases-00-program.md) · [01 Structural validation](new-surfaces-missing-validation-cases-01-structural-validation.md) · [02 Options](new-surfaces-missing-validation-cases-02-options.md) · [03 ASP.NET Core](new-surfaces-missing-validation-cases-03-aspnetcore.md) · [04 MediatR & bridges](new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md) · [05 Rule batches](new-surfaces-missing-validation-cases-05-rule-batches.md) · **06 Analyzers**
<!-- /plan-nav -->
