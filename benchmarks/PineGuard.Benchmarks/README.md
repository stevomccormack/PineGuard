# PineGuard.Benchmarks

BenchmarkDotNet measurements of PineGuard validation paths: time per call and, above all, bytes allocated per
call.

This is TP-BENCH in the approved test-project layout
([TEST-LAYOUT-01](../../docs/ai/plans/technical-readiness-to-1.0/references/test-project-layout.md)). It is a
benchmark executable, **not a test project**: it references no test framework and no test assembly, sets
`IsTestProject` to `false`, and lives outside `tests/`, so the ordinary-test discovery in
`tools/.shared/dotnet-projects.ps1` never finds it and `dotnet test` runs nothing in it. It is registered in
`PineGuard.slnx`, so CI builds it, formats it and checks it for compiler warnings; no CI job runs it.

**It sets no thresholds and no budgets.** Numbers are reported, not judged by a gate (decision register D11). Do not
turn a figure here into an assertion in a unit test.

## What it measures

Today it holds the benchmarks of the string→DateOnly pilot (decision register D29): the rule "this string parses
to a `DateOnly` in the past", through the Core predicate and through each of the four adapters.

| Folder | Class | Benchmark | Calls | Inputs |
|---|---|---|---|---|
| `Rules/` | `StringRulesDateOnlyBenchmarks` | `IsInPast` | `StringRules.DateOnly.IsInPast(value, Inclusion.Exclusive, styles, clock)` | `PastDate`, `Today`, `NotADate`, `NullValue` |
| `Projections/` | `MustStringDateOnlyClausesBenchmarks` | `PastDateOnly` | `Must.Be.PastDateOnly(value, styles, clock, paramName: "value")` | all five |
| `Projections/` | `GuardStringDateOnlyClausesBenchmarks` | `FutureOrPresentDateOnly` | `Guard.Against.FutureOrPresentDateOnly(value, styles, clock, paramName: "value")` | `PastDate` |
| `Projections/` | `FluentStringDateOnlyExtensionsBenchmarks` | `PastDateOnly` | `validator.Validate(model)` on a validator using `.PastDateOnly(clock, styles)` | all five |
| `Projections/` | `StringDateOnlyAttributesBenchmarks` | `PastDateOnlyString` | `PastDateOnlyStringAttribute.GetValidationResult(value, context)` | all five |
| `Failures/` | `GuardStringDateOnlyClausesFailureBenchmarks` | `FutureOrPresentDateOnly` | the same Guard call inside `try`, returning the `ArgumentException` it throws | `Today`, `NotADate`, `NullValue` |

The layout's folders are used as follows: rejected inputs are parameter values of the `Rules/` and `Projections/`
classes, beside the passing ones; `Failures/` holds only the Guard exception path; `Common/` holds what every class
shares, the inputs and the fixed clock, and is support code, not one of the layout's workload folders;
`Composition/` (object and collection validation) is not used yet. The project keeps its own fixed `TimeProvider`,
pinned to the same instant as `PineGuard.Testing`'s, instead of referencing `PineGuard.Testing`, because that
library references xunit and this project references no test framework and no test assembly.

The five inputs (`Common/PastDateOnlyInput.cs`) each take a different path through the rule:

| Input | Value | Styles | Path |
|---|---|---|---|
| `PastDate` | `"2000-01-01"` | `AllowWhiteSpaces` | parses, before today: passes |
| `Today` | today on the fixed clock, `"2026-06-15"` | `AllowWhiteSpaces` | parses, not before today: fails |
| `NotADate` | `"not-a-date"` | `AllowWhiteSpaces` | does not parse: fails before the clock is read |
| `NullValue` | `null` | `AllowWhiteSpaces` | fails in Core, Must and Guard; passes in FluentValidation and DataAnnotations |
| `InvalidStyles` | `"2000-01-01"` | `AssumeUniversal` | styles rejected before the value is read: fails |

How each workload is set up:

- **Deterministic clock.** Every workload reads `FixedTimeProvider.Default` (`Common/FixedTimeProvider.cs`), pinned to
  2026-06-15T12:00:00Z in UTC, the instant the repository's temporal test scenarios use. Parsing uses the invariant
  culture. The outcome of every input is therefore the same on every machine and every day.
- **Set-up outside the measurement.** Inputs are resolved, and the FluentValidation validator, the attribute and the
  `ValidationContext` are built, once in `[GlobalSetup]`; each benchmark is the one call in the table and returns
  its result, so nothing is optimised away.
- **Parameter name passed explicitly.** The Must and Guard calls pass `paramName: "value"`. Otherwise the clauses
  would capture the field's name, `_value`, and the longer name in the failure messages that name the value would
  change the bytes allocated.
- **What the adapter figures include.** The FluentValidation figure includes FluentValidation's own per-call work
  (its validation context and result). The DataAnnotations figure is the attribute alone, on one reused
  `ValidationContext` whose service provider supplies the clock; it excludes `Validator.TryValidateObject`.
- **Guard, split by path.** Guard returns the parsed date on a pass and throws on every failure. The passing input
  is measured with the other projections, as the mapping alone. The failing inputs are measured in `Failures/`
  (the layout's folder for error construction): each call builds, throws and catches one exception and returns it,
  so those figures are the whole exception path, of which the Must clause's share is measured on the same inputs
  in `Projections/`. A failing input that does not throw makes the benchmark fail rather than measure a pass.
- **`InvalidStyles` only where a result reports it.** Neither the Core predicate nor Guard is run on it: the
  predicate returns `false` before it parses, and Guard's path is the Must clause's `InvalidStyles` failure, measured
  in `Projections/`, followed by an exception like the ones measured in `Failures/`.
- `[MemoryDiagnoser]` is on every class, so each row reports `Allocated` (bytes per call) and, when a table had a
  collection, `Gen0`.

Only the Guard exception path is in `Failures/`, because it cannot share a benchmark method with Guard's passing
path. The failing inputs of the Core predicate and of the result-returning projections are rejected inputs too, but
each of those calls returns its failed result the same way as a passed one, so they sit beside their passing inputs
as parameter values and each table compares every input of one call.

## Run

From the repository root, Release only (BenchmarkDotNet refuses an unoptimised build):

```bash
# Everything, default job, on .NET 10
dotnet run -c Release -f net10.0 --project benchmarks/PineGuard.Benchmarks -- --filter '*' --artifacts artifacts/benchmarks

# One class
dotnet run -c Release -f net10.0 --project benchmarks/PineGuard.Benchmarks -- --filter '*MustStringDateOnlyClausesBenchmarks*' --artifacts artifacts/benchmarks

# Smoke check that every benchmark executes (one iteration each, numbers meaningless)
dotnet run -c Release -f net8.0 --project benchmarks/PineGuard.Benchmarks -- --filter '*' --job Dry --artifacts artifacts/benchmarks
```

`--artifacts artifacts/benchmarks` keeps BenchmarkDotNet's logs and reports under the git-ignored `artifacts/`
folder, as the repository's file-hygiene rule requires; without it they go to `BenchmarkDotNet.Artifacts/` in the
current directory, which is git-ignored too. Never commit raw artifacts: results are committed as a reviewed
Markdown summary under `Results/`.

The project targets `net8.0`, `net10.0` and `net11.0` (from `benchmarks/Directory.Build.props`); pick the runtime
with `-f`. BenchmarkDotNet 0.15.8, the newest stable release, predates .NET 11 and does not recognise it: on
`net11.0` its default toolchain stops before running anything (`GetRuntimeVersion not implemented for
NotRecognized`). There, add `--inProcess`, which runs each benchmark inside the host process instead of a generated
one:

```bash
dotnet run -c Release -f net11.0 --project benchmarks/PineGuard.Benchmarks -- --filter '*' --inProcess --artifacts artifacts/benchmarks
```

The committed results use `net10.0` and the default out-of-process toolchain.

## Results

| Results | What they compare |
|---|---|
| [`Results/string-past-date-only.md`](Results/string-past-date-only.md) | The string→DateOnly pilot: the current path on `main` against the rule-object path on the pilot branch, same benchmarks, same inputs, same machine |

### How the main-versus-branch comparison is produced

The benchmarks call public API only, and only API that exists unchanged on both `main` and the pilot branch, so the
identical source runs against both trees (pilot tracker, design decision F-4). The rule object itself is
`internal` and is reached, on the branch, through the Must layer.

1. Create a detached worktree of `main` outside the repository: `git worktree add --detach <dir> origin/main`.
2. Copy `benchmarks/` (without `bin/` and `obj/`) into it, and add the `BenchmarkDotNet` `PackageVersion` line to
   its `Directory.Packages.props`. Nothing is committed in that worktree.
3. Run the same command in both trees, one after the other on the same machine, each with its own `--artifacts`
   folder.
4. Compare the two result tables row by row, and remove the worktree with `git worktree remove <dir>`.

Treat the `Allocated` column as the evidence. On a shared or virtualised machine the timing columns vary from run to
run by more than the differences between the two trees; each results file says where it was run.
