---
name: format-code
description: Run dotnet format to enforce .editorconfig rules. Use before committing or after bulk edits.
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
