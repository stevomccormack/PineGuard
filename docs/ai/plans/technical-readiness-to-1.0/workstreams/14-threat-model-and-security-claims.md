<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w14
version: 1.1
status: planned
last_updated: 2026-09-26
-->
# W14 — Threat model and defensible security claims

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W14 specifies what security-relevant validation does and does not establish, based on the actual new contracts and consumer boundaries. Rubric security/data is6/10 with medium confidence; v1.6 §8 and the handoff call for formalized claims and hostile evidence.

Inputs: W01 OWASP reproduction and semantics, W03 call paths, W04 failure/privacy contract, W07 fuzz priorities, W15 regex and W16 resource policy. Verified anchors: `src/PineGuard.Core/Rules/OwaspRules.cs`, `src/PineGuard.MustClauses/MustOwaspClauses.cs`, `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs`. Output: `docs/reports/readiness-threat-model.md`.

## Tasks

| Task | Action | Output |
|---|---|---|
| 14.1 | Identify assets, actors, trust boundaries, attacker-controlled inputs and deployment assumptions | Threat model with concrete call/consumer boundaries |
| 14.2 | Inventory security-related rule names/messages/docs and inferable claims | Claim register linked to source and current evidence |
| 14.3 | Reproduce specific candidate concerns without generalizing to untested composites | Minimal demonstrations, preconditions, consequence and uncertainty |
| 14.4 | Decide names/contracts/claim language consistent with actual guarantees | Astra decisions and W22/W01 changes where needed |
| 14.5 | Map threats to independent cases, fuzz/regex/resource controls and privacy tests | Threat-to-mitigation-to-proof matrix |
| 14.6 | Review residual limitations and package/sample documentation | Scoped approved claims for W20/W21 |

## Required distinctions

Validation is not automatically sanitization, encoding, authorization or a complete injection defense. Any security name or documentation must identify the boundary it addresses and the assumptions it requires. A source pattern or scanner finding is not proof of exploitability; conversely a green test suite is not a threat model.

The trimming/newline observation must be reproduced with the implicated individual/composite paths and returned value before assigning a consequence. Do not label every OWASP path vulnerable or safe based on one observation. Review leakage through attempted values, messages, paths, exception payloads and diagnostics.

## Acceptance

Every security-relevant affirmative claim has explicit scope, threat assumptions and executable evidence. Important threats have a mitigation or are outside a clearly declared supported boundary; unresolved material threats block the affected10/10 security claim. Test data is synthetic unless an approved handling policy permits otherwise.

## Controls

Checkpoint after threat/claim inventory and before changing behavior or public claim language. Astra approves security semantics; Sol implements approved shared fixes; Luna gathers source/research evidence. Fix root causes across all impacted projections, not a single failing test path. New limits/regex policies belong to W15/W16 and must not be guessed inside a security test.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.
