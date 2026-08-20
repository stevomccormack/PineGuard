# Workflow: Run Last Test Command

> [!NOTE]
> Re-executes the most recent test command via the agent-generated runner under
> `tools/code-inspection/auto/`, which forwards to `tools/testing/Run-Tests.ps1`.

// turbo-all

1. Run Latest Test Command

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-inspection/auto/Run-Last.ps1"
   ```

   Optional parameters:
   - `-Project` — path to a specific `*.UnitTests.csproj` (defaults to all unit tests).
   - `-Filter` — filter expression for selective runs (e.g. `FullyQualifiedName~Tests`).
