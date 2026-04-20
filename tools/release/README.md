# Release

PowerShell tooling for cutting GitHub Releases and managing the resulting nuget.org artifacts.

## Entry Points

| Script | Purpose |
|--------|---------|
| [`Run-GithubRelease.ps1`](Run-GithubRelease.ps1) | Main orchestrator. Cuts a GitHub Release that triggers `publish.yml` → nuget.org. Switches compose in sub-operations (ruleset toggle, nuget unlist, watch the workflow). |
| [`Run-GithubRuleset.ps1`](Run-GithubRuleset.ps1) | Standalone ruleset toggle. Flips enforcement between `active` and `disabled` on a named ruleset (default `main-branch`) without deleting the configuration. |
| [`Run-NugetUnlist.ps1`](Run-NugetUnlist.ps1) | Standalone nuget.org unlist. Unlists older prereleases across all six PineGuard packages; keeps the latest prerelease listed by default. |

All scripts run from the repository root.

## `Run-GithubRelease.ps1` — the orchestrator

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass `
    -File ./tools/release/Run-GithubRelease.ps1 `
    -Version <semver> [-BypassPR] [-Unlist] [-Draft] [-Force] [-Watch]
```

| Switch | Effect |
|--------|--------|
| `-Version <semver>` | Required. Normalised to a `v`-prefixed tag. Prereleases auto-detected from `-alpha`/`-beta`/`-rc` suffix and flagged with `--prerelease --latest=false`. |
| `-BypassPR` | Before cutting, disables the `main-branch` ruleset, pushes local commits, re-enables the ruleset. No-op if nothing is ahead of upstream. |
| `-Draft` | Creates a draft release. `publish.yml` does not fire until the draft is published manually in the GitHub UI. `-Watch` and `-Unlist` are ignored. |
| `-Force` | Skips the main-branch and clean-tree pre-flight checks. |
| `-Watch` | Tails the triggered `publish.yml` run in the terminal until it completes. Required for `-Unlist`. |
| `-Unlist` | After the workflow succeeds (only works with `-Watch`), unlists older prereleases on nuget.org, keeping the latest listed. |
| `-DryRun` | Prints the plan — pre-flight, BypassPR intent, the exact `gh release create` invocation — without actually cutting the release or running sub-scripts. |

Full-flow example — push pending commits, cut the release, watch the workflow, unlist older alphas:

```powershell
pwsh -File ./tools/release/Run-GithubRelease.ps1 -Version 0.1.0-alpha.6 -BypassPR -Watch -Unlist
```

## `Run-GithubRuleset.ps1` — standalone ruleset toggle

```powershell
pwsh -File ./tools/release/Run-GithubRuleset.ps1 <Enable|Disable> [<Name>]
```

| Parameter | Default | Meaning |
|-----------|---------|---------|
| `Action` | — | `Enable` sets `enforcement=active`; `Disable` sets `enforcement=disabled`. |
| `Name` | `main-branch` | Ruleset short key. Currently `main-branch` or `v-tags`. |

Use when you need to push to a protected branch outside a release flow.

## `Run-NugetUnlist.ps1` — standalone nuget.org cleanup

```powershell
pwsh -File ./tools/release/Run-NugetUnlist.ps1 [-All] [-DryRun] [-Force]
```

| Switch | Effect |
|--------|--------|
| `-All` | Unlist every prerelease, including the latest. Default keeps the latest prerelease listed. |
| `-DryRun` | Print the plan without making API calls. |
| `-Force` | Skip the interactive confirmation prompt. |

The `NUGET_TOKEN` env var is loaded from `.etc/powershell/.env` via `tools/.shared/dotenv.ps1`. The token requires the `Unlist Package` scope on the `PineGuard.*` glob.

## Auth Requirements

| Operation | Authentication |
|-----------|---------------|
| `Run-GithubRelease` | `gh` CLI authenticated. Cutting the release uses the `gh` auth token. The subsequent `publish.yml` workflow authenticates to nuget.org via OIDC Trusted Publishing — no long-lived key needed. |
| `Run-GithubRuleset` | `gh` CLI with Repository Administration: Read and write. |
| `Run-NugetUnlist` | `NUGET_TOKEN` in `.etc/powershell/.env` with the Unlist Package scope on `PineGuard.*`. |

## Safety

Per [`docs/ai/specs/safety.md`](../../docs/ai/specs/safety.md):

- `Run-GithubRelease` — Tier 1 (creates durable artifacts on GitHub and nuget.org).
- `Run-NugetUnlist` — Tier 1 (affects publicly-visible listings; reversible via re-list).
- `Run-GithubRuleset Disable` — Tier 1 (short window of no protection; always re-enable after).
- `Run-GithubRuleset Enable` — Tier 2 (restores protection).
