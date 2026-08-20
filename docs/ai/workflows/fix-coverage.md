# Workflow: Debug and Fix Coverage

> [!NOTE]
> Interactive workflow for closing coverage gaps.

## Context

- **Roles**: [Senior Engineer / Owner](../roles/owner.md), [Test Engineer / Verifier](../roles/verifier.md)
- **Skill**: [Improve Code Coverage](../skills/improve-coverage/SKILL.md)
- **Spec**: [Code Coverage Spec](../specs/testing/coverage.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing, All)

## Steps

1. **Baseline Analysis**
   Run the `/coverage-[Scope]` workflow to generate the initial report.

2. **Gap Identification**
   Check the output for any classes with < 100% Line or Branch coverage.

3. **Remediation Loop**
   For each gap:
   - Analyze the source code logic.
   - Add a targeted test case in the corresponding UnitTests project.
   - Re-run `/coverage-[Scope]` to verify.
4. **Completion**
   Stop when 100% coverage is achieved.
