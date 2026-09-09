# Scan Rules (SonarQube)

> Inherits from: `docs/ai/rules/global.md` (read first)

Before doing any scan-related work, also read:
- `docs/ai/specs/scan/spec.md` (normative specification: severity model, API endpoints, fix rules)
- `tools/code-scan/sonarqube/README.md` (operational docs: usage, parameters, examples)

## Key Rules

1. **Never hard-code tokens** in scripts or docs. Resolution order: `-ProjectToken` parameter ->
   `$env:SONARQUBE_TOKEN` -> the `SONARQUBE_TOKEN` key in `.etc/powershell/.env` (the same file
   also holds `SONARQUBE_ADMIN_PASSWORD`).
2. **Verify SonarQube is UP** (`/api/system/status`) before querying the API.
3. **Fix issues one file at a time**. Verify `dotnet build PineGuard.slnx --no-incremental` compiles after each fix.
4. **Never suppress warnings** — fix the root cause, don't hide it.
5. **Apply idiomatic C# fixes** following `docs/ai/specs/coding-standard.md`.
6. **All script output** goes to `artifacts/` or `logs/` (never the project root).
