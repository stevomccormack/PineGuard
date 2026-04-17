<!-- metadata_header
type: agent
id: agent-clean-log
version: 1.0
-->

# Agent: Clean Logs

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. **Clean Logs (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Clean-Logs.ps1" -Recursive -All`
   - This executes the maintenance scripts which wipe out testing logs and run logs under the `logs/` folder.
