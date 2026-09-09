# Skill: Fix Roslyn Compiler Diagnostics
**ID**: pineguard.skill.fix-roslyn
**Version**: 1.0

## 1. Context & Goal
Run the Roslyn compiler diagnostics tool, then fix all reported CS warnings using idiomatic C#.

## 2. Inputs
- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`,
  `Options`, `DependencyInjection`, `AspNetCore`, `ErrorOr`, `FluentResults`, `OneOf`, `MediatR`,
  `Analyzers`, `Testing`) — which projects to analyze
- **Code**: (optional) Regex pattern to filter diagnostic codes (e.g. `CS86` for nullability, `CS0618` for obsolete)

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> - Fix one file at a time. Verify `dotnet build PineGuard.slnx --no-incremental` after each file.
> - Never suppress warnings (`#pragma warning disable`, `[SuppressMessage]`). Fix the root cause.
> - Understand the *why* before fixing. Do not hot-fix.
> - Apply idiomatic C# fixes following `docs/ai/specs/coding-standard.md`.
> - If a fix introduces a build error, revert and skip that warning.

## 4. Execution Steps

1. **Run Diagnostics**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope [SCOPE] -OutputFormat Json
   ```

   If a Code filter is provided, add `-Code [CODE]`.

2. **Parse the JSON Output**

   Read `artifacts/code-diagnostics/<scope>/diagnostics.json`. The report object is:

   | Field | Meaning |
   |-------|---------|
   | `Scope`, `Configuration`, `Code`, `Timestamp` | The invocation that produced this report (`Code` is the `-Code` filter, or `null`) |
   | `BuildSucceeded` | `false` if any build target failed to compile outright |
   | `FailedBuildTargets` | The projects/solution that failed to compile |
   | `TotalWarnings`, `TotalErrors` | Counts after the `-Code` filter |
   | `ByCode`, `ByFile` | Warning counts grouped by diagnostic code and by file |
   | `Warnings`, `Errors` | Two separate arrays, split by severity |

   Each entry in `Warnings` / `Errors` contains: `File`, `Line`, `Column`, `Severity`,
   `CodePrefix`, `CodeNumber`, `Code`, `Message`, `Project`, `TargetFrameworks` (the TFMs the
   diagnostic was seen in — entries are deduped across a multi-targeting project's per-TFM build
   passes) and `Occurrences`.

   > [!WARNING]
   > **Check `BuildSucceeded` and `Errors` before fixing anything.** A report with
   > `BuildSucceeded: false` came from a broken build, and its warning list is whatever the
   > compiler managed to emit before giving up — not a complete picture. Fix the compile errors in
   > `Errors` / `FailedBuildTargets` first (the script also exits `2` in this case), then re-run
   > the diagnostics and work from the fresh report.

3. **Fix Warnings (per file)**

   For each unique file in the warning list:
   1. Read the affected file.
   2. Understand the warning from `Code` and `Message`.
   3. Investigate the root cause — do not hot-fix.
   4. Apply an idiomatic fix.
   5. Build: `dotnet build PineGuard.slnx --no-incremental`
   6. If build fails, revert changes to that file and log the skip reason.

4. **Report**

   Summarize:
   - Total warnings found
   - Warnings fixed (file, code, line)
   - Warnings skipped (file, code, reason)

## 5. Definition of Done
- [ ] All fixable warnings for the requested scope/code filter are resolved
- [ ] Solution builds cleanly after all fixes
- [ ] Summary report provided

## 6. Reference Material (Deep Dive)
- `docs/ai/specs/tools/code-diagnostics/spec.md` (warning categories, fix rules)
- `docs/ai/specs/coding-standard.md` (formatting, naming)
- `tools/code-diagnostics/README.md` (tool usage)
