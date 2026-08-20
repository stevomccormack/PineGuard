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

- `tools/audit-cli/` (~40 scripts) — deferred to separate plan
- `tools/code-inspection/Install-Scoop.ps1` — replaced, not converted

---

## Fable Intelligence (2026-08-20)

### Challenge the premise before Phase 1: pwsh is already cross-platform

PowerShell 7+ (`pwsh`) runs natively on macOS and Linux, and GitHub-hosted `ubuntu-latest` runners ship it preinstalled. Before rewriting ~55 scripts in a second language, evaluate the cheaper alternative: **audit the existing scripts for Windows-only assumptions** (`Scoop`, registry access, `\` path literals, `cmd.exe` calls, Windows-only cmdlets) and fix those instead. A one-language toolchain is dramatically cheaper to maintain than a dual `.ps1` + `.sh` set that will drift the moment one side is edited under deadline pressure. The Bash rewrite is justified only if contributors genuinely won't install pwsh — worth an explicit decision note here before starting Phase 2.

### If the Bash migration proceeds — additions

- **Concrete CI payoff to bank first**: the `audit` job in `.github/workflows/ci.yml` runs on `windows-latest` solely because the tooling is PowerShell (documented in the workflow comments). Windows runners are slower to spin up and bill at 2x Linux minutes. Whichever path is chosen (Bash rewrite or pwsh-on-Linux), migrating that one job to `ubuntu-latest` is the highest-value single step — consider pulling it into Phase 1 as proof of the approach.
- **Phase 0 — lint/test harness before any conversion**: add `shellcheck` (mandatory, in CI) and `shfmt` for every `.sh` file, plus `bats-core` for the shared helpers in `tools/.shared/`. Converting 55 scripts without shellcheck in CI guarantees quoting and word-splitting bugs that PowerShell's object pipeline never had.
- **Drift is the real risk, not conversion**: "keep `.ps1` as reference" becomes "two diverging implementations" within months. Recommend: after a script's `.sh` is verified, either delete the `.ps1` or mark it frozen with a header comment pointing at the `.sh`. The Verification Strategy's diff-based validation should be a one-time gate, not an ongoing promise.
- **Dependency corrections**: `python3` is *not* guaranteed present — modern macOS only provides it via Xcode Command Line Tools, and Git Bash on Windows doesn't bundle it. Either document it as a hard prerequisite in `install-tools.sh`, or (better) replace the Cobertura XML parsing with the `dotnet-reportgenerator-globaltool` already vendored under `.dotnet/tools/`, keeping the toolchain .NET-only.
- **macOS Bash is 3.2** (GPL v3 licensing freeze): no associative arrays, no `mapfile`, older `[[` semantics. Either target Bash 3.2-compatible syntax or make Homebrew Bash 5 a documented prerequisite; decide once in Phase 1, not per-script.
- **Windows line-ending guard**: beyond `.gitattributes`, add a CI check that no `.sh` file contains CRLF — a single contributor with `core.autocrlf=true` produces `\r': command not found` failures that are miserable to diagnose.
