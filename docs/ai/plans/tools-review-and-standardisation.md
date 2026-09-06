<!-- metadata_header
type: plan
id: tools-review-and-standardisation
version: 1.1
status: decisions-signed-off
last_updated: 2026-09-06
scope: tools/** excluding tools/audit-cli/** (under separate review)
-->

# Plan: `tools/` Review and Standardisation

## 0. Executive summary

A full read of every file under `tools/` (59 scripts, 11 READMEs, 14 Qodana configs, 14 Qodana
solution files, 2 compose files, 1 runsettings, the governing spec, the CI workflows, and every
adapter that references a tool path) found a toolchain that is functionally rich but has drifted
into **three naming vocabularies, four repo-root resolvers, sixteen near-identical commit scripts,
two sources of truth for coverlet settings, a Docker stack nothing uses, one Tier 0 safety-spec
violation in the git helper, and a compiler-diagnostics tool that cannot see warnings and ignores
build failures.**

Coverage keeps two engines. Coverlet, which the current `xplat/` folder misnames after its
collector's display string, stays authoritative; JetBrains dotCover is reintroduced as a
first-class local engine under the same script contract (§3.5).

The plan has six phases. Phase 0 is a single Fable-tier decision session that fixes the naming
taxonomy once, so every later rename cascades exactly one time. Phases 1 and 2 are hotfixes and a
lint/test harness that need no naming decision and can start immediately in parallel. Phase 3
consolidates by domain. Phase 4 executes the rename cascade under a stale-path gate. Phase 5
rewrites docs and the spec. Phase 6 is independent verification.

Orchestration is a **Sonnet 5 orchestrator that never edits**; Haiku does bulk reads and
mechanical rewrites, Sonnet does all coding, Opus verifies, Fable decides names. The playbook in
§7 lists every task with its agent tier, dependency, and fan-out group.

---

## 1. Scope, method, and exclusions

| Item | Detail |
|---|---|
| In scope | `tools/**` except `tools/audit-cli/**` |
| Read | Every `.ps1`, `.md`, `.yml`, `.yaml`, `.slnx`, `.runsettings` under scope; `docs/ai/specs/tools/**`, `docs/ai/specs/scan/spec.md`, `docs/ai/specs/safety.md`, `docs/ai/rules/tools.md`, `.github/workflows/ci.yml`, `.vscode/tasks.json`, `.gitattributes`, `.editorconfig`, `.etc/powershell/**` (adjacent, for overlap only) |
| Tooling checks run | BOM/CRLF/StrictMode/CmdletBinding/trailing-whitespace inventory per script; dangling-reference sweep over `docs/ai`, `.claude`, `.github`, `.agent`, `.pi`; scope-parity diff between the registry, commit scripts, release lists, Qodana configs, and `PineGuard.slnx`; PSScriptAnalyzer 1.24 baseline (Phase 2 records the numbers) |
| Excluded | `tools/audit-cli/**` (another agent owns it). Two in-scope scripts dot-source it; Phase 3 removes that coupling so the two reviews cannot collide. `.etc/powershell/**` is adjacent, not in scope; one optional task is listed. |
| Environment | pwsh 7.6.5, PSScriptAnalyzer 1.24.0 present, .NET 10 SDK, Windows 11 |

---

## 2. Findings

Findings are numbered `F-nn` so playbook tasks can cite them. Severity: **S1** safety or
correctness bug, **S2** standards or DRY defect, **S3** hygiene or documentation drift.

### 2.1 Naming and taxonomy (the primary concern)

| ID | Sev | Finding | Evidence |
|---|---|---|---|
| F-01 | S2 | **Three vocabularies for the same domains.** Folders use activity names (`code-coverage`, `code-diagnostics`, `code-formatter`, `code-inspection`) *and* product names (`sonar-scanner`, `docker`, `git`). Scripts use a third mix (`Run-CompilerDiagnostics` in `code-diagnostics`, `Run-Qodana` in `code-inspection`, `Run-Format` in `code-formatter`). Slash commands and rules use a fourth (`/scan-roslyn-*`, `rules/roslyn.md`, `/scan-sonar`, `rules/scan.md`). One thing has up to four names. | `tools/README.md`, `CLAUDE.md` palette, `docs/ai/rules/` |
| F-02 | S2 | **`Run-` is defined as "orchestrator" but two `Run-` scripts are single actions.** `Run-GithubRuleset.ps1` toggles one flag; `Run-NugetUnlist.ps1` performs one operation. Neither orchestrates anything. | `tools/release/*.ps1`, spec §1.1 |
| F-03 | S2 | **`Gen-` is an invented verb.** PowerShell's approved verb for generators is `New-`. The spec blesses `Gen-` (§1.1) on the strength of one script. | `tools/code-coverage/xplat/Gen-CoverageReport.ps1` |
| F-04 | S2 | **`Initialize-` and `Setup-` both mean "bootstrap" in the same folder, but do different things.** `Initialize-SonarQube` installs Java and the scanner then starts the container; `Setup-SonarQube` commissions the server via API. `Setup` is not an approved verb. `Initialize-Qodana` matches the *install* meaning. | `tools/sonar-scanner/`, `tools/code-inspection/` |
| F-05 | S2 | **`tools/docker/*.ps1` are lowercase kebab-case.** The spec (§1.3) calls the rename to `Start-*`/`Stop-*` "a tracked follow-up, not current practice". That is an admitted outlier that was never chosen, only deferred. | spec §1.3 |
| F-06 | S2 | **Unapproved verbs in shared functions**: `Ensure-Directory`, `Ensure-DockerNetwork`, `Ensure-ReportGenerator`, `Ensure-IndexClean`, `Unstage-AllChanges`, `Try-ParseConditionCoverage`, `Normalize-CoberturaFilename`. PSScriptAnalyzer flags every one; a module export would warn on import. | `tools/.shared/*.ps1` |
| F-07 | S2 | **Four implementations of "find the repo root" under three names**: `Get-RepoRoot` (path.ps1, looks for `PineGuard.slnx`), `Resolve-RepoRoot` (git.ps1, `git rev-parse`), `Resolve-PineGuardRepoRoot` (audit-cli helpers, used by Qodana and Sonar runners), and an inline copy in `Test-StructuralIntegrity.ps1`. Two implementations of "ensure directory" (`Ensure-Directory`, `Ensure-PineGuardDirectory`). | grep in §1 |
| F-08 | S2 | **Parameter names for one concept vary per script.** "Open the result": `-NoOpen`, `-OpenReport`, `-OpenHtml`, `-Open`, `-ShowReport`. "Output folder": `-Output`, `-ResultsDir`, `-ResultsRoot`. "Token": `-ProjectToken`, `-Token`. "Preview": `-DryRun` (git, release) vs `-WhatIf` (maintenance, testing). `-Filter` means a test filter in coverage/testing and a warning-code regex in diagnostics. `-NoBuild` in `Run-Format` actually passes `--no-restore`. | all entry scripts |
| F-09 | S2 | **The 14-scope `ValidateSet` literal is copied into 7 scripts** in three different orderings (`'All','Testing'` vs `'Testing','All'` vs `'All'` first). Adding MediatR and Analyzers had to touch each copy. pwsh 7 supports `IValidateSetValuesGenerator`, which sources the set from the registry. | `Run-CodeCoverage`, `Gen-CoverageReport`, `Test-CoverageAnalysis`, `Run-CompilerDiagnostics`, `Run-Format`, `Run-Qodana`, `auto/Run-Coverage` |
| F-10 | S2 | **Aggregator shims exist in two domains only.** `Import-CodeCoverageUtility.ps1` and `Import-GitHelpers.ps1` dot-source `.shared`; diagnostics, format, Qodana, Sonar, maintenance, release dot-source `.shared` directly. "Utility" (singular) vs "Helpers" (plural). Neither pattern is the rule. | `tools/code-coverage/`, `tools/git/` |
| F-11 | S2 | **`xplat/` is named after a collector's friendly name, not the tool.** "XPlat Code Coverage" is the `friendlyName` that the `coverlet.collector` package registers with `dotnet test --collect`; the product is **Coverlet** (`coverlet.collector` 10.0.1 in `Directory.Packages.props`). The folder should be `coverlet/`. The second engine it was meant to sit beside, JetBrains dotCover, has no wrapper in the repo at all (the README says "deliberately no `dotcover/` folder"; `docs/ai/memory/coverage-analyst.md` says "the wrapper is what needs restoring"; git history never contained one, the tree was squashed at `94d25fb`). The owner wants dotCover reintroduced as a first-class engine, so the two-engine layout is restored under correct names (§3.5). A stale local `.dotnet/dotcover/2024.3.9` copy also lingers; session memory records it as unusable. | `tools/code-coverage/README.md`, `Directory.Packages.props`, `.dotnet/dotcover/` |
| F-12 | S2 | **`code-inspection/auto/` holds a coverage wrapper and a test wrapper**, neither of which is code inspection. `Run-Last.ps1` says "edit the parameters below" (a scratch file). Their stated reason, an agent allow-list wildcard, is obsolete: `.claude/settings.json` already allows `Bash(pwsh:*)`. | `tools/code-inspection/auto/`, `.claude/settings.json:47` |
| F-13 | S2 | **Qodana slug naming is inconsistent**: `data-annotations`, `fluent-validation`, `guard-clauses`, `must-clauses`, `dependency-injection` (split) vs `fluentresults`, `erroror`, `oneof`, `aspnetcore` (unsplit). | `tools/.shared/dotnet-projects.ps1`, `tools/code-inspection/qodana/config/` |
| F-14 | S2 | **`Test-CoverageAnalysis` is a noun that does not describe the script** ("test the coverage analysis"). It is the coverage gate/report. | `tools/code-coverage/xplat/` |
| F-15 | S3 | **Spec file naming is itself inconsistent**: `docs/ai/specs/tools/code-diagnostics/spec.md` vs `docs/ai/specs/tools/code-inspection/qodana.md`. | `docs/ai/specs/tools/spec.md` §4 |

### 2.2 Safety and correctness (S1)

| ID | Sev | Finding | Evidence |
|---|---|---|---|
| F-16 | S1 | **Tier 0 violation.** `Ensure-IndexClean` runs `git restore --staged .` whenever anything is staged, then continues. `docs/ai/specs/safety.md` §2.1 lists `git restore --staged .` as NEVER. The README claims the opposite ("Scripts refuse to run if you already have staged changes"). Every `/commit-*` slash command reaches this code. | `tools/.shared/git.ps1:88-115`, `tools/git/README.md:89` |
| F-17 | S1 | **`Run-CompilerDiagnostics` cannot see warnings and ignores build failure.** `Directory.Build.props` sets `TreatWarningsAsErrors=true`, so warnings surface as `error CS…` lines that the `warning CS` regex never matches; `$LASTEXITCODE` from `dotnet build` is never checked. A broken build prints "No compiler warnings found" and exits 0. Multi-TFM builds also emit each warning three times with no dedupe. | `tools/code-diagnostics/Run-CompilerDiagnostics.ps1:91-111` |
| F-18 | S1 | **`Test-CoverageAnalysis` tab output is broken.** `'\t'` and `"\t"` are literal backslash-t in PowerShell (the escape is `` `t ``). Header and rows print `Line%\tBranch%…` verbatim. `-Isolated` copies to `$env:TEMP` (Windows-only) and never deletes. | `tools/code-coverage/xplat/Test-CoverageAnalysis.ps1:180-186,128-140` |
| F-19 | S1 | **`Gen-CoverageReport -Clean` wipes every scope's results**, then `Test-CoverageAnalysis` aggregates whatever is left under `testresults/**` regardless of scope. This is the "stale data problem" already recorded in session memory. The scope-content validation is dead code (`if ($false)`), so the "missing expected scope" retry message is misleading. | `Gen-CoverageReport.ps1:142-149,121-133`; `Test-CoverageAnalysis.ps1:126` |
| F-20 | S1 | **Registry points at Qodana config and solution files that do not exist.** `QodanaConfig = 'tools/code-inspection/qodana/config/qodana.mediatr.yaml'` has no file and no `PineGuard.MediatR.Qodana.slnx`; `Run-Qodana -Scope MediatR` throws. `PineGuard.All.Qodana.slnx` (used by CI's Qodana job) omits MediatR and both Analyzers projects, so the "All" scan is not all. | `dotnet-projects.ps1:224`, `tools/code-inspection/qodana/` |
| F-21 | S1 | **Scope drift in five hand-maintained lists.** No `Commit-MediatR.ps1` / `Commit-Analyzers.ps1`; `Run-Commits` has no `-MediatR`/`-Analyzers` switch; `Commit-Agent.ps1` omits `src/PineGuard.Analyzers/AGENTS.md`, `src/PineGuard.MediatR/AGENTS.md`, `tests/PineGuard.Testing/AGENTS.md`; `Run-NugetUnlist` and `Run-GithubRelease` package lists omit `PineGuard.MediatR` and `PineGuard.Analyzers` (both packable) and still say "six packages". | `tools/git/`, `tools/release/` |
| F-22 | S1 | **The Qodana compose stack is a stray parallel path.** `docker-compose.qodana.yml` runs a full-repo scan with the `all` config as its `command` whenever `docker-up.ps1`/`qodana-up.ps1` runs, writing to `artifacts/qodana/` root. `Run-Qodana.ps1` never uses it: it invokes the Qodana CLI with `--within-docker`, which spins its own container. "Start the infrastructure" therefore starts an unrequested background scan. | `tools/docker/docker-compose.qodana.yml:26`, `Run-Qodana.ps1:288-305` |
| F-23 | S1 | **SonarQube is exposed on all interfaces with a documented password.** Compose binds `"9001:9000"` (0.0.0.0); `Setup-SonarQube` sets admin password `Scanner-1234` by default and the value is printed in the README and `.env.example`. Anyone on the LAN can administer it. | `docker-compose.sonarqube.yml:18`, `Setup-SonarQube.ps1:46` |
| F-24 | S1 | **Secrets on command lines and in the registry.** Sonar token passed as `/d:sonar.token=` (twice), Qodana token as `-e QODANA_TOKEN=` (redundant, the CLI already reads the env var), NuGet key as `--api-key`. `Setup-SonarQube` persists the token as a **User-level environment variable**, a machine-wide side effect outside the repo, while release tooling uses the gitignored `.etc/powershell/.env`. Three secret mechanisms coexist. | `Run-SonarScanner.ps1:148,169`, `Run-Qodana.ps1:291-294`, `Run-NugetUnlist.ps1:162`, `Setup-SonarQube.ps1:222` |
| F-25 | S1 | **`Sync-Env` clobbers the current session.** It merges Machine+User registry values into the process, overwriting any `$env:` value set in the current shell (a session-scoped `SONARQUBE_TOKEN` loses to the persisted one). It is Windows-only. The scan spec §4 says `.env` is "loaded by Sync-Env", which is false: Sync-Env reads the registry, not `.env`. | `tools/.shared/env.ps1`, `docs/ai/specs/scan/spec.md` §4 |
| F-26 | S1 | **`Run-SonarScanner` renames `sonar-project.properties` to `.bak` in the repo root during the scan.** A killed process leaves the root without its properties file. This also breaks the README rule "scripts NEVER create files in the project root". `-Framework net8.0` is hard-coded with no parameter or documented reason. | `Run-SonarScanner.ps1:132-137,161` |
| F-27 | S1 | **`Clean-Root -Recursive` can delete across the whole repository including `.git/`.** `Get-ChildItem -Recurse -Force` from the repo root with `txt`/`log` extensions. The file's own comments admit it was left unfinished ("Given the request…", "We will trust ShouldProcess"). `Clean-Artifacts` defaults to non-recursive, so `Run-Clean -Artifacts` deletes only the top-level redirect HTML and leaves every report folder. | `tools/maintenance/Clean-Root.ps1:33-72`, `Clean-Artifacts.ps1` |
| F-28 | S1 | **`$args` automatic variable is assigned and shadowed** in `Get-StatusPorcelain`, `Add-Paths`, `Invoke-Commit`, and `Invoke-Git`'s parameter is named `$Args`. Works today, flagged by PSScriptAnalyzer, and a latent trap. | `tools/.shared/git.ps1:49,127,163,435` |
| F-29 | S1 | **`Run-GithubRelease` throws under StrictMode when there is no upstream**: `(git rev-list … 2>$null) -as [string]` yields `$null`, then `.Trim()` runs before the `$null` guard. `$ghStatus` is assigned and unused in two scripts. | `Run-GithubRelease.ps1:166-167,123` |
| F-30 | S1 | **Windows path literals break on pwsh/Linux.** `Join-Path $PSScriptRoot '..\.shared\path.ps1'`, `"artifacts\code-diagnostics\$scopeSlug"`, `'xplat\Gen-CoverageReport.ps1'`, registry `SourceDir = 'src\PineGuard.Core'`, `reportgenerator.exe`, `$env:TEMP`. On Linux a backslash is a filename character, so the dot-source fails or a directory literally named `artifacts\code-diagnostics\all` is created. This is why the CI `audit` job runs on `windows-latest` (2× billing). The existing `cross-platform-tools-migration.md` plan's own Fable note says to audit for exactly this instead of rewriting in Bash. | `Import-CodeCoverageUtility.ps1`, `Run-CompilerDiagnostics.ps1:47-48,68`, `Run-Format.ps1:92-93`, `Import-GitHelpers.ps1:17`, `maintenance/*.ps1`, `Run-NugetUnlist.ps1:75-76`, `Run-CodeCoverage.ps1:113-114`, `dotnet-projects.ps1` |
| F-31 | S1 | **`Test-StructuralIntegrity` exits with the issue count** (wraps past 255 on Unix), its Sonar check looks for `resourceKey=` syntax the properties file does not use (always "nothing to check"), and it writes to `artifacts/audit/`, which is audit-cli's namespace. | `Test-StructuralIntegrity.ps1:257,309,58` |
| F-32 | S1 | **`Run-Tests -Output` and `auto/Run-Last`'s `dotnet clean` resolve against the current directory**, not the repo root, so results land wherever the shell happens to be. `-Async` discards the exit code. | `tools/testing/Run-Tests.ps1:73-80,85-88`, `auto/Run-Last.ps1:27` |

### 2.3 DRY and structure (S2)

| ID | Sev | Finding | Evidence |
|---|---|---|---|
| F-33 | S2 | **Sixteen `Commit-*.ps1` files differ only in a title string and one or two paths** (diff-verified). `Run-Commits` spawns a new `pwsh` process per scope (16 spawns, each re-loading `git.ps1`) and cannot forward `-Message`. `Get-CommitTitleSuggestion` knows seven hard-coded path fragments, all Core-era. `New-AutoCommitMessage` emits "Core: updates / Summary: Small scoped update (Files: 2 …)", which contradicts the owner's standing conventional-commits + prose-body convention. | `tools/git/*.ps1`, `git.ps1:224-329` |
| F-34 | S2 | **`README.md` is staged by both `Commit-Docs` and `Commit-Solution`**; whichever runs first takes it. `Commit-Solution` omits `Directory.Build.props`, `tests/Directory.Build.props`, `.gitattributes`. `Commit-Agent` includes `.github/workflows` (CI, needs a workflow-scoped token to push) and `.vscode/*`. | `tools/git/Commit-Docs.ps1`, `Commit-Solution.ps1`, `Commit-Agent.ps1` |
| F-35 | S2 | **Two sources of truth for coverlet settings.** `tools/code-coverage/coverlet.runsettings` (used by CI, `<Include>[PineGuard.*]*`) and the XML heredoc in `Write-CoverletRunSettings` (used locally, exact per-scope includes). The README calls the static file "the template" but the generator never reads it. | `coverlet.runsettings`, `coverage.ps1:55-98`, `.github/workflows/ci.yml:324` |
| F-36 | S2 | **Fourteen Qodana YAMLs are identical except for one path; fourteen `.slnx` files duplicate `PineGuard.slnx` membership by hand.** The registry already knows every project. | `tools/code-inspection/qodana/` |
| F-37 | S2 | **Console helpers defined three times** (`Write-Step/Info/Ok/Warn/Fail` in each release script, with different parameter names), plus `Write-Check/Pass/Fail/Info` in the integrity script, plus raw `Write-Host` colour conventions elsewhere. "Docker not found" guard is copied 8 times; "apply sonar defaults" block 3 times. | `tools/release/*.ps1`, `tools/docker/*.ps1`, `tools/sonar-scanner/*.ps1` |
| F-38 | S2 | **Three `Clean-*` scripts are 90% identical**; `Run-Clean` forwards `-All` to a child where it is documented as a no-op. Stale `Cleanup-` names remain in every help block. | `tools/maintenance/` |
| F-39 | S2 | **`html.ps1` redirect pages are over-engineered and half dead.** `Write-OptionalRedirectHtml` has no callers. The auto-redirect's `fetch(HEAD)` cannot work on `file://` and always falls to the "not generated" branch; the meta-refresh masks it. Three files to open one report. | `tools/.shared/html.ps1`, `Gen-CoverageReport.ps1:319-320` |
| F-40 | S2 | **Cross-domain coupling to audit-cli.** `Run-Qodana.ps1` and `Run-SonarScanner.ps1` dot-source `../audit-cli/helpers/Load-AuditHelpers.ps1` for two trivial helpers. Spec §2.1 says cross-domain code lives in `.shared/`. The owner has declared audit-cli non-authoritative and it is under separate review right now. | `Run-Qodana.ps1:86`, `Run-SonarScanner.ps1:49` |
| F-41 | S2 | **No local tool manifest.** ReportGenerator is installed unpinned into `.dotnet/tools` by a bespoke helper; CI pins `5.4.*`; `dotnet-sonarscanner` is installed globally by `sonarqube-up -InstallScanner`. The .NET standard is `.config/dotnet-tools.json` + `dotnet tool restore`. | `dotnet-tools-reportgenerator.ps1`, `ci.yml:358`, `sonarqube-up.ps1:102-113` |
| F-42 | S2 | **`Run-Tests` has no `-Scope`.** It is the most-referenced script (14 doc references, 12 VS Code tasks) and the only dotnet wrapper that does not use the registry; every VS Code task hand-copies a csproj path. | `tools/testing/Run-Tests.ps1`, `.vscode/tasks.json` |
| F-43 | S2 | **Nothing writes to `logs/`.** The README promises output to `artifacts/` or `logs/`; `Clean-Logs` cleans a directory nobody populates. No orchestrator records a transcript, so agents cannot read back a run. | `tools/README.md:52`, `tools/maintenance/Clean-Logs.ps1` |
| F-44 | S2 | **Exit-code convention is ad hoc**: `throw`, `exit 1`, `Write-Error; exit 1`, `Fail()`, exit = issue count, exit 2 for missing token, 124 for timeout. | all entry scripts |
| F-45 | S2 | **`.env.example` and `Run-GithubRuleset` still reference the legacy `.etc/powershell/` scripts**, and `.etc/powershell/.shared/index.ps1` hard-codes an absolute path to a *different repository* (`…\Onboarding\`). Adjacent scope; noted for the owner. | `.etc/powershell/.shared/index.ps1:14`, `Run-GithubRuleset.ps1:82` |

### 2.4 Hygiene and documentation (S3)

| ID | Sev | Finding | Evidence |
|---|---|---|---|
| F-46 | S3 | **21 scripts carry the placeholder help text "See the param block for details"**; 15 have a `.SYNOPSIS` that is just the filename with the hyphen removed; 25 entry scripts have no `.EXAMPLE`. | inventory in §1 |
| F-47 | S3 | **Mixed encodings and headers.** 15 files have a UTF-8 BOM, 44 do not; 10 lack `Set-StrictMode`; 8 lack `$ErrorActionPreference = 'Stop'`; 7 contain trailing whitespace; `auto/*` use tabs in help. `.editorconfig` sets only `indent_size` for `.ps1`. | inventory in §1 |
| F-48 | S3 | **No lint or tests for the tooling.** No `PSScriptAnalyzerSettings.psd1`, no Pester, no CI job for `tools/**`. The C# side is gated at 100% coverage; the scripts that produce that number have zero automated checks. | repo root, `ci.yml` |
| F-49 | S3 | **Scope lists in docs are stale in at least ten places.** `tools/README.md` (4 rows), `code-formatter/README.md`, `code-inspection/README.md`, `code-diagnostics/spec.md` §4 still list the original seven scopes; `git/README.md` tree and table omit five commit scripts; `code-coverage/README.md` output paths (`xplat/html`, `xplat-report.html`) do not match what the script writes (`html-<scope>/`, `xplat-<scope>-report.html`). | READMEs |
| F-50 | S3 | **README claims contradicted by code**: "Standard Parameters" lists three; "-DryRun: git" (release has it too); "scripts NEVER create files in the project root" (F-26, F-32). | `tools/README.md` |
| F-51 | S3 | **Qodana containers mount the whole repo, including the gitignored `.env`**; no `exclude:` for `.etc/`, `artifacts/`, `.git/` in any Qodana config. SonarQube already excludes them. `sonarqube:community` image tag is unpinned; Qodana's is pinned. | `qodana/config/*.yaml`, `docker-compose.sonarqube.yml:15` |
| F-52 | S3 | **`tools/*/AGENTS.md` exist for three domains only**, matching the three `docs/ai/rules/` files (`tools.md`, `roslyn.md`, `scan.md`). Acceptable, but the rule-file names are part of the vocabulary problem in F-01. | `tools/**/AGENTS.md` |

---

## 3. Target conventions (proposal for the Phase 0 decision session)

Every name below is a **recommendation with alternatives**, per the owner's naming standard
(on-the-tin, unambiguous, precedent-checked, rejected alternatives shown, owner signs off).
Nothing in §3 is applied until D-1 is signed.

### 3.1 Vocabulary rule

**One name per thing, used identically in the folder, the orchestrator noun, the slash-command
family, the rules file, the spec folder, and the artifacts folder.**

| Domain (what it drives) | Folder | Orchestrator | Command family | Rules file | Artifacts |
|---|---|---|---|---|---|
| dotnet test | `testing/` | `Run-Tests.ps1` (sanctioned plural outlier) | `/test-*` | `testing.md` | `artifacts/testing/` |
| Coverlet and dotCover, reported by ReportGenerator | `code-coverage/` with engine subfolders `coverlet/` and `dotcover/` | `Run-CodeCoverage.ps1 -Engine Coverlet\|DotCover` | `/coverage-*` | `tools.md` | `artifacts/code-coverage/<engine>/<scope>/` |
| Roslyn compiler diagnostics | `code-diagnostics/` | `Run-CodeDiagnostics.ps1` | `/scan-roslyn-*` → **`/diagnostics-*`** (or keep and rename folder `roslyn/`; see D-1c) | `roslyn.md` → `code-diagnostics.md` | `artifacts/code-diagnostics/` |
| dotnet format | `code-format/` (from `code-formatter`) | `Run-CodeFormat.ps1` (from `Run-Format`) | `/format-*` | `tools.md` | none |
| JetBrains Qodana | `qodana/` (from `code-inspection`) | `Run-Qodana.ps1` | `/scan-qodana-*` | `qodana.md` | `artifacts/qodana/` |
| SonarQube | `sonarqube/` (from `sonar-scanner`) | `Run-SonarQube.ps1` (from `Run-SonarScanner`) | `/scan-sonar-*` → `/scan-sonarqube-*` | `scan.md` → `sonarqube.md` | `artifacts/sonarqube/` |
| git | `git/` | `Run-Commits.ps1` (sanctioned plural outlier) | `/commit-*` | `tools.md` | `artifacts/git/` |
| GitHub + nuget.org release | `release/` | `Run-Release.ps1` (from `Run-GithubRelease`) | `/github-release-*`, `/nuget-*` | `tools.md` | `artifacts/release/` |
| housekeeping | `maintenance/` | `Run-Clean.ps1` | `/clean-*` | `tools.md` | `artifacts/maintenance/` |
| Docker | *removed* (see D-3) | | | | |

**Principle:** activities that are generic dotnet workflows get activity names (`code-*`,
`testing`); third-party analysers get the product name because the activity ("static analysis")
does not distinguish Qodana from SonarQube. Rejected: all-product naming (`coverlet/`,
`roslyn/`) hides what the tool is *for*; all-activity naming (`code-inspection/`, `code-scan/`)
made two folders that mean the same thing.

### 3.2 Verb rule (approved PowerShell verbs, with two sanctioned outliers)

| Verb | Meaning here | Sanctioned outlier? |
|---|---|---|
| `Run-` | The one orchestrator per domain. Never for a single action. | Yes, deliberately (not an approved verb; reads as "the runner"). |
| `Commit-` | *removed*; commit scripts collapse into `Run-Commits -Scope`. | — |
| `New-` | Generators (`New-CoverageReport`). Replaces `Gen-`. | — |
| `Test-` | Pass/fail gates (`Test-Coverage`, `Test-StructuralIntegrity`). | — |
| `Get-` | Read-only queries (`Get-SonarQubeIssues`). | — |
| `Set-` | One-shot state change (`Set-GithubRuleset -Enforcement Active`). | — |
| `Unpublish-` | nuget unlist (`Unpublish-NugetPrerelease`). Alt: `Remove-NugetListing`. | — |
| `Install-` | Get prerequisites onto the machine (`Install-Qodana`, `Install-SonarQube`). Replaces the *install* half of `Initialize-`. | — |
| `Initialize-` | First-run configuration of a running service (`Initialize-SonarQube`). Replaces `Setup-`. | — |
| `Start-` / `Stop-` | Container lifecycle (`Start-SonarQube`, `Stop-SonarQube`). Replaces `*-up`/`*-down`. | — |
| `Clear-` | Delete regenerable content (`Clear-Artifacts`). Replaces `Clean-`. Alt: keep `Clean-` as a third outlier; rejected because `Clear-` is approved and reads the same. | — |
| `Import-` | Loaders (only the module manifest remains). `Load-` removed from spec. | — |

Shared-function renames: `Ensure-Directory` → inline `New-Item -Force`; `Ensure-DockerNetwork` →
`Initialize-DockerNetwork`; `Ensure-IndexClean` → `Assert-IndexClean` (throws, never unstages);
`Unstage-AllChanges` → deleted; `Try-ParseConditionCoverage` → `ConvertFrom-ConditionCoverage`;
`Normalize-CoberturaFilename` → `Resolve-CoberturaFilename`; `Get-RepoRoot`/`Resolve-RepoRoot`/
`Resolve-PineGuardRepoRoot` → one `Get-RepoRoot`; `Test-CommandExists` → `Assert-Command`
(throws with an install hint) plus `Test-Command`; `Sync-Env`/`Sync-Path`/`Get-JavaVersion` →
deleted; `Ensure-ReportGenerator` → deleted (tool manifest).

### 3.3 Standard parameter names

| Parameter | Meaning | Replaces |
|---|---|---|
| `-Scope` | Registry scope, validated by `IValidateSetValuesGenerator`; `All` as aggregate | 7 literal copies |
| `-Engine` | `Coverlet` \| `DotCover`, default `Coverlet` | new; coverage only |
| `-Configuration` | `Debug` \| `Release` | — |
| `-Clean` | Delete this scope's previous output first | — |
| `-WhatIf` | Preview via `SupportsShouldProcess` everywhere | `-DryRun` (rejected: non-native; two spellings of one idea) |
| `-Open` | Open the result in the browser; default off (CI-safe) | `-NoOpen`, `-OpenReport`, `-OpenHtml`, `-ShowReport` |
| `-OutputPath` | Override the artifacts folder | `-Output`, `-ResultsDir`, `-ResultsRoot` |
| `-Token` | Explicit secret; resolution order param → `$env:` → `.etc/powershell/.env` | `-ProjectToken` |
| `-Filter` | `dotnet test --filter` expression only | diagnostics regex becomes `-Code` |
| `-NoRestore` | `--no-restore` | `-NoBuild` in `Run-Format` |
| `-Framework` | TFM passthrough | — |
| `-Force` | Skip confirmations / pre-flight | — |
| `-Timeout` | Seconds (one unit everywhere) | `-TimeoutMinutes` |

### 3.4 Structural rules

- **`tools/.shared/` becomes a script module** `PineGuard.Tools` (`PineGuard.Tools.psd1` +
  `PineGuard.Tools.psm1` that dot-sources `functions/*.ps1`). Every entry script starts with one
  `Import-Module (Join-Path $PSScriptRoot '../.shared/PineGuard.Tools.psd1') -Force`. Removes
  load-order comments ("requires path.ps1 loaded first"), the two aggregator shims, and the
  `$script:` constant fragility. Alt (rejected): keep dot-sourcing and add an ordering test.
- **The registry is the only place a scope is spelled out.** Consumers: ValidateSet generator,
  `Run-Tests -Scope`, commit scopes, release package list, Qodana per-scope config (generated at
  run time into `artifacts/qodana/<slug>/qodana.yaml`, mirroring how coverage generates
  runsettings), `PineGuard.All.Qodana.slnx` (generated, or replaced by `PineGuard.slnx`; D-3).
- **One Pester parity test** asserts registry ↔ `src/*.csproj` ↔ `tests/*.csproj` ↔
  `PineGuard.slnx` ↔ packable list ↔ Qodana config presence ↔ per-project `AGENTS.md`.
- **Secrets**: one resolver `Get-ToolSecret -Name X` (param → `$env:X` → `.env`). Never written
  to the registry, never echoed, passed via environment (`SONAR_TOKEN`, `QODANA_TOKEN`) where the
  tool supports it. SonarQube bound to `127.0.0.1`. Admin password generated and stored in `.env`.
- **Output**: every orchestrator writes under `artifacts/<domain>/` and records a transcript under
  `logs/<domain>/<yyyyMMdd-HHmmss>.log`. Nothing touches the repo root.
- **Exit codes**: `0` success, `1` failure, `2` usage/prerequisite, `3` quality gate not met,
  `124` timeout.
- **Encoding**: UTF-8 without BOM, CRLF-normalised by `.gitattributes`, `.editorconfig` gains
  `charset = utf-8` and `trim_trailing_whitespace = true` for `*.ps1`. StrictMode + `Stop` in
  every script via a fixed header template.
- **Cross-platform**: no `\` in path literals, no `.exe`, no `$env:TEMP`, no registry access.
  A Pester test greps for these. Enables moving the CI `audit` job to `ubuntu-latest`.
- **Help**: every `.PARAMETER` has real text; every entry script has ≥ 1 `.EXAMPLE`; a Pester
  test fails on the placeholder string.

### 3.5 Coverage engine layout (two engines, one contract)

The engine folder is named after the tool, never after a collector's display string.
`coverlet.collector` registers the data collector `friendlyName` "XPlat Code Coverage"; that is
where `xplat` came from. The tool is Coverlet. The second engine is JetBrains dotCover, driven by
the `jetbrains.dotcover.commandlinetools` global tool (`dotCover.exe`, 2025.3.3 installed).

| Layer | Coverlet | dotCover | Shared |
|---|---|---|---|
| Collect | `dotnet test --collect:"XPlat Code Coverage" --settings <generated runsettings>` | `dotCover cover --xml-report-output <out>.xml --snapshot-output <out>.dcvr --exclude-assemblies *.UnitTests --exclude-attributes <same set as coverlet> -- test <csproj> -c <cfg> -f <tfm>` | `New-CoverageReport.ps1` in each engine folder: same parameters, same output layout |
| Raw output | `artifacts/code-coverage/coverlet/<scope>/testresults/**/coverage.cobertura.xml` | `artifacts/code-coverage/dotcover/<scope>/snapshots/<project>.<tfm>.dcvr` and `.dotcover.xml` | |
| Report | ReportGenerator, input format `Cobertura` | ReportGenerator, input format `DotCover` | `-reporttypes:Html;HtmlSummary;Cobertura` into `artifacts/code-coverage/<engine>/<scope>/report/` |
| Gate | reads the Cobertura it collected | reads `report/Cobertura.xml` emitted by ReportGenerator | one `Test-Coverage.ps1 -Engine`, engine-agnostic Cobertura parser |

Rules:

- **Coverlet is authoritative.** CI and the 100% gate use it. dotCover is the local second
  opinion and the Rider-native format; a scope must pass on Coverlet, and dotCover disagreement
  is reported, not gated, until T3.10 shows the two agree.
- **One-step `cover --xml-report-output`.** The 2025.3 CLI has only `cover`, `merge`, `report`,
  and no HTML report type; HTML comes from ReportGenerator. The separate `report` command hangs on
  2025.3.3 (session memory) and 2024.3.9 cannot read 2025.3.3 snapshots, so `report` is never
  called. Whether `cover` with `--xml-report-output` avoids the hang is **unverified** and is the
  first question of spike T3.10.
- **Same script name in both engine folders.** `coverlet/New-CoverageReport.ps1` and
  `dotcover/New-CoverageReport.ps1` share one parameter contract; the orchestrator routes on
  `-Engine`. Alternative rejected: one script with `-Engine` branches, because the two collection
  paths share almost no code and would become one file full of `if ($Engine)`.
- **Exclusions are declared once** in the registry (`CoverageExcludeAttributes`,
  `CoverageExcludeAssemblies`) and rendered into coverlet runsettings and dotCover arguments.

---

## 4. Phases

### Phase 0 — Decisions (Fable, one session, owner sign-off)

| ID | Decision | Recommendation | Alternatives to present |
|---|---|---|---|
| D-1a | Folder and orchestrator taxonomy (§3.1) | Adopt §3.1 | All-activity; all-product; leave as is and only document outliers |
| D-1b | Verb rule and outlier register (§3.2) | `Run-` and plural `Run-Tests`/`Run-Commits` are the only outliers | Keep `Gen-`, `Clean-`, `Setup-` as extra outliers |
| D-1c | Command-family vocabulary (`/scan-roslyn` vs folder) | Rename folder or commands so both agree; recommend commands follow folders | Keep both, document mapping |
| D-1d | Standard parameter table (§3.3), `-WhatIf` vs `-DryRun` | `-WhatIf` everywhere | `-DryRun` everywhere |
| D-2 | `.shared` → module | Module | Dot-source + ordering test |
| D-3 | Remove Qodana compose stack and therefore the `docker/` folder; per-scope Qodana config generated at run time; investigate `dotnet.project:` to drop the 14 `.slnx` | Remove; generate | Keep compose; keep hand-written `.slnx` |
| D-4 | Secrets policy (§3.4) | `.env` + `Get-ToolSecret`; loopback bind; generated password | Keep User env var |
| D-5 | Cross-platform stance | Supersede `cross-platform-tools-migration.md` with "pwsh everywhere, Windows-isms removed"; move CI `audit` job to Ubuntu | Proceed with Bash rewrite |
| D-6 | Commit tooling redesign | Table-driven scopes, `Run-Commits -Scope`, conventional-commit auto message, `Assert-IndexClean` | Keep 16 scripts, patch only |
| D-7 | Where tool tests live | `tools/.tests/` (dot-prefix marks internal, like `.shared`) | `tests/PineGuard.Tools.Tests/` |
| D-8 | Lift the `docs/` edit freeze for this plan | Yes; New Surfaces Program is complete (`d94b85b`) | Defer Phase 5 |
| D-9 | Coverage engine layout (§3.5): `coverlet/` and `dotcover/` subfolders, same-named collector script, Coverlet authoritative | Adopt §3.5 | `-Engine` branches in one script; dotCover authoritative; flatten to Coverlet only |

Output: a `## Decisions` section appended to this file with each answer and the rejected
alternatives. Phase 3 onward is blocked on D-1 through D-7; Phases 1 and 2 are not.

### Phase 1 — Safety and correctness hotfixes (no rename dependency)

Each task edits the files named, adds or updates a Pester test where one exists by then, runs
PSScriptAnalyzer on the touched files, smoke-runs the script, and commits with a conventional
message. Tasks in group **P1-A** touch disjoint files and run in parallel; **P1-B** serialises on
`git.ps1`.

| Task | Fixes | Files |
|---|---|---|
| T1.01 | F-16, F-28: replace `Ensure-IndexClean`/`Unstage-AllChanges` with `Assert-IndexClean` that throws; rename `$Args`/`$args` | `tools/.shared/git.ps1`, `tools/git/README.md` |
| T1.02 | F-17: build with `-p:TreatWarningsAsErrors=false`, check `$LASTEXITCODE`, parse `warning` and `error`, dedupe on (file, line, col, code), keep TFM as a field | `tools/code-diagnostics/Run-CompilerDiagnostics.ps1` |
| T1.03 | F-18: `` `t `` escapes; `[IO.Path]::GetTempPath()`; `try/finally` cleanup of the isolated copy | `tools/code-coverage/xplat/Test-CoverageAnalysis.ps1` |
| T1.04 | F-19: `-Clean` deletes only `testresults/<this scope's projects>` and `html-<scope>`; analysis reads only the results folders of the scope's `TestCsproj` (All = every registry TestCsproj); delete the `if ($false)` block and the unused `$expected` | `Gen-CoverageReport.ps1`, `Test-CoverageAnalysis.ps1`, `coverage.ps1` |
| T1.05 | F-20: add `qodana.mediatr.yaml` + `PineGuard.MediatR.Qodana.slnx`; add MediatR and both Analyzers projects to `PineGuard.All.Qodana.slnx` (interim until D-3 generates them) | `tools/code-inspection/qodana/**` |
| T1.06 | F-21 (data only): add MediatR and Analyzers to both release package lists; add the three missing `AGENTS.md` paths to `Commit-Agent.ps1`; fix "six packages" text | `tools/release/*.ps1`, `tools/git/Commit-Agent.ps1`, `tools/release/README.md` |
| T1.07 | F-23 (bind), F-51 (pin): `127.0.0.1:9001:9000`; pin `sonarqube` image to a specific community tag; add `healthcheck` | `tools/docker/docker-compose.sonarqube.yml` |
| T1.08 | F-29: guard the upstream count before `.Trim()`; drop unused `$ghStatus` | `tools/release/Run-GithubRelease.ps1`, `Run-GithubRuleset.ps1` |
| T1.09 | F-27: remove `-Recursive` from `Clean-Root`; hard exclusion list; `Clean-Artifacts` recursive by default | `tools/maintenance/Clean-Root.ps1`, `Clean-Artifacts.ps1`, `Run-Clean.ps1` |
| T1.10 | F-26 (root file): move `sonar-project.properties` to `tools/sonar-scanner/sonar-project.properties` so nothing is renamed at run time; update `Test-StructuralIntegrity` Sonar path and the properties file's own header comment | `tools/sonar-scanner/Run-SonarScanner.ps1`, `sonar-project.properties`, `tools/maintenance/Test-StructuralIntegrity.ps1` |
| T1.11 | F-31: exit `0/1`; write to `artifacts/maintenance/`; replace the dead `resourceKey` check with "every `sonar.*.exclusions` path exists" | `tools/maintenance/Test-StructuralIntegrity.ps1` |
| T1.12 | F-32: resolve `-Output` against repo root; return the process exit code from `-Async` (or remove `-Async`, D-1d) | `tools/testing/Run-Tests.ps1` |
| T1.13 | F-24 (Qodana `-e`): stop passing the token on the command line; rely on `$env:QODANA_TOKEN` | `tools/code-inspection/Run-Qodana.ps1` |

Verification (Opus, T1.V): re-run the §1 inventory; every touched script executes with `-?`,
`-WhatIf`/no-op arguments; `Run-CodeCoverage -Scope Core` and `-Scope All` still report 100%;
`Run-CompilerDiagnostics -Scope All` now reports the exit code of a deliberately broken build.

### Phase 2 — Foundation: lint, tests, and parity gate

| Task | Deliverable |
|---|---|
| T2.01 | `PSScriptAnalyzerSettings.psd1` at repo root: `Severity = Error, Warning`; include `PSUseApprovedVerbs`, `PSAvoidAssignmentToAutomaticVariable`, `PSProvideCommentHelp`, `PSUseConsistentIndentation`, `PSUseConsistentWhitespace`, `PSAvoidTrailingWhitespace`, `PSUseBOMForUnicodeEncodedFile` **off** (we choose no BOM); exclude `PSAvoidUsingWriteHost` (spec §2.3 mandates coloured console output) |
| T2.02 | Record the PSScriptAnalyzer baseline (count by rule, per file) in `artifacts/tools-lint/baseline.json` and summarise it in this plan's `## Baselines` section |
| T2.03 | Pester scaffold under the D-7 location with tests for: registry parity (§3.4), dotenv parser, Cobertura parser against a fixture XML, `Get-RepoRoot`, git helpers against a temp repo (`Assert-IndexClean` throws when staged), help-placeholder absence, BOM absence, Windows-ism grep (`\\` inside `Join-Path`/dot-source literals, `.exe`, `$env:TEMP`, `SetEnvironmentVariable(...,'User')`) |
| T2.04 | `tools/testing/Test-Tools.ps1` (name subject to D-1: a `Test-` gate that runs PSSA + Pester and exits `3` on failure) |
| T2.05 | CI job `tools-lint` on `ubuntu-latest` running T2.04; initially `continue-on-error: true` until Phase 3 clears the baseline, then hard |
| T2.06 | `.editorconfig`: `[*.{ps1,psm1,psd1}] charset = utf-8`, `trim_trailing_whitespace = true`, `insert_final_newline = true`; strip BOMs and trailing whitespace repo-wide under `tools/` (mechanical) |
| T2.07 | `.config/dotnet-tools.json` with `dotnet-reportgenerator-globaltool` (pin to CI's `5.4.x`) and `dotnet-sonarscanner`; `dotnet tool restore` replaces `Ensure-ReportGenerator` and the `-InstallScanner` switch; CI uses the manifest |

### Phase 3 — Consolidation by domain (blocked on D-1…D-7; 3a first, then fan-out)

| Task | Scope | Fixes |
|---|---|---|
| T3.01 | **`.shared` → `PineGuard.Tools` module** (D-2). Functions split into `functions/<area>.ps1`: `path`, `registry` (with `IValidateSetValuesGenerator` class `PineGuardScope`), `console` (`Write-Step`, `Write-Success`, `Write-Warn`, `Write-Fail`, `Write-Detail`), `command` (`Assert-Command`, `Test-Command`), `secret` (`Get-ToolSecret`, `Import-DotEnv`), `git`, `coverage`, `sonarqube`, `docker` (or none, per D-3). Approved verbs per §3.2. Remove `Import-CodeCoverageUtility.ps1`, `Import-GitHelpers.ps1`, `env.ps1`, `html.ps1`, `dotnet-tools-reportgenerator.ps1`, `commands.ps1`. Remove the two audit-cli dot-sources. Transcript helper `Start-ToolTranscript -Domain X`. | F-06, F-07, F-10, F-25, F-37, F-39, F-40, F-41, F-43 |
| T3.02 | **git**: scope table derived from the registry plus meta-scopes (`Agent`, `Docs`, `Tools`, `Solution`, `Ci`); one `Run-Commits.ps1 -Scope <string[]> [-All] [-IncludeTests] [-Message] [-AutoMessage] [-Push] [-Rebase] [-WhatIf]` calling `Invoke-ScopedCommit` in-process; delete 16 `Commit-*.ps1`; `README.md` owned by one scope; `.github/workflows` moves to `Ci`; auto message = conventional-commit subject + prose body per owner convention; `Assert-IndexClean`. Slash commands `/commit-*` call `Run-Commits -Scope X`. | F-21, F-33, F-34 |
| T3.03 | **code-coverage (Coverlet)**: rename `xplat/` to `coverlet/`; `coverlet/New-CoverageReport.ps1`; engine-agnostic `Test-Coverage.ps1 -Engine` at the domain root; single coverlet source (generator reads `coverlet.runsettings` and patches `<Include>`; CI keeps using the static file); ReportGenerator emits `Html;HtmlSummary;Cobertura` into `artifacts/code-coverage/coverlet/<scope>/report/`; open `index.html` directly; per-scope clean and analysis from T1.04 retained; `-Engine`, `-Open`, `-OutputPath`, `-Framework`, `-Format` validated in the orchestrator. | F-03, F-11, F-14, F-35, F-39 |
| T3.04 | **qodana**: folder rename; remove `auto/`; generate `artifacts/qodana/<slug>/qodana.yaml` from the registry at run time; either generate `.slnx` per scope or use `dotnet.project:` (spike first, 1 task); `exclude:` `.etc`, `artifacts`, `.git`; `Install-Qodana.ps1`; slugs normalised (D-1). `qodana.all.yaml` stays checked in for CI but is itself generated by `Sync-QodanaConfig.ps1` (spec §1.1 `Sync-` verb) with a parity test that it is current. | F-12, F-13, F-20, F-36, F-51 |
| T3.05 | **sonarqube**: folder rename; `Install-SonarQube.ps1` (prereqs + start), `Initialize-SonarQube.ps1` (commission; password generated with `[System.Security.Cryptography.RandomNumberGenerator]`, stored to `.env` as `SONARQUBE_ADMIN_PASSWORD`, token stored to `.env`, never to User env), `Start-SonarQube.ps1`/`Stop-SonarQube.ps1` (absorb `sonarqube-up/down`, `docker-network`), `Run-SonarQube.ps1` (token via `SONAR_TOKEN` env; `-Framework` parameter with the net8.0 reason documented; properties from `tools/sonarqube/sonar-project.properties`), `Get-SonarQubeIssues.ps1`. Delete `tools/docker/` and `docker.ps1` per D-3. | F-04, F-05, F-22, F-23, F-24, F-25, F-26 |
| T3.06 | **testing / code-format / code-diagnostics**: `Run-Tests -Scope` from the registry (All = `PineGuard.slnx`), `-Framework`, `-NoRestore`, `-OutputPath`, trx named per project; `Run-CodeFormat` with `-NoRestore`; `Run-CodeDiagnostics` with `-Code` filter, cross-platform output path, transcript; all three emit the §3.4 exit codes. Regenerate `.vscode/tasks.json` test entries from the registry (small `Sync-VsCodeTasks.ps1`, optional). | F-08, F-17, F-30, F-42, F-44 |
| T3.07 | **maintenance**: `Clear-Artifacts.ps1`, `Clear-Logs.ps1`, `Clear-Root.ps1` sharing one `Clear-ToolDirectory` function; `Run-Clean -Target Artifacts,Logs,Root`; `Test-StructuralIntegrity` reads the registry for the namespace check, single-pass file reads, `artifacts/maintenance/`. | F-27, F-31, F-38 |
| T3.08 | **release**: `Run-Release.ps1`, `Set-GithubRuleset.ps1`, `Unpublish-NugetPrerelease.ps1`; console helpers from the module; package list from the registry (`SourceCsprojs` where the csproj is packable); backup path via `Get-RepoRoot`; `-WhatIf`. | F-02, F-21, F-29, F-37 |
| T3.09 | **Windows-ism sweep**: apply the T2.03 grep test across all of `tools/` (excluding audit-cli) and fix every hit; move the CI `audit` job to `ubuntu-latest` if audit-cli's owner confirms it is clean, else leave a TODO in the workflow. | F-30 |
| T3.10 | **dotCover spike** (time-boxed, blocks T3.11). With `jetbrains.dotcover.commandlinetools` 2025.3.3 establish: (a) `dotCover cover --xml-report-output … -- test <Core csproj> -c Release -f net10.0` and `-f net8.0` complete without the `report` hang; (b) ReportGenerator accepts the XML as `DotCover` input and its Cobertura output matches Coverlet's Core line and branch totals within a documented tolerance; (c) which `--exclude-attributes` values reproduce coverlet's `ExcludeByAttribute` set, including the `GeneratedRegex` output; (d) whether per-TFM runs need `dotCover merge` or ReportGenerator merging. Record every answer in `## Baselines`. Delete `.dotnet/dotcover/2024.3.9`. Add the tool to `.config/dotnet-tools.json` if it installs as a local tool. | F-11 |
| T3.11 | **code-coverage (dotCover)**: `dotcover/New-CoverageReport.ps1` with the same parameter contract as the Coverlet script; snapshots and XML under `artifacts/code-coverage/dotcover/<scope>/`; ReportGenerator `Html;HtmlSummary;Cobertura`; `Run-CodeCoverage -Engine DotCover` routes to it; `Test-Coverage -Engine DotCover` gates on the emitted Cobertura; README engine table replaces the "deliberately no dotcover folder" note; Pester test that both engines produce the same class list for Core. If T3.10 (a) fails, ship snapshot-only collection for Rider and record the JetBrains issue. | F-11 |

### Phase 4 — Rename cascade (blocked on Phase 3 completion)

| Task | Deliverable |
|---|---|
| T4.01 | `git mv` every folder and script per D-1 in one commit; update intra-`tools/` references |
| T4.02 | Haiku fan-out, one agent per surface: `docs/ai/agents`, `docs/ai/commands`, `docs/ai/skills`, `docs/ai/specs`, `docs/ai/rules`, `docs/ai/workflows`, `docs/ai/README.md`, `.claude/`, `.agent/`, `.pi/`, `.github/` (prompts, instructions, skills, agents, workflows), `.clinerules/`, `.cursor/rules/`, `.windsurf/rules/`, `.amazonq/rules/`, `.junie/`, `.vscode/tasks.json`, `CLAUDE.md`/`AGENTS.md`/`GEMINI.md`. Each agent receives the exact old→new path table and replaces only those strings. |
| T4.03 | Gate: `Test-StructuralIntegrity -Scope Paths -StalePaths <every old path>` returns zero; a Haiku sweep confirms every `tools/**/*.ps1` path mentioned anywhere in the repo exists on disk. |

### Phase 5 — Documentation and spec (blocked on D-8)

| Task | Deliverable |
|---|---|
| T5.01 | `docs/ai/specs/tools/spec.md` v2: §1 verb table with the outlier register; §3.3 parameter table; exit codes; secrets rule; encoding rule; help rule; cross-platform rule; lint/test requirement; domain index with `spec.md` naming for every domain (rename `qodana.md` → `code-inspection/spec.md` or to the new folder name) |
| T5.02 | `docs/ai/specs/scan/spec.md` §4 corrected (no `Sync-Env`), §6 script table; `docs/ai/specs/tools/code-diagnostics/spec.md` §4 scope table replaced by "see registry" |
| T5.03 | Every `tools/**/README.md` regenerated: no hand-enumerated scope lists (one canonical table in `tools/README.md` produced by `Get-PineGuardScope -All`), correct output paths, correct parameter tables, the safety tier of every script |
| T5.04 | `docs/ai/plans/cross-platform-tools-migration.md` marked superseded per D-5 with a pointer here |
| T5.05 | `docs/ai/rules/tools.md` gains the header template and the "read the registry, never enumerate scopes" rule |

### Phase 6 — Independent verification (Opus)

| Task | Check |
|---|---|
| T6.01 | Every entry script: `-?` renders real help; `-WhatIf` previews without side effects; exit codes match §3.4 |
| T6.02 | `Run-CodeCoverage -Scope All` on net8.0 and net10.0 still 100%/100%; `Run-Tests -Scope All` green; `Run-CodeDiagnostics -Scope All` zero warnings; `Run-CodeFormat -Scope All -VerifyNoChanges` clean |
| T6.03 | PSScriptAnalyzer zero findings under the settings file; Pester green; `tools-lint` CI job hard-failing and green |
| T6.04 | Safety review: grep for every Tier 0 command in `docs/ai/specs/safety.md` §2.1 across `tools/`; none present |
| T6.05 | Secrets review: no token on any command line where an env alternative exists; nothing writes to User/Machine environment; SonarQube bound to loopback |
| T6.06 | Docs review: every path in every README and spec resolves; scope lists match the registry |

---

## 5. Orchestration rules (Sonnet 5 orchestrator)

1. **The orchestrator never reads file content and never edits.** It reads state only (`git
   status`, `git log`, file existence, test exit codes) and dispatches. Every content read belongs
   to a sub-agent whose context is disposable.
2. **Dispatch prompt template** (every task):
   - Files to touch (exact list) and files that must not be touched (`tools/audit-cli/**`, always).
   - Findings being fixed (`F-nn`) and the acceptance criteria copied from §4.
   - "Run `Invoke-ScriptAnalyzer -Settings PSScriptAnalyzerSettings.psd1` on every touched file
     and the Pester suite before each commit. Commit after each green step with a conventional
     subject and a prose body. Rebuild/re-run before trusting your own claim."
   - "Invoke this worktree's own copy of every `tools/` script by relative path."
   - "Report: files changed, commits made, tests run with output, anything left undone."
3. **After every dispatch** the orchestrator runs `git status`, `git diff --stat`, and the
   relevant smoke command itself before dispatching the next step (single-agent dispatches have
   been cut off mid-work before; uncommitted progress is normal, not a failure).
4. **Model routing** (per `docs/ai/plans/new-surfaces-orchestration.md` §4 and session memory):

   | Tier | Model | Tasks |
   |---|---|---|
   | Judgment (high) | **Fable** | D-1…D-8 only. One session; `/ask-council` for D-1 if the owner wants it pressure-tested. |
   | Judgment (light) | **Opus** | T1.V, T6.*, the D-3 "is the Qodana compose really unused" confirmation before deletion, review of the T5.01 spec text |
   | Implementation | **Sonnet** | Every T1, T2, T3 task; T4.01; T5.01–T5.05 authoring |
   | Bulk IO | **Haiku** | T2.02 baseline capture, T2.06 BOM/whitespace strip, T4.02 fan-out, T4.03 sweep, T5.03 README table regeneration from registry output |

5. **Worktree**: one feature branch `feature/tools-standardisation` with a worktree; Phase 1 may
   merge to `main` on its own (`--no-ff`, no PR, per owner convention) because it is pure hotfix;
   Phases 3–5 merge together after Phase 6.
6. **Coordination with the audit-cli review**: nothing here edits `tools/audit-cli/**`. T3.01
   removes our two dependencies on it. If the other agent renames `Load-AuditHelpers.ps1` before
   T3.01 lands, T1.05/T1.13 still work because they do not touch the dot-source line.

---

## 6. Risks

| Risk | Mitigation |
|---|---|
| Rename cascade misses an adapter surface (it did once before; see `docs/ai/meta/adapter-surfaces.md`) | T4.02 fans out from the §5 inventory of ten surfaces plus root boot files; T4.03 gate is mechanical |
| Slash-command names change (D-1c) and the owner's muscle memory breaks | Keep old command files as one-line pointers for one release; delete in a follow-up |
| Module conversion changes `$script:` semantics for constants | T2.03 tests cover every exported constant; module state uses `$script:` inside the psm1 |
| Removing `Sync-Env` breaks a user who relies on the persisted `SONARQUBE_TOKEN` | `Get-ToolSecret` still reads `$env:`; T3.05 migrates the value into `.env` on first run and prints what it did |
| Coverage numbers change after per-scope analysis (T1.04) | Expected direction is *more accurate*, not lower; T1.V compares old vs new for every scope and explains any delta |
| Qodana `dotnet.project:` spike fails | Fallback: generate `.slnx` files from the registry (still one source of truth) |
| `TreatWarningsAsErrors=false` build (T1.02) diverges from the real build | Diagnostics build goes to a separate `-o artifacts/code-diagnostics/build` and never touches `bin/` |
| dotCover `cover --xml-report-output` also hangs, or its numbers disagree with Coverlet | T3.10 is a gated spike; on failure T3.11 ships snapshot-only collection for Rider, records the JetBrains issue, and Coverlet remains the only gate |

Out of scope, flagged for the owner: `.etc/powershell/**` (hard-coded path to another repo,
duplicate `sonarqube.ps1`, legacy `github-*.ps1`); `.claude/settings.local.json` stale allow
entries for `.etc/powershell/github-rulesets.ps1`.

---

## 7. Playbook

Status values: `Pending`, `Blocked (D-x)`, `In progress`, `Done`, `Verified`. Fan-out group:
tasks sharing a group letter run concurrently; a group runs after the group it depends on.

| ID | Task | Agent / model | Status | Fan-out group | Depends on | Notes |
|---|---|---|---|---|---|---|
| D-1..9 | Decision session, append `## Decisions` | Owner + Sonnet 5 (no council) | Done | P0 | — | Signed off 2026-09-06. D-1a replaces §3.1 with the owner's two-level rule; D-1c, D-1d, D-2 reverse defaults; D-9 adds a front door to §3.5; see `## Decisions` follow-up before dispatching. |
| T1.01 | git.ps1 Tier 0 fix + `$args` rename | Sonnet | Pending | P1-B | — | Serialised on `git.ps1` |
| T1.02 | Diagnostics: exit code, errors, dedupe | Sonnet | Pending | P1-A | — | |
| T1.03 | Coverage analysis tab + temp cleanup | Sonnet | Pending | P1-A | — | |
| T1.04 | Per-scope clean and analysis; dead code | Sonnet | Pending | P1-A | T1.03 | Same files as T1.03; run after |
| T1.05 | Qodana MediatR/Analyzers config + slnx | Sonnet | Pending | P1-A | — | Interim; superseded by T3.04 |
| T1.06 | Release/commit list parity (data) | Sonnet | Pending | P1-A | — | |
| T1.07 | SonarQube loopback bind + pin + healthcheck | Sonnet | Pending | P1-A | — | |
| T1.08 | Release null-Trim + unused vars | Sonnet | Pending | P1-A | — | |
| T1.09 | Clean-Root safety, Clean-Artifacts default | Sonnet | Pending | P1-A | — | |
| T1.10 | Move `sonar-project.properties` into tools | Sonnet | Pending | P1-A | — | Also touches integrity script |
| T1.11 | Integrity exit code, artifacts path, Sonar check | Sonnet | Pending | P1-A | T1.10 | |
| T1.12 | Run-Tests output path, `-Async` exit code | Sonnet | Pending | P1-A | — | |
| T1.13 | Qodana: stop passing token on CLI | Sonnet | Pending | P1-A | — | |
| T1.V | Verify Phase 1 (inventory, smoke, coverage parity) | Opus | Pending | P1-V | all T1 | Merge Phase 1 to main after this |
| T2.01 | PSScriptAnalyzer settings file | Sonnet | Pending | P2-A | — | Parallel with Phase 1 |
| T2.02 | Capture lint baseline | Haiku | Pending | P2-A | T2.01 | Writes `## Baselines` here |
| T2.03 | Pester suite (parity, parsers, git, hygiene, Windows-isms) | Sonnet | Pending | P2-A | D-7 for location only; may start in `tools/.tests/` and move | |
| T2.04 | `Test-Tools.ps1` gate | Sonnet | Pending | P2-B | T2.01, T2.03 | |
| T2.05 | CI `tools-lint` job (soft, then hard) | Sonnet | Pending | P2-B | T2.04 | Ubuntu runner |
| T2.06 | `.editorconfig` + BOM/whitespace strip | Haiku | Pending | P2-A | — | Mechanical |
| T2.07 | `.config/dotnet-tools.json`; remove bespoke installers | Sonnet | Pending | P2-A | — | CI switches to `dotnet tool restore` |
| T3.01 | `.shared` → `PineGuard.Tools` module | Sonnet | Blocked (D-2) | P3-A | P1, P2, D-1, D-2 | Everything in P3-B depends on this |
| T3.02 | git consolidation | Sonnet | Blocked (D-6) | P3-B | T3.01, D-6 | Deletes 16 files |
| T3.03 | code-coverage flatten + single runsettings source | Sonnet | Blocked (D-1) | P3-B | T3.01 | |
| T3.04 | qodana: generated config, remove `auto/`, spike `dotnet.project:` | Sonnet | Blocked (D-3) | P3-B | T3.01, D-3 | Opus confirms compose unused first |
| T3.05 | sonarqube: Install/Initialize/Start/Stop/Run/Get; secrets; delete `docker/` | Sonnet | Blocked (D-3, D-4) | P3-B | T3.01, D-3, D-4 | |
| T3.06 | testing / code-format / code-diagnostics parity | Sonnet | Blocked (D-1) | P3-B | T3.01 | |
| T3.07 | maintenance consolidation | Sonnet | Blocked (D-1) | P3-B | T3.01 | |
| T3.08 | release renames + registry package list | Sonnet | Blocked (D-1) | P3-B | T3.01 | |
| T3.09 | Windows-ism sweep; CI audit job to Ubuntu | Sonnet | Blocked (D-5) | P3-C | P3-B | Coordinate with audit-cli owner for the CI job |
| T3.10 | dotCover CLI spike | Sonnet | Blocked (D-9) | P3-B | T3.01, D-9 | Time-boxed; findings to `## Baselines` |
| T3.11 | dotCover engine wrapper | Sonnet | Blocked (D-9) | P3-C | T3.03, T3.10 | Same contract as the Coverlet script |
| T3.V | Verify Phase 3 per domain (build, test, coverage, lint, Pester) | Opus | Pending | P3-V | P3-C | |
| T4.01 | `git mv` cascade inside `tools/` | Sonnet | Blocked (D-1) | P4-A | P3-V | One commit |
| T4.02 | Reference rewrite, one agent per surface (≈18) | Haiku ×18 | Blocked (D-1) | P4-B | T4.01 | Exact old→new table supplied |
| T4.03 | Stale-path gate + existence sweep | Haiku | Pending | P4-C | P4-B | Zero tolerance |
| T5.01 | Tools spec v2 | Sonnet (author), Opus (review) | Blocked (D-8) | P5-A | P4-C | |
| T5.02 | Scan + diagnostics spec corrections | Sonnet | Blocked (D-8) | P5-A | P4-C | |
| T5.03 | All `tools/**/README.md` regenerated | Sonnet + Haiku (tables) | Blocked (D-8) | P5-A | P4-C | No hand-enumerated scopes |
| T5.04 | Supersede cross-platform plan | Haiku | Blocked (D-5, D-8) | P5-A | — | |
| T5.05 | `rules/tools.md` header template + registry rule | Sonnet | Blocked (D-8) | P5-A | T5.01 | |
| T6.01–06 | Final verification matrix | Opus | Pending | P6 | P5-A | Then merge `--no-ff` to main |

### Estimated dispatch count

| Phase | Sonnet | Haiku | Opus | Fable |
|---|---|---|---|---|
| 0 | 0 | 0 | 0 | 1 |
| 1 | 13 | 0 | 1 | 0 |
| 2 | 5 | 2 | 0 | 0 |
| 3 | 11 | 0 | 2 | 0 |
| 4 | 1 | 19 | 0 | 0 |
| 5 | 4 | 2 | 1 | 0 |
| 6 | 0 | 0 | 1 | 0 |
| **Total** | **34** | **23** | **5** | **1** |

---

## Decisions

Session: 2026-09-06, owner (Steve McCormack) sign-off via interactive Q&A, Sonnet 5 then
Fable 5.1 (no `/ask-council` pressure-test requested). D-1 and D-9 were each revised by the
owner within the session; the table records the final answer and lists the withdrawn one.

| ID | Decision | Rejected alternatives | Notes |
|---|---|---|---|
| D-1a | **Owner's rule of thumb.** A root folder that hosts, or is planned to host, more than one tool takes the abstract activity name — spelled as the bare activity noun (the owner's phrase: "plural in its singular form" — `code-format`, not `code-formatter`; `code-scan`, not `code-scanner`) — with the CLI-specific names in nested subfolders. A root folder with a single purpose is named for its CLI directly (`git/`, `docker/`, `github/`, `nuget/`). Where the single tool is a dotnet SDK subcommand with no CLI name of its own (`dotnet build` → `code-diagnostics/`, `dotnet format` → `code-format/`, `dotnet test` → `testing/`), or there is no tool at all (`clean/`), the activity name applies. Final root list, owner-signed 2026-09-06: `code-coverage/`, `code-diagnostics/`, `code-format/` (from `code-formatter/`), `code-scan/` (absorbs `code-inspection/` and `sonar-scanner/`), `clean/` (from `maintenance/`), `github/` (from `release/`), `nuget/` (new; takes `Run-NugetUnlist.ps1` from `release/`), `testing/`, `git/`, `docker/`, plus the dot-folders `.shared/` and `.tests/`; `audit-cli/` is being removed by another agent and is ignored here. Everything *inside* a folder — subfolders, scripts, and the slash-command families that drive them — is named for the concrete tool: `code-coverage/coverlet/` (from `xplat/`), `code-coverage/dotcover/`, `code-scan/qodana/` (from `code-inspection/`, with the inner `qodana/` level flattened away so config and `.slnx` sit directly under it), `code-scan/sonarqube/` (from `sonar-scanner/`), `Run-Qodana.ps1`, `Run-SonarScanner.ps1`, `/scan-roslyn-*`, `/scan-qodana-*`, `/scan-sonar`. | §3.1 as written ("one name per thing" across every level, with `code-inspection`→`qodana`, `sonar-scanner`→`sonarqube`, and abstract orchestrator names `Run-CodeDiagnostics`/`Run-CodeFormat`/`Run-SonarQube`); `code-scan/` as a rename of `sonar-scanner/` alone (owner's interim answer, withdrawn: `scan` is the shared verb of every `scan-*` skill and SonarScanner is a CLI, not an activity); nesting `sonarqube/` under `code-inspection/`; all-activity; all-product; leave as-is | Owner's own rule, replacing §3.1's. F-01 is resolved by declaring which level each name lives at, not by collapsing to one name. `code-scan/` is the home for every external scanning CLI, one subfolder per tool; the owner intends to add more (semgrep, snyk, others) later — YAGNI now, but the shape is chosen for them. Spelled `code-scan`, not `code-scanner`, to match the abstract name the Brain already uses: `docs/ai/specs/scan/`, `docs/ai/rules/scan.md`, `docs/ai/commands/scan.md`, and the `scan-*` skill prefix. Roslyn stays in `code-diagnostics/`: it is a build by-product, not a scanning CLI. `code-diagnostics/` qualifies as abstract under the multi-tool branch in its own right — other engines emit build-time diagnostics that are not Roslyn: NuGet audit (`NU1901–NU1904`, emitted by `dotnet restore` today and silently dropped by the current `warning CS` regex), ApiCompat/package validation (`CP0001…`, relevant after first release), and the IL trimmer/AOT analysis (`IL2xxx`/`IL3xxx`, only if `IsTrimmable`/`IsAotCompatible` is ever set). Analyzer packages (CA, StyleCop, SonarAnalyzer, Roslynator, PineGuard.Analyzers) are Roslyn and never get a subfolder; ReSharper `InspectCode` is Qodana's engine and belongs in `code-scan/`. YAGNI corollary: one `Run-CompilerDiagnostics.ps1`, no engine subfolders until a second engine actually gets a wrapper. Consequence for T1.02: parse any `warning|error <PREFIX><digits>`, not only `CS`. `code-analyzer` was considered for the diagnostics domain and rejected: it collides with `src/PineGuard.Analyzers` and with the scanner vocabulary, and Roslyn's own term is `Diagnostic`. §3.1's `code-formatter/` → `code-format/` rename is kept (plain activity noun, matching `code-scan`); its other folder and orchestrator renames are withdrawn; `Run-CompilerDiagnostics.ps1` and `Run-Format.ps1` stay. The registry's `QodanaConfig` paths and `rules/scan.md` / `specs/scan/spec.md` `applies_to` follow the moves in T4.02. Artifacts folders follow the folder names (`artifacts/clean/`, `artifacts/github/`), and the `/clean-*` command family now matches its folder outright. `Run-NugetUnlist.ps1` moves to a new `nuget/` sibling (owner decision: nuget.org tooling never sits under `github/`), becoming `Unpublish-NugetPrerelease.ps1` per §3.2; `/nuget-unlist` and `docs/ai/agents/nuget-unlist.md` keep their names and follow the path in T4.02. `Test-StructuralIntegrity.ps1` stays in `clean/`. |
| D-1b | Only `Run-` (+ plural `Run-Tests`/`Run-Commits`) are sanctioned verb outliers | Keep `Gen-`, `Clean-`, `Setup-` as extra outliers | `Gen-`, `Clean-`, `Setup-` get renamed to approved verbs (`New-`, `Clear-`, `Initialize-`) per §3.2 |
| D-1c | **No mismatch to fix.** Under D-1a a command family names the tool and its folder names the activity by design, so `/scan-roslyn-*` inside `code-diagnostics/` is the intended layering. Nothing renames on either side. | Folder → `tools/roslyn/` (owner's first answer, withdrawn once D-1a was stated); commands → `/scan-diagnostics-*`; keep both and document the mapping | `/scan-roslyn-*`, `/fix-roslyn-all`, `docs/ai/rules/roslyn.md`, the CLAUDE.md palette, `tools/code-diagnostics/` and `Run-CompilerDiagnostics.ps1` all stay exactly as they are. The same reasoning keeps `/scan-sonar` and `rules/scan.md` beside `code-scan/sonarqube/`, and `/scan-qodana-*` beside `code-scan/qodana/`. §3.1's table row and the D-1c row in §4 need replacing with this. |
| D-1d | Support **both** `-WhatIf` (native `SupportsShouldProcess`) and `-DryRun` (alias) everywhere | `-WhatIf` only; `-DryRun` only | **Reverses §3.3's "-WhatIf … replaces -DryRun (rejected)" row** — `-DryRun` becomes a supported alias, not removed; §3.3 needs correcting |
| D-2 | **Keep dot-sourcing**, add a Pester ordering test | Convert `.shared` to a `PineGuard.Tools` script module | **Reverses the plan's default and removes the basis for T3.01 as written** — T3.01 must be rewritten: no module conversion; instead consolidate duplicate functions (F-06, F-07, F-10) and remove the two aggregator shims within the existing dot-source pattern, plus the new ordering test |
| D-3 | **Leave the Qodana Docker compose stack and the 14 hand-written `.slnx` files as-is** — no removal, no run-time generation | Remove compose stack; generate per-scope config from the registry | T1.05 (hand-add the missing MediatR/Analyzers `.slnx`/config so `Run-Qodana -Scope MediatR` stops throwing) still happens — that is a bug fix, not the D-3 restructuring. T3.04's compose-removal and config-generation scope is dropped. |
| D-4 | One `Get-ToolSecret` resolver + `.env` + SonarQube bound to loopback + generated admin password | Keep the persisted User-level environment variable | Adopted as recommended (§3.4) |
| D-5 | **PowerShell only for now** — fix Windows-only path literals in place across `tools/**` (excluding `audit-cli`) | Proceed with a Bash rewrite now | Bash is a **future aspiration, not executed now**: `cross-platform-tools-migration.md` is marked **deferred**, not superseded/deleted. No CI job changes — the Windows-pinned `audit` job (`.github/workflows/ci.yml:701-704`) runs `tools/audit-cli/Run-All.ps1`, which is out of scope here and owned by the separate audit-cli review. |
| D-6 | Table-driven `Run-Commits.ps1 -Scope <string[]>` replacing all 16 `Commit-*.ps1`, conventional-commit auto message | Keep 16 scripts, patch only | Adopted as recommended |
| D-7 | Tool tests live under `tools/.tests/` | `tests/PineGuard.Tools.Tests/` | Adopted as recommended |
| D-8 | **Defer Phase 5** — docs/spec rewrites do not proceed on this pass | Lift the freeze now (New Surfaces Program is complete, `d94b85b`) | Owner deferred despite the freeze's original justification no longer applying; Phase 5 stays blocked until the owner reopens it |
| D-9 | **Two engine scripts behind a delegating front door.** `coverlet/New-CoverageReport.ps1` and `dotcover/New-CoverageReport.ps1` (the §3.5 pair, one parameter contract) sit behind a root `tools/code-coverage/New-CoverageReport.ps1 -Engine Coverlet\|DotCover` that validates and delegates only; `Run-CodeCoverage` calls the front door | One script with `if ($Engine)` branches (owner's first answer, revised in the same session); §3.5 as written (`Run-CodeCoverage` routes to the engine folders directly, no front door); dotCover authoritative; flatten to Coverlet only | Coverlet stays the CI/100%-gate authority; dotCover is the local/Rider second opinion, reported not gated. Engine scripts keep the same basename because the folder supplies the engine — the repo's own precedent (fixture inner classes drop the parent's infix, `IsIsoAlpha2Code` → `Countries.IsAlpha2Code`); engine-named files (`New-CoverletReport.ps1`) were considered and rejected as repeating the folder in the name. §3.5's routing sentence and T3.03/T3.11 need a one-line update: the routing point is the front door, not the orchestrator. |

### Follow-up required before Phase 1 dispatch

D-1a replaces §3.1's vocabulary rule and table outright; D-1c, D-1d and D-2 reverse a default
recommendation the rest of the document was written around; D-9 adds a front-door script to §3.5.
Before dispatching Phase 1/2/3 tasks, §3.1 (rewrite to the two-level rule; the folder moves are
`code-inspection/` → `code-scan/qodana/`, `sonar-scanner/` → `code-scan/sonarqube/`,
`code-formatter/` → `code-format/`, `maintenance/` → `clean/`, `release/` → `github/` with
`Run-NugetUnlist.ps1` out to a new `nuget/`, and `code-coverage/xplat/` → `code-coverage/coverlet/`),
§3.2's orchestrator examples, §3.3, §3.5, and the Phase 3/4/5 task tables (T3.01, T3.03, T3.04,
T3.05, T3.06, T3.07, T3.08, T3.11, T4.01, T4.02, T5.01) need a
text pass to match the decisions above — not a re-decision, just bringing the prose in line with
what was actually chosen. Phase 4's rename cascade is those six folder moves, the new `nuget/` folder, plus the
script-level renames in §3.2 as decided in D-1b (`Gen-`→`New-`, `Clean-`→`Clear-`,
`Setup-`→`Initialize-`, `Initialize-`→`Install-` where it installs, `*-up`/`*-down`→`Start-`/`Stop-`,
`Test-CoverageAnalysis`→`Test-Coverage`, and the F-02 `Run-` singles); no slash command, skill, or
rules file changes name.

## Baselines

*(appended by T2.02)*
