# Command: Debugging

> [!NOTE]
> **Interface Definition**: These commands start interactive debugging sessions. They are NOT auto-approved as they require loop intervention.

## 1. Triggers (Slash Commands)

Map these to your Agent's slash command palette:

| Command           | Target        | Auto-Approve | Description                              |
| :---------------- | :------------ | :----------- | :--------------------------------------- |
| `/debug-coverage` | Coverage Gaps | ❌ No        | Interactive loop to close coverage gaps. |
| `/debug-tests`    | Test Failures | ❌ No        | Interactive loop to fix broken tests.    |

## 2. Execution Logic

- **Target**: Coverage -> `docs/ai/agents/fix-coverage-all.md` (or `debug-and-fix-[scope].md`)
- **Target**: Tests -> `docs/ai/agents/fix-test-all.md` (or `debug-and-test-[scope].md`)

## 3. Auto-Approval Rules

- These workflows are **Interactive**. They may run sub-commands (like `dotnet test`) which can be auto-approved, but the _Logic Loop_ requires AI thought.
