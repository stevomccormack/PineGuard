# Mandatory quality constitution

> **Scope note (2026-11-09):** this reference governs wave activation. The council verdict of 2026-11-09 ([transcript](../../technical-readiness-to-1.0.council-transcript.md), register D29) takes the string→DateOnly pilot PR out of its scope: the pilot uses the tracker page, one Fable code review per PR and the owner's name sign-off. The role names Astra/Luna/Sol describe the planning-era protocol; the owner's current model policy is Fable for judgment and review, Opus for implementation, Sonnet for reads.

**Essential maintenance rule:** every workstream, major block, major section and wave must keep BRAIN, all relevant Markdown/indexes and required scripts/tools aligned; save lessons/decisions; and complete full Astra review → feedback → fix → recheck before closure. The rationale is that working code without current execution knowledge or tested support machinery is incomplete. Apply the [closure contract](knowledge-and-tooling-closure.md), impact matrix and feedback log; record no-new-lesson and reasoned N/A outcomes explicitly. Promote reusable hard rules through existing canonical Specs and pointer/cascade rules, not a duplicate guidance hierarchy.

This applies to every workstream/task, including children drafted before this reference. Current explicit user requirements supersede inherited v1.6 backward-compatibility and late-benchmark priorities.

1. **Greenfield contract.** Choose the best new design. No old-release compatibility work, shims, migration scaffolding or preservation of redundant APIs is required.
2. **Global consistency.** Load BRAIN precedents first; inventory repository-wide blast radius before every fix. Source, tests, generated projections, CLI audits, packages, names and docs must agree.
3. **One executable truth.** Correct shared root causes and use a coherent static rule structure/manifest. Do not copy conditions into wrappers, add a second metadata list or patch only a failing local example.
4. **DRY/SOLID with restraint.** Separate real responsibilities and remove duplication. Do not invent interfaces, factories, graphs or frameworks for hypothetical future use.
5. **Code quality and hygiene.** Follow approved naming/taxonomy, formatting, analyzers and repository conventions consistently. Eliminate abandoned code, misleading comments, accidental public exposure and one-off local patterns.
6. **Independent proof.** Expected semantics come from reviewed specifications. Coverage, conformance, mutation sensitivity, performance and deployment are different evidence dimensions; no one substitutes for another.
7. **Hard validation coverage.** Require100% line and100% branch in the audited validation denominator across every declared validation surface. No new exclusions, weaker thresholds, skipped artifacts or hidden branches to achieve green.
8. **No hidden failures.** Fail closed on missing required evidence. Resolve findings at the shared cause; do not suppress, retry or relabel unexplained failures into success.
9. **Explicit decisions.** Astra resolves semantic/API/privacy/resource/support questions. A blocked decision stays visible while independent work continues.
10. **Every technical rubric row10/10.** Demonstrate each agreed criterion in declared scope. No aggregate score may conceal a gap; no claim of mathematical perfection or unsupported superiority.

## Required review packet for each change

Also require the stable block/wave ID, exact docs/BRAIN/tools impact matrix, script/tool inventory and build/update/test/run evidence, saved lesson destination/outcome, durable-rule promotion disposition, affected indexes/adapters, accepted/rejected feedback and recheck evidence. The [closeout ledger](closeout-ledger.md) records these outcomes. A material omission rejects closure even when feature tests pass.

Include: relevant BRAIN/source hashes; exact task/decision IDs; all affected layers/files; duplicate/root-cause search findings; alternatives and chosen abstraction; code/test/docs/manifest consistency;100% coverage denominator/results; relevant conformance/bench/deployment proof; formatting/analyzer checks; limitations; exact owned diff and staged paths.

A reviewer rejects a local fix whose root cause is shared elsewhere, an invented abstraction without present benefit, or a result supported only by passing old tests. Where a decision removes an obsolete surface in the greenfield design, update the complete scope and rationale rather than silently dropping its evidence.

## BRAIN conformance before drafting and at review

Before Sol drafts or changes production-bound code, tests, scripts, configuration or examples, Luna supplies a scoped applicability table: exact governing source/section, one current precedent, affected artifact, required check, and any unresolved conflict. Cover applicable project/file/type/member/parameter naming, abstraction and dependency boundaries, fixture taxonomy, test data and assertions, script/configuration conventions, documentation and formatting. Mark a category N/A only with a reason; do not invent a rule where the inspected sources establish only precedent.

For test examples, verify the complete chain: canonical independently authored RuleScenario expectations → approved typed projection → TheoryData payload → MemberData target → operation-group test name → actual API signature/generic constraints → existing assertion helper. Global `docs/ai/specs/testing/unit-test.md` §§4.1,4.5,5.1 and `docs/ai/specs/testing/fixture.md` §2 apply alongside the relevant layer spec. Must unit-test.md “TestData Pattern” requires fixture-backed ValidCases/InvalidCases and forbids inline hardcoded TestData; boundary/edge scenarios belong in the canonical fixture taxonomy, not invented Must datasets. Preserve surface-specific null behavior.

Astra reviews the actual patch against this table. Build success, source inspection, or a Proposed label does not waive applicable standards. A package-free mechanical smoke harness must be clearly separated from repository test guidance and cannot serve as evidence of fixture conformance. Missing applicable evidence or an unresolved mismatch blocks closure. Fix feedback, recheck the affected chain, and persist the outcome in the closeout ledger; avoid repeating unrelated verification.

The matrix must cover every new or changed example category, including C#, typed tests, PowerShell, project/configuration and Markdown. Evidence may state unverified where necessary, but production-bound examples cannot be accepted on an unsupported compliance assertion.
