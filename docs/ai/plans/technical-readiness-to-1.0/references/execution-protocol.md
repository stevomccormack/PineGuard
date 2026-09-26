# Execution protocol

This protocol applies to every child plan. It is a future execution contract; current scope is documentation/planning only.

## Roles and context

Luna owns all repository reading, searches, external lookups, web fetches and MCP use. Before operations, Luna refreshes the relevant BRAIN context and supplies exact source paths/sections, evidence dates and limitations. Astra owns architecture, decisions, plans and review. Sol owns approved implementation and the requested comparative synthesis. Agents may consume supplied evidence packets in their prompts; consuming a packet does not authorize a non-Luna agent to inspect additional files.

Before any build/test/coverage action read `docs/ai/rules/coordination.md`. Before test edits also read `tests/AGENTS.md`, `docs/ai/rules/testing.md` and applicable `docs/ai/specs/testing/{project,unit-test,fixture,coverage,gold-standard}.md`. Source edits require scoped project specifications. Follow `docs/ai/specs/{spec,protocol,safety,orchestration}.md`, applicable global rules, documentation skill and commit workflow. Refresh context when scope changes; do not inherit special standing rules from an unrelated historical plan.

## Required dispatch packet

Every assignment contains: workstream/task IDs; exact intent and exclusions; model/role; verified source excerpt references; accepted decision IDs; prerequisites; exact owned paths; expected input/output artifacts; commands approved after repository inspection; acceptance and deliberate-failure checks; requested time/token bounds; check-in interval; review/commit checkpoint; stop conditions. The packet must identify unknowns explicitly.

Use one bounded outcome per assignment. Prefer 10–20 minutes and 8k–16k requested tokens for a substantive implementation packet, with a checkpoint at least every two minutes and earlier on blockers. Short read-only inventory packets should be smaller. These are planning defaults, not measured capacity or tool-enforced hard limits. Adjust the packet before dispatch if complexity warrants it; do not keep an indefinitely running agent under an obsolete budget.

The current collaboration tools do not expose a hard token/runtime budget parameter. Record requested bounds separately from actual enforcement. The orchestrator monitors check-ins, requests checkpoints and interrupts/reassigns stalled work. An external process timeout can bound a process only when actually configured and recorded. Neither a textual instruction nor an intended timer proves enforcement.

## Evidence and decisions

Report observations separately from interpretations. Include revision, dirty-tree scope, runtime/SDK, target framework, configuration, OS/architecture, command, result, artifact paths and known exclusions. Avoid secret/environment dumps. Test execution counts must state the denominator; coverage requires its own generated artifact. A repaired reference cache is not a production-code fix.

When blocked, send Astra a packet: exact question, triggering evidence, viable options, public/API/performance/privacy/compatibility implications, recommended choice and the work it blocks. Never choose silently. Independent evidence collection may continue.

Do not promote source-only hypotheses to reproduced failures. A new gate is accepted only after it catches an intentionally seeded representative regression and correctly accepts a legitimate case. Deliberate failures belong in controlled fixtures/temporary patches, must be cleaned up, and must not leak into committed production behavior.

## Ownership, checkpoints and commits

Luna inventories initial status and staged changes. Require an empty staged area before task staging, or stop to resolve ownership without unstaging another person's work. Record baseline hashes for pre-existing modified files. The known exclusions are `.codex/config.toml`, `docs/ai/plans/schema-driven-validation-agent.md`, and the uncommitted v1.6 content except for an explicitly reviewed preservation/migration operation.

Use exact-file staging only. Do not run a blind broad `-Scope Docs` command or stage by directory when unrelated changes exist. Review the staged diff and prove only owned artifacts are included before each commit. Follow the repository commit message/workflow contract after Luna supplies it.

Checkpoint after an independently reviewable unit: inventory/decision evidence; minimal approved change; required validation; Astra review; commit owned paths. Regular commits should represent coherent reviewed progress, not arbitrary timer snapshots or failing unrelated state. Record commit IDs and evidence links in the workstream ledger. At the end, propose a squash of only this work's owned commits on its approved branch; do not rewrite shared history or absorb pre-existing edits implicitly.

## Stop and replan

Stop dependent work for missing context, unresolved contract decisions, scope beyond owned files, unexplained test/gate failure, unavailable required model, inconsistent source versions, new public API impact, unsupported deployment claims, or budget overrun without a checkpoint. Replan through `orchestration.md` when evidence changes scope. Do not replace an unavailable Sol/Astra/Luna role with another model and pretend the requirement was met.

Completion requires concrete evidence, exact changed paths, test limitations, remaining decisions and a reviewable artifact. A tool returning success, an agent declaring done, or a passing subset without the expected artifacts is insufficient.
