# Mandatory quality constitution

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

Include: relevant BRAIN/source hashes; exact task/decision IDs; all affected layers/files; duplicate/root-cause search findings; alternatives and chosen abstraction; code/test/docs/manifest consistency;100% coverage denominator/results; relevant conformance/bench/deployment proof; formatting/analyzer checks; limitations; exact owned diff and staged paths.

A reviewer rejects a local fix whose root cause is shared elsewhere, an invented abstraction without present benefit, or a result supported only by passing old tests. Where a decision removes an obsolete surface in the greenfield design, update the complete scope and rationale rather than silently dropping its evidence.
