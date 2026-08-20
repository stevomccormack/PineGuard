<!-- metadata_header
type: agent
id: agent-clean-artifact
version: 1.0
-->

# Agent: Clean Artifacts

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2** — confined to the `artifacts/` safe zone ([../specs/safety.md](../specs/safety.md) §7.3). Preview with `-WhatIf` if unsure of scope.

## Steps

1. **Clean Artifacts (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Clean-Artifacts.ps1" -Recursive -All`
   - This executes the maintenance scripts which wipe out test coverage results, generated outputs, and analysis data under the `artifacts/` folder.

## Related

- [`../specs/safety.md`](../specs/safety.md) — Tier 0/1/2 classification and safe zones
- Paired agents: [`clean-all.md`](clean-all.md), [`clean-log.md`](clean-log.md)
