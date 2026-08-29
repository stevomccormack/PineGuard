---
spec:
  id: pineguard.ai.council
  title: "LLM Council (Normative)"
  version: 1
  parent:
    - spec.md
  dependencies:
    - dependencies.md
applies_to:
  - "docs/ai/plans/**"
  - "docs/ai/skills/ask-council/**"
---

# LLM Council (Normative)

> [!NOTE]
> Defines when the ask-council procedure MUST run, MUST NOT run, and the invariants every implementation must respect. Procedural detail lives in [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md); advisor personas in [`../roles/council.md`](../roles/council.md); command contract in [`../commands/council.md`](../commands/council.md).

## Context

The council is expensive (≥10 sub-agent invocations) and changes the tone of the session (the agent stops answering directly and spawns reviewers). It must therefore only run on decisions where multi-perspective review is worth the cost. This spec defines the gate.

## 1. When the Council MUST Run

A plan file under `docs/ai/plans/` MUST be pressure-tested by the council **before its status moves from `Planned` → `Active`** if it meets **any** of:

1. **Public API impact** — changes exported symbols in `src/PineGuard.*` or the `netstandard2.1`/`net8.0`/`net10.0` surface.
2. **Cross-project scope** — affects ≥3 projects in `src/` or `tests/`.
3. **Protocol / spec cascade** — modifies files under `docs/ai/specs/` that other specs inherit from (`spec.md`, `protocol.md`, `dependencies.md`, `safety.md`).
4. **Release / IP implications** — affects NuGet packaging, licensing, sync to the public PineGuard mirror, or separation of IP between Internal and public repos.
5. **Explicit flag** — the plan's `metadata_header` declares `council_required: true`.

When a plan passes the gate, its `metadata_header` MUST include `council_verdict_ref: <path-to-transcript.md>` before the status changes.

## 2. When the Council SHOULD Run (Non-Plan)

Ad-hoc questions qualify when the user explicitly asks (`/ask-council` or any mandatory trigger phrase from [`../commands/council.md`](../commands/council.md)) **and** the question presents a real trade-off with non-trivial cost-of-being-wrong.

## 3. When the Council MUST NOT Run

- Factual lookups with one right answer ("what targets does `PineGuard.Core` support?").
- Creation tasks ("write a README", "draft a commit message").
- Processing tasks ("summarize this file", "list the public methods").
- Low-stakes phrasing without a real trade-off ("should I use markdown tables here?").
- Questions about already-decided matters where the user is seeking validation — the council will tell them things they did not want to hear; if validation is the goal, answer directly and move on.

If any of the above applies, the agent MUST answer directly and decline to convene.

## 4. Invariants

Every council session MUST satisfy all of:

1. **Parallelism** — Step 2 (advisors) and Step 3 (reviewers) each spawn all five sub-agents in a single batched tool call. Sequential spawning is a defect.
2. **Anonymization** — Step 3 reviewers see responses labeled `A`–`E` using a random permutation. The advisor↔letter mapping is private to the orchestrating agent until Step 4.
3. **No hedging (advisors)** — each advisor response leans fully into its persona. The chairman alone reconciles.
4. **Chairman independence** — the chairman MAY overrule the majority when a minority argument is strongest, and MUST state the reason.
5. **Output schema fidelity** — the verdict is rendered in chat using exactly the five sections defined in §5 below, in that order, with those headings.
6. **Length caps** — advisors 150–300 words; peer reviews under 200 words; "The One Thing to Do First" is a single action, not a list.
7. **No side files by default** — do not write HTML, PDFs, or auxiliary documents. Transcripts are written only under the conditions in §6.
8. **Model: Fable** — every sub-agent spawned by the council (the five advisors, the five peer reviewers, and the chairman) runs on the Fable model. The council exists specifically for judgment the orchestrating session should not make alone; Fable is this repo's designated high-reasoning tier for exactly that kind of call (`docs/ai/plans/new-surfaces-orchestration.md` §4), and using it uniformly across all eleven sub-agents keeps every voice — including the chairman reconciling them — at the same reasoning tier.

## 5. Output Structure (Chairman Verdict)

```markdown
## Council Verdict: {short topic}

### Where the Council Agrees
### Where the Council Clashes
### Blind Spots the Council Caught
### The Recommendation
### The One Thing to Do First
```

Each section MUST be present. "Where the Council Clashes" MUST name at least one genuine disagreement when one exists; it MUST NOT smooth over differences. "The Recommendation" MUST be a direct answer — not "it depends."

## 6. Transcript Persistence

Write a transcript ONLY when:

- The session is plan-gated per §1 → write `docs/ai/plans/<plan-slug>.council-transcript.md` and stamp the plan's `metadata_header` with `council_verdict_ref`.
- The user explicitly requests it → write `artifacts/council/council-transcript-<yyyyMMdd-HHmm>.md`.

Transcripts MUST include: framed question, all five advisor responses, all five peer reviews, the anonymization mapping used, and the chairman verdict.

## 7. Cost & Safety

- Token budget: one session is roughly equivalent to 10–12 normal agent turns. Do not auto-invoke the council in hooks, CI, or background tasks.
- Safety: the council inherits [`safety.md`](safety.md). Sub-agents do not execute destructive commands; they produce text only.
- Scope containment: Step 1 context enrichment is capped at 2–3 files and 30 seconds.

## References

- [`../skills/ask-council/SKILL.md`](../skills/ask-council/SKILL.md)
- [`../agents/ask-council.md`](../agents/ask-council.md)
- [`../commands/council.md`](../commands/council.md)
- [`../roles/council.md`](../roles/council.md)
- [`../workflows/plan-with-council.md`](../workflows/plan-with-council.md)
- [`safety.md`](safety.md)

<!-- footer
last_verified: 2026-04-24
-->
