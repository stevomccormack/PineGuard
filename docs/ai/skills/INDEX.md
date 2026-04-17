# PineGuard Skills Catalog

> Quick-reference index of all skills across the three tiers.
> Each entry links to its canonical SKILL.md.

## Brain Skills (`docs/ai/skills/`)

Canonical, model-agnostic skill definitions. These are the source of truth.

### Implementation

| Skill | ID | Description |
|-------|----|-------------|
| [Implement Core Rule](implement-core-rule/SKILL.md) | `pineguard.skill.scaffold-rule` | Add a Core Rule or Util in `PineGuard.Core` |
| [Implement Must Clauses](scaffold-musts/SKILL.md) | `pineguard.skill.scaffold-must` | Add a MustClause fluent validation method |
| [Implement Guard Clauses](scaffold-guards/SKILL.md) | `pineguard.skill.scaffold-guard` | Add a GuardClause (`Guard.Against.X`) |
| [Implement Fluent Validation](implement-fluent-validation/SKILL.md) | `pineguard.skill.scaffold-fluent` | Add a FluentValidation `IRuleBuilder` extension |
| [Implement Data Annotations](implement-data-annotations/SKILL.md) | `pineguard.skill.scaffold-annotation` | Add a DataAnnotations `ValidationAttribute` |
| [Implement Unit Tests](implement-unit-tests/SKILL.md) | `pineguard.skill.scaffold-unit-test` | Add xUnit tests for any PineGuard class |

### Documentation

| Skill | ID | Description |
|-------|----|-------------|
| [Generate XML Docs](generate-xml-docs/SKILL.md) | `pineguard.skill.document` | Generate gold-standard XML documentation for all public members |

### Quality & Analysis

| Skill | ID | Description |
|-------|----|-------------|
| [Improve Code Coverage](improve-code-coverage/SKILL.md) | `pineguard.skill.improve-coverage` | Analyze gaps and add tests to reach 100% coverage |
| [Roslyn Run](roslyn-run/SKILL.md) | `pineguard.skill.scan-roslyn` | Run Roslyn compiler diagnostics and report CS warnings |
| [Roslyn Fix](roslyn-fix/SKILL.md) | `pineguard.skill.fix-roslyn` | Fix all Roslyn CS warnings using idiomatic C# |
| [Scan Run](scan-run/SKILL.md) | `pineguard.skill.scan-sonar` | Run SonarQube static analysis |
| [Scan Fix](scan-fix/SKILL.md) | `pineguard.skill.fix-sonar` | Fix SonarQube issues by severity |

### Maintenance & Scaffolding

| Skill | ID | Description |
|-------|----|-------------|
| [Format Code](format-code/SKILL.md) | `pineguard.skill.format-code` | Run `dotnet format` to enforce `.editorconfig` rules |
| [Create Workflow](create-workflow/SKILL.md) | `pineguard.skill.scaffold-workflow` | Create a canonical agent playbook in `docs/ai/agents/` |
| [Scaffold Quality Tool](scaffold-quality-tool/SKILL.md) | `pineguard.skill.scaffold-quality-tool` | Add a new quality/inspection tool as a first-class Brain citizen |

### Meta

| Skill | Description |
|-------|-------------|
| [Skills Format](skills-format/SKILL.md) | Template defining the standard SKILL.md structure (not executable) |

---

## Claude Code Adapter Skills (`.claude/skills/`)

Thin `context: fork` wrappers that delegate to Brain skills or standalone tooling.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [implement-core-rule](../../.claude/skills/scaffold-rule/SKILL.md) | `implement-core-rule` | Core Rule/Util implementation |
| [scaffold-must](../../.claude/skills/scaffold-must/SKILL.md) | `scaffold-musts` | MustClause implementation |
| [scaffold-guard](../../.claude/skills/scaffold-guard/SKILL.md) | `scaffold-guards` | GuardClause implementation |
| [implement-fluent-validation](../../.claude/skills/scaffold-fluent/SKILL.md) | `implement-fluent-validation` | FluentValidation extension |
| [implement-data-annotations](../../.claude/skills/scaffold-annotation/SKILL.md) | `implement-data-annotations` | DataAnnotations attribute |
| [implement-unit-tests](../../.claude/skills/scaffold-unit-test/SKILL.md) | `implement-unit-tests` | xUnit test implementation |
| [improve-coverage](../../.claude/skills/improve-coverage/SKILL.md) | `improve-code-coverage` | Coverage gap analysis |
| [new-validation](../../.claude/skills/new-validation/SKILL.md) | *(multi-skill orchestration)* | Simple in-memory predicate vertical slice |
| [format-code](../../.claude/skills/format-code/SKILL.md) | `format-code` | Code formatting |
| [roslyn-run](../../.claude/skills/scan-roslyn/SKILL.md) | `roslyn-run` | Roslyn diagnostics |
| [roslyn-fix](../../.claude/skills/fix-roslyn/SKILL.md) | `roslyn-fix` | Roslyn warning fixes |
| [scan-run](../../.claude/skills/scan-sonar/SKILL.md) | `scan-run` | SonarQube analysis |
| [scan-fix](../../.claude/skills/fix-sonar/SKILL.md) | `scan-fix` | SonarQube issue fixes |
| [generate-xml-docs](../../.claude/skills/document/SKILL.md) | `generate-xml-docs` | XML documentation generation |
| [changelog](../../.claude/skills/changelog/SKILL.md) | *(standalone)* | Generate changelog from git history |
| [dependency-audit](../../.claude/skills/dependency-audit/SKILL.md) | *(standalone)* | Check NuGet vulnerabilities and outdated packages |

---

## GitHub Adapter Skills (`.github/skills/`)

Copilot-compatible adapters for GitHub-hosted workflows.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [implement-unit-tests](../../.github/skills/scaffold-unit-test/SKILL.md) | `implement-unit-tests` | xUnit test implementation |
| [improve-coverage](../../.github/skills/improve-coverage/SKILL.md) | `improve-code-coverage` | Coverage gap analysis |
| [roslyn-run](../../.github/skills/scan-roslyn/SKILL.md) | `roslyn-run` | Roslyn diagnostics |
| [roslyn-fix](../../.github/skills/fix-roslyn/SKILL.md) | `roslyn-fix` | Roslyn warning fixes |
| [scan-run](../../.github/skills/scan-sonar/SKILL.md) | `scan-run` | SonarQube analysis |
| [scan-fix](../../.github/skills/fix-sonar/SKILL.md) | `scan-fix` | SonarQube issue fixes |

---

## Skill Architecture

```
.claude/skills/        ← Claude Code adapters (context: fork)
.github/skills/        ← GitHub Copilot adapters
docs/ai/skills/        ← Brain (canonical, model-agnostic)
    ├── INDEX.md       ← This file
    └── <skill-name>/
        ├── SKILL.md       ← Canonical definition
        └── references/    ← Exemplar pointers (DRY)
```

**Flow**: Adapter → Brain → Specs. Adapters never embed logic; they point to Brain skills.
