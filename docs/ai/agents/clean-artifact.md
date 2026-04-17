<!-- metadata_header
type: agent
id: agent-clean-artifact
version: 1.0
-->

# Agent: Clean Artifacts

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. **Clean Artifacts (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Clean-Artifacts.ps1" -Recursive -All`
   - This executes the maintenance scripts which wipe out test coverage results, generated outputs, and analysis data under the `artifacts/` folder.
