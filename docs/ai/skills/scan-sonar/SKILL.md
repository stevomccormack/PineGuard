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
> - Never hard-code tokens. Use `$env:SONARQUBE_TOKEN` or `-ProjectToken`.
> - If the script prompts for a token, notify the user to generate one at `http://localhost:9001`.
> - Do not attempt to run MSBuild or coverage separately — the wrapper script handles everything.

## 4. Execution Steps

1. **Initialize the Container**

   Always verify the container is running first (idempotent):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Initialize-SonarQube.ps1"
   ```

2. **Run the Analysis**

   Execute the main build wrapper:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Run-SonarScanner.ps1"
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
| Token prompt appears | No `$env:SONARQUBE_TOKEN` set | Generate token at `http://localhost:9001` and set env var |
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
- `tools/sonar-scanner/README.md` (usage, parameters, first-run setup)
