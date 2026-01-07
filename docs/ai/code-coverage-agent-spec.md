# PineGuard Code Coverage Agent Spec

This document is a **fast-start instruction set** for future sessions to reach **100% line + 100% branch coverage** for the repo’s **filtered scope** (typically `src/PineGuard.Core/**`).

Related: `docs/ai/unit-tests-agent-spec.md` (repo-specific unit test conventions and rules).

It captures the proven workflow and the common failure modes hit during the “drive to 100%” thread, so a new session can pick up immediately with minimal re-discovery.

---

## Ground Rules

- The repo’s gating standard (when enforcing) is **exactly 100%** for:
  - **Line coverage**, and
  - **Branch coverage**
  - within the **filtered scope** used by the analyzer script.
- Prefer **deterministic unit tests** (xUnit) over environment-dependent behavior.
- If a branch is **truly unreachable** (or redundant), prefer a **small production refactor** that preserves behavior rather than writing contrived tests.

---

## What “Filtered Scope” Means

Coverage is _collected_ broadly (via Coverlet/XPlat collector), then **analyzed** for a subset of files using:

- `AnalyzeCodeCoverage.ps1 -IncludeFileRegex` (default: `^src[\/]+PineGuard\.Core[\/]+`)
- `AnalyzeCodeCoverage.ps1 -ExcludeFileRegex` (default: `^src[\/]+PineGuard\.Core[\/]obj[\/]+`)

Important:

- Cobertura “class filename” values are sometimes odd (absolute paths, missing drive letters, or paths rooted under `src/PineGuard.Core`). The analyzer normalizes these so the default regex filters stay stable.

---

## Quick Start (Copy/Paste)

From repo root:

```powershell
# 1) Generate coverage (Cobertura XML + HTML)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1"

# 2) Analyze filtered scope and find the next targets
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Top 30

# 3) Enforce 100% (fails non-zero if not 100%)
# NOTE: The actual parameter name is -Enforce100 (PowerShell also accepts -Enforce as a prefix match).
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Enforce100
```

If you want a clean slate:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Clean
```

---

## Where Outputs Go

After `GenerateCodeCoverageReport.ps1`:

- HTML report:
  - `etc/generated/code-coverage/html/index.html`
- Cobertura XML (per test project run):
  - `etc/generated/code-coverage/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`

The analyzer automatically picks the **newest** Cobertura file per project.

---

## The Fast Iteration Loop (How to Reach 100%)

Repeat this loop until enforcement passes:

1. **Run generator** (`GenerateCodeCoverageReport.ps1`)
2. **Run analyzer** (`AnalyzeCodeCoverage.ps1 -Top 30`)
3. Pick the worst offenders (lowest branch/line)
4. **Write minimal tests** that flip the missing branches
5. Regenerate + reanalyze

This repo’s “last mile” is almost always **branch coverage**, not line coverage.

---

## How to Attack Remaining Branch Misses (The Proven Method)

### 1) Use the analyzer output as your target list

`AnalyzeCodeCoverage.ps1` prints “Lowest-covered classes” with:

- line %
- branch %
- (covered/total) lines and branches
- class + source file

Work top-down until all classes are 100%.

### 2) When branch is < 100%, consult Cobertura XML for exact conditions

Open the newest Cobertura file for the relevant test project under:

- `etc/generated/code-coverage/testresults/.../coverage.cobertura.xml`

Search inside for:

- the `<class name="..." filename="...">` entry
- `<line ... condition-coverage="50% (1/2)">` (or any `<100%`)

Those lines tell you:

- which methods still have partial condition coverage
- how many condition outcomes are missing

This is the fastest way to stop “guessing” and write exactly the tests required.

Tip for AI sessions: Workspace search may ignore `etc/generated/**`.

- If searching from tooling, ensure “include ignored files” is enabled.

### 3) Translate missing branches into TestData permutations

In this repo, most remaining branch misses map directly to **missing test permutations** rather than complex mocking.

Preferred approach:

- Add rows to `XxxTestData` datasets (`ValidCases`, `InvalidCases`, `ValidEdgeCases`, `InvalidEdgeCases`) to flip the exact missing condition outcomes.
- Prefer `TheoryData<TCase>` with record case types (named fields) so expanding permutations stays readable and deterministic.
- For guard/argument failures, assert exception `Type` and (when relevant) `ParamName` (see the unit test spec for the shared expected-exception shape).

See: `docs/ai/unit-tests-agent-spec.md`.

---

## Common Branch Patterns (and how to cover them)

### Short-circuit boolean (`||` / `&&`)

Example:

```csharp
if (minDigits < 1 || maxDigits < 1 || minDigits > maxDigits)
    return false;
```

To get **full branch coverage**, you often need **separate tests** that make each term independently responsible:

- `minDigits < 1` true
- `maxDigits < 1` true
- `minDigits > maxDigits` true

If you only hit one of them, Cobertura often stays at 66%/83%.

### Ternary selection

Example:

```csharp
var start = Start > other.Start ? Start : other.Start;
```

Write two tests:

- `Start > other.Start` true
- `Start > other.Start` false

### Pattern matching / type checks

Example:

```csharp
public override bool Equals(object? obj) => obj is Enumeration<TValue> other && Equals(other);
```

You need tests for:

- `obj` is an `Enumeration<TValue>`
- `obj` is **not** an `Enumeration<TValue>` (e.g., `new object()`, string, etc.)

### “Fallback” logic (territory/provider/lookup)

These often hide a missing branch that only occurs when:

- territory is null/whitespace
- territory equals default, but default mapping is missing
- provider argument is null so `??=` runs

For fallbacks, don’t guess values—build **test-local indexes** (e.g., tiny dictionaries) and call `internal static` overloads when available.

---

## Determinism Over Environment (OS-specific code)

Some code paths differ on Windows vs non-Windows (e.g., `OperatingSystem.IsWindows()`).

Preferred strategies:

1. If there is an **internal core method** that accepts `isWindows` (or similar), test that directly.

   - Example pattern used in this repo: `TryGetWindowsTimeZoneIdCore(..., bool isWindows)`

2. If the production method checks the OS internally, wrap tests with:

```csharp
if (!OperatingSystem.IsWindows())
    return;
```

and add a counterpart non-Windows test if needed.

Goal: avoid flaky tests that depend on local machine timezone availability.

---

## When It’s Better to Refactor Production Code

Sometimes the analyzer reports a branch that is **functionally unreachable** or impossible to distinguish with tests.

Guidelines:

- Only refactor when you can explain why the branch is unreachable or redundant.
- Preserve public behavior.
- Prefer small, local changes.
- After refactor, immediately rerun coverage to confirm the missing branch count actually decreases.

Example (pattern): avoid multi-enumeration and remove unreachable “logical gaps” in combinators.

Concrete example from the 100% push:

- A small refactor in the Core combinator for `MustResult` removed an unreachable branch and prevented repeated enumeration, which made branch accounting match reality.

---

## Stubborn Hotspots (Observed in the 100% Push)

These were repeatedly “last 1–2 branches” types of files. When a future session gets stuck, check these patterns first:

- Phone parsing/normalization utilities: typically hide `||` validation branches and “try-parse failed” branches.
- CLDR / Windows time zone utilities: OS-specific behavior and provider fallback branches.
- Rule helpers like printable ASCII checks: control chars/whitespace often require multiple inputs to flip all conditions.
- `Enumeration<T>` equality: needs an explicit non-enumeration `object` case to cover the negative type-check branch.
- Default provider implementations: have subtle “default territory” fallbacks that require carefully crafted input where the default mapping is missing.

---

## PowerShell / Regex Gotchas

- Prefer regexes that match both separators: use `[\\/]` inside patterns.
- Don’t rely on abbreviated parameter names long-term.
  - Use `-Enforce100` explicitly.
  - Use `-IncludeFileRegex` / `-ExcludeFileRegex` explicitly.
- When composing regex strings in PowerShell, remember backslashes are regex metacharacters; in double-quoted strings they also interact with escaping.
  - Simplest approach: keep regex patterns simple and use character classes like `[\\/]` rather than attempting to escape literal `\` sequences everywhere.

---

## Records and Compiler-Generated Members (Coverage Gotchas)

If you need to cover record copy/clone paths, explicitly invoke the compiler-generated copy behavior using:

```csharp
var copy = original with { };
```

This reliably executes synthesized members that might otherwise remain uncovered.

---

## GeneratedRegex and Generated Code

Coverage collection is configured via:

- `etc/powershell/code-coverage/coverlet.runsettings`

This repo intentionally excludes generated code via `ExcludeByAttribute` (including source generators such as `GeneratedRegex`).

Implications:

- You typically **do not** need to “cover generated regex files” to satisfy 100% in the filtered scope.
- If you intentionally change runsettings to include generated code, be prepared for:
  - unstable paths under `obj/**`
  - coverage drift between machines/builds

If the team decides generated code _should_ be included, update both:

- `coverlet.runsettings` (what gets collected)
- `AnalyzeCodeCoverage.ps1 -IncludeFileRegex` (what gets enforced)

---

## Troubleshooting (Known Issues and Fixes)

### 1) "No test is available" / test discovery weirdness

Use the repo scripts rather than IDE-integrated runners for coverage.

- The generator script only runs test projects that contain real `*.cs` sources.

### 2) "Coverage output looked invalid … Retrying once…"

This is expected occasionally.

- The generator script validates Cobertura output and retries once when it detects empty/invalid data.

### 3) Analyzer parameter confusion

The enforcement switch is:

- `-Enforce100`

PowerShell may accept abbreviated `-Enforce` today, but prefer `-Enforce100` for clarity and future-proofing.

### 4) Workspace search can miss generated coverage files

Some tools exclude `etc/generated/**`.

- If you’re hunting `condition-coverage="50%"`, ensure the search includes ignored/excluded folders.

---

## “Next Session” AI Prompt Template

Use this when starting a new session to avoid re-explaining context:

- Repo uses PowerShell scripts:
  - `etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1`
  - `etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1`
- Objective is **100% line + 100% branch** for filtered scope (default: `src/PineGuard.Core/**`).
- Workflow:
  1. Run generator
  2. Run analyzer
  3. Inspect Cobertura XML for `condition-coverage < 100%`
  4. Add minimal deterministic xUnit tests or remove unreachable branches
  5. Repeat until `-Enforce100` returns exit code 0

---

## Reference

- [etc/powershell/code-coverage/README.md](../../etc/powershell/code-coverage/README.md)
- [etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1](../../etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1)
- [etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1](../../etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1)
- `etc/powershell/code-coverage/coverlet.runsettings`
