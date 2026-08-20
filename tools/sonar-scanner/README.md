# SonarQube

Local [SonarQube](https://www.sonarsource.com/products/sonarqube/) static analysis for PineGuard.

The SonarQube **server** runs in Docker. The **scanner** (`dotnet-sonarscanner`) runs locally and requires Java.
Results are viewed at `http://localhost:9001`.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) running
- [Winget](https://learn.microsoft.com/en-us/windows/package-manager/winget/) (Windows 10/11 built-in)
- .NET SDK

## Workflow

### 1. Start the Docker infrastructure

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-up.ps1
```

See [tools/docker/README.md](../docker/README.md) for the compose stacks, the shared network, and the
per-stack up/down scripts (`sonarqube-up.ps1` starts SonarQube alone).

### 2. Initialize (first run only)

Installs Java (OpenJDK 21) and `dotnet-sonarscanner` if not present, then waits for SonarQube to be healthy.

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Initialize-SonarQube.ps1 -Open
```

Open `http://localhost:9001` — default credentials: `admin` / `admin` (you will be prompted to change them).

### 3. Commission the server (first run only)

Automates password change, project creation, and token generation:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Setup-SonarQube.ps1
```

This will:

1. Change the default `admin:admin` password to `Scanner-1234`
2. Create the `PineGuard` project
3. Generate a `LocalDev` token
4. Persist the token as `SONARQUBE_TOKEN` (User environment variable)

To customise the password:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Setup-SonarQube.ps1 -NewPassword "MyPassword123"
```

> **Re-running is safe** — the script detects existing configuration and skips completed steps.

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

### 4. Run the analysis

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Run-SonarScanner.ps1
```

The token is already persisted as `SONARQUBE_TOKEN` by the setup script. To pass it explicitly:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Run-SonarScanner.ps1 -ProjectToken "sqa_xxx"
```

The pipeline will:
1. Verify SonarQube is UP at `http://localhost:9001`
2. Begin a SonarScanner session
3. Build the solution
4. Collect Cobertura code coverage
5. Submit findings to SonarQube

### 5. Review findings

Refresh `http://localhost:9001` → open the **PineGuard** project dashboard.

### 6. Stop the server

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-down.ps1
```

## Parameters — Run-SonarScanner.ps1

| Parameter | Default | Description |
|---|---|---|
| `-ProjectToken` | env `SONARQUBE_TOKEN` | SonarQube project authentication token |
| `-SonarUrl` | `http://localhost:9001` | URL of the local SonarQube instance |
| `-ProjectKey` | `PineGuard` | SonarQube project key |
| `-RepoRoot` | auto-detected | Path to repository root |

## Get-SonarIssues.ps1

Queries the SonarQube API for issues, filtered by severity. Outputs structured JSON to stdout for consumption by AI agents or scripts.

### Usage

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity Blocker
```

### Parameters

| Parameter | Default | Description |
|---|---|---|
| `-Severity` | `All` | Filter: `All`, `Blocker`, `High`, `Medium`, `Low` |
| `-SonarUrl` | `http://localhost:9001` | URL of the local SonarQube instance |
| `-ProjectKey` | `PineGuard` | SonarQube project key |
| `-ProjectToken` | env `SONARQUBE_TOKEN` | SonarQube project authentication token |
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
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity Blocker

# Get first 100 issues of any severity
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity All -MaxIssues 100

# Get Medium issues with explicit token
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity Medium -ProjectToken "sqa_xxx"
```

## CI/CD

For CI/CD, use the [SonarQube GitHub Action](https://github.com/SonarSource/sonarqube-scan-action) with `SONARQUBE_TOKEN` stored as a GitHub repository secret. Do not use these scripts in pipelines.

```bash
gh secret set SONARQUBE_TOKEN --body "sqa_xxx"
```
