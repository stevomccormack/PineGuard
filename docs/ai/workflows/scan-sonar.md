<!-- metadata_header
type: workflow
id: workflow-scan-sonar
version: 1.0
-->

# Workflow: Scan Sonar

> [!NOTE]
> Runs SonarQube analysis locally via the repo wrapper under `tools/sonar-scanner/`.

## Context

- **Role**: [Code Reviewer](../roles/reviewer.md)
- **Reference**: `tools/sonar-scanner/Run-SonarScanner.ps1`
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

2. **Initialize SonarQube** (idempotent — safe to re-run):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Initialize-SonarQube.ps1"
   ```

3. **Run the analysis pipeline**:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Run-SonarScanner.ps1"
   ```

   Notes:
   - If the script prompts for a token, the user must generate one at `http://localhost:9001`.
   - See `tools/sonar-scanner/README.md` § "Create a local project and token" for first-run setup.

4. **Review findings**

   Open `http://localhost:9001/dashboard?id=PineGuard` in the browser.
