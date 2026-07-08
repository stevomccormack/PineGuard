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
- **RunRole**: Brain role for run commands (e.g., "Code Reviewer", "Test Engineer").
- **FixRole**: Brain role for fix commands (e.g., "Senior Engineer"). Only if HasFixWorkflow=true.
- **RequiresDocker**: Boolean — does the tool need Docker? (e.g., SonarQube=yes, Roslyn=no).

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
> **Brain-first**: All logic lives in `docs/ai/`. Adapters (`.claude/`, `.agent/`, `.github/`) are thin pointers only.

> [!IMPORTANT]
> **DRY**: Specs are the single source of truth for rules. Skills/Workflows are the single source of truth for procedures. Agents compose from Skills/Workflows. Commands map intent to agents.

> [!IMPORTANT]
> **Consistency**: Follow the exact file naming and structure conventions of existing tools (SonarQube, Qodana, Roslyn). Every new tool must be indistinguishable in structure from existing ones.

> [!WARNING]
> **Registration**: Every new tool MUST be registered in `CLAUDE.md` (Command Palette), `docs/ai/README.md` (Rules Hierarchy + Skills Inventory), and all adapter layers.

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
| `docs/ai/skills/{prefix}_run/SKILL.md` | See `docs/ai/skills/scan-roslyn/SKILL.md` or `docs/ai/skills/scan-sonar/SKILL.md` |
| `docs/ai/skills/{prefix}_fix/SKILL.md` | See `docs/ai/skills/fix-roslyn/SKILL.md` or `docs/ai/skills/fix-sonar/SKILL.md` |

Only create the `_fix` skill if `HasFixWorkflow=true`.

### Step 5 — Brain Workflows (1–2 files)

| File | Template |
|------|----------|
| `docs/ai/workflows/scan-{tool}.md` | See `docs/ai/workflows/scan-roslyn.md` or `docs/ai/workflows/scan-sonar.md` |
| `docs/ai/workflows/fix-{ToolDir}.md` | See `docs/ai/workflows/fix-roslyn.md` or `docs/ai/workflows/fix-sonar.md` |

Only create the fix workflow if `HasFixWorkflow=true`.

### Step 6 — Brain Agents (N files)

Create one agent per scope for the run workflow, plus fix agents if applicable:

| File | Pattern |
|------|---------|
| `docs/ai/agents/{prefix}-all.md` | 2-line body: Role + reference to run workflow with Scope=All |
| `docs/ai/agents/{prefix}-core.md` | Same, Scope=Core |
| `docs/ai/agents/{prefix}-{scope}.md` | One per scope in ScopeModel |
| `docs/ai/agents/{prefix}-fix-all.md` | Fix role + reference to fix workflow (if HasFixWorkflow) |

**Agent body pattern:**
```markdown
Act as **{Role}**. Execute workflow `docs/ai/workflows/scan-{tool}.md` with Scope = {Scope}.
```

### Step 7 — Brain Commands (1 file)

| File | Template |
|------|----------|
| `docs/ai/commands/{prefix}.md` | See `docs/ai/commands/scan.md` or `docs/ai/commands/scan.md` |

**Contents:** Table mapping each command to its agent file.

### Step 8 — Adapter Layer — `.claude/` (multiple files)

| File | Template |
|------|----------|
| `.claude/rules/{RuleName}.md` | Path-scoped YAML pointer to Brain rules. See `.claude/rules/roslyn.md` |
| `.claude/skills/{prefix}-run/SKILL.md` | `context: fork` wrapper. See `.claude/skills/scan-roslyn/SKILL.md` |
| `.claude/skills/{prefix}-fix/SKILL.md` | `context: fork` wrapper (if HasFixWorkflow). See `.claude/skills/fix-roslyn/SKILL.md` |
| `.claude/commands/{prefix}-all.md` | `Act as **{Role}**. Read and execute docs/ai/agents/{prefix}-all.md.` |
| `.claude/commands/{prefix}-{scope}.md` | One per scope |
| `.claude/commands/{prefix}-fix-all.md` | Fix command (if HasFixWorkflow) |

### Step 9 — Registration (modify existing files)

| File | What to add |
|------|-------------|
| `CLAUDE.md` | New `### {ToolName} Workflows` section in Command Palette |
| `docs/ai/README.md` | Rule in Rules Hierarchy tree, skills in Skills Inventory table |
| `.claude/settings.json` | Verify tool commands are whitelisted (e.g., `Bash(pwsh:*)`) |

### Step 10 — Verification

1. **Script test**: Run the PowerShell script with `-Scope All`
2. **Scoped test**: Run with a single scope — verify filtering
3. **JSON output**: Verify artifacts file is valid JSON
4. **Slash commands**: Verify all `/prefix-*` commands trigger correctly
5. **Build**: `dotnet build PineGuard.slnx` — solution still builds cleanly
6. **Cross-reference**: Verify `CLAUDE.md`, `docs/ai/README.md`, and all `.claude/commands/` are consistent

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
- [ ] Adapter commands: Slash commands created in `.claude/commands/`
- [ ] Registration: `CLAUDE.md` Command Palette updated
- [ ] Registration: `docs/ai/README.md` Rules Hierarchy + Skills Inventory updated
- [ ] Verification: Script runs, commands trigger, cross-references consistent

## 6. Reference Material (Deep Dive)

**Existing tools to use as templates:**

| Tool | Tools Dir | Spec | Rules | Skills | Workflows | Agents | Commands |
|------|-----------|------|-------|--------|-----------|--------|----------|
| SonarQube | `tools/sonar/` | `specs/scan/spec.md` | `rules/scan.md` | `skills/scan-sonar/`, `skills/fix-sonar/` | `workflows/scan-sonar.md`, `workflows/fix-sonar.md` | `agents/scan-sonar.md`, `agents/fix-sonar-*.md` | `commands/scan.md` |
| Qodana | `tools/qodana/` | `specs/inspection/spec.md` | — | — | `workflows/scan-qodana.md` | `agents/scan-qodana-*.md` | `commands/scan.md` |
| Roslyn | `tools/code-diagnostics/` | `specs/tools/code-diagnostics/spec.md` | `rules/roslyn.md` | `skills/scan-roslyn/`, `skills/fix-roslyn/` | `workflows/scan-roslyn.md`, `workflows/fix-roslyn.md` | `agents/scan-roslyn-*.md`, `agents/fix-roslyn-*.md` | `commands/scan.md` |
| Coverage | `tools/code-coverage/` | `specs/testing/coverage-spec.md` | `rules/testing.md` | `skills/improve-coverage/` | `workflows/coverage.md` | `agents/coverage-*.md` | `commands/coverage.md` |

**Meta-references:**
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
| Adapter Skills (run + fix) | 2 |
| Adapter Commands (7 scopes + fix) | 8 |
| Registration (modify existing) | 2–3 |
| **Total** | **~30 new + 2–3 modified** |
