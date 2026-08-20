<!-- metadata_header
type: agent
id: agent-fix-roslyn-all
version: 1.0
-->

# Agent: Fix All Roslyn Compiler Warnings

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: owner ([../roles/owner.md](../roles/owner.md))

> [!NOTE]
> This is the only agent in the family by design. `docs/ai/workflows/fix-roslyn.md` also accepts a
> narrower **Scope** and an optional **Filter** (e.g. `CS86` for nullability); those are requested
> conversationally rather than through dedicated per-scope commands.

## Steps

1. Read the master workflow at `docs/ai/workflows/fix-roslyn.md`.
2. Execute it with parameter **Scope = All** (no filter — fix all warnings).
