# Qodana (Code Quality)

This repo can be analyzed with JetBrains Qodana locally and in GitHub Actions.

For full usage examples, parameters, and directory structure, see:

- `tools/code-inspection/README.md` (source of truth for operational documentation)

## GitHub Actions Setup

1. Create a repository secret named `QODANA_TOKEN`.
   - Create it in Qodana Cloud (project token) and paste it into GitHub: **Settings > Secrets and variables > Actions**.
2. The workflow is defined in `.github/workflows/code_quality.yml`.

Notes:

- The workflow uploads SARIF results to GitHub Code Scanning.
- The analysis uses `tools/code-inspection/qodana/config/qodana.all.yaml` and builds `tools/code-inspection/qodana/PineGuard.All.Qodana.slnx`.

## Local Installation

### Windows (Scoop)

```powershell
scoop bucket add jetbrains https://github.com/JetBrains/scoop-utils
scoop install qodana
```

Or use: `tools/code-inspection/Install-Scoop.ps1`

### macOS / Linux

```bash
export QODANA_TOKEN="<your token>"
qodana scan
```

## Key Design Decisions

- Each scope uses a dedicated `.slnx` solution file under `tools/code-inspection/qodana/` (keeps analysis focused).
- The wrapper forces non-interactive mode by default (CI-safe).
- Docker is the default runtime; use `-Native` for Docker-free runs.
- Hard timeout (`-TimeoutMinutes 30`) prevents hung Qodana processes.
