# Skill: Run Roslyn Compiler Diagnostics
**ID**: pineguard.skill.scan-roslyn
**Version**: 1.0

## 1. Context & Goal
Run the Roslyn compiler diagnostics tool against a specified scope and report all CS warnings.

## 2. Inputs
- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`)

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> - No Docker or external tools required. This uses `dotnet build` directly.
> - Do not attempt to fix any warnings during the run phase. Report only.
> - Output artifacts go to `artifacts/code-diagnostics/<scope>/`.

## 4. Execution Steps

1. **Run the Diagnostics Script**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope [SCOPE]
   ```

2. **Review the Output**

   The script produces:
   - A summary grouped by warning code and by file (stdout)
   - A structured JSON report at `artifacts/code-diagnostics/<scope>/diagnostics.json`

3. **Report to User**

   Summarize:
   - Total warning count
   - Top warning codes (with counts)
   - Top affected files (with counts)

## 5. Definition of Done
- [ ] Build completed without errors
- [ ] Warning summary reported (count by code, count by file)
- [ ] JSON artifact written to `artifacts/code-diagnostics/<scope>/`

## 6. Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| Build errors (not warnings) | Missing dependency or syntax error | Fix build errors first; diagnostics only capture warnings |
| No JSON output | Script ran without `-OutputFormat Json` | Add `-OutputFormat Json` flag to the script call |
| Warnings from test projects | Scope set to `All` | Use specific scope (e.g., `Core`) to focus on production code |
| Zero warnings reported | Project already clean | Confirm scope matches target; try `All` to verify |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Check Core for compiler warnings" | Run diagnostics with `-Scope Core` | Warning summary grouped by code and file |
| "How many CS8600 warnings do we have?" | Run diagnostics with `-Scope All`, filter for CS8600 | Count of nullability warnings across all projects |
| "Run Roslyn on the whole solution" | Run diagnostics with `-Scope All` | Full warning report + JSON artifact |

## 8. Reference Material (Deep Dive)
- `docs/ai/specs/tools/code-diagnostics/spec.md` (warning categories, scopes)
- `tools/code-diagnostics/README.md` (usage, parameters)
