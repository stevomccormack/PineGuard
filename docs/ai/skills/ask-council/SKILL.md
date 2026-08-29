# Skill: Ask Council

**ID**: pineguard.skill.ask-council
**Version**: 1.0

## 1. Context & Goal

Pressure-test a decision, plan, or open question through a council of five independent advisor perspectives, an anonymous peer-review round, and a chairman synthesis. Produces a single, actionable verdict rather than a balanced non-answer.

Adapted from Karpathy's LLM Council methodology. Five advisor archetypes (Contrarian, First Principles Thinker, Expansionist, Outsider, Executor) plus a Chairman are defined in [`docs/ai/roles/council.md`](../../roles/council.md). Normative rules (when the council MUST run, anonymization invariant, output structure) live in [`docs/ai/specs/council.md`](../../specs/council.md).

## 2. Inputs

- **Question**: a decision, plan recommendation, strategy, or trade-off the user wants pressure-tested.
- **Context Files** (auto-discovered): `CLAUDE.md`, `AGENTS.md`, `docs/ai/plans/*.md`, any file the user attaches.
- **Save Transcript**: optional — `true` persists the session under `docs/ai/plans/<plan>.council-transcript.md` or `artifacts/council/`.

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
>
> 1. **Refuse trivial questions.** If there is one right answer, no real trade-off, or zero stakes, do not convene the council. Answer directly and stop.
> 2. **Always spawn in parallel.** Steps 2 and 3 spawn five sub-agents each, all in a single tool-call batch. Sequential spawning leaks earlier responses into later ones.
> 3. **Anonymize peer review.** Before Step 3, map advisors to `Response A`–`Response E` with a random permutation. Reviewers never see which advisor authored which response.
> 4. **No hedging in advisor responses.** Each advisor leans fully into their angle. Balance is the chairman's job, not theirs.
> 5. **Chairman may overrule the majority.** If a minority argument is strongest, the chairman sides with it and explains why.
> 6. **Present the verdict in chat.** Do not generate HTML reports or side files unless the user asked to save the transcript.
> 7. **Respect scope containment.** Context enrichment (Step 1A) is capped at 30 seconds and 2–3 highly relevant files; do not read the whole repo.

## 4. Execution Steps

### Step 1 — Frame the Question

**1A. Enrich with context.** Glob for and read (max 2–3 files, ≤30 s):
- `CLAUDE.md`, `AGENTS.md` at repo root
- Any `docs/ai/plans/*.md` referenced by the user
- Any file the user attached
- Any `docs/ai/memory/*.md` relevant to the question

**1B. Reframe the question.** Produce a neutral prompt containing:
1. The core decision
2. Key context from the user's message
3. Key context from the enriched files (constraints, past decisions, relevant numbers)
4. What is at stake

If the question is too vague to frame, ask **one** clarifying question, then proceed.

Save the framed question — it is reused verbatim in Steps 2, 3, and 4.

### Step 2 — Convene the Council (5 sub-agents in parallel)

Spawn five sub-agents in a single batched tool call. Each receives:
1. Its advisor identity and thinking style from [`docs/ai/roles/council.md`](../../roles/council.md)
2. The framed question
3. The instruction to lean fully into its angle (150–300 words, no preamble, no hedging)

Template: see [`references/README.md`](references/README.md) "Advisor Prompt".

### Step 3 — Peer Review (5 sub-agents in parallel, anonymized)

1. Assign the five advisor responses to letters `A`–`E` using a random permutation. Record the mapping privately.
2. Spawn five reviewer sub-agents in a single batched tool call. Each sees all five anonymized responses and answers:
   - Which response is strongest and why?
   - Which response has the biggest blind spot?
   - What did all five responses miss?

Template: see [`references/README.md`](references/README.md) "Reviewer Prompt".

### Step 4 — Chairman Synthesis

Spawn a single chairman sub-agent with: the framed question, all five de-anonymized advisor responses, and all five peer reviews. It produces the verdict using the exact five-section structure defined in [`docs/ai/specs/council.md`](../../specs/council.md).

Template: see [`references/README.md`](references/README.md) "Chairman Prompt".

### Step 5 — Present the Verdict

Render the chairman's output in chat as markdown using the schema:

```
## Council Verdict: {short topic}

### Where the Council Agrees
### Where the Council Clashes
### Blind Spots the Council Caught
### The Recommendation
### The One Thing to Do First
```

No HTML. No side files — unless Step 6 triggers.

### Step 6 — Save Transcript (optional)

Save only if the user asked, or the question is plan-gated by [`docs/ai/specs/council.md`](../../specs/council.md):

- **Plan-gated**: write `docs/ai/plans/<plan-slug>.council-transcript.md` and stamp the plan's `metadata_header` with `council_verdict_ref: <path>`.
- **Ad-hoc**: write `artifacts/council/council-transcript-<yyyyMMdd-HHmm>.md`.

Transcript includes: framed question, all five advisor responses, all five peer reviews, the anonymization map, and the chairman verdict.

## 5. Definition of Done

- [ ] Framed question captured
- [ ] Five advisor responses produced (150–300 words each, no hedging)
- [ ] Five peer reviews produced against anonymized inputs
- [ ] Chairman verdict rendered in chat using the exact five-section schema
- [ ] Single "One Thing to Do First" is concrete and actionable
- [ ] Transcript saved only if required (Step 6)

## 6. Reference Material

- [`docs/ai/specs/council.md`](../../specs/council.md) — normative rules and triggers
- [`docs/ai/roles/council.md`](../../roles/council.md) — advisor personas and chairman
- [`docs/ai/commands/council.md`](../../commands/council.md) — command contract
- [`docs/ai/workflows/plan-with-council.md`](../../workflows/plan-with-council.md) — plan orchestration
- [`references/README.md`](references/README.md) — prompt templates and example verdicts
