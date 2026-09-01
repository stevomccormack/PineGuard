---
name: analyzer-package-verification
description: Verifying a packed PineGuard.Analyzers .nupkg end to end — Info diagnostics never print in dotnet build output, and a consumer smoke project needs local props stubs to escape the repo build
metadata:
  type: project
---

# Verifying the analyzer package as a consumer

Two traps make a correctly working analyzer package look broken when you smoke-test it.

**Info-severity diagnostics never reach `dotnet build` console output** — not at `-v n`, not at
`-v d`. `PG1001`–`PG1004` ship at `DiagnosticSeverity.Info` by design (plan 06 §2.1), so a smoke
build prints nothing even when the analyzer is loaded and firing. To observe one, drop an
`.editorconfig` beside the scratch project with `dotnet_diagnostic.PG1001.severity = warning` and
rebuild `--no-incremental` (analyzer results are cached, so an incremental build re-prints nothing).
The `PG2xxx` pair are Warnings and do print unaided.

**Why:** Plan 06's Definition of Done says the smoke project "shows `PG1001` in `dotnet build`
output". Read literally that is unachievable at the shipped severity — it is a wording gap in the
plan, not a packaging defect.

**How to apply:** When checking a packed analyzer, verify two independent things instead of one
console line: (1) the csc command line in `-v d` output passes
`/analyzer:…\.nuget\packages\pineguard.analyzers\<ver>\analyzers\dotnet\cs\*.dll` — that proves NuGet
accepted the package layout; (2) the diagnostic fires, with severity temporarily raised.

**A scratch consumer project under `artifacts/` is still inside the repo**, so the root
`Directory.Build.props` (warnings-as-errors, XML docs, MinVer, SourceLink) and
`Directory.Packages.props` (central package management) apply and fight the consumer scenario. Put
an empty `Directory.Build.props` and a `Directory.Packages.props` with
`ManagePackageVersionsCentrally=false` in the scratch folder to stop the upward walk, plus a
`nuget.config` with `<clear/>` and a `local` source pointing at `../nupkg`. `artifacts/` is
gitignored, so nothing there can be committed by accident.

Consuming `PineGuard.GuardClauses` from the local feed also needs `PineGuard.Core` and
`PineGuard.MustClauses` packed into the same folder — `dotnet pack` takes one project at a time
(`MSB1008` otherwise), so pack all three in a loop.

Related: [[layer-signatures]].
