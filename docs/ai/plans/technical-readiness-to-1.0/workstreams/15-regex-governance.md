<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w15
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W15 — Regex semantics and governance

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose and baseline

W15 resolves regex-specific correctness/resource behavior early where it blocks W01, then supplies P3 enforcement. Source-only observations: `src/PineGuard.Core/Rules/StringRules.cs` exposes raw regex timeout behavior and `src/PineGuard.MustClauses/MustStringClauses.cs` Match/NotMatch lack a verified mapping; built-in regex timeout behavior is anchored by `src/PineGuard.Core/Rules/Owasp/OwaspRegex.cs`. Reproduction remains necessary.

Inputs: W01 contracts, W14 threat priorities, W10 early measurements and W16 general resource policy. Output: `docs/reports/readiness-regex.md`.

## Decisions and tasks

D05/D14 require Astra answers before implementation: invalid-pattern versus invalid-value behavior; timeout/operational outcome; negation; built-in versus caller patterns; engine/options/culture; timeout source and configurability; resource defaults.

| Task | Action | Output |
|---|---|---|
| 15.1 | Inventory regex use, source-generated/compiled/interpreted paths, patterns/options/timeouts and public entry points | Complete regex governance map with exact owned paths |
| 15.2 | Reproduce Match/NotMatch invalid-pattern/timeout behavior and built-in cases | Environment-specific independent outcome evidence |
| 15.3 | Decide a globally consistent outcome taxonomy and pattern/timeout ownership policy | Astra records; no timeout-to-success negation accident |
| 15.4 | Implement shared policy at the correct abstraction and update every impacted surface | Global diff, independent cases and100% scoped coverage |
| 15.5 | Evaluate hostile patterns/inputs under bounded measurements and fuzz cases | W07/W10 evidence and approved finite runtime policy |
| 15.6 | Enforce inventory/review for newly introduced patterns/options and publish truthful semantics | CI/audit checks, deliberate missing-policy proof and documentation |

## Acceptance

Every supported regex path has an intentional finite-resource policy or a specifically approved boundary that states what can be controlled. A timeout is not silently converted into ordinary non-match, especially under NotMatch, unless that is the explicitly reviewed semantic decision. Invalid patterns/configuration and normal invalid inputs are distinguished.

Engine changes, nonbacktracking choices, generated regex and caches require measured semantics/performance/deployment evidence. Do not assume an engine option supports every pattern. Cache bounds/thread-safety, if a cache exists or is proposed, require W16/W19 review.

## Controls

Checkpoint after inventory/reproduction, after semantic decision and before broad migration. Sol fixes the shared cause; Luna supplies source and official runtime documentation; Astra decides policy. Stop for arbitrary timeout constants, blanket exception catching, local special-case wrappers, unbounded pattern caches or claims extending beyond tested engine/target behavior. W16 owns cross-operation limits; W15 owns regex policy and contributes its measurements.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
