# Skill: Format Code

**ID**: pineguard.skill.format-code
**Version**: 1.0

## 1. Context & Goal

Run `dotnet format` to enforce `.editorconfig` rules across PineGuard source and test projects.

## 2. Inputs

- **Target Scope**: (e.g., `Core`, `MustClauses`, `All`)

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
>
> 1.  **EditorConfig is the source of truth**: The `.editorconfig` at the repo root defines all formatting rules. Do not override via CLI flags.
> 2.  **Verify before committing**: Use `-VerifyNoChanges` to check formatting status without modifying files.
> 3.  **Format source first**: When targeting a specific scope, format the source project. Use `-Scope All` or `-Solution` to include tests.

## 4. Execution Steps

1.  **Run Format Command**
    - Run the script: `tools/code-formatter/Run-Format.ps1 -Scope [ScopeName]`
    - _Or_ use dotnet directly: `dotnet format [path-to-project-or-solution]`

2.  **Verify Results**
    - Check the output for any formatting changes applied.
    - Optionally re-run with `-VerifyNoChanges` to confirm zero drift.

## 5. Definition of Done

- [ ] `dotnet format` exits with code 0 (no formatting violations remain).
- [ ] `-VerifyNoChanges` passes cleanly.

## 6. Reference Material

- `.editorconfig` (repo root)
- `docs/ai/specs/coding-standard.md`
