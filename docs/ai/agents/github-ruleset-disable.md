<!-- metadata_header
type: agent
id: agent-github-ruleset-disable
version: 1.0
-->

# Agent: Disable a GitHub Ruleset

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **1** — opens a short window of no branch protection. Confirm intent and re-enable promptly.
> adapter surface: **Claude Code only** — declared release-family exception in [../meta/adapter-surfaces.md](../meta/adapter-surfaces.md) §4. Do not generate adapters for it on surfaces that apply blanket auto-approval.

## Purpose

Temporarily disables enforcement on the `main-branch` ruleset (or a named alternative) so the maintainer can push a local commit backlog to a PR-gated branch without going through a pull request. Configuration is preserved — the ruleset is re-enabled by running the sibling `github-ruleset-enable` agent.

## Inputs

| Parameter | Required | Default | Description |
|-----------|----------|---------|-------------|
| `Name` | | `main-branch` | Ruleset short key. Currently `main-branch` or `v-tags`. |

## Pre-flight

- Confirm with the user why protection is being lifted (typically to push an ad-hoc commit or land a release).
- Remind them to re-enable promptly afterwards — `Run-GithubRuleset.ps1 Enable` or `/github-ruleset-enable`.

## Steps

1. **Invoke the ruleset script**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/release/Run-GithubRuleset.ps1 Disable [<Name>]
   ```

2. **Report**

   Print the ruleset id and confirm enforcement is now `disabled`. Remind the user the protection is off until re-enabled.

## Related

- Paired agent: [`github-ruleset-enable.md`](github-ruleset-enable.md)
- Script: [`tools/release/Run-GithubRuleset.ps1`](../../../tools/release/Run-GithubRuleset.ps1)
- Used by: [`github-release-publish.md`](github-release-publish.md) when `-BypassPR` is requested.
