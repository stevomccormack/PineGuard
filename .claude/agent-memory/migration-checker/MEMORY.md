# Migration Checker Memory

> **Role:** `docs/ai/roles/owner.md` (Owner)
> Directives: Implement + Test, Refactor Safely, Diagnose Fast, Keep the Stack Coherent, Bias for Small Diffs.
> Constraints: A check produces a gap list — do not implement the fix during a check.

## Learned Patterns

### Trace Discipline
- Trace every Core rule the whole way: Core → Must → Guard → Fluent → DataAnnotations → Tests
- A rule is only "done" when every layer that should adapt it does, and each has a `XxxTests.cs` / `XxxTestData.cs` pair
- `docs/ai/specs/spec.md` and `docs/ai/specs/dependencies.md` decide which layers a given rule is expected to reach
- Report a gap as the specific file that should exist, never as a vague "missing coverage"

### Common Gap Types
- Core rule with a MustClause but no GuardClause, or a Guard with no Fluent/DataAnnotations adapter
- A layer implementation with no paired test files
- Only the `Not*` half of a Guard family implemented — the affirmative companions are missing
- Core rule with no fixture under `tests/PineGuard.Testing/Fixtures/`
- Fixture partial that does not mirror its source Rules partial (`XxxRules.Yyy.cs` → `XxxRulesFixtures.Yyy.cs`)

### Drift Signals
- Signature mismatch between layers (parameter names, nullability, optional parameters)
- Message strings duplicated outside Must instead of reused from it
- Parameter naming inconsistency that breaks `CallerArgumentExpression` capture
- A new Core rule landing with Must only is the usual shape of an abandoned slice

## Canonical References
- `docs/ai/memory/migration-checker.md` (Brain counterpart)
- `docs/ai/agents/audit-gap.md`
- `docs/ai/agents/scaffold-vertical-slice.md`
- `docs/ai/rules/fixture-conventions.md`

## Topic Files
- (none yet — will grow organically)
