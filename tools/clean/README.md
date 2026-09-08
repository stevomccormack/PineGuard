# Clean

Cleanup scripts for deleting artifacts, logs, and root-level build detritus, plus the structural-integrity
checker that runs after folder or namespace moves.

## Directory Structure

```
tools/clean/
├── Run-Clean.ps1                # Master orchestrator
├── Clear-Artifacts.ps1          # Delete artifacts/
├── Clear-Logs.ps1               # Delete logs/
├── Clear-Root.ps1               # Delete files by extension from the repo root
└── Test-StructuralIntegrity.ps1 # Post-move regression checks (build, test, stale paths/namespaces)
```

`Clear-Artifacts.ps1`, `Clear-Logs.ps1` and `Clear-Root.ps1` share their enumerate/preview/delete
mechanics through one `Clear-ToolDirectory` function in `tools/.shared/clean.ps1` — each script
just calls it with its own target path, extensions, and recursion/exclusion behavior (see
"Safety differences between the three scripts" below).

## Usage

Run from the repository root.

### Selective cleanup

```powershell
# Clean logs only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Logs

# Clean artifacts only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Artifacts

# Clean root build files only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Root
```

### Combined cleanup

```powershell
# Clean logs and artifacts
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Logs,Artifacts

# Clean everything (either form works)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Logs,Artifacts,Root
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -All
```

### Advanced options

```powershell
# Recursive cleanup (Logs target only — Root and Artifacts no longer accept -Recursive)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Logs -Recursive

# Clean all file types
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Artifacts -All

# Filter by extension
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -Target Root -Extensions ".log",".tmp"

# Preview only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Run-Clean.ps1" -All -WhatIf
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Target` | string[] | `@()` | Which scripts to run: `Artifacts`, `Logs`, `Root` (any combination) |
| `-Extensions` | string[] | — | File extensions to target |
| `-All` | switch | `$false` | Passed to children (clean all file types); also a shorthand for `-Target Artifacts,Logs,Root` when `-Target` is not itself given |
| `-Recursive` | switch | `$false` | Passed to `Clear-Logs.ps1` only (recurse subdirectories); has no effect on the Root or Artifacts targets |

## Safety differences between the three scripts

Per T1.09, these three scripts are deliberately **not** behaviorally identical, and the shared
`Clear-ToolDirectory` function (`tools/.shared/clean.ps1`) preserves each difference through its
own call — never by adding a capability back that a script chose not to expose:

| Script | Recursion | Notes |
|--------|-----------|-------|
| `Clear-Artifacts.ps1` | Always recursive, unconditionally | No `-Recursive` parameter exists — artifacts are inherently nested report folders |
| `Clear-Logs.ps1` | Optional, via its own `-Recursive` switch | Forwards its switch value straight through to `Clear-ToolDirectory -AllowRecurse:$Recursive` |
| `Clear-Root.ps1` | Never — no `-Recursive` parameter exists, at all | Never passes `-AllowRecurse` to the shared function; also refuses a bare `-Extensions '*'` before calling it, and applies a hard exclusion list (`.git`, `.claude`, `node_modules`, `bin`, `obj`, `.vs`, `.idea`, `packages`) |

## Structural Integrity

`Test-StructuralIntegrity.ps1` catches regressions after a folder or namespace move: it builds, runs the tests,
and greps for stale path references in `.md` and `.ps1` files, stale namespace references in `.cs` files,
namespaces that no longer match their folder, and hardcoded Sonar paths that no longer exist on disk.

```powershell
# Everything (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Test-StructuralIntegrity.ps1"

# Quick build-only check
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Test-StructuralIntegrity.ps1" -Scope Build

# After a move: flag anything still referencing the old path or namespace
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Test-StructuralIntegrity.ps1" -Scope Paths -StalePaths "src/PineGuard.Old"
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/clean/Test-StructuralIntegrity.ps1" -Scope Namespaces -StaleNamespaces "PineGuard.Old"
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Scope` | string | `All` | Which checks to run: `All`, `Build`, `Test`, `Paths`, `Namespaces`, `Sonar` |
| `-StalePaths` | string[] | — | Old path patterns that must no longer appear in `.md`/`.ps1` files |
| `-StaleNamespaces` | string[] | — | Old namespace patterns that must no longer appear in `.cs` files |
| `-SkipBuild` | switch | `$false` | Skip the `dotnet build` check |
| `-SkipTest` | switch | `$false` | Skip the `dotnet test` check |

Results are written under `artifacts/clean/`.

### Namespace/folder alignment is registry-driven

The namespace/folder alignment check no longer walks all of `src/` with a hand-maintained folder
exclude list. It instead asks the registry (`Get-PineGuardScope -All`, `tools/.shared/dotnet-projects.ps1`)
for the authoritative set of real scope-root folders under `src/`, so a newly registered scope is
covered automatically. `Polyfills/` subfolders stay excluded unconditionally — every polyfill in
this repo intentionally declares a real BCL namespace (`System.Runtime.CompilerServices`, etc.), so
they can never "align" with their folder by design. `Common/` subfolders are **not** excluded:
most of them already align with their folder path (e.g. `PineGuard.DataAnnotations/Common` →
`PineGuard.DataAnnotations.Common`); the one real exception (`PineGuard.Core/Common`, which
declares the shared `PineGuard.Common` namespace) is reported as an informational note rather than
hidden, since this check never fails the run on a misalignment — it only informs.

The stale-namespace check (`-StaleNamespaces`) and the namespace/folder alignment check share a
single file enumeration and a single `Get-Content -Raw` read per `.cs` file, instead of each
scanning and reading the overlapping file set separately.

## Safe Zones

| Target | What Gets Deleted | Risk |
|--------|-------------------|------|
| `artifacts/` | Coverage reports, audit output, generated previews | Low — regenerable |
| `logs/` | Script execution logs | Low — regenerable |
| Root | `*.txt`, `*.log` (by default) and other files at the repo root | Medium — check extensions before running |
