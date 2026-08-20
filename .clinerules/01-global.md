# PineGuard Global Rules

This file is an adapter. The canonical Brain lives in `docs/ai/`. Do not add logic here.

Cline reads `.clinerules/*.md` only — it has no per-command prompt-file format, so this surface is
**rules-only** by design and carries no command directory. See `docs/ai/meta/adapter-surfaces.md` §3.

## Before Any Work

Read `docs/ai/rules/global.md` for invariants that apply to all code in this repository. It is
authoritative: beyond the layer and messaging invariants it also covers workflow orchestration,
engineering discipline, and multi-session coordination (`docs/ai/rules/coordination.md`) — read it
before running builds, tests, or coverage.

## Running a Named Workflow

There are no Cline slash commands. To run one of the repository's named workflows, read and execute
the matching playbook in `docs/ai/agents/` directly; `docs/ai/commands/` maps user phrasing to the
right playbook.

## Coding Standards

Read `docs/ai/specs/coding-standard.md` for formatting, naming, and style rules.

## Safety

Read `docs/ai/specs/safety.md` for Tier 0/1/2 command classification.
