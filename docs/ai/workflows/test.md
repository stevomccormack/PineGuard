# Workflow: Run Unit Tests

> [!NOTE]
> Standard workflow for executing unit tests without coverage analysis.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Skill**: [Implement Unit Tests](../skills/scaffold-unit-test.md)
- **Spec**: [Unit Tests Spec](../specs/testing/unit-test.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, All, Testing*)
  - *Testing = `PineGuard.Testing` is a shared library; build-only via solution run, not a runnable test project.

## Auto-Approval

- **Gemini**: `// turbo-all`
- **Claude**: `Project Rules` allow tests.
- **Cursor**: `cmd: dotnet test` allowed.

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
   - Testing: `tests/PineGuard.Testing/PineGuard.Testing.csproj` _(shared library — build-only; no test runner)_

   > **PineGuard.Testing** is a shared test infrastructure library. It has no test methods and cannot be run directly. It is built automatically as a dependency of all other `*.UnitTests` projects. To verify it builds cleanly, include it via the solution run below.

   **All**: run the five project commands above (sequentially).

   Optional (final verification): run the solution (slower).

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Solution "./PineGuard.slnx"
   ```

2. **Check Results**
   Ensure all tests passed (Green).
