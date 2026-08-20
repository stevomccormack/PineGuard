<!-- metadata_header
type: agent
id: agent-clean-log
version: 1.0
-->

# Agent: Clean Logs

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2** — confined to the `logs/` safe zone ([../specs/safety.md](../specs/safety.md) §7.3). Preview with `-WhatIf` if unsure of scope.

## Steps

1. **Clean Logs (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Clean-Logs.ps1" -Recursive -All`
   - This executes the maintenance scripts which wipe out testing logs and run logs under the `logs/` folder.

## Related

- [`../specs/safety.md`](../specs/safety.md) — Tier 0/1/2 classification and safe zones
- Paired agents: [`clean-all.md`](clean-all.md), [`clean-artifact.md`](clean-artifact.md)
