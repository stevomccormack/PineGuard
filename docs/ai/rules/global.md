# Global Rules

Before implementing any feature, read these specifications:
- `docs/ai/specs/spec.md` (root invariants, layer ordering, feature checklist)
- `docs/ai/specs/coding-standard.md` (formatting, Resharper, SonarQube rules)
- `docs/ai/specs/orchestration.md` (process, orchestration, verification, safety)
- `docs/ai/specs/dependencies.md` (layer dependency map, cascading rules)

## Key Invariants
- Layer order: Core Utils → Core Rules → MustClauses → GuardClauses → Integrations
- Must owns canonical messages. Guard/Fluent/Data REUSE them. Never duplicate.
- Guard calls Must. Never duplicate logic.
- Deterministic: No IO in Core Rules/Utils.
- File-scoped namespaces, sorted usings, arrow functions for single-line expressions.

## Workflow Orchestration (see `orchestration.md`)
- Plan before acting (3+ steps or architectural decisions → plan mode).
- Verify before claiming success (tests, logs, reviewable proof).
- Track progress visibly (task lists, mark done immediately).
- Use subagents for parallel work; one task per subagent.
- Learn from corrections — write rules that prevent repeating mistakes.

## Engineering Discipline (see `spec.md` §6.3–6.6)
- Simplicity first: minimal code, minimal files, minimal scope.
- Root-cause discipline: find the real problem, no temporary workarounds.
- Minimal impact: no side effects, no defensive restructuring of nearby code.
- Demand elegance for non-trivial work; skip it for simple fixes.

## Precedent and Consistency (owner instruction, 2026-11-16)
- Consistency is the standard. Before writing code, tests, fixtures or docs, find the nearest existing
  precedent in the repository (the sibling member, file or fixture) and the spec that governs it, and
  match both exactly: structure, naming, dataset shape, helper usage.
- Never deviate on your own judgment. "The existing type does not quite fit" is not a reason; adapt the
  work to the precedent.
- A deviation is VERY exceptional. If you believe one is unavoidable, stop before writing it and ask the
  owner: state the precedent, why it cannot be followed, and the smallest alternative. Proceed only on
  the owner's answer. Under an unattended run, record it under "Awaiting owner" and do only work that
  does not depend on it.
- Do not add a label, dataset or pattern the precedent does not use (for example `EdgeCases` in Core
  test data, or edge scenario arrays where `fixture.md` §2 prescribes two arrays). Changing a standard
  is the owner's decision and is applied to every affected member at once, never to one file.
- Reviewers treat an unrecorded deviation from precedent or spec as a blocker.

## File Hygiene
- All output files (logs, reports, temp) MUST go to `artifacts/` or `logs/`.
- NEVER create files in the project root.

## Multi-Session Coordination
- Read and follow `docs/ai/rules/coordination.md` before running builds, tests, or coverage.
- Announce the scope you are working on before starting; never run a build, test, or coverage pass
  concurrently with another session; never clear another session's status or lock.
- Claude Code sessions get this enforced automatically via `.claude/hooks/`. Other surfaces satisfy the
  same contract by hand — see coordination.md §Universal Contract.
