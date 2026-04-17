# Tools & Scripts

> Inherits from: `docs/ai/rules/global.md` (read first)

Before writing or editing PowerShell tooling, also read:
- `docs/ai/specs/tools/spec.md` (normative specification: naming, parameters, output rules)
- `tools/README.md` (operational index: all tool directories, standard parameters, usage)

All script output MUST go to `artifacts/` or `logs/`.
NEVER create temporary files in the project root.
Follow existing parameter patterns (see `tools/code-coverage/` or `tools/sonar-scanner/` for reference).
Each tool directory has a `README.md` — consult it for usage, parameters, and examples.
