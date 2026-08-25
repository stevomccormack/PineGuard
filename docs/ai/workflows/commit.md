<!-- metadata_header
type: workflow
id: workflow-commit
version: 1.1
-->

# Workflow: Commit

> [!NOTE]
> Creates clean, scoped commits using the repo's PowerShell helpers under `tools/git/**`.

## Context

- **Role**: [DevOps Engineer](../roles/shipper.md)
- **Reference**: `tools/git/Run-Commits.ps1`

## Parameters

- **Scope**: one scope switch — `-All`, `-Agent`, `-Core`, `-MustClauses`, `-GuardClauses`,
  `-FluentValidation`, `-DataAnnotations`, `-Testing`, `-Docs`, `-Tools`, `-Solution`.
- **IncludeTests**: (optional switch) include the paired `*.UnitTests` project in the same commit.
  Implied by `-All`; has no effect on non-layer scopes (`-Agent`, `-Docs`, `-Tools`, `-Solution`).
- **AutoMessage**: (optional switch) auto-generate the commit message; omit to open the editor per scope.
- **SafePush**: (optional switch) implies `-AutoRebase -Push` — rebases if needed, then pushes with guardrails.

## Auto-Approval

Not auto-approved on any surface. The agent proposes the plan (dry run), the user confirms.
The scripts intentionally refuse to run if staging is non-empty, and only the named scope may be
staged — never `git add -A`. See [`../commands/commit.md`](../commands/commit.md).

## Steps

1. **Preconditions**
   - No staged changes; working tree clean except the files you intend to commit.

2. **Dry-run the commit plan**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -DryRun
   ```

3. **Create scoped commits with auto-generated messages**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage
   ```

4. **(Optional) Safe push**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage -SafePush
   ```

## Common variants

- Interactive commit messages (editor opens per scope):

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

## Notes

- If a scope has no changes, its commit is skipped.
- `.vscode/tasks.json` carries equivalent tasks for human runs (`Git: Run Commits (all, dry run)`,
  `Git: Run Commits (all, auto message)`, `Git: Run Commits (all, auto message, safe push)`);
  an agent must invoke the PowerShell commands directly.
