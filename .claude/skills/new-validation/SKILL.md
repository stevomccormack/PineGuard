---
name: new-validation
description: Add a simple new validation rule across ALL layers (Core -> Must -> Guard -> Fluent -> DataAnnotations -> Tests). Use whenever the user says "add a new validation", "implement Must.Be.X", "add Guard.Against.X", or wants a validation rule based on an in-memory predicate (string format, numeric range, enum check, etc.).
argument-hint: "[DomainName] [ConditionName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: implementation
---
# New Validation: Vertical Slice Implementation

## Step 0: Load Root Specifications (MANDATORY — read before writing ANY code)
Read these files completely:
1. `docs/ai/specs/spec.md` (root invariants — especially §3 "Feature Implementation Checklist")
2. `docs/ai/specs/dependencies.md` (layer dependency map)
3. `docs/ai/specs/coding-standard.md` (formatting rules)
4. `docs/ai/specs/orchestration.md` (process/logging)
5. `docs/ai/skills/new-validation/SKILL.md` (canonical orchestration recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/new-validation/SKILL.md` exactly as written.
Its §4 table is the single source for the per-layer spec and scaffold-skill routing — do not
restate it here, and do not improvise a layer order.

## Step 2: Verify
- Solution builds clean and all tests pass
- 100% line AND branch coverage for the new code
- No validation logic outside `PineGuard.Core`
- Summary lists every file created or modified, grouped by layer
