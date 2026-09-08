<!-- metadata_header
type: plan
id: code-scan-tooling
version: 1.0
status: decisions-signed-off
last_updated: 2026-09-08
scope: apps/cli (new scan, dev and test command groups and their config), tools/Install-Cli (new root installer), tools/code-scan/**, tools/.shared (cli, dotenv, scopes), .env.example, a new scan workflow under .github/workflows, the docs/ai scan spec, rules, skills, workflows, agents and commands, and the adapter cascade
-->

# Plan: Code-Scan Tooling — one contract, twenty tools, `pineguard scan`

## 0. Executive summary

The owner asked on 2026-09-08 for the repository's quality tooling to cover a much wider set of
scanners, split cleanly into what runs on a developer machine and what runs only in the GitHub
pipeline, with an installer and a run script for every tool, narrowing to a branch or pull request,
output-directory and environment control for CI, and a `pineguard scan` and `pineguard test
coverage` surface in the TypeScript CLI, each with a skill. The nineteen names in the request were
curated to **twenty tools** (§1.2): fifteen runnable scanners, three fetch-only GitHub services, one
deferred enterprise slot, plus the two existing coverage engines behind `pineguard test coverage`.
Six names were removed or folded (§1.3), one of them mid-session (OWASP Dependency-Check, D-9).

The design is **not twenty scripts**. It is one run contract (§2.3), one manifest per tool (§2.2),
SARIF as the single output spine (§2.5), and one changed-set helper for narrowing (§2.4). The
engine lives in TypeScript under `apps/cli/src/scan/` and reuses what the audit CLI already has:
commander, the reporters, the baseline ratchet, and `listChangedFiles`. PowerShell keeps a
`Run-<Tool>.ps1` and `Install-<Tool>.ps1` front door per tool for local development (D-1); each
maps the standardisation plan's parameter vocabulary onto CLI flags and delegates. The four
existing tools (Roslyn, SonarQube, Qodana, coverage) stay PowerShell-owned and the CLI shells out
to them, so nothing that works today is rewritten.

The **two-tier principle** (§1.1) is the rule every later choice follows: every concern has a
local, token-free tool that runs on a laptop and in CI alike; cloud services run only in the
pipeline, or on GitHub's side with no job at all; cloud-only services still get a `pineguard scan`
verb, but there the verb means *fetch* — their alerts are pulled through `gh api` into the same
SARIF and summary shape as everything else (D-14).

Developer-environment concerns get their own `dev` group (`pineguard dev install|doctor|list|env`)
and a root `+ tools/Install-Cli.ps1`, so a contributor goes from clone to a complete toolchain in one
command and CI runs that same command (D-7, D-15). A repository-root `.env` is the one place every
variable lives; every entry script and the CLI load it first, and `.env.example` is generated from
the manifests rather than maintained by hand (D-15, D-16).

Six phases. Phase 0 is prerequisites and one handoff from the standardisation plan. Phase 1 lands
the whole contract with two reference adapters (`nuget-audit`, `inspectcode`). Phase 2 fans the
local tier out, one Sonnet dispatch per tool. Phase 3 wraps the existing tools, the cloud-backed
ones, the fetch-only services, and the `test` group. Phase 4 is the pipeline. Phase 5 is the Brain
and adapter cascade. Phase 6 is independent verification and merge. Orchestration follows the
standardisation plan's §5 rule set: a Sonnet orchestrator that never edits, Haiku for bulk,
Opus for verification, Fable for names.

---

## 1. Scope, tiers, and the principle

| Item | Detail |
|---|---|
| In scope | Everything in the metadata header above. New CLI command groups `scan`, `dev`, `test`. A root `+ tools/Install-Cli.ps1`. New folder `tools/code-scan/` with one subfolder per tool. The two folder moves the standardisation plan listed as T3.04/T3.05 (`code-inspection/` → `code-scan/qodana/`, `sonar-scanner/` → `code-scan/sonarqube/`) transfer here as T1.08. |
| Out of scope | Rewriting `Run-CodeCoverage.ps1`, `Run-Tests.ps1`, `Run-Qodana.ps1`, `Run-SonarScanner.ps1` or `Run-CompilerDiagnostics.ps1` in TypeScript — the CLI dispatches to them. The standardisation plan's remaining Phase 3 (git collapse, `.shared` consolidation, coverlet/dotcover layout, `clean/`, `github/`, `nuget/` moves). Debt burn-down of whatever the new scanners find (baselined, tracked separately, same as the audit ratchet). |
| Environment | pwsh 7.6, Node 22 + pnpm, .NET 10 SDK, Windows 11 locally; `ubuntu-latest` in CI (pwsh preinstalled). The repository is public under MIT, which is what makes CodeQL, secret scanning, push protection, Dependabot, and SonarCloud free. |
| Branch | `feature/code-scan-tooling` in worktree `.claude/worktrees/code-scan-tooling`, from `main` at `63ff60e`. |

### 1.1 Two tiers

| Tier | Runs where | Needs | Members |
|---|---|---|---|
| **Local** | Developer machine and CI, identically | No account, no token. A vulnerability database download at most, cacheable and skippable with `--offline`. | Every row in §1.2 with locality `local` |
| **Pipeline** | GitHub Actions, or GitHub itself with no job | A token in Actions secrets, or a repository setting | Every row with locality `cloud-backed` (runs in Actions when its token is present) or `cloud-only` (runs on GitHub's side; the CLI fetches its alerts) |

Rules that follow:

1. **Every concern has a local tool.** A developer can get the same class of signal before pushing
   as the pipeline gives after. Secrets: gitleaks locally, secret scanning plus push protection on
   GitHub. Dependencies: nuget-audit and trivy locally, Dependabot on GitHub. Code: inspectcode,
   semgrep and codeql locally, code scanning and SonarCloud on GitHub.
2. **`pineguard scan all` is tier-aware.** Outside CI it runs the local set. Inside CI (`--ci`,
   defaulted from `GITHUB_ACTIONS`) it adds every cloud-backed tool whose token is present and
   skips, with a one-line notice, every one whose token is absent. It never runs a fetch-only
   service; `pineguard scan dependabot` is an explicit fetch.
3. **Cloud tier uses the vendor's own runner** (the existing Qodana action, the SonarCloud action,
   CodeQL default setup, Dependabot, secret scanning). Local tier runs through the CLI in CI too,
   so the command a developer types is the command CI runs (D-12).

### 1.2 The curated list (final)

Slugs are the CLI name, the `tools/code-scan/<slug>/` folder, the artifacts folder, the skill
suffix and the command suffix. They are proposed under D-13 and need the owner's naming sign-off.

| Slug | Tool | Concern | Locality | Install channel (local) | Cloud runner | Native narrowing | SARIF | Phase |
|---|---|---|---|---|---|---|---|---|
| `roslyn` | Roslyn compiler diagnostics | SAST | local | dotnet SDK, existing | existing `roslyn` job | scope | native via `-p:ErrorLog` | 3 |
| `inspectcode` | JetBrains ReSharper InspectCode (`jb inspectcode`) | SAST | local | dotnet tool `JetBrains.ReSharper.GlobalTools` | — | scope, paths (`--include`) | native | 1 |
| `qodana` | JetBrains Qodana | SAST | cloud-backed | docker, existing | existing `qodana` job | scope, `--diff-start` | native | 3 |
| `codeql` | GitHub CodeQL CLI | SAST | local | gh extension `github/gh-codeql` | code scanning default setup (D-11) | scope (one database per project), post-filter | native | 2 |
| `semgrep` | Semgrep Community Edition | SAST | local, cloud optional | pipx `semgrep`, docker fallback | — (`semgrep ci` only with a token; not planned) | commit range (`--baseline-commit`), paths | native | 2 |
| `sonarqube` | SonarQube | SAST | local (Community, docker) and pipeline (SonarCloud) | dotnet tool `dotnet-sonarscanner`, existing | SonarCloud scan action | branch and PR on SonarCloud only | converted from the issues API | 3 |
| `snyk` | Snyk Open Source + Snyk Code | SAST + SCA | cloud-backed | npm, `apps/cli` devDependency | same CLI in Actions with `SNYK_TOKEN` | none | native (`--sarif`) | 3 |
| `nuget-audit` | NuGet Audit (`dotnet list package --vulnerable --include-transitive`) | SCA | local | dotnet SDK | — | none, change-triggered | converted from `--format json` | 1 |
| `trivy` | Trivy | SCA + secrets + config + licence | local | winget/brew, release binary | — | paths | native | 2 |
| `cyclonedx` | CycloneDX for .NET | SBOM | local | dotnet tool `CycloneDX` | — | none, change-triggered | n/a, produces `bom.json` | 2 |
| `grype` | Grype | SCA over the SBOM | local | winget/brew, release binary | — | none, change-triggered | native | 2 |
| `gitleaks` | Gitleaks | secrets | local | winget/brew, release binary | — | commit range (`--log-opts`) | native | 2 |
| `actionlint` | actionlint | config (workflows) | local | winget/brew, release binary | — | paths | converted via the documented SARIF template | 2 |
| `zizmor` | zizmor | config (workflow security) | local | pipx, release binary | — | paths | native | 2, optional |
| `hadolint` | hadolint | config (Dockerfiles) | local | winget/brew, release binary | — | paths | native | 2 |
| `dependabot` | Dependabot alerts | SCA | cloud-only, fetch | — | `.github/dependabot.yml`, existing | n/a | converted from alerts | 3 |
| `secret-scanning` | GitHub secret scanning + push protection | secrets | cloud-only, fetch | — | repository setting | n/a | converted from alerts | 3 |
| `code-scanning` | GitHub code scanning alerts | SAST | cloud-only, fetch | — | default setup | n/a | converted from alerts | 3 |
| `wiz` | Wiz (`wizcli`) | SCA + config | cloud-only, enterprise | wizcli | tenant needed | n/a | native | deferred, stub only |
| — | Copilot code review | AI review | cloud-only | — | repository setting | n/a | n/a | outside `scan`; noted in §2.9 |

Coverage engines `coverlet` and `dotcover` are not scanners; they sit behind `pineguard test
coverage --engine` (§2.8). PSScriptAnalyzer stays the `tools/` lint gate in `Test-Tools.ps1` and is
not a `scan` member.

### 1.3 Removed, folded, parked

| Name in the request | Outcome | Reason |
|---|---|---|
| csharpier | **Removed** (D-6) | `dotnet format` is fully wired; two formatters fight; owner rule: prefer standard tooling. `jb cleanupcode` is out for the same reason. |
| owasp-dc | **Removed** (D-9) | Its only job is SCA against the NVD. Trivy and Grype read the same lock files and draw on the same advisory sources, faster, without Java or an NVD API key; Snyk covers the cloud side. Its .NET analyzers are weaker and noisier than Trivy's. Re-add only if a client mandates it by name. |
| gh-codeql | **Folded** into `codeql` | It is the installer and version pinner for the CodeQL CLI, not a scanner. |
| reportgen | **Folded** into coverage | ReportGenerator is already pinned in `+ .config/dotnet-tools.json` on the standardisation branch. |
| dotnet-counters | **Parked** (D-5) | Observes a running process. PineGuard is a library. If a perf family is ever wanted, BenchmarkDotNet is the tool. |
| gherklin | **Deferred** (D-5) | No feature files exist. BDD is a no for the library's unit tests: `Theory` plus `TheoryData` is already the executable spec, SpecFlow is discontinued, and Reqnroll plus gherklin would only earn its keep for integration-surface acceptance scenarios later. |
| copilot | **Kept, outside `scan`** | A pull-request reviewer, not a scanner. Enabled as a repository setting; `dev doctor --remote` reports whether it is on. |
| trivy secret scanner | **Kept enabled, not the secrets tier** | Working-tree only; gitleaks is history-aware and commit-range capable. |

---

## 2. Target design

### 2.1 Layout

```
apps/cli/src/
  scan/
    catalog.ts             manifest registry, mirrors audit/catalog.ts
    contract.ts            ScanOptions, Target, Tier, Severity, ExitCode
    engine.ts              resolve → doctor check → narrow → run → normalise → baseline → gate → report
    changed.ts             branch (reuses audit/repo listChangedFiles), pr (gh pr diff), paths; scope mapping; change triggers
    scopes.ts              reads tools/.shared/scopes.json (D-10)
    sarif/
      normalise.ts         SARIF 2.1.0 shape, tool.driver, automationDetails.id = pineguard/<slug>[/<scope>]
      summary.ts           counts by severity, per rule, per file; aggregate for `all`
      filter.ts            post-filter results to the changed set
      convert/<slug>.ts    JSON/XML/API → SARIF for tools without native SARIF
    tools/<slug>.ts        ScannerAdapter: manifest + detect + buildArgs + run + parse
  dev/
    channels.ts            dotnet-tool, gh-extension, npm, winget, brew, release-binary, pipx, docker
    install.ts             --all | --cli | --dependencies | <slug...>
    doctor.ts  list.ts
    env.ts                 --check | --list | --example (generates .env.example) | --relocate <root> [--write] (§2.7.2 block under one writable root)
  test/
    run.ts                 dispatch to tools/testing/Run-Tests.ps1; --include-cli adds pnpm -C apps/cli test
    coverage.ts            dispatch to tools/code-coverage/New-CoverageReport.ps1 -Engine (Run-CodeCoverage.ps1 until D-9 of the standardisation plan lands)
  commands/scan.ts  dev.ts  test.ts
apps/cli/config/
  scan-baseline.json       ratchet floor keyed by (slug, ruleId, file, normalised message)
  scan-exceptions.json
apps/cli/test/
  scan/**                  engine, changed, sarif, per-adapter suites
  fixtures/scan/<slug>/    sample native output (pass and fail), expected summary, can-fail assertion (VIBE, as for audit)
.env                       gitignored; the one place every variable lives (D-16)
.env.example               committed; generated by `pineguard dev env --example`, never hand-edited
tools/
  Install-Cli.ps1          -All | -Cli | -Dependencies | -Tool <slug[]>  (D-15); -Cli is self-sufficient, the rest delegates to pineguard dev install
  .shared/cli.ps1          Resolve-PineGuardCli, Invoke-PineGuardCli (builds dist on demand, exit 2 if Node 22 or pnpm missing)
  .shared/dotenv.ps1       Import-DotEnv (exists) + Get-DotEnvVariable, Set-DotEnvVariable, Test-DotEnvVariable (D-15)
  .shared/scopes.json      the scope registry (D-10)
  code-scan/
    README.md
    Run-Scan.ps1           -Tool <slug>|All  → pineguard scan
    <slug>/
      Run-<Tool>.ps1       tool-specific ValidateSets and help; maps to the contract; -ToolArgs passthrough
      Install-<Tool>.ps1   → ../../Install-Cli.ps1 -Tool <slug>
      README.md
      config/              the tool's native config, passed explicitly (see structural rule below)
    qodana/                moved from code-inspection/, inner qodana/ level flattened (T1.08)
    sonarqube/             moved from sonar-scanner/ (T1.08)
.github/workflows/scan.yml
artifacts/code-scan/<slug>/[<scope>/]{results.sarif, summary.json, results.md}
artifacts/code-scan/summary.json      aggregate written by `scan all`
```

Structural rules, in addition to the standardisation plan's §3.4:

- **Tool-native config lives under `tools/code-scan/<slug>/config/` and is passed explicitly**
  (`--config`, `-c`, `--rules`). No new dot-files at the repository root. The single exception is
  actionlint, whose convention is `+ .github/actionlint.yaml`.
- **Downloaded binaries live outside the repository**: `$env:LOCALAPPDATA/pineguard/bin` on
  Windows, `~/.local/share/pineguard/bin` elsewhere, overridable with `PINEGUARD_TOOLS_BIN`. They
  are added to `PATH` for the process only. `Clean-Artifacts` never touches them.
- **Every adapter is cross-platform on day one.** No `.exe`, no backslash literals, no
  `$env:TEMP`. The vitest suite runs on `windows-latest` and `ubuntu-latest` in the `cli` CI job.
- **A dispatch adapter** (roslyn, sonarqube, qodana, `test run`, `test coverage`) invokes the
  PowerShell script by relative path via `pwsh -NoProfile -File`. This is the only place the CLI
  depends on pwsh; it is documented as transitional.
- **`.env` is loaded first, everywhere.** Every entry script's fixed header dot-sources
  `tools/.shared/dotenv.ps1` and calls `Import-DotEnv` before anything else; the CLI loads the same file
  with `process.loadEnvFile`. No script reads a token any other way. `.env.example` is generated
  from the manifests and is the documentation of every variable (D-15).

### 2.2 Manifest

One per tool, in TypeScript so it is typed, tested, and the single source the PowerShell front
doors, the skills, and the READMEs are generated from.

```ts
interface ScannerManifest {
  slug: string;                       // "trivy"
  name: string;                       // "Trivy"
  concern: ("sast" | "sca" | "secrets" | "config" | "sbom" | "licence")[];
  locality: "local" | "cloud-backed" | "cloud-only";
  fetchOnly?: true;                   // cloud-only services: `scan <slug>` pulls alerts, never runs a scanner
  status: "active" | "optional" | "deferred";
  install: {
    channel: "dotnet-tool" | "gh-extension" | "npm" | "winget" | "brew" | "release-binary" | "pipx" | "docker" | "sdk" | "none";
    package: string;                  // "JetBrains.ReSharper.GlobalTools", "aquasecurity/trivy"
    version: string;                  // exact pin; `tool doctor` reports drift
    fallback?: "docker";              // image pinned by tag
    detect: string[];                 // ["trivy", "--version"]
  };
  env: {                               // vendor names only, never renamed (D-17)
    required: string[];                // tokens a run cannot proceed without: SNYK_TOKEN
    optional: string[];                // endpoints and overrides: SNYK_API, SNYK_CFG_ORG
    homes: string[];                   // relocation variables `dev env --relocate` sets: SNYK_CACHE_PATH
  };
  targets: {                           // what narrowing the tool supports natively
    scope: boolean;                    // per project or solution file
    paths: boolean;                    // explicit file list
    commitRange: boolean;              // base..head
    changeTriggers?: string[];         // solution-wide tools: run under branch/pr only if one of these changed
  };
  output: { native: "sarif" | "json" | "xml" | "text" | "api"; converter?: string };
  gate: { defaultFailOn: "none" | "low" | "medium" | "high" | "critical" };
  scopeVariants: boolean;              // D-8: whether /scan-<slug>-<scope> commands exist
  fix: boolean;                        // D-8: whether a fix-<slug> skill exists
  ciAction?: string;                   // cloud tier only: the vendor action that runs it in Actions
}
```

`pineguard scan --list --format json` emits the catalog; a Pester parity test asserts that
`Run-Scan.ps1`'s `-Tool` ValidateSet, the `tools/code-scan/*/` folders, `docs/ai/commands/scan.md`,
and the skills index all agree with it.

### 2.3 Run contract (CLI flags ↔ PowerShell parameters)

Parameter names follow the standardisation plan §3.3 (D-13). Every flag has the same meaning on
every tool; tool-specific switches go through `--tool-args`.

| CLI flag | PowerShell | Meaning | Default |
|---|---|---|---|
| `<slug>` \| `all` | `-Tool` | Which scanner, or the tier-aware set | required |
| `--scope <name>` | `-Scope` | Project scope from the registry; `all` | `all` |
| `--target solution\|branch\|pr\|paths` | `-Target` | What to narrow to (§2.4) | `solution` locally; `pr` in CI on a pull request |
| `--base <ref>` | `-BaseRef` | Base for `branch`; `main` then `origin/main` when omitted | resolved |
| `--pr <n>` | `-PullRequest` | Pull request for `pr`; current PR when omitted | current |
| `--paths <glob...>` | `-Paths` | Explicit file set for `paths` | — |
| `--output <dir>` | `-OutputPath` | Artifacts root | `artifacts/code-scan/` |
| `--format pretty\|json\|github\|sarif` | `-Format` | Console reporter; SARIF and summary files are always written | `pretty`; `github` under `--ci` |
| `--fail-on none\|low\|medium\|high\|critical` | `-FailOn` | Gate threshold (exit 3) | manifest default; `high` under `--ci` |
| `--baseline <path>` / `--update-baseline` / `--no-baseline` | `-Baseline` / `-UpdateBaseline` / `-NoBaseline` | Ratchet, same semantics as `pineguard audit` | `+ apps/cli/config/scan-baseline.json` |
| `--offline` | `-Offline` | Skip database updates; fail with exit 2 if no cache | off |
| `--ci` | `-CI` | Non-interactive, `github` reporter, no browser, tier = pipeline | on when `GITHUB_ACTIONS=true` |
| `--timeout <s>` | `-Timeout` | Seconds; exit 124 | per manifest |
| `--open` | `-Open` | Open the HTML or SARIF viewer afterwards | off |
| `--dry-run` | `-WhatIf` (`-DryRun` alias) | Print the resolved command line and target set, run nothing | off |
| `--tool-args "<raw>"` | `-ToolArgs` | Appended verbatim to the tool's command line | — |
| `--force` | `-Force` | Run a change-triggered tool even when nothing relevant changed; run a cloud-backed tool outside CI | off |

Exit codes, aligned with the standardisation plan §3.4: `0` clean or under threshold; `1` the
tool itself failed; `2` usage or prerequisite (not installed, token missing, offline with no cache —
the message names the `tool install` or `.env` fix); `3` quality gate not met; `124` timeout.

`scan all` exits with the highest code any member produced and prints one table.

### 2.4 Narrowing to a branch or pull request

The changed set is computed once, by `changed.ts`, and every adapter receives it. Three sources:

| Target | How | Notes |
|---|---|---|
| `branch` | `listChangedFiles` from the audit CLI: `git merge-base <base> HEAD` then a two-dot diff to the working tree, so uncommitted work is included | `--base` overrides the resolved `main`/`origin/main` |
| `pr` | `gh pr diff --name-only [<n>]`; in Actions, `GITHUB_BASE_REF` and the event payload | Falls back to `branch` with a notice when no PR exists |
| `paths` | `--paths` globs, resolved against `git ls-files` | |

The adapter then narrows by the best mechanism its manifest declares, in this order:

1. **Commit range**, when `targets.commitRange` (semgrep, gitleaks): pass base and head natively.
2. **Paths**, when `targets.paths` (inspectcode, trivy, actionlint, hadolint, zizmor): pass the
   file list, or the set of directories it collapses to.
3. **Scope mapping**, when only `targets.scope` (codeql, qodana, roslyn, sonarqube): changed files
   resolve to scopes through the registry; the tool runs per affected scope.
4. **Post-filter**, always available: run in full, then `+ apps/cli/src/scan/sarif/filter.ts` keeps only results whose
   location is in the changed set. `--no-filter` disables it. This is the fallback for any tool
   with no native narrowing and the second pass on top of scope mapping.

Solution-wide tools (`nuget-audit`, `cyclonedx`, `grype`, `snyk`) declare `changeTriggers`
(`Directory.Packages.props`, `packages.lock.json`, `**/*.csproj`, `*.slnx`). Under `branch` or `pr`
they run only when a trigger changed, otherwise they report "skipped, nothing relevant changed"
with exit 0. `--force` overrides.

### 2.5 SARIF spine and output layout

- Every run writes `results.sarif` (SARIF 2.1.0), `summary.json`, and `results.md` under
  `artifacts/code-scan/<slug>/[<scope>/]`. `scan all` adds `+ artifacts/code-scan/summary.json`.
- Tools without native SARIF get a converter under `sarif/convert/`: nuget-audit (JSON),
  sonarqube (issues API), roslyn (none needed — `dotnet build -p:ErrorLog=<file>,version=2.1`
  emits SARIF natively, which is the standard route), actionlint (its documented SARIF template),
  and the three fetch-only alert shapes.
- `automationDetails.id` is `pineguard/<slug>` or `pineguard/<slug>/<scope>`; this is the upload
  category in CI and never collides with default-setup CodeQL's own category.
- The reporters are the audit CLI's (`pretty`, `json`, `github`, `sarif`), extended with a
  severity column. The `github` reporter emits workflow annotations with the SARIF level mapped
  to notice, warning, error.
- The baseline ratchet reuses `applyBaseline`, `computeBaselineSnapshot`, and
  `writeBaselineFile` with the key `(slug, ruleId, file, normalised message)`. First run per tool
  snapshots the debt; the gate then only fires on new findings; entries are deleted as debt is
  paid, exactly as the audit ratchet works.

### 2.6 Install channels

Preference order, a corollary of D-6: dotnet tool, gh extension, npm as an `apps/cli`
devDependency, winget or brew, pinned release binary with SHA-256 verification, pipx, docker.

| Channel | Local install | Pin lives in | CI |
|---|---|---|---|
| `dotnet-tool` | `dotnet tool restore` | `+ .config/dotnet-tools.json` (adds `JetBrains.ReSharper.GlobalTools`, `CycloneDX`) | same |
| `gh-extension` | `gh extension install github/gh-codeql && gh codeql set-version <v>` | manifest | same |
| `npm` | `pnpm install` (snyk is a devDependency of `apps/cli`) | `pnpm-lock.yaml` | same |
| `winget` / `brew` | `winget install --id <id> --version <v>` / `brew install <formula>` | manifest | prefer release-binary for determinism |
| `release-binary` | download from the vendor's GitHub release, verify SHA-256, place in the tools bin | manifest (version + checksums per OS) | same, cached by `actions/cache` |
| `pipx` | `pipx install semgrep==<v>` | manifest | same |
| `docker` | `docker run <image>:<tag>` | manifest | fallback only |
| `sdk` | nothing; the .NET SDK provides it | — | — |

`pineguard dev install` is idempotent and honours `--version`, `--force`, `--dry-run`. `--all`
installs the CLI build, the dependencies (`dotnet tool restore`, `pnpm install`) and every active
tool; `--cli`, `--dependencies` and `<slug>...` select subsets. `+ tools/Install-Cli.ps1` is the
PowerShell front door with the same switches (D-15) and is the one command a contributor runs
after cloning; CI runs the same path.
`pineguard dev doctor` prints one row per tool: installed version versus pin, tokens present or
absent, database cache age; exits 2 if anything a requested run needs is missing.
`pineguard dev doctor --remote` queries `gh api repos/{owner}/{repo}` and reports whether secret
scanning, push protection, Dependabot alerts, code scanning default setup, and Copilot review are
enabled, so the cloud tier's state is visible from the terminal.
`pineguard dev env --check` lists every variable the selected manifests declare, present or
missing, without echoing a value, and fails on a relocation variable that names an unwritable
directory or on `NODE_TLS_REJECT_UNAUTHORIZED=0`; `--example` regenerates `.env.example`;
`--relocate <root>` emits the §2.7.2 block for a locked-down machine.

### 2.7 Environment variables

Four groups. Every name in the first three is the name the vendor documents, checked against the
vendor's own page on 2026-09-08 (D-17; sources at the end of this section). `.env.example` is
generated from the manifests in the same four groups, one commented line per variable naming its
consumer and tier, and nothing is ever echoed by any script or command.

#### 2.7.1 Tokens and endpoints

| Variable | Consumer | Tier | Notes |
|---|---|---|---|
| `GH_TOKEN`, then `GITHUB_TOKEN` | `gh`; zizmor (also `ZIZMOR_GITHUB_TOKEN`); CodeQL upload and pack registries (`GITHUB_TOKEN` only); CycloneDX licence lookup fallback; the three fetch-only adapters | both | Precedence as `gh` documents it. Actions provides `GITHUB_TOKEN`; locally `gh auth login` is enough for `gh`. |
| `GH_ENTERPRISE_TOKEN`, `GH_HOST` | `gh` and zizmor against GitHub Enterprise Server | both | Federated customers on GHES. `GITHUB_HOST` is the MCP server's spelling of the same thing. |
| `COPILOT_GITHUB_TOKEN` | Copilot CLI, ahead of `GH_TOKEN` and `GITHUB_TOKEN` | local | |
| `GITHUB_PERSONAL_ACCESS_TOKEN`, `GITHUB_HOST` | GitHub MCP server | local | |
| `SONAR_TOKEN`, `SONAR_HOST_URL` | SonarScanner CLI's documented names; PineGuard's scripts read them and pass `/d:sonar.token` and `/d:sonar.host.url` to `dotnet-sonarscanner` explicitly; the SonarCloud action reads `SONAR_TOKEN` | both | Replaces the repository's current `SONARQUBE_TOKEN` in scripts (T1.07). Whether the .NET scanner reads them natively is unconfirmed (its tracking issue closed without a visible note), and moot because they are passed explicitly. |
| `SONARQUBE_TOKEN`, `SONARQUBE_URL`, `SONARQUBE_ORG` | SonarQube MCP server only | local | The name the repository uses today; it stays, but only for the MCP server. |
| `QODANA_TOKEN`, `QODANA_ENDPOINT` | Qodana CLI and action | pipeline; local docker optional | |
| `SNYK_TOKEN`, `SNYK_API`, `SNYK_CFG_ORG` | Snyk CLI | pipeline | Org selection is the generic `SNYK_CFG_<KEY>` mechanism. |
| `SEMGREP_APP_TOKEN`, `SEMGREP_APP_URL` | Semgrep, only for `semgrep ci` | none by default | Not set; the CE run uses `SEMGREP_RULES` or `--config`, which is incompatible with the token. |
| `CYCLONEDX_GITHUB_BEARER_TOKEN` | CycloneDX licence resolution; falls back to `GITHUB_TOKEN` | both, optional | |
| `CODEQL_REGISTRIES_AUTH` | CodeQL pack registries on GHES | federated | |
| `GITLEAKS_CONFIG` | gitleaks config path | both | PineGuard passes `--config` explicitly; listed so a contributor's global value is understood. |
| `WIZ_CLIENT_ID`, `WIZ_CLIENT_SECRET`, `WIZ_ENV`, `WIZ_DIR` | `wizcli` | deferred | **Unverified**: the Wiz documentation site rate-limited every fetch. Confirm on the vendor page when a tenant exists; the stub adapter does not use them until then. |

Not variables, despite appearing in the request: OWASP Dependency-Check takes `--nvdApiKey` on the
command line and lets the user pick any variable name to source it from, so `NVD_API_KEY` is a
convention, not a vendor name, and is moot under D-9. OWASP ZAP takes `-config api.key=<key>`; no
official ZAP CLI, image or action reads a `ZAP_API_KEY` variable. ZAP is not adopted; if a DAST tool
is ever wanted for the AspNetCore surface it enters through the manifest like everything else.

#### 2.7.2 Tool homes and caches, for federated machines

Locked-down enterprise machines allow writes only under a demilitarised root such as `C:\Dev` or
`C:\Tools`. Every row below is the vendor's documented variable for moving that tool's home or
cache. `pineguard dev env --relocate <root>` prints this whole block pointed under `<root>`, and
`--write` appends it to `.env`; `dev env --check` verifies each one that is set names a writable
directory. Persisting them to the user profile is the contributor's own step and is documented, not
automated.

| Variable | Tool | Moves | Notes |
|---|---|---|---|
| `DOTNET_ROOT` | .NET | Runtime location used by generated apphosts | Needed when the SDK is unpacked rather than installed. |
| `DOTNET_CLI_HOME` | .NET CLI | Workload packs, first-run sentinels, the default local-tool install location | The SDK appends `.dotnet` itself: set `C:\Dev`, never `C:\Dev\.dotnet`. |
| `NUGET_PACKAGES` | NuGet | Global packages folder, which is also where `dotnet tool restore` places local tools | Takes precedence over `globalPackagesFolder` in `nuget.config`. |
| `NUGET_HTTP_CACHE_PATH`, `NUGET_PLUGINS_CACHE_PATH`, `NUGET_SCRATCH` | NuGet | HTTP cache, plugins cache, lock files | `NUGET_SCRATCH` must be identical for every NuGet process on the machine. |
| `DOTNET_CLI_TELEMETRY_OPTOUT`, `DOTNET_NOLOGO` | .NET CLI | Telemetry and first-run text | `DOTNET_SKIP_FIRST_TIME_EXPERIENCE` is obsolete. `DOTNET_INSTALL_DIR` is not a variable; it is the install script's `-InstallDir` parameter. |
| `npm_config_prefix`, `npm_config_cache` | npm | Global prefix, cache | The vendor's casing is lowercase; `NPM_CONFIG_PREFIX` is documented as equivalent. |
| `PNPM_HOME` | pnpm | Home; global bin under it; the store under `$PNPM_HOME/store` takes priority | `store-dir`, `cache-dir` and `global-dir` are settings; no dedicated variable form is documented. |
| `COREPACK_HOME`, `COREPACK_NPM_REGISTRY` | corepack | Package-manager binaries; the registry they come from | Windows default `%LOCALAPPDATA%\node\corepack`. |
| `PIPX_HOME`, `PIPX_BIN_DIR`, or `UV_TOOL_DIR`, `UV_TOOL_BIN_DIR`, `UV_CACHE_DIR` | pipx or uv, for the semgrep install | Virtual environments and shims | The manifest picks one channel. |
| `PIP_CACHE_DIR`, `PIP_INDEX_URL` | pip | Cache; index mirror | |
| `JAVA_HOME` | Java, for the local SonarQube stack | JDK location | |
| `GH_CONFIG_DIR` | `gh` | Config and extension state | |
| `COPILOT_HOME`, `COPILOT_CACHE_HOME` | Copilot CLI | Config and data; cache | |
| `SONAR_USER_HOME` | SonarScanner | Downloads and cache, default `~/.sonar` | |
| `SNYK_CACHE_PATH` | Snyk | Cache | Not `SNYK_CACHE_HOME`, which does not exist. |
| `TRIVY_CACHE_DIR`, `TRIVY_DB_REPOSITORY`, `TRIVY_SKIP_DB_UPDATE` | Trivy | Cache; database mirror for air-gapped use; offline | Every flag is available as `TRIVY_<FLAG>`. |
| `GRYPE_DB_CACHE_DIR`, `GRYPE_DB_UPDATE_URL`, `GRYPE_DB_AUTO_UPDATE` | Grype | Database cache; mirror; auto-update off | `GRYPE_<SECTION>_<KEY>` convention; the registry-auth keys are documented with an inconsistent prefix upstream and are verified empirically in T2.07. |
| `SYFT_CACHE_DIR` | Syft | Cache | Only if Syft is ever used beside CycloneDX. |
| `DOCKER_CONFIG`, `DOCKER_HOST` | Docker | Client config, which Trivy prefers for registry auth; daemon socket | |
| `XDG_CONFIG_HOME`, `XDG_CACHE_HOME`, `XDG_DATA_HOME` | Go-based tools and hadolint config discovery on Linux | | On Windows these tools use `%APPDATA%` and `%LOCALAPPDATA%` instead. zizmor's docs spell its cache variable `XDG_CACHE_DIR`, which is not the specification's name. |
| `PINEGUARD_TOOLS_BIN` | PineGuard | Where release binaries are placed | The one PineGuard variable in this group. |

Flags only, no variable: `jb inspectcode --caches-home`; the CodeQL CLI, whose database PineGuard
keeps under `artifacts/`; actionlint, which has no environment variables at all.

#### 2.7.3 Proxy and trust

| Variable | Honoured by | Notes |
|---|---|---|
| `HTTP_PROXY`, `HTTPS_PROXY`, `NO_PROXY`, `ALL_PROXY` | .NET and NuGet, `dotnet-sonarscanner`, Snyk, Semgrep, uv, Git through curl, Go-based tools | Set both cases: .NET checks lowercase first; curl honours `http_proxy` in lowercase only. |
| `NODE_EXTRA_CA_CERTS` | Node, and so the `pineguard` CLI and Snyk | The corporate CA bundle under TLS inspection. |
| `npm_config_cafile`, `npm_config_strict_ssl` | npm | |
| `SSL_CERT_FILE`, `SSL_CERT_DIR` | uv; OpenSSL-backed .NET on Linux | |
| `PIP_CERT`, `REQUESTS_CA_BUNDLE` | pip; Semgrep through Python requests | |
| `GIT_SSL_CAINFO`, `GIT_SSL_CAPATH` | Git | |
| `GRYPE_DB_CA_CERT`, `GRYPE_REGISTRY_CA_CERT`, `SYFT_REGISTRY_CA_CERT` | Grype, Syft | |
| `JAVA_TOOL_OPTIONS`, `SONAR_SCANNER_JAVA_OPTS` | Any JVM; SonarScanner 6 and later | `-Dhttps.proxyHost`, `-Djavax.net.ssl.trustStore`. |
| `NODE_TLS_REJECT_UNAUTHORIZED=0` | Nothing, ever | Node's own docs call it strongly discouraged. `dev env --check` fails when it is set. |

#### 2.7.4 PineGuard's own variables

| Variable | Purpose |
|---|---|
| `PINEGUARD_OUTPUT_PATH` | Default for `--output` |
| `PINEGUARD_BASE_REF` | Default for `--base` |
| `PINEGUARD_FAIL_ON` | Default for `--fail-on` |
| `PINEGUARD_TIER` | `local` or `pipeline`; overrides the `GITHUB_ACTIONS` inference |
| `PINEGUARD_OFFLINE` | Default for `--offline` |
| `PINEGUARD_TOOLS_BIN` | Where release binaries are placed (also in §2.7.2) |

Precedence: flag, then environment, then the repository-root `.env` (D-16), loaded by every
PowerShell entry script through `Import-DotEnv` and by the CLI through Node's built-in
`process.loadEnvFile`. `pineguard dev env --check` is how a contributor or a CI preflight learns
what is missing. `GITHUB_ACTIONS=true` turns `--ci` on. PineGuard-prefixed variables exist only for PineGuard's own knobs; a variable a
tool already names is never duplicated under a PineGuard name.

Sources checked on 2026-09-08 for the names above: https://cli.github.com/manual/gh_help_environment
(also `gh help environment` locally on 2.87); https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-programmatic-reference
and the config-dir reference beside it; https://github.com/github/github-mcp-server (README);
https://docs.github.com/en/code-security/codeql-cli/codeql-cli-manual/github-upload-results and
database-analyze; https://github.com/gitleaks/gitleaks (README); https://docs.zizmor.sh/usage/;
https://github.com/rhysd/actionlint; https://github.com/hadolint/hadolint;
https://docs.snyk.io/developer-tools/snyk-cli/configure-the-snyk-cli/environment-variables-for-snyk-cli;
https://semgrep.dev/docs/semgrep-ci/ci-environment-variables and https://semgrep.dev/docs/cli-reference;
https://trivy.dev/docs/latest/configuration/ and the air-gap page; https://oss.anchore.com/docs/reference/grype/configuration/
and the Syft equivalent; https://github.com/CycloneDX/cyclonedx-dotnet (README);
https://www.jetbrains.com/help/qodana/github.html; https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner
and the .NET scanner's configuring page; https://github.com/SonarSource/sonarqube-mcp-server (README);
https://dependency-check.github.io/DependencyCheck/dependency-check-cli/arguments.html;
https://www.zaproxy.org/docs/docker/about/; https://www.jetbrains.com/help/resharper/InspectCode.html;
https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables and the NuGet
cache-folders and environment-variables references; https://docs.npmjs.com/cli/v10/using-npm/config;
https://pnpm.io/cli/setup and https://pnpm.io/settings/store; https://github.com/nodejs/corepack;
https://nodejs.org/api/cli.html; https://pipx.pypa.io/latest/reference/environment-variables.html;
https://docs.astral.sh/uv/reference/environment/; https://pip.pypa.io/en/stable/topics/https-certificates/;
https://git-scm.com/docs/git-config; https://docs.docker.com/reference/cli/docker/;
https://specifications.freedesktop.org/basedir/latest/. The Wiz page returned HTTP 429 on every attempt.

### 2.8 Command surface

```
pineguard scan <slug>|all [--scope] [--target] [--base] [--pr] [--paths] [--output] [--format]
                          [--fail-on] [--baseline|--update-baseline|--no-baseline] [--offline]
                          [--ci] [--timeout] [--open] [--dry-run] [--force] [--tool-args]
pineguard scan --list [--format json]

pineguard dev install [--all] [--cli] [--dependencies] [<slug>...] [--version <v>] [--force] [--dry-run]
pineguard dev doctor [--remote] [--format json]
pineguard dev list
pineguard dev env [--check] [--list] [--example] [--relocate <root> [--write]]

pineguard test run [all|<scope>] [--include-cli] [--filter <expr>] [--framework <tfm>] [--configuration]
pineguard test coverage [--engine coverlet|dotcover] [--scope] [--fail-below <n>] [--fail-branch-below <n>]
                        [--format] [--output] [--open] [--clean]

pineguard audit ...                                  unchanged
```

`test run` maps to `tools/testing/Run-Tests.ps1 -Scope`; `--include-cli` appends
`pnpm -C apps/cli test`. `test coverage` maps to the standardisation plan's D-9 front door
(`tools/code-coverage/New-CoverageReport.ps1 -Engine`) and, until that lands, to
`Run-CodeCoverage.ps1`. The `/coverage-*` and `/test-*` slash-command families do not change
(rename folders, not commands).

The contributor path this gives, from clone to the signal CI will produce, and the path CI itself
runs:

```
./tools/Install-Cli.ps1 -All           # Node/pnpm check, pnpm install, apps/cli build, dotnet tool restore, every scanner
pineguard dev env --check              # every variable the manifests need, present or missing, from .env
pineguard dev doctor                   # versions vs pins, tokens, caches; --remote adds the GitHub settings
pineguard scan all --target branch     # what the pipeline will say about this branch
```

### 2.9 Cloud tier in the pipeline

A new workflow `+ .github/workflows/scan.yml`, separate from `ci.yml` so the build-and-test pipeline
does not grow, triggered on `pull_request`, `push` to `main`, a weekly `schedule` (dependency
findings appear without commits), and `workflow_dispatch` with a `tool` input.

| Job | Runner | Does |
|---|---|---|
| `local-tier` | `ubuntu-latest` | `pineguard dev install --all` (or `tools/Install-Cli.ps1 -All`), `pineguard dev env --check`, `pineguard dev doctor`, `pineguard scan all --ci --target pr --format github`; uploads every `results.sarif` with `github/codeql-action/upload-sarif` under its `pineguard/<slug>` category; caches the Trivy and Grype databases and the CodeQL database by day. Fetch-only adapters run here too so the aggregate summary includes cloud alerts, but they never upload. |
| `sonarcloud` | `ubuntu-latest` | The SonarCloud scan action with `SONAR_TOKEN`; gated by `vars.SONARCLOUD_ENABLED`, mirroring the existing `QODANA_ENABLED` gate. Gives branch and PR analysis that Community Build cannot. |
| `qodana` | unchanged | Stays in `ci.yml` as it is. |
| code scanning | none | CodeQL default setup, a repository setting (D-11). The local `codeql` adapter never uploads, so the two never collide. |
| Dependabot, secret scanning, push protection, Copilot review | none | Repository settings, verified by `dev doctor --remote` in the `local-tier` job as a preflight. |

Permissions: `contents: read`, `pull-requests: read`, `security-events: write`. The gate is
`--fail-on high` with the baseline ratchet absorbing existing debt. Target wall-clock for the
`local-tier` job is under ten minutes; codeql is the long pole and gets its own cache.

### 2.10 Brain and adapter fan-out policy (D-8)

| Artefact | Rule |
|---|---|
| Skill `scan-<slug>` | One per slug, all twenty, on the skill surfaces (`docs/ai`, `.claude`, `.github`, `.agents`). The body carries what the tool finds, how to read its SARIF, and how to act on it. Generated from the manifest, then given a tool-specific section by Sonnet. |
| Agent `scan-<slug>` | One per slug, two-line body, referencing the shared workflow with `Tool = <slug>`. |
| Agent `scan-<slug>-<scope>` | Only where the manifest says `scopeVariants: true`: `inspectcode`, `codeql`, `semgrep`. Existing `roslyn` and `qodana` variants are unchanged. Everything else follows the `/scan-sonar` precedent: one command, whole solution, narrowed by `--target`. |
| Workflow | One parameterised `+ docs/ai/workflows/scan.md` for every new slug. The three existing workflows (`scan-roslyn.md`, `scan-sonar.md`, `scan-qodana.md`) are left as they are. |
| Command | `docs/ai/commands/scan.md` gains one row per agent. |
| Skill `fix-<slug>` | Only where the manifest says `fix: true`: `inspectcode`, `semgrep`, `codeql`, backed by one shared `+ docs/ai/workflows/fix-scan.md` that reads the SARIF and fixes by severity, never suppressing. SCA "fixes" are dependency bumps and belong to Dependabot. Phase 5b, lower priority. |
| Adapter surfaces | `docs/ai/meta/adapter-surfaces.md` §4 gains an exception row: solution-wide scanners have no scope variants by policy. Copilot prompt files follow whatever §4 of that file declares at the time. |
| `scaffold-quality-tool` | Rewritten (T5.06) so the recipe becomes: manifest, adapter, fixtures, two front doors, README, skill, agent, command row. The 52-file recipe is retired. |

---

## 3. Phases

### Phase 0 — Prerequisites and handoffs

| ID | Task | Notes |
|---|---|---|
| P0.01 | Merge `feature/tools-standardisation` (Phases 1 and 2) to `main` with `--no-ff`; rebase this branch | Clean merge verified 2026-09-08 (32 ahead, 47 behind, no conflicts). Gives this plan `+ .config/dotnet-tools.json`, `Test-Tools.ps1`, the Pester suite, and the `tools-lint` job. Owner go-ahead is D-2. |
| P0.02 | `packages.lock.json`: `RestorePackagesWithLockFile=true`, `NuGetAuditMode=all`, `NuGetAuditLevel=low` in `Directory.Build.props`; commit lock files; `dotnet restore --locked-mode` in CI | D-3. Without a lock file, trivy, grype and snyk miss transitive dependencies. `NuGetAudit` warnings (`NU1901`–`NU1904`) then surface at restore and are parsed by the diagnostics tool after the standardisation plan's T1.02. Do not add them to `TreatWarningsAsErrors`; the scan gate handles them. |
| P0.03 | Record the handoff in the standardisation plan: T3.04 and T3.05 transfer to this plan as T1.08 | One-line status change in that plan's playbook. |
| P0.04 | Remove the merged `audit-cli-rebuild` worktree and `worktree-audit-cli-rebuild` branch; delete the empty `tools/audit-cli/solution` directory | Hygiene. Merged on 2026-09-07 (`7d3547f`). |
| P0.05 | `.claude/settings.json`: allow `pnpm -C apps/cli *`, `pineguard *`, `pwsh tools/code-scan/*` | Check before Phase 1 dispatch. |

### Phase 1 — Foundation and two reference adapters

| ID | Task | Acceptance |
|---|---|---|
| T1.01 | Scope registry as data: `+ tools/.shared/scopes.json`; PowerShell reader replacing the literal lists; TS reader `+ apps/cli/src/scan/scopes.ts`; Pester and vitest parity tests against `src/*.csproj`, `tests/*.csproj`, `PineGuard.slnx`, and the `ci.yml` path filter | D-10. One place spells a scope. Both readers produce identical tables. |
| T1.02 | `+ apps/cli/src/scan/contract.ts`, `+ apps/cli/src/scan/catalog.ts`, the manifest type, `scan --list` | `--list --format json` validates against the schema. |
| T1.03 | `+ apps/cli/src/scan/changed.ts`: branch, pr, paths; scope mapping; change triggers | Fixture repository tests for each target, including uncommitted work and a missing PR. |
| T1.04 | `scan/sarif/`: normalise, summary, filter, aggregate | Golden-file tests; a filtered SARIF keeps only changed-set locations. |
| T1.05 | `+ apps/cli/src/scan/engine.ts`: resolve, doctor check, narrow, spawn with timeout, normalise, baseline, gate, report; exit codes | Every exit code has a test. `all` returns the highest member code. |
| T1.06 | `dev/`: channels, `install` (`--all`, `--cli`, `--dependencies`, slugs), `list`, `doctor`, `doctor --remote`, `env` (`--check`, `--list`, `--example`, `--relocate <root> [--write]`) | Install is idempotent and honours the §2.7.2 relocation variables when placing tools; `doctor` exit 2 on a missing pin; `env --check` never echoes a value and fails on an unwritable relocation directory; `--relocate` output is byte-identical to the §2.7.2 table's variable set; `--dry-run` prints and does nothing; `.env.example` round-trips through `env --example`. |
| T1.07 | `+ tools/Install-Cli.ps1`; `+ tools/.shared/cli.ps1`; `Get-`/`Set-`/`Test-DotEnvVariable` in `tools/.shared/dotenv.ps1`; the fixed script header that imports `.env` first; the `.env` move to the root with `Clean-Root.ps1` allow-listing it (D-16); the scripts' `SONARQUBE_TOKEN` read switched to `SONAR_TOKEN` and `SONAR_HOST_URL` with `docs/ai/rules/scan.md` updated (D-17); `+ tools/code-scan/Run-Scan.ps1`; `Install-Cli.ps1 -Cli` also sets `core.hooksPath tools/git/hooks` so the tracked pre-commit hook from `fix/line-endings` is active in every new clone; Pester tests for the switch mapping | `Install-Cli.ps1 -All -WhatIf` prints every step; `-Cli` succeeds on a machine with only Node 22 and pnpm; `Run-Scan -Tool inspectcode -Target Branch -WhatIf` prints the resolved CLI command; exit 2 with guidance when Node 22 or pnpm is absent. |
| T1.08 | Folder moves: `code-inspection/` → `code-scan/qodana/` (inner `qodana/` flattened), `sonar-scanner/` → `code-scan/sonarqube/`; reference sweep across `docs/ai`, `.claude`, `.github`, `.agent`, `.agents`, `.opencode`, `.codex`, `.vscode/tasks.json`, `ci.yml`, the registry's Qodana config paths | Haiku sweep from an exact old→new table; `pineguard audit doc-links` clean; a grep for the old paths returns nothing. |
| T1.09 | Reference adapter A: `nuget-audit` | SDK channel; JSON converter; change triggers; fixtures with a known-vulnerable package; can-fail test. |
| T1.10 | Reference adapter B: `inspectcode` | dotnet-tool channel pinned in `+ .config/dotnet-tools.json`; `--format=Sarif`; `--include` for paths, `--project` for scope; `-ToolArgs` passthrough; verify `.slnx` support in the pinned version, else pass `PineGuard.slnx` through a generated `.sln` and record the workaround in `## Baselines`. |
| T1.11 | Front-door template: `Run-<Tool>.ps1`, `Install-<Tool>.ps1` (delegates to `tools/Install-Cli.ps1 -Tool <slug>`), `README.md` per slug, generated from the manifest; instantiate for the two reference adapters | Pester: every generated script parses, has real `.PARAMETER` text, and at least one `.EXAMPLE`. |
| T1.12 | Scan baseline: `+ apps/cli/config/scan-baseline.json`, `--update-baseline`; snapshot the two reference adapters on this repository | Numbers recorded in `## Baselines`. |
| T1.V | Verify Phase 1 (Opus): contract conformance, `--target branch` correctness on a fixture repo and on this one, exit codes, doctor, both adapters end to end on Windows and ubuntu | Written report appended to `## Baselines`. |

### Phase 2 — Local tier fan-out

One Sonnet dispatch per tool, template T1.10, all concurrent after T1.V.

| ID | Tool | Notes |
|---|---|---|
| T2.01 | `trivy` | `trivy fs --scanners vuln,secret,misconfig,license --format sarif`; database cache; `--offline` maps to `--skip-db-update`; config under `+ tools/code-scan/trivy/config/trivy.yaml`. |
| T2.02 | `gitleaks` | `gitleaks git --log-opts="<base>..HEAD" --report-format sarif`; `+ tools/code-scan/gitleaks/config/gitleaks.toml` with PineGuard-specific patterns (Sonar, Qodana, NuGet API keys); optional pre-commit hook task listed as T2.10. |
| T2.03 | `semgrep` | `semgrep scan --config <pinned packs> --sarif --metrics=off --baseline-commit <base>`; rule packs pinned by version or vendored under `config/`. |
| T2.04 | `actionlint` | Paths; the documented SARIF template under `config/`; `+ .github/actionlint.yaml` is the one root-convention exception. |
| T2.05 | `codeql` | gh-codeql install and `set-version`; `codeql database create --language=csharp --build-mode=none` per scope; `database analyze` with the `codeql/csharp-security-and-quality` pack; database cached under `artifacts/code-scan/codeql/db/`; never uploads (D-11). |
| T2.06 | `cyclonedx` | `dotnet CycloneDX PineGuard.slnx --json --out artifacts/code-scan/cyclonedx/`; produces `bom.json`; no SARIF; change-triggered. |
| T2.07 | `grype` | `grype sbom:artifacts/code-scan/cyclonedx/bom.json -o sarif --fail-on <sev>`; runs T2.06 first when the SBOM is missing or stale; database cache. |
| T2.08 | `hadolint` | Paths; exits 0 with "nothing to scan" when no Dockerfile exists. |
| T2.09 | `zizmor` | Optional; `--format sarif --min-severity`. |
| T2.10 | Pre-commit integration (optional, owner decides) | `pineguard scan gitleaks nuget-audit --target branch --fail-on high` in the existing hook. |
| T2.V | Verify Phase 2 (Opus) | Every adapter has a pass fixture, a fail fixture, and a can-fail assertion; `scan all --target branch` on this repository; `tool install all` on a clean Windows and ubuntu runner (the `cli` job). |

### Phase 3 — Existing, cloud-backed, fetch-only; the `test` group

| ID | Task | Notes |
|---|---|---|
| T3.01 | `roslyn` adapter | Dispatch to `Run-CompilerDiagnostics.ps1 -Scope`, or run `dotnet build -p:ErrorLog=<file>,version=2.1` directly for native SARIF — decide in the task by whichever keeps the diagnostics tool's exit-code and warning semantics; record in `## Baselines`. |
| T3.02 | `sonarqube` adapter | Local: dispatch to `Run-SonarScanner.ps1`, then `Get-SonarIssues.ps1` output converted to SARIF. Pipeline: `sonar.pullrequest.*` and `sonar.branch.name` when `SONAR_TOKEN` and `SONARCLOUD_ENABLED` are set. |
| T3.03 | `qodana` adapter | Dispatch to `Run-Qodana.ps1 -Scope`; reads `qodana.sarif.json`; `--diff-start` mapped from `--target branch`. |
| T3.04 | `snyk` adapter | `snyk test --file=PineGuard.slnx --sarif` and `snyk code test --sarif`; requires `SNYK_TOKEN`; skipped with a notice in `all` when absent; devDependency pin. |
| T3.05 | Fetch-only adapters: `dependabot`, `secret-scanning`, `code-scanning` | `gh api` with pagination; open alerts converted to SARIF and summary; never uploaded; `--state` passthrough. |
| T3.06 | `wiz` adapter stub | Manifest `status: deferred`; adapter returns exit 2 with setup guidance when `WIZ_CLIENT_ID` is absent; `wizcli dir scan --format sarif` mapped for when a tenant exists. Not exercised; a unit test covers the guidance path only. |
| T3.07 | `pineguard test run` and `pineguard test coverage` | Dispatch per §2.8; every existing `Run-CodeCoverage.ps1` switch that matters reachable (`--engine`, `--scope`, `--fail-below`, `--fail-branch-below`, `--format`, `--open`, `--clean`); `--include-cli`. |
| T3.08 | Front doors and READMEs for every Phase 3 slug | From the T1.11 template. |
| T3.V | Verify Phase 3 (Opus) | Dispatch adapters preserve the wrapped scripts' exit semantics; `test coverage --engine coverlet --scope core` still reports 100/100 on both TFMs. |

### Phase 4 — Pipeline

| ID | Task | Notes |
|---|---|---|
| T4.01 | `+ .github/workflows/scan.yml` | §2.9 `local-tier` job: triggers, install, doctor preflight, `scan all --ci --target pr`, per-slug SARIF upload, caches, permissions, `--fail-on high`, baseline. |
| T4.02 | `sonarcloud` job | Token-gated by `vars.SONARCLOUD_ENABLED`; SonarCloud project created by the owner (Tier 1, outside this plan). |
| T4.03 | Repository settings | Enable CodeQL default setup; confirm secret scanning, push protection, Dependabot alerts, Copilot review. Tier 1 operations — the owner confirms each; `tool doctor --remote` then reports them green. |
| T4.04 | `ci.yml`: `dotnet restore --locked-mode`; the `cli` job runs the vitest suite on both runners | Follow-through of P0.02 and the cross-platform rule. |
| T4.05 | Weekly schedule and `workflow_dispatch` with a `tool` input | Scheduled runs use `--target solution`. |
| T4.06 | `publish.yml`: attach `bom.json` to the GitHub release (optional, owner decides) | The SBOM is a release artefact; nothing else changes in publishing. |
| T4.V | Verify Phase 4 (Opus) | Open a throwaway PR: annotations appear for a seeded finding; no cloud-backed tool runs without its token; runtime under ten minutes. |

### Phase 5 — Brain and adapter cascade

| ID | Task | Notes |
|---|---|---|
| T5.01 | `docs/ai/specs/scan/spec.md` v2 | Tiers, manifest, contract, narrowing, SARIF spine, output paths, exit codes, fetch-only semantics, config placement rule. `docs/ai/specs/tools/code-inspection/qodana.md` follows the folder move. |
| T5.02 | `docs/ai/rules/scan.md` | Generalised from SonarQube-only to the scan family; keeps the "never suppress" and token rules. |
| T5.03 | Skills `scan-<slug>` for all twenty slugs on `docs/ai`, `.claude`, `.github`, `.agents` | Haiku instantiates from the manifest template; Sonnet writes each tool-specific section. |
| T5.04 | Workflow `+ docs/ai/workflows/scan.md`; agents `scan-<slug>` and `scan-<slug>-<scope>` per §2.10; `docs/ai/commands/scan.md`; palettes in `CLAUDE.md` and `AGENTS.md`; `.claude/commands`, `.github/prompts`, `.agent/workflows`, `.opencode/commands`, per the parity policy in `docs/ai/meta/adapter-surfaces.md` §4 | Cascade checklist in `adapter-surfaces.md` §5, row by row. |
| T5.05 | `adapter-surfaces.md` §4 exception row and §5 checklist row for manifest and CLI registration; `docs/ai/skills/INDEX.md`; `docs/ai/README.md` | |
| T5.06 | `scaffold-quality-tool` skill v2 | The new add-a-scanner recipe (§2.10 last row). |
| T5.07 | `+ tools/code-scan/README.md`, `tools/README.md` index, `apps/cli/README.md` | READMEs generated from the manifest, never hand-enumerated. |
| T5.08 | `fix-<slug>` skills for `inspectcode`, `semgrep`, `codeql`; shared `+ docs/ai/workflows/fix-scan.md` | Phase 5b, after T5.04. |
| T5.V | `pineguard audit surface-parity` and `doc-links` clean; Opus review of the spec text | |

### Phase 6 — Verification and merge

| ID | Task |
|---|---|
| T6.01 | Full matrix on Windows local and ubuntu CI: `Install-Cli.ps1 -All` on a clean machine, `dev env --check`, `dev doctor`, `scan all` under `solution` and `branch`, `test run all --include-cli`, `test coverage` on both engines, coverage still 100/100 on both TFMs, Pester + PSScriptAnalyzer + vitest + `pineguard audit --gate` green. |
| T6.02 | Plan status, `## Baselines`, and session memory updated; the standardisation plan's handoff row closed. |
| T6.03 | Merge `--no-ff` to `main` after the owner's go-ahead; remove the worktree. |

---

## 4. Orchestration rules

The standardisation plan's §5 applies unchanged, with these substitutions:

1. **Worktree**: `feature/code-scan-tooling` at `.claude/worktrees/code-scan-tooling`. Use
   `git -C` and relative script paths inside the worktree. Never edit the standardisation
   worktree; the only cross-plan touch is the one-line handoff in P0.03.
2. **Dispatch template additions**: "Run `pnpm -C apps/cli typecheck`, `lint`, and `test`, and the
   Pester suite on every touched `.ps1`, before each commit. A new adapter is not done without a
   pass fixture, a fail fixture, and a can-fail assertion. Never write a token to a file or a
   command line."
3. **Model routing**:

   | Tier | Model | Tasks |
   |---|---|---|
   | Judgment (high) | Fable | Decisions (done 2026-09-08); slug sign-off under D-13; any naming question a task raises |
   | Judgment (light) | Opus | T1.V, T2.V, T3.V, T4.V, T5.V, T6.01; review of T5.01 |
   | Implementation | Sonnet | Every T1, T2, T3, T4 task; T5.01, T5.02, T5.06; tool-specific skill sections in T5.03 |
   | Bulk IO | Haiku | T1.08 sweep; T1.11 and T3.08 template instantiation; T5.03 skeletons; T5.04 command fan-out; T5.07 README generation |

4. **Merge policy**: Phases 1–2 may merge to `main` together after T2.V (`--no-ff`, no PR) so the
   local tier is usable early; Phases 3–5 merge after T6.

---

## 5. Risks

| Risk | Mitigation |
|---|---|
| InspectCode's pinned version does not open `.slnx` | T1.10 verifies first; fallback is a generated `.sln` recorded in `## Baselines`; the reference adapter B then still proves the dotnet-tool channel and native SARIF path |
| CodeQL `--build-mode=none` misses generated code or the Analyzers project | Accept for the local tier; default setup in the cloud tier builds properly; note the delta in the codeql skill |
| Semgrep registry rules carry the Semgrep Rules Licence and phone home | `--metrics=off`; pin packs by version or vendor them under `config/`; no `semgrep ci` |
| Trivy handles Central Package Management partially | P0.02's lock file is the real input; T2.01's fixture includes a CPM project |
| SARIF category collision with default-setup CodeQL | Local `codeql` never uploads (D-11); every upload category is `pineguard/<slug>` |
| Baseline debt is large on first run | Same ratchet as audit; debt burn-down is a separate plan; the gate only fires on new findings |
| `local-tier` job too slow | Parallel per-tool steps, database caches, codeql database cache; ten-minute target measured in T4.V |
| Token sprawl across seven services | `dev doctor` and `dev env --check` are the single view; the root `.env` is the single local store; `.env.example` is generated so it cannot drift; CI uses Actions secrets only |
| Windows-only assumptions creep into adapters | vitest on both runners in the `cli` job; the standardisation Pester Windows-ism test extended to the new scripts |
| The CLI's pwsh dependency for dispatch adapters | Documented as transitional; ubuntu runners have pwsh; the four wrapped scripts migrate in a later plan if ever |
| Two initiatives touching `tools/` at once | This plan owns only `code-scan/` and the two moves; the standardisation plan's remaining Phase 3 tasks touch none of the same files |
| Owner finds the fan-out "too much" | Every phase is independently mergeable; Phase 5b and the optional tasks (T2.09, T2.10, T4.06) are explicit opt-ins |
| Enterprise machines only allow writes under a demilitarised folder, and tools default to the user profile | `dev env --relocate <root>` emits every vendor relocation variable under one root; `dev install` places binaries under `PINEGUARD_TOOLS_BIN` and honours the relocation variables; `dev env --check` verifies writability before anything runs |
| TLS inspection breaks downloads and API calls behind a corporate proxy | §2.7.3 lists the trust variable each runtime honours; `dev env --check` refuses `NODE_TLS_REJECT_UNAUTHORIZED=0` so the fix is always the CA bundle, never disabling verification |

---

## 6. Playbook

Status values: `Pending`, `Blocked (D-x)`, `In progress`, `Done`, `Verified`. Tasks sharing a group
letter run concurrently; a group runs after the group it depends on.

| ID | Task | Agent / model | Status | Group | Depends on |
|---|---|---|---|---|---|
| D-1..14 | Decision session, `## Decisions` | Owner + Fable | Done | P0 | — |
| P0.01 | Merge standardisation P1–P2, rebase | Owner + Sonnet | Pending | P0 | D-2 |
| P0.02 | Lock files, NuGetAudit | Sonnet | Pending | P0 | — |
| P0.03 | Handoff note in standardisation plan | Sonnet | Pending | P0 | P0.01 |
| P0.04 | Remove merged audit worktree | Sonnet | Pending | P0 | — |
| P0.05 | settings allow-list | Sonnet | Pending | P0 | — |
| T1.01 | Scope registry as data | Sonnet | Pending | P1-A | P0.01 |
| T1.02 | Contract, catalog, manifest, `--list` | Sonnet | Pending | P1-A | — |
| T1.03 | `changed.ts` | Sonnet | Pending | P1-A | — |
| T1.04 | SARIF normalise/summary/filter | Sonnet | Pending | P1-A | — |
| T1.05 | Engine and exit codes | Sonnet | Pending | P1-B | T1.02–T1.04 |
| T1.06 | `dev` group | Sonnet | Pending | P1-A | — |
| T1.07 | `Install-Cli.ps1`, `cli.ps1`, dotenv helpers, `.env` move, `Run-Scan.ps1` | Sonnet | Pending | P1-B | T1.02 |
| T1.08 | Folder moves + sweep | Sonnet + Haiku | Pending | P1-A | P0.01 |
| T1.09 | `nuget-audit` adapter | Sonnet | Pending | P1-C | T1.05 |
| T1.10 | `inspectcode` adapter | Sonnet | Pending | P1-C | T1.05, T1.06 |
| T1.11 | Front-door template | Sonnet + Haiku | Pending | P1-C | T1.07 |
| T1.12 | Scan baseline | Sonnet | Pending | P1-D | T1.09, T1.10 |
| T1.V | Verify Phase 1 | Opus | Pending | P1-V | P1-D |
| T2.01–T2.09 | Local adapters | Sonnet ×9 | Pending | P2-A | T1.V |
| T2.10 | Pre-commit (optional) | Sonnet | Pending | P2-B | T2.02, owner |
| T2.V | Verify Phase 2 | Opus | Pending | P2-V | P2-A |
| T3.01–T3.06 | Existing, cloud-backed, fetch-only, wiz stub | Sonnet ×6 | Pending | P3-A | T1.V |
| T3.07 | `test run`, `test coverage` | Sonnet | Pending | P3-A | T1.07 |
| T3.08 | Front doors for Phase 3 slugs | Haiku | Pending | P3-B | P3-A |
| T3.V | Verify Phase 3 | Opus | Pending | P3-V | P3-B |
| T4.01–T4.06 | Pipeline | Sonnet | Pending | P4-A | T2.V, T3.V |
| T4.V | Verify Phase 4 | Opus | Pending | P4-V | P4-A |
| T5.01–T5.02 | Spec and rules | Sonnet, Opus review | Pending | P5-A | T3.V |
| T5.03 | Skills ×20 ×4 surfaces | Haiku + Sonnet | Pending | P5-B | T5.01 |
| T5.04–T5.05 | Workflow, agents, commands, palettes, adapter surfaces | Haiku + Sonnet | Pending | P5-B | T5.01 |
| T5.06–T5.07 | scaffold skill v2, READMEs | Sonnet + Haiku | Pending | P5-B | T5.01 |
| T5.08 | `fix-<slug>` (5b) | Sonnet | Pending | P5-C | T5.04 |
| T5.V | Parity and doc-links gates; spec review | Opus | Pending | P5-V | P5-C |
| T6.01–T6.03 | Matrix, status, merge | Opus, then owner | Pending | P6 | P5-V |

### Estimated dispatch count

| Phase | Sonnet | Haiku | Opus | Fable |
|---|---|---|---|---|
| 0 | 4 | 0 | 0 | 1 |
| 1 | 12 | 2 | 1 | 0 |
| 2 | 10 | 0 | 1 | 0 |
| 3 | 7 | 1 | 1 | 0 |
| 4 | 6 | 0 | 1 | 0 |
| 5 | 6 | 4 | 2 | 0 |
| 6 | 0 | 0 | 1 | 0 |
| **Total** | **45** | **7** | **7** | **1** |

---

## Decisions

Session: 2026-09-08, owner (Steve McCormack) by chat, Fable 5.1. D-2, D-9, D-11 to D-14 and D-16
are Fable recommendations the owner asked for by name or that fell out of the accepted design;
D-7 and D-15 were revised mid-session on the owner's instruction. All are recorded as decided but
the owner can veto any of them before Phase 1 dispatch. Slugs in D-13 need explicit naming
sign-off.

| ID | Decision | Rejected alternatives | Notes |
|---|---|---|---|
| D-1 | **TypeScript adapters are the engine; PowerShell front doors stay for local development.** `apps/cli/src/scan/` implements every adapter; `tools/code-scan/<slug>/Run-<Tool>.ps1` and `Install-<Tool>.ps1` map §3.3 parameter names to CLI flags and delegate. The four existing tools stay PowerShell-owned and the CLI dispatches to them. | PowerShell engine with the CLI spawning pwsh for everything (quicker start, bakes pwsh into the CLI forever, every argv builder written once in PS and never testable with SARIF fixtures); TypeScript only with no PS scripts (owner wants PS locally) | Owner: "Lets start moving to TS adapters - can we have both though? I'd prefer PS as well for local dev." Consistent with the recorded direction that new tooling is TS under `apps/` and existing `tools/*.ps1` are not rewritten. |
| D-2 | **Merge the standardisation branch first, then branch and build here.** `feature/tools-standardisation` Phases 1–2 merge to `main` `--no-ff`; this branch rebases; the two `code-scan/` folder moves transfer to this plan (T1.08); the standardisation plan's remaining Phase 3 proceeds independently later. The audit rebuild is already on `main`. | Start here against `main` now and rebase later (two branches renaming `tools/` folders; the Pester suite and `dotnet-tools.json` this plan builds on would arrive mid-flight); do the whole standardisation Phase 3 first (large blast radius, was paused for its own go-ahead, and the scanners need none of it beyond the two moves) | Owner asked for the recommendation. Clean merge verified 2026-09-08. |
| D-3 | **Enable `packages.lock.json`** with `RestorePackagesWithLockFile`, plus `NuGetAuditMode=all` and `NuGetAuditLevel=low`; CI restores with `--locked-mode`. | Stay lock-file-free and accept that SCA sees direct dependencies only | Owner: "Sure." |
| D-4 | **Gaps accepted**: `gitleaks`, `actionlint`, `nuget-audit`, `cyclonedx` (SBOM), and `zizmor` as optional. | Rely on Trivy's secret scanner (working-tree only); rely on GitHub secret scanning alone (fires only after push, no custom patterns without Advanced Security on a private repo) | Owner: "Yes." Gitleaks is not the engine behind GitHub secret scanning; the two are independent, which is exactly why both tiers need one. |
| D-5 | **Park `dotnet-counters`; BDD is a no; `gherklin` deferred.** | Add a perf family now; adopt Reqnroll and gherklin for the library's unit tests | Owner: "Ok." |
| D-6 | **Remove CSharpier; always prefer standard tooling.** `dotnet format` is the formatter; `jb cleanupcode` is out too. Corollary: dotnet SDK features and dotnet tools are the first install channel; `gh api` over a hand-written GitHub client; vendor-official actions for the cloud tier. | Run CSharpier check-only beside `dotnet format` | Owner: "Remove CSharpier - we already have full support for dotnet format. Always prefer standard tooling e.g. dotnet format or other dotnet tools." |
| D-7 | **CLI nouns**: `pineguard scan <slug>\|all`, `pineguard dev install\|doctor\|list\|env`, `pineguard test run\|coverage`. `scan` mirrors `audit <rule>\|all`; `test` is a group because it hosts two operations and the owner wants room for more (`test run all\|<scope> --include-cli`); `dev` is the group for developer-environment concerns, so that installing the toolchain reads as a repository-development action and never as integrating PineGuard into a consumer's project. `scan`, `test` and `audit` stay top-level: they are work verbs, and a consumer-facing PineGuard CLI, if one is ever built, would be a dotnet tool (`dotnet pineguard …`) under D-6, so the two namespaces cannot collide. | `pineguard tool install\|list\|doctor` (Fable's first proposal, withdrawn: "tool" names the thing installed, "dev" names the concern); `pineguard install <slug>`, `pineguard init`, `pineguard bootstrap` flat (all read as consumer actions); `pineguard coverage` top-level (matches the `/coverage-*` family and the `code-coverage/` root, but the owner prefers grouping under `test`; slash commands do not rename either way); `pineguard tools` plural (the CLI's nouns are singular); `pineguard doctor` top-level; `pineguard scan run <slug>` (asymmetric with `audit`); moving `scan`, `test` and `audit` under `dev` as well (consistent but verbose, renames a shipped command across ten adapter surfaces, and unnecessary given the dotnet-tool point) | Owner preferred `pineguard test coverage` and, mid-session, `pineguard dev install --dependencies --cli` "so it is DEAD CLEAR that these are dev concerns not implementation integration concerns"; asked for the recommendation on the rest. |
| D-8 | **Fan-out policy**: scope variants only for SAST tools that honour project scope (`inspectcode`, `codeql`, `semgrep`; existing `roslyn`, `qodana` unchanged); everything else is one command narrowed by `--target`; one parameterised `+ docs/ai/workflows/scan.md`; `fix-<slug>` only for SAST, via a shared `fix-scan.md`; skills for all twenty. | Full 52-file recipe per tool (roughly a thousand files); one parameterised `scan` skill with a tool argument (breaks the `<verb>-<tool>` naming invariant and the surface-parity rule) | Owner: "Sure." Needs the `adapter-surfaces.md` §4 exception row (T5.05). |
| D-9 | **Remove OWASP Dependency-Check.** | Keep as the lowest-priority third SCA tool | Owner asked mid-session whether it has value or whether snyk, wiz and the others cover it. They do: Trivy and Grype read the same lock files and advisory sources, Snyk covers the pipeline. Re-add only if a client mandates it by name. |
| D-10 | **Scope registry becomes data**: `+ tools/.shared/scopes.json`, read by PowerShell and TypeScript; a parity test keeps it aligned with the csproj files, the solution, and the CI path filter. | Keep the registry in `dotnet-projects.ps1` and have the CLI shell out to read it; derive scopes from `PineGuard.slnx` at run time in both languages (two derivations drift); `.config/` (that folder is dotnet's); `apps/cli/config/` (PowerShell reading under `apps/` is inverted) | Location is a proposal; the owner may prefer another. Three scope vocabularies exist today (PS `MustClauses`, CI `must-clauses`, CLI `library\|testing\|docs`); the JSON carries all three spellings per scope so nothing renames. |
| D-11 | **CodeQL in the cloud tier is GitHub's default setup; the local `codeql` adapter never uploads.** | `github/codeql-action` with an explicit workflow (more control, but a second CodeQL run to maintain and category collisions with local uploads) | Standard tooling, zero YAML, free on public repos, and a clean tier split. |
| D-12 | **Pipeline runs the same CLI for the local tier** in a new `scan.yml`; vendor scan actions are used only for the cloud tier (existing Qodana action, SonarCloud action). | Vendor actions for every tool (the command CI runs is then not the command a developer runs, and the contract is never exercised in CI); more jobs inside `ci.yml` | |
| D-13 | **Exit codes and parameter names align with the standardisation plan** (§3.3 names, §3.4 codes `0/1/2/3/124`). **Slugs**: `roslyn`, `inspectcode`, `qodana`, `codeql`, `semgrep`, `sonarqube`, `snyk`, `nuget-audit`, `trivy`, `cyclonedx`, `grype`, `gitleaks`, `actionlint`, `zizmor`, `hadolint`, `dependabot`, `secret-scanning`, `code-scanning`, `wiz`. Root front doors `+ tools/Install-Cli.ps1` and `tools/code-scan/Run-Scan.ps1 -Tool`; shared helpers `+ tools/.shared/cli.ps1` and `tools/.shared/dotenv.ps1`. | `resharper` or `jb` for `inspectcode` (the product and the host CLI; `inspectcode` is the concrete command, and `jb` also hosts a formatter); `dotnet-list-package` or `nuget` for `nuget-audit` (NuGetAudit is Microsoft's own feature name for the same data); `sbom` for `cyclonedx` (activity, not tool — folders inside `code-scan/` name the tool per D-1a of the standardisation plan); `Install-Clis.ps1` or `Install-Scanners.ps1` plural (PowerShell nouns are singular, the owner's own phrase is "plural in its singular form", and only `Run-Tests`/`Run-Commits` are sanctioned plurals); `Install-Scanner.ps1` under `code-scan/` (withdrawn under D-15) | **Owner naming sign-off required** before T1.02. |
| D-15 | **Root installer and `.env` discipline.** `tools/Install-Cli.ps1 -All \| -Cli \| -Dependencies \| -Tool <slug[]>` is the one command from clone to a complete toolchain: `-Cli` is self-sufficient (checks Node 22 and pnpm, `pnpm install`, builds `apps/cli`, sets `core.hooksPath tools/git/hooks`), `-Dependencies` runs `dotnet tool restore` and `pnpm install`, `-Tool` delegates to `pineguard dev install`. Per-tool `Install-<Tool>.ps1` scripts delegate to it. `tools/.shared/dotenv.ps1` gains `Get-DotEnvVariable`, `Set-DotEnvVariable` and `Test-DotEnvVariable` beside the existing `Import-DotEnv`; every entry script's fixed header imports `.env` first; the CLI loads the same file; `.env.example` is generated from the manifests by `pineguard dev env --example` and lists every variable with its owner and purpose. CI runs the same install path, so local and pipeline never diverge. | Per-tool installers with no single entry point; `Install-Scanner.ps1` per family (Fable's first proposal, withdrawn: the owner wants one root helper for every CLI); `Load-DotEnv` (not an approved PowerShell verb, and `Import-DotEnv` already exists); a hand-maintained `.env.example` (drifts) | Owner, mid-session 2026-09-08: a public library owes contributors one documented path "from local development to integration". |
| D-16 | **`.env` moves to the repository root**: `.env` gitignored, `.env.example` committed and generated, replacing the `.etc/powershell/` location and its hand-written example. Root is the convention every tool, Node's `process.loadEnvFile` and every contributor already expects; `.etc/powershell/` is legacy the standardisation plan flagged as out of scope. `Clean-Root.ps1` allow-lists both names. `Import-DotEnv` keeps its `-Path` parameter so nothing breaks during the move. | Keep the `.etc/powershell/` location (the CLI and every new script would hard-code a legacy path); `.etc/`; `tools/` (a secret store inside the tools tree) | Fable recommendation; owner to confirm. |
| D-17 | **Vendor names only, verified against the vendor.** Every token, endpoint, home, cache, proxy and trust variable in §2.7 is the name the vendor documents, checked on 2026-09-08 against the pages listed at the end of §2.7, with three parallel research passes and independent re-checks of every name that contradicted the request. Corrections to the requested list: `SNYK_CACHE_PATH`, not `SNYK_CACHE_HOME`; `SONAR_TOKEN` and `SONAR_HOST_URL` for the scanners and SonarCloud, with `SONARQUBE_TOKEN`, `SONARQUBE_URL` and `SONARQUBE_ORG` kept only for the SonarQube MCP server, so the repository's scripts switch (T1.07); `NVD_API_KEY` is a user-chosen name for Dependency-Check's `--nvdApiKey` flag and is moot under D-9; `ZAP_API_KEY` does not exist, ZAP takes `-config api.key=`, and ZAP is not adopted; `npm_config_prefix` is the vendor's casing with uppercase accepted; `DOTNET_INSTALL_DIR` is an install-script parameter, not a variable; `DOTNET_SKIP_FIRST_TIME_EXPERIENCE` is obsolete. `GH_TOKEN`, `GITHUB_PERSONAL_ACCESS_TOKEN`, `COPILOT_GITHUB_TOKEN`, `SNYK_TOKEN`, `SEMGREP_APP_TOKEN`, `DOTNET_ROOT`, `DOTNET_CLI_HOME` and `NUGET_PACKAGES` are exactly right. Unverified and flagged in place: the Wiz variables (the vendor site rate-limited every fetch) and Qodana's forward-report variables (not needed with the action, omitted). Federated machines are a first-class case: the manifest carries each tool's relocation variables in `env.homes`, `dev env --relocate <root>` emits them under one writable root, and `dev env --check` verifies writability. | PineGuard-prefixed aliases for vendor variables; keep `SONARQUBE_TOKEN` for the scanner scripts because it is what exists today; a hand-maintained variable list; a single umbrella `PINEGUARD_DEV_ROOT` variable that the scripts expand into every vendor variable (rejected: the vendor variables must be set for the vendor tools to see them from an IDE or a plain shell, so a generator that emits them is right and an alias that hides them is wrong) | Owner, mid-session 2026-09-08: "MUST look these up for specificity NOT make our own", and the federated-customer framing with `C:\Dev` and `C:\Tools`. |
| D-14 | **Cloud-only services are fetch adapters.** `pineguard scan dependabot\|secret-scanning\|code-scanning` pull open alerts through `gh api` into SARIF and summary; they never run a scanner and never upload. `wiz` is a stub until a tenant exists. | Leave cloud-only services out of the CLI (then `scan all` cannot show the whole picture and the skills have nothing to run) | |

## Baselines

To be filled by T1.12, T1.V, T2.V, T3.01 and T3.V.
