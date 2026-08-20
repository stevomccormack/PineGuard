---
name: scan-sonar
description: Run SonarQube static analysis. Use when asked to scan, analyze code quality, or check for code smells. Do NOT use to fix issues; use fix-sonar instead.
---

# Skill: Run SonarQube Analysis

## Step 0: Load Specifications (MANDATORY — read before running)
Read these files completely:
1. `docs/ai/specs/scan/spec.md` (severity model, API, token management)
2. `docs/ai/rules/scan.md` (scan-specific rules)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scan-sonar/SKILL.md` exactly as written.

## Step 2: Verify
- SonarQube container is healthy
- Analysis pipeline completed without errors
- User directed to dashboard URL
