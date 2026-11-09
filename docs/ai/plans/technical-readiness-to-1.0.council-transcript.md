<!-- metadata_header
type: evidence
id: technical-readiness-to-1.0-council-transcript
version: 1.0
status: complete
last_updated: 2026-11-09
-->

# Technical Readiness to 1.0 — Claude Fable 5.1 council transcript (2026-11-09)

> Editorial integration note (2026-11-09): this transcript records the council
> discussion at its reviewed revision. Its references to open Core/Common decisions
> are historical; use the [current implementation record](core-common-api-implementation.md)
> for their disposition. A `+` inside a path marks a proposed output, not an existing file.

Plan-gated council on [technical-readiness-to-1.0/plan.md](technical-readiness-to-1.0/plan.md) (v1.10,
status planned), convened on 2026-11-09 at the owner's direction as the fourth and last phase of the
November recovery ("council run on the readiness plan"). The plan's own D19 requires "a real council
plus implementation authorization" before Planned → Active; this transcript is the council half. The
owner's authorization is a separate act and is recorded in the plan when given.

## Run provenance

- Orchestrating session: Claude Code, Claude Fable 5.1 (`claude-fable-5-1`), session
  `2931302a-3655-4586-926e-28617941bb32`, 2026-11-09.
- Eleven sub-agents, all Fable (`model: fable`, `subagent_type: general-purpose`, no tools): five
  advisors spawned in one batch, five peer reviewers spawned in one batch, one chairman.
- Anonymisation mapping used for the reviewers (private until the chairman step):
  **A = Executor, B = Outsider, C = First Principles, D = Contrarian, E = Expansionist.**
- Context enrichment: `technical-readiness-to-1.0/plan.md` (header, workstream index, waves, decision
  register), the owner's statements in this session, and the state of main after the Temporal Phase 1
  merge (`ee08ccd`).
- Only the sub-agents' text output is recorded here; no hidden reasoning. A council recommendation
  becomes a decision only when the owner acts on it.

## Framed question

> **Core decision.** Should PineGuard activate the "Technical Readiness to 1.0" plan (`docs/ai/plans/technical-readiness-to-1.0/plan.md`, v1.10, status planned) as written — 23 workstreams in six waves A–F, estimated 204–430 engineering-equivalent hours for core work — and if so, what is the minimum process under which Waves A and B (naming/taxonomy → new static rule structure + executable manifest, with early benchmarks) can start now?
>
> **Key context from the owner.** The owner's stated goals are (1) the new Rule classes — a greenfield static rule structure where each rule has one executable source of truth and every projection layer (Must, Guard, FluentValidation, DataAnnotations) only adapts it — and (2) taking the library "to the top of the world": measured performance, hostile-input proof, trimming/AOT consumers, and a 10/10 on every row of the technical rubric. The owner has said process weight stalled the previous attempt: the plan was published 2026-09-26, then amended three times with procedure (role separation Astra/Luna/Sol, per-block closure ledgers, two-minute checkpoints, hash-keyed source caches) and no workstream ever started. The owner's reaction to governance ceremony today was "why all the ceremony about this" when the underlying decision was obvious. As of 2026-11-09: main CI is green, the New Surfaces program is complete, and Phase 1 of the Temporal program is merged to main (ee08ccd) with its charter register ratified; the Temporal program's Phases 2–6 (time-zone rules, validity/relative, durations/calendar, formats/epoch) are not started.
>
> **Key context from the plan.** Current rubric: performance/resource 4/10 (an evidence gap — nothing is measured), CI/tooling 5, security 6, API/package 6, the rest 7–8. Decision register: D01 greenfield/no shims ACCEPTED; D06 pilot rules (Email, bounded numeric, string→DateOnly) ACCEPTED; D19 Planned→Active requires a real council plus implementation authorization; D22 exact file paths need inventory + approval before coding; D24 100% line+branch within the audited Coverlet denominator is a hard requirement; D27 the knowledge/tooling closure contract applies to every block. The plan itself says "no execution agent invents a design to fill silence" and that the predecessor New Surfaces program shipped under a one-page tracker and bounded units. The repository already enforces 100% line and branch coverage in CI, a 14-rule TypeScript audit with a baseline ratchet, and dotnet format; those are the discipline the plan is trying not to lose.
>
> **Trade-offs the author is least sure about.** (a) Whether the per-block closure contract (D27/D28) and the Astra/Luna/Sol role protocol are what makes the work trustworthy, or what makes it never start — can they be reduced to "one tracker row + one review per unit" without losing the evidence discipline the rubric needs? (b) Whether W22 (naming/taxonomy) must fully complete before W03 (static structure) begins, or whether the three pilot rules can be built and renamed cheaply afterwards given D01's no-compatibility stance. (c) Whether to fold the 36 open decisions in `core-common-api-decisions.md` (another session is revising that file right now) into W22 or decide them separately first. (d) Whether the Temporal program's Phase 2+ should wait for the new structure or proceed on the current one, given that Phase 1 just landed on the current structure.
>
> **What is at stake.** Starting too heavy repeats September: another month of planning artefacts and no Rule classes. Starting too light risks a second rewrite of the public surface and losing the 100%-coverage and audit discipline that currently distinguishes the library. The library is pre-1.0 with no compatibility obligation, so the cost of a wrong public name is low now and permanent after 1.0.

Each advisor received the question with its persona header (Contrarian, First Principles Thinker,
Expansionist, Outsider, Executor) and the instruction: "Respond from your perspective. Be direct and
specific. Don't hedge or try to be balanced. Lean fully into your assigned angle. Keep your response
between 150–300 words. No preamble. Do not use any tools."

## Advisor — Executor (anonymous response A)

Activate nothing "as written." 23 workstreams and six waves is a planning document, not a Monday-morning task. Here is the first concrete action:

**Monday morning:** open a worktree, create `+ src/PineGuard.Core/Rules/Email/EmailRule.cs` (or wherever the inventory says — D22 is satisfied by a ten-line file list in the PR description, not a separate approval cycle), and write the Email pilot rule with one executable source of truth plus its Must adapter and tests. Ship it as one PR under the existing CI gates. That is the whole first unit. Bounded numeric and string→DateOnly follow as PRs two and three.

**Strip the process to this:** one tracker file with one row per pilot rule (Not started / In PR / Merged), and one code review per PR. The evidence discipline already exists in CI — 100% line+branch, the 14-rule audit ratchet, dotnet format. D24 and D27 are *already enforced by machines*. The per-block closure ledger and the Astra/Luna/Sol protocol add nothing a green CI run and a reviewer do not already give you. Delete them from the critical path.

**On the trade-offs:** (b) Build the three pilots first, rename after. D01 says no shims; a rename is a `git mv` plus find-replace across three rules. W22 completing first is the September trap in a new coat. (c) Do not fold 36 open decisions into anything. Decide only the ones the three pilots actually touch — probably five — in the PR itself, and leave the rest open. (d) Temporal Phase 2 proceeds on the current structure now; it is a separate team-equivalent and waiting on a structure that does not exist yet is idle time.

**Things without a clear first step — call them out:** performance 4/10 ("nothing is measured") has a trivial first step everyone is skipping: add one BenchmarkDotNet project with one benchmark for the Email pilot, and commit the numbers. Do that in PR one. AOT/trimming and hostile-input have no first step named anywhere in the plan — they stay out until a pilot exists to point them at.

Activate by merging PR one. Status flips to Active when code lands, not when a council votes.

## Advisor — Outsider (anonymous response B)

Read the framed question as someone who has never seen this repository and you hit a wall of undefined terms in the first paragraph. "Astra/Luna/Sol", "D27/D28", "W22→W03", "Coverlet denominator", "closure ledger", "hash-keyed source cache", "charter register", "New Surfaces program", "Temporal program". None of these are explained. The question cannot be answered by anyone who has not lived inside the plan — and that is the diagnosis, not a complaint about the prose.

A plan that needs its own glossary to be discussed is a plan that needs its own glossary to be *executed*. The owner's "why all the ceremony" reaction is exactly what an outsider feels reading this: the real decision is one sentence ("build three pilot rules under the new structure, keep 100% coverage, rename afterwards") and it is buried under a numbering scheme for decisions, a numbering scheme for workstreams, a lettering scheme for waves, and a three-persona role protocol with celestial names.

Specific things I cannot evaluate from what is written:

- What "activate" means. Does it change a status field, or does someone start typing code? The question treats a status change (D19 "Planned→Active") as if it were work.
- What "hours of engineering-equivalent" measures when the labour is agents. 204–430 of what?
- Whether the "rubric" is an external standard or something the owner wrote. A 4/10 against your own scorecard is not evidence of anything.
- Trade-off (b) is stated clearly and already answers itself: pre-1.0, no shims, renaming is cheap. Build the pilots. Why is this a question?
- Trade-off (c) references "36 open decisions in a file another session is editing right now". An outsider reads that as: two people are rewriting the same page while asking whether to start the book.

My read: the governance vocabulary is the ceremony. If a new contributor cannot understand the plan in ten minutes without the glossary, the minimum process is "delete the glossary." One tracker row, one review, the existing CI gates. Everything else is insider shorthand that only the author can execute — which is why only the author's planning sessions ever ran.

## Advisor — First Principles (anonymous response C)

**The question is wrong. You are asking "which process is light enough to let the plan start." The real question is "why does a plan exist at all for something whose first artefact is three C# classes?"**

The underlying goal is stated plainly by the owner: one executable source of truth per rule, projections that only adapt. That is a *design claim*, and design claims are falsified by code, not by ledgers. Nothing in 23 workstreams, six waves, or 204–430 hours teaches you whether the claim holds. The D06 pilot — Email, bounded numeric, string→DateOnly — teaches you in about two days. Until those three rules exist with all four projections hanging off them and 100% coverage, every other workstream is speculation about a structure nobody has touched.

Assumption to strip: "naming must precede structure" (trade-off b). This inverts cause and effect. You cannot name a thing well before you know its shape; the 36 open decisions in `core-common-api-decisions.md` are open *because* nobody has built the shape. D01 says no shims — so renaming three classes costs a find-and-replace. W22 is not a prerequisite; it is the *output* of the pilot.

Second assumption: that the rubric's 4/10 performance score is a goal. It is a measurement gap. One BenchmarkDotNet project against the current structure, run today, converts "unmeasured" to "measured" and gives the pilot a baseline to beat. That is not Wave B; it is a Tuesday.

On (a): the discipline you fear losing already lives in CI — 100% line+branch, audit ratchet, format. Those gates are what makes work trustworthy. D27/D28 and Astra/Luna/Sol add no evidence those gates do not already produce; they add narration about evidence. Delete them.

On (d): Temporal Phase 2 waits on nothing. If the pilot succeeds, Temporal migrates like everything else under D01.

Reframe: **Do not activate the plan. Build the three pilot rules on a branch, with the benchmark harness, under existing CI. Then write the plan — from what you learned, not what you imagined.**

## Advisor — Contrarian (anonymous response D)

The strongest-sounding part of this plan is "one executable source of truth, every layer only adapts it." That is the fatal flaw, not the selling point. Email, bounded numeric, and string→DateOnly were chosen as pilots because they are easy. They will prove nothing. The place this architecture dies is where Must wants a `Result` with a parameter name, Guard wants a typed exception with `CallerArgumentExpression`, FluentValidation wants a lazily-formatted message with placeholders, and DataAnnotations wants a `ValidationResult` with member names resolved at attribute instantiation. The "rule" the pilots will produce is a predicate plus a message template; the real cost sits in the adapters, which the plan budgets as mechanical. The Temporal program's Phase 1 just landed on the current structure and nobody said "this was painful because the structure is wrong." If the current structure can absorb Temporal cleanly, the business case for a greenfield rewrite is weaker than the plan admits.

Second: 204–430 hours is not an estimate, it is a confession of a 2x uncertainty on a plan already amended three times. The honest reading is that nobody knows what W03 costs because D22 says file paths need inventory and approval before coding. A plan that forbids inventing design, forbids coding before path approval, and has 36 undecided API names in a file another session is editing right now has no startable unit. That is why September stalled — not Astra/Luna/Sol, but a plan with no first step that is both authorised and defined.

Third, the question the owner is avoiding: D01 "no shims" plus pre-1.0 means every rename is free *until the Temporal Phases 2–6 pile code onto whichever structure exists*. Answering (d) "proceed on current" and (b) "rename later" together guarantees two structures coexist for months, and the audit ratchet will enforce consistency against the old one.

Minimum process: pick ONE hard pilot (string→DateOnly, because Temporal depends on it), build all four adapters, measure the adapter cost against the current implementation. If adapters are not trivially mechanical, kill W03 before W22 spends a month on names for a structure that will not ship.

## Advisor — Expansionist (anonymous response E)

The framing undersells this badly. It treats the new Rule structure as a refactor with a rubric attached. It is actually the product.

**The upside the plan buries.** "One executable source of truth per rule, every layer only adapts it" is not a tidy-up — it is a rule *engine* with four projections. Once that spine exists, every new projection is a thin adapter: a Roslyn analyzer that flags inputs the rule would reject at compile time, a source generator that emits AOT-friendly validators, a JSON Schema / OpenAPI emitter, a MinimalApis/Blazor binding. The executable manifest (W03) is the hidden asset: it is a machine-readable catalogue of every rule's contract. That is what lets you auto-generate docs, the benchmark harness, the hostile-input corpus, *and* the cross-layer consistency test — one manifest, four surfaces proven identical. No competing .NET validation library has that. That is "top of the world", not a 10/10 on a rubric row.

**Performance 4/10 is a gift, not a gap.** Nothing is measured, so the first BenchmarkDotNet run is the single cheapest rubric jump available. Start it in Wave A as a one-afternoon spike against the three pilot rules. If the static structure is faster than the current one (likely — no allocation, no closure), you have a marketing number before any public name is final.

**On the trade-offs:** (b) build the three pilots *now* on the best current guess at names; D01 means a rename is a find-and-replace. W22 blocking W03 forecloses the one thing that would actually inform W22 — real code. (c) Fold the 36 decisions in; most dissolve once three rules exist. (d) Make Temporal Phase 2 the *fourth pilot*. It is greenfield, un-started, and exercises exactly the hard case (TimeZoneInfo-dependent rules) that will stress the structure early rather than after twenty easy rules.

**Minimum process:** one tracker row per pilot, one review, CI already enforces 100%/100%. Ship the pilots in two weeks and the plan rewrites itself from evidence.

## Peer reviews

Each reviewer received the framed question and the five responses labelled A–E (mapping above), and
was asked: which response is strongest and why; which has the biggest blind spot; what all five
missed. Under 200 words, no tools.

### Reviewer 1

1. **Strongest: D.** It is the only response that interrogates the design claim rather than the process. A, C, and E all assume the four-projection adapter is mechanical and that pilots prove the structure; D names the exact place it breaks (Result with ParamName vs CallerArgumentExpression exception vs lazily-formatted FluentValidation message vs ValidationResult with member names) and points out Temporal Phase 1 landed on the current structure without complaint, which undercuts the rewrite's business case. Its remedy, one hard pilot with all four adapters and an adapter-cost measurement, is a kill-or-confirm experiment, not a slogan. It also catches the (b)+(d) interaction that A and C wave through: two structures coexisting for months with the audit ratchet enforcing the old one.

2. **Biggest blind spot: B.** It reviews the prose, not the decision. "Delete the glossary" is a readability note; the owner needs a Monday action. It offers nothing on adapter cost, Temporal sequencing, or benchmarks.

3. **All five missed:** none mentions D24's trap. 100% line+branch is machine-enforced, so a greenfield branch that adds a parallel rule spine must hit 100% on day one or CI is red the whole pilot, which pushes toward one big PR, exactly the pattern the plan's bounded-unit discipline forbids. Decide upfront: a `[ExcludeFromCodeCoverage]`-free staging convention, or a temporary Coverlet-scope carve-out with an explicit expiry.

### Reviewer 2

**1. Strongest: D.** It is the only response that treats "one executable source of truth, every layer adapts" as a hypothesis rather than a slogan, and names where it breaks: Must's `Result` + param name, Guard's `CallerArgumentExpression`, FluentValidation's lazy placeholders, DataAnnotations' member-name resolution. It also catches the interaction nobody else saw: answering (b) "rename later" and (d) "proceed on current" *together* guarantees two coexisting structures with the audit ratchet enforcing the old one. Its test (one hard pilot, measure adapter cost, kill W03 if adapters are not mechanical) is falsifiable and cheap.

**2. Biggest blind spot: E.** It assumes the spine works and sells the downstream catalogue (analyzers, source generators, JSON Schema emitters) before a single rule exists. Making Temporal Phase 2 the fourth pilot stakes an un-started program on an unproven structure. D's adapter-cost question is exactly what E never asks.

**3. All five missed:** D24's "100% within the audited Coverlet denominator" plus the greenfield pilot means the new tree must be *inside* the denominator from PR one, or it is quietly exempt and the discipline the owner fears losing is lost at the first commit. None of them specified how the pilot branch stays gated, nor who owns the 36-decision file's concurrent revision while pilots invent names it will contradict.

### Reviewer 3

**1. Strongest: D.** It is the only response that attacks the plan's *premise* rather than its *process*. A, B, C and E all converge on "strip ceremony, build the three pilots, rename later" — a consensus so uniform it is suspicious. D alone asks whether the four adapters are actually mechanical (Must wants a `Result`+`ParamName`, Guard wants `CallerArgumentExpression`, Fluent wants lazy placeholders, DataAnnotations wants attribute-time member names), picks the pilot that stresses that (string→DateOnly, which Temporal depends on), and defines a kill criterion. It also spots the interaction nobody else did: answering (b) "rename later" *and* (d) "Temporal proceeds on current" together guarantees two structures coexisting for months with the audit ratchet enforcing the old one.

**2. Biggest blind spot: B.** Its "delete the glossary" diagnosis is a critique of the *question's prose*, not the decision. It offers no startable unit, no kill criterion, and treats "an outsider cannot read this" as evidence about architecture. Also partly wrong: the rubric scoring and D-numbers are insider shorthand, but the CI gates behind them are real.

**3. All five missed:** the plan is not a spec and the pilots are not design. Nobody defined the **acceptance test for "one source of truth"** — a cross-layer consistency test asserting all four projections agree with the manifest on every input (E gestures at it, nobody makes it the pilot's exit criterion). Without that, "the pilot succeeded" means "it compiled with 100% coverage", which the current structure already achieves. Also: no one addressed who *reviews* when the owner is the only reviewer and labour is agents — "one review per PR" is still ceremony unless the reviewer is named.

### Reviewer 4

**1. Strongest: Response D.** It is the only one that interrogates the design claim rather than the process. A, C and E all assume the pilots will succeed and argue about how lightly to run them; D names where the architecture actually breaks (Must's `Result`+param name, Guard's `CallerArgumentExpression`, Fluent's lazy placeholders, DataAnnotations' `ValidationResult` member names) and proposes the one pilot that tests it. It also catches the interaction between (b) and (d) that nobody else sees: "rename later" plus "Temporal proceeds on current" means two structures for months with the audit ratchet defending the old one.

**2. Biggest blind spot: Response B.** Its complaint that the question has insider vocabulary is true but inert — it answers nothing about (a)–(d) and ignores that the owner wrote the glossary and does not need it translated. "Delete the glossary" is a style note dressed as a decision.

**3. What all five missed:**
- Nobody asked what the owner has *already* authorised. D19 requires implementation authorization; the owner is in the room. One sentence from the owner is the activation — none of A–E say "ask for it now."
- Nobody addressed the concurrent session editing `core-common-api-decisions.md` as a merge hazard for the pilot branch.
- Nobody costed the benchmark baseline against the *current* structure before the pilot exists — the only way to know whether the static structure is actually faster.

### Reviewer 5

**1. Strongest: D.** It is the only response that tests the plan's premise rather than its process. "Adapters are mechanical" is the load-bearing assumption of W03, and D names exactly where it breaks (Result/param-name vs. CallerArgumentExpression vs. lazy placeholders vs. ValidationResult member names). It also catches that A/C/E's answers to (b) and (d) together guarantee two structures coexisting under one audit ratchet. Its "kill W03" conclusion is premature, but its falsification test (one hard pilot, all four adapters, measure adapter cost) is the right first unit. A's Monday plan is the best execution wrapper for D's test.

**2. Biggest blind spot: E.** It re-inflates scope while claiming to shrink it: fourth pilot (Temporal Phase 2, the hardest case), fold all 36 decisions in, plus analyzers/generators/schema emitters. That is September's failure mode in enthusiastic prose. It also asserts the static structure is faster with zero measurement.

**3. All five missed:**
- D24's 100% line+branch gate makes a half-wired greenfield structure *fail CI* on day one; the pilot needs an explicit coverage scope or it cannot merge incrementally.
- The owner signs off every public name (repo policy). That, not Astra/Luna/Sol, is the real serialization point. Budget it.
- No exit criterion: what measured result confirms or kills W03?

## Chairman verdict

The chairman received the framed question, the five advisor responses with their persona names, the
five peer reviews, and four verified facts from the orchestrator: coverage gating is per test project
through Coverlet with the CI job enforcing 100% on the merged report, and a new `tests/PineGuard.*.UnitTests`
project is auto-discovered, so a pilot is inside the denominator the moment it exists; the owner signs
off every public name and is the reviewer of record, with Fable sub-agents and GPT-6 Astra as reviewers
in practice; Temporal Phase 1 landed on the current structure with no finding that the structure was the
obstacle; and `core-common-api-decisions.md` is under concurrent revision by the owner with Astra and must
not be edited by this program until that lands.

### Council Verdict: Activating Technical Readiness to 1.0

#### Where the Council Agrees

All five advisors and all five reviewers converge on four points, and the Chairman adopts them without reservation:

1. **Do not activate the plan as written.** Twenty-three workstreams, six waves and a 204–430 hour range are a planning document, not a startable unit. Nobody argued for executing the wave structure.
2. **The evidence discipline already lives in machines, not in the protocol.** 100% line+branch (D24), the 14-rule audit ratchet and dotnet format are enforced by CI. The per-block closure ledger (D27/D28), the Astra/Luna/Sol role protocol, two-minute checkpoints and hash-keyed caches add narration about evidence, not evidence. A, C, E say delete them; B says they are the ceremony; D says they were not even the cause of the stall, which is a sharper version of the same conclusion.
3. **W22 (naming) must not gate W03 (structure).** D01 says no shims and the library is pre-1.0; a rename is `git mv` plus find-replace. C puts it best: names are the *output* of building the shape, not the prerequisite.
4. **Performance 4/10 is a measurement gap with a one-afternoon fix.** One BenchmarkDotNet project, run against the pilot and the current path, in the first PR.

The minimum process is also unanimous in outline: one tracker row per unit, one review per PR, existing CI gates.

#### Where the Council Clashes

**Does the pilot test the premise, or assume it?** This is the genuine split. A, C and E treat "one executable source of truth, every layer only adapts" as settled and argue about how lightly to run the pilots. D alone treats it as a hypothesis and names where it breaks: Must wants a `MustResult` with a code and parameter name, Guard wants a typed exception with `CallerArgumentExpression`, FluentValidation wants a lazily-formatted message with placeholders, DataAnnotations wants a `ValidationResult` with member names resolved at attribute time. If the rule object cannot supply all four, the "adapters" carry rule logic and the architecture is a predicate plus a message template with the real cost hidden in the layers the plan budgets as mechanical. All five reviewers ranked D strongest on this ground. Verified fact 3 sharpens D's point further than D knew: the current structure *already* has a single delegation chain (Core predicate → Must → Guard/Fluent/DA delegate to Must), and Temporal Phase 1 put 114 files through it without the structure being the obstacle. So the question the pilot must answer is not "can we build a rule class" but "what does a rule object give us that the Must clause does not already give as the de facto source of truth, and at what adapter cost?"

**Which pilot, and how many.** A and C: the three D06 pilots, easiest first. D: one hard pilot (string→DateOnly) because easy pilots prove nothing. E: the three plus Temporal Phase 2 as a fourth. The Chairman rules with D on *order* and against D on *count* (below).

**Temporal Phase 2 sequencing.** A and C: proceed on the current structure now, idle time is waste. D: "rename later" plus "proceed on current" together guarantee two structures coexisting for months with the audit ratchet defending the old one. E: make it the fourth pilot. The majority (A, C) is overruled here, see the Recommendation for the reason.

**The 36 open decisions.** E: fold them in, most dissolve once code exists. A, C, D: decide only the ones the pilots touch. The Chairman sides with the majority, and verified fact 4 makes it mandatory rather than preferable.

**Whether to "activate" at all.** C says do not activate; build on a branch and write the plan afterwards. A says activation *is* merging PR one. These are compatible once D19 is read correctly (below), but they are not the same claim and should not be smoothed over: C wants the plan's status to stay Planned until evidence exists; A wants Active to mean "code merged." The Chairman adopts A's definition.

#### Blind Spots the Council Caught

The reviewers surfaced six things none of the five advisors addressed:

1. **D24 and the greenfield branch.** A new `tests/PineGuard.*.UnitTests` project is auto-discovered and merged into the 100% gate the moment it exists (verified fact 1). The pilot cannot land half-wired and cannot be gated incrementally without a carve-out. Consequence: each pilot is exactly one PR containing the rule, all four adapters, all tests and the benchmark, and `[ExcludeFromCodeCoverage]` is not used. No separate decision needed; the gate stands as-is.
2. **No acceptance test for "one source of truth."** Without a cross-layer consistency test, "the pilot succeeded" means "it compiled at 100%," which the current structure already achieves. Reviewer 3's point becomes the pilot's exit criterion.
3. **The real serialization point is the owner's name sign-off**, not Astra/Luna/Sol (verified fact 2). Every public name in the pilot needs the owner's tick. That must be budgeted as a named step in the PR, or the pilot stalls on the review the protocol was pretending to replace.
4. **The concurrent revision of `core-common-api-decisions.md`** is a merge hazard (verified fact 4). The pilot must not touch that file, and must expect some of its provisional names to be contradicted when the Astra revision lands.
5. **Benchmark the current structure, not just the new one.** The only way to know whether the static structure is faster is a baseline of today's `StringRules`/Must path for the same rule, in the same PR. E's "likely faster, no allocation" is an unmeasured assertion.
6. **The owner is in the room.** D19 requires a real council plus implementation authorization. This is the council. One sentence from the owner is the authorization. Nobody said "ask for it now."

#### The Recommendation

**Core decision: No, do not activate the plan as written. Activate one unit.** This council satisfies D19's "real council." The owner's one-line reply authorizing the pilot below is the implementation authorization. The plan's status becomes Active with scope restricted to the pilot; Waves A–F stay in the document as a map to be redrawn from pilot evidence, not as a schedule. Nothing else in the plan is authorized until the pilot returns a verdict.

**The pilot.** Build **string→DateOnly** first, as one PR, with the rule object (predicate + code + message template + metadata) and all four adapters: a Must clause returning `MustResult` with code, message and `ParamName`; a Guard clause using `CallerArgumentExpression`; a FluentValidation extension with placeholder formatting; a DataAnnotations attribute with member-name resolution. Email and bounded numeric follow as PRs two and three *only if* PR one passes. Reason for overruling the A/C "easy ones first" majority: D is right that Email proves nothing about adapter cost, and string→DateOnly is the hard case (parse plus format plus culture metadata) that Temporal Phases 2–6 will lean on. Reason for overruling D's "one pilot, then kill W03": one hard pilot confirms or kills the *adapter* claim; the two easy ones are still needed to confirm the structure is not merely a DateOnly-shaped special case. D06's three pilots stand, reordered.

**Exit criterion for PR one (all five, or the pilot has not passed):**
- No adapter contains rule logic: no predicate branch, no message text literal, no code literal; each adapter is a mapping call into the rule object. This is the falsification test for D's objection and is checkable by reading the diff.
- A cross-layer consistency test, driven by a single `TheoryData`, asserting that Must, Guard, Fluent and DataAnnotations agree with the rule object on validity, code and rendered message for every input.
- A BenchmarkDotNet project with two benchmarks: the new rule-object path and the current `StringRules.DateOnly`/Must path for the same inputs, with numbers committed to the tracker.
- CI green on the existing gates: 100% line+branch across the merged report, audit ratchet, dotnet format. No `[ExcludeFromCodeCoverage]`.
- Owner has signed off every public name in the PR's names table.

**Kill criterion:** if any adapter must carry logic the rule object cannot supply, or the benchmark shows the new path allocating where the current path does not, W03 returns for redesign before PRs two and three, and the owner decides whether the rule object is worth a second attempt. That is a two-week answer to a question the plan has been circling since September.

**Time-box:** two weeks from authorization to merge of PR one. If it is not merged, the work stops and the reason is written into the tracker as the finding.

**Minimum process, concretely.** Three artefacts exist and no others:
1. `+ docs/ai/plans/technical-readiness-to-1.0/tracker.md`, one page, three rows (one per pilot): status (Not started / In PR / Merged / Killed), PR link, names signed-off, benchmark numbers, verdict. This is the format the New Surfaces program shipped under.
2. The PR itself, whose description carries the ten-line file list (satisfying D22 without a separate approval cycle) and a names table listing every new public identifier for the owner to tick.
3. One Fable code review per PR (Fable is the designated review tier), then the owner signs the names and merges.

Deleted from the critical path: per-block closure ledgers (D27/D28 as a procedure; the *content* they wanted is the CI run and the tracker row), the Astra/Luna/Sol protocol, two-minute checkpoints, hash-keyed source caches.

**(a) Closure contract and role protocol.** Reduced to "one tracker row + one Fable review + owner name sign-off per PR." The evidence discipline is not lost because it was never in the ledgers; it is in CI and in the consistency test this verdict adds.

**(b) W22 before W03.** No. The pilot ships under provisional names the owner signs as provisional-for-pilot in the PR. W22 runs *after* the pilot, from the shape the pilot reveals. D01 makes any rename free.

**(c) The 36 open decisions.** Decide separately, and not by this program. `core-common-api-decisions.md` is being revised by the owner with Astra in another worktree and must not be edited by pilot work. The pilot decides only the names it touches (expect roughly five), records them in its names table, and accepts that W22 will reconcile them against the revised file when it lands. Any contradiction is a find-replace.

**(d) Temporal Phase 2.** Wait, but only for the pilot's two-week time-box, not for the new structure to exist. Reason for overruling A and C: D's interaction argument is correct and the majority waved it through. Temporal Phases 2–6 are the largest unstarted body of work in the repository; putting them on the current structure while the pilot runs is exactly how two structures come to coexist with the ratchet enforcing the old one. If the pilot passes, Temporal Phase 2 becomes the first real consumer of the rule object, which is E's instinct without E's scope inflation. If the pilot is killed, Temporal Phase 2 proceeds on the current structure having lost two weeks and nothing else. Phase 1 is merged and untouched either way.

**Beyond the pilot.** Hostile-input and AOT/trimming have no first step until a spine exists to point them at; they stay out. The benchmark in PR one is the whole of Wave B for now.

#### The One Thing to Do First

With the owner's one-line "go" on this verdict as the authorization, open the worktree `.claude/worktrees/pilot-dateonly` and write the first failing test: the cross-layer consistency test for string→DateOnly, driving all four projections from one `TheoryData` and asserting they agree with a rule object that does not yet exist. Everything else in PR one — the rule object, the four adapters, the benchmark, the names table — is written to make that test pass.

## Disposition

- The verdict was presented to the owner on 2026-11-09. Under the plan's D19, this transcript is the
  "real council"; the owner's implementation authorization for the string→DateOnly pilot is the second
  half and is recorded in the plan header (`status`) and in `+ technical-readiness-to-1.0/tracker.md` when
  given. Until then the plan stays `planned` and nothing in it is authorized.
- The plan's `metadata_header` is stamped with `council_verdict_ref`, `council_verdict_date` and
  `council_trigger` by the same commit that adds this transcript.

<!-- footer
last_verified: 2026-11-09
-->
