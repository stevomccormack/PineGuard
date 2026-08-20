---
name: document
description: Generate gold-standard XML documentation comments for all public members in a PineGuard project. Use whenever the user says "add XML docs", "document the X class", "generate docs", "add doc comments", "fix CS1591 warnings", or wants layer-aware documentation with cross-references, examples, and doc site links.
argument-hint: "[ProjectName] (e.g., PineGuard.Core, PineGuard.MustClauses, or all)"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: documentation
---
# Skill: Generate XML Documentation

## Step 0: Load Specifications (MANDATORY — read before writing ANY docs)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants, layer ordering)
2. `docs/ai/specs/coding-standard.md` (formatting rules)
3. `docs/ai/skills/document/SKILL.md` (canonical doc generation recipe with per-layer templates)

## Step 1: Identify Scope
- Determine which project(s) to document from the user's input
- If "all", process in layer order: Core → MustClauses → GuardClauses → FluentValidation → DataAnnotations
- List all public source files in the target project (exclude `obj/`, `bin/`, `Common/` internals)

## Step 2: Follow the Recipe
Execute `docs/ai/skills/document/SKILL.md` exactly as written.
- Apply the correct layer-specific template (§5.1-5.6) for each file
- Use Rico Suter phrasing conventions (§8)
- Add `<see cref>` cross-references following the dependency chain
- Add `<see href>` links using the URL patterns (§7)
- Add `<example>` blocks with real PineGuard syntax
Do NOT improvise. Do NOT skip tags required by the template.

## Step 3: Verify
- Code compiles: `dotnet build <project>.csproj`
- Generate XML: `dotnet build <project>.csproj -p:GenerateDocumentationFile=true`
- Check for CS1591 warnings and fix any remaining gaps
- No logic changes — only XML comments added
