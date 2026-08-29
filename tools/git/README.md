# tools/git

Small PowerShell helpers for creating clean, scoped commits.

## Directory Structure

```
tools/git/
├── Run-Commits.ps1              # Master orchestrator (preferred entrypoint)
├── Import-GitHelpers.ps1        # Aggregator that dot-sources shared helpers
├── Commit-Agent.ps1             # Scoped commit: every assistant adapter surface (.agent, .claude, .pi, .github/*, .clinerules, .cursor, .windsurf, .junie, .amazonq, .vscode) + root AGENTS.md/CLAUDE.md/GEMINI.md + per-project AGENTS.md
├── Commit-Core.ps1              # Scoped commit: PineGuard.Core
├── Commit-DataAnnotations.ps1   # Scoped commit: PineGuard.DataAnnotations
├── Commit-Docs.ps1              # Scoped commit: docs/
├── Commit-FluentValidation.ps1  # Scoped commit: PineGuard.FluentValidation
├── Commit-GuardClauses.ps1      # Scoped commit: PineGuard.GuardClauses
├── Commit-MustClauses.ps1       # Scoped commit: PineGuard.MustClauses
├── Commit-Options.ps1           # Scoped commit: PineGuard.Extensions.Options
├── Commit-Solution.ps1          # Scoped commit: solution-level files
├── Commit-Testing.ps1           # Scoped commit: PineGuard.Testing
└── Commit-Tools.ps1             # Scoped commit: tools/
```

## Typical usage

Dry-run what would be committed:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -DryRun
```

Create scoped commits (interactive commit message editor opens for each commit):

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests
```

Create scoped commits with auto-generated, descriptive messages:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -AutoMessage
```

Optionally rebase if upstream is ahead, then push:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -AutoRebase -Push
```

Safe push (shorthand for `-AutoRebase -Push` with extra guardrails):

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -AutoMessage -SafePush
```

## Parameters (Run-Commits.ps1)

### Scope switches

| Parameter | Description |
|-----------|-------------|
| `-Agent` | Include every assistant adapter surface and every `AGENTS.md` (see the `$paths` array in `Commit-Agent.ps1`, and `docs/ai/meta/adapter-surfaces.md` for the surface inventory). The Brain itself (`docs/`) is committed by `-Docs`. |
| `-Core` | Include PineGuard.Core |
| `-MustClauses` | Include PineGuard.MustClauses |
| `-GuardClauses` | Include PineGuard.GuardClauses |
| `-FluentValidation` | Include PineGuard.FluentValidation |
| `-DataAnnotations` | Include PineGuard.DataAnnotations |
| `-Options` | Include PineGuard.Extensions.Options |
| `-Testing` | Include PineGuard.Testing |
| `-Docs` | Include docs/ |
| `-Tools` | Include tools/ |
| `-Solution` | Include solution-level files |
| `-All` | Enable all scopes (implies `-IncludeTests`) |

### Behaviour switches

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-IncludeTests` | switch | `$false` | Include corresponding test projects in each scope |
| `-AutoMessage` | switch | `$false` | Generate commit messages automatically |
| `-AutoRebase` | switch | `$false` | Run `git pull --rebase --autostash` before/after |
| `-Push` | switch | `$false` | Push to remote after committing |
| `-SafePush` | switch | `$false` | Shorthand for `-AutoRebase -Push` with extra guardrails |
| `-Remote` | string | `origin` | Git remote name |
| `-DryRun` | switch | `$false` | Show what would be committed without making changes |

## Notes

- Scripts refuse to run if you already have staged changes (prevents mixing commits).
- If a scope has no changes, its commit is skipped.
- `-AutoRebase` uses `git pull --rebase --autostash` and stops on conflicts; resolve conflicts then run `git rebase --continue`.
