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
    - Run the script: `tools/code-coverage/Run-CodeCoverage.ps1 -Scope [ProjectName]`
    - _Or_ use dotnet: `dotnet test --collect:"XPlat Code Coverage"` (but use the script if available).

2.  **Analyze Report**
    - Open the generated HTML report (usually in `artifacts/coverage/`).
    - Identify red lines (uncovered) and yellow diamonds (partial branches).

3.  **Fill Gaps**
    - **Null Checks**: Did you test passing `null`?
    - **Edge Cases**: MinValue, MaxValue, Empty strings?
    - **Conditions**: Did you hit both `true` and `false` paths for every `if`?

4.  **Verification**
    - Re-run step 1 and confirm 100%.

## 5. Definition of Done

- [ ] Report shows 100% coverage for the target class/project.

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
