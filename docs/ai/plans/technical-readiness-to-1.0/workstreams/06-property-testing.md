<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w06
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W06 — Risk-focused property testing

## Purpose and evidence limit

P3 checks semantic invariants across generated inputs. v1.6 §8 and rubric assertion/adversarial findings support focused properties, but the bounded intake did not identify a dedicated property harness. Do not claim none exists globally or mandate a library merely by name.

Inputs: W01 specifications, W05 independent oracle conventions, W14 risk priorities and scoped test specifications. Output: `docs/reports/readiness-properties.md`. D10/D22 require a Luna inventory and Astra approval of tool, exact project/files, input domains and reproducibility settings before implementation.

## Tasks

| Task | Action | Output |
|---|---|---|
| 06.1 | Inspect existing dependencies/fixtures/generators and identify reusable facilities | Harness inventory, candidate tool tradeoffs and approved path map |
| 06.2 | Derive properties from the reviewed specification rather than current implementation | Property-to-rule/requirement matrix with domain preconditions |
| 06.3 | Design generators and shrinkers emphasizing boundaries and invalid structural inputs | Reviewed domain/seed/shrink policy, including exclusions |
| 06.4 | Implement initial high-risk pilot properties and deterministic regression examples | Reproducible test artifacts and minimized counterexamples |
| 06.5 | Seed a representative semantic defect for each property family | Evidence the property can detect the intended fault class |
| 06.6 | Define pull-request versus scheduled execution based on observed cost | W00 routing/artifact requirements and approved run budgets |

## Candidate properties requiring contract review

Range monotonicity is valid only for the selected range semantics and type domain. Negation/complement is valid only for normally evaluated outcomes. Culture invariance applies only to rules promised invariant. Normalize-then-validate idempotence applies only where normalization is explicitly part of the contract. Conversion round trips require a specified format/domain. Code/path stability applies only to supported mappings.

These candidates are not automatic acceptance requirements. Astra chooses the actual properties after W01, records why each invariant follows, and rejects attractive but false identities.

## Acceptance

Every property records its domain, preconditions, independent rationale, seed/replay information and shrinking behavior. Counterexamples become durable regression fixtures once their intended behavior is decided. Results state generated-case counts and runtime; neither is a substitute for coverage or correctness proof.

Seeded defects fail for the expected reason. Rerunning a captured failing seed reproduces the failure under its recorded environment. The harness does not call the same implementation to calculate expected results or hide failures behind retry-until-pass behavior.

## Boundaries

W06 owns semantic properties and generators; W07 owns hostile-input fuzz campaigns; W09 evaluates assertion sensitivity through mutation. Checkpoint after property review, then after the first reproducible counterexample exercise. Stop for unbounded generation, unstable time/culture, false properties, excessive runtime or unexplained seed-dependent failures. Any new package dependency goes through W13.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md) and [source map](../references/source-map.md). This child remains Planned; no implementation is authorized by this document.
