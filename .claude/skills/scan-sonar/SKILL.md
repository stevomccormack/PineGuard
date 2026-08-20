---
name: scan-sonar
description: Run SonarQube static analysis and report findings. Use whenever the user says "run Sonar", "run the scan", "check code quality", "check for code smells", "sonar analysis", or wants a SonarQube quality report. Do NOT use to fix issues; use fix-sonar instead.
argument-hint: "[Scope]"
context: fork
allowed-tools: Read, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.1.0
  category: analysis
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
