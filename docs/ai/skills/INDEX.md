# PineGuard Skills Catalog

> Quick-reference index of all skills across the Brain and its three skill-hosting adapter
> surfaces (`.claude/`, `.github/`, `.agents/`). OpenCode hosts no skills of its own: it loads
> `.claude/skills/` and `.agents/skills/` natively. Each entry links to its canonical SKILL.md,
> except the `.agents/skills/`-only command-family routers noted in their own section below, which
> have no Brain-level canonical file. The full surface inventory lives in
> [`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md).

## Brain Skills (`docs/ai/skills/`)

Canonical, model-agnostic skill definitions. These are the source of truth.

### Implementation

| Skill | ID | Description |
|-------|----|-------------|
| [Scaffold Rule](scaffold-rule/SKILL.md) | `pineguard.skill.scaffold-rule` | Add a Core Rule or Util in `PineGuard.Core` |
| [Scaffold Must](scaffold-must/SKILL.md) | `pineguard.skill.scaffold-must` | Add a MustClause fluent validation method |
| [Scaffold Guard](scaffold-guard/SKILL.md) | `pineguard.skill.scaffold-guard` | Add a GuardClause (`Guard.Against.X`) |
| [Scaffold Fluent](scaffold-fluent/SKILL.md) | `pineguard.skill.scaffold-fluent` | Add a FluentValidation `IRuleBuilder` extension |
| [Scaffold Annotation](scaffold-annotation/SKILL.md) | `pineguard.skill.scaffold-annotation` | Add a DataAnnotations `ValidationAttribute` |
| [Scaffold Unit Test](scaffold-unit-test/SKILL.md) | `pineguard.skill.scaffold-unit-test` | Add xUnit tests for any PineGuard class |
| [New Validation](new-validation/SKILL.md) | `pineguard.skill.new-validation` | Drive a predicate-based validation through every layer |

### Documentation

| Skill | ID | Description |
|-------|----|-------------|
| [Document](document/SKILL.md) | `pineguard.skill.document` | Generate gold-standard XML documentation for all public members |

### Quality & Analysis

| Skill | ID | Description |
|-------|----|-------------|
| [Improve Coverage](improve-coverage/SKILL.md) | `pineguard.skill.improve-coverage` | Analyze gaps and add tests to reach 100% coverage |
| [Scan Roslyn](scan-roslyn/SKILL.md) | `pineguard.skill.scan-roslyn` | Run Roslyn compiler diagnostics and report CS warnings |
| [Fix Roslyn](fix-roslyn/SKILL.md) | `pineguard.skill.fix-roslyn` | Fix all Roslyn CS warnings using idiomatic C# |
| [Scan Sonar](scan-sonar/SKILL.md) | `pineguard.skill.scan-sonar` | Run SonarQube static analysis |
| [Fix Sonar](fix-sonar/SKILL.md) | `pineguard.skill.fix-sonar` | Fix SonarQube issues by severity |

### Maintenance & Scaffolding

| Skill | ID | Description |
|-------|----|-------------|
| [Format Code](format-code/SKILL.md) | `pineguard.skill.format-code` | Run `dotnet format` to enforce `.editorconfig` rules |
| [Scaffold Workflow](scaffold-workflow/SKILL.md) | `pineguard.skill.scaffold-workflow` | Create a canonical agent playbook and cascade it to every adapter surface |
| [Scaffold Quality Tool](scaffold-quality-tool/SKILL.md) | `pineguard.skill.scaffold-quality-tool` | Add a new quality/inspection tool as a first-class Brain citizen |

### Decision Support

| Skill | ID | Description |
|-------|----|-------------|
| [Ask Council](ask-council/SKILL.md) | `pineguard.skill.ask-council` | Pressure-test a decision via 5 advisors + anonymous peer review + chairman synthesis |

---

## Claude Code Adapter Skills (`.claude/skills/`)

Thin `context: fork` wrappers that delegate to Brain skills or standalone tooling.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [scaffold-rule](../../../.claude/skills/scaffold-rule/SKILL.md) | `scaffold-rule` | Core Rule/Util implementation |
| [scaffold-must](../../../.claude/skills/scaffold-must/SKILL.md) | `scaffold-must` | MustClause implementation |
| [scaffold-guard](../../../.claude/skills/scaffold-guard/SKILL.md) | `scaffold-guard` | GuardClause implementation |
| [scaffold-fluent](../../../.claude/skills/scaffold-fluent/SKILL.md) | `scaffold-fluent` | FluentValidation extension |
| [scaffold-annotation](../../../.claude/skills/scaffold-annotation/SKILL.md) | `scaffold-annotation` | DataAnnotations attribute |
| [scaffold-unit-test](../../../.claude/skills/scaffold-unit-test/SKILL.md) | `scaffold-unit-test` | xUnit test implementation |
| [improve-coverage](../../../.claude/skills/improve-coverage/SKILL.md) | `improve-coverage` | Coverage gap analysis |
| [new-validation](../../../.claude/skills/new-validation/SKILL.md) | `new-validation` | Simple in-memory predicate vertical slice |
| [format-code](../../../.claude/skills/format-code/SKILL.md) | `format-code` | Code formatting |
| [scan-roslyn](../../../.claude/skills/scan-roslyn/SKILL.md) | `scan-roslyn` | Roslyn diagnostics |
| [fix-roslyn](../../../.claude/skills/fix-roslyn/SKILL.md) | `fix-roslyn` | Roslyn warning fixes |
| [scan-sonar](../../../.claude/skills/scan-sonar/SKILL.md) | `scan-sonar` | SonarQube analysis |
| [fix-sonar](../../../.claude/skills/fix-sonar/SKILL.md) | `fix-sonar` | SonarQube issue fixes |
| [document](../../../.claude/skills/document/SKILL.md) | `document` | XML documentation generation |
| [scaffold-workflow](../../../.claude/skills/scaffold-workflow/SKILL.md) | `scaffold-workflow` | New agent playbook + adapter cascade |
| [scaffold-quality-tool](../../../.claude/skills/scaffold-quality-tool/SKILL.md) | `scaffold-quality-tool` | New quality/inspection tool scaffold |
| [changelog](../../../.claude/skills/changelog/SKILL.md) | *(surface-native — [§2.1](../meta/adapter-surfaces.md#surface-native-utility-skills-declared-exemption))* | Generate changelog from git history |
| [dependency-audit](../../../.claude/skills/dependency-audit/SKILL.md) | *(surface-native — [§2.1](../meta/adapter-surfaces.md#surface-native-utility-skills-declared-exemption))* | Check NuGet vulnerabilities and outdated packages |
| [ask-council](../../../.claude/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

---

## GitHub Adapter Skills (`.github/skills/`)

Copilot-compatible wrappers - one per Brain skill.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [scaffold-rule](../../../.github/skills/scaffold-rule/SKILL.md) | `scaffold-rule` | Core Rule/Util implementation |
| [scaffold-must](../../../.github/skills/scaffold-must/SKILL.md) | `scaffold-must` | MustClause implementation |
| [scaffold-guard](../../../.github/skills/scaffold-guard/SKILL.md) | `scaffold-guard` | GuardClause implementation |
| [scaffold-fluent](../../../.github/skills/scaffold-fluent/SKILL.md) | `scaffold-fluent` | FluentValidation extension |
| [scaffold-annotation](../../../.github/skills/scaffold-annotation/SKILL.md) | `scaffold-annotation` | DataAnnotations attribute |
| [scaffold-unit-test](../../../.github/skills/scaffold-unit-test/SKILL.md) | `scaffold-unit-test` | xUnit test implementation |
| [new-validation](../../../.github/skills/new-validation/SKILL.md) | `new-validation` | Simple in-memory predicate vertical slice |
| [improve-coverage](../../../.github/skills/improve-coverage/SKILL.md) | `improve-coverage` | Coverage gap analysis |
| [format-code](../../../.github/skills/format-code/SKILL.md) | `format-code` | Code formatting |
| [scan-roslyn](../../../.github/skills/scan-roslyn/SKILL.md) | `scan-roslyn` | Roslyn diagnostics |
| [fix-roslyn](../../../.github/skills/fix-roslyn/SKILL.md) | `fix-roslyn` | Roslyn warning fixes |
| [scan-sonar](../../../.github/skills/scan-sonar/SKILL.md) | `scan-sonar` | SonarQube analysis |
| [fix-sonar](../../../.github/skills/fix-sonar/SKILL.md) | `fix-sonar` | SonarQube issue fixes |
| [document](../../../.github/skills/document/SKILL.md) | `document` | XML documentation generation |
| [scaffold-workflow](../../../.github/skills/scaffold-workflow/SKILL.md) | `scaffold-workflow` | New agent playbook + adapter cascade |
| [scaffold-quality-tool](../../../.github/skills/scaffold-quality-tool/SKILL.md) | `scaffold-quality-tool` | New quality/inspection tool scaffold |
| [ask-council](../../../.github/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

---

## Agents Adapter Skills (`.agents/skills/`)

Generic `AGENTS.md`-convention adapters. Same delegation contract as the Claude Code set.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [scaffold-rule](../../../.agents/skills/scaffold-rule/SKILL.md) | `scaffold-rule` | Core Rule/Util implementation |
| [scaffold-must](../../../.agents/skills/scaffold-must/SKILL.md) | `scaffold-must` | MustClause implementation |
| [scaffold-guard](../../../.agents/skills/scaffold-guard/SKILL.md) | `scaffold-guard` | GuardClause implementation |
| [scaffold-fluent](../../../.agents/skills/scaffold-fluent/SKILL.md) | `scaffold-fluent` | FluentValidation extension |
| [scaffold-annotation](../../../.agents/skills/scaffold-annotation/SKILL.md) | `scaffold-annotation` | DataAnnotations attribute |
| [scaffold-unit-test](../../../.agents/skills/scaffold-unit-test/SKILL.md) | `scaffold-unit-test` | xUnit test implementation |
| [improve-coverage](../../../.agents/skills/improve-coverage/SKILL.md) | `improve-coverage` | Coverage gap analysis |
| [new-validation](../../../.agents/skills/new-validation/SKILL.md) | `new-validation` | Simple in-memory predicate vertical slice |
| [format-code](../../../.agents/skills/format-code/SKILL.md) | `format-code` | Code formatting |
| [scan-roslyn](../../../.agents/skills/scan-roslyn/SKILL.md) | `scan-roslyn` | Roslyn diagnostics |
| [fix-roslyn](../../../.agents/skills/fix-roslyn/SKILL.md) | `fix-roslyn` | Roslyn warning fixes |
| [scan-sonar](../../../.agents/skills/scan-sonar/SKILL.md) | `scan-sonar` | SonarQube analysis |
| [fix-sonar](../../../.agents/skills/fix-sonar/SKILL.md) | `fix-sonar` | SonarQube issue fixes |
| [document](../../../.agents/skills/document/SKILL.md) | `document` | XML documentation generation |
| [scaffold-workflow](../../../.agents/skills/scaffold-workflow/SKILL.md) | `scaffold-workflow` | New agent playbook + adapter cascade |
| [scaffold-quality-tool](../../../.agents/skills/scaffold-quality-tool/SKILL.md) | `scaffold-quality-tool` | New quality/inspection tool scaffold |
| [changelog](../../../.agents/skills/changelog/SKILL.md) | *(surface-native — [§2.1](../meta/adapter-surfaces.md#surface-native-utility-skills-declared-exemption))* | Generate changelog from git history |
| [dependency-audit](../../../.agents/skills/dependency-audit/SKILL.md) | *(surface-native — [§2.1](../meta/adapter-surfaces.md#surface-native-utility-skills-declared-exemption))* | Check NuGet vulnerabilities and outdated packages |
| [ask-council](../../../.agents/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

### `.agents/skills/`-only: Command-Family Routers

Codex carries no per-command prompt-file format at all, so `.agents/skills/` also carries one
`SKILL.md` per multi-command family in [`docs/ai/commands/`](../commands/) — these have **no**
Brain delegate under `docs/ai/skills/`, unlike every entry above. Each reads its matching
`docs/ai/commands/<family>.md` intent table and resolves the user's requested scope/tool/severity
to the row's `docs/ai/agents/*.md` playbook. See
[`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §2.1.1 for the full rationale and
the excluded families (`ask-council`, `document`, `scaffold-*`, and the Claude-only release family).

| Skill | Command contract | Description |
|-------|-------------------|-------------|
| [commit](../../../.agents/skills/commit/SKILL.md) | [`commands/commit.md`](../commands/commit.md) | Route `/commit-*` to its scoped commit agent |
| [scan](../../../.agents/skills/scan/SKILL.md) | [`commands/scan.md`](../commands/scan.md) | Route `/scan-roslyn-*`, `/scan-qodana-*`, `/scan-sonar` to its tool+scope agent |
| [fix](../../../.agents/skills/fix/SKILL.md) | [`commands/fix.md`](../commands/fix.md) | Route `/fix-coverage-*`, `/fix-test-*`, `/fix-roslyn-all`, `/fix-sonar-*` to its agent |
| [test](../../../.agents/skills/test/SKILL.md) | [`commands/test.md`](../commands/test.md) | Route `/test-*` to its scoped test agent |
| [coverage](../../../.agents/skills/coverage/SKILL.md) | [`commands/coverage.md`](../commands/coverage.md) | Route `/coverage-*` to its scoped coverage agent |
| [format](../../../.agents/skills/format/SKILL.md) | [`commands/format.md`](../commands/format.md) | Route `/format-*` to its scoped format agent |
| [clean](../../../.agents/skills/clean/SKILL.md) | [`commands/clean.md`](../commands/clean.md) | Route `/clean-*` to its target-scoped clean agent |
| [audit](../../../.agents/skills/audit/SKILL.md) | [`commands/audit.md`](../commands/audit.md) | Route `/audit-cli`, `/audit-gap` to its agent |

---

## Skill Architecture

```
.claude/skills/        ← Claude Code adapters (context: fork)
.github/skills/        ← GitHub Copilot adapters
.agents/skills/        ← Generic AGENTS.md-convention adapters
docs/ai/skills/        ← Brain (canonical, model-agnostic)
    ├── INDEX.md       ← This file
    └── <skill-name>/
        ├── SKILL.md       ← Canonical definition
        └── references/    ← Exemplar pointers (DRY)
```

**Flow**: Adapter → Brain → Specs. Adapters never embed logic; they point to Brain skills. The two
surface-native utility skills are a declared exemption — see
[adapter-surfaces §2.1](../meta/adapter-surfaces.md#surface-native-utility-skills-declared-exemption).
