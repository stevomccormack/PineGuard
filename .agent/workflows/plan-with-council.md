---
description: Draft or revise a plan, pressure-test its recommendation through the LLM Council, then revise using the verdict.
---

1. Read and execute `docs/ai/workflows/plan-with-council.md`.
2. Respect the invariants in `docs/ai/specs/council.md` (parallel spawning, anonymized peer review, five-section output schema).
3. Do not auto-approve (`// turbo-all` is intentionally omitted) — the council is an explicit action.
