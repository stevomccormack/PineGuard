<!-- metadata_header
type: plan
id: temporal-07-closeout
version: 1.0
status: planned
last_updated: 2026-09-27
-->

# Plan T-07 — Phase 7: Documentation, roadmap reconciliation, agent memory, Standards Brain sync, cleanup

> Charter: [T-00](temporal-00-program.md). Depends on Phases 1–6 merged in PineGuard and Phase 3
> merged in Standards. Repos: both. Worktrees `.claude/worktrees/temporal-07-closeout` in each.

> Review authority: T-00 §0 and [Astra register](temporal-astra-decisions.md). Codex team implement; Claude Fable 5.1 owns every decision, review and council. [A-01]

## 1. Business plan

Docs are edited once, program-wide (repo lesson), so every README row, roadmap line and memory
note lands here. The program is not Done until both repos are back to a single worktree and the
Brain mirror in Standards is current.

## 2. Units

| Unit | Title | Repo | Model | Depends on | Fan-out |
|---|---|---|---|---|---|
| U1 | Package READMEs + root README rows | PineGuard | Codex team (`general-purpose`) | Phases 1–6 | ∥ U2 |
| U2 | Brain reconciliation (roadmap, competitive analysis, decisions, specs) | PineGuard | Codex team | Phases 1–6 | ∥ U1 |
| U3 | Agent memory + project memory | PineGuard | Codex team | U2 | after U2 |
| U4 | Standards Brain sync + Standards docs | Standards | Codex team | U2 merged | — |
| U5 | Worktree/branch cleanup, tracker close | both | orchestrator | U1–U4 merged | last |
| U6 | Program retrospective | — | Claude Fable 5.1 | U5 | last |

## 3. Unit specifications

### U1 — READMEs

Include consumer guidance for A-06: new strict instant DateTime clauses reject Local and
Unspecified kinds, including values obtained from ASP.NET binding/System.Text.Json; show
DateTimeOffset as the input path for offset-bearing instants. Explain coexistence with legacy
host-converting `DateTimeUtility.ToUtc`. Document null-provider host-zone today and injected
provider semantics. Record Expansionist suggestions for further README/TestTimeZones consumer
features as additive follow-ups, not temporal readiness blockers. [Fable corrective council]

Inventory first (GPT-6 Luna): `rg -n "TimeSpan|DateTime|DateOnly|TimeOnly" README.md src/*/README.md`.
Then add one row per new rule family in the same table style each README already uses:
`src/PineGuard.Core/README.md` (TimeZoneRules, UnixTimeRules, StringRules.DateTime, StringRules.UtcOffset,
new TimeSpan/calendar members), `src/PineGuard.MustClauses/README.md`, `GuardClauses`,
`FluentValidation`, `DataAnnotations`, `tests/PineGuard.Testing/README.md` (`FixedTimeProvider`
zone parameter, `TestTimeZones`). Root `README.md`: only if it lists rule counts or families (the
inventory identifies relevant lines). Never restate a number that CI computes (memory rule: doc pass
runs no tests; take counts from the last CI log). Budget 100k / 30 min.

### U2 — Brain reconciliation

- `docs/ai/plans/library-expansion-roadmap.md`: Part 4 "Clock injection" → done (link T-01);
  Part 7 "Temporal semantics" → done (link T-04/T-05); Part 7 string/identifier list → mark the
  items F-13 found already implemented and leave slug/ULID/MAC/MIME as the remainder; Part 6
  ISO 8601 duration → done (T-06).
- `docs/ai/plans/competitive-analysis.md`: temporal row updated (zone-aware rules, validity
  vocabulary, ISO/RFC formats now present).
- `docs/ai/plans/core-common-api-decisions.md`: C-2 closed (calendar predicates now on all three types).
- `docs/ai/README.md` Active Plans table: T-00 row → "Completed" and move the plan set to
  `docs/ai/plans/completed/` **only after U5**; until then the row reads "Active — see
  temporal-orchestration.md".
- `docs/ai/specs/spec.md` §5.3: confirm the existing Core no-IO rule remains intact; no broad
  culture/time-zone environment exception or Core host-ID resolver was introduced. [A-05]
- `docs/ai/skills/scaffold-rule/SKILL.md` (or the skill that lists Core rule families): add the
  time-zone family to its routing table if it enumerates families.
- T1-U3 source-review stale-documentation fixes (Claude Fable 5.1,
  [`temporal-u3-source-review-fable.md`](temporal-u3-source-review-fable.md)): update Pattern D in
  `docs/ai/specs/data-annotations/unit-test.md` from `BetweenTimeOnlyAttribute(min, max)` and
  MinParam/MaxParam to start/end and StartParam/EndParam; update both A-3 and A-8 references in
  `docs/ai/plans/core-common-api-decisions.md` to state that TimeOnly wraps on start/end while
  DateOnly/DateTime/DateTimeOffset retain min/max. Keep A-3/A-8 open and otherwise unchanged. This
  is T7-U2 work, not T1-U3/T1-U6; it runs after Phases 1–6 merge, in parallel with T7-U1, and before
  T7-U3/T7-U5. If done early, land it as a standalone docs commit only after Phase 1 merges — never
  fold it into T-01. Verify with `audit --gate` (including doc-links) and a grep for the stale
  phrases, then its own Claude Fable 5.1 review; no build, test, coverage run or `exceptions.json`
  change is expected.

Budget 100k / 30 min.

### U3 — Memory

GPT-6 Luna reads `docs/ai/memory/` and the per-agent folders `.claude/agent-memory/validation-builder/` and
`.claude/agent-memory/test-writer/` (each with its own `MEMORY.md`); Codex team writes to the Brain's
`docs/ai/memory/` file for validation-builder (and the matching `.claude/agent-memory/<agent>/MEMORY.md`
pointer line) the "temporal patterns" entry: custom test zones over real ids, `guarded TryGetTodayDate` over `raw clock dates`, guard polarity rule,
`timeZone` required-positional convention. The orchestrator (not a sub-agent) updates the user
memory `project_temporal-timezone-gap-assessment.md` to "COMPLETE, merged <hash>" and adds a
lessons file if the retrospective (U6) produces any. Budget 60k / 20 min.

### U4 — Standards Brain sync

Pre-flight (GPT-6 Luna): confirm `apps/cli/src/cli.ts` still registers the `sync` command (`rg -n '.command("sync")'`)
and that `pnpm -C apps/cli dev --help` lists it; if not, stop and report. Then, in the Standards
worktree: `pnpm -C apps/cli dev sync --dry-run` → review the plan; then
`pnpm -C apps/cli dev sync` (never `--force`); commit the mirrored `docs/ai/specs|rules|skills|agents|workflows`
changes as `docs(brain): sync upstream Brain after temporal program`. Update Standards-local
docs: `docs/ai/standards/**` time-zone pages (SystemTimeZone host semantics, IANA links and canonical membership) [A-12], the Iana generator README
(`backward` step), `docs/ai/plans/temporal-timezone-sync.md` status → completed. Budget 80k / 30 min.

### U5 — Cleanup (orchestrator, PowerShell, one command per call)

Per repo: `git worktree list` → every `temporal-*` worktree removed (`git worktree remove` only
after `git -C <path> status` is clean); `git branch -d feature/temporal-*`; `git branch -r` shows no
`claude/temporal-*` left (delete merged ones with `git push origin --delete <branch>`, Tier 1:
confirm with the owner); `git status` clean on `main`; `git log --oneline -12` shows the merge
commits. Tracker: all rows Done; T-00 header `status: completed`; move the nine plan files to
`docs/ai/plans/completed/` in a final `docs(plans): complete temporal program` commit.

### U6 — Retrospective (Claude Fable 5.1)

Input: the tracker's Notes column and every Claude Fable 5.1 review report under `artifacts/reviews/`
(collected before cleanup). Output: ≤ 40 lines appended to `temporal-orchestration.md` §4
"Lessons" — only lessons the Brain does not already state — and, if any lesson is a standing rule,
a `docs/ai/rules/` or memory update proposed to the owner (not applied).

## 4. Definition of Done

Closeout verifies the actual API/type/layer/TFM acceptance matrix, including nullable and Not*
domain behavior; removals/renames reached all applicable layers and consumer migration notes
identify deliberate pre-1.0 breaks. Confirm DateTime new instant UTC-only behavior, DateTime
civil Kind-ignored behavior, DTO fixed-offset age, frozen wall-clock versus real timers, explicit
format profiles and epoch units. Download-only reference receipts must contain version/source/
UTC/hash and no generated-class changes from refresh. Record actual required Claude Fable 5.1 council/review
outcomes and real checks; pending authentication or execution remains visible. Preserve unrelated
worktrees/branches: cleanup targets only this program's verified clean, merged work. [A-02–A-11]

- Both repos: single worktree, clean `main`, no `feature/temporal-*` or `claude/temporal-*` branches.
- `docs/ai/README.md` lists the temporal program under completed plans.
- Standards `docs/ai` mirror is current (`sync --dry-run` reports no drift).
- Retrospective appended; memory updated.
