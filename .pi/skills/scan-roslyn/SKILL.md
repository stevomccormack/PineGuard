---
name: scan-roslyn
description: Run Roslyn compiler diagnostics. Use when asked to check compiler warnings, CS warning codes, or build diagnostics. Do NOT use to fix warnings; use fix-roslyn instead.
---

# Skill: Run Roslyn Compiler Diagnostics

## Step 0: Load Specifications (MANDATORY — read before running)
Read these files completely:
1. `docs/ai/specs/tools/code-diagnostics/spec.md` (warning categories, scopes, fix rules)
2. `docs/ai/rules/roslyn.md` (diagnostics-specific rules)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scan-roslyn/SKILL.md` exactly as written.

## Step 2: Verify
- Build completed without errors
- Warning summary reported (count by code, count by file)
- JSON artifact written to `artifacts/code-diagnostics/<scope>/`
