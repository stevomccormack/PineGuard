---
name: ask-council
description: Run any question, idea, or decision through a council of 5 AI advisors who independently analyze it, peer-review each other anonymously, and synthesize a final verdict. Based on Karpathy's LLM Council methodology. MANDATORY TRIGGERS - "council this", "run the council", "war room this", "pressure-test this", "stress-test this", "debate this". STRONG TRIGGERS (use when combined with a real decision or tradeoff) - "should I X or Y", "which option", "what would you do", "is this the right move", "validate this", "get multiple perspectives", "I cant decide", "Im torn between". Do NOT trigger on simple yes/no questions, factual lookups, or casual "should I" without a meaningful tradeoff. DO trigger when the user presents a genuine decision with stakes, multiple options, and context that suggests they want it pressure-tested from multiple angles.
argument-hint: "[question or decision]"
context: fork
allowed-tools: Read, Glob, Grep, Write, Task
metadata:
  author: stevomccormack
  version: 1.0.0
  category: decision-support
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
- Step 2 (5 advisors) and Step 3 (5 reviewers) MUST each spawn in a single batched Task call.
- Anonymize advisor → Response A–E with a random permutation before Step 3.

## Step 3: Verify
- Chairman verdict rendered in chat using the five-section schema.
- No side files unless the session is plan-gated per `docs/ai/specs/council.md` §6.
