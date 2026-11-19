# String→DateOnly "in the past": `main` against the pilot branch

Benchmarks of the string→DateOnly pilot (decision register D29, pilot tracker chunk C6), run on 18 November 2026
between 09:56 and 20:00 UTC. The same benchmark source ran against the current path on `main` and the
rule-object path on the pilot branch, for the same inputs, on the same machine, one tree after the other. How the
comparison is produced, and what each benchmark calls, is in the project [README](../README.md).

**No row allocates more on the branch than on `main`.** Where the two trees differ, the branch allocates 64 bytes
less. No budget or threshold is set (D11); the figures are reported, not judged.

## Trees

| Tree | Commit measured | Path measured |
|---|---|---|
| `main` (current path) | `8b24472` (`8b24472919dc9a922030194475ec87362e0f74c0`), `origin/main` when measured, in a detached worktree with the benchmark project copied in uncommitted | `Must.Be.PastDateOnly` carries the rule inline; FluentValidation and DataAnnotations each skip a null value in their own code |
| Pilot branch (new path) | Main comparison and supporting checks: `98ca171` (`98ca17127042d82f6ea3c255609a6fc8e40298e5`). Guard failure paths: `ee4f9a5` (`ee4f9a52c9a8312ccfa3284651fc1acf15ad0849`). Both on `feature/dateonly-pilot`, with the benchmark project uncommitted and `src/` as committed | `Must.Be.PastDateOnly` maps the outcome of the internal rule object `StringPastDateOnlyRule`; FluentValidation and DataAnnotations call the Must layer's internal overload with `allowNull: true` |

`src/` is the same at both branch commits and at `main`'s later tip: `git diff 98ca171 ee4f9a5 -- src` and
`git diff 8b24472 d79da96 -- src` are empty (`d79da96`, `origin/main` when the Guard failure paths were measured,
adds only documentation and hook changes, which the branch merged as `4fb4369`).

In every pair of runs on this page the benchmark project was identical in both trees (compared with `diff -r`).
The Guard failure class (`Failures/GuardStringDateOnlyClausesFailureBenchmarks`) was added after the main comparison
and measured in its own pair of runs ([Guard failure paths](#guard-failure-paths)). After the measurements, the
benchmark classes changed in three ways only: their private fields were renamed to the repository's `_camelCase`
convention; the Must and Guard calls pass `paramName: "value"` explicitly, so the parameter name the clauses capture
stays `value`, the name they captured from the field before the rename; and the doc comment of
`GuardStringDateOnlyClausesBenchmarks` gained a pointer to the failure class. The measured calls, their inputs and
the messages they produce are unchanged. Guard and the Core predicate `StringRules.DateOnly.IsInPast` are
unchanged by the pilot; the Core predicate is the control.

## Environment

As printed by BenchmarkDotNet:

```text
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon Processor 2.30GHz, 1 CPU, 4 logical and 4 physical cores
.NET SDK 11.0.100
  [Host]     : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
```

Also from the run log: `GC=Concurrent Workstation`; AVX-512 hardware intrinsics, `VectorSize=256`. The machine is a
shared cloud container on a virtual machine (the CPU reports the `hypervisor` flag; Linux kernel 6.18.44), with other
work on the same host outside its control. Timing on it varies from run to run; see
[Uncertainty](#uncertainty).

## Job

BenchmarkDotNet's default job (`DefaultJob`), not `ShortRun` or `Dry`: the out-of-process `CsProjCoreToolchain`, one
launch per benchmark, automatic pilot and warm-up stages (6 to 15 warm-up iterations here), then 15 to 100 measured
iterations each, stopped by BenchmarkDotNet's default accuracy rule, with upper outliers removed. `[MemoryDiagnoser]`
counts the bytes allocated over one extra iteration after the measured ones and divides by its number of operations
(for example 1,258,291,200 B over 2,097,152 calls is the 600 B of FluentValidation on `PastDate` on `main`), so
`Allocated` is exact bytes per call whenever every call allocates the same. Release build, `net10.0`, from each
tree's root:

```bash
dotnet run -c Release -f net10.0 --project benchmarks/PineGuard.Benchmarks -- --filter '*' --artifacts <folder>
```

Wall time of the main comparison: `main` 09:56:00 to 10:15:49 UTC (19 min 49 s), then the branch 10:15:50 to
10:31:43 UTC (15 min 53 s); 35 min 43 s in all, builds included.

The tables are BenchmarkDotNet's GitHub Markdown export with its bold markup removed. BenchmarkDotNet decides per
table whether to show a `Median` column, and leaves out `Gen0` when no row of the table triggered a collection, as
for the Core predicate, which allocates nothing.

## Results on `main`

`StringRulesDateOnlyBenchmarks` (Core predicate, the control):

| Method   | Input     | Mean       | Error     | StdDev    | Median     | Allocated |
|--------- |---------- |-----------:|----------:|----------:|-----------:|----------:|
| IsInPast | PastDate  | 84.6219 ns | 1.8541 ns | 5.3495 ns | 82.4618 ns |         - |
| IsInPast | Today     | 83.7364 ns | 1.6956 ns | 2.3770 ns | 83.2994 ns |         - |
| IsInPast | NotADate  | 46.2166 ns | 0.9454 ns | 1.1254 ns | 45.8668 ns |         - |
| IsInPast | NullValue |  0.0000 ns | 0.0000 ns | 0.0000 ns |  0.0000 ns |         - |

`MustStringDateOnlyClausesBenchmarks`:

| Method       | Input         | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------- |-------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| PastDateOnly | PastDate      |  91.60 ns | 0.997 ns | 0.833 ns |  91.90 ns | 0.0002 |      64 B |
| PastDateOnly | Today         | 126.84 ns | 2.578 ns | 5.324 ns | 124.81 ns | 0.0005 |     152 B |
| PastDateOnly | NotADate      |  88.96 ns | 1.807 ns | 2.760 ns |  87.62 ns | 0.0005 |     152 B |
| PastDateOnly | NullValue     |  40.61 ns | 0.854 ns | 2.063 ns |  40.26 ns | 0.0005 |     136 B |
| PastDateOnly | InvalidStyles |  57.56 ns | 1.265 ns | 3.711 ns |  56.60 ns | 0.0008 |     216 B |

`GuardStringDateOnlyClausesBenchmarks`:

| Method                  | Input    | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |--------- |---------:|---------:|---------:|-------:|----------:|
| FutureOrPresentDateOnly | PastDate | 91.76 ns | 0.317 ns | 0.281 ns | 0.0002 |      64 B |

`FluentStringDateOnlyExtensionsBenchmarks`:

| Method       | Input         | Mean     | Error    | StdDev   | Median   | Gen0   | Allocated |
|------------- |-------------- |---------:|---------:|---------:|---------:|-------:|----------:|
| PastDateOnly | PastDate      | 302.2 ns |  4.10 ns |  3.42 ns | 299.9 ns | 0.0019 |     600 B |
| PastDateOnly | Today         | 920.2 ns | 18.41 ns | 50.41 ns | 895.3 ns | 0.0076 |    2064 B |
| PastDateOnly | NotADate      | 872.7 ns | 17.40 ns | 40.66 ns | 866.4 ns | 0.0076 |    2064 B |
| PastDateOnly | NullValue     | 190.1 ns |  3.86 ns |  7.16 ns | 190.5 ns | 0.0021 |     600 B |
| PastDateOnly | InvalidStyles | 819.0 ns | 16.36 ns | 44.78 ns | 801.4 ns | 0.0076 |    2168 B |

`StringDateOnlyAttributesBenchmarks`:

| Method             | Input         | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------------- |-------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| PastDateOnlyString | PastDate      | 100.59 ns | 2.723 ns | 7.811 ns |  97.00 ns | 0.0002 |      64 B |
| PastDateOnlyString | Today         | 149.85 ns | 2.868 ns | 7.755 ns | 145.44 ns | 0.0007 |     208 B |
| PastDateOnlyString | NotADate      | 114.01 ns | 2.918 ns | 8.372 ns | 111.60 ns | 0.0007 |     208 B |
| PastDateOnlyString | NullValue     |  12.37 ns | 0.342 ns | 0.992 ns |  12.18 ns | 0.0002 |      64 B |
| PastDateOnlyString | InvalidStyles |  88.59 ns | 1.524 ns | 1.273 ns |  87.97 ns | 0.0010 |     272 B |

BenchmarkDotNet warned `ZeroMeasurement` for `IsInPast` on `NullValue`: the call is indistinguishable from an empty
method, because the predicate returns `false` before it parses (its parsing helper rejects a null or blank string
first) and so never reads the clock.

## Results on the pilot branch

`StringRulesDateOnlyBenchmarks` (Core predicate, the control):

| Method   | Input     | Mean       | Error     | StdDev    | Median     | Allocated |
|--------- |---------- |-----------:|----------:|----------:|-----------:|----------:|
| IsInPast | PastDate  | 85.7692 ns | 1.8860 ns | 5.3808 ns | 83.3610 ns |         - |
| IsInPast | Today     | 80.7432 ns | 0.4867 ns | 0.4064 ns | 80.7297 ns |         - |
| IsInPast | NotADate  | 45.9530 ns | 0.7098 ns | 1.2053 ns | 45.4063 ns |         - |
| IsInPast | NullValue |  0.0336 ns | 0.0296 ns | 0.0518 ns |  0.0085 ns |         - |

`MustStringDateOnlyClausesBenchmarks`:

| Method       | Input         | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------- |-------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| PastDateOnly | PastDate      | 103.43 ns | 3.152 ns | 8.733 ns |  99.92 ns | 0.0002 |      64 B |
| PastDateOnly | Today         | 130.45 ns | 1.035 ns | 0.808 ns | 130.42 ns | 0.0005 |     152 B |
| PastDateOnly | NotADate      |  94.56 ns | 1.136 ns | 0.887 ns |  94.37 ns | 0.0005 |     152 B |
| PastDateOnly | NullValue     |  48.50 ns | 0.253 ns | 0.237 ns |  48.46 ns | 0.0005 |     136 B |
| PastDateOnly | InvalidStyles |  62.85 ns | 1.566 ns | 4.542 ns |  61.61 ns | 0.0007 |     216 B |

`GuardStringDateOnlyClausesBenchmarks`:

| Method                  | Input    | Mean     | Error   | StdDev   | Median   | Gen0   | Allocated |
|------------------------ |--------- |---------:|--------:|---------:|---------:|-------:|----------:|
| FutureOrPresentDateOnly | PastDate | 115.1 ns | 5.07 ns | 14.63 ns | 109.6 ns | 0.0002 |      64 B |

`FluentStringDateOnlyExtensionsBenchmarks`:

| Method       | Input         | Mean     | Error    | StdDev    | Median   | Gen0   | Allocated |
|------------- |-------------- |---------:|---------:|----------:|---------:|-------:|----------:|
| PastDateOnly | PastDate      | 304.3 ns |  4.77 ns |   3.73 ns | 302.3 ns | 0.0019 |     600 B |
| PastDateOnly | Today         | 895.7 ns | 17.60 ns |  17.29 ns | 887.9 ns | 0.0076 |    2064 B |
| PastDateOnly | NotADate      | 899.9 ns | 20.16 ns |  58.18 ns | 883.1 ns | 0.0076 |    2064 B |
| PastDateOnly | NullValue     | 197.1 ns |  3.94 ns |   7.68 ns | 195.0 ns | 0.0021 |     600 B |
| PastDateOnly | InvalidStyles | 918.9 ns | 43.13 ns | 127.16 ns | 876.6 ns | 0.0076 |    2168 B |

`StringDateOnlyAttributesBenchmarks`:

| Method             | Input         | Mean       | Error     | StdDev     | Median     | Gen0   | Allocated |
|------------------- |-------------- |-----------:|----------:|-----------:|-----------:|-------:|----------:|
| PastDateOnlyString | PastDate      | 108.946 ns | 3.3088 ns |  9.4401 ns | 104.825 ns | 0.0002 |      64 B |
| PastDateOnlyString | Today         | 164.861 ns | 5.3368 ns | 15.0524 ns | 157.755 ns | 0.0007 |     208 B |
| PastDateOnlyString | NotADate      | 122.356 ns | 2.2011 ns |  3.0128 ns | 121.455 ns | 0.0007 |     208 B |
| PastDateOnlyString | NullValue     |   7.906 ns | 0.1262 ns |  0.1054 ns |   7.879 ns |      - |         - |
| PastDateOnlyString | InvalidStyles |  91.548 ns | 0.5976 ns |  0.5298 ns |  91.560 ns | 0.0010 |     272 B |

BenchmarkDotNet warned `ZeroMeasurement` for `IsInPast` on `NullValue`, as on `main`, and `MultimodalDistribution`
for one `PastDateOnly` row of the Must class.

## Guard failure paths

`GuardStringDateOnlyClausesFailureBenchmarks` (`Failures/`): Guard on the three failing inputs, where each call
builds, throws and catches one exception. It was added after the runs above and measured in its own pair of runs on
the same machine, environment and job (the BenchmarkDotNet header was identical), `main` at `8b24472` from 19:51:53
to 19:55:26 UTC, then the branch at `ee4f9a5` from 19:55:26 to 19:57:53 UTC, with the command above filtered to
`'*GuardStringDateOnlyClausesFailureBenchmarks*'`. BenchmarkDotNet counted one exception per call on every row of
both runs, and leaves out `Gen0` because no collection fell in the counting iteration.

On `main`:

| Method                  | Input     | Mean     | Error     | StdDev    | Allocated |
|------------------------ |---------- |---------:|----------:|----------:|----------:|
| FutureOrPresentDateOnly | Today     | 1.392 μs | 0.0278 μs | 0.0660 μs |     488 B |
| FutureOrPresentDateOnly | NotADate  | 1.371 μs | 0.0273 μs | 0.0709 μs |     488 B |
| FutureOrPresentDateOnly | NullValue | 1.265 μs | 0.0253 μs | 0.0555 μs |     472 B |

On the pilot branch:

| Method                  | Input     | Mean     | Error     | StdDev    | Allocated |
|------------------------ |---------- |---------:|----------:|----------:|----------:|
| FutureOrPresentDateOnly | Today     | 1.336 μs | 0.0266 μs | 0.0355 μs |     488 B |
| FutureOrPresentDateOnly | NotADate  | 1.353 μs | 0.0260 μs | 0.0525 μs |     488 B |
| FutureOrPresentDateOnly | NullValue | 1.284 μs | 0.0249 μs | 0.0448 μs |     472 B |

BenchmarkDotNet raised no warning in either run.

## Allocation comparison

Bytes allocated per call, from the default-job runs above. A negative delta means the branch allocates less.

| Benchmark | Input | `main` | Branch | Delta | Explanation |
|---|---|---:|---:|---:|---|
| Core `IsInPast` | `PastDate` | 0 B | 0 B | 0 | |
| Core `IsInPast` | `Today` | 0 B | 0 B | 0 | |
| Core `IsInPast` | `NotADate` | 0 B | 0 B | 0 | |
| Core `IsInPast` | `NullValue` | 0 B | 0 B | 0 | |
| Must `PastDateOnly` | `PastDate` | 64 B | 64 B | 0 | |
| Must `PastDateOnly` | `Today` | 152 B | 152 B | 0 | |
| Must `PastDateOnly` | `NotADate` | 152 B | 152 B | 0 | |
| Must `PastDateOnly` | `NullValue` | 136 B | 136 B | 0 | |
| Must `PastDateOnly` | `InvalidStyles` | 216 B | 216 B | 0 | |
| Guard `FutureOrPresentDateOnly` | `PastDate` | 64 B | 64 B | 0 | |
| Guard `FutureOrPresentDateOnly`, failure path | `Today` | 488 B | 488 B | 0 | |
| Guard `FutureOrPresentDateOnly`, failure path | `NotADate` | 488 B | 488 B | 0 | |
| Guard `FutureOrPresentDateOnly`, failure path | `NullValue` | 472 B | 472 B | 0 | |
| FluentValidation `PastDateOnly` | `PastDate` | 600 B | 600 B | 0 | |
| FluentValidation `PastDateOnly` | `Today` | 2064 B | 2064 B | 0 | |
| FluentValidation `PastDateOnly` | `NotADate` | 2064 B | 2064 B | 0 | |
| FluentValidation `PastDateOnly` | `NullValue` | 600 B | 600 B | 0 | Equal on .NET 10 only. Code reading expected the branch to allocate one `MustResult` (64 B) less, because `main` replaces the failed result for a null value with a new successful one; .NET 8 and .NET 10 with the JIT's object stack allocation switched off both show exactly that (below). On .NET 10 as shipped the JIT keeps the extra object off the heap, so the trees allocate the same |
| FluentValidation `PastDateOnly` | `InvalidStyles` | 2168 B | 2168 B | 0 | |
| DataAnnotations `PastDateOnlyString` | `PastDate` | 64 B | 64 B | 0 | |
| DataAnnotations `PastDateOnlyString` | `Today` | 208 B | 208 B | 0 | |
| DataAnnotations `PastDateOnlyString` | `NotADate` | 208 B | 208 B | 0 | |
| DataAnnotations `PastDateOnlyString` | `NullValue` | 64 B | 0 B | −64 B | On `main` the attribute receives a failed `MustResult` for the null value and then discards it in its own null branch. On the branch the null policy is the rule's, and the Must layer returns a successful `MustResult` that the attribute reads only for its success flag; on .NET 10 the JIT allocates that object on the stack. On .NET 8, and on .NET 10 with object stack allocation switched off, both trees allocate 64 B (below), so the zero is a property of the .NET 10 JIT, not of the code |
| DataAnnotations `PastDateOnlyString` | `InvalidStyles` | 272 B | 272 B | 0 | |

Every other row is equal because the pilot changes how the Must result is assembled, not what is allocated. In the
Must clause both trees build one `MustResult` (64 B) per call, plus the formatted message on the four failing inputs
and the boxed `DateTimeStyles` on `InvalidStyles`: 152 B is 64 B and the 33-character message, 136 B is 64 B and the
23-character null message, 216 B is 64 B, a 24 B box and the 53-character styles message. Guard adds nothing on a
pass. On a failure Guard adds the same 336 B to the Must clause's figure on both trees (488 B is 152 B and 336 B,
472 B is 136 B and 336 B): the exception and what is built while throwing and catching it, by Guard code the pilot
does not change and by the runtime. The FluentValidation and DataAnnotations failure rows add the adapters' and the
frameworks' failure objects, built by code the pilot does not change. The Core predicate allocates nothing on
.NET 10 on either tree.

## Kill criterion

D29 kills the pilot if "the new path allocates where the current one does not". In no row, on any of the runs on
this page, does the branch allocate more than `main`: not in the 20 default-job rows on .NET 10, the 3 default-job
Guard failure rows or the 6 rows of the reverse-order repeat, not in the 23 `ShortRun` rows on .NET 8, not in the
10 `ShortRun` rows of the .NET 10 repeat, and not in the 10 rows on .NET 10 with object stack allocation switched
off. The only row in which one tree allocates and the other does not is DataAnnotations on `NullValue` on .NET 10,
where `main` allocates and the branch does not.

## Supporting allocation checks

Run on the same machine after the main comparison, between 10:32 and 10:56 UTC against `8b24472` and `98ca171`, and
for the Guard failure class between 19:58:03 and 19:59:49 UTC against `8b24472` and `ee4f9a5`. Except where stated,
they use BenchmarkDotNet's `ShortRun` job (3 warm-up and 3 measured iterations, one launch): their timing columns are
not meaningful and are not reproduced, but their `Allocated` figures are exact, like the default job's.

**.NET 8** (`-f net8.0`, both trees, every benchmark), to see whether the .NET 10 figures depend on the runtime:

| Benchmark | Input | `main` | Branch | Delta |
|---|---|---:|---:|---:|
| Core `IsInPast` | `PastDate` / `Today` / `NotADate` / `NullValue` | 0 / 0 / 24 / 0 B | 0 / 0 / 24 / 0 B | 0 |
| Must `PastDateOnly` | `PastDate` / `Today` / `NotADate` / `NullValue` / `InvalidStyles` | 64 / 152 / 176 / 136 / 216 B | 64 / 152 / 176 / 136 / 216 B | 0 |
| Guard `FutureOrPresentDateOnly` | `PastDate` | 64 B | 64 B | 0 |
| Guard `FutureOrPresentDateOnly`, failure path | `Today` / `NotADate` / `NullValue` | 616 / 640 / 600 B | 616 / 640 / 600 B | 0 |
| FluentValidation `PastDateOnly` | `PastDate` / `Today` / `NotADate` / `InvalidStyles` | 664 / 2128 / 2152 / 2232 B | 664 / 2128 / 2152 / 2232 B | 0 |
| FluentValidation `PastDateOnly` | `NullValue` | 728 B | 664 B | −64 B |
| DataAnnotations `PastDateOnlyString` | `PastDate` / `Today` / `NotADate` / `NullValue` / `InvalidStyles` | 64 / 208 / 232 / 64 / 272 B | 64 / 208 / 232 / 64 / 272 B | 0 |

The runtime, not the pilot, moves these figures away from .NET 10's, by the same amount on both trees. On .NET 8 the
unparseable input allocates 24 B more than the not-past input in every benchmark that runs both, the Core control
included (24 B against none); on .NET 10 the two allocate the same, and the Core predicate's only work on that input
is the runtime's `DateOnly.TryParse`. FluentValidation allocates 64 B more per call on every input than on .NET 10,
as it does on .NET 10 with object stack allocation switched off (below); `main`'s `NullValue` row adds the 64 B
`MustResult` discussed above. The Guard failure path allocates 128 B more per call on `Today` and `NullValue` (616 B
against 488 B, 600 B against 472 B) and 152 B more on `NotADate` (640 B against 488 B): the same 128 B, plus the
24 B the Must clause allocates more on that input.

**.NET 10 with object stack allocation switched off** (`--envVars DOTNET_JitObjectStackAllocation:0`, both trees,
FluentValidation and DataAnnotations):

| Benchmark | Input | `main` | Branch | Delta |
|---|---|---:|---:|---:|
| FluentValidation `PastDateOnly` | `PastDate` / `Today` / `NotADate` / `InvalidStyles` | 664 / 2128 / 2128 / 2232 B | 664 / 2128 / 2128 / 2232 B | 0 |
| FluentValidation `PastDateOnly` | `NullValue` | 728 B | 664 B | −64 B |
| DataAnnotations `PastDateOnlyString` | `PastDate` / `Today` / `NotADate` / `NullValue` / `InvalidStyles` | 64 / 208 / 208 / 64 / 272 B | 64 / 208 / 208 / 64 / 272 B | 0 |

**Repeats on .NET 10 as shipped.** A `ShortRun` of FluentValidation and DataAnnotations on both trees, and a
default-job run of Must and Guard with the branch first (see [Uncertainty](#uncertainty)): every `Allocated` figure
matched the main runs, including DataAnnotations `NullValue` at 64 B on `main` and 0 B on the branch.

The `ShortRun` repeat, `main` from 10:39:44 to 10:41:32 UTC, then the branch from 10:41:32 to 10:43:13 UTC:

| Benchmark | Input | `main` | Branch | Delta |
|---|---|---:|---:|---:|
| FluentValidation `PastDateOnly` | `PastDate` / `Today` / `NotADate` / `NullValue` / `InvalidStyles` | 600 / 2064 / 2064 / 600 / 2168 B | 600 / 2064 / 2064 / 600 / 2168 B | 0 |
| DataAnnotations `PastDateOnlyString` | `PastDate` / `Today` / `NotADate` / `InvalidStyles` | 64 / 208 / 208 / 272 B | 64 / 208 / 208 / 272 B | 0 |
| DataAnnotations `PastDateOnlyString` | `NullValue` | 64 B | 0 B | −64 B |

The reverse-order repeat, on the default job like the main comparison, the branch from 10:48:17 to 10:53:01 UTC,
then `main` from 10:53:01 to 10:56:20 UTC. [Uncertainty](#uncertainty) quotes its timing, so its tables are given
in full.

On the pilot branch (run first):

| Method       | Input         | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------- |-------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| PastDateOnly | PastDate      | 100.56 ns | 2.077 ns | 4.559 ns |  97.69 ns | 0.0002 |      64 B |
| PastDateOnly | Today         | 135.19 ns | 2.702 ns | 4.285 ns | 135.00 ns | 0.0005 |     152 B |
| PastDateOnly | NotADate      |  98.69 ns | 2.034 ns | 5.066 ns |  96.82 ns | 0.0005 |     152 B |
| PastDateOnly | NullValue     |  48.20 ns | 1.015 ns | 1.128 ns |  47.75 ns | 0.0005 |     136 B |
| PastDateOnly | InvalidStyles |  62.66 ns | 1.121 ns | 1.607 ns |  62.45 ns | 0.0007 |     216 B |

| Method                  | Input    | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |--------- |---------:|---------:|---------:|-------:|----------:|
| FutureOrPresentDateOnly | PastDate | 97.18 ns | 0.873 ns | 0.774 ns | 0.0002 |      64 B |

On `main`:

| Method       | Input         | Mean      | Error    | StdDev   | Gen0   | Allocated |
|------------- |-------------- |----------:|---------:|---------:|-------:|----------:|
| PastDateOnly | PastDate      |  93.07 ns | 1.884 ns | 2.313 ns | 0.0002 |      64 B |
| PastDateOnly | Today         | 121.48 ns | 1.651 ns | 1.379 ns | 0.0005 |     152 B |
| PastDateOnly | NotADate      |  88.10 ns | 1.741 ns | 4.304 ns | 0.0005 |     152 B |
| PastDateOnly | NullValue     |  39.05 ns | 0.306 ns | 0.272 ns | 0.0005 |     136 B |
| PastDateOnly | InvalidStyles |  54.03 ns | 0.484 ns | 0.405 ns | 0.0007 |     216 B |

| Method                  | Input    | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |--------- |---------:|---------:|---------:|-------:|----------:|
| FutureOrPresentDateOnly | PastDate | 91.19 ns | 0.316 ns | 0.280 ns | 0.0002 |      64 B |

BenchmarkDotNet warned `MultimodalDistribution` for two `PastDateOnly` rows of the Must class on the branch, and
raised no warning on `main`.

**.NET 11.** BenchmarkDotNet 0.15.8 does not recognise .NET 11 in its default toolchain (see the README). An
in-process `ShortRun` of the Must class on the branch on .NET 11.0.0 allocated the same bytes per input as on .NET 10
(64, 152, 152, 136 and 216 B). `main` was not run on .NET 11, so there is no .NET 11 comparison.

## Uncertainty

- **Allocations are the evidence.** They are exact per call for the runtime they were measured on, and they repeated
  exactly between runs. They are not a property of the source alone: the JIT's object stack allocation changes them
  between .NET 8 and .NET 10, as the two `NullValue` rows show. A claim of zero allocation holds only for the runtime
  measured.
- **Timing is reported, not relied on.** The machine is shared and virtualised, and the trees ran one after the
  other, not interleaved. The Core control, the same code in both trees, moved by up to 3.6% between the two runs
  (`Today`: 83.74 ns on `main`, 80.74 ns on the branch), leaving out its `NullValue` row, where both runs measured
  an empty method (`ZeroMeasurement`, 0.0000 ns and 0.0336 ns). Against that, the branch's Must clause was slower
  on every input, by 3% to 19% (3.6 to 11.8 ns), and Guard by 25% (115.1 ns against 91.76 ns, with a 14.63 ns
  standard deviation on the branch). A second default-job run of the Must and Guard classes in the reverse order
  (branch first, 10:48 to 10:56 UTC, [tables](#supporting-allocation-checks)) pointed the same way: Must slower by 8%
  to 23% (7.5 to 13.7 ns) on every input, Guard by 7% (97.18 ns against 91.19 ns), allocations identical.
  DataAnnotations, which calls the Must layer, was 3% to 10% slower on four inputs and faster on `NullValue` (7.91 ns
  against 12.37 ns), where the branch no longer allocates; FluentValidation, whose own work dominates its figures,
  moved between −3% and +12%, and the Guard failure paths, which the exception dominates, between −4% and +2% (their
  own pair of runs). The consistent direction in the Must clause suggests that the new path costs a few nanoseconds
  more per call, plausibly the forwarding call and the outcome lookups it adds, but this machine cannot establish it:
  a timing claim needs a dedicated machine and interleaved runs. D29's kill criterion concerns allocation, and D11
  sets no timing budget.
- **Scope.** One rule, steady state, one call per operation, the system under test warmed up. Cold start, validator
  and attribute construction, `Validator.TryValidateObject`, object and collection composition, and Guard on
  `InvalidStyles` are not measured, and no tree was compared on .NET 11. The inputs are short ISO dates; long or
  hostile strings are not measured.
