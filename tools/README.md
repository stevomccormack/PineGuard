# PineGuard Tools

PowerShell scripts for building, testing, auditing, formatting, generating, and maintaining the PineGuard codebase.

## Prerequisites

- PowerShell 7+ (`pwsh`)
- .NET 10 SDK (builds every target framework). Libraries multi-target `netstandard2.1;net8.0;net10.0`; test projects target `net8.0;net10.0`.

## Tool Directories

| Directory | Purpose | Entry Point | Scopes |
|-----------|---------|-------------|--------|
| [code-coverage](code-coverage/README.md) | Coverage collection and analysis — Coverlet (Cobertura + HTML, authoritative) and JetBrains dotCover (Rider `.dcvr` snapshots) behind one front door | `Run-CodeCoverage.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All (plus Custom on `Test-Coverage.ps1` only) |
| [code-diagnostics](code-diagnostics/README.md) | Roslyn compiler warning capture and reporting | `Run-CompilerDiagnostics.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All |
| [code-format](code-format/README.md) | `dotnet format` wrapper with scope support | `Run-Format.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All |
| [code-scan/qodana](code-scan/qodana/README.md) | JetBrains Qodana static inspection | `Run-Qodana.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All |
| [code-scan/sonarqube](code-scan/sonarqube/README.md) | SonarQube static analysis scanning | `Run-SonarScanner.ps1` | Project-level scanning |
| [git](git/README.md) | Scoped commit orchestration | `Run-Commits.ps1` | Registry scopes: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing. Meta-scopes: Agent, Docs, Tools, Solution, Ci |
| [clean](clean/README.md) | Cleanup of artifacts, logs, and root build files; structural-integrity checks after folder/namespace moves | `Run-Clean.ps1`, `Test-StructuralIntegrity.ps1` | `-Target Logs`, `-Target Artifacts`, `-Target Root`; Build, Test, Paths, Namespaces, Sonar, All |
| [github](github/README.md) | GitHub Release publishing and ruleset toggles | `Run-Release.ps1`, `Set-GithubRuleset.ps1` | `-BypassPR`, `-Draft`, `-Force`, `-Watch`, `-Unlist`, `-WhatIf`; `Enable`/`Disable` + `-Name` |
| [nuget](nuget/README.md) | NuGet package management | `Unpublish-NugetPrerelease.ps1` | `-Package`, `-All`, `-WhatIf`/`-DryRun`, `-Force`, `-EnvFile` |
| [testing](testing/README.md) | `dotnet test` wrapper with scope, project or solution targeting and async support | `Run-Tests.ps1` | `-Scope` (the fourteen registry scopes plus All), or `-Project` / `-Solution` |

### Internal Directories

| Directory | Purpose |
|-----------|---------|
| [.shared](.shared/) | Shared PowerShell helper modules (path, project registry, coverage, git, Docker, SonarQube, dotenv, secrets, transcript, console, clean, commands) imported by other tools |
| [.tests](.tests/README.md) | Pester suite for `tools/**` (D-7) — registry parity, Cobertura/dotenv parsers, repo-root resolution, git helpers, and BOM/Windows-ism/help hygiene. Run via [`testing/Test-Tools.ps1`](testing/Test-Tools.ps1) |
| [docker](docker/README.md) | Docker Compose stacks, the shared network helper, and the combined up/down scripts backing the two containerised scanners (Qodana, SonarQube) |

## Related Surfaces

`tools/` is the **portable** PowerShell surface: self-contained, CI-safe, no external
dependencies. [`.etc/powershell/`](../.etc/powershell/README.md) is the **maintainer workstation**
surface — one-time clone bring-up plus GitHub and nuget.org account administration. It follows the
same Verb-Noun spec, but depends on the maintainer's personal `Onboarding` helper library and
carries their identity, so nothing there ever runs in CI.

Three scripts exist on both surfaces; the maintainer-shell README says which to prefer in each
case. `.etc/powershell/.env` is the repository's single credential file, read from `tools/` via
[`.shared/secret.ps1`](.shared/secret.ps1).

## Running Scripts

All scripts should be run from the **repository root**:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/<directory>/<Script>.ps1" [parameters]
```

## Standard Parameters

These parameters are common across multiple tools:

| Parameter | Tools | Description |
|-----------|-------|-------------|
| `-Scope` | code-coverage, code-diagnostics, code-format, code-scan/qodana, testing, git, clean (`Test-StructuralIntegrity.ps1`) | Named scope resolving to that scope's project(s). The fourteen registry scopes come from `tools/.shared/dotnet-projects.ps1`; `git` adds the five meta-scopes and `clean` uses its own check names |
| `-Engine` | code-coverage | `Coverlet` (default, authoritative) or `DotCover`. Validated and delegated by the `New-CoverageReport.ps1` front door (D-9) |
| `-Configuration` | code-coverage, code-diagnostics, code-format, testing | `Debug` (default) or `Release` |
| `-Clean` | code-coverage, code-diagnostics, code-scan/qodana | Delete previous output / run a clean build before collecting |
| `-WhatIf` / `-DryRun` | git, github, nuget, clean | Preview the plan without making changes. `-DryRun` is a declared alias of the same switch on `git`, `github` and `nuget` (D-1d); `clean` exposes the native `SupportsShouldProcess` `-WhatIf` only |
| `-Filter` | code-coverage, testing | `dotnet test`'s own `--filter` expression. Reserved exclusively for that meaning — a diagnostic-code regex is `-Code`, and a test-project glob is `-ProjectFilter` |
| `-Framework` | code-coverage, testing | Target framework moniker (e.g. `net10.0`) passed through as `--framework` |
| `-NoRestore` | code-format | Skip the implicit restore phase |
| `-OutputPath` | testing | Directory for test results (trx). Relative paths resolve against the repository root |
| `-OpenReport` / `-Open` | code-scan/qodana (`-OpenReport`), code-scan/sonarqube (`-Open`) | Open the generated report or the server UI in a browser afterwards |
| `-Token` | code-scan/qodana | Qodana Cloud token; falls back to `QODANA_TOKEN`. SonarQube's equivalent is `-ProjectToken`, resolved through `Resolve-SonarQubeToken` / `.shared/secret.ps1` (D-4) |
| `-Force` | github, nuget | Skip the interactive confirmation prompt |
| `-TimeoutMinutes` | code-scan/qodana | Hard timeout (1–1440); exceeding it exits with `-TimeoutExitCode` (default `124`) |

## Output Conventions

- All runtime output goes to `artifacts/` or `logs/`
- Scripts NEVER create files in the project root

## Specifications

For normative rules governing tool implementation:

- Root tool spec: `docs/ai/specs/tools/spec.md`
- Safety tiers: `docs/ai/specs/safety.md`

**Note:** The audit CLI was migrated to TypeScript as `pnpm -C apps/cli exec tsx src/index.ts audit`. See `apps/cli/README.md` and `docs/ai/agents/audit-cli.md` for the new command usage.
