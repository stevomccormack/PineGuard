<!-- metadata_header
type: agent
id: agent-github-ruleset-enable
version: 1.0
-->

# Agent: Enable a GitHub Ruleset

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2** — restores protection; safe to run without confirmation.
> adapter surface: **Claude Code only** — declared release-family exception in [../meta/adapter-surfaces.md](../meta/adapter-surfaces.md) §4. It ships with its Tier 1 counterpart, [`github-ruleset-disable.md`](github-ruleset-disable.md), and stays on the same surface.

## Purpose

Sets enforcement back to `active` on the `main-branch` ruleset (or a named alternative). Run immediately after completing any work that required the protection to be lifted.

## Inputs

| Parameter | Required | Default | Description |
|-----------|----------|---------|-------------|
| `Name` | | `main-branch` | Ruleset short key. Currently `main-branch` or `v-tags`. |

## Steps

1. **Invoke the ruleset script**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/release/Run-GithubRuleset.ps1 Enable [<Name>]
   ```

2. **Report**

   Confirm enforcement is now `active`.

## Related

- Paired agent: [`github-ruleset-disable.md`](github-ruleset-disable.md)
- Script: [`tools/release/Run-GithubRuleset.ps1`](../../../tools/release/Run-GithubRuleset.ps1)
- Used by: [`github-release-publish.md`](github-release-publish.md) when `-BypassPR` is requested.
