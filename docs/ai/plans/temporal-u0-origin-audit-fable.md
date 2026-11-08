<!-- metadata_header
type: receipt
id: temporal-u0-origin-audit-fable
version: 1.0
status: binding-ruling
last_updated: 2026-09-27
-->

# T1-U0 — Fable DOC-DEBT-01 origin/main audit ruling (public mirror)

**Reviewer:** Claude Fable 5.1, resumed persisted session `04825da4-0e28-4721-b665-2adbc91d7796`.
**Date:** 2026-09-27. Read-only review; verified init reports tools `Glob`, `Grep`, `Read` only and
an explicitly empty MCP server list.
**Authoritative source:** `artifacts/reviews/t1-u0-standalone-commit-validation/fable-doc-debt-01-ruling.json`
(SHA-256 `A8944A803041B640FA0AEDF2C7F0710E9A3D0464A1B87FCE1A431F9893CD820A`). This document is a
public-only mirror of that JSON ruling; it changes no source, test, or historical receipt file.

## What this ruling is about

The `origin/main` audit failure U0 inherits is **base-branch documentation debt, not a U0 overlay
regression**. Fable's validation base is `origin/main` at `84dff7e195a114b031f7de13783f81513b9e4e49`.
Against that base, the canonical shared `audit --gate` exits **1** with **34 findings**, all under
Rule11 `doc-links`, and **0 of them attributable to U0's own files**. This is the actual, current
state of the base branch — it is not "unrelated/pre-existing and nonblocking"; it is a real red gate
on `origin/main` that must be fixed on `origin/main` before it can be inherited into any temporal
worktree merge.

## DOC-DEBT-01 scope

- **Ruling ID:** `DOC-DEBT-01`.
- **Owner:** the technical-readiness plan initiative (separate from the temporal program).
- **Worktree:** `.claude/worktrees/readiness-doc-links`.
- **Branch:** `feature/readiness-doc-links`.
- **Worktree base:** `origin/main` at `84dff7e`.
- **Exact file scope:** 31 files — the 30 `docs/ai/plans/technical-readiness-to-1.0/**` files listed
  in the ruling JSON's `ownedPlanFiles`, plus the one owned memory file
  `.claude/agent-memory/test-writer/must-validator-layer-test-pattern.md`.

## Exact edit categories

- **Planned-path fix:** use a visible `+ ` marker in existing backticked planned-output paths; wrap
  the five bare readiness paths in backticks with the same marker. Do not create stub reports.
- **Other corrections:**
  - Fence the indented scratch examples and state they are not repository files.
  - Replace prose path-like fragments with ordinary prose/comma lists.
  - Describe the external Zod package path as upstream repository context, without a checkable path
    token.
  - Correct the Pester script reference to `tools/testing/Test-Tools.ps1`.
  - Correct the stale memory reference to `src/PineGuard.Core/MustClauses/MustValidatorCast.cs`.

No parser or config change, no new tests (docs-only), and no baseline exception or suppression of
any kind.

## Expected verification and audit result

```text
git diff --stat            # exactly 31 files
git diff --check
pnpm -C apps/cli install --frozen-lockfile
pnpm -C apps/cli exec tsx src/index.ts audit --gate
pnpm -C apps/cli exec tsx src/index.ts audit --gate --format json
```

Expected: exit 0; the three selected gate rules (Rule11 `doc-links`, Rule12 `surface-parity`, Rule50
`test-files`) each report zero findings; total zero.

## U0 status consequences

- **Unit acceptance:** `ACCEPTED` — unchanged. U0's own provider/coverage implementation findings
  are not affected by this ruling.
- **Local commit readiness:** `READY after closure receipt amendment` — i.e., only after
  `docs/ai/plans/temporal-u0-fable-closure.md` carries the corrected, dated addendum this ruling
  requires (see below); this document plus that addendum constitute the amendment.
- **Phase merge readiness:** `BLOCKED` until (a) DOC-DEBT-01 is merged to `main`, (b) the corrected
  `origin/main` is merged into the temporal worktree, and (c) the post-merge `audit --gate` exits 0.
- **Pre-commit evidence:** the existing U0 receipts, plus this closure record stating the
  `origin/main` audit exit 1 / 34 findings / 0 attributable to U0 / DOC-DEBT-01 assigned.
- **Coverage rerun:** only required if the later `origin/main` merge touches `tests/` or `tools/`.
- **No baseline exception or suppression** is authorized by this ruling.

## Phase actions (from the ruling)

1. Prepare DOC-DEBT-01 in its `origin/main`-based worktree; verify the exact 31-file diff and an
   audit exit 0.
2. Obtain owner confirmation only for the concrete merge/push to `main` — no code or baseline
   approval gate is implied.
3. Merge the corrected `origin/main` into the temporal worktree and require a post-merge audit
   exit 0.
4. Rerun Testing coverage on both TFMs only if that merge touched `tests/` or `tools/`.

The merge block remains in force until all of the above complete.
