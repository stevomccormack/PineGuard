---
name: fix-roslyn
description: Fix Roslyn compiler warnings. Use when asked to fix CS warnings, compiler diagnostics, or nullability issues.
---

# Skill: Fix Roslyn Compiler Diagnostics

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/tools/code-diagnostics/spec.md` (warning categories, scopes, fix rules)
2. `docs/ai/specs/coding-standard.md` (formatting, naming)
3. `docs/ai/rules/roslyn.md` (diagnostics-specific rules)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/fix-roslyn/SKILL.md` exactly as written — it owns the diagnostics command
line and the fix-one-file-at-a-time loop.
Do NOT suppress warnings. Fix the root cause.

## Step 2: Verify
- Solution builds cleanly
- Summary of fixed vs skipped warnings provided
