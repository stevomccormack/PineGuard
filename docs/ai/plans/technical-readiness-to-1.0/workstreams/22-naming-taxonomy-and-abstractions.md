<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w22
version: 1.4
status: planned
last_updated: 2026-11-09
-->
# W22 — Naming, taxonomy and shared abstractions (after the pilot)

> **Status 2026-11-09:** not activated. The plan-gated council ran on 2026-11-09 and ruled that this plan is not activated as written; the only unit that may start, on the owner's explicit go, is the string→DateOnly pilot PR described in [plan.md § Where this plan stands](../plan.md#where-this-plan-stands-2026-11-09) and register decision D29. The completion gate, Astra/Luna/Sol roles, checkpoints and D22 inventory steps below apply to a later wave activation, not to the pilot PR.

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and priority

W22 runs AFTER the string→DateOnly pilot (council 2026-11-09): names are the output of the shape, D01 makes renames free, and the pilot's owner-signed names table is W22's first input. This workstream applies the greenfield instruction by choosing the best globally consistent naming/taxonomy and shared abstractions before rolling the proven rule structure out beyond the pilot.

Existing APIs impose no backward-compatibility constraint. DRY/SOLID, clarity and formatting hygiene are mandatory. Avoid speculative abstractions: a shared mechanism must serve a demonstrated responsibility.

Inputs: BRAIN specifications/precedents, repository-wide source/project/package/rule/surface inventory, W01 semantic questions and official-source comparisons. Output: `+ docs/reports/readiness-taxonomy-abstractions.md`, then the approved naming/structure specification at a path selected under repository conventions.

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

After a passing pilot, W01, W10 and W19 may proceed concurrently with W22. W03's premise is tested by the pilot PR before W22 runs; the broader W03 rollout and W02 depend on a passing pilot and then on W22 approval. The new structure/manifest is an early committed design objective, not optional research.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Approved test-project boundary

The [approved testing project layout](../references/test-project-layout.md) is authoritative for this workstream's project identity, folder responsibility and execution kind. Use its workstream mapping; do not create one project per testing technique or move this crosscutting work into existing library unit suites. Reuse PineGuard.Testing fixtures and typed projections. Required projects and artifacts must exist before the applicable implementation gate can close; Planned status is not execution evidence.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

Purpose: concrete source mapping plus D07 candidate descriptor ID; D22 paths remain open. Current `IsPositive`→`NumberRules.IsPositive`→`Must.Be.Positive`→complement `Guard.Against.ZeroOrNegative`→`MustCodes.Number.Sign.NotPositive`; FV `.Positive()` and DA `[PositiveNumber]` are source-backed adapters. The candidate teaching descriptor is deliberately named illustration.number.positive and is not the selected manifest taxonomy.

```csharp
var descriptor = DerivedManifest.Rules.Single();
Checks.Require(descriptor.Id == "illustration.number.positive" &&
    descriptor.Code == "illustration.number.not-positive", "candidate name mapping");
```

Input descriptor→exact candidate literals; no CLI vocabulary file becomes an executable validation manifest. Status pending.
