<!-- metadata_header
type: workflow
id: workflow-test-last
version: 1.1
-->

# Workflow: Test Last

> [!NOTE]
> Re-executes the most recent test command via the agent-generated runner under
> `tools/code-inspection/auto/`, which forwards to `tools/testing/Run-Tests.ps1`.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Reference**: `tools/code-inspection/auto/Run-Last.ps1`

## Parameters

- **Project**: (optional) path to a specific `*.UnitTests.csproj` — defaults to all unit tests.
- **Filter**: (optional) filter expression for selective runs (e.g. `FullyQualifiedName~Tests`).

## Auto-Approval

Same policy as [Test](test.md) — test runs are auto-approved on every surface.
See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run the latest test command**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-inspection/auto/Run-Last.ps1"
   ```

   Add `-Project` and/or `-Filter` as needed (see Parameters).
