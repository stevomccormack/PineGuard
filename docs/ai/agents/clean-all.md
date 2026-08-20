<!-- metadata_header
type: agent
id: agent-clean-all
version: 1.0
-->

# Agent: Clean All (Logs + Artifacts)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2** — `-Logs -Artifacts` is confined to the `artifacts/` and `logs/` safe zones ([../specs/safety.md](../specs/safety.md) §7.3). Preview with `-WhatIf` if unsure of scope.

## Steps

1. **Clean Logs & Artifacts (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Logs -Artifacts -Recursive -All`
   - This cleans completely all auto-generated developer outputs across both logging and artifacts.

## Not covered

`-Root` (`tools/maintenance/Clean-Root.ps1`) is deliberately absent from this agent and from the `/clean-*` palette. It deletes by extension from the repository root, outside the safe zones, across the protected directories listed in [`../specs/safety.md`](../specs/safety.md) §7.2 — a Tier 1 operation the maintainer runs by hand.

## Related

- [`../specs/safety.md`](../specs/safety.md) — Tier 0/1/2 classification and safe zones
- Paired agents: [`clean-artifact.md`](clean-artifact.md), [`clean-log.md`](clean-log.md)
