<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w22
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W22 — Early naming, taxonomy and shared abstractions

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and priority

W22 runs EARLY despite its numerical ID: dependencies, not filenames, determine order. This workstream applies the greenfield instruction by choosing the best globally consistent naming/taxonomy and shared abstractions before locking the new rule structure and manifest.

Existing APIs impose no backward-compatibility constraint. DRY/SOLID, clarity and formatting hygiene are mandatory. Avoid speculative abstractions: a shared mechanism must serve a demonstrated responsibility.

Inputs: BRAIN specifications/precedents, repository-wide source/project/package/rule/surface inventory, W01 semantic questions and official-source comparisons. Output: docs/reports/readiness-taxonomy-abstractions.md, then the approved naming/structure specification at a path selected under repository conventions.

## Tasks

| Task | Action | Output |
|---|---|---|
| 22.1 | Luna inventories naming, namespaces, folders/packages, rule families, overloads and duplicate semantics repository-wide | Map covering source/tests/docs/generators/CLI |
| 22.2 | Astra defines rule, predicate, clause, guard, result/failure, input/output, projection and manifest responsibilities | Reviewed vocabulary and separation-of-concerns invariants |
| 22.3 | Evaluate precedents and alternative naming/composition/shared boundaries | Decisions and rejected alternatives; no legacy-shim requirement |
| 22.4 | Approve target static structure and exact path map with W03/W02 | New structure/manifest interface and runtime/construction responsibilities |
| 22.5 | Sol applies coherent changes in bounded vertical slices | Every impacted surface updated; independent behavior review |
| 22.6 | Detect residual aliases/duplicates/outliers/formatting issues/abandoned structures | Audit evidence,100% validation coverage and benchmark deltas |

## Acceptance

A new rule has one obvious home, consistent identity and one executable source of truth. Methods, codes, namespaces, packages, manifest, fixtures and docs follow the approved vocabulary. Necessary exceptions are justified; accidental local conventions are eliminated.

No interface/factory/generator exists solely for hypothetical future use. Duplicated semantics are corrected at their shared root. Consolidation preserves the new approved contract, not old shapes.

## Controls

Checkpoint after whole-repository inventory, taxonomy approval and each structural slice. Astra decides/reviews; Sol codes; Luna performs all reading and BRAIN retrieval. Stop for local fixes without blast-radius review, missed consumers/tests/docs or abstractions without present need.

W01, W10 early benchmarks and W19 lifecycle characterization may proceed concurrently. W03/W02 depend on W22 approval. The new structure/manifest is an early committed design objective, not optional research.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
