# Workflow: Run Unit Tests

> [!NOTE]
> Standard workflow for executing unit tests without coverage analysis.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Skill**: [Implement Unit Tests](../skills/scaffold-unit-test/SKILL.md)
- **Spec**: [Unit Tests Spec](../specs/testing/unit-test.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing, All)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow tests.
- **Cursor**: `cmd: dotnet test` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Execute Tests**
   Run the unit test project(s) for the specified scope.

   **Command Template**:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "[TEST_PROJECT]"
   ```

   **Project map**:
   - Core: `tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj`
   - MustClauses: `tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj`
   - GuardClauses: `tests/PineGuard.GuardClauses.UnitTests/PineGuard.GuardClauses.UnitTests.csproj`
   - FluentValidation: `tests/PineGuard.FluentValidation.UnitTests/PineGuard.FluentValidation.UnitTests.csproj`
   - DataAnnotations: `tests/PineGuard.DataAnnotations.UnitTests/PineGuard.DataAnnotations.UnitTests.csproj`
   - Testing: `tests/PineGuard.Testing.UnitTests/PineGuard.Testing.UnitTests.csproj`

   > `tests/PineGuard.Testing/` is the shared test-infrastructure library itself — it has no test methods and is never run directly. Its tests live in `tests/PineGuard.Testing.UnitTests/`, which is what the `Testing` scope runs.

   **All**: run the six project commands above (sequentially).

   Optional (final verification): run the solution (slower).

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Solution "./PineGuard.slnx"
   ```

2. **Check Results**
   Ensure all tests passed (Green).
