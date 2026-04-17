# Skill: Fix SonarQube Issues
**ID**: pineguard.skill.fix-sonar
**Version**: 1.0

## 1. Context & Goal
Query the SonarQube API for issues filtered by severity, then fix them in-place using idiomatic C#.

## 2. Inputs
- **Severity**: (`All`, `Blocker`, `High`, `Medium`, `Low`) — which issues to fetch and fix

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> - Fix one file at a time. Verify `dotnet build PineGuard.slnx --no-incremental` after each file.
> - Never suppress warnings (`#pragma warning disable`, `[SuppressMessage]`). Fix the root cause.
> - Apply idiomatic C# fixes following `docs/ai/specs/coding-standard.md`.
> - If a fix introduces a build error, revert and skip that issue.
> - Never hard-code tokens.

## 4. Execution Steps

1. **Verify SonarQube is UP**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -Command "Invoke-RestMethod -Uri 'http://localhost:9001/api/system/status'"
   ```

   If not UP, instruct user to run `Initialize-SonarQube.ps1`.

2. **Fetch Issues**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Get-SonarIssues.ps1" -Severity [SEVERITY]
   ```

   Parse the JSON output. Each entry: `{ file, line, rule, severity, message, component }`.

3. **Fix Issues (per file)**

   For each unique file in the issue list:
   1. Read the file.
   2. Understand the rule violation from `rule` and `message`.
   3. Apply an idiomatic fix.
   4. Build: `dotnet build PineGuard.slnx --no-incremental`
   5. If build fails, revert changes to that file and log the skip reason.

4. **Report**

   Summarize:
   - Total issues fetched
   - Issues fixed (file, rule, line)
   - Issues skipped (file, rule, reason)

## 5. Definition of Done
- [ ] All fixable issues for the requested severity are resolved
- [ ] Solution builds cleanly after all fixes
- [ ] Summary report provided

## 6. Reference Material (Deep Dive)
- `docs/ai/specs/scan/spec.md` (severity model, API, fix rules)
- `docs/ai/specs/coding-standard.md` (formatting, naming)
- `tools/sonar-scanner/README.md` (tool usage)
