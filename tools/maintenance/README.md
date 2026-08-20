# Maintenance

Cleanup scripts for deleting artifacts, logs, and root-level build detritus, plus the structural-integrity
checker that runs after folder or namespace moves.

## Directory Structure

```
tools/maintenance/
├── Run-Clean.ps1                # Master orchestrator
├── Clean-Artifacts.ps1          # Delete artifacts/
├── Clean-Logs.ps1               # Delete logs/
├── Clean-Root.ps1               # Delete files by extension from the repo root
└── Test-StructuralIntegrity.ps1 # Post-move regression checks (build, test, stale paths/namespaces)
```

## Usage

Run from the repository root.

### Selective cleanup

```powershell
# Clean logs only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Logs

# Clean artifacts only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Artifacts

# Clean root build files only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Root
```

### Combined cleanup

```powershell
# Clean logs and artifacts
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Logs -Artifacts

# Clean everything
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Logs -Artifacts -Root
```

### Advanced options

```powershell
# Recursive cleanup
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Artifacts -Recursive

# Clean all file types
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Artifacts -All

# Filter by extension
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Run-Clean.ps1" -Root -Extensions ".log",".tmp"
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Logs` | switch | `$false` | Run `Clean-Logs.ps1` |
| `-Artifacts` | switch | `$false` | Run `Clean-Artifacts.ps1` |
| `-Root` | switch | `$false` | Run `Clean-Root.ps1` |
| `-Extensions` | string[] | — | File extensions to target |
| `-All` | switch | `$false` | Pass to children (clean all file types) |
| `-Recursive` | switch | `$false` | Pass to children (recurse subdirectories) |

## Structural Integrity

`Test-StructuralIntegrity.ps1` catches regressions after a folder or namespace move: it builds, runs the tests,
and greps for stale path references in `.md` and `.ps1` files, stale namespace references in `.cs` files,
hardcoded Sonar paths that no longer exist on disk, and namespaces that no longer match their folder.

```powershell
# Everything (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Test-StructuralIntegrity.ps1"

# Quick build-only check
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Test-StructuralIntegrity.ps1" -Scope Build

# After a move: flag anything still referencing the old path or namespace
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Test-StructuralIntegrity.ps1" -Scope Paths -StalePaths "src/PineGuard.Old"
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/maintenance/Test-StructuralIntegrity.ps1" -Scope Namespaces -StaleNamespaces "PineGuard.Old"
```

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Scope` | string | `All` | Which checks to run: `All`, `Build`, `Test`, `Paths`, `Namespaces`, `Sonar` |
| `-StalePaths` | string[] | — | Old path patterns that must no longer appear in `.md`/`.ps1` files |
| `-StaleNamespaces` | string[] | — | Old namespace patterns that must no longer appear in `.cs` files |
| `-SkipBuild` | switch | `$false` | Skip the `dotnet build` check |
| `-SkipTest` | switch | `$false` | Skip the `dotnet test` check |

Results are written under `artifacts/audit/`.

## Safe Zones

| Target | What Gets Deleted | Risk |
|--------|-------------------|------|
| `artifacts/` | Coverage reports, audit output, generated previews | Low — regenerable |
| `logs/` | Script execution logs | Low — regenerable |
| Root | `*.txt`, `*.log` (by default) and other files at the repo root | Medium — check extensions before running |
