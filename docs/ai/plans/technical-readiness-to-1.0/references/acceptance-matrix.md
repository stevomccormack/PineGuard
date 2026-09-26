# Acceptance matrix for each 10/10 criterion

The target is each technical row at10/10; no weights or aggregate. These are proposed observable exit requirements to be confirmed through formal review before activation. Completion is evidence-based within explicitly declared scope, not a claim that all possible bugs are impossible.

| Rubric row / baseline | Required 10/10 evidence | Owners | Blocking condition |
|---|---|---|---|
| Semantic/failure7 | Reviewed independent rule contracts; all ambiguities decided; input/output/operational/failure semantics consistent through actual projections | W01/W04/W05/W18/W19 | Undecided normative behavior or unexplained mismatch |
| Architecture/cross-surface8 | New coherent static structure, approved taxonomy, one executable truth, executable manifest completeness, no duplicate catalogs/local outliers | W22/W03/W02/W05 | Duplication, missing projection provenance or unreviewed abstraction |
| Assertion/adversarial7 |100% audited validation line+branch coverage plus independent conformance, properties, bounded fuzz/differential and meaningful mutation sensitivity | W00/W05–W09 | Missing report, uncovered validation branch, false oracle or meaningful unresolved survivor |
| Consumer API/composition8 | Best globally consistent new API; explicit lifecycle/composition/caller behavior; runnable representative use cases and late major samples in declared scope | W12/W19/W20A–C | Redundant/confusing API, undocumented invocation or unusable sample |
| Security/data6 | Threat/claim matrix, regex/resources policies, privacy proof and adversarial regressions | W14–W17/W07 | Material unresolved threat, unsupported security claim or leakage |
| Performance/resource4 | Early baseline, measured new structure, approved quantitative budgets, reproducible allocation/runtime evidence and enforced regressions | W10/W15/W16/W17/W19 | No baseline/budget, unexplained regression or unbounded supported operation |
| API/package/deployment6 | Approved NEW contract drift gates; intentional dependency/package graph; actual supported trimmed/native consumers | W11–W13 | Old-release migration work is not required; missing actual consumer proof or unintended public/package drift blocks |
| CI/tooling/release5 | Correct routing incl self-tests, fail-closed expected artifacts/coverage, seeded gate proof, reproducible release evidence | W00/W21 | A required job/artifact can silently disappear or checks cannot catch seeded defects |
| Docs/maintainability7 | Current claims/source support, executable examples, links/format quality, coherent naming/DRY/SOLID and portable evidence | W22/W20/W21 | False/stale claims, unexplained duplicates, broken reference or unreviewed debt |

## Evidence record schema

Each gate records: criterion/workstream; exact supported scope; source/spec decisions; artifact path/hash; revision and dirty-tree state; environment/command; expected versus actual result; negative-test proof;100% validation scope where applicable; reviewer; confidence/limitations; remaining blockers; timestamp. A green checkbox without these facts is not a release gate.

No field may say 'N/A' solely because evidence is difficult. Not-applicable scope requires a reason tied to the new supported product contract. Scope removal cannot hide an explicitly requested validation surface or change the user's hard coverage requirement.

The original rubric remains unchanged; publish reassessment separately. If any row lacks sufficient evidence, report its actual remaining gap and do not declare the entire technical target achieved.
