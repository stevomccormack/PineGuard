---
name: format-code
description: Run dotnet format to enforce .editorconfig rules. Use before committing, after bulk edits, or whenever the user says "format my code", "run dotnet format", "fix formatting", "clean up whitespace", "editorconfig violations", or "formatting drift". Trigger on any request to format or tidy code.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: maintenance
---
# Skill: Format Code

## Step 0: Load Specifications (MANDATORY)
Read these files:
1. `docs/ai/specs/coding-standard.md` (formatting rules)
2. `docs/ai/skills/format-code/SKILL.md` (canonical recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/format-code/SKILL.md` exactly as written.

## Step 2: Format
- Scope-specific: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-formatter/Run-Format.ps1" -Scope [ScopeName]`
- Or direct: `dotnet format [path-to-project-or-solution]`

## Step 3: Verify
- `dotnet format --verify-no-changes [path]` exits with code 0
- No formatting drift remains
