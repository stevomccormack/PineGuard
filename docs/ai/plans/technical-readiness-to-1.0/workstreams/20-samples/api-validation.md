<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w20a
version: 1.0
status: planned
last_updated: 2026-09-26
-->
# W20A — Major API validation sample, scheduled last

## Purpose and entry gate

Demonstrate the newly approved validation contracts in a runnable API consumer after core semantics, structure/manifest,100% validation coverage, lightweight deployment consumers and new public API have passed. This is a large late work item, not a prerequisite for early benchmarking or manifest work.

Inputs: W01/W04/W05 semantics/errors, W11 package/target support, W12 public contract, W14 security boundaries and W20 claim register. Before coding, Astra decides API host style, endpoint scope, validation invocation, failure-to-HTTP mapping, DI lifetimes and async/cancellation behavior from Luna's current repository/platform evidence. No framework, route, serializer or persistence technology is silently selected.

## Tasks

1. Luna inventories existing API samples and conventions. Output an exact source/project/test/doc path map and a decision packet at `docs/reports/readiness-sample-api.md`.
2. Astra approves a minimal but representative scenario covering valid input, field failures, conversion failures, nested/collection behavior if supported, and operational/cancellation outcomes. Select only approved package/target combinations.
3. Sol builds the host and shared application boundary without duplicating rule truth or adding app-specific validation hacks. Every new validation path goes through the canonical rule structure.
4. Exercise real requests through the selected HTTP pipeline; assert documented response status/body/error mapping, invocation actually occurring and no sensitive attempted-value disclosure.
5. Verify async and platform caller-specific behavior where in scope; absence of generated metadata/registration must be detected rather than silently skipping validation.
6. Run package-shaped build/start/request/shutdown instructions from a clean approved environment, record artifacts and audit validation coverage.
7. Astra reviews global consistency, clarity, documentation and integration evidence; commit exact owned files.

## Acceptance and boundaries

The sample is independently runnable with exact commands and deterministic expected responses. The chosen host correctly invokes validation; unit tests alone do not establish HTTP integration. Every validation branch meets the hard coverage contract. Non-validation hosting infrastructure is tested proportionately;100% is not imposed on unrelated app code.

Do not add database/authentication/cloud deployment/UI machinery unless needed for the approved validation scenario. The project is an educational/integration proof, not a claim of production readiness for unrelated infrastructure.

## Estimate and controls

Engineering-equivalent effort16–32 hours; active Sol/Luna tool-and-model work5–12 hours; Astra/user review2–4 hours, low confidence until exact scope is approved. Break into2–4-hour engineering-equivalent packets with10–20-minute requested agent bounds, checkpoints and role-specific tokens from the [execution protocol](../../references/execution-protocol.md). These estimates are not wall-clock promises or hard tool limits.

Stop for core contract gaps, new API choices, scope growth or duplicate validation logic; return the issue to its owning workstream. W20A is accepted separately from W20B/W20C.
