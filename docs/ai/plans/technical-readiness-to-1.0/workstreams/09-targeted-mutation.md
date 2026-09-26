<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w09
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W09 — Targeted mutation and assertion strength

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W09 demonstrates that important assertions detect realistic defects. v1.6 §8 supports targeted mutation. The substantial passing execution count and100% coverage requirement do not themselves prove assertions are strong; this workstream complements both.

Inputs: W01 contracts, W05 conformance, W06–W08 tests and W14 risk priorities. No dedicated mutation configuration was established. D10/D22 requires tool/target/file approval. Output: `docs/reports/readiness-mutation.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 09.1 | Inventory existing tools and identify risk-ranked semantic sites | Exact target methods/files, supported frameworks and runner options |
| 09.2 | Select fault models tied to requirements | Mutation catalog: boundary flip, negation, missing check, wrong code, normalization/order changes where applicable |
| 09.3 | Approve bounded operators, exclusions and run scheduling | Astra-reviewed cost/coverage policy; no vanity score target |
| 09.4 | Execute pilot mutations and inspect survivors/timeouts | Reproducible mutant outcomes and tested-assertion mapping |
| 09.5 | Classify survivors as meaningful gaps, equivalent, unreachable under approved domain or tool defects | Explicit rationale with no blanket ignore category |
| 09.6 | Strengthen independent assertions and rerun affected mutants plus validation coverage | Closed meaningful survivors, clean source restoration and W00 artifacts |

## Acceptance

Every targeted contractual fault has a demonstrably failing assertion. Meaningful surviving mutants are resolved or block the affected10/10 criterion. Equivalent/unreachable classifications are individually justified and reviewed; source is not changed solely to game the mutation tool.

Record generated, executed, killed, survived, timed-out and excluded counts with denominator and tool settings. A timeout does not automatically count as a meaningful kill. Coverage thresholds remain100% for validation scope; mutation is a separate evidence dimension, never an excuse to lower coverage.

## Controls

Checkpoint after risk/target selection, after first results and before assertion changes. Use approved isolated mutation facilities so temporary production mutations never enter committed code. Sol owns harness/test changes, Luna supplies context/evidence, Astra reviews survivors. Stop for broad expensive campaigns without value, unexplained runner errors or exclusions hiding gaps. Do not impose an arbitrary universal mutation percentage before the target domain and meaningful fault set are approved.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
static bool SeededBoundaryMutant(int? value) => value is not null && value.Value >= 0;
Checks.Require(SeededBoundaryMutant(0), "mutant accepts zero");
Checks.Require(!PositiveInt.Evaluate(0).Passed, "explicit zero expectation kills mutant");
```

Purpose: explicit zero expectation kills seeded `>=0` boundary mutant; zero must fail canonical and pass mutant. This does not measure a selected mutation engine or score. Tool selection remains D10. Status pending.
