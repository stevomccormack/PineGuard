<!-- metadata_header
type: plan
id: cross-platform-tools-migration
version: 1.0
status: planned
last_updated: 2026-08-20
-->

# Plan: Cross-Platform Tools Migration (PowerShell → Bash)

## Context

The `tools/` directory contains 90 PowerShell scripts that only run natively on Windows. To support Mac and Linux development, we will create Bash equivalents for all scripts (except audit-cli, deferred). The `.ps1` files will be kept as reference alongside the new `.sh` files.

## Goals

- Platform-agnostic tooling (Windows Git Bash, macOS, Linux)
- Bash `.sh` equivalents for all 53 in-scope scripts (everything except `tools/audit-cli/`)
- Platform-specific alternatives where a script assumes a Windows-only installer (e.g. Qodana CLI acquisition via brew/apt/direct download)
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
- Mirror the existing `tools/.shared/` split — one `.sh` per `.ps1` helper, same domain boundaries:
  `commands.sh`, `coverage.sh`, `docker.sh`, `dotenv.sh`, `dotnet-projects.sh`,
  `dotnet-tools-reportgenerator.sh`, `env.sh`, `git.sh`, `html.sh`, `path.sh`, `sonarqube.sh` (11 helpers)
- Do **not** invent a `common.sh` catch-all; the PowerShell side is already domain-split and the
  Bash side must stay one-to-one so the two can be diffed
- ~~Add `*.sh text eol=lf` to `.gitattributes`~~ — already satisfied (`.gitattributes` line 33)

### Phase 2: Docker & Infrastructure
- Convert all `tools/docker/*.ps1` (7 scripts)
- Convert `tools/sonar-scanner/*.ps1` (4 scripts)
- Key pattern: `Invoke-RestMethod` → `curl` + `jq`

### Phase 3: Dev Workflow
- Convert `tools/testing/Run-Tests.ps1`
- Convert `tools/code-formatter/Run-Format.ps1`
- Convert `tools/maintenance/*.ps1` (5 scripts)

### Phase 4: Git Automation
- Convert `tools/git/*.ps1` (12 scripts)
- Key challenge: `tools/.shared/git.ps1` (616 lines) — object returns become stdout + `$?`.
  `tools/git/Import-GitHelpers.ps1` is now a 17-line aggregator that dot-sources it, so its Bash
  counterpart is a one-line `source` shim

### Phase 5: Code Inspection
- Convert `tools/code-inspection/*.ps1` (4 scripts: `Initialize-Qodana.ps1`, `Run-Qodana.ps1`, and
  the two under `auto/`)
- Add cross-platform Qodana CLI acquisition (brew / apt / direct download)

### Phase 6: Code Coverage
- Convert `tools/code-coverage/**/*.ps1` (4 scripts)
- Key challenge: `tools/.shared/coverage.ps1` (469 lines) — XML streaming → Python helper script.
  `tools/code-coverage/Import-CodeCoverageUtility.ps1` is now a 21-line aggregator and becomes a
  one-line `source` shim
- Most complex phase; may need `parse-cobertura.py` support file

### Phase 7: Release & Diagnostics
- Convert `tools/release/*.ps1` (3 scripts) — `gh` CLI driven, so mostly argument plumbing
- Convert `tools/code-diagnostics/Run-CompilerDiagnostics.ps1` (1 script)

### Phase 8: Documentation
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

- `tools/audit-cli/` (37 scripts) — deferred to separate plan
