# Docker

Docker Compose stacks backing the two containerised analysis tools: [Qodana](../code-scan/qodana/README.md)
and [SonarQube](../code-scan/sonarqube/README.md).

These scripts only start and stop containers. The analysis itself is run by
`tools/code-scan/qodana/Run-Qodana.ps1` and `tools/code-scan/sonarqube/Run-SonarScanner.ps1`.

SonarQube's own single-stack lifecycle scripts (formerly `sonarqube-up.ps1` / `sonarqube-down.ps1`
here) now live next to the rest of the SonarQube domain as
`tools/code-scan/sonarqube/Start-SonarQube.ps1` / `Stop-SonarQube.ps1` — this folder keeps only the
compose files, the combined multi-stack scripts, and the shared network helper.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running
- `QODANA_TOKEN` (optional — only for uploading Qodana results to Qodana Cloud)

## Directory Structure

```
tools/docker/
├── docker-compose.qodana.yml    # jetbrains/qodana-dotnet:2025.3
├── docker-compose.sonarqube.yml # sonarqube:2025.1-community, host port 9001 (loopback-only)
├── docker-up.ps1                # Start both stacks
├── docker-down.ps1              # Stop both stacks
├── docker-network.ps1           # Create/remove/inspect the shared pineguard network
├── qodana-up.ps1                # Start Qodana only
└── qodana-down.ps1              # Stop Qodana only
```

SonarQube's single-stack scripts (`Start-SonarQube.ps1` / `Stop-SonarQube.ps1`) live in
[tools/code-scan/sonarqube/](../code-scan/sonarqube/README.md), not here — see that domain's own
README. They reference `docker-compose.sonarqube.yml` in this folder, which stays here (D-3).

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
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Start-SonarQube.ps1 -Open
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Stop-SonarQube.ps1

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
| `docker-down.ps1`, `qodana-down.ps1` | `-RemoveVolumes` | `$false` | Also delete the named volumes |
| `docker-network.ps1` | `-Name` | `pineguard` | Network name |
| `docker-network.ps1` | `-Driver` | `bridge` | Network driver (`bridge`, `host`, `overlay`) |
| `docker-network.ps1` | `-Remove` | `$false` | Remove the network instead of creating it |
| `docker-network.ps1` | `-Info` | `$false` | Inspect and print the network configuration |

`Start-SonarQube.ps1` / `Stop-SonarQube.ps1` (`-NetworkName`, `-Port`, `-HealthTimeoutSeconds`,
`-Open`, `-RemoveVolumes`) are documented in
[tools/code-scan/sonarqube/README.md](../code-scan/sonarqube/README.md), not here.

## Endpoints and volumes

| Stack | Container | Endpoint | Volumes |
|-------|-----------|----------|---------|
| SonarQube | `pineguard-sonarqube` | `http://localhost:9001` (default `admin` / `admin`) | `pineguard-sonarqube-{data,extensions,logs}` |
| Qodana | `pineguard-qodana` | none — writes to `artifacts/qodana/` | `pineguard-qodana-cache` |

Qodana's host paths are overridable through the environment: `QODANA_PROJECT_DIR`, `QODANA_RESULTS_DIR`,
and `QODANA_CONFIG_PATH` (which defaults to `tools/code-scan/qodana/config/qodana.all.yaml`).

## Notes

- Do not invoke the `.yml` files with `docker compose` directly — the scripts set the project name and
  ensure the network exists first.
- `-RemoveVolumes` on SonarQube discards the project, users, and analysis history; the commissioning
  steps in [tools/code-scan/sonarqube/README.md](../code-scan/sonarqube/README.md) then have to be
  repeated.
- `dotnet-sonarscanner` is not installed by these scripts. It comes from the repo-root local tool
  manifest (`.config/dotnet-tools.json`); run `dotnet tool restore` (or just run
  `tools/code-scan/sonarqube/Run-SonarScanner.ps1`, which restores it automatically) to make it
  available.
