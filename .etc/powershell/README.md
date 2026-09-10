# Maintainer Shell

PowerShell scripts for **maintainer workstation** tasks: one-time bring-up of a clone, and
administration of the GitHub and nuget.org accounts behind PineGuard.

## How this differs from `tools/`

| | `.etc/powershell/` (here) | [`tools/`](../../tools/README.md) |
|---|---|---|
| Audience | The maintainer, on their own machine | Anyone, including CI |
| Dependencies | The maintainer's personal **`Onboarding`** helper library, in a separate repository | Self-contained; only `tools/.shared/` |
| Identity | Carries the maintainer's git identity and account credentials | No identity of its own |
| Runs in CI | Never | Yes |
| Console style | `Write-MastHead` / `Write-Var` / `Write-OkMessage` (Onboarding chrome) | `Write-Step` / `Write-Success` (`tools/.shared/console.ps1`) |

Anything that needs to run in CI, or on a contributor's machine, belongs in `tools/` — not here.
Three scripts here deliberately overlap a `tools/` counterpart; the table below says which to
prefer.

## Prerequisites

- PowerShell 7+ (`pwsh`)
- The **`Onboarding`** repository, cloned beside this one (or anywhere, via
  `$Env:PINEGUARD_ONBOARDING_ROOT`). Without it, every script here fails immediately with an
  explanation rather than an unhelpful "term 'Write-MastHead' is not recognized".
- `gh`, `dotnet`, `git`, `docker` and `qodana` as each script requires — each checks and reports.

## Entry Points

Run everything from the repository root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./.etc/powershell/<Script>.ps1 [parameters]
```

### Workstation bring-up

| Script | Purpose |
|--------|---------|
| [`Set-GitConfig.ps1`](Set-GitConfig.ps1) | Writes the repository's `--local` git config — identity, pull strategy, HTTPS→SSH rewrite. |
| [`Initialize-GitRepository.ps1`](Initialize-GitRepository.ps1) | One-time `git init` + initial commit + push for a fresh working copy. `-CreateRemote` creates the GitHub repository first. |
| [`Initialize-Solution.ps1`](Initialize-Solution.ps1) | Verifies all 14 registry scopes are present on disk, then restores, builds and tests `PineGuard.slnx`. `-CreateMissing` scaffolds absent projects. |
| [`Install-DotnetTool.ps1`](Install-DotnetTool.ps1) | Restores the pinned local tool manifest (`.config/dotnet-tools.json`). `-List` reports global installs that would shadow the pinned versions. |

### Account administration

| Script | Purpose |
|--------|---------|
| [`Set-GithubSecret.ps1`](Set-GithubSecret.ps1) | Pushes the Actions secrets the workflows consume: `NUGET_USER`, `QODANA_TOKEN`. |
| [`Set-GithubVariable.ps1`](Set-GithubVariable.ps1) | Pushes the Actions variables the workflows read: `MIN_CODE_COVERAGE`, `QODANA_ENABLED`. |
| [`Sync-GithubRuleset.ps1`](Sync-GithubRuleset.ps1) | Reconciles branch and tag rulesets against the catalog declared in the script. `Apply` / `Enable` / `Disable` / `List`. |

### Release

| Script | Purpose | Prefer instead |
|--------|---------|----------------|
| [`New-GithubRelease.ps1`](New-GithubRelease.ps1) | Cuts the GitHub Release that triggers `publish.yml`. | [`tools/github/Run-Release.ps1`](../../tools/github/README.md), which wraps the same call with the bypass-PR and unlist phases. |
| [`Unpublish-NugetPrerelease.ps1`](Unpublish-NugetPrerelease.ps1) | Unlists older prereleases on nuget.org. | [`tools/nuget/Unpublish-NugetPrerelease.ps1`](../../tools/nuget/README.md) — same job, portable. |
| [`Publish-NugetPackage.ps1`](Publish-NugetPackage.ps1) | Packs locally and pushes with the maintainer's own key. **No `tools/` counterpart** — this is the fallback for when OIDC publishing is unavailable. | — |

### Quality

| Script | Purpose | Prefer instead |
|--------|---------|----------------|
| [`Initialize-Qodana.ps1`](Initialize-Qodana.ps1) | Commissions the local Qodana environment (token, endpoint) and runs a repo-wide scan. | [`tools/code-scan/qodana/Run-Qodana.ps1`](../../tools/code-scan/qodana/README.md) for scoped scans; `Install-Qodana.ps1` there installs the CLI. |

### Brand assets

| Script | Purpose |
|--------|---------|
| [`New-BrandIcon.ps1`](New-BrandIcon.ps1) | Generates the transparent package icon from the 512px master. Preview-only unless `-EnableUpdateTarget`. |
| [`New-BrandIconPreview.ps1`](New-BrandIconPreview.ps1) | Composites the icon onto a solid background to check for halos. |
| [`Get-ImagePixelGrid.ps1`](Get-ImagePixelGrid.ps1) | Read-only pixel-grid sampler, for tuning the chroma-key above. |

### Desktop

| Script | Purpose |
|--------|---------|
| [`Stop-ClaudeDesktop.ps1`](Stop-ClaudeDesktop.ps1) | Stops the Microsoft Store Claude Desktop app (never the Claude Code CLI or VS Code extension). `-Restart` relaunches it. Standalone — needs no Onboarding library. |

## Naming

Every script follows the Verb-Noun rule in
[`docs/ai/specs/tools/spec.md`](../../docs/ai/specs/tools/spec.md) §1: `Initialize-` for one-time
commissioning, `Install-` for environment bootstrap, `Set-` for configuration writes, `Sync-` for
desired-state reconciliation, `New-` for generators, `Get-` for read-only queries, `Publish-` /
`Unpublish-` for registry operations, `Stop-` for process control.

[`.shared/`](.shared/) keeps lowercase module names under the §1.3 carve-out, matching
[`tools/.shared/`](../../tools/.shared/): these are dot-sourced libraries, not commands.

## `.shared/` — the loader

`.shared/index.ps1` is the first statement of every script here. It loads, in order:

1. `Get-RepoRoot` from `tools/.shared/path.ps1`.
2. The **Onboarding** library, located via `$Env:PINEGUARD_ONBOARDING_ROOT` or by walking up from
   the repository root looking for a sibling `Onboarding/`. The upward walk matters inside a git
   worktree under `.claude/worktrees/`, where the immediate parent is not the GitHub folder.
3. This repository's `.env`, **process-scoped**.
4. `project.ps1`, `solution.ps1`, `sonarqube.ps1`, `github.ps1`.

`$Solution` is projected from `tools/.shared/dotnet-projects.ps1` — the project registry is the
only place a scope is declared, so this folder cannot drift behind it.

Set `$Env:PINEGUARD_SHELL_VERBOSE` to see the Onboarding library's own banner output, which is
suppressed by default.

## Secrets

`.env` in this folder is **gitignored**; [`.env.example`](.env.example) documents its keys.

Values are loaded into the **current process only**. This matters: `Import-DotEnv` in the
Onboarding library defaults to `-Scope User`, which persists every key into the Windows registry.
The loader passes `-Scope Process` explicitly, per the secrets policy documented in
[`tools/.shared/secret.ps1`](../../tools/.shared/secret.ps1) (D-4 / F-24 / F-25) — a token should
not outlive the session that needed it.

Scripts that need a credential resolve it through `Get-ToolSecret`: the process environment
first, then `.env`. No script echoes a secret value.

| Key | Used by |
|-----|---------|
| `GH_TOKEN` | `gh` CLI, for every GitHub script here |
| `NUGET_TOKEN` | `Publish-NugetPackage.ps1` (push), `Unpublish-NugetPrerelease.ps1` (unlist) |
| `NUGET_USER` | Pushed as an Actions secret; `publish.yml` exchanges it for a short-lived OIDC key |
| `QODANA_TOKEN` | `Initialize-Qodana.ps1`; pushed as an Actions secret |
| `SONARQUBE_TOKEN` | `tools/code-scan/sonarqube/` — written there, read from here |

There is no `NUGET_API_KEY`. That name exists only inside `publish.yml`, as the runtime output of
`NuGet/login@v1`, and is never available on a workstation.

## Safety

Per [`docs/ai/specs/safety.md`](../../docs/ai/specs/safety.md):

| Tier | Scripts |
|------|---------|
| **Tier 1** — durable external effects | `New-GithubRelease`, `Publish-NugetPackage`, `Unpublish-NugetPrerelease`, `Initialize-GitRepository`, `Sync-GithubRuleset Apply`/`Disable`, `Set-GithubSecret`, `Set-GithubVariable` |
| **Tier 2** — local or restorative | `Set-GitConfig`, `Initialize-Solution`, `Install-DotnetTool`, `Initialize-Qodana`, `Sync-GithubRuleset Enable`/`List`, `New-BrandIcon`, `New-BrandIconPreview`, `Get-ImagePixelGrid`, `Stop-ClaudeDesktop` |

Every Tier 1 script supports `-WhatIf` (with `-DryRun` as an alias where the surrounding tooling
uses that spelling) and prints its full plan before acting.
