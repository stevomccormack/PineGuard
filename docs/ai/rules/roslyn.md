# Code Diagnostics Rules (Roslyn Compiler Warnings)

> Inherits from: `docs/ai/rules/global.md` (read first)

Before doing any diagnostics-related work, also read:
- `docs/ai/specs/tools/code-diagnostics/spec.md` (normative specification: warning categories, scopes, fix rules)
- `tools/code-diagnostics/README.md` (operational docs: usage, parameters, examples)

## Key Rules

1. **Never suppress warnings** with `#pragma warning disable` or `[SuppressMessage]`. Fix the root cause.
2. **Understand the why**: Investigate the root cause before fixing. Do not hot-fix.
3. **Fix issues one file at a time**. Verify `dotnet build PineGuard.slnx --no-incremental` compiles after each fix.
4. **Apply idiomatic C# fixes** following `docs/ai/specs/coding-standard.md`.
5. **All script output** goes to `artifacts/` (never the project root).
6. **No Docker required**: Roslyn diagnostics are built into `dotnet build`.
