# Skill: New Validation (Vertical Slice)
**ID**: pineguard.skill.new-validation
**Version**: 1.0

## 1. Context & Goal
Drive a single predicate-based validation (string format, numeric range, enum check, …) through
every layer of the stack: Core → MustClauses → GuardClauses → FluentValidation → DataAnnotations → Tests.
This skill is **orchestration only** — each layer's procedure is owned by the per-layer scaffold skill.

The agent entrypoint for this work is `docs/ai/agents/scaffold-vertical-slice.md`; this skill is the
procedure that agent follows.

## 2. Inputs
- **Domain**: The domain the validation belongs to (e.g. `Json`, `Network`, `GeoLocation`).
- **Condition**: The good state being asserted (e.g. `Json`, `ZeroOrPositive`).

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Read the root specs first** — before writing any code:
>     *   `docs/ai/specs/spec.md` (root invariants, §3 "Feature Implementation Checklist")
>     *   `docs/ai/specs/dependencies.md` (layer dependency map)
>     *   `docs/ai/specs/coding-standard.md` (formatting rules)
>     *   `docs/ai/specs/orchestration.md` (process/logging)
> 2.  **Layer order is fixed**: never implement an upper layer before the layer it delegates to exists.
> 3.  **No logic duplication**: each layer adapts the one beneath it. Only Core carries the predicate.
> 4.  **No inline procedure**: follow the per-layer skill in the table below; do not restate its rules here.

## 4. Execution Steps

1.  **Implement each layer in order.** For each row, read the project spec first, then follow the skill.

    | Order | Layer | Spec | Skill |
    |---|---|---|---|
    | 1 | Core Utils | `docs/ai/specs/core/project.md` | `docs/ai/skills/scaffold-rule/SKILL.md` |
    | 2 | Core Rules | `docs/ai/specs/core/project.md` | `docs/ai/skills/scaffold-rule/SKILL.md` |
    | 3 | MustClauses | `docs/ai/specs/must-clauses/project.md` | `docs/ai/skills/scaffold-must/SKILL.md` |
    | 4 | GuardClauses | `docs/ai/specs/guard-clauses/project.md` | `docs/ai/skills/scaffold-guard/SKILL.md` |
    | 5 | FluentValidation | `docs/ai/specs/fluent-validation/project.md` | `docs/ai/skills/scaffold-fluent/SKILL.md` |
    | 6 | DataAnnotations | `docs/ai/specs/data-annotations/project.md` | `docs/ai/skills/scaffold-annotation/SKILL.md` |
    | 7 | Unit Tests (all layers) | `docs/ai/specs/testing/unit-test.md` | `docs/ai/skills/scaffold-unit-test/SKILL.md` |

2.  **Build & Test**
    *   `dotnet build PineGuard.slnx`
    *   `dotnet test`
    *   Verify 100% line/branch coverage for the new code (`docs/ai/skills/improve-coverage/SKILL.md`).

3.  **Summarize**
    *   Report every file created or modified, grouped by layer.

## 5. Definition of Done
- [ ] All seven layers implemented in order.
- [ ] Solution builds clean; all tests pass.
- [ ] 100% line and branch coverage for the new code.
- [ ] No validation logic exists outside `PineGuard.Core`.

## 6. Reference Material
- `docs/ai/agents/scaffold-vertical-slice.md` — the agent entrypoint
- `docs/ai/specs/spec.md` §3 — Feature Implementation Checklist (Master)
- `docs/ai/specs/dependencies.md` — layer dependency map
