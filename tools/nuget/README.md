# NuGet

PowerShell tooling for managing PineGuard packages on nuget.org.

This folder was split out during Phase 3 (T3.08, D-1a): nuget.org tooling is never nested
under `github/`, since it wraps a different product with its own auth and API. The GitHub-side
release orchestrator that can trigger an unlist as its last step now lives in
[`tools/github/`](../github/README.md).

## Entry Points

| Script | Purpose |
|--------|---------|
| [`Unpublish-NugetPrerelease.ps1`](Unpublish-NugetPrerelease.ps1) | Standalone nuget.org unlist. Unlists older prereleases across every packable PineGuard package; keeps the latest prerelease listed by default. |

All scripts run from the repository root.

## `Unpublish-NugetPrerelease.ps1` — standalone nuget.org cleanup

```powershell
pwsh -File ./tools/nuget/Unpublish-NugetPrerelease.ps1 [-Package <ids>] [-All] [-WhatIf] [-Force] [-EnvFile <path>]
```

| Switch | Effect |
|--------|--------|
| `-Package` | Restrict to specific package IDs. Defaults to the registry-derived list of every packable PineGuard project (`tools/.shared/dotnet-projects.ps1`'s `Get-PineGuardPackableProjects`, F-21) — every scope's project except one marked `IsPackable=false` (today, `PineGuard.Analyzers.CodeFixes`, which ships bundled inside the `PineGuard.Analyzers` package). |
| `-All` | Unlist every prerelease, including the latest. Default keeps the latest prerelease listed. |
| `-WhatIf` | Print the plan without making API calls. `-DryRun` is a supported alias of the same switch. |
| `-Force` | Skip the interactive confirmation prompt. |
| `-EnvFile` | Path to the `.env` file holding `NUGET_TOKEN`. Defaults to `.etc/powershell/.env` under the repository root. |

The `NUGET_TOKEN` env var is loaded from `-EnvFile` (default `.etc/powershell/.env`) via `tools/.shared/dotenv.ps1`. The token requires the `Unlist Package` scope on the `PineGuard.*` glob.

## Auth Requirements

| Operation | Authentication |
|-----------|---------------|
| `Unpublish-NugetPrerelease` | `NUGET_TOKEN` in `.etc/powershell/.env` with the Unlist Package scope on `PineGuard.*`. |

## Safety

Per [`docs/ai/specs/safety.md`](../../docs/ai/specs/safety.md):

- `Unpublish-NugetPrerelease` — Tier 1 (affects publicly-visible listings; reversible via re-list).
