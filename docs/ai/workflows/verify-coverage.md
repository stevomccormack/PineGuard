---
description: Verify code coverage sequentially (Core -> Must -> Guards -> Fluent -> Data) to isolate issues faster.
---

# Verify Coverage Sequential

1. **PineGuard.Core** (Foundation)
   - **Context**: Read `docs/ai/specs/core/project.md`.
   - Run coverage for Core.
   - **Stop** if coverage is < 100%. Fix immediately.

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Scope Core
   ```

2. **PineGuard.MustClauses** (Depends on Core)
   - **Context**: Read `docs/ai/specs/must-clauses/project.md`.
   - Run coverage for MustClauses.
   - **Stop** if coverage is < 100%. Fix immediately.

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Scope MustClauses
   ```

3. **PineGuard.GuardClauses** (Depends on Must)
   - **Context**: Read `docs/ai/specs/guard-clauses/project.md`.
   - Run coverage for GuardClauses.
   - **Stop** if coverage is < 100%. Fix immediately.

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Scope GuardClauses
   ```

4. **PineGuard.FluentValidation** (Depends on Guards)
   - **Context**: Read `docs/ai/specs/fluent-validation/project.md`.
   - Run coverage for FluentValidation.
   - **Stop** if coverage is < 100%. Fix immediately.

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Scope FluentValidation
   ```

5. **PineGuard.DataAnnotations** (Depends on Must)
   - **Context**: Read `docs/ai/specs/data-annotations/project.md`.
   - Run coverage for DataAnnotations.
   - **Stop** if coverage is < 100%. Fix immediately.

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Scope DataAnnotations
   ```
