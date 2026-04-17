# Copilot Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It maps natural language intent to the canonical Brain in `docs/ai/`.
> Do not add logic here. Add logic to the Brain.
> 👉 Start at **[docs/ai/README.md](../docs/ai/README.md)** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
👉 **[docs/ai/business-units/engineering.md](../docs/ai/business-units/engineering.md)**

## 2. Intent Mapping

If the user asks to run coverage, check tests, or fix bugs, refer to the canonical agents:

| User Intent | Agent to Execute | Role |
|---|---|---|
| "Check coverage for [scope]" | `../docs/ai/agents/coverage-[scope].md` | Test Engineer |
| "Run all tests" | `../docs/ai/agents/test-all.md` | Test Engineer |
| "Run Qodana for [scope]" | `../docs/ai/agents/scan-qodana-[scope].md` | Code Reviewer |
| "Fix coverage gaps" | `../docs/ai/agents/fix-coverage-all.md` | Senior Engineer / Test Engineer |
| "Why is this test failing?" | `../docs/ai/agents/fix-test-all.md` | Senior Engineer / Test Engineer |
| "Format the code" | `../docs/ai/agents/format-all.md` | Software Engineer |
| "Clean workspace" | `../docs/ai/agents/clean-all.md` | DevOps Engineer |
| "Generate references" | `../docs/ai/agents/generate-all.md` | Software Engineer |
| "Run library audit" | `../docs/ai/agents/audit-cli.md` | Software Engineer |
| "Run SonarQube" | `../docs/ai/agents/scan-sonar.md` | Code Reviewer |
| "Fix SonarQube issues" | `../docs/ai/agents/fix-sonar-all.md` | Senior Engineer |
| "Check Roslyn warnings" | `../docs/ai/agents/scan-roslyn-all.md` | Code Reviewer |
| "Fix Roslyn warnings" | `../docs/ai/agents/fix-roslyn-all.md` | Senior Engineer |
| "Commit changes" | `../docs/ai/agents/commit-all.md` | Software Engineer |
| "Implement new validation" | `../docs/ai/agents/scaffold-vertical-slice.md` | Software Engineer |

*All scoped commands are available for: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing.*

## 3. Safety

Before executing commands, read the safety spec:
👉 **[docs/ai/specs/safety.md](../docs/ai/specs/safety.md)**

## 4. Knowledge Base

- **Brain index**: `../docs/ai/README.md`
- **Specs**: `../docs/ai/specs/` (normative engineering rules, coding standards, testing specs)
- **Safety**: `../docs/ai/specs/safety.md` (Tier 0/1/2 command classification)
- **Rules**: `../docs/ai/rules/` (scope-specific, inheriting from `global.md`)
- **Skills**: `../docs/ai/skills/` (reusable implementation recipes)
- **Agents**: `../docs/ai/agents/` (canonical playbooks)
- **Workflows**: `../docs/ai/workflows/` (multi-step orchestration)
- **Commands**: `../docs/ai/commands/` (intent-to-agent mappings)
- **Roles**: `../docs/ai/roles/` (personas and responsibilities)
- **Architecture**: `../docs/ai/specs/` (structural design)
- **Meta**: `../docs/ai/meta/` (taxonomy, tooling alignment)
- **Memory**: `../docs/ai/memory/` (portable, checked-in learned patterns shared across tools)
- **Plans**: `../docs/ai/plans/` (implementation roadmaps)

## 5. Copilot-Native Customizations

When available, prefer the thin adapters in this repository instead of duplicating Brain logic in prompts:

- **Path instructions**: `./instructions/*.instructions.md`
- **Custom agents**: `./agents/*.agent.md`
- **Prompt files**: `./prompts/*.prompt.md`
- **Agent skills**: `./skills/*/SKILL.md`

## 6. Comments Policy

- Production source code (src/**) must have XML doc comments (///) on public API.
- Test projects (tests/**) should NOT have XML doc comments (///).
- Tests can have comments but should be minimal and focused on clarifying complex logic.
- Tests with existing comments already, are fine to keep them, but new comments should follow this guideline.

## 7. Tooling Workarounds

### `run_build` hangs with `.slnx`

This workspace uses the `.slnx` solution format (VS 2022 17.10+ / .NET 9+). The `run_build` tool does not reliably support `.slnx` — it may hang or timeout.

**Do NOT use `run_build`.** Use the terminal instead:

```powershell
dotnet build PineGuard.slnx --verbosity quiet
```