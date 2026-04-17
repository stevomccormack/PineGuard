# Maintenance

Cleanup scripts for deleting artifacts, logs, and root-level build detritus.

## Directory Structure

```
tools/maintenance/
├── Run-Clean.ps1          # Master orchestrator
├── Clean-Artifacts.ps1    # Delete artifacts/
├── Clean-Logs.ps1         # Delete logs/
└── Clean-Root.ps1         # Delete files by extension from the repo root
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

## Safe Zones

| Target | What Gets Deleted | Risk |
|--------|-------------------|------|
| `artifacts/` | Coverage reports, audit output, generated previews | Low — regenerable |
| `logs/` | Script execution logs | Low — regenerable |
| Root | `*.txt`, `*.log` (by default) and other files at the repo root | Medium — check extensions before running |
