<!-- metadata_header
type: workflow
id: workflow-build-all
version: 1.0
-->

# Workflow: Build All

> [!NOTE]
> Full, non-incremental rebuild of every PineGuard library and test project. Use it after a
> cross-layer change, a dependency bump, or whenever incremental output is suspect.

## Context

- **Role**: [DevOps Engineer](../roles/shipper.md)
- **Reference**: `PineGuard.slnx`

## Parameters

- **Configuration**: (`Debug`, `Release`) — defaults to `Debug`.

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow builds.
- **Cursor**: `cmd: dotnet build` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Rebuild the solution**

   ```powershell
   dotnet build PineGuard.slnx --no-incremental -c [CONFIGURATION]
   ```

   The solution builds every `src/PineGuard.*` library and every `tests/PineGuard.*.UnitTests`
   project, so layer ripple effects surface here rather than in a later scoped run.

2. **Triage failures by layer**

   Fix in dependency order — Core, then MustClauses, then GuardClauses / FluentValidation /
   DataAnnotations, then Testing. A Core break cascades, so never chase downstream errors first.

3. **Verify**

   Once the build is clean, run `/test-all`, then `/format-all` if any source file was touched.
