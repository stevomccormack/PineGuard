<!-- metadata_header
type: plan
id: naming-convention-rename
version: 1.0
status: completed
-->

# Naming Convention Rename Plan

> [!WARNING]
> **Archived — do not reuse the Phase 8 cascade table below.** It omits `.cursorrules`,
> `.windsurfrules` and `.junie/guidelines.md`, and those three are exactly the surfaces that were
> left holding pre-rename names. The living, complete inventory is
> `docs/ai/meta/adapter-surfaces.md`; use its §5 cascade checklist for any future rename.

## Convention Rules (from taxonomy.md §N.1–N.8)

- **Pattern**: `{verb}-{target}.md` for action files, `{noun}.md` for identity files
- **Verb-first**: Always. `scan-roslyn` not `roslyn-scan`.
- **Singular**: Always. `must` not `must-clauses`. `artifact` not `artifacts`.
- **Retired verbs**: `run-` → drop, `debug-and-fix-` → `fix-`, `debug-and-test-` → `fix-`, `implement-` → `scaffold-`, `improve-` → domain verb, `create-` → `scaffold-`, `rebuild-` → `build-`, `xml-docs-` → `document-`, `git-commit-` → `commit-`, `analyze-` → `audit-`

---

## Phase 1: Rules (6 renames × 4 adapter tiers)

| Old | New | Reason |
|-----|-----|--------|
| `must-clauses.md` | `must.md` | §N.4 scope identifier |
| `guard-clauses.md` | `guard.md` | §N.4 scope identifier |
| `fluent-validation.md` | `fluent.md` | §N.4 scope identifier |
| `data-annotations.md` | `annotation.md` | §N.4 scope identifier |
| `unit-tests.md` | `testing.md` | §N.4 scope for PineGuard.Testing |
| `code-diagnostics.md` | `roslyn.md` | §N.4 tool identifier |

**Locations**: `docs/ai/rules/`, `.claude/rules/`, `.cursor/rules/` (.mdc), `.github/instructions/`

---

## Phase 2: Skills (14 renames, 1 delete)

### docs/ai/skills/ (canonical)

| Old dir | New dir | Reason |
|---------|---------|--------|
| `implement-core-rule` | `scaffold-rule` | §N.3 implement → scaffold |
| `scaffold-musts` | `scaffold-must` | §N.3 + §N.5 singular |
| `scaffold-guards` | `scaffold-guard` | §N.3 + §N.5 singular |
| `implement-fluent-validation` | `scaffold-fluent` | §N.3 + §N.4 scope |
| `implement-data-annotations` | `scaffold-annotation` | §N.3 + §N.4 scope |
| `implement-unit-tests` | `scaffold-unit-test` | §N.3 + §N.5 singular |
| `improve-code-coverage` | `improve-coverage` | Drop redundant "code" |
| `roslyn-run` | `scan-roslyn` | §N.7 verb-first |
| `roslyn-fix` | `fix-roslyn` | §N.7 verb-first |
| `scan-run` | `scan-sonar` | §N.7 explicit tool |
| `scan-fix` | `fix-sonar` | §N.7 explicit tool |
| `create-workflow` | `scaffold-workflow` | §N.3 create → scaffold |
| `generate-xml-docs` | `document` | §N.3 xml-docs → document |

**Delete**: `implement-custom-validation` (empty), `skills-format` (meta-template, not a skill)

**Mirror renames in**: `.claude/skills/`, `.github/skills/`, `.pi/skills/`

---

## Phase 3: Workflows (15 renames, 2 move to plans, 1 delete)

| Old | New | Reason |
|-----|-----|--------|
| `run-tests.md` | `test.md` | §N.3 drop run- |
| `run-coverage.md` | `coverage.md` | §N.3 drop run- |
| `run-qodana.md` | `scan-qodana.md` | §N.3 + §N.7 |
| `run-sonar.md` | `scan-sonar.md` | §N.3 + §N.7 |
| `run-code-diagnostics.md` | `scan-roslyn.md` | §N.3 + §N.7 |
| `run-audit-cli.md` | `audit.md` | §N.3 drop run- |
| `run-git-commits.md` | `commit.md` | §N.3 drop run-, singular |
| `run-custom.md` | `custom.md` | §N.3 drop run- |
| `debug-fix-tests.md` | `fix-test.md` | §N.3 compound → fix, singular |
| `debug-fix-coverage.md` | `fix-coverage.md` | §N.3 compound → fix |
| `fix-code-diagnostics.md` | `fix-roslyn.md` | §N.7 tool name |
| `fix-sonar-issues.md` | `fix-sonar.md` | §N.5 drop "issues" |
| `format-code.md` | `format.md` | Simplify |
| `rebuild-all-libraries.md` | `build-all.md` | §N.3 rebuild → build, singular |
| `verify-coverage-sequential.md` | `verify-coverage.md` | Drop qualifier |

**Move to plans/completed/**: `implement-nullability.md`, `refactor-nullability-ordering.md` (one-off tasks, not reusable workflows)

**Delete**: `standards.md` (if redundant with specs — verify first)

---

## Phase 4: Agents (~65 renames)

### Scope renames (applied across ALL agent categories):

`-must-clauses` → `-must`, `-guard-clauses` → `-guard`, `-fluent-validation` → `-fluent`, `-data-annotations` → `-annotation`

### Verb renames by category:

| Category | Old pattern | New pattern | Count |
|----------|------------|-------------|-------|
| Coverage | `coverage-{scope}` | scope-only renames | 4 |
| Test | `test-{scope}` | scope-only renames | 4 |
| Format | `format-{scope}` | scope-only renames | 4 |
| Debug-fix | `debug-and-fix-{scope}` | `fix-coverage-{scope}` | 7 |
| Debug-test | `debug-and-test-{scope}` | `fix-test-{scope}` | 7 |
| XML Docs | `xml-docs-{scope}` | `document-{scope}` | 6 |
| Git Commit | `git-commit-{scope}` | `commit-{scope}` | 11 |
| Qodana | `qodana-{scope}` | `scan-qodana-{scope}` | 7 |
| Roslyn | `roslyn-{scope}` | `scan-roslyn-{scope}` | 7 |
| Roslyn Fix | `roslyn-fix-all` | `fix-roslyn-all` | 1 |
| Sonar Run | `sonar-run` | `scan-sonar` | 1 |
| Sonar Fix | `sonar-fix-{sev}` | `fix-sonar-{sev}` | 5 |
| Clean | `clean-{target}` | singular: `artifact`, `log` | 2 |
| Analysis | `analyze-gaps` | `audit-gap` | 1 |
| Analysis | `implement-vertical-slice` | `scaffold-vertical-slice` | 1 |

**No change**: `generate-*` (4), `clean-all` (1), `coverage-all/core/testing` (3), `test-all/core/testing` (3), `format-all/core/testing` (3)

---

## Phase 5: Commands (2 renames, 1 merge)

| Old | New | Reason |
|-----|-----|--------|
| `tests.md` | `test.md` | §N.5 singular |
| `debug.md` | `fix.md` | §N.3 debug → fix |
| `roslyn.md` + `sonar.md` | `scan.md` | Merge into single scan intent contract |

---

## Phase 6: Roles (6 renames)

| Old | New | Reason |
|-----|-----|--------|
| `code-reviewer.md` | `reviewer.md` | Simplify |
| `software-engineer.md` | `builder.md` | Use archetype name |
| `test-engineer.md` | `verifier.md` | Use archetype name |
| `senior-engineer.md` | `owner.md` | Use archetype name |
| `devops-engineer.md` | `shipper.md` | Use archetype name |
| `test-analyst.md` | `planner.md` | Use archetype name |

**Keep as-is**: `architect.md`, `business-analyst.md`, `lead-engineer.md`, `principal-engineer.md`

---

## Phase 7: Deletions

| File | Reason |
|------|--------|
| `docs/ai/automation-plan.md` | 100% duplication of CLAUDE.md + README.md + INDEX.md |

---

## Phase 8: Adapter Cascade (CRITICAL)

Every rename above MUST cascade to ALL adapter tiers:

| Tier | Location | Files | Pattern |
|------|----------|-------|---------|
| Claude commands | `.claude/commands/*.md` | ~77 | Rename file + update `docs/ai/agents/` path inside |
| Claude skills | `.claude/skills/*/SKILL.md` | ~15 | Rename dir + update `docs/ai/skills/` path inside |
| Claude rules | `.claude/rules/*.md` | ~10 | Rename file + update `docs/ai/rules/` path inside |
| Gemini workflows | `.agent/workflows/*.md` | ~68 | Rename file + update `docs/ai/agents/` path inside |
| Cursor rules | `.cursor/rules/*.mdc` | ~8 | Rename file + update path inside |
| Copilot skills | `.github/skills/*/SKILL.md` | ~6 | Rename dir + update path inside |
| Copilot agents | `.github/agents/*.agent.md` | ~4 | Update internal references |
| Copilot instructions | `.github/instructions/*.md` | ~8 | Rename + update path |
| Pi skills | `.pi/skills/*/SKILL.md` | ~14 | Rename dir + update path |
| Pi prompts | `.pi/prompts/*.md` | ~80 | Rename file + update path |
| Amazon Q | `.amazonq/rules/*.md` | ~2 | Update references |
| Cline | `.clinerules/*.md` | ~1 | Update references |
| Windsurf | `.windsurf/rules/*.md` | ~2 | Update references |
| Root adapters | `CLAUDE.md`, `AGENTS.md`, `GEMINI.md` | 3 | Update all paths |
| Per-layer AGENTS | `src/PineGuard.*/AGENTS.md` | ~5 | Update references |
| Skills INDEX | `docs/ai/skills/INDEX.md` | 1 | Full rewrite |
| README | `docs/ai/README.md` | 1 | Update all paths |
| Taxonomy | `docs/ai/meta/taxonomy.md` | 1 | Already correct |

---

## Execution Strategy

**Scripted via PowerShell** — manual renames of 400+ files is not viable.

For each phase:
1. PowerShell script renames the files/dirs
2. PowerShell script does find-and-replace across ALL `.md`, `.mdc`, `.ps1` files for old → new paths
3. Verification grep confirms zero remaining references to old names
4. Git commit per phase

**Verification after ALL phases**:
```powershell
# Must return ZERO hits
rg "debug-and-fix|debug-and-test|xml-docs-|git-commit-|run-tests|run-coverage|run-qodana|run-sonar|run-code-diagnostics|must-clauses|guard-clauses|fluent-validation|data-annotations|unit-tests|code-diagnostics|roslyn-run|roslyn-fix|scan-run|scan-fix|improve-code-coverage|implement-core|implement-must|implement-guard|implement-fluent|implement-data|implement-unit|implement-external|sonar-run|sonar-fix|analyze-gaps|automation-plan" --type md --type-add "mdc:*.mdc" --type mdc
```

---

## Risk Mitigation

1. **Git branch**: All work on `refactor/naming-convention` branch
2. **One phase per commit**: Easy to bisect if something breaks
3. **Verification grep after each phase**: Catch stale references immediately
4. **No logic changes**: Only file renames and path updates — zero behavioral changes
