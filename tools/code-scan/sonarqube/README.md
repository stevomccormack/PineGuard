# SonarQube

Local [SonarQube](https://www.sonarsource.com/products/sonarqube/) static analysis for PineGuard.

The SonarQube **server** runs in Docker. The **scanner** (`dotnet-sonarscanner`) runs locally and requires Java.
Results are viewed at `http://localhost:9001`.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running
- [Winget](https://learn.microsoft.com/en-us/windows/package-manager/winget/) (Windows 10/11 built-in)
- .NET SDK

## Workflow

### 1. Install prerequisites and start the server

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Install-SonarQube.ps1 -Open
```

Installs Java (OpenJDK 21) if not present, then starts the SonarQube container and waits for it to be
healthy. `dotnet-sonarscanner` comes from the repo-root local tool manifest
(`.config/dotnet-tools.json`) and is restored automatically by `Run-SonarScanner.ps1` (or run
`dotnet tool restore` yourself).

If the container is already running (or Java is already installed) and only the container needs to be
started or stopped, use `Start-SonarQube.ps1` / `Stop-SonarQube.ps1` directly — see
[tools/docker/README.md](../../docker/README.md) for the shared network and the combined
`docker-up.ps1`/`docker-down.ps1` entry points that also bring up Qodana.

Open `http://localhost:9001` — default credentials: `admin` / `admin` (you will be prompted to change them).

### 2. Commission the server (first run only)

Automates password change, project creation, and token generation:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Initialize-SonarQube.ps1
```

This will:

1. Change the default `admin:admin` password to a fresh, cryptographically random value
   (or reuse the one persisted by a previous run — see below)
2. Create the `PineGuard` project
3. Generate a `LocalDev` token
4. Write both secrets to `.etc/powershell/.env` (gitignored) as `SONARQUBE_ADMIN_PASSWORD` and
   `SONARQUBE_TOKEN` — **never** to a User/Machine environment variable

To customise the admin password instead of letting the script generate one:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Initialize-SonarQube.ps1 -NewPassword "MyPassword123"
```

> **Re-running is safe** — the script detects existing configuration and skips completed steps.
> A re-run with no `-NewPassword` reuses whatever password `.etc/powershell/.env` already has,
> rather than generating a new one and locking you out of the account it already changed.

<details>
<summary>Manual alternative (if you prefer)</summary>

1. Open `http://localhost:9001` and log in with `admin` / `admin`.
2. Change the password when prompted.
3. Click **Create a local project** → name it `PineGuard`, key `PineGuard` → click **Next**.
4. Leave the default setting → click **Create project**.
5. Under "How do you want to analyze your repository?" select **Locally**.
6. Click **Generate** → name the token (e.g. `LocalDev`) → copy the token.
7. Set the token: `$env:SONARQUBE_TOKEN = 'sqa_xxx'`

</details>

### 3. Run the analysis

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Run-SonarScanner.ps1
```

The token is resolved automatically — explicit `-ProjectToken`, then `$env:SONARQUBE_TOKEN`, then the
`SONARQUBE_TOKEN` key `Initialize-SonarQube.ps1` wrote to `.etc/powershell/.env`. To pass it explicitly
instead:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Run-SonarScanner.ps1 -ProjectToken "sqa_xxx"
```

The pipeline will:
1. Verify SonarQube is UP at `http://localhost:9001`
2. Begin a SonarScanner session
3. Build the solution
4. Collect Cobertura code coverage
5. Submit findings to SonarQube

### 4. Review findings

Refresh `http://localhost:9001` → open the **PineGuard** project dashboard.

### 5. Stop the server

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Stop-SonarQube.ps1
```

Or `tools/docker/docker-down.ps1` to stop both SonarQube and Qodana together.

## Parameters — Run-SonarScanner.ps1

| Parameter | Default | Description |
|---|---|---|
| `-ProjectToken` | resolved via `Get-ToolSecret` (`$env:SONARQUBE_TOKEN`, then `.etc/powershell/.env`) | SonarQube project authentication token |
| `-SonarUrl` | `http://localhost:9001` | URL of the local SonarQube instance |
| `-ProjectKey` | `PineGuard` | SonarQube project key |
| `-RepoRoot` | auto-detected | Path to repository root |

`-Framework net8.0` is pinned internally when collecting coverage for this pipeline (not a script
parameter): running every target framework would double/triple-count lines in the single opencover
XML SonarQube imports, so one TFM — the actively-supported LTS baseline — is used deliberately.

## Install-SonarQube.ps1 / Start-SonarQube.ps1 / Stop-SonarQube.ps1

| Script | Does |
|---|---|
| `Install-SonarQube.ps1` | Installs Java (OpenJDK 21) if missing, then calls `Start-SonarQube.ps1` |
| `Start-SonarQube.ps1` | Ensures the Docker network exists, starts the container via `tools/docker/docker-compose.sonarqube.yml`, waits for it to report healthy |
| `Stop-SonarQube.ps1` | Runs `docker compose down` against the same compose file (`-RemoveVolumes` to also discard persisted data) |

The compose file itself stays in [tools/docker/](../../docker/README.md) alongside Qodana's — only the
container lifecycle scripts live here, next to the rest of the SonarQube domain.

## Initialize-SonarQube.ps1

Commissions a **running** SonarQube server: admin password, project, token. See step 2 above for the
full behaviour and the D-4 secrets policy (generated password, `.etc/powershell/.env`, never a User
environment variable).

| Parameter | Default | Description |
|---|---|---|
| `-SonarUrl` | `http://localhost:9001` | URL of the local SonarQube instance |
| `-NewPassword` | previously-persisted value, else a fresh random 32-byte base64 string | Admin password to set |
| `-ProjectKey` | `PineGuard` | SonarQube project key |
| `-ProjectName` | `PineGuard` | SonarQube project display name |
| `-TokenName` | `LocalDev` | Name of the generated user token |

## Get-SonarQubeIssues.ps1

Queries the SonarQube API for issues, filtered by severity. Outputs structured JSON to stdout for consumption by AI agents or scripts.

### Usage

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Get-SonarQubeIssues.ps1 -Severity Blocker
```

### Parameters

| Parameter | Default | Description |
|---|---|---|
| `-Severity` | `All` | Filter: `All`, `Blocker`, `High`, `Medium`, `Low` |
| `-SonarUrl` | `http://localhost:9001` | URL of the local SonarQube instance |
| `-ProjectKey` | `PineGuard` | SonarQube project key |
| `-ProjectToken` | resolved via `Get-ToolSecret` (`$env:SONARQUBE_TOKEN`, then `.etc/powershell/.env`) | SonarQube project authentication token |
| `-MaxIssues` | `500` | Maximum number of issues to retrieve (1–10000) |

### Severity Mapping

| PineGuard Alias | SonarQube API Value(s) |
|-----------------|------------------------|
| Blocker | `BLOCKER` |
| High | `CRITICAL` |
| Medium | `MAJOR` |
| Low | `MINOR,INFO` |
| All | *(omit parameter — all severities)* |

### Output Format

JSON array to stdout. Each entry:

```json
{
    "file": "src/PineGuard.Core/Rules/StringRules.cs",
    "line": 42,
    "rule": "csharpsquid:S1135",
    "severity": "MINOR",
    "message": "Complete the task associated to this 'TODO' comment.",
    "component": "PineGuard:src/PineGuard.Core/Rules/StringRules.cs"
}
```

### Examples

```powershell
# Get all Blocker issues
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Get-SonarQubeIssues.ps1 -Severity Blocker

# Get first 100 issues of any severity
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Get-SonarQubeIssues.ps1 -Severity All -MaxIssues 100

# Get Medium issues with explicit token
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Get-SonarQubeIssues.ps1 -Severity Medium -ProjectToken "sqa_xxx"
```

## CI/CD

For CI/CD, use the [SonarQube GitHub Action](https://github.com/SonarSource/sonarqube-scan-action) with `SONARQUBE_TOKEN` stored as a GitHub repository secret. Do not use these scripts in pipelines.

```bash
gh secret set SONARQUBE_TOKEN --body "sqa_xxx"
```
