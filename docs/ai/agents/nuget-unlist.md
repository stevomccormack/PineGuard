<!-- metadata_header
type: agent
id: agent-nuget-unlist
version: 1.0
-->

# Agent: Unlist NuGet Prereleases

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **1** — affects publicly-visible listings on nuget.org. Reversible via relist, but confirm scope first.

## Purpose

Unlists older prerelease versions across the six PineGuard packages on nuget.org. By default the latest prerelease of each package stays listed so `install latest alpha` still resolves; everything below it is hidden from search and default resolution. Stable versions (no `-alpha`/`-beta`/`-rc` suffix) are never touched.

## Inputs

| Parameter | Required | Default | Description |
|-----------|----------|---------|-------------|
| `Package` | | all six PineGuard packages | Restrict to specific package IDs. |
| `All` | | | Unlist every prerelease, including the latest. |
| `DryRun` | | | Print the plan without calling the NuGet API. |
| `Force` | | | Skip the interactive confirmation prompt. |

## Auth

Requires `NUGET_TOKEN` in `.etc/powershell/.env` with the `Unlist Package` scope on the `PineGuard.*` glob. The script loads the token via `tools/.shared/dotenv.ps1` — no CI / GitHub secret involved.

## Pre-flight

- Run with `-DryRun` first and share the plan with the user if the unlist is not part of a just-completed release.
- If `-All` is requested, confirm explicitly — it removes the latest prerelease too, which can break consumers using floating-version ranges (`0.1.0-*`).

## Steps

1. **Dry run first (optional)**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/release/Run-NugetUnlist.ps1 -DryRun
   ```

2. **Execute**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/release/Run-NugetUnlist.ps1 [-All] [-Force]
   ```

3. **Report**

   Summarise operation counts per package and flag any failures.

## Related

- Script: [`tools/release/Run-NugetUnlist.ps1`](../../../tools/release/Run-NugetUnlist.ps1)
- Used by: [`github-release-publish.md`](github-release-publish.md) when `-Unlist` + `-Watch` are both requested.
