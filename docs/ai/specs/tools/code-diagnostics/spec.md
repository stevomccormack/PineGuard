<!-- metadata_header
type: spec
id: spec-code-diagnostics
version: 1.0
-->

# Code Diagnostics Specification (Roslyn Compiler Warnings)

> [!IMPORTANT]
> Normative specification for Roslyn compiler diagnostics tooling in PineGuard.

## 1. Overview

PineGuard uses the Roslyn C# compiler (built into `dotnet build`) to detect code quality issues at compile time. Unlike SonarQube or Qodana, Roslyn diagnostics require **no Docker container or external tool** — they are emitted natively during the build process.

Warning codes use the `CS` prefix (e.g., CS8604, CS8619, CS0618).

## 2. How It Differs From Other Tools

| Tool | Nature | External? | Output |
|------|--------|-----------|--------|
| **Roslyn** | Compiler-native warnings from `dotnet build` | No | Structured JSON + text |
| **SonarQube** | External static analysis + web dashboard | Yes (Docker) | Dashboard + API |
| **Qodana** | JetBrains inspections + SARIF reports | Yes (Docker) | SARIF JSON + HTML |
| **Audit CLI** | Custom Roslyn workspace AST analysis | No (custom tool) | Text + JSON |

## 3. Warning Categories

| Category | Code Range | Description |
|----------|-----------|-------------|
| Nullability | CS8600-CS8655 | Nullable reference type analysis (CS8604 null arg, CS8619 nullability mismatch, etc.) |
| Obsolete | CS0612, CS0618 | Deprecated API usage |
| Unused | CS0168, CS0219, CS8321 | Unused variables, parameters, local functions |
| Async | CS1998, CS4014 | Missing await, async methods without await |
| General | Other CS codes | Type conversion, accessibility, miscellaneous |

## 4. Scope Model

| Scope | Build Target |
|-------|-------------|
| All | `PineGuard.slnx` |
| Core | `src/PineGuard.Core/PineGuard.Core.csproj` |
| MustClauses | `src/PineGuard.MustClauses/PineGuard.MustClauses.csproj` |
| GuardClauses | `src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj` |
| FluentValidation | `src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj` |
| DataAnnotations | `src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj` |
| Testing | `tests/PineGuard.Testing/PineGuard.Testing.csproj` |

## 5. Output Paths

| Artifact | Path |
|----------|------|
| JSON report | `artifacts/code-diagnostics/<scope>/diagnostics.json` |
| Text summary | stdout |

## 6. Tool Scripts

| Script | Purpose |
|--------|---------|
| `tools/code-diagnostics/Run-CompilerDiagnostics.ps1` | Build, capture, parse, and report warnings |

## 7. Fix Workflow Rules

1. **One file at a time**: Fix warnings in a single file, then verify build.
2. **Build after each fix**: Run `dotnet build PineGuard.slnx --no-incremental` after each file.
3. **Never suppress warnings**: Do not add `#pragma warning disable` or `[SuppressMessage]` to hide issues.
4. **Idiomatic C#**: Apply fixes using PineGuard coding standards (`docs/ai/specs/coding-standard.md`).
5. **Understand the root cause**: Do not hot-fix. Investigate *why* the warning exists and fix correctly.
6. **Report**: Summarize fixed vs skipped warnings when done.

## 8. References

- Tool README: `tools/code-diagnostics/README.md`
- Coding standards: `docs/ai/specs/coding-standard.md`
- Safety spec: `docs/ai/specs/safety.md`
