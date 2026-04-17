# Code Inspection (Qodana)

Local [JetBrains Qodana](https://www.jetbrains.com/qodana/) static code inspection for PineGuard.

Qodana runs as a Docker container. Results are written to `artifacts/qodana/<scope>/report/index.html`.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running
- [Winget](https://learn.microsoft.com/en-us/windows/package-manager/winget/) (Windows 10/11 built-in)
- `QODANA_TOKEN` environment variable (from [Qodana Cloud](https://qodana.cloud/))

## Workflow

### 1. Start the Docker infrastructure

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-up.ps1
```

### 2. Initialize (first run only)

Installs the Qodana CLI via Winget if not present and starts the Qodana container.

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Initialize-Qodana.ps1
```

### 3. Set your Qodana Cloud token

```powershell
$env:QODANA_TOKEN = '<your-token>'
```

Token is optional for local dev — omit it to keep results local only (no cloud upload).

### 4. Run an inspection

```powershell
# Core scope (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Run-Qodana.ps1 -Scope Core -Clean

# All projects
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Run-Qodana.ps1 -Scope All -Clean

# Open the HTML report after scan
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Run-Qodana.ps1 -Scope Core -Clean -OpenReport
```

### 5. Review findings

Results are written to `artifacts/qodana/<scope>/report/index.html`. Open in a browser or use `-OpenReport`.

### 6. Stop the container

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-down.ps1
```

## Parameters — Run-Qodana.ps1

| Parameter | Default | Description |
|---|---|---|
| `-Scope` | `Core` | `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`, `All` |
| `-Clean` | `$false` | Delete previous results before scanning |
| `-OpenReport` | `$false` | Open HTML report in browser after scan |
| `-ShowReport` | `$false` | Show inline SARIF summary after scan |
| `-NonInteractive` | `$false` | Suppress Qodana's interactive prompts (script-friendly) |
| `-Token` | env `QODANA_TOKEN` | Qodana Cloud token (optional for local dev) |
| `-Endpoint` | env `QODANA_ENDPOINT` | Qodana Cloud endpoint override |
| `-Linter` | `qodana-dotnet` | Qodana linter image (`qodana-dotnet` or `auto`) |
| `-TimeoutMinutes` | `30` | Hard timeout in minutes (1--1440) |
| `-TimeoutExitCode` | `124` | Exit code returned on timeout (1--255) |
| `-RepoRoot` | auto-detected | Path to repository root |
| `-ResultsDir` | auto | Override results output directory |

## Per-Scope Solution Files

Each scope maps to a dedicated `.slnx` file and Qodana config. Per-scope solution files live in `tools/code-inspection/qodana/` and config YAML files live in `tools/code-inspection/qodana/config/`.

| Scope | Config |
|---|---|
| `Core` | `tools/code-inspection/qodana/config/qodana.core.yaml` |
| `MustClauses` | `tools/code-inspection/qodana/config/qodana.must-clauses.yaml` |
| `GuardClauses` | `tools/code-inspection/qodana/config/qodana.guard-clauses.yaml` |
| `FluentValidation` | `tools/code-inspection/qodana/config/qodana.fluent-validation.yaml` |
| `DataAnnotations` | `tools/code-inspection/qodana/config/qodana.data-annotations.yaml` |
| `Testing` | `tools/code-inspection/qodana/config/qodana.testing.yaml` |
| `All` | `tools/code-inspection/qodana/config/qodana.all.yaml` |

## Artifacts

Results go to `artifacts/qodana/<scope>/`.

## Auto Scripts

The `auto/` subdirectory contains agent-generated convenience wrappers whitelisted for automated execution:

| Script | Purpose |
|---|---|
| `auto/Run-Coverage.ps1` | Forwards to `Run-CodeCoverage.ps1` with `-Clean`. Accepts `-Scope` and `-Filter`. |
| `auto/Run-Last.ps1` | Re-runs the last requested test command via `Run-Tests.ps1`. Accepts `-Project` and `-Filter`. |

These scripts are intended to be whitelisted in agent settings via the wildcard `tools/code-inspection/auto/*.ps1`.

## CI/CD

For CI/CD, use the [JetBrains Qodana GitHub Action](https://github.com/JetBrains/scan-qodana-action) with `QODANA_TOKEN` stored as a GitHub repository secret. Do not use these scripts in pipelines.

```bash
gh secret set QODANA_TOKEN --body "<ci-token>"
```
