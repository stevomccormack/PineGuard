# README verification — 25 September 2026

Source revision: `2aeac39b88effef5518741a9cf9b505a04f926f4`.
This is a local verification snapshot, not a live CI result. Production and test source were unchanged.

## Full test suite

After repairing the reference cache described below, the documented test runner rebuilt and ran all
15 test projects on both target frameworks in one `-Scope All` invocation, using its default Debug
configuration. The machine used Windows, .NET SDK 10.0.401, and the .NET 8 and .NET 10 runtimes.
Counts come from the 30 generated TRX files and include expanded theory cases. The run exited **0**.

| Target framework | Executed | Passed | Failed | Skipped | Exit code |
|---|---:|---:|---:|---:|---:|
| `net8.0` | 18,862 | 18,862 | 0 | 0 | 0 |
| `net10.0` | 18,887 | 18,887 | 0 | 0 | 0 |
| Combined | 37,749 | 37,749 | 0 | 0 | 0 |

The combined total includes the same tests running on both frameworks; it is not a count of distinct
test definitions. All 15 projects passed on both frameworks.

| Test project | .NET 8 passed / executed | .NET 10 passed / executed |
|---|---:|---:|
| Core | 5,329 / 5,329 | 5,329 / 5,329 |
| MustClauses | 3,303 / 3,303 | 3,303 / 3,303 |
| GuardClauses | 2,954 / 2,954 | 2,954 / 2,954 |
| FluentValidation | 3,758 / 3,758 | 3,758 / 3,758 |
| DataAnnotations | 2,396 / 2,396 | 2,396 / 2,396 |
| Extensions.Options | 24 / 24 | 24 / 24 |
| Extensions.DependencyInjection | 47 / 47 | 47 / 47 |
| AspNetCore | 118 / 118 | 143 / 143 |
| MediatR | 18 / 18 | 18 / 18 |
| ErrorOr | 15 / 15 | 15 / 15 |
| FluentResults | 24 / 24 | 24 / 24 |
| OneOf | 9 / 9 | 9 / 9 |
| Xml | 89 / 89 | 89 / 89 |
| Analyzers | 118 / 118 | 118 / 118 |
| Testing | 660 / 660 | 660 / 660 |

### Analyzer reference-cache repair

The initial .NET 8 run had **115 failed, 3 passed** in the analyzer project. A retry outside the execution
sandbox reproduced the same failures: `CS0518` for predefined types such as `System.Object`,
`System.String` and `System.Void`, alongside `CS0012` for `System.Runtime, Version=8.0.0.0`.

Investigation found that both extracted copies of `Microsoft.NETCore.App.Ref` version `8.0.0` in the
Roslyn test harness's temporary package cache had empty `ref/net8.0` directories. Their existing NuGet
archives were intact. Restoring the missing 163 reference DLLs and 104 XML files into each cache repaired
the local compiler-reference installation. The repair preserved existing files and did not change
PineGuard's production code, test assertions or analyzer behavior.

The targeted .NET 8 analyzer rerun passed **118/118**, followed by the successful full-suite run above.

### Reproduce

Run these sequentially from the repository root, choosing fresh output directories for each run:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/testing/Run-Tests.ps1 -Scope Analyzers -Framework net8.0 -OutputPath artifacts/testing/readme-analyzers-net8
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/testing/Run-Tests.ps1 -Scope All -OutputPath artifacts/testing/readme-all
```

Local evidence is retained in these ignored artifact directories; it is not distributed with a checkout:

- `artifacts/testing/20260925-full-suite-postrepair-terra` — final full-suite run, 30 passing TRX files.
- `artifacts/testing/20260925-analyzers-net8-postrepair-terra` — targeted verification, one passing TRX file.
- `artifacts/testing/20260925-full-suite-net8-terra` — initial run before repair, 15 TRX files.
- `artifacts/testing/20260925-full-suite-net10-terra` — initial .NET 10 run, 15 TRX files.
- `artifacts/testing/20260925-analyzers-net8-retry-terra` — initial failure reproduction, one TRX file.

The final aggregate JSON summary is retained alongside the results:

```text
artifacts/testing/20260925-full-suite-postrepair-terra/evidence.json
```

## Coverage and other checks

This test run did **not** collect new coverage or run SonarQube or Qodana. The README labels their
archived reports separately from today's test results and from the configured CI coverage threshold.

The checked-in CI workflow defaults to a 100% threshold for both line and branch coverage, overridable
through `MIN_CODE_COVERAGE`. Qodana is opt-in through `QODANA_ENABLED`; SonarQube has no job in the current
workflow. No dedicated fuzzing/property-based test harness, benchmark suite or automated cross-release
API compatibility gate was found in the source, project dependencies or workflows.

## Release and documentation checks

The public NuGet package indexes were checked on 25 September 2026. Core, MustClauses, GuardClauses,
FluentValidation, DataAnnotations and Testing were published at `0.1.0-alpha.7`; the other nine packages
were not published. The cached alpha package metadata points to source revision
`25cba5be60ae022ac76268e8d280504bfa1b2045`.

The quick start uses APIs present in that alpha. Failure codes, `MustValidator<T>` and
`GuardExceptionPolicy.Map` belong to the newer source tree. The README now distinguishes these from
the published package APIs.

All three README Mermaid diagrams were parsed and rendered with Mermaid 11 and inspected visually.
Local file links, README heading anchors and code-fence balance were also checked.
