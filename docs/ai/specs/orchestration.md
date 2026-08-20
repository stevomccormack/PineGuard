---
spec:
  id: pineguard.ai.orchestration
  title: "AI Working Instructions"
  version: 1
  parent:
    - spec.md
  dependencies:
    - dependencies.md
applies_to:
  - "**/*"
---

# AI Working Instructions (PineGuard)

This file governs **how agents work** — process, orchestration, discipline, and verification. It does not override the generator specs in `docs/ai/**` (which govern **what** to build).

Primary goals:
- Prevent loss of work and intent between sessions.
- Ensure agents plan before acting, verify before claiming success, and learn from mistakes.

## Context refresh (required)

LLM chat context is lossy and may be summarized or truncated. To avoid losing intent during development, periodically refresh from the repo.

Refresh triggers (do this whenever any apply):

- You see a system-generated “conversation summary”.
- You are starting a new session.
- You are about to make non-trivial changes (multi-file edits, refactors, spec changes, broad sweeps).
- You are switching areas (e.g., Core Rules/Utils → MustClauses → GuardClauses → adapters → tests).

Refresh checklist (non-negotiable to avoid summarising and losing context regulary):

1. Read the process rules:

- `docs/ai/specs/orchestration.md` (this file)
- `docs/ai/specs/spec.md`

2. Rehydrate current state:

- `docs/ai/plans/` — the active roadmap for the work in flight
- `.claude/agent-memory/` — patterns and corrections learned in earlier sessions

3. Load the local spec for the area you’re editing:

- `docs/ai/specs/<area>/project.md`
- `docs/ai/specs/<area>/unit-test.md` (if adding/changing tests)
- `docs/ai/specs/<area>/coverage.md` (if doing coverage work)

Notes:

- When in doubt, prefer reading the repo files over guessing.
- If you cannot find prior context in the repo or current chat, say so explicitly and log uncertainty instead of inventing details.

## Recovery work

If the task is recovery/reconstruction-related:

- Also record new discoveries, newly identified missing items, or changes in assumptions in the relevant plan under `docs/ai/plans/`.
- Prefer writing down _what we know_ vs. guessing.

## Safety rules (destructive operations)

All safety rules for destructive operations (git, filesystem, build tools, process management, environment mutation) are defined in the dedicated safety specification:

**Read: [`docs/ai/specs/safety.md`](safety.md)**

This is non-negotiable. Every agent must read and follow the safety spec before executing any command that could destroy, overwrite, or irrecoverably alter user work.

## Agent Workflows

You have the ability to use and create workflows.

There are two layers:

- **Canonical agent playbooks**: `docs/ai/agents/*.md` (model-agnostic)
- **Adapter surfaces**: the per-tool entry points that point at those playbooks

### Adapter surfaces

Every adapter surface in the repo is inventoried in [`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md), which also records which tier each one belongs to. The thin-pointer rule is normative for all of them — see `docs/ai/specs/protocol.md` Rule #1.

The `// turbo` / `// turbo-all` annotations below are specific to `.agent/workflows/*.md`; the other surfaces have no equivalent.

- **Must be thin**: no embedded logic or command scripts.
- **Format** (`.agent/workflows/*.md`): YAML frontmatter (`description`) + minimal markdown steps.
- **Required behavior**: point to a canonical playbook in `docs/ai/agents` (and, on `.agent/workflows`, optionally include `// turbo` / `// turbo-all`).

### Turbo Mode (Auto-Approval)

To skip user confirmation for command execution:

- **Single Step**: Add `// turbo` above the specific step.
- **Entire Workflow**: Add `// turbo-all` anywhere in the file.

Use these annotations ONLY when you are confident the commands are safe or when the user explicitly requests full automation.

**Only Tier 2 commands per [`docs/ai/specs/safety.md`](safety.md) §8.4 may be placed in a `// turbo-all` workflow; never Tier 0 or Tier 1.** Writing a command into a script or playbook does not change its tier — see safety.md §8.3 ("No Laundry Pattern").

### Reusable command workflows

To re-run a specific, ad-hoc command (like `dotnet test` with a filter) without rebuilding the invocation each time, use the workflow that already exists rather than authoring a new playbook:

- Workflow: [`docs/ai/workflows/custom.md`](../workflows/custom.md)
- Script: `tools/code-inspection/auto/Run-Last.ps1`

Parameterise the run through the script's `-Project` / `-Filter` parameters. Do not inline ad-hoc command scripts into an adapter workflow — that is logic in an adapter, which `docs/ai/specs/protocol.md` Rule #1 forbids.

## Honesty rule

If prior information is not present in the repo or the current chat context, say so explicitly and record the uncertainty in the log rather than inventing details.

---

## Workflow Orchestration

These rules govern how agents approach non-trivial work. They apply to all agents, all adapters, and all sessions.

### Plan before acting

- Enter plan mode for any non-trivial task (3+ steps or architectural decisions).
- Plans must state **what** will change and **why**, identify affected files, and surface ambiguities before implementation begins.
- Write detailed specs upfront to reduce ambiguity — the user should approve the approach before code is written.
- If something goes sideways during implementation, **STOP and re-plan**. Do not patch around obstacles or drift from the approved plan without surfacing the change.
- Skip plan mode for: single-line fixes, typos, one-off command runs, pure research.

### Use subagents effectively

- Use subagents liberally to keep the main context window clean.
- Offload research, exploration, and parallel analysis to specialized agents.
- One task per subagent for focused execution — avoid vague mandates like "make the code better."
- For complex problems, throw more compute at it via parallel subagents.

### Track progress visibly

- All multi-step work must have a task list with checkpoints.
- Mark each task in_progress before starting; mark complete immediately upon finishing. Do not batch completions.
- If blocked, create a new task describing the blocker — do not leave tasks hanging with no explanation.
- Provide high-level status updates at natural milestones.

### Verify before claiming success

- **Never** mark a task complete without proving it works.
- Run tests, check logs, demonstrate correctness. Do not assume.
- Diff behavior between main and your changes when relevant.
- Ask yourself: "Would a staff engineer approve this?"
- Verification steps are part of the plan, not an afterthought — include them in task lists upfront.
- Document results: "✅ All 42 tests pass. Coverage: 100%. No Roslyn warnings."

### Learn from corrections

- After any user correction, write a rule that prevents repeating the mistake.
- Rules must state both the constraint and the rationale (so agents can judge edge cases).
- Store lessons in specs, rules, or agent memory as appropriate.
- Review learned patterns at session start when relevant context exists.

### Fix bugs autonomously

- When given a bug report, investigate and fix it. Do not ask for hand-holding.
- Trace root cause via logs, failing tests, and error messages.
- Apply the minimal fix and verify with tests before reporting back.
- If you cannot fix it, surface blockers clearly — point at specific logs or errors so the human's next step is obvious.
