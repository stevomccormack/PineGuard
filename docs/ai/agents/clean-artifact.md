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

1. **Clean Artifacts (All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Clear-Artifacts.ps1" -All`
   - This executes the clean scripts which wipe out test coverage results, generated outputs, and analysis data under the `artifacts/` folder. Artifact cleanup is always recursive, unconditionally — `Clear-Artifacts.ps1` has no `-Recursive` switch to pass.

## Related

- [`../specs/safety.md`](../specs/safety.md) — Tier 0/1/2 classification and safe zones
- Paired agents: [`clean-all.md`](clean-all.md), [`clean-log.md`](clean-log.md)
