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

- **Scope**: `-Scope <Name[,Name...]>` — one or more of the fourteen registry scopes (`Core`,
  `MustClauses`, `GuardClauses`, `DataAnnotations`, `FluentValidation`, `Options`,
  `DependencyInjection`, `AspNetCore`, `ErrorOr`, `FluentResults`, `OneOf`, `MediatR`,
  `Analyzers`, `Testing`) or the five meta-scopes (`Agent`, `Docs`, `Tools`, `Solution`, `Ci`).
  `-All` commits every scope in one invocation instead.
- **IncludeTests**: (optional switch) include the paired `*.UnitTests` project in the same commit.
  Implied by `-All`; has no effect on the five meta-scopes (`-Scope Agent`, `-Scope Docs`,
  `-Scope Tools`, `-Scope Solution`, `-Scope Ci`).
- **AutoMessage**: (optional switch) auto-generate the commit message; omit to open the editor per scope.
- **Push** / **Rebase**: (optional switches) `-Rebase` fetches and rebases onto the remote if
  behind, before and after committing; `-Push` pushes afterward. `-Push -Rebase` together is the
  old `-SafePush` shorthand — rebase if needed, then push.

## Auto-Approval

Not auto-approved on any surface. The agent proposes the plan (dry run), the user confirms.
The script intentionally refuses to run if staging is non-empty, and only the named scope may be
staged — never `git add -A`. See [`../commands/commit.md`](../commands/commit.md).

## Steps

1. **Preconditions**
   - No staged changes; working tree clean except the files you intend to commit.

2. **Dry-run the commit plan**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -WhatIf
   ```

3. **Create scoped commits with auto-generated messages**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage
   ```

4. **(Optional) Rebase and push**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage -Push -Rebase
   ```

## Common variants

- Interactive commit messages (editor opens per scope):

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All
  ```

- Core-only (plus tests):

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope Core -IncludeTests -AutoMessage
  ```

- Tools-only:

  ```powershell
  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope Tools -AutoMessage
  ```

## Notes

- If a scope has no changes, its commit is skipped.
- `.vscode/tasks.json` carries equivalent tasks for human runs (`Git: Run Commits (all, dry run)`,
  `Git: Run Commits (all, auto message)`, `Git: Run Commits (all, auto message, safe push)`);
  an agent must invoke the PowerShell commands directly.
