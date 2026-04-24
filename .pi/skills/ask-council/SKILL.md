---
name: ask-council
description: Pressure-test a decision through five independent advisors, an anonymous peer-review round, and a chairman synthesis.
---

# Skill: Ask Council

## Step 0: Load Specifications (MANDATORY)
Read these files:
1. `docs/ai/specs/council.md` (when MUST run, invariants, output schema)
2. `docs/ai/roles/council.md` (advisor personas and chairman)
3. `docs/ai/skills/ask-council/SKILL.md` (canonical recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/ask-council/SKILL.md` exactly as written.

## Step 2: Convene
- Spawn the 5 advisor sub-agents in one parallel batch.
- Spawn the 5 reviewer sub-agents in one parallel batch against anonymized responses.
- Chairman is spawned last, alone, with de-anonymized responses and all peer reviews.

## Step 3: Verify
- Chairman verdict rendered in chat using the five-section schema.
- No side files unless the session is plan-gated or the user requested a transcript.
