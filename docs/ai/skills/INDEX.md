# PineGuard Skills Catalog

> Quick-reference index of all skills across the Brain and its three skill-hosting adapter
> surfaces (`.claude/`, `.github/`, `.pi/`). Each entry links to its canonical SKILL.md.
> The full surface inventory lives in [`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md).

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
| [changelog](../../../.claude/skills/changelog/SKILL.md) | *(standalone)* | Generate changelog from git history |
| [dependency-audit](../../../.claude/skills/dependency-audit/SKILL.md) | *(standalone)* | Check NuGet vulnerabilities and outdated packages |
| [ask-council](../../../.claude/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

---

## Pi Adapter Skills (`.pi/skills/`)

Thin wrappers for the Pi adapter. Same delegation contract as the Claude Code set.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [scaffold-rule](../../../.pi/skills/scaffold-rule/SKILL.md) | `scaffold-rule` | Core Rule/Util implementation |
| [scaffold-must](../../../.pi/skills/scaffold-must/SKILL.md) | `scaffold-must` | MustClause implementation |
| [scaffold-guard](../../../.pi/skills/scaffold-guard/SKILL.md) | `scaffold-guard` | GuardClause implementation |
| [scaffold-fluent](../../../.pi/skills/scaffold-fluent/SKILL.md) | `scaffold-fluent` | FluentValidation extension |
| [scaffold-annotation](../../../.pi/skills/scaffold-annotation/SKILL.md) | `scaffold-annotation` | DataAnnotations attribute |
| [scaffold-unit-test](../../../.pi/skills/scaffold-unit-test/SKILL.md) | `scaffold-unit-test` | xUnit test implementation |
| [improve-coverage](../../../.pi/skills/improve-coverage/SKILL.md) | `improve-coverage` | Coverage gap analysis |
| [new-validation](../../../.pi/skills/new-validation/SKILL.md) | `new-validation` | Simple in-memory predicate vertical slice |
| [format-code](../../../.pi/skills/format-code/SKILL.md) | `format-code` | Code formatting |
| [scan-roslyn](../../../.pi/skills/scan-roslyn/SKILL.md) | `scan-roslyn` | Roslyn diagnostics |
| [fix-roslyn](../../../.pi/skills/fix-roslyn/SKILL.md) | `fix-roslyn` | Roslyn warning fixes |
| [scan-sonar](../../../.pi/skills/scan-sonar/SKILL.md) | `scan-sonar` | SonarQube analysis |
| [fix-sonar](../../../.pi/skills/fix-sonar/SKILL.md) | `fix-sonar` | SonarQube issue fixes |
| [document](../../../.pi/skills/document/SKILL.md) | `document` | XML documentation generation |
| [changelog](../../../.pi/skills/changelog/SKILL.md) | *(standalone)* | Generate changelog from git history |
| [dependency-audit](../../../.pi/skills/dependency-audit/SKILL.md) | *(standalone)* | Check NuGet vulnerabilities and outdated packages |
| [ask-council](../../../.pi/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

---

## GitHub Adapter Skills (`.github/skills/`)

Copilot-compatible adapters for GitHub-hosted workflows.

| Skill | Brain Delegate | Description |
|-------|---------------|-------------|
| [scaffold-unit-test](../../../.github/skills/scaffold-unit-test/SKILL.md) | `scaffold-unit-test` | xUnit test implementation |
| [improve-coverage](../../../.github/skills/improve-coverage/SKILL.md) | `improve-coverage` | Coverage gap analysis |
| [scan-roslyn](../../../.github/skills/scan-roslyn/SKILL.md) | `scan-roslyn` | Roslyn diagnostics |
| [fix-roslyn](../../../.github/skills/fix-roslyn/SKILL.md) | `fix-roslyn` | Roslyn warning fixes |
| [scan-sonar](../../../.github/skills/scan-sonar/SKILL.md) | `scan-sonar` | SonarQube analysis |
| [fix-sonar](../../../.github/skills/fix-sonar/SKILL.md) | `fix-sonar` | SonarQube issue fixes |
| [ask-council](../../../.github/skills/ask-council/SKILL.md) | `ask-council` | Pressure-test a decision via LLM Council |

---

## Skill Architecture

```
.claude/skills/        ← Claude Code adapters (context: fork)
.github/skills/        ← GitHub Copilot adapters
.pi/skills/            ← Pi adapters
docs/ai/skills/        ← Brain (canonical, model-agnostic)
    ├── INDEX.md       ← This file
    └── <skill-name>/
        ├── SKILL.md       ← Canonical definition
        └── references/    ← Exemplar pointers (DRY)
```

**Flow**: Adapter → Brain → Specs. Adapters never embed logic; they point to Brain skills.
