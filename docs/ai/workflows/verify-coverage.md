---
description: Verify code coverage sequentially in dependency order (Core -> Must -> Guards -> Fluent -> Data -> Testing) to isolate issues faster.
---

# Verify Coverage Sequential

> [!NOTE]
> This workflow adds nothing to the coverage tooling itself — [Run Coverage](coverage.md) owns the
> command surface. What it contributes is the **dependency order** and a **stop-on-first-gap gate**:
> a gap in Core will surface again in every layer above it, so the sweep halts at the first scope
> below 100% instead of collecting downstream noise.

Each step runs the scoped coverage command and stops the sweep if the scope is < 100%.

1. **PineGuard.Core** (Foundation)
   - **Context**: Read `docs/ai/specs/core/project.md`.
   - Run `/coverage-core`.
   - **Stop** if coverage is < 100%. Fix immediately.

2. **PineGuard.MustClauses** (Depends on Core)
   - **Context**: Read `docs/ai/specs/must-clauses/project.md`.
   - Run `/coverage-must`.
   - **Stop** if coverage is < 100%. Fix immediately.

3. **PineGuard.GuardClauses** (Depends on Must)
   - **Context**: Read `docs/ai/specs/guard-clauses/project.md`.
   - Run `/coverage-guard`.
   - **Stop** if coverage is < 100%. Fix immediately.

4. **PineGuard.FluentValidation** (Depends on Must)
   - **Context**: Read `docs/ai/specs/fluent-validation/project.md`.
   - Run `/coverage-fluent`.
   - **Stop** if coverage is < 100%. Fix immediately.

5. **PineGuard.DataAnnotations** (Depends on Must)
   - **Context**: Read `docs/ai/specs/data-annotations/project.md`.
   - Run `/coverage-annotation`.
   - **Stop** if coverage is < 100%. Fix immediately.

6. **PineGuard.Testing** (Shared test infrastructure)
   - **Context**: Read `docs/ai/specs/testing/unit-test.md`.
   - Run `/coverage-testing`.
   - **Stop** if coverage is < 100%. Fix immediately.

7. **Full sweep** (optional, once every scope is green)
   - Run `/coverage-all` to confirm the solution-wide numbers agree with the per-scope runs.
