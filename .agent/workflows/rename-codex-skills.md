---
description: Rename Codex Skills
---

1. Read and execute `docs/ai/agents/rename-codex-skills.md`. Argument: `--remove-prefix "<prefix>"`, default `source-command-`.
2. Do not auto-approve (`// turbo-all` is intentionally omitted) — the script commits, and it deletes the gitignored `source-command-*` folders it promotes (`docs/ai/specs/safety.md` §2.3).
