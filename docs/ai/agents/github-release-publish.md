<!-- metadata_header
type: agent
id: agent-github-release-publish
version: 1.0
-->

# Agent: Publish a GitHub Release

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **1** — creates durable artifacts on GitHub and nuget.org. Confirm scope with the user before proceeding.

## Purpose

Cuts a GitHub Release that triggers `.github/workflows/publish.yml`, which builds, packs, and pushes the six PineGuard packages to nuget.org via OIDC (Trusted Publishing). Optionally handles the full release flow end-to-end: push pending commits through the protected `main` branch, cut the release, watch the workflow, unlist older prereleases.

## Inputs

| Parameter | Required | Description |
|-----------|----------|-------------|
| `Version` | ✓ | Semver, with or without leading `v` (e.g. `0.1.0-alpha.6`, `v1.0.0`). Prereleases auto-detected from `-alpha`/`-beta`/`-rc` suffix. |
| `BypassPR` | | Disable the `main-branch` ruleset, push local commits, re-enable. No-op if nothing is ahead of upstream. |
| `Unlist` | | After the publish workflow succeeds (requires `Watch`), unlist older prereleases on nuget.org. Latest prerelease stays listed. |
| `Draft` | | Create as a draft. `publish.yml` does not fire until the draft is published manually. |
| `Force` | | Skip the main-branch and clean-tree pre-flight checks. |
| `Watch` | | Tail the triggered publish run until it completes. Required for `Unlist`. |
| `DryRun` | | Print the plan — including the exact `gh release create` invocation — without cutting the release, running sub-scripts, or contacting nuget.org. |

## Pre-flight

- Confirm with the user: the semver they've supplied, whether this is intentionally a prerelease, and whether `BypassPR`/`Unlist` are desired.
- If `Version` looks like a stable semver (`1.0.0`) after a history of prereleases, pause and explicitly confirm the stable cut is intentional.

## Steps

1. **Invoke the release script**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/release/Run-GithubRelease.ps1 `
       -Version <VERSION> [-BypassPR] [-Unlist] [-Draft] [-Force] [-Watch]
   ```

   Pass only the switches the user requested — do not add `-Force` or `-BypassPR` unless explicitly asked.

2. **Observe output**

   - Pre-flight block (`gh authenticated`, `On main`, `Working tree clean`, `Tag … is available`)
   - Optional `BypassPR` block (ruleset cycle + `git push`)
   - `Creating release <tag>` — the `gh release create` call
   - Optional `Tailing run` block when `-Watch` is passed
   - Optional unlist block when `-Unlist` + `-Watch` are both passed

3. **On success**

   Report the release tag, the workflow run URL, and the six nuget.org package URLs from the script's summary output.

4. **On failure**

   Identify which stage failed (pre-flight, BypassPR, release creation, workflow, unlist). Do not auto-retry — surface the error and let the user decide next steps.

## Related

- Script: [`tools/release/Run-GithubRelease.ps1`](../../../tools/release/Run-GithubRelease.ps1)
- Sub-scripts: [`Run-GithubRuleset.ps1`](../../../tools/release/Run-GithubRuleset.ps1), [`Run-NugetUnlist.ps1`](../../../tools/release/Run-NugetUnlist.ps1)
- Workflow: [`.github/workflows/publish.yml`](../../../.github/workflows/publish.yml)
- Safety classification: [`docs/ai/specs/safety.md`](../specs/safety.md)
