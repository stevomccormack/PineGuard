---
name: scan-roslyn
description: Run Roslyn compiler diagnostics and report CS warnings. Use whenever the user says "check warnings", "what are the CS warnings", "run Roslyn", "show compiler diagnostics", "how many warnings", or wants a build diagnostic report before fixing. Do NOT use to fix warnings; use fix-roslyn instead.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: analysis
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
