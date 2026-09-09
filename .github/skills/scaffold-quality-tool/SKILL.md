---
name: scaffold-quality-tool
description: Add a new quality or inspection tool (scanner, analyser, linter) to the Brain with full layering — Tools, Spec, Rules, Skills, Workflows, Agents, Commands, Adapters.
---
# Skill: Scaffold Quality Tool

## Load First

Read these files before scaffolding a new tool:
1. [protocol.md](../../../docs/ai/specs/protocol.md)
2. [tools spec](../../../docs/ai/specs/tools/spec.md)
3. [adapter-surfaces.md](../../../docs/ai/meta/adapter-surfaces.md)
4. [scaffold-quality-tool SKILL.md](../../../docs/ai/skills/scaffold-quality-tool/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-quality-tool SKILL.md](../../../docs/ai/skills/scaffold-quality-tool/SKILL.md) exactly.

## Verify

- The tool script lives under `tools/<ToolDir>/` and follows the tools spec
- Scan and fix agents declare canonical roles from `docs/ai/roles/`
- Every adapter surface in the cascade checklist is done or N/A under a declared policy exception
- The Brain remains the source of truth.
