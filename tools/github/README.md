# GitHub

PowerShell tooling for cutting GitHub Releases and toggling branch-ruleset enforcement.

nuget.org tooling (unlisting prereleases) used to live here too, but moved out to its own
[`tools/nuget/`](../nuget/README.md) folder (T3.08, D-1a) — nuget.org tooling is never nested
under `github/`.

## Entry Points

| Script | Purpose |
|--------|---------|
| [`Run-Release.ps1`](Run-Release.ps1) | Main orchestrator. Cuts a GitHub Release that triggers `publish.yml` → nuget.org. Calls the sub-scripts below for the ruleset toggle and the nuget unlist, and can watch the triggered workflow. |
| [`Set-GithubRuleset.ps1`](Set-GithubRuleset.ps1) | Standalone ruleset toggle. Flips enforcement between `active` and `disabled` on a named ruleset (default `main-branch`) without deleting the configuration. |

All scripts run from the repository root.

## `Run-Release.ps1` — the orchestrator

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass `
    -File ./tools/github/Run-Release.ps1 `
    -Version <semver> [-BypassPR] [-Unlist] [-Draft] [-Force] [-Watch]
```

| Switch | Effect |
|--------|--------|
| `-Version <semver>` | Required. Normalised to a `v`-prefixed tag. Prereleases auto-detected from `-alpha`/`-beta`/`-rc` suffix and flagged with `--prerelease --latest=false`. |
| `-BypassPR` | Before cutting, disables the `main-branch` ruleset, pushes local commits, re-enables the ruleset. No-op if nothing is ahead of upstream. |
| `-Draft` | Creates a draft release. `publish.yml` does not fire until the draft is published manually in the GitHub UI. `-Watch` and `-Unlist` are ignored. |
| `-Force` | Skips the main-branch and clean-tree pre-flight checks. |
| `-Watch` | Tails the triggered `publish.yml` run in the terminal until it completes. Required for `-Unlist`. |
| `-Unlist` | After the workflow succeeds (only works with `-Watch`), unlists older prereleases on nuget.org via `tools/nuget/Unpublish-NugetPrerelease.ps1`, keeping the latest listed. |
| `-WhatIf` | Prints the plan — pre-flight, BypassPR intent, the exact `gh release create` invocation — without actually cutting the release or running sub-scripts. `-DryRun` is a supported alias of the same switch. |

The package URLs printed at the end are derived from the registry (`tools/.shared/dotnet-projects.ps1`'s `Get-PineGuardPackableProjects`), not a hand-maintained list — every packable project gets a line, and a project marked `IsPackable=false` is automatically excluded.

Full-flow example — push pending commits, cut the release, watch the workflow, unlist older alphas:

```powershell
pwsh -File ./tools/github/Run-Release.ps1 -Version 0.1.0-alpha.6 -BypassPR -Watch -Unlist
```

## `Set-GithubRuleset.ps1` — standalone ruleset toggle

```powershell
pwsh -File ./tools/github/Set-GithubRuleset.ps1 <Enable|Disable> [<Name>] [-WhatIf]
```

| Parameter | Default | Meaning |
|-----------|---------|---------|
| `Action` | — | `Enable` sets `enforcement=active`; `Disable` sets `enforcement=disabled`. |
| `Name` | `main-branch` | Ruleset short key. Currently `main-branch` or `v-tags`. |
| `WhatIf` | | Looks up the ruleset and prints what would change, without backing it up, deleting it, or recreating it. `-DryRun` is a supported alias of the same switch. |

Use when you need to push to a protected branch outside a release flow.

## Auth Requirements

| Operation | Authentication |
|-----------|---------------|
| `Run-Release` | `gh` CLI authenticated. Cutting the release uses the `gh` auth token. The subsequent `publish.yml` workflow authenticates to nuget.org via OIDC Trusted Publishing — no long-lived key needed. |
| `Set-GithubRuleset` | `gh` CLI with Repository Administration: Read and write. |

## Safety

Per [`docs/ai/specs/safety.md`](../../docs/ai/specs/safety.md):

- `Run-Release` — Tier 1 (creates durable artifacts on GitHub and nuget.org).
- `Set-GithubRuleset Disable` — Tier 1 (short window of no protection; always re-enable after).
- `Set-GithubRuleset Enable` — Tier 2 (restores protection).
