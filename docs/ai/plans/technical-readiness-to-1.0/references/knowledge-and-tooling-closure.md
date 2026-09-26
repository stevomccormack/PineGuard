# Mandatory knowledge, documentation and tooling closure

This binding project requirement applies to **every workstream, every major task block, every major section/wave and each large sample project**. A feature or passing test is insufficient for completion. BRAIN, relevant Markdown, scripts, tools, tests and indexes must be aligned; lessons and decisions must be saved; Astra must complete a full review and feedback loop.

This document implements the user's requirement through existing BRAIN authorities. It does not create a parallel global rule hierarchy or implement future supporting scripts during this planning-only amendment.

## Canonical authority and placement

- `docs/ai/meta/taxonomy.md` §Core Concepts / Memory (reviewed lines93–99): portable learned-pattern files live under `docs/ai/memory/*.md`, one per applicable subagent. Memory records observations, not normative rules; hardened patterns move to a Spec and leave a memory pointer. Adapter memory is seeded from the portable baseline.
- `docs/ai/specs/orchestration.md` §Learn from corrections (153–158): record a user correction as constraint plus rationale in the appropriate spec/rule/memory location; review relevant learned patterns at session start.
- `docs/ai/specs/spec.md` §9 (443–456): assess child-spec applicability, lift genuinely cross-cutting guidance to the root specification and replace duplicates with pointers.
- `docs/ai/rules/global.md` (3–7) and taxonomy §Rules: rule files summarize or point to authoritative specs rather than restating them.

Use existing applicable agent memory homes. No planner/execution memory file or template/index was found in the inspected scope; do not place planning lessons in an unrelated agent's memory. Project-specific observations belong in this plan's [closeout ledger](closeout-ledger.md). Any new durable home or repo-wide normative promotion requires the appropriate Astra scope/cascade decision.

## Required loop for each closure unit

Before dispatch, assign a stable unit ID such as W03-B02, its parent workstream and wave, exact owned paths and review checkpoint. Every major block closes independently; parent-wave consolidation does not postpone documentation, memory or tooling maintenance until the end. The next agent receives the accepted closure packet plus only refreshed changed-context excerpts.

### Supporting-tool authority

Luna loads `docs/ai/rules/tools.md` lines 5–13, `docs/ai/specs/tools/spec.md` header and §§1–2, `tools/README.md` lines 30–32 and `docs/ai/meta/tooling.md` §Single Source of Truth before tool changes. Respect existing tool specifications, root/per-tool READMEs, Test-/Sync- naming and shared-module structure. Inventory/reuse existing registry and Pester/Test-Tools.ps1 mechanisms; add or update required self-tests/help/registration/docs and run the actual relevant checks. Do not create a duplicate registry or assume an unverified command. `docs/ai/meta/adapter-surfaces.md` §5 supplies the affected-adapter cascade. The source map records exact references and the tools-review precedent.

1. **Refresh changed context.** Luna reads relevant BRAIN, prior lessons and affected source/docs/tooling with revision plus dirty-file hashes. Reuse valid cached packets, but invalidate changed inputs and downstream conclusions. The orchestrator coordinates; Luna reads; Sol implements; Astra decides/reviews.
2. **Map the complete impact.** Before edits, record exact source/contract changes and every affected Markdown file, spec, rule pointer, adapter, manifest, test, script, tool, workflow, sample and index. Search for duplicate or contradictory guidance. Broad 'docs updated' statements are insufficient.
3. **Maintain supporting machinery.** Build or update required scripts/tools as part of the feature's scope; test and run them using approved actual repository commands. Demonstrate that they catch the relevant failure and accept valid input. An existing tool is acceptable only when its current version was run and its suitability verified.
4. **Synchronize knowledge.** Update every relevant .md file and index, save decisions/open issues, and capture concrete lessons with evidence and outcome. Record an explicit 'no new durable lesson' conclusion with reviewed scope if appropriate; never omit the check silently.
5. **Promote rules correctly.** Distinguish observation, project constraint and reusable normative rule. Record constraint+rationale; use the authoritative Spec and existing rule/adapter/index cascade at the narrowest valid scope. Replace duplicates with pointers. Do not copy the same hard rule into multiple competing sources.
6. **Full Astra review.** Review implementation or planning outputs, tests, coverage, semantic/architecture consistency, docs/BRAIN, scripts/tools, source freshness and owned diff. Log accepted and rejected feedback with rationale and exact task/file references.
7. **Fix and recheck.** Sol addresses approved code/tool changes; Astra authors/reviews plan decisions; Luna supplies refreshed evidence. Rerun affected checks, review feedback again and reopen the unit when evidence changes. Do not count an unanswered finding as accepted.
8. **Persist and commit.** Save the completed closure packet/ledger and relevant durable knowledge. Stage only owned artifacts after staged-diff/ownership review. A coherent commit records the unit and evidence; future squash includes only this task's approved commits.

## Exact impact matrix

Create one row per affected relationship; enumerate concrete paths rather than just directory names.

| Source/decision + hash | Affected .md/spec/rule/adapter/index | Script/tool/workflow | Test/check command | Expected output/artifact | Owner/status |
|---|---|---|---|---|---|
| Exact source path or decision ID | Every required exact path, or justified none | Exact implementation/config paths | Actual command from verified context | Result path/hash and pass/fail contract | Role and blocker |

A reasoned not-applicable entry names the reviewed scope and why no update or tool is needed. Missing machinery, stale generated docs, unrun tools or undocumented relevant behavior blocks closure. No blanket N/A or postponed-supporting-script promise can close an implementation feature.

## Ready-to-use closure record

Copy this record into the closeout ledger or link a bounded per-unit record from it:

- Unit ID, workstream/wave/major block, intent and current status:
- Revision, dirty-tree scope, relevant BRAIN/source hashes and cache invalidations:
- Completed outputs and exact owned paths:
- Impact matrix and all relevant Markdown/index updates:
- Required supporting scripts/tools: built/updated/reused, exact paths/version, tests, run commands, artifacts; reasoned N/A where appropriate:
- Validation/conformance/coverage/benchmark/deployment checks required by this unit, results and limitations:
- Lessons: observation, evidence, outcome, reusable scope and canonical destination; or explicit no-new-durable-lesson conclusion:
- User correction/hard requirement: constraint, rationale, canonical spec/rule/memory/plan destination and cascade/pointer changes:
- Decisions made, open issues, deferred scope, owners and blocking dependencies:
- Full Astra review reference and feedback-log entries:
- Fixes/rechecks, reopened items and final reviewer acceptance:
- Ownership/staged-diff verification, immutable-source exceptions and exact commit IDs:
- Next context-refresh trigger and next checkpoint:

## Feedback log

| Finding ID | Reviewer + unit/revision | Finding / affected paths | Accepted or rejected + rationale | Required fix/check | Recheck evidence | Final state / reopen trigger |
|---|---|---|---|---|---|---|
| Stable ID | Astra review identity | Concrete issue | Explicit reason, never silence | Exact task/command | Artifact/hash | Open, fixed, rejected-with-rationale or reopened |

Closure requires all blocking findings fixed and rechecked, or a reviewed finding rejection grounded in evidence. A valid deferred item remains visible with owner and impact; it cannot be deferred while claiming a dependent gate complete. The reviewer explicitly accepts the knowledge/tooling closure as well as the feature.

## Planning-session application

**PROMOTE-01 — named future decision:** Astra resolves durable scope at Wave A's first closure review. Candidate normative destination is `docs/ai/specs/orchestration.md` for execution/closure guidance; use root `docs/ai/specs/spec.md` only if §9's cross-cutting applicability test is met. Luna supplies existing-rule/duplicate/applicability evidence. On approval, cascade affected child specs, Rule pointers, agents/skills/workflows/adapters and indexes; replace duplicates with pointers. Otherwise record a reasoned no-promotion decision while retaining this binding project constraint. This task has a named owner, destination, trigger and acceptance; it is not an indefinite unknown or permission to edit unrelated global rules now.

This amendment does not build future implementation scripts. Its present supporting checks are documentation link/metadata/ownership/diff/source-integrity checks using existing verified tooling. The publisher records actual final results and commit in the ledger after execution; unrun checks stay pending. Current concrete session lessons are already saved in [closeout ledger](closeout-ledger.md), with the role/scope limits above.
