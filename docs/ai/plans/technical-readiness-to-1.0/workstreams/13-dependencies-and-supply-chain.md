<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w13
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W13 — Dependencies and package supply-chain evidence

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W13 makes dependency and package composition intentional, reproducible and supportable. It evaluates the new product design; it does not preserve old dependency/API arrangements. v1.6 §10 and handoff dependency gates provide review inputs.

Verified anchor: `apps/cli/package.json`. Luna must inventory actual .NET project/package-centralization files, npm workspace manifests, lockfiles, licensing/SBOM/security tooling, package generation and release workflows before choosing paths or commands. Output: `docs/reports/readiness-dependencies.md`.

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
