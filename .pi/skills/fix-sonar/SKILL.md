---
name: scan-fix
description: Fix SonarQube issues by severity. Use when asked to fix scan findings, code smells, or SonarQube violations.
---

# Skill: Fix SonarQube Issues

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/scan/spec.md` (severity model, API, fix rules)
2. `docs/ai/specs/coding-standard.md` (formatting, naming)
3. `docs/ai/rules/scan.md` (scan-specific rules)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/fix-sonar/SKILL.md` exactly as written.
Do NOT suppress warnings. Fix the root cause.

## Step 2: Iterative Loop
1. Fetch issues: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/sonar-scanner/Get-SonarIssues.ps1" -Severity [SEVERITY]`
2. Fix one file at a time
3. Build: `dotnet build PineGuard.slnx --no-incremental`
4. Repeat until all fixable issues resolved

## Step 3: Verify
- Solution builds cleanly
- Summary of fixed vs skipped issues provided
