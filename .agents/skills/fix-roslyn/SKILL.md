---
name: roslyn-fix
description: Fix Roslyn compiler warnings. Use whenever the user says "fix warnings", "fix CS8600", "fix nullability issues", "clean up compiler warnings", "fix CS diagnostics", or wants compiler warnings resolved. Always fix the root cause — do NOT suppress. Trigger on any CS-prefixed warning fix request.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: analysis
---
# Skill: Fix Roslyn Compiler Diagnostics

## Step 0: Load Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/tools/code-diagnostics/spec.md` (warning categories, scopes, fix rules)
2. `docs/ai/specs/coding-standard.md` (formatting, naming)
3. `docs/ai/rules/roslyn.md` (diagnostics-specific rules)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/fix-roslyn/SKILL.md` exactly as written.
Do NOT suppress warnings. Fix the root cause.

## Step 2: Iterative Loop
1. Run diagnostics: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope [SCOPE] -OutputFormat Json`
2. Fix one file at a time
3. Build: `dotnet build PineGuard.slnx --no-incremental`
4. Repeat until all fixable warnings resolved

## Step 3: Verify
- Solution builds cleanly
- Summary of fixed vs skipped warnings provided
