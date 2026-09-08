<!-- metadata_header
type: workflow
id: workflow-test
version: 1.0
-->

# Workflow: Test

> [!NOTE]
> Standard workflow for executing unit tests without coverage analysis.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Skill**: [Implement Unit Tests](../skills/scaffold-unit-test/SKILL.md)
- **Spec**: [Unit Tests Spec](../specs/testing/unit-test.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options,
  DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All)

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
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope [SCOPE]
   ```

   `-Scope` resolves the scope's own `*.UnitTests` project from the shared registry
   (`tools/.shared/dotnet-projects.ps1`), so there is no project-path map to keep in step here.
   A new registry scope becomes a valid `-Scope` value with no edit to this workflow.

   > `tests/PineGuard.Testing/` is the shared test-infrastructure library itself — it has no test methods and is never run directly. Its tests live in `tests/PineGuard.Testing.UnitTests/`, which is what the `Testing` scope runs.

   **All**: `-Scope All` resolves to `PineGuard.slnx` and runs every project in one invocation —
   it replaces the old "run each project command in sequence" step.

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope All
   ```

   `-Project <path>` and `-Solution <path>` remain available for a one-off target outside the
   registry. Exactly one of `-Scope`, `-Project` or `-Solution` may be given; supplying none or
   more than one exits `2`.

2. **Check Results**
   Ensure all tests passed (Green).
