# Workflow: Debug and Fix Unit Tests

> [!NOTE]
> Workflow for resolving unit test failures.

## Context

- **Role**: [Software Engineer](../roles/builder.md)
- **Skill**: [Implement Unit Tests](../skills/scaffold-unit-test.md)
- **Spec**: [Unit Tests Spec](../specs/testing/unit-test.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, All)

## Steps

1. **Analyze Failures**
   Run the tests for the scope.

   Preferred: use the auto-approved command wrapper (`/test-[scope]`) which delegates to `docs/ai/agents/test-[scope].md`.

2. **Fix Loop**
   For each failure:
   - Read the Exception Message and Stack Trace.
   - Determine if it's a Logic Bug or a Test Bug.
   - Fix the code.
   - Re-run the specific test.

3. **Verification**
   Run all tests in the scope to ensure no regressions.
