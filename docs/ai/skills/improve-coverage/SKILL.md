# Skill: Improve Code Coverage

**ID**: pineguard.skill.improve-coverage
**Version**: 1.0

## 1. Context & Goal

Run code coverage analysis and improve unit tests to reach 100% line and branch coverage.

## 2. Inputs

- **Target Project**: (e.g., `PineGuard.Core`)

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
>
> 1.  **Goal**: 100% Line and Branch coverage is the target.
> 2.  **No Cheating**: Do not use `[ExcludeFromCodeCoverage]` unless code is truly unreachable/platform-specific.
> 3.  **Iterative**: Run coverage -> Find gap -> Add test case -> Repeat.

## 4. Execution Steps

1.  **Run Coverage Command**
    - One-shot: `tools/code-coverage/Run-CodeCoverage.ps1 -Scope [Scope]`
      (`-Mode Generate|Analyze|GenerateAndAnalyze`; scopes: `Core`, `MustClauses`, `GuardClauses`,
      `DataAnnotations`, `FluentValidation`, `Testing`, `All`).
    - Or drive the two stages directly, which is the loop the coverage spec defines:
      - `tools/code-coverage/xplat/Gen-CoverageReport.ps1 -Scope [Scope]`
      - `tools/code-coverage/xplat/Test-CoverageAnalysis.ps1 -Scope [Scope] -Top 30`

2.  **Analyze Report**
    - HTML: `artifacts/code-coverage/xplat/html/index.html`
      (stable redirect: `artifacts/code-coverage/xplat-report.html`).
    - Identify red lines (uncovered) and yellow diamonds (partial branches).
    - For a fast console-only pass, add `-SkipHtml` and read the ranked gap table from
      `Test-CoverageAnalysis.ps1` instead of opening the report.

3.  **Fill Gaps**
    - **Null Checks**: Did you test passing `null`?
    - **Edge Cases**: MinValue, MaxValue, Empty strings?
    - **Conditions**: Did you hit both `true` and `false` paths for every `if`?

4.  **Verification**
    - Re-run step 1 with `-Enforce100` and confirm it exits 0.

## 5. Definition of Done

- [ ] Report shows 100% coverage for the target class/project.
- [ ] `Run-CodeCoverage.ps1 -Scope [Scope] -Enforce100` (or `Test-CoverageAnalysis.ps1 -Enforce100`) exits 0.

## 6. Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| Coverage report missing | Build failed silently | Run `dotnet build` first to confirm clean build |
| Yellow diamonds remain | Partial branch not covered | Add test cases for both `true` and `false` paths |
| `[ExcludeFromCodeCoverage]` temptation | Uncovered code seems unreachable | Only exclude truly unreachable platform-specific code |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Get Core to 100% coverage" | Run coverage script, analyze gaps, add missing test cases | 100% line + branch coverage for PineGuard.Core |
| "Coverage for MustClauses is at 94%" | Identify uncovered branches (null paths, edge cases), add targeted tests | Coverage raised to 100% |
| "Why is this line not covered?" | Trace execution paths, identify missing test input combination | New test case covering the specific branch |

## 8. Reference Material

- `docs/ai/specs/testing/coverage.md`
