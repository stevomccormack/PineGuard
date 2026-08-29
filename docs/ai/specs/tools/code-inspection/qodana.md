---
spec:
  id: pineguard.ai.tools.code-inspection.qodana
  title: "Qodana (Code Quality)"
  version: 1
  template:
    - ../../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tools/code-inspection/**"
---

# Qodana (Code Quality)

This repo can be analyzed with JetBrains Qodana locally and in GitHub Actions.

For full usage examples, parameters, and directory structure, see:

- `tools/code-inspection/README.md` (source of truth for operational documentation)

## GitHub Actions Setup

1. Create a repository secret named `QODANA_TOKEN`.
   - Create it in Qodana Cloud (project token) and paste it into GitHub: **Settings > Secrets and variables > Actions**.
2. Qodana runs as the `qodana` job in `.github/workflows/ci.yml`. It is opt-in: set the repository variable `QODANA_ENABLED=true`. The job is gated on a successful `build` job.

Notes:

- The job uses `JetBrains/qodana-action@v2026.2` with `upload-result: true` — results are attached as a workflow artifact and sent to Qodana Cloud. There is no SARIF upload to GitHub Code Scanning.
- The analysis uses `tools/code-inspection/qodana/config/qodana.all.yaml` and builds `tools/code-inspection/qodana/PineGuard.All.Qodana.slnx`.

## Local Installation

### Windows

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Initialize-Qodana.ps1
```

Installs the Qodana CLI via Winget if missing, then starts the Qodana Docker Compose stack (requires Docker Desktop running). Do not use it in CI.

Manual alternative (Scoop):

```powershell
scoop bucket add jetbrains https://github.com/JetBrains/scoop-utils
scoop install qodana
```

### macOS / Linux

```bash
export QODANA_TOKEN="<your token>"
qodana scan
```

## Key Design Decisions

- Each scope uses a dedicated `.slnx` solution file under `tools/code-inspection/qodana/` (keeps analysis focused).
- The wrapper forces non-interactive mode by default (CI-safe).
- Docker is the default runtime.
- Hard timeout (`-TimeoutMinutes 30`) prevents hung Qodana processes.
