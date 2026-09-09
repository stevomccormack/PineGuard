---
spec:
  id: pineguard.ai.scan.sonar
  title: "Scan Specification (SonarQube)"
  version: 2
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "tools/code-scan/sonarqube/**"
---

# Scan Specification (SonarQube)

> [!IMPORTANT]
> Normative specification for SonarQube static analysis tooling in PineGuard.

## 1. Overview

PineGuard uses a local SonarQube Community Edition (Docker) for static analysis.
The scanner (`dotnet-sonarscanner`) runs locally and requires Java (OpenJDK 21).

## 2. SonarQube Severity Model (2025.1)

| PineGuard Alias | SonarQube API Value(s) | Description |
|-----------------|------------------------|-------------|
| Blocker | `BLOCKER` | Must fix — breaks the build or is a security vulnerability |
| High | `CRITICAL` | Likely a bug or severe code smell |
| Medium | `MAJOR` | Code smell or quality issue |
| Low | `MINOR`, `INFO` | Minor code smell or informational |
| All | *(omit parameter)* | All severities |

## 3. API Endpoints

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/system/status` | GET | Health check — verify SonarQube is `UP` |
| `/api/issues/search` | GET | Fetch issues with filtering (severity, component, etc.) |

### Query Parameters (`/api/issues/search`)

| Parameter | Description |
|-----------|-------------|
| `componentKeys` | Project key (default: `PineGuard`) |
| `severities` | Comma-separated severity filter |
| `ps` | Page size (max 500) |
| `p` | Page number (1-based) |
| `resolved` | `false` to exclude resolved issues |

### Authentication

All API calls use Bearer token authentication:
```
Authorization: Bearer <token>
```

## 4. Token Management

Priority order:
1. `-ProjectToken` parameter (explicit)
2. `$env:SONARQUBE_TOKEN` environment variable
3. `.etc/powershell/.env` (loaded by `Import-DotEnv` / `Get-ToolSecret` in `tools/.shared/dotenv.ps1` and `tools/.shared/secret.ps1`)

**Never hard-code tokens in scripts or documentation.**

`tools/code-scan/sonarqube/Initialize-SonarQube.ps1` commissions the server: it generates a
cryptographically random 32-byte, base64-encoded admin password (via
`System.Security.Cryptography.RandomNumberGenerator`), replacing the old hardcoded default. Both
`SONARQUBE_ADMIN_PASSWORD` and `SONARQUBE_TOKEN` are written only to `.etc/powershell/.env` —
neither is ever written to a User/Machine environment variable.

## 5. Output Paths

| Artifact | Path |
|----------|------|
| SonarQube dashboard | `http://localhost:9001/dashboard?id=PineGuard` |
| Issue JSON (stdout) | Piped to consuming agent — not persisted to disk |

## 6. Tool Scripts

| Script | Purpose |
|--------|---------|
| `tools/code-scan/sonarqube/Install-SonarQube.ps1` | Install prerequisites (OpenJDK 21) and start the SonarQube server |
| `tools/code-scan/sonarqube/Run-SonarScanner.ps1` | Full analysis pipeline |
| `tools/code-scan/sonarqube/Get-SonarQubeIssues.ps1` | Fetch issues by severity (JSON output) |
| `tools/docker/docker-compose.sonarqube.yml` | Docker Compose definition |
| `tools/code-scan/sonarqube/Start-SonarQube.ps1` | Start SonarQube container |

## 7. Fix Workflow Rules

1. **Verify UP**: Always check `/api/system/status` before querying issues.
2. **One file at a time**: Fix issues in a single file, then verify build.
3. **Build after each fix**: Run `dotnet build PineGuard.slnx --no-incremental` after each file.
4. **Never suppress warnings**: Do not add `#pragma warning disable` or `[SuppressMessage]` to hide issues.
5. **Idiomatic C#**: Apply fixes using PineGuard coding standards (`docs/ai/specs/coding-standard.md`).
6. **Report**: Summarize fixed vs skipped issues when done.

## 8. References

- Tool README: `tools/code-scan/sonarqube/README.md`
- Coding standards: `docs/ai/specs/coding-standard.md`
- Safety spec: `docs/ai/specs/safety.md`
