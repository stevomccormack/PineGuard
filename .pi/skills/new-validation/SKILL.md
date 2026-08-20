---
name: new-validation
description: Add a complete new validation across ALL layers (Core -> Must -> Guard -> Fluent -> Data -> Tests). Use for new Must.Be.X / Guard.Against.X validation. Do NOT use for single-layer changes.
---

# New Validation: Vertical Slice Implementation

## Step 0: Load Root Specifications (MANDATORY — read before writing ANY code)
Read these files completely before writing ANY code:
1. `docs/ai/specs/spec.md` (root invariants — especially "Feature Implementation Checklist")
2. `docs/ai/specs/dependencies.md` (layer dependency map)
3. `docs/ai/specs/coding-standard.md` (formatting rules)
4. `docs/ai/specs/orchestration.md` (process/logging)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/new-validation/SKILL.md` exactly as written. It owns the layer order and
the per-layer spec/skill mapping; do not restate them here.

## Step 2: Build & Test
- Build entire solution: `dotnet build`
- Run all tests: `dotnet test`
- Verify 100% coverage for new code

## Step 3: Summary
Return a summary of all files created/modified, organized by layer.
