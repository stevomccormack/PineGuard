<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w07
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W07 — Bounded fuzz testing and corpus governance

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and scope

P3 explores hostile inputs at identified parsing/validation boundaries. The original handoff suggests fuzzing; v1.6 §8 permits risk-focused tooling without mandating a named tool. No exact fuzz harness was established by the bounded intake.

Inputs: W01 contracts, W14 threat model, W15 regex semantics, W16 approved resource policy and existing test specifications. Output: `docs/reports/readiness-fuzzing.md`; approved corpus/harness paths are selected under D10/D22.

## Tasks

| Task | Action | Output |
|---|---|---|
| 07.1 | Inventory candidate parsers, regex boundaries, normalization and graph traversal entry points | Prioritized target list with attacker control and expected outcomes |
| 07.2 | Compare reusable harnesses/tools and isolation needs against repository/platform constraints | Astra-approved harness and exact ownership; dependency review |
| 07.3 | Define bounded input sizes, execution time/memory, campaign scheduling and reproducible seeds | Campaign contract tied to W16; no arbitrary public API limit |
| 07.4 | Seed a corpus from independent edge cases, prior defects and adversarial encodings | Documented corpus provenance and synthetic/non-sensitive contents |
| 07.5 | Run campaigns, minimize interesting failures and classify invalid-input outcomes versus bugs | Reproduction bundles and Astra triage decisions |
| 07.6 | Add approved minimized regressions and route smoke/scheduled campaigns through W00 | Artifact manifest, retention and failure escalation policy |

## What to detect

Unspecified exceptions, nontermination/resource amplification, inconsistent normalization, deterministic crashes, unstable outputs and contract violations are candidate signals. An ordinary documented invalid result is not a bug. A timeout or cancellation is classified according to D05/W16, not automatically coerced into pass/fail semantics.

Use targeted inputs such as long repetitive strings, unusual Unicode and malformed parse inputs only for relevant supported domains. Do not publish an exploit claim without a reproduction, threat preconditions and a demonstrated consequence.

## Acceptance

The campaign has finite, recorded bounds and produces replayable artifacts. A known seeded fault is detected and minimized, and a clean smoke corpus runs deterministically. Every retained finding has a disposition: confirmed defect, specification question, harness defect, duplicate or expected behavior. Unknown findings block the affected claim; a fixed campaign duration with no findings is not proof of absence.

Sensitive real user data is excluded from corpora and logs unless separately authorized under an approved handling policy. Store reproduction inputs only where permitted.

## Controls

Checkpoint after target/tool selection, after corpus review and after each campaign. Sol executes approved harness work; Luna performs reads and evidence collection; Astra triages contract/security decisions. Stop on runaway resource use, unexpected network/process access, personal-data capture or unsupported tool/platform assumptions. W07 never expands supported public input limits by implication.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.
