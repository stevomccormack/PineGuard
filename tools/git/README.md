# tools/git

One PowerShell script for creating clean, scoped commits, driven by the PineGuard project
registry (`tools/.shared/dotnet-projects.ps1`) plus a small fixed table of cross-cutting
meta-scopes.

## Directory Structure

```
tools/git/
└── Run-Commits.ps1   # -Scope <Name[,Name...]> | -All, in-process, no child pwsh spawned
```

`Run-Commits.ps1` replaced sixteen near-identical `Commit-*.ps1` scripts (one per scope) and the
old switch-per-scope orchestrator. Every registry scope (`Core`, `MustClauses`, `GuardClauses`,
`DataAnnotations`, `FluentValidation`, `Options`, `DependencyInjection`, `AspNetCore`, `ErrorOr`,
`FluentResults`, `OneOf`, `MediatR`, `Analyzers`, `Testing`) and five meta-scopes that do not
correspond to a single shipped project (`Agent`, `Docs`, `Tools`, `Solution`, `Ci`) are valid
`-Scope` values; the registry ones are validated dynamically, so a new registry scope needs no
edit here to become usable.

## Typical usage

Dry-run what would be committed:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -WhatIf
```

Create scoped commits (interactive commit message editor opens for each commit):

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests
```

Create scoped commits with auto-generated, conventional-commits-style messages:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -AutoMessage
```

Commit one or more specific scopes:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope Core,Tools -AutoMessage
```

Rebase onto the remote if needed, then push:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -IncludeTests -AutoMessage -Push -Rebase
```

## Parameters (Run-Commits.ps1)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Scope` | string[] | — | One or more of the fourteen registry scopes, or the five meta-scopes `Agent`, `Docs`, `Tools`, `Solution`, `Ci`. Case-insensitive. Ignored if `-All` is also given. |
| `-All` | switch | `$false` | Every registry scope and every meta-scope (implies `-IncludeTests`). |
| `-IncludeTests` | switch | `$false` | For registry-derived scopes, also stage the paired test project's directory. Has no effect on the five meta-scopes. |
| `-Message` | string | — | An explicit commit message, applied to every scope selected in this invocation. |
| `-AutoMessage` | switch | `$false` | Generate a conventional-commits-style message (subject + flowing prose body) per scope instead of requiring `-Message` or opening an editor. |
| `-Push` | switch | `$false` | Push to `-Remote` after all selected scopes have committed. Alone, throws if the local branch is behind upstream; combined with `-Rebase`, becomes a safe push (rebase-if-behind, then push). |
| `-Rebase` | switch | `$false` | Fetch and rebase onto `-Remote` (`git pull --rebase --autostash`) if behind, before and after committing. |
| `-Remote` | string | `origin` | Git remote name for `-Push` and `-Rebase`. |
| `-WhatIf` | switch | `$false` | Preview what would be staged and committed, without making changes. `-DryRun` is a supported alias of the same switch. |

## Notes

- Scripts throw an error and stop if the git index already has staged changes, instead of unstaging them for you (prevents mixing commits, and avoids ever running `git restore --staged .`, a Tier 0 destructive command — see `docs/ai/specs/safety.md` §2.1). Commit or unstage the pre-existing changes yourself, then re-run.
- If a scope has no changes, its commit is skipped.
- `README.md` is staged by exactly one scope: `Docs`. `.github/workflows` is staged by exactly one scope: `Ci` (moved out of `Agent`, which used to include it).
- `-Rebase` uses `git pull --rebase --autostash` and stops on conflicts; resolve conflicts then run `git rebase --continue`.
- Everything runs in one pwsh process — no child `pwsh` is spawned per scope — so `-Message` reaches every commit in the invocation, and `-AutoMessage`'s generator (`tools/.shared/git.ps1`'s `New-AutoCommitMessage`) is a best-effort mechanical summary of *what* changed, not *why*; prefer `-Message` for anything worth explaining.
