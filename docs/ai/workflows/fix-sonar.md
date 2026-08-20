# Workflow: Fix SonarQube Issues

> [!NOTE]
> Fetches SonarQube issues by severity and fixes them in-place using idiomatic C#.

## Context

- **Role**: [Senior Engineer](../roles/owner.md)
- **Reference**: `tools/sonar-scanner/Get-SonarIssues.ps1`
- **Spec**: `docs/ai/specs/scan/spec.md`

## Parameters

- **Severity**: (`All`, `Blocker`, `High`, `Medium`, `Low`)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Verify SonarQube is UP**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -Command "Invoke-RestMethod -Uri 'http://localhost:9001/api/system/status'"
   ```

   Confirm the response contains `"status": "UP"`. If not, instruct the user to run `Initialize-SonarQube.ps1`.

2. **Fetch issues**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Get-SonarIssues.ps1" -Severity [SEVERITY]
   ```

   Parse the JSON output. Each issue contains: `file`, `line`, `rule`, `severity`, `message`, `component`.

3. **Fix issues (one file at a time)**

   For each unique file in the issue list:
   1. Read the affected file.
   2. Understand the SonarQube rule violation from the `rule` and `message` fields.
   3. Apply an idiomatic C# fix following `docs/ai/specs/coding-standard.md`.
   4. **Never suppress warnings** — fix the root cause.

4. **Verify build after each file**

   ```powershell
   dotnet build PineGuard.slnx --no-incremental
   ```

   If the build fails, revert the last change and investigate.

5. **Report**

   Summarize:
   - Total issues fetched
   - Issues fixed (with file and rule)
   - Issues skipped (with reason)
