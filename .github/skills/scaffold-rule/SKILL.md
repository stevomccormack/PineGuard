---
name: scaffold-rule
description: Implement a new Core Rule or Util in PineGuard.Core.
---
# Skill: Implement Core Rule/Util

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [project.md (Core)](../../../docs/ai/specs/core/project.md)
3. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
4. [scaffold-rule SKILL.md](../../../docs/ai/skills/scaffold-rule/SKILL.md)

## Execute

Follow the canonical recipe in [scaffold-rule SKILL.md](../../../docs/ai/skills/scaffold-rule/SKILL.md) exactly.

## Verify

- No user-facing messages in Core (pure logic only)
- No IO (File/Network) in Core Rules
- Null inputs handled explicitly (usually return `false`)
- The Brain remains the source of truth.
