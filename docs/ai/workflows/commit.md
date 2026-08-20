<!-- metadata_header
type: workflow
id: workflow-run-git-commits
version: 1.0
-->

# Workflow: Run Scoped Git Commits (tools/git)

## Goal

Create clean, scoped commits using the repo’s PowerShell helpers under `tools/git/**`.

## Preconditions / Safety

- No staged changes (the scripts intentionally refuse to run if staging is non-empty).
- Working tree should be clean except for the files you intend to commit.
- Prefer running a dry run first to see what will be included.

## Recommended execution order

1. Dry-run the commit plan:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -DryRun
   ```

2. Create scoped commits with auto-generated messages:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage
   ```

3. (Optional) Safe push (rebases if needed, then pushes with extra guardrails):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage -SafePush
   ```

## Common variants

- Interactive commit messages (editor opens per-scope):

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All
  ```

- Core-only (plus tests):

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Core -IncludeTests -AutoMessage
  ```

- Tools-only:

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Tools -AutoMessage
  ```

## VS Code tasks

If available in `.vscode/tasks.json`, prefer using the built-in tasks:

- `Git: Run Commits (all, dry run)`
- `Git: Run Commits (all, auto message)`
- `Git: Run Commits (all, auto message, safe push)`

## Notes

- `-All` expands to all scopes and implicitly sets `-IncludeTests`.
- `-SafePush` implies `-AutoRebase -Push`.
- If a scope has no changes, its commit is skipped.
