# Authoritative source map

## Canonical knowledge and tooling closure sources

These Luna-verified sources govern the [closure contract](knowledge-and-tooling-closure.md) and [session lesson ledger](closeout-ledger.md):

| Source | Exact reviewed section | Application |
|---|---|---|
| `docs/ai/meta/taxonomy.md` | Core Concepts / Memory, lines 93–99; Rules | Per-applicable-subagent memory observations; normative Spec promotion and rule pointers |
| `docs/ai/README.md` | Memory adapter mapping, lines 223–228 | Portable memory versus adapter surfaces |
| `docs/ai/specs/orchestration.md` | Learn from corrections, lines 153–158 | Save correction constraint+rationale in appropriate durable home |
| `docs/ai/specs/spec.md` | §9, lines 443–456 | Child/root promotion and replacement of duplicate guidance with pointers |
| `docs/ai/rules/global.md` | Lines 3–7 | Authoritative specification sources |
| `docs/ai/rules/tools.md` | Lines 5–13 | Tool specification plus root/per-tool README obligations |
| `tools/README.md` | Lines 30–32 | Pester registration and Test-Tools.ps1 |
| `docs/ai/specs/tools/spec.md` | Header, §§1–2 | Scope tools/** and .etc/powershell/**; Test-/Sync- naming, structure/shared modules |
| `docs/ai/meta/tooling.md` | Single Source of Truth, lines 13–20 | Reuse canonical tooling inventory rather than duplicate registry |
| `docs/ai/meta/adapter-surfaces.md` | §5, lines 165–182 | Affected adapter cascade checklist |
| `docs/ai/plans/tools-review-and-standardisation.md` | §3.4, lines 225–251; Phase 2 T2.03–T2.06, lines 343–353; Phases 5–6 | Precedent: registry/help/Pester/self-test evidence, docs/spec upkeep and independent feedback; special standing rules are not inherited |

These references were supplied by Luna's read-only intake. Paths are repository-relative unless marked project workspace. Section references are used deliberately; line numbers can change during the planned work.

| Source | Relevant sections | Use and limits |
|---|---|---|
| `AGENTS.md` and applicable scoped adapters | Repository entry points | Refresh BRAIN and scoped authority before operations |
| `docs/ai/specs/spec.md`, `protocol.md`, `safety.md` | Governing specifications; safety §§2,7,8 | Scope, safe operations, ownership |
| `docs/ai/specs/orchestration.md` | Context refresh; Workflow Orchestration | Explicit plan, checkpoints, drift/replan |
| `docs/ai/specs/council.md` | §§1,4,5,6 | Activation criteria, recorded council procedure; user model override is explicit |
| `docs/ai/workflows/plan-with-council.md` | Formal workflow | Transcript, revisions and verdict reference |
| `docs/ai/meta/taxonomy.md` | Plans | Plan organization; user-requested folder overrides flat convention |
| `docs/ai/README.md` | Active Plans | Canonical master index and matching metadata |
| `docs/ai/rules/global.md`, `coordination.md` | Applicable global/test-operation rules | Coordination before builds/tests/coverage |
| `tests/AGENTS.md`, `docs/ai/rules/testing.md` | Test editing | Apply before tests are changed |
| `docs/ai/specs/testing/project.md`, `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/testing/fixture.md`, `docs/ai/specs/testing/coverage.md`, `docs/ai/specs/testing/gold-standard.md` | Scoped testing contracts | Harness, fixture and evidence requirements |
| `docs/ai/skills/document/SKILL.md` | Documentation authoring | Apply to publication and link/metadata checks |
| `docs/ai/workflows/commit.md` and its command contract | Commit procedure | Exact-file ownership; no blind broad Docs scope |
| `docs/ai/plans/technical-readiness-to-1.0.md` v1.6 | §§1–11 | Preserved baseline; P0–P6 and source-audited constraints |
| `docs/reports/technical-readiness-rubric-2026-09-25.md` | How to interpret; Rubric; R1–R7; Assessment decision | Nine independent scores/confidence, no aggregate |
| `docs/reports/readme-verification-2026-09-25.md` | Full test suite; Analyzer reference-cache repair; Reproduce; Coverage and other checks; Release and documentation checks | Dated test/README/package evidence |
| `README.md` | Quality metrics; Packages; Where PineGuard fits; Built by AI | Claims to verify against release evidence |
| `PineGuard-Engineering-Handoff.md` (project workspace) | Entire proposed brief | Design input written without repository access; not authority for current code/counts/budgets |
| `docs/ai/plans/competitive-analysis.md` | Living reference | Existing comparison context, not an implementation work item |
| `docs/ai/plans/new-surfaces-missing-validation-cases-00-program.md` and 01–06 children | Program/child structure | Modular-plan precedent; its special standing rules are not inherited |

## Verified implementation anchors

- P0 semantics: `src/PineGuard.Core/Rules/OwaspRules.cs`, `src/PineGuard.MustClauses/MustOwaspClauses.cs`, `tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs`.
- Regex: `src/PineGuard.Core/Rules/StringRules.cs`, `src/PineGuard.MustClauses/MustStringClauses.cs`, `src/PineGuard.Core/Rules/Owasp/OwaspRegex.cs`.
- CI/CLI: `.github/workflows/ci.yml`, `tests/Directory.Build.props`, `apps/cli/package.json` (scripts at reviewed lines 15–21), `apps/cli/src/audit/engine.ts`, `apps/cli/src/audit/rules/layer-parity.ts`, `apps/cli/src/audit/rules/must-codes.ts`.
- Pilot code provenance: `src/PineGuard.Core/Codes/MustCodes.Email.cs`.
- Mutable lifecycle: `src/PineGuard.Core/MustClauses/MustValidator.cs`, `InlineMustValidator.cs`, `MustPropertyRule.cs`; `tests/PineGuard.Core.UnitTests/MustClauses/MustValidatorTests.cs`.

No concrete property/fuzz/differential/mutation harness, benchmark project, trimmed/native consumer project, API baseline or analyzer-test file was established by this bounded intake. Reviewed configuration/source/dependencies/workflows did not reveal dedicated evidence for several of those categories; this is not proof of repository-wide absence. Each affected workstream first inventories and approves exact paths.

## Provenance rules

Preserve source snapshots/commit references and checked dates. Mark handoff suggestions, current behavior, normative requirements, research facts and architectural inferences separately. Missing or inaccessible material remains unknown. The unavailable user snapshot and the incomplete ChatGPT project mirror cannot be treated as complete evidence.
