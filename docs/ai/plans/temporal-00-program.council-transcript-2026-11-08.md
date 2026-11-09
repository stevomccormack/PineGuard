<!-- metadata_header
type: evidence
id: temporal-00-program-council-transcript-2026-11-08
version: 1.0
status: complete
last_updated: 2026-11-09
-->

# Temporal charter decision rows — Claude Fable 5.1 council transcript (2026-11-08)

Plan-gated council on [T-00 §3](temporal-00-program.md) (the decisions register), convened on
2026-11-08 by the recovery orchestrator at the owner's direction ("run a council on them first")
before the Phase 1 merge of `codex/temporal-foundation`. It is the second Fable council on this
charter; the first (2026-09-26) is preserved as
[temporal-00-program.fable-council-transcript.md](temporal-00-program.fable-council-transcript.md),
and the original 2026-09-22 council as
[temporal-00-program.council-transcript.md](temporal-00-program.council-transcript.md).

**Outcome.** The chairman recommended flipping D-1 to the UTC calendar date for a null
`TimeProvider`, ratifying the other thirteen rows, rewording D-14, adding D-16 and a D-12
tombstone, and inverting the `core-null-provider-timezone` CI job. The owner ratified the register
and the D-1 flip on 2026-11-09 ("PineGuard never guesses your zone. Without an injected clock,
dates are UTC; inject a `TimeProvider` to choose a zone"). GPT-6 Astra's independent Phase 2 review
(2026-11-08) reached the same D-1 answer.

## Run provenance

- Orchestrating session: Claude Code, Claude Fable 5.1 (`claude-fable-5-1`), session
  `2931302a-3655-4586-926e-28617941bb32`, 2026-11-08.
- Eleven sub-agents, all Fable (`model: fable`, `subagent_type: general-purpose`, no tools):
  five advisors spawned in one batch, five peer reviewers spawned in one batch, one chairman.
- Anonymisation mapping used for the reviewers (recorded privately until the chairman step):
  **A = Expansionist, B = Executor, C = Contrarian, D = Outsider, E = First Principles.**
- Context enrichment for the framed question: `temporal-00-program.md` §3, the three Fable Phase 1
  reviews of 2026-11-08 (correctness, design fitness, record), and
  `src/PineGuard.Core/Utils/DateTimeUtility.cs`.
- Only the sub-agents' text output is recorded here; no hidden reasoning. The council's
  recommendation became a decision only when the owner ratified it on 2026-11-09.

## Framed question

> **Core decision.** PineGuard's Temporal & Time-Zone Program charter (`docs/ai/plans/temporal-00-program.md` §3) has fourteen decision rows (D-1..D-15, no D-12). Every row's status reads "Recommended" or "Astra final under owner authorization"; the owner never signed any of them, yet Phase 1 of the program (branch `codex/temporal-foundation`, ~114 files across Core, Must, Guard, FluentValidation, DataAnnotations and the published Testing package, every gate green: 100% line and branch coverage on net8.0 and net10.0, full CI including a dedicated non-UTC job) embodies them. The owner must decide, before merging Phase 1 to main: ratify the rows as final, re-open specific ones, and in particular what D-1 should say.
>
> **D-1 as implemented.** When no `TimeProvider` is injected, "today" for `DateOnly` Past/Future/MinimumAge and `DateTime` MinimumAge is the calendar date in `TimeProvider.System.LocalTimeZone` — the host machine's zone — derived from one `GetUtcNow` sample with the zone's offset at that instant (DST-safe). Previously PineGuard used the UTC calendar date. The alternative is to default to the UTC date and make zone-aware "today" opt-in by injecting a `TimeProvider` whose `LocalTimeZone` is set.
>
> **Context from the record and the code.** Core rules carry a "deterministic, no IO" invariant. The September council deferred D-1 to the Astra reviewer as "a preference between two defensible policies" rather than refuting the objection; two advisors predicted host-zone dependence would break existing tests — it did (14 failures on non-UTC developer machines), since fixed by injecting a fixed clock. A `TZ=Pacific/Auckland` Linux CI job now pins the null-provider behaviour and is green. There is currently no production way to choose a zone other than subclassing `TimeProvider`; D-14 states "no zone-aware age overload is promised". Most production servers run in UTC, so host-local equals UTC in production but not on developer machines. .NET's `DateTime.Today` is host-local, and `TimeProvider.LocalTimeZone` exists precisely so that "local" can be injected. The library is pre-1.0 with no compatibility obligation.
>
> **Other embodied, unsigned decisions.** D-2: `DateTimeRules.IsSameDay` ignores `DateTimeKind` (calendar predicates read the civil date; instant predicates convert Local→UTC and treat Unspecified as UTC — now documented at class level). D-4: the published `FixedTimeProvider`'s zone defaults to UTC, never the host. D-9: TimeSpan guard boundary polarity fixed by calling the complement Must clause with complemented inclusion. The TimeSpan rename `IsGreaterThan/IsLessThan → IsLongerThan/IsShorterThan` across all layers has no D-row at all, only a naming-canon entry. `TimePrecision.Tick` was renumbered 4→5 to insert `Microsecond` (a silent binary break for anything compiled against alpha.7; pre-1.0 policy permits it). `TimeOnly` Between wraps midnight while `TimeOnlyRange` still rejects reversed bounds. `IsWithinDaysFromNow` kept its "UTC now" wording against the charter's interim D-6 wording.
>
> **What is at stake.** A wrong default for "today" is cheap to change now and permanent after 1.0; it is also the kind of hidden environment dependence the Core invariant exists to prevent. Re-opening every row repeats the September stall in which the program was planned three times and merged zero times. The owner needs: which rows to ratify as-is, which (if any) to change before merge, and the single first action.

Each advisor received the question with its persona header (Contrarian, First Principles Thinker,
Expansionist, Outsider, Executor) and the instruction: "Respond from your perspective. Be direct and
specific. Don't hedge or try to be balanced. Lean fully into your assigned angle. Keep your response
between 150–300 words. No preamble. Do not use any tools."

## Advisor — Contrarian (anonymous response C)

The strongest-sounding part is "every gate green, including a `TZ=Pacific/Auckland` job." That job doesn't prove D-1 is correct; it proves the suite was bent until host-zone dependence stopped hurting. Fourteen tests failed on real developer machines. The fix was to inject a fixed clock — which is exactly the argument against the default: the behaviour is only testable once you remove the behaviour.

D-1 violates the Core invariant on its face. "Deterministic, no IO" means two identical calls with identical inputs return identical results. `IsPast(someDate)` with no provider now returns `true` in Auckland and `false` in Los Angeles for roughly ten hours a day. A library asking "is this date in the past" that answers from `TimeProvider.System.LocalTimeZone` is sampling the environment. Calling it "DST-safe" is a distraction — the bug is the zone, not the transition. "Most servers run UTC" is a hope, not a contract, and it makes the failure invisible in production and visible only in support tickets from the Toronto-hosted tenant. Worst of all: D-14 promises no way to pick a zone, so the default is also the only behaviour short of subclassing `TimeProvider`. A default you can't override is not a default; it's policy.

The `DateTime.Today` precedent cuts the other way. `DateTime.Today` is widely regarded as a footgun .NET has spent a decade steering people away from, hence `TimeProvider` itself.

The risk you're avoiding: ratifying en bloc to end the stall, then shipping 1.0 with D-1 baked in. Do not re-open everything — D-2, D-4, D-9, the TimeSpan rename, the Tick renumber are pre-1.0 noise; ratify them. Re-open exactly D-1 and D-14 together: UTC date by default, zone opt-in via injected `TimeProvider.LocalTimeZone`, and the Auckland job repointed to assert UTC-independence rather than pinning the host behaviour.

First action: flip D-1 on the branch and watch which tests the Auckland job now catches. That tells you what the green was hiding.

## Advisor — Expansionist (anonymous response A)

The framing treats D-1 as a liability to be contained. It is actually the most valuable product surface in the whole program, and both options on the table undersell it.

**The hidden upside: PineGuard is the only validation library in .NET that can be honest about "today".** FluentValidation, DataAnnotations, Ardalis.GuardClauses all hard-code `DateTime.Now` or `UtcNow` and nobody can test them without a clock seam. Phase 1 already built the seam, shipped `FixedTimeProvider` in a public Testing package, and proved it with a `TZ=Pacific/Auckland` CI job. That is a marketing story: "the validator that passes in Auckland and Reykjavik". Ratify D-1 as host-local, because host-local is what makes the seam *matter*. A UTC default makes the `TimeProvider` parameter look like ceremony nobody needs; a host-local default makes injecting a clock the obviously-correct thing every tutorial shows. The default should be the one that teaches the feature.

**Adjacent opportunity D-14 forecloses too early.** "No zone-aware age overload is promised" is leaving money on the table. The single-sample-plus-offset derivation is already written; exposing `TimeZoneInfo zone` as an optional parameter on `MinimumAge`/`Past`/`Future` is a two-hour change that unlocks KYC, age-gating, and insurance customers where the *user's* zone, not the server's, is legally decisive. Downgrade D-14 from "never" to "deferred to Phase 2" and the program has a roadmap item instead of a closed door.

**Ratify the rest as-is in one commit.** The midnight-wrapping `TimeOnly` Between and the `IsLongerThan` rename are not drift, they are the library learning to speak temporal vocabulary other libraries lack. Write D-16 for the rename so it is on the record, then stop.

**First action:** flip every "Recommended" to "Ratified 2026-11-08", change D-14's wording to "deferred", merge Phase 1 today. Three plans and zero merges is the real risk to the upside; shipping is how you find out it works better than expected.

## Advisor — First Principles (anonymous response E)

**What are we actually solving?** Not "which zone is 'today' in." The goal is a temporal foundation that is correct by construction, shipped before 1.0, without a fourth planning cycle. Measure every row against that.

**The assumption to kill:** that D-1 is a choice between two zones. Both UTC-date and host-local-date read the wall clock; both violate "deterministic, no IO" identically. The 14 non-UTC failures were not evidence that host-local is dangerous — they were evidence that tests had always depended on an unpinned clock. The fixed-clock fix was the real correction and it is zone-independent. So D-1 is not a Core invariant question at all; the invariant question is *where the clock enters*. Answer from first principles: Core predicates take `today` as a parameter and stay pure; the null-provider fallback lives only in Must/Guard/Fluent/Annotations. If Phase 1 already has that shape, D-1 is a convenience-layer default and should be judged on surprise, not purity.

On surprise, host-local strictly dominates UTC: it is right on UTC servers *and* on the developer's machine, and matches `DateTime.Today`. UTC is wrong for ten hours a day in Auckland. The genuine defect is D-14 — there is no production way to supply a zone short of subclassing `TimeProvider`. That is the row the owner should actually care about, and it is additive, so it can follow the merge.

**Reframing of "ratify or re-open":** the rows are not peers. Everything embodied, tested at 100%, and reversible pre-1.0 (D-2, D-4, D-9, the rename, `Tick` renumbering) should be ratified en bloc in one commit — "re-open" for those means "re-plan," which is the failure mode you already know. Only two things merit a word: D-1 (ratify as implemented, state the Core-pure/convenience-default split explicitly) and D-14 (convert "not promised" into "Phase 2 backlog").

**First action:** one commit flipping all fourteen statuses to "Ratified — owner, 2026-11-08", D-14 amended to "deferred, additive". Then merge.

## Advisor — Outsider (anonymous response D)

Reading this cold, the question is not really "UTC or host-local." The question is "what does a stranger reading PineGuard's docs expect `Must.Be.InThePast(dateOnly)` to compare against?" — and the framing already answers it in one sentence it then buries: Core rules carry a "deterministic, no IO" invariant. Reading the wall clock *and* the OS time-zone table is two environment reads, not zero. D-1 as implemented violates the library's own stated rule. Do not ratify it. Default to the UTC date; make host-local an explicit injection. That is also the only option whose behaviour a newcomer can predict without knowing where the server lives.

Things in this brief that require prior context to make sense, and that an outside reader of the charter will trip on in the same way:

- "Astra final under owner authorization" — I do not know what Astra is. If it means "a reviewer approved it on the owner's behalf," say so; if it means "the owner approved it," the brief contradicts itself ("the owner never signed any of them").
- "Recommended" as a status on code that already ships is misleading. A row either governs the branch or it does not. Rename the column to Proposed / Ratified and fill it in.
- "D-12" is missing with no explanation. Add a one-line tombstone or a reader will assume a page is lost.
- The `IsLongerThan` rename and the `TimePrecision.Tick` renumbering are behaviour changes with no decision row. Anything an external user can observe needs a row, or the charter is not the record it claims to be.
- "`IsWithinDaysFromNow` says UTC now, `MinimumAge` says host today" — two adjacent methods with two different clocks and no shared sentence explaining why. That is the single most confusing thing here for a user.

First action: edit D-1 to "UTC date by default; zone via injected `TimeProvider.LocalTimeZone`," flip the code to match (the `TZ=Pacific/Auckland` job already exists to prove it), then mark every remaining row Ratified in one commit. Ratify the rest as-is; the inconsistencies above are documentation debt, not blockers.

## Advisor — Executor (anonymous response B)

Monday morning: flip D-1 to UTC-default, merge Phase 1. That is the whole plan.

**The first concrete action.** Open the null-provider "today" path (one helper, judging by "derived from one `GetUtcNow` sample"), change the zone from `TimeProvider.System.LocalTimeZone` to `TimeZoneInfo.Utc`, run the suite. The `TZ=Pacific/Auckland` CI job already exists and pins the behaviour; invert its expectation to "non-UTC host still gets the UTC date" and it becomes the regression guard for the invariant instead of a monument to the exception. The `FixedTimeProvider` tests already inject a fixed clock, so they do not move. This is a half-day cut, not a re-plan.

**Why UTC and not the implemented host-local.** Not theory: operations. Host-local means a bug report arrives from one developer's laptop and does not reproduce on CI or on the UTC server. You cannot execute on a defect that only exists in Auckland. UTC-default plus `LocalTimeZone` opt-in is the path where every environment agrees by default and the person who wants a zone states it. Changing this later is a 1.0 breaking change; changing it now is one line.

**Ratify everything else as-is.** D-2, D-4, D-9, the TimeSpan rename, the `Tick` renumber, the midnight-wrap asymmetry, the `IsWithinDaysFromNow` wording: each is implemented, tested at 100%, and documented. None has a cheaper moment than now, and none has a credible first step beyond "argue about it again". Write "Ratified 2026-11-08" in the status column of thirteen rows and "Re-opened and resolved: UTC default" in D-1. Add a D-row for the TimeSpan rename so it has one.

**No clear first step, so cut them:** the `TimeOnlyRange` reversed-bounds question and any zone-aware age overload (D-14 already says none is promised). Log them as post-merge tickets. Do not let either block the branch.

Merge target: end of day Monday.

## Peer reviews

Each reviewer received the framed question and the five responses labelled A–E (mapping above), and
was asked: which response is strongest and why; which has the biggest blind spot; what all five
missed. Under 200 words, no tools.

### Reviewer 1

**1. Strongest: C.** It is the only response that ties D-1 and D-14 into one defect ("a default you can't override is not a default; it's policy") and reads the green Auckland job correctly: it pins the exception rather than proving the invariant. Its first action (flip D-1, see what Auckland catches) is cheap and diagnostic. E's reframe — both zones read the clock; the real question is *where* the clock enters — is the one correction C needs, but E then undercuts itself by calling host-local "strictly dominant" while ignoring dev/CI/prod non-reproducibility (B's operational point).

**2. Biggest blind spot: A.** It chooses a default because it *teaches a feature*. A default that exists to make injection look necessary is a footgun by design, and A never engages with reproducibility or the Core invariant at all.

**3. What all five missed:** a `DateOnly` birth date is a civil date in the *user's* zone. Neither UTC nor host-local is "correct"; every null-provider default is a guess. The owner should ratify D-1 as a documented, deliberately-surprising fallback (whichever zone), make D-14 the real roadmap item, and adopt one explicit rule for all temporal predicates — `IsWithinDaysFromNow` already proves the library has two clocks and nobody made single-clock policy the decision criterion.

### Reviewer 2

**1. Strongest: C.** It lands the one argument that decides the question: the Auckland job does not validate D-1, it pins it, and the 14 failures were cured by removing the behaviour from the tests. Combined with "D-14 makes the default the only behaviour", it shows D-1 is policy, not a default. D is a close second for the newcomer-prediction test and the status-column fixes, which the owner actually needs.

**2. Biggest blind spot: A.** It treats D-1 as a marketing story and calls the zone parameter a "two-hour change", ignoring that Phase 1 touched ~114 files across five layers and a published package; a new overload is a vertical slice, not an afternoon. E shares a softer version: "host-local strictly dominates on surprise" ignores B's reproducibility point, which is the real operational cost.

**3. All five missed:**
- The `TZ=Pacific/Auckland` job is Linux-only. `TZ` does not move `LocalTimeZone` on Windows, so the environment where the 14 failures actually occurred is not covered by CI under either policy.
- `DateTime` MinimumAge interacts with D-2: a `Kind=Utc` birth date compared against a host-local "today" is a mixed-zone comparison no row addresses.
- Nobody checked whether Phases 2–6 assume D-1 as implemented, so "cheap to change now" is asserted, not verified.

### Reviewer 3

**1. Strongest: E.** It alone sees that UTC-date and host-local-date both read the clock, so the Core invariant is about *where* the clock enters (Core pure, convenience layers default), not which zone. That reframes D-1 from a purity fight into a surprise/consistency call and keeps the ratify-en-bloc discipline. Its conclusion is weaker than its analysis, though: "right on the developer's machine" assumes developer zone = data zone, and it waves off B/C's reproducibility point.

**2. Biggest blind spot: A.** It never mentions the dev/CI/prod divergence that B and C centre on, calls a zone overload across five layers plus Testing plus tests "a two-hour change", and treats a default nobody can override (D-14) as a marketing asset. C also overclaims — "deterministic on its face" indicts UTC equally.

**3. All five missed:**
- The opt-in is unreachable in two layers. `[MinimumAge]` attributes and static `Guard.Against` calls have no constructor seam for a `TimeProvider`. Whatever default is chosen *is* the behaviour there; D-1 and D-14 must be decided together with "how does DataAnnotations get a clock".
- "Production servers run UTC" is a Linux-container assumption. Windows/IIS hosts — a large share of .NET deployments — run host-local. Host-local default silently varies by *hosting platform*, not just by laptop.
- A `DateOnly` carries no zone, so neither default is "correct"; the documented contract matters more than the pick.

### Reviewer 4

**1. Strongest: C.** It is the only response that treats the green `TZ=Pacific/Auckland` job as evidence *against* the branch rather than for it, names the real defect (a default you cannot override "is policy", given D-14), and proposes a diagnostic first action (flip D-1, see what the job catches) rather than a declarative one. D reaches the same verdict and adds useful record hygiene, but C's argument is tighter.

**2. Biggest blind spot: E.** Its reframe ("both read the clock; the question is where the clock enters") is the best idea in the set, but it then judges "surprise" from the developer's laptop. On a UTC server host-local *is* UTC, so host-local buys nothing for Auckland users; it only buys dev/prod divergence, which B correctly identifies as the operational cost. E also hedges ("if Phase 1 already has that shape") on the one fact its conclusion depends on.

**3. All five missed:** DataAnnotations. Attribute constructor arguments must be constants, so `[MinimumAge]` cannot receive a `TimeProvider`; unless the attribute resolves one via `ValidationContext.GetService`, the null-provider default is the *only* behaviour for the entire annotation surface. That raises D-1's stakes well above "convenience default" and should force the question of whether the ambient fallback exists at all pre-1.0.

### Reviewer 5

**1. Strongest: C.** It is the only response that treats the green Auckland job as evidence about the suite rather than about D-1, and it correctly identifies that D-1 and D-14 are a single decision: a default nobody can override is policy, not a default. Its first action (flip D-1, watch what the Auckland job now catches) produces information, not just a merge. B is a close second on execution clarity but asserts "one line" without knowing the helper's shape.

**2. Biggest blind spot: A.** It argues host-local "teaches the feature", but the teaching mechanism it describes is tests breaking on developers' machines — the very failure mode the branch had to paper over with fixed clocks. It never engages the Core invariant, and it recommends adding a `TimeZoneInfo` overload in a "two-hour change" to a program whose charter explicitly declined that surface. E shares the blind spot partly: "host-local is right on the developer's machine" is only true if the developer is the user.

**3. All five missed:** nobody asked whether "today" even belongs in Core. E gestured at it (Core takes `today` as a parameter) but did not check Phase 1 for that shape. If Core predicates already take an explicit date and the fallback lives in the adapter layers, the invariant debate is moot and D-1 is purely a convenience default; if not, that is the fix, and the zone argument is downstream. Also missed: documenting the default *visibly* — an XML remark stating which clock each method reads, so adjacent methods (`IsWithinDaysFromNow` vs `MinimumAge`) stop disagreeing silently — regardless of which way D-1 lands.

## Chairman verdict

The chairman received the framed question, the five advisor responses with their persona names, the
five peer reviews, and three facts from the orchestrator's code reading that the reviewers had
flagged as unverified: the opt-in is reachable in DataAnnotations through
`ValidationContext.GetService(typeof(TimeProvider))` (33 call sites); pre-Phase-1 Core already read
`DateTime.UtcNow` for "today", so the zone read, not the clock read, is the new environment
dependence; and the class-level and attribute remarks documenting the default were already written
in `acc6970`.

### Council Verdict: Temporal charter D-rows and the D-1 "today" default

#### Where the Council Agrees

- **Do not re-open the rows en bloc.** All five advisors, independently, said the same thing about D-2, D-4, D-9, the `IsLongerThan/IsShorterThan` rename, the `TimePrecision.Tick` renumber, the `TimeOnly` midnight-wrap asymmetry and the `IsWithinDaysFromNow` wording: implemented, covered at 100%, reversible pre-1.0, no credible first step beyond "argue again". Ratify them in one commit. This is the highest-confidence signal the council produced.
- **D-1 and D-14 are one decision, not two.** Three advisors and four reviewers converged on this: the meaning of the null-provider default depends entirely on how reachable the override is.
- **The Auckland job is evidence about the suite, not about D-1.** It pins whatever the null path does; it would be equally green under a UTC default with the expectation inverted. Nobody defended it as validation of the policy.
- **Record hygiene is debt, not a blocker.** The missing D-12 tombstone, the "Recommended" status on shipping code, "Astra final under owner authorization" needing a gloss, and the rename lacking a row were raised by the Outsider and endorsed by reviewers. Fix them in the ratification commit, do not let them gate the merge.
- **Phase 1 must merge this cycle.** Even the advisors who want D-1 flipped sized the flip at one helper, not a re-plan. Three plans and zero merges is the failure mode everyone is steering around.

#### Where the Council Clashes

**UTC date vs host-local date for the null-provider "today".** Contrarian, Outsider and Executor say UTC; First Principles and Expansionist say host-local. Peer review rated the Contrarian strongest in four of five reviews, but the First Principles reframe exposed a real weakness in the UTC camp's main argument.

*The host-local case:* both options read the wall clock, so "deterministic, no IO" indicts them identically; the invariant question is where the clock enters, not which zone it reads. Given a clock read, host-local is right on UTC servers *and* on the developer's laptop, matches `DateTime.Today`, and makes the `TimeProvider` seam visibly matter. The 14 failures were an unpinned-clock bug in the tests, cured correctly, and would have existed under any default.

*The UTC case:* the Core invariant argument is indeed overclaimed, but reproducibility is not. Host-local produces a defect that exists on a developer's laptop and vanishes on CI and on the UTC container; the Executor is right that you cannot operate on a bug that only exists in Auckland. The "servers run UTC" premise is a Linux-container assumption; Windows/IIS hosts run host-local, so host-local makes the library's answer vary by hosting platform, not just by developer. And the orchestrator's facts settle the one point First Principles hedged on: the clock read in Core is not new (pre-Phase-1 Core already read `DateTime.UtcNow`), but the *zone* read is. The question is precisely whether to add a second environment dependence to a path that previously had one.

Reasonable advisors disagree because they weigh different populations: First Principles optimises for the single developer whose data zone equals their machine zone; the UTC camp optimises for the team whose dev, CI and prod must agree. For a library, the second population is the one that files bug reports.

#### Blind Spots the Council Caught

- **Neither default is "correct".** A `DateOnly` birth date is a civil date in the *user's* zone, which no server-side default can know. Every null-provider "today" is a guess; the only honest contract is a stated, predictable guess plus an opt-in (Reviewer 1). This reframes D-1 from "which answer is right" to "which guess is easiest to predict and reproduce", and UTC wins that framing outright.
- **The opt-in is reachable everywhere, including DataAnnotations.** Reviewers 3 and 4 feared attributes could not receive a `TimeProvider`; the code refutes this (`ValidationContext.GetService`, 33 call sites). This matters in the UTC camp's favour: a UTC default does not strand anyone, because a host that registers a `TimeProvider` with `LocalTimeZone` set controls every layer. The Contrarian's "a default you can't override is policy" is therefore false as stated, but the weaker form survives: short of subclassing, there is no way to say "this zone" per call (D-14).
- **The Auckland job does not cover the environment where the failures happened.** It is Linux-only; `TZ` does not move `LocalTimeZone` on Windows (Reviewer 2). Under host-local, Windows developers remain the uncovered population. Under UTC, the null path stops depending on the host zone and the gap closes by construction.
- **`DateTime` MinimumAge is a mixed-zone comparison no row addresses.** A `Kind=Utc` birth instant compared against a host-local "today" is two zones in one predicate (Reviewer 2). A UTC default makes the comparison single-zone; host-local leaves a D-2/D-1 interaction undocumented.
- **Two clocks, two adjacent methods.** `IsWithinDaysFromNow` reads UTC now; `MinimumAge` reads host-local today. The Outsider flagged it as the most confusing thing for a user, and Reviewer 1 generalised it to "adopt one explicit clock rule". A UTC default resolves the inconsistency for free; host-local requires either changing `IsWithinDaysFromNow` too or writing a paragraph explaining why they differ.
- **Nobody verified that Phases 2-6 are D-1-neutral.** "Cheap to change now" was asserted. Still true for the code (one helper), but the plan set must be grepped for assumptions before the status flip.
- **"Document the default visibly" is largely done** (class-level remarks, 25 attribute remarks in acc6970), so Reviewer 5's mitigation is not an open item, only a wording update if D-1 flips.

#### The Recommendation

**Flip D-1 to the UTC calendar date when no `TimeProvider` is injected; honour `provider.LocalTimeZone` when one is. Ratify the other thirteen rows as implemented. Merge Phase 1.**

This overrules the First Principles / Expansionist minority and it also partially overrules the Contrarian: the Core invariant is not the reason, because both defaults read a clock and Core always has. The reasons are the ones that survived peer review:

1. **Reproducibility.** UTC is the only default under which a developer's laptop, the Linux CI runner, the Windows CI runner, a UTC container and an IIS host all return the same answer for the same inputs. Host-local is wrong on exactly the machines where bugs are first noticed and right on exactly the machines where nobody is looking.
2. **It restores pre-Phase-1 behaviour.** The zone read, not the clock read, is the new environment dependence; removing it returns the library to a contract it already had, with the new injectable zone-aware path as a strict addition.
3. **One clock rule.** `IsWithinDaysFromNow` already says UTC. With a UTC default every temporal predicate reads the same clock unless the caller says otherwise, and the `DateTime` MinimumAge mixed-zone case disappears.
4. **The opt-in is real in every layer.** Because DataAnnotations resolve `TimeProvider` from DI, a UTC default strands nobody; a host that wants its zone sets `LocalTimeZone` once. The Expansionist's "the default should teach the feature" is answered: the feature is taught by the one-line DI registration that makes the whole app zone-aware, not by tests breaking on laptops.
5. **It is permanent after 1.0 and one helper today.**

Specific rulings for the ratification commit:

- **D-1:** "Re-opened and resolved 2026-11-08: null provider reads the UTC calendar date; an injected `TimeProvider` is read in its `LocalTimeZone`." Update the class-level remark and the 25 attribute remarks to match.
- **D-14:** reword from "no zone-aware age overload is promised" to "deferred, additive: zone-awareness via `TimeProvider.LocalTimeZone` is the supported path; a per-call `TimeZoneInfo` overload is Phase 2 backlog, to be decided with the user-zone KYC case in hand." Do not build it now; it is a five-layer vertical slice plus a published package, not a two-hour change.
- **D-2, D-4, D-9:** Ratified as implemented. D-4's UTC default for `FixedTimeProvider` is now consistent with D-1 rather than an exception to it.
- **New D-16:** the TimeSpan rename, so an observable behaviour change has a row. **D-12:** one-line tombstone.
- **Status column:** rename to Proposed / Ratified; one-sentence gloss for "Astra final under owner authorization" so the record does not contradict itself.
- **CI:** invert the `TZ=Pacific/Auckland` job's expectation to "non-UTC host still yields the UTC date"; it becomes the regression guard for the invariant instead of a monument to the exception. The existing `FixedTimeProvider` tests with a non-UTC `LocalTimeZone` guard the opt-in path.
- **Not blockers, log as post-merge tickets:** `TimeOnlyRange` reversed bounds; any per-call zone overload; a Windows CI job for the zone-aware path if the owner wants belt and braces.

#### The One Thing to Do First

Change the null-provider branch of `DateTimeUtility.TryGetTodayDate` (Core Utils, `DateTimeUtility.cs:57-61`) so that a null `timeProvider` yields the UTC calendar date from `TimeProvider.System.GetUtcNow()`, while a supplied provider is still read in its `LocalTimeZone`; then run the full suite on your (non-UTC, Windows) machine before touching a single doc. Whatever fails is the exact list of tests and remarks the host-local default was propping up, and it is the complete scope of the change. The status flips, the D-16/D-12 edits and the Auckland job inversion follow in the same commit once that list is green.

## Disposition (2026-11-09)

- The orchestrator presented the verdict to the owner on 2026-11-08 and noted that GPT-6 Astra's
  independent Phase 2 review had recommended UTC for the same reasons.
- The owner ratified the register and the D-1 flip on 2026-11-09. Wording adopted for the
  contract: "PineGuard never guesses your zone. Without an injected clock, dates are UTC; inject a
  `TimeProvider` to choose a zone."
- Applied on `codex/temporal-foundation`: `TryGetTodayDate` null branch → UTC calendar date
  (offset zero; an injected provider's `LocalTimeZone` is honoured); the null-provider test renamed
  `TryGetTodayDate_NullProviderUsesUtcDate` and its expectation inverted; the
  `core-null-provider-timezone` CI job repointed at it; the remarks on `DateOnlyRules`,
  `DateTimeRules` and the fifteen DataAnnotations attribute paragraphs reworded. The chairman's
  predicted failure list was empty: the full suite was already green on the Windows host because
  every legacy test injects a fixed clock, so the change surfaced in the one dedicated test only.
- Applied on `main` in T-00: D-1 rewritten; D-14 reworded; D-16 added; D-12 tombstone; status
  vocabulary note with the "Astra final under owner authorization" gloss; `council_verdict_ref`
  stamped to this file. The Phases 2–6 grep for D-1 assumptions found one sentence in T-04
  (clock/configuration contract), amended in the same commit.
- Post-merge tickets per the verdict: `TimeOnlyRange` reversed bounds; per-call `TimeZoneInfo`
  overload (D-14, T-04 candidate); optional Windows CI job for the zone-aware path.

<!-- footer
last_verified: 2026-11-09
-->
