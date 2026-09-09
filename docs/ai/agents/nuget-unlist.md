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
> adapter surface: **Claude Code only** — declared release-family exception in [../meta/adapter-surfaces.md](../meta/adapter-surfaces.md) §4. Do not generate adapters for it on surfaces that apply blanket auto-approval.

## Purpose

Unlists older prerelease versions across every packable PineGuard package on nuget.org. By default the latest prerelease of each package stays listed so `install latest alpha` still resolves; everything below it is hidden from search and default resolution. Stable versions (no `-alpha`/`-beta`/`-rc` suffix) are never touched.

## Inputs

| Parameter | Required | Default | Description |
|-----------|----------|---------|-------------|
| `Package` | | every packable PineGuard package (registry-derived, F-21) | Restrict to specific package IDs. |
| `All` | | | Unlist every prerelease, including the latest. |
| `WhatIf` | | | Print the plan without calling the NuGet API. `-DryRun` is a supported alias. |
| `Force` | | | Skip the interactive confirmation prompt. |
| `EnvFile` | | `.etc/powershell/.env` | Alternate `.env` file supplying `NUGET_TOKEN`. |

## Auth

Requires `NUGET_TOKEN` with the `Unlist Package` scope on the `PineGuard.*` glob. The script loads the token via `tools/.shared/dotenv.ps1` from `.etc/powershell/.env` by default, or from whatever `-EnvFile` points at — no CI / GitHub secret involved.

## Pre-flight

- Run with `-WhatIf` first and share the plan with the user if the unlist is not part of a just-completed release.
- If `-All` is requested, confirm explicitly — it removes the latest prerelease too, which can break consumers using floating-version ranges (`0.1.0-*`).

## Steps

1. **Dry run first (optional)**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/nuget/Unpublish-NugetPrerelease.ps1 -WhatIf
   ```

2. **Execute**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass `
       -File ./tools/nuget/Unpublish-NugetPrerelease.ps1 [-All] [-Force]
   ```

3. **Report**

   Summarise operation counts per package and flag any failures.

## Related

- Script: [`tools/nuget/Unpublish-NugetPrerelease.ps1`](../../../tools/nuget/Unpublish-NugetPrerelease.ps1)
- Used by: [`github-release-publish.md`](github-release-publish.md) when `-Unlist` + `-Watch` are both requested.
