# Workflow: Plan with Council

> [!NOTE]
> Draft or revise a plan under `docs/ai/plans/`, pressure-test its recommendation through the council, then revise the plan using the verdict. Required by [`../specs/council.md`](../specs/council.md) §1 for plans crossing the gate.

## Context

- **Role**: [Architect](../roles/architect.md), [Council](../roles/council.md)
- **Reference**: [`../agents/ask-council.md`](../agents/ask-council.md), [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md), [`../specs/council.md`](../specs/council.md)

## Parameters

- **Plan path**: (required) absolute or repo-relative path to a plan file under `docs/ai/plans/`.
- **Trigger reason**: (optional) which §1 trigger applied — API, scope, cascade, release/IP, explicit flag. Recorded in the plan's `metadata_header`.

## Auto-Approval

Not auto-approved on any adapter. The council is intentional.

## Steps

1. **Draft the plan.**
   - Ensure the plan has a clear "Recommendation" section and a "One Thing to Do First" (or equivalent).
   - Stamp the `metadata_header` with `status: Planned`.

2. **Confirm the gate.**
   - Re-read [`../specs/council.md`](../specs/council.md) §1. Confirm the plan meets at least one trigger.
   - If it does not, do not run the council — move directly to `status: Active` via normal review.

3. **Frame the council question.**
   - Produce a single framed question derived from the plan's Recommendation + the trade-offs the author is least sure about.
   - Include relevant context from the plan body (≤3 snippets).

4. **Run the council.**
   - Follow [`../agents/ask-council.md`](../agents/ask-council.md) Steps 2–5 exactly.
   - Parallel advisor spawning, anonymized peer review, chairman synthesis.

5. **Persist the transcript.**
   - Save to `docs/ai/plans/<plan-slug>/council-transcript.md`.
   - Include: framed question, advisor responses, peer reviews, anonymization map, chairman verdict.

6. **Revise the plan.**
   - Integrate the chairman's "Recommendation" and "One Thing to Do First".
   - Address the "Blind Spots" — every blind spot must either be resolved in the plan text or explicitly noted as accepted risk.
   - Re-check the "Council Clashes" section — if a clash is unresolved, the plan must state which side was chosen and why.

7. **Stamp the plan.**
   - Update the plan's `metadata_header`:
     ```yaml
     council_verdict_ref: docs/ai/plans/<plan-slug>/council-transcript.md
     council_verdict_date: <yyyy-MM-dd>
     council_trigger: <api|scope|cascade|release-ip|explicit>
     ```
   - Move `status: Planned` → `status: Active`.

8. **Commit.**
   - One commit: plan revision + transcript, message `plan(council): <plan-slug> — verdict applied`.

## Definition of Done

- [ ] Plan meets at least one §1 trigger, or the workflow halts at Step 2.
- [ ] Council session completed per [`../agents/ask-council.md`](../agents/ask-council.md).
- [ ] Transcript saved alongside the plan.
- [ ] Plan revised so every blind spot is addressed or accepted explicitly.
- [ ] `metadata_header` stamped with `council_verdict_ref`.
- [ ] Plan promoted to `status: Active`.

## References

- [`../specs/council.md`](../specs/council.md)
- [`../agents/ask-council.md`](../agents/ask-council.md)
- [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md)
- [`../commands/council.md`](../commands/council.md)
