# Docker

Docker Compose stacks backing the two containerised analysis tools: [Qodana](../code-inspection/README.md)
and [SonarQube](../sonar-scanner/README.md).

These scripts only start and stop containers. The analysis itself is run by
`tools/code-inspection/Run-Qodana.ps1` and `tools/sonar-scanner/Run-SonarScanner.ps1`.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running
- `QODANA_TOKEN` (optional — only for uploading Qodana results to Qodana Cloud)

## Directory Structure

```
tools/docker/
├── docker-compose.qodana.yml    # jetbrains/qodana-dotnet:2025.3
├── docker-compose.sonarqube.yml # sonarqube:community, host port 9001
├── docker-up.ps1                # Start both stacks
├── docker-down.ps1              # Stop both stacks
├── docker-network.ps1           # Create/remove/inspect the shared pineguard network
├── qodana-up.ps1                # Start Qodana only
├── qodana-down.ps1              # Stop Qodana only
├── sonarqube-up.ps1             # Start SonarQube only
└── sonarqube-down.ps1           # Stop SonarQube only
```

All containers share the external Docker network `pineguard` and the Compose project name `pineguard`.
The `*-up.ps1` scripts create the network if it is missing, so `docker-network.ps1` is only needed to
inspect or remove it.

## Usage

Run from the repository root.

### Both stacks

```powershell
# Start Qodana + SonarQube
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-up.ps1

# Stop both (add -RemoveVolumes to discard the SonarQube database and Qodana cache)
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-down.ps1
```

### One stack at a time

```powershell
# SonarQube only — waits for the server to report healthy, then optionally opens it
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-up.ps1 -Open
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-down.ps1

# Qodana only
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/qodana-up.ps1
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/qodana-down.ps1
```

### Network

```powershell
# Create (idempotent)
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1

# Inspect
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1 -Info

# Remove (stop the stacks first)
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1 -Remove
```

## Parameters

| Script | Parameter | Default | Description |
|--------|-----------|---------|-------------|
| `docker-up.ps1`, `qodana-up.ps1` | `-NetworkName` | `pineguard` | Docker network name |
| `docker-down.ps1`, `qodana-down.ps1`, `sonarqube-down.ps1` | `-RemoveVolumes` | `$false` | Also delete the named volumes |
| `sonarqube-up.ps1` | `-NetworkName` | `pineguard` | Docker network name |
| `sonarqube-up.ps1` | `-Port` | `9001` | Host port mapped to the container's `9000` |
| `sonarqube-up.ps1` | `-HealthTimeoutSeconds` | `120` | How long to wait for the server to report UP (10–600) |
| `sonarqube-up.ps1` | `-InstallScanner` | `$false` | Install `dotnet-sonarscanner` if it is missing |
| `sonarqube-up.ps1` | `-Open` | `$false` | Open the SonarQube UI once it is healthy |
| `docker-network.ps1` | `-Name` | `pineguard` | Network name |
| `docker-network.ps1` | `-Driver` | `bridge` | Network driver (`bridge`, `host`, `overlay`) |
| `docker-network.ps1` | `-Remove` | `$false` | Remove the network instead of creating it |
| `docker-network.ps1` | `-Info` | `$false` | Inspect and print the network configuration |

## Endpoints and volumes

| Stack | Container | Endpoint | Volumes |
|-------|-----------|----------|---------|
| SonarQube | `pineguard-sonarqube` | `http://localhost:9001` (default `admin` / `admin`) | `pineguard-sonarqube-{data,extensions,logs}` |
| Qodana | `pineguard-qodana` | none — writes to `artifacts/qodana/` | `pineguard-qodana-cache` |

Qodana's host paths are overridable through the environment: `QODANA_PROJECT_DIR`, `QODANA_RESULTS_DIR`,
and `QODANA_CONFIG_PATH` (which defaults to `tools/code-inspection/qodana/config/qodana.all.yaml`).

## Notes

- Do not invoke the `.yml` files with `docker compose` directly — the scripts set the project name and
  ensure the network exists first.
- `-RemoveVolumes` on SonarQube discards the project, users, and analysis history; the setup steps in
  [tools/sonar-scanner/README.md](../sonar-scanner/README.md) then have to be repeated.
