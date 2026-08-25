<!-- metadata_header
type: workflow
id: workflow-fix-test
version: 1.1
-->

# Workflow: Fix Test

> [!NOTE]
> Workflow for resolving unit test failures.

## Context

- **Roles**: [Senior Engineer / Owner](../roles/owner.md), [Test Engineer / Verifier](../roles/verifier.md)
- **Skill**: [Implement Unit Tests](../skills/scaffold-unit-test/SKILL.md)
- **Spec**: [Unit Tests Spec](../specs/testing/unit-test.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing, All)

## Auto-Approval

Not auto-approved on any surface — this workflow writes code. The scoped test runs it chains are
auto-approved, but the repair loop requires explicit user intent.
See [`../commands/fix.md`](../commands/fix.md).

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
