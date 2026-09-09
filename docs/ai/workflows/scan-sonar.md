<!-- metadata_header
type: workflow
id: workflow-scan-sonar
version: 1.0
-->

# Workflow: Scan Sonar

> [!NOTE]
> Runs SonarQube analysis locally via the repo wrapper under `tools/code-scan/sonarqube/`.

## Context

- **Role**: [Code Reviewer](../roles/reviewer.md)
- **Reference**: `tools/code-scan/sonarqube/Run-SonarScanner.ps1`
- **Docs**: `docs/ai/specs/scan/spec.md`

## Parameters

None — SonarQube is project-wide (no per-project scope).

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Verify Docker is running** before proceeding.

2. **Install prerequisites and start the server** (idempotent — safe to re-run):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Install-SonarQube.ps1"
   ```

3. **Commission the server** (first run; idempotent):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Initialize-SonarQube.ps1"
   ```

   Generates the admin password and a `SONARQUBE_TOKEN`, writing both to
   `.etc/powershell/.env` — never to a User/Machine environment variable. Safe to re-run; it
   detects existing configuration and skips completed steps.

4. **Run the analysis pipeline**:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-scan/sonarqube/Run-SonarScanner.ps1"
   ```

   Notes:
   - The token is resolved automatically: `-ProjectToken` parameter, then `$env:SONARQUBE_TOKEN`,
     then the `SONARQUBE_TOKEN` key written to `.etc/powershell/.env` by step 3.
   - See `tools/code-scan/sonarqube/README.md` § "3. Run the analysis" for details.

5. **Review findings**

   Open `http://localhost:9001/dashboard?id=PineGuard` in the browser.
