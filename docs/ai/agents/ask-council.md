<!-- metadata_header
type: agent
id: agent-ask-council
version: 1.0
-->

# Agent: Ask Council (Pressure-Test a Decision)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: architect ([../roles/architect.md](../roles/architect.md)), council ([../roles/council.md](../roles/council.md))

## Context

Convene a five-advisor council to pressure-test a decision, a plan recommendation, or a strategic trade-off. Follows the canonical procedure in [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md) and the normative rules in [`../specs/council.md`](../specs/council.md).

## Steps

1. **Decide whether to convene.**
   - Apply the "when to run" gate in [`../specs/council.md`](../specs/council.md).
   - Trivial question, one right answer, zero stakes → answer directly and stop.
   - Plan-gated (see spec triggers) or stakes-significant → continue.

2. **Frame the question (skill Step 1).**
   - Read [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md) §4 Step 1.
   - Enrich from `CLAUDE.md`, `AGENTS.md`, referenced plan files, and any user-attached files (≤30 s, 2–3 files).
   - Produce the neutral framed question.

3. **Convene the council (skill Step 2).**
   - Spawn five advisor sub-agents in a single batched tool call.
   - Each advisor uses the persona from [`../roles/council.md`](../roles/council.md).
   - Each response: 150–300 words, no hedging.

4. **Peer review (skill Step 3).**
   - Randomly permute advisor → `Response A`–`Response E`.
   - Spawn five reviewer sub-agents in a single batched tool call against the anonymized set.

5. **Chairman synthesis (skill Step 4).**
   - Spawn one chairman sub-agent with the de-anonymized responses and the five peer reviews.
   - Output must follow the five-section schema in [`../specs/council.md`](../specs/council.md).

6. **Present the verdict (skill Step 5).**
   - Render the chairman's verdict in chat using the markdown schema.

7. **Transcript (skill Step 6, conditional).**
   - If plan-gated: write `docs/ai/plans/<plan>/council-transcript.md` and stamp the plan's `metadata_header` with `council_verdict_ref`.
   - If user requested: write `artifacts/council/council-transcript-<yyyyMMdd-HHmm>.md`.
   - Otherwise skip.

## Definition of Done

- [ ] Five advisor responses produced in parallel
- [ ] Peer review ran against anonymized responses (mapping recorded)
- [ ] Chairman verdict rendered in chat in the five-section schema
- [ ] "The One Thing to Do First" is a single concrete action
- [ ] Transcript saved only when required

## References

- [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md)
- [`../roles/council.md`](../roles/council.md)
- [`../specs/council.md`](../specs/council.md)
- [`../commands/council.md`](../commands/council.md)
- [`../workflows/plan-with-council.md`](../workflows/plan-with-council.md)
