---
name: scaffold-quality-tool
description: Add a new quality or inspection tool (scanner, analyser, linter) to the Brain with full layering — Tools, Spec, Rules, Skills, Workflows, Agents, Commands, Adapters. Use whenever the user says "add a new scan tool", "wire up SonarQube/Qodana/Roslyn", "add a quality gate", or wants a new inspection tool made a first-class citizen.
argument-hint: "[ToolName]"
context: fork
allowed-tools: Read, Write, Edit, Glob, Grep, Bash
metadata:
  author: stevomccormack
  version: 1.0.0
  category: meta
---
# Skill: Scaffold Quality Tool

## Step 0: Load Specifications (MANDATORY)
Read these files:
1. `docs/ai/specs/protocol.md` (Brain/Adapter contract — Rule #1: adapters carry no logic)
2. `docs/ai/specs/tools/spec.md` (tool script conventions)
3. `docs/ai/meta/adapter-surfaces.md` (the single inventory of surfaces and the cascade checklist)
4. `docs/ai/skills/scaffold-quality-tool/SKILL.md` (canonical recipe)

## Step 1: Follow the Recipe
Execute `docs/ai/skills/scaffold-quality-tool/SKILL.md` exactly as written.
Specs own the rules, skills and workflows own the procedures, agents compose them, commands map
intent to agents — do not collapse those layers.

## Step 2: Verify
- The tool script lives under `tools/<ToolDir>/` and follows the tools spec
- Scan and fix agents declare canonical roles from `docs/ai/roles/`
- Every adapter surface in the cascade checklist is done or N/A under a declared policy exception