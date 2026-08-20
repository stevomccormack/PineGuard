# Skill: Scaffold Quality Tool

**ID**: pineguard.skill.scaffold-quality-tool
**Version**: 1.0

## 1. Context & Goal

Adds a new quality/inspection tool to the PineGuard Brain as a first-class citizen with full layering: Tools → Spec → Rules → Skills → Workflows → Agents → Commands → Adapter. This is a meta-skill — it documents the process so future tools can be added systematically.

## 2. Inputs

- **ToolName**: Human-readable name (e.g., "Roslyn Compiler Diagnostics", "SonarQube").
- **ToolDir**: Directory under `tools/` (e.g., `code-diagnostics`, `code-coverage`).
- **CommandPrefix**: Slash command prefix (e.g., `roslyn`, `sonar`, `qodana`).
- **ScopeModel**: List of scopes (e.g., All, Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing).
- **HasFixWorkflow**: Boolean — does this tool have a fix/remediation workflow?
- **RunRole**: Canonical role from `docs/ai/roles/` for scan commands (e.g. `reviewer`, `verifier`).
- **FixRole**: Canonical role from `docs/ai/roles/` for fix commands (e.g. `owner`). Only if HasFixWorkflow=true.
- **RequiresDocker**: Boolean — does the tool need Docker? (e.g., SonarQube=yes, Roslyn=no).

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
> **Brain-first**: All logic lives in `docs/ai/`. Every adapter surface is a thin pointer only. The
> authoritative surface inventory is `docs/ai/meta/adapter-surfaces.md` — read it before Step 8, and do
> not maintain a competing list here.

> [!IMPORTANT]
> **DRY**: Specs are the single source of truth for rules. Skills/Workflows are the single source of truth for procedures. Agents compose from Skills/Workflows. Commands map intent to agents.

> [!IMPORTANT]
> **Naming invariant**: Skill, agent and command names are `<verb>-<tool>[-<scope>]` — `scan-roslyn`,
> `fix-sonar-all`. Never `<tool>_<verb>` or `<tool>-<verb>`. Convention drift here is what silently rots
> `docs/ai/skills/INDEX.md`.

> [!IMPORTANT]
> **Consistency**: Follow the exact file naming and structure conventions of existing tools (SonarQube, Qodana, Roslyn). Every new tool must be indistinguishable in structure from existing ones.

> [!WARNING]
> **Registration**: Every new tool MUST be registered in `CLAUDE.md` (Command Palette), `docs/ai/README.md` (Rules Hierarchy + Skills Inventory), `docs/ai/skills/INDEX.md` (Brain table + one row per adapter table), and all adapter layers.

## 4. Execution Steps

### Step 1 — Tools Layer (2 files)

Create the PowerShell wrapper and operational docs:

| File | Template |
|------|----------|
| `tools/{ToolDir}/Run-{ScriptName}.ps1` | See `tools/code-diagnostics/Run-CompilerDiagnostics.ps1` or `tools/code-coverage/Run-CodeCoverage.ps1` |
| `tools/{ToolDir}/README.md` | See `tools/code-diagnostics/README.md` or `tools/code-coverage/README.md` |

**Script conventions:**
- `-Scope` parameter mapping All → `.slnx`, per-project → `.csproj`
- Structured output to `artifacts/{ToolDir}/{scope}/`
- Exit code: 0 = clean, 1 = issues found
- `-OutputFormat Text|Json` parameter

### Step 2 — Brain Spec (1 file)

| File | Template |
|------|----------|
| `docs/ai/specs/tools/{ToolDir}/spec.md` | See `docs/ai/specs/tools/code-diagnostics/spec.md` or `docs/ai/specs/scan/spec.md` |

**Must include:** Overview, how it differs from other tools, issue categories, scope model, output paths, tool scripts, fix rules (if applicable).

### Step 3 — Brain Rules (1 file)

| File | Template |
|------|----------|
| `docs/ai/rules/{RuleName}.md` | See `docs/ai/rules/roslyn.md` or `docs/ai/rules/scan.md` |

**Pattern:** Inherits from `global.md`. Compressed invariants. References the spec and tool README.

### Step 4 — Brain Skills (1–2 files)

| File | Template |
|------|----------|
| `docs/ai/skills/scan-{tool}/SKILL.md` | See `docs/ai/skills/scan-roslyn/SKILL.md` or `docs/ai/skills/scan-sonar/SKILL.md` |
| `docs/ai/skills/fix-{tool}/SKILL.md` | See `docs/ai/skills/fix-roslyn/SKILL.md` or `docs/ai/skills/fix-sonar/SKILL.md` |

Only create the `fix-{tool}` skill if `HasFixWorkflow=true`.

### Step 5 — Brain Workflows (1–2 files)

| File | Template |
|------|----------|
| `docs/ai/workflows/scan-{tool}.md` | See `docs/ai/workflows/scan-roslyn.md` or `docs/ai/workflows/scan-sonar.md` |
| `docs/ai/workflows/fix-{tool}.md` | See `docs/ai/workflows/fix-roslyn.md` or `docs/ai/workflows/fix-sonar.md` |

Only create the fix workflow if `HasFixWorkflow=true`.

### Step 6 — Brain Agents (N files)

Create one agent per scope for the run workflow, plus fix agents if applicable:

| File | Pattern |
|------|---------|
| `docs/ai/agents/scan-{tool}-all.md` | 2-line body: Role + reference to run workflow with Scope=All |
| `docs/ai/agents/scan-{tool}-core.md` | Same, Scope=Core |
| `docs/ai/agents/scan-{tool}-{scope}.md` | One per scope in ScopeModel |
| `docs/ai/agents/fix-{tool}-all.md` | Fix role + reference to fix workflow (if HasFixWorkflow) |

**Agent body pattern:**
```markdown
Act as **{Role}**. Execute workflow `docs/ai/workflows/scan-{tool}.md` with Scope = {Scope}.
```

### Step 7 — Brain Commands (1 file)

| File | Template |
|------|----------|
| `docs/ai/commands/{family}.md` | See `docs/ai/commands/scan.md` or `docs/ai/commands/fix.md` |

**Contents:** Table mapping each command to its agent file.

### Step 8 — Adapter Layer (multiple files, multiple surfaces)

Work `docs/ai/meta/adapter-surfaces.md` §5 cascade checklist row by row. Each row below is either
**created** or **N/A under a declared policy** (§4 of that file) — never silently skipped. Three
surfaces host skills (`.claude/`, `.github/`, `.pi/`); omitting two of them is what leaves adapters
permanently behind the Brain.

| File | Template |
|------|----------|
| `.claude/rules/{RuleName}.md` | Path-scoped YAML pointer to Brain rules. See `.claude/rules/roslyn.md` |
| `.claude/skills/scan-{tool}/SKILL.md` | `context: fork` wrapper. See `.claude/skills/scan-roslyn/SKILL.md` |
| `.claude/skills/fix-{tool}/SKILL.md` | `context: fork` wrapper (if HasFixWorkflow). See `.claude/skills/fix-roslyn/SKILL.md` |
| `.claude/commands/scan-{tool}-all.md` | `Act as **{Role}**. Read and execute docs/ai/agents/scan-{tool}-all.md.` |
| `.claude/commands/scan-{tool}-{scope}.md` | One per scope |
| `.claude/commands/fix-{tool}-all.md` | Fix command (if HasFixWorkflow) |
| `.github/skills/scan-{tool}/SKILL.md` | Copilot wrapper. See `.github/skills/scan-roslyn/SKILL.md` |
| `.github/skills/fix-{tool}/SKILL.md` | Copilot wrapper (if HasFixWorkflow). See `.github/skills/fix-roslyn/SKILL.md` |
| `.github/prompts/*.prompt.md` | Only if the command is in the declared Copilot subset (adapter-surfaces §4) |
| `.pi/skills/scan-{tool}/SKILL.md` | Pi wrapper. See `.pi/skills/scan-roslyn/SKILL.md` |
| `.pi/skills/fix-{tool}/SKILL.md` | Pi wrapper (if HasFixWorkflow). See `.pi/skills/fix-roslyn/SKILL.md` |
| `.pi/prompts/scan-{tool}-{scope}.md`, `.pi/prompts/fix-{tool}-all.md` | One per command |
| `.agent/workflows/scan-{tool}-{scope}.md`, `.agent/workflows/fix-{tool}-all.md` | One per command |

Rules-only adapters (`docs/ai/meta/adapter-surfaces.md` §3) are touched only if the new tool changes a
**layer mapping** — a new quality tool normally does not.

### Step 9 — Registration (modify existing files)

| File | What to add |
|------|-------------|
| `CLAUDE.md` | New `### {ToolName}` section in the Command Palette |
| `AGENTS.md` | Same palette rows, tool-neutral phrasing |
| `.pi/AGENTS.md` | Same palette rows for the Pi surface |
| `docs/ai/README.md` | Rule in Rules Hierarchy tree, skills in Skills Inventory table |
| `docs/ai/skills/INDEX.md` | Brain rows, plus one row per adapter table the tool lands in |
| `.claude/settings.json` | Verify tool commands are whitelisted (e.g., `Bash(pwsh:*)`) |

### Step 10 — Verification

1. **Script test**: Run the PowerShell script with `-Scope All`
2. **Scoped test**: Run with a single scope — verify filtering
3. **JSON output**: Verify artifacts file is valid JSON
4. **Slash commands**: Verify all `/scan-{tool}-*` and `/fix-{tool}-*` commands trigger correctly
5. **Build**: `dotnet build PineGuard.slnx` — solution still builds cleanly
6. **Cross-reference**: Verify `CLAUDE.md`, `AGENTS.md`, `.pi/AGENTS.md`, `docs/ai/README.md`, `docs/ai/skills/INDEX.md` and every adapter command directory are consistent
7. **Parity**: `pwsh ./tools/audit-cli/Run-All.ps1` — the adapter-parity rule is clean

## 5. Definition of Done

- [ ] Tools layer: Script + README created and tested
- [ ] Brain spec: Normative spec created
- [ ] Brain rules: Compressed invariants created, inheriting from `global.md`
- [ ] Brain skills: Run skill created (+ fix skill if applicable)
- [ ] Brain workflows: Run workflow created (+ fix workflow if applicable)
- [ ] Brain agents: One agent per scope created (+ fix agents if applicable)
- [ ] Brain commands: Command contract table created
- [ ] Adapter rules: Path-scoped pointer created in `.claude/rules/`
- [ ] Adapter skills: `context: fork` wrappers created in `.claude/skills/`
- [ ] Adapter skills: wrappers created in `.github/skills/` and `.pi/skills/`
- [ ] Adapter commands: created in `.claude/commands/`, `.pi/prompts/` and `.agent/workflows/`
- [ ] Adapter commands: `.github/prompts/` — created, or N/A under the declared Copilot subset
- [ ] Registration: `CLAUDE.md`, `AGENTS.md` and `.pi/AGENTS.md` palettes updated
- [ ] Registration: `docs/ai/README.md` Rules Hierarchy + Skills Inventory updated
- [ ] Registration: `docs/ai/skills/INDEX.md` updated in the Brain table and every adapter table
- [ ] Verification: Script runs, commands trigger, cross-references consistent

## 6. Reference Material (Deep Dive)

**Existing tools to use as templates:**

| Tool | Tools Dir | Spec | Rules | Skills | Workflows | Agents | Commands |
|------|-----------|------|-------|--------|-----------|--------|----------|
| SonarQube | `tools/sonar-scanner/` | `specs/scan/spec.md` | `rules/scan.md` | `skills/scan-sonar/`, `skills/fix-sonar/` | `workflows/scan-sonar.md`, `workflows/fix-sonar.md` | `agents/scan-sonar.md`, `agents/fix-sonar-*.md` | `commands/scan.md` |
| Qodana | `tools/code-inspection/` | `specs/tools/code-inspection/qodana.md` | — | — | `workflows/scan-qodana.md` | `agents/scan-qodana-*.md` | `commands/scan.md` |
| Roslyn | `tools/code-diagnostics/` | `specs/tools/code-diagnostics/spec.md` | `rules/roslyn.md` | `skills/scan-roslyn/`, `skills/fix-roslyn/` | `workflows/scan-roslyn.md`, `workflows/fix-roslyn.md` | `agents/scan-roslyn-*.md`, `agents/fix-roslyn-*.md` | `commands/scan.md` |
| Coverage | `tools/code-coverage/` | `specs/testing/coverage.md` | `rules/testing.md` | `skills/improve-coverage/` | `workflows/coverage.md` | `agents/coverage-*.md` | `commands/coverage.md` |

**Meta-references:**
- `docs/ai/meta/adapter-surfaces.md` — Surface inventory, parity policy, cascade checklist
- `docs/ai/meta/taxonomy.md` — How concepts compose
- `docs/ai/skills/scaffold-workflow/SKILL.md` — Workflow creation recipe

## 7. File Count Summary

For a tool with 7 scopes and a fix workflow:

| Layer | Files |
|-------|-------|
| Tools (script + README) | 2 |
| Brain Spec | 1 |
| Brain Rules | 1 |
| Brain Skills (run + fix) | 2 |
| Brain Workflows (run + fix) | 2 |
| Brain Agents (7 scopes + fix) | 8 |
| Brain Commands | 1 |
| Adapter Rules | 1 |
| Adapter Skills (scan + fix × `.claude`, `.github`, `.pi`) | 6 |
| Adapter Commands (7 scopes + fix × `.claude`, `.pi`, `.agent`) | 24 |
| Registration (modify existing) | 5–6 |
| **Total** | **~52 new + 5–6 modified** |
