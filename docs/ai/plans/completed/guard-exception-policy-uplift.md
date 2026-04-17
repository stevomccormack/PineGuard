# Guard Exception Policy Uplift

## Context

PineGuard already has a strong architectural split:

- `Must.Be.*` returns `MustResult<T>` and stays result-first.
- `Guard.Against.*` throws on failure and reuses the canonical `MustResult.Message`.

That split is a differentiator and should stay intact.

The current `GuardExceptionPolicy` proves the concept, but the public naming is harder to read than it needs to be:

- `ExceptionReplacer`
- `ReplaceArgumentExceptions`

The uplift goal is to make Guard exception substitution feel deliberate, ergonomic, and marketable without destabilizing the current Guard/Must boundary.

### Blast-radius assessment

A direct rename is low impact in the current repo.

`ReplaceArgumentExceptions` is currently referenced in:

- one production file: `src/PineGuard.Core/GuardClauses/GuardExceptionPolicy.cs`
- a small Guard-focused unit-test cluster
- one Guard project spec
- this plan

It is not broadly distributed across the solution, and there is no external compatibility constraint for this project.

---

## Goals

1. Replace `ReplaceArgumentExceptions` with the clearer name `ReplaceDefaultExceptions`.
2. Keep the Guard/Must boundary intact while improving the Guard policy surface.
3. Add scoped policy support with explicit precedence.
4. Update internal tests to the new property name and add coverage for scoped policy behavior.
5. Add concise README examples for:
   - `MustResult.ThrowIfFailed(...)`
   - `GuardExceptionPolicy` global substitution
   - `GuardExceptionPolicy` scoped substitution
6. Refresh spec/docs examples so they align with the new API.
7. Give PineGuard a short, professional differentiator line for exception substitution.

---

## Non-Goals

- Do not move global exception policy into `Must`.
- Do not remove `exceptionCreator` from Guard APIs.
- Do not preserve `ReplaceArgumentExceptions` as a compatibility alias.
- Do not redesign `MustResult.ThrowIfFailed`, `ThrowNullIfFailed`, or `OrThrow` beyond documentation positioning.
- Do not introduce a large DI-heavy exception strategy system if a small focused uplift solves the problem.

---

## Constraints

- The project is new, so **backward compatibility is not required**.
- The code impact should remain tightly scoped and easy to validate.
- The quickest path is preferred, as long as the resulting API is still clean and supportable.
- `Must` remains the explicit, local throwing escape hatch.
- `Guard` remains the home of app-wide exception substitution.

---

## Proposed API Direction (Direct Rename + Scoped Policy)

### 1. Rename the property outright

Replace:

```csharp
GuardExceptionPolicy.ReplaceArgumentExceptions = true;
```

with:

```csharp
GuardExceptionPolicy.ReplaceDefaultExceptions = true;
```

Implementation guidance:

- `ReplaceDefaultExceptions` becomes the only property name used in production code, tests, specs, and examples.
- Remove `ReplaceArgumentExceptions` instead of carrying it forward.
- Rename related local variables, test data fields, and human-readable test case names in the same slice.

Why this is the right move now:

- the blast radius is small and internal
- the new name is clearer immediately
- it avoids dragging legacy naming into future APIs like scoped policy

### 2. Keep the current global replacement seam

Retain the existing global replacement hook:

```csharp
GuardExceptionPolicy.ExceptionReplacer = ex => new BusinessException(ex.Message, ex);
GuardExceptionPolicy.ReplaceDefaultExceptions = true;
```

This keeps the current feature intact while improving readability.

### 3. Add a scoped policy API

Add a small scoped overlay API on top of the global policy.

Preferred shape:

```csharp
using var _ = GuardExceptionPolicy.BeginScope(options =>
{
    options.ExceptionReplacer = ex => new CheckoutException(ex.Message, ex);
    options.ReplaceDefaultExceptions = true;
});
```

Implementation guidance:

- Use an async-safe scope implementation (`AsyncLocal`) rather than raw mutable globals.
- Scoped policy should temporarily override the current effective policy and restore automatically on dispose.
- Keep the global static policy as the baseline.
- New scope options should only expose `ReplaceDefaultExceptions`, not the removed name.

### 4. Keep precedence simple and explicit

Guard failure resolution order should be:

1. per-call `exceptionCreator`
2. scoped `GuardExceptionPolicy`
3. global `GuardExceptionPolicy`
4. built-in default exception

Built-in defaults remain:

- `ArgumentNullException` when `value is null`
- otherwise `ArgumentException`

This preserves the current Guard model while making global/scoped substitution predictable.

---

## Proposed Implementation Shape

### Core files likely touched

- `src/PineGuard.Core/GuardClauses/GuardExceptionPolicy.cs`
- `src/PineGuard.Core/GuardClauses/GuardFailure.cs`
- `src/PineGuard.Core/GuardClauses/GuardExceptionPolicyScope.cs` (new, if needed)
- `src/PineGuard.Core/GuardClauses/GuardExceptionPolicyOptions.cs` (new, if needed)

### Test files likely touched

- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardExceptionPolicyTests.cs`
- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardExceptionPolicyTestData.cs`
- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardFailureTests.cs`
- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardFailureTestData.cs`

### Docs likely touched

- `README.md`
- `docs/ai/specs/guard-clauses/project.md`
- `docs/ai/plans/guard-exception-policy-uplift.md`

---

## Recommended Phases

## Phase 0 — Baseline and decision lock

Use `Plan` to confirm the uplift shape before code changes:

- direct rename, not compatibility aliasing
- scoped policy added on top of global policy
- no `Must` policy expansion
- README and spec examples updated in the same slice

Deliverable:

- approved plan and target API wording

## Phase 1 — Minimal implementation uplift

Use `validation-builder` to implement the smallest viable code change:

- rename `ReplaceArgumentExceptions` to `ReplaceDefaultExceptions`
- update `GuardFailure` / `ShouldReplace` to use the new name consistently
- add scoped policy support
- update internal references in the same slice so the solution stays coherent

Decision rule:

- prefer the smallest number of new types needed to keep the code readable
- avoid large abstraction layers unless they materially improve clarity

Deliverable:

- direct code change with the new name established end-to-end

## Phase 2 — Tests for rename + scope behavior

Use `test-writer` to update and extend coverage.

Required test strategy:

- rename existing test references to `ReplaceDefaultExceptions`
- update theory-data field names and display names to the new terminology
- add new tests for scoped override precedence
- add nested-scope tests if scope nesting is supported
- add a test proving `exceptionCreator` still wins over global/scoped policy
- add a test proving scope disposal restores the prior policy

Deliverable:

- renamed tests green
- new tests green
- branch behavior covered for global/scoped precedence

## Phase 3 — README and spec examples

Use `validation-builder` for code examples and doc updates, then `code-reviewer` for wording drift.

README should gain short, concrete examples for:

### Must example

```csharp
var result = Must.Be.Email(email);
result.ThrowIfFailed((message, paramName) => new BusinessException($"{paramName}: {message}"));
```

### Guard global example

```csharp
GuardExceptionPolicy.ExceptionReplacer = ex => new BusinessException(ex.Message, ex);
GuardExceptionPolicy.ReplaceDefaultExceptions = true;
```

### Guard scoped example

```csharp
using var _ = GuardExceptionPolicy.BeginScope(options =>
{
    options.ExceptionReplacer = ex => new CheckoutException(ex.Message, ex);
    options.ReplaceDefaultExceptions = true;
});
```

Also update the existing spec example in `docs/ai/specs/guard-clauses/project.md` so it matches the new property name and intended usage.

Deliverable:

- README examples aligned with the uplifted API
- spec example no longer contradicts runtime behavior

## Phase 4 — Review and coverage confirmation

Use:

- `code-reviewer` to catch architectural drift and wording mismatch
- `coverage-analyst` to confirm new branches/scenarios are covered

Deliverable:

- design review sign-off
- no uncovered branches introduced by the scope/precedence logic

---

## Test Plan

### Existing suites affected by the rename

- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardFailureTests.cs`
- `tests/PineGuard.Core.UnitTests/GuardClauses/GuardExceptionPolicyTests.cs`
- `tests/PineGuard.MustClauses.UnitTests/MustResultTests.cs`
- `tests/PineGuard.Core.UnitTests/MustClauses/MustResultTests.cs`

Note:

- The rename should only require Guard-policy test updates.
- The `MustResult` test suites are listed as validation checkpoints, not because they are expected to change.

### New tests to add

1. `ReplaceDefaultExceptions` can be set and read correctly.
2. Global policy replacement works through `ReplaceDefaultExceptions`.
3. Scoped policy temporarily overrides global policy.
4. Scope disposal restores the previous policy.
5. `exceptionCreator` beats both scoped and global policy.
6. Default fallback still throws `ArgumentNullException` / `ArgumentException` when no policy applies.
7. If nested scopes are supported, inner scope wins and outer scope is restored afterward.

### Test style guidance

- Extend existing `TheoryData`-based suites rather than creating disconnected one-off tests.
- Prefer additive test data in:
  - `GuardExceptionPolicyTestData`
  - `GuardFailureTestData`
- Keep current assertions and naming conventions intact while renaming terminology to `ReplaceDefaultExceptions`.

---

## README Positioning

### Must messaging

Keep Must framed as:

- result-first
- exception-free by default
- explicit throwing when the caller chooses it

### Guard messaging

Keep Guard framed as:

- throw-on-failure by design
- per-call override supported
- app-wide substitution supported
- scoped substitution supported

### Short differentiator line candidates

Preferred:

> **PineGuard lets you standardize validation once and substitute exception types to match your domain policy — without forking the library.**

Alternates:

- **One validation model, your exception policy.**
- **Keep NuGet updates. Keep your domain exceptions.**
- **Centralized exception substitution for Guard clauses — no source fork required.**

---

## Subagent-Oriented Execution Plan

### `Plan`

Use first to lock scope, file list, and rollout order.

### `validation-builder`

Use for:

- `GuardExceptionPolicy` implementation
- `GuardFailure` precedence updates
- README/spec example refresh

### `test-writer`

Use for:

- renamed and new unit tests
- extending existing theory data
- adding scope and precedence coverage

### `code-reviewer`

Use after implementation to check:

- Guard/Must boundary remains intact
- no accidental drift toward a `Must` global exception policy
- README/spec wording matches behavior

### `coverage-analyst`

Use last if the new scope/precedence logic introduces branch complexity that needs confirmation or a targeted follow-up.

---

## Success Criteria

- `ReplaceDefaultExceptions` is the active property name in code, tests, specs, and README examples.
- `ReplaceArgumentExceptions` is removed from the implementation plan and target API.
- Updated tests pass after the rename.
- New tests cover global, scoped, and per-call precedence.
- README contains concise Must and Guard exception examples.
- Guard spec example matches real behavior.
- PineGuard gains a short, professional differentiator for configurable Guard exception substitution.

---

## Recommended Rollout Decision

**Choose the direct rename path.**

That means:

- replace the current property name outright
- add scoped policy support on the clean name
- update tests/docs in the same slice
- verify with review and coverage

This is the quickest clean path for a new project and avoids carrying legacy naming into the long-term API.
