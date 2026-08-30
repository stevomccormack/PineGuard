# PineGuard Tools

PowerShell scripts for building, testing, auditing, formatting, generating, and maintaining the PineGuard codebase.

## Prerequisites

- PowerShell 7+ (`pwsh`)
- .NET 10 SDK (builds every target framework). Libraries multi-target `netstandard2.1;net8.0;net10.0`; test projects target `net8.0;net10.0`; `tools/audit-cli` targets `net10.0`.

## Tool Directories

| Directory | Purpose | Entry Point | Scopes |
|-----------|---------|-------------|--------|
| [audit-cli](audit-cli/README.md) | Static analysis, parity checks, naming audits | `Run-All.ps1` | Rule filtering (`-RuleId`, `-RuleName`) |
| [code-coverage](code-coverage/README.md) | Cobertura coverage collection and analysis | `Run-CodeCoverage.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, Testing, All, Custom |
| [code-diagnostics](code-diagnostics/README.md) | Roslyn compiler warning capture and reporting | `Run-CompilerDiagnostics.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, Testing, All |
| [code-formatter](code-formatter/README.md) | `dotnet format` wrapper with scope support | `Run-Format.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, Testing, All |
| [code-inspection](code-inspection/README.md) | JetBrains Qodana static inspection | `Run-Qodana.ps1` | Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, Testing, All |
| [docker](docker/README.md) | Docker Compose stacks for Qodana and SonarQube | `docker-up.ps1` | Qodana, SonarQube |
| [git](git/README.md) | Scoped commit orchestration | `Run-Commits.ps1` | Agent, Core, DataAnnotations, Docs, FluentValidation, GuardClauses, MustClauses, Options, Testing, Tools, Solution |
| [maintenance](maintenance/README.md) | Cleanup of artifacts, logs, and root build files; structural-integrity checks after folder/namespace moves | `Run-Clean.ps1`, `Test-StructuralIntegrity.ps1` | `-Logs`, `-Artifacts`, `-Root`; Build, Test, Paths, Namespaces, Sonar, All |
| [release](release/README.md) | GitHub Release publishing, ruleset toggles, nuget.org unlist | `Run-GithubRelease.ps1` | `-BypassPR`, `-Unlist`, `-Draft`, `-Force`, `-Watch` |
| [sonar-scanner](sonar-scanner/README.md) | SonarQube static analysis scanning | `Run-SonarScanner.ps1` | Project-level scanning |
| [testing](testing/README.md) | `dotnet test` wrapper with async support | `Run-Tests.ps1` | Project or Solution targeting |

### Internal Directories

| Directory | Purpose |
|-----------|---------|
| [.shared](.shared/) | Shared PowerShell helper modules (path, coverage, git, HTML, Docker, etc.) imported by other tools |

## Running Scripts

All scripts should be run from the **repository root**:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/<directory>/<Script>.ps1" [parameters]
```

## Standard Parameters

These parameters are common across multiple tools:

| Parameter | Tools | Description |
|-----------|-------|-------------|
| `-Configuration` | coverage, diagnostics, formatter, testing | `Debug` (default) or `Release` |
| `-Scope` | coverage, diagnostics, formatter, inspection | Named scope resolving to a specific project |
| `-DryRun` | git | Preview what would be committed without making changes |

## Output Conventions

- All runtime output goes to `artifacts/` or `logs/`
- Scripts NEVER create files in the project root

## Specifications

For normative rules governing tool implementation:

- Root tool spec: `docs/ai/specs/tools/spec.md`
- Audit CLI spec: `docs/ai/specs/tools/audit-cli/spec.md`
- Safety tiers: `docs/ai/specs/safety.md`
