<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w02
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W02 — Rule manifest and completeness

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and baseline

P1 creates auditable completeness using the existing audit machinery, as required by v1.6 §6. It must not introduce a second manual method/code/support list or Supports* booleans. Start with the three approved pilot families and widen only after their drift checks work.

Verified inputs: `apps/cli/src/audit/engine.ts`, `apps/cli/src/audit/rules/layer-parity.ts`, `must-codes.ts`, and `src/PineGuard.Core/Codes/MustCodes.Email.cs`. The exact existing extraction schema and extensibility mechanism remain to be inventoried.

## Decisions and outputs

D07 blocks schema/lowering implementation until Astra approves provenance, representation and reconciliation rules. Output evidence: `docs/reports/readiness-manifest-completeness.md`. Exact manifest/generator/fixture file paths are selected from the Luna inventory under D22, not invented as existing files.

The design review must distinguish authoritative rule identity from public method aliases, overloads, code provenance, supported surface projections and test-case references. Absence/not-applicable/unsupported/unmapped must be unambiguous without a competing list of booleans.

## Tasks

| Task | Action | Deliverable |
|---|---|---|
| 02.1 | Map current audit inputs, extraction, suppressions, findings and code attribution | Provenance graph and gap list in report |
| 02.2 | Map pilot semantics from W01 onto actual methods/overloads/codes/surfaces | Pilot inventory with sources for every relationship |
| 02.3 | Compare options for extending existing extraction, generated metadata and reviewed exceptions | Astra D07 selection; maintenance/compatibility tradeoffs |
| 02.4 | Implement approved pilot representation through existing audit mechanism | Single authoritative path from rule contract to derived audit views |
| 02.5 | Seed omitted method, orphan code, wrong alias, duplicate identity and unsupported projection claims | Expected diagnostic evidence with stable actionable locations |
| 02.6 | Define incremental rollout and exception expiration policy | Approved family-by-family expansion checklist, no silent global gate switch |
| 02.7 | Execute approved rollout across every declared rule and projection, reconciling public inventory and independent contract/test references | Complete executable manifest and no unexplained orphan, duplicate or missing declared entry at final acceptance |

## Acceptance

Every pilot public entry point has justified coverage or a reviewed not-applicable explanation. Every code relationship resolves to source provenance; do not require a synthetic distinct code for each surface. Add/remove/rename changes trigger a deterministic finding when required metadata or evidence is missing. A clean run after seed removal proves the detector does not leave stale state.

No parallel hand-maintained canonical inventory is accepted. Generated outputs must be reproducible and clearly distinguished from authored inputs. The report states whether source-generated artifacts, overloads, aliases and conditional target-framework APIs are included.

## Boundaries and checkpoints

W02 owns completeness metadata and audit reconciliation, not semantic truth (W01), failure design (W04) or runtime implementation architecture (W03). Checkpoint after current extraction inventory, after schema decision and after seeded drift proof. Stop when the proposed manifest duplicates an existing authority, forces public API changes, or assumes all rules project to every surface. Gate activation belongs to W00 after W05 validates the expectation contract.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
internal sealed record Descriptor(string Id, string Code, Func<int?, Verdict> Evaluate);
internal static class DerivedManifest
{
    internal static readonly Descriptor[] Rules =
        [new(PositiveInt.Id, PositiveInt.FailureCode, PositiveInt.Evaluate)];
    internal static string[] Differences(IEnumerable<string> expectedIds) =>
        expectedIds.Except(Rules.Select(x => x.Id)).Select(x => "missing:" + x)
            .Concat(Rules.Select(x => x.Id).Except(expectedIds)
                .Select(x => "orphan:" + x)).Order(StringComparer.Ordinal).ToArray();
}
```

Purpose: proposed D07 descriptor points to the single static implementation. Input expected ID `illustration.number.positive` → no differences; seeded expected `illustration.absent` → exactly missing:illustration.absent and orphan:illustration.number.positive. No current public manifest is claimed. Status pending.
