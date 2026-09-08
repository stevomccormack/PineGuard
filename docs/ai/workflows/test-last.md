<!-- metadata_header
type: workflow
id: workflow-test-last
version: 1.1
-->

# Workflow: Test Last

> [!NOTE]
> Re-executes unit tests via `tools/testing/Run-Tests.ps1`.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Reference**: `tools/testing/Run-Tests.ps1`

## Parameters

- **Scope**: the primary target selector — one of the fourteen registry scopes, or `All` for the
  whole solution. Exactly one of `-Scope`, `-Project` or `-Solution` must be given; the script has
  no implicit default and exits `2` if none is supplied.
- **Project** / **Solution**: (alternatives to `-Scope`) path to a specific `*.UnitTests.csproj`
  or `.sln`/`.slnx` outside the registry.
- **Filter**: (optional) filter expression for selective runs (e.g. `FullyQualifiedName~Tests`).
- **Framework**: (optional) target framework moniker (e.g. `net10.0`).

## Auto-Approval

Same policy as [Test](test.md) — test runs are auto-approved on every surface.
See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run the latest test command**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope All
   ```

   Swap `All` for the scope you last ran, or use `-Project` / `-Solution` for an off-registry
   target. Add `-Filter` and/or `-Framework` as needed (see Parameters).
