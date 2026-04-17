# Plan: Cross-Platform Tools Migration (PowerShell → Bash)

## Context

The `tools/` directory contains ~91 PowerShell scripts that only run natively on Windows. To support Mac and Linux development, we will create Bash equivalents for all scripts (except audit-cli, deferred). The `.ps1` files will be kept as reference alongside the new `.sh` files.

## Goals

- Platform-agnostic tooling (Windows Git Bash, macOS, Linux)
- Bash `.sh` equivalents for all in-scope scripts (~55 scripts)
- Platform-specific alternatives for Windows-only scripts (e.g., Scoop → brew/apt)
- Audit-cli deferred to a separate future plan

## Conventions

- **Naming**: kebab-case for all `.sh` files (e.g., `Run-Tests.ps1` → `run-tests.sh`)
- **Shebang**: `#!/usr/bin/env bash` + `set -euo pipefail`
- **Colors**: ANSI escape codes (replacing `Write-Host -ForegroundColor`)
- **Args**: `while/case` argument parsing (replacing `[CmdletBinding()] param()`)
- **Imports**: `source` (replacing dot-sourcing)
- **Line endings**: `*.sh text eol=lf` in `.gitattributes`

## External Dependencies

| Tool | Purpose | Install |
|------|---------|---------|
| `jq` | JSON parsing (replaces `ConvertFrom-Json`) | `brew install jq` / `apt install jq` |
| `curl` | HTTP requests (replaces `Invoke-RestMethod`) | Built-in |
| `python3` | XML parsing for coverage utility (replaces `XmlReader`) | Built-in on Mac/Linux |

## Phases

### Phase 1: Foundation
- Create `tools/.shared/common.sh` — boilerplate, colors, `resolve_repo_root`, `command_exists`
- Create `tools/.shared/docker.sh` — network helpers
- Add `*.sh text eol=lf` to `.gitattributes`

### Phase 2: Docker & Infrastructure
- Convert all `tools/docker/*.ps1` (7 scripts)
- Convert `tools/sonar-scanner/*.ps1` (2 scripts)
- Key pattern: `Invoke-RestMethod` → `curl` + `jq`

### Phase 3: Dev Workflow
- Convert `tools/testing/Run-Tests.ps1`
- Convert `tools/code-formatter/Run-Format.ps1`
- Convert `tools/maintenance/*.ps1` (4 scripts)

### Phase 4: Git Automation
- Convert `tools/git/*.ps1` (~13 scripts)
- Key challenge: `Import-GitHelpers.ps1` (545 lines) — object returns become stdout + `$?`

### Phase 5: Code Inspection
- Convert `tools/code-inspection/*.ps1` (~5 scripts)
- Replace `Install-Scoop.ps1` with `install-tools.sh` (platform detection: brew/apt/scoop)

### Phase 6: Code Coverage
- Convert `tools/code-coverage/**/*.ps1` (~7 scripts)
- Key challenge: `Import-CodeCoverageUtility.ps1` (705 lines) — XML streaming → Python helper script
- Most complex phase; may need `parse-cobertura.py` support file

### Phase 7: Documentation
- Update `tools/README.md` and subdirectory READMEs with Bash usage
- Update `docs/ai/` agent references to support both `.ps1` and `.sh`

## Risks & Mitigations

| Risk | Mitigation |
|------|------------|
| macOS BSD `sed` differs from GNU `sed` | Use portable syntax or detect platform |
| XML parsing in pure Bash is fragile | Use Python helper for Cobertura parsing |
| Scripts may change during migration | Keep phases independent; convert from latest `.ps1` at time of phase |
| Git Bash on Windows has quirks | Test each phase on Git Bash before moving on |

## Verification Strategy

- Each script supports `--help`
- Side-effect scripts support `--dry-run` where practical
- Diff-based validation: run both PS1 and SH against same inputs, compare outputs
- Test on: Windows Git Bash, macOS (if available), Linux (CI)

## Out of Scope

- `tools/audit-cli/` (~40 scripts) — deferred to separate plan
- `tools/code-inspection/Install-Scoop.ps1` — replaced, not converted
