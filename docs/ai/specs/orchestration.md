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

- `docs/todo.md` (if present)

3. Load the local spec for the area you’re editing:

- `docs/ai/specs/<area>/project.md`
- `docs/ai/specs/<area>/unit-test.md` (if adding/changing tests)
- `docs/ai/specs/<area>/coverage.md` (if doing coverage work)

Notes:

- When in doubt, prefer reading the repo files over guessing.
- If you cannot find prior context in the repo or current chat, say so explicitly and log uncertainty instead of inventing details.

## Recovery work

If the task is recovery/reconstruction-related:

- Also update `docs/todo.md` with new discoveries, newly identified missing items, or changes in assumptions.
- Prefer writing down _what we know_ vs. guessing.

## Safety rules (destructive operations)

All safety rules for destructive operations (git, filesystem, build tools, process management, environment mutation) are defined in the dedicated safety specification:

**Read: [`docs/ai/specs/safety.md`](safety.md)**

This is non-negotiable. Every agent must read and follow the safety spec before executing any command that could destroy, overwrite, or irrecoverably alter user work.

## Agent Workflows

You have the ability to use and create workflows.

There are two layers:

- **Canonical agent playbooks**: `docs/ai/agents/*.md` (model-agnostic)
- **Adapter workflows**: `.agent/workflows/*.md` (Gemini adapter; must be thin pointers)

### Adapter workflows (`.agent/workflows/*.md`)

- **Must be thin**: no embedded logic or command scripts.
- **Format**: YAML frontmatter (`description`) + minimal markdown steps.
- **Required behavior**: point to a canonical playbook in `docs/ai/agents` (and optionally include `// turbo` / `// turbo-all`).

### Turbo Mode (Auto-Approval)

To skip user confirmation for command execution:

- **Single Step**: Add `// turbo` above the specific step.
- **Entire Workflow**: Add `// turbo-all` anywhere in the file.

Use these annotations ONLY when you are confident the commands are safe or when the user explicitly requests full automation.

### Zero-Friction Command Execution (Auto-Approval Hack)

When you need to run specific, ad-hoc commands (like `dotnet test` with specific filters) without waiting for user approval:

1. **Write** the command steps to a canonical agent playbook: `docs/ai/agents/run-custom.md`.
2. **Create/update** the adapter workflow stub: `.agent/workflows/custom.md`:

```markdown
---
description: Run Custom Command
---

// turbo-all

1. Read and execute `docs/ai/agents/run-custom.md`.
```

3. **Execute** the adapter workflow: `view_file .agent/workflows/custom.md`

This is the **preferred** way to run `dotnet build`, `dotnet test`, and safe `git` operations to maintain flow state.

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
