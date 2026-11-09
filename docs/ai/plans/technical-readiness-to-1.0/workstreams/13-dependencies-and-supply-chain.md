<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w13
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W13 — Dependencies and package supply-chain evidence

> **Status 2026-11-09:** planned; owner authorization is pending. Only the string→DateOnly pilot may be authorized under [the current plan disposition](../plan.md#where-this-plan-stands-2026-11-09) (register D29). The wave procedures and role assignments below are historical proposals, subject to redrafting from pilot evidence and owner approval.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W13 makes dependency and package composition intentional, reproducible and supportable. It evaluates the new product design; it does not preserve old dependency/API arrangements. v1.6 §10 and handoff dependency gates provide review inputs.

Verified anchor: `apps/cli/package.json`. Luna must inventory actual .NET project/package-centralization files, npm workspace manifests, lockfiles, licensing/SBOM/security tooling, package generation and release workflows before choosing paths or commands. Output: `+ docs/reports/readiness-dependencies.md`.

## Decisions and tasks

D23 blocks tool/threshold/exception policy until inventory. D12 governs target support. No lockfile names or existing vulnerability tooling are assumed.

| Task | Action | Output |
|---|---|---|
| 13.1 | Inventory direct/transitive runtime, build, analyzer, generator, test and CLI dependencies | Version/source/license/role/package map with lock/restore behavior |
| 13.2 | Challenge redundant/heavy dependencies against W22/W03 architecture and supported deployment | Astra retain/remove/replace decisions, evidence and global impact |
| 13.3 | Define restore reproducibility, vulnerability/license policy, trusted sources and exception expiry | Approved dependency policy and exact enforcement paths |
| 13.4 | Verify generated package contents/dependencies match intended boundaries | Package inspection evidence, unexpected assets/transitives resolved |
| 13.5 | Route dependency/config changes through W00 and prove deliberate violations fail | Gate fixtures and source/lockfile routing evidence |
| 13.6 | Produce release dependency inventory and consumer/deployment implications | W11/W20/W21 evidence inputs with checked dates |

## Acceptance

Every shipped dependency has an identified purpose and approved source/license/support impact. The new package graph has no unexplained redundant layers or accidental test/build assets. Restore behavior is documented and repeatable using the selected approved mechanisms.

Security findings are evaluated against actual package/version/use/reachability evidence without inventing automatic universal severity cutoffs. Exceptions, if permitted, require owner, rationale, compensating evidence, expiry and effect on the10/10 criterion. An unresolved material risk prevents claiming the affected criterion is complete.

## Controls

Checkpoint after inventory and policy decisions, then after package/gate proof. Luna performs external/database reading and gathers provenance; Sol implements approved tooling; Astra decides tradeoffs. Stop for unexplained package-source changes, speculative package upgrades, copied credentials or blanket suppressions. This workstream may remove dependencies when the best greenfield design warrants it; it does not maintain old package graphs for compatibility.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```powershell
$resolved = (Resolve-Path -LiteralPath $Project).Path
$directory = Split-Path -Parent $resolved
$lock = Join-Path $directory 'packages.lock.json'
if (-not (Test-Path -LiteralPath $lock -PathType Leaf)) { throw 'A reviewed lock file is required for this proposed gate.' }
dotnet restore $resolved --locked-mode
if ($LASTEXITCODE -ne 0) { throw 'Locked restore failed.' }
Write-Output 'PASS: locked restore.'
```

Purpose: complete proposed lock gate requires a reviewed packages.lock.json then runs locked restore, failing if absent/drift. The package-free pilot currently has no reviewed lock and must fail this prerequisite; do not imply repository lock files exist. Actual pins supplied by Luna: xUnit2.9.3/FV11.12.0/ErrorOr2.1.1/TestSDK18.10.1; CLI Node>=22/TS6.0.3/Vitest4.2.2. Status not run.
