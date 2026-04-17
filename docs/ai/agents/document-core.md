<!-- metadata_header
type: agent
id: agent-document-core
version: 1.0
-->

# Agent: Generate XML Docs for PineGuard.Core

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: builder ([../roles/builder.md](../roles/builder.md))

## Steps

1. Read the skill at `docs/ai/skills/document/SKILL.md`.
2. Execute it with parameter **Project = PineGuard.Core** (Rules + Utils).
3. Apply §5.1 (Rules), §5.2 (Standard Rules), and §5.3 (Utils) templates.
4. Verify: `dotnet build src/PineGuard.Core/PineGuard.Core.csproj`.
