# Skill: Run SonarQube Analysis
**ID**: pineguard.skill.scan-sonar
**Version**: 1.0

## 1. Context & Goal
Run a full SonarQube static analysis against the PineGuard codebase and direct the user to the results dashboard.

## 2. Inputs
- None (SonarQube is project-wide)

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> - Docker Desktop must be running before initialization.
> - Never hard-code tokens. Resolution order: `-ProjectToken` parameter -> `$env:SONARQUBE_TOKEN` ->
>   the `SONARQUBE_TOKEN` key in `.etc/powershell/.env`.
> - Commissioning (admin password, project, token) is automated by `Initialize-SonarQube.ps1`,
>   which writes `SONARQUBE_TOKEN` to `.etc/powershell/.env` — never persist it to a
>   User/Machine environment variable.
> - Do not attempt to run MSBuild or coverage separately — the wrapper script handles everything.

## 4. Execution Steps

1. **Install & start**

   Always verify the container is running first (idempotent):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Install-SonarQube.ps1"
   ```

1b. **Commission (first run only)**

   Idempotent — safe to re-run. Generates the admin password and `SONARQUBE_TOKEN`, writing both
   to `.etc/powershell/.env` (never to a User/Machine environment variable):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Initialize-SonarQube.ps1"
   ```

2. **Run the Analysis**

   Execute the main build wrapper:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Run-SonarScanner.ps1"
   ```

3. **Interpret Results**

   The analysis runs asynchronously inside the Docker container. Once the script succeeds:
   1. Notify the user the pipeline succeeded.
   2. Instruct them to open `http://localhost:9001/dashboard?id=PineGuard` to view Code Smells, Bugs, and Coverage.

## 5. Definition of Done
- [ ] SonarQube container is healthy
- [ ] Analysis pipeline completed without errors
- [ ] User directed to dashboard URL

## 6. Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| Container not starting | Docker Desktop not running | Start Docker Desktop and wait for it to be ready |
| Token prompt appears | Server not yet commissioned | Run `Initialize-SonarQube.ps1` to generate `SONARQUBE_TOKEN` and persist it to `.etc/powershell/.env` |
| Analysis hangs | Container resource limits | Increase Docker memory allocation (4 GB+ recommended) |
| Dashboard shows stale results | Previous run cached | Wait for analysis task to complete; refresh dashboard |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Run a SonarQube scan" | Initialize container, run scanner, report dashboard URL | Analysis complete, user directed to dashboard |
| "Check code quality" | Same as above — trigger full analysis pipeline | Dashboard with code smells, bugs, coverage metrics |
| "Scan for code smells" | Run SonarQube analysis, summarize findings from dashboard | Categorized findings by severity |

## 8. Reference Material (Deep Dive)
- `docs/ai/specs/scan/spec.md` (severity model, API, fix rules)
- `tools/code-scan/sonarqube/README.md` (usage, parameters, first-run setup)
