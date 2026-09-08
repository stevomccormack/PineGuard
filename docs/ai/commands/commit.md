<!-- metadata_header
type: command
id: cmd-commit
version: 1.0
-->

# Command: Commit

Stages and commits one slice of work with a conventional-commits subject and a prose body.

## Intent Mapping

| Command | Slice | Agent |
|---------|-------|-------|
| `/commit-all` | Everything staged-worthy in the working tree | `docs/ai/agents/commit-all.md` |
| `/commit-core` | PineGuard.Core + its tests | `docs/ai/agents/commit-core.md` |
| `/commit-must` | PineGuard.MustClauses + its tests | `docs/ai/agents/commit-must.md` |
| `/commit-guard` | PineGuard.GuardClauses + its tests | `docs/ai/agents/commit-guard.md` |
| `/commit-fluent` | PineGuard.FluentValidation + its tests | `docs/ai/agents/commit-fluent.md` |
| `/commit-annotation` | PineGuard.DataAnnotations + its tests | `docs/ai/agents/commit-annotation.md` |
| `/commit-testing` | PineGuard.Testing + its tests | `docs/ai/agents/commit-testing.md` |
| `/commit-doc` | `docs/` and the root `README.md` | `docs/ai/agents/commit-doc.md` |
| `/commit-agent` | The Brain and its adapter surfaces | `docs/ai/agents/commit-agent.md` |
| `/commit-tool` | `tools/` | `docs/ai/agents/commit-tool.md` |
| `/commit-solution` | Solution, props, and build configuration | `docs/ai/agents/commit-solution.md` |
| _(none)_ | `.github/workflows` — the `Ci` meta-scope | No dedicated slash command; run `Run-Commits.ps1 -Scope Ci` directly |

**Shared orchestration**: `docs/ai/workflows/commit.md`

`Ci` is the one scope with no slash command of its own. It exists in
`tools/git/Run-Commits.ps1`'s meta-scope table (`Ci = @('.github/workflows')`, split out of `Agent`
so CI configuration is not committed as an assistant-adapter surface), and
[`../agents/commit-agent.md`](../agents/commit-agent.md) points at it, but it is driven from the
script rather than the palette:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope Ci -WhatIf
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope Ci -AutoMessage
```

Commits are **not** auto-approved: the agent proposes the message and the staged set, the user
confirms. Only the named slice may be staged — never `git add -A`.
