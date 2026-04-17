<!-- metadata_header
type: agent
id: agent-clean-all
version: 1.0
-->

# Agent: Clean All (Logs + Artifacts)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. **Clean Logs & Artifacts (Recursive + All)**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Logs -Artifacts -Recursive -All`
   - This cleans completely all auto-generated developer outputs across both logging and artifacts.
