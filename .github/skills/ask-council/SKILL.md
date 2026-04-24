---
name: ask-council
description: Pressure-test a decision through a council of five independent advisor perspectives, an anonymous peer-review round, and a chairman synthesis. Based on Karpathy's LLM Council methodology.
---
# Skill: Ask Council

## Load First

Read these files before convening:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [council.md](../../../docs/ai/specs/council.md)
3. [roles/council.md](../../../docs/ai/roles/council.md)
4. [ask-council/SKILL.md](../../../docs/ai/skills/ask-council/SKILL.md)

## Execute

Follow the canonical recipe in [docs/ai/skills/ask-council/SKILL.md](../../../docs/ai/skills/ask-council/SKILL.md) exactly.

Key invariants (from `docs/ai/specs/council.md`):
- Advisors and reviewers are each spawned as a single parallel batch.
- Reviewers see responses anonymized as `A`–`E` using a random permutation.
- Chairman output uses the five-section schema — no extras, no omissions.

## Verify

- Chairman verdict rendered in the conversation using the five-section schema.
- Transcript saved only when plan-gated or explicitly requested.
- Keep the Brain as the source of truth.
