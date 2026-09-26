<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w11
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W11 — Lightweight trimmed and native package consumers

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and sequencing

W11 provides early, lightweight consumer evidence once the new contracts/package shape are coherent. These smoke consumers are prerequisites for accurate support claims and are distinct from the large API/DDD/Clean Architecture projects scheduled last.

v1.6 §9 and the handoff call for actual consumer execution. A successful publish or analyzer warning suppression alone is not deployment proof. No exact existing consumer project was verified.

Inputs: package inventory, W01/W03/W04 contracts, W00 evidence contract, W13 dependency facts. D12/D22 require exact target/package/project/path decisions. Output: `docs/reports/readiness-deployment-consumers.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 11.1 | Inventory source projects/packages and supported candidate TFMs/RIDs/platforms | Candidate support matrix with source/published distinction |
| 11.2 | Select minimal representative consumer shapes covering static, object and integration paths only where supported | Astra-approved scope and exact project/files/commands |
| 11.3 | Build package-shaped artifacts and consume them through the approved restore path | Package IDs/versions/hashes and restore provenance |
| 11.4 | Publish trimmed and native candidate variants with explicit warnings/policy | Complete publish logs and unsuppressed finding dispositions |
| 11.5 | Execute the resulting binaries and validate both success/failure behavior under deployed conditions | Exit/results and semantic assertions, not publish-only evidence |
| 11.6 | Route repeatable supported targets into CI and record unsupported/unverified variants honestly | W00 artifact contract and W20 support-table inputs |

## Acceptance

Every affirmative trimmed/native claim has an executed consumer for the exact declared package, target framework, runtime/platform and feature subset. Consumers must load/use relevant paths; an empty application or unused reference proves nothing. Dynamic discovery, reflection, generated metadata and integration registration are exercised where present.

Missing generator metadata and source-generation claims need direct consumer verification. A compiler generator or generic static API is not itself proof of trimming/native safety. Warnings are fixed at the shared cause or explicitly evaluated against the precise supported scope; blanket suppression does not satisfy10/10.

## Controls and handoffs

Checkpoint after support selection, after package restore and after executed publish. Sol owns approved consumers; Luna gathers commands/logs/source context; Astra decides scope/support. Stop for unsupported toolchain assumptions, local project-reference shortcuts that bypass packaging, unexplained warnings or a target claimed without execution.

W11 tests the newly chosen product contract. There is no obligation to support old releases, retain old API shapes or build migration shims. W20's large sample projects reuse the established interfaces and evidence; they must not be used to postpone the minimal deployment proof.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
