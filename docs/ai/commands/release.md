<!-- metadata_header
type: command
id: cmd-release
version: 1.0
-->

# Command: Release

Publishes releases, mutates branch protection, and unlists packages.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/github-release-publish` | Publish a GitHub release for the current version | `docs/ai/agents/github-release-publish.md` |
| `/github-ruleset-enable` | Re-enable the branch-protection ruleset | `docs/ai/agents/github-ruleset-enable.md` |
| `/github-ruleset-disable` | Temporarily disable the branch-protection ruleset | `docs/ai/agents/github-ruleset-disable.md` |
| `/nuget-unlist` | Unlist a published package version from nuget.org | `docs/ai/agents/nuget-unlist.md` |

## Surface Policy

This family is exposed on **Claude Code only**. Every command here is a Tier 0/1 irreversible
operation under [`../specs/safety.md`](../specs/safety.md), so it must not reach a surface that
applies blanket auto-approval. The exception is recorded in
[`../meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §4 — a missing adapter file for these
four agents is a decision, not parity debt.

None of these commands is auto-approved. Each one requires explicit user confirmation of the exact
tag, ruleset, or package version being acted on.
