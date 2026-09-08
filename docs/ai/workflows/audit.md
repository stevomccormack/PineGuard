<!-- metadata_header
type: workflow
id: workflow-audit
version: 2.0
-->

# Workflow: Audit

> [!NOTE]
> Runs PineGuard's repo audits via `pineguard audit`, the TypeScript CLI in `apps/cli`.
> These audits check convention compliance and cross-layer mapping/parity. See
> `docs/ai/specs/tools/audit-cli/spec.md` (v2) for the full specification — layout, command
> surface, rule catalog, baseline ratchet, and parser approach.

## Context

- **Role**: [DevOps Engineer](../roles/shipper.md)
- **Reference**: `apps/cli/src/commands/audit.ts`; spec: `docs/ai/specs/tools/audit-cli/spec.md`

## `ordering` notes (cross-layer method ordering)

- MustClauses define the canonical concept ordering.
- GuardClauses are frequently named for forbidden states and implemented via Must complements;
  `ordering` (legacy Rule08) compares Guard ordering using the **Must clause each Guard method
  invokes** (not the Guard method name).

## Parameters

- **Rule selection**: zero or more slugs or legacy `RuleNN` ids as positional arguments, e.g.
  `pineguard audit layer-parity test-files` or `pineguard audit Rule50` (legacy ids still
  resolve — see the spec §7). No selection (or the literal token `all`) runs every registered
  rule.
- **`--scope <scope>`**: `library`, `testing`, or `docs` — see the spec §3 for which rules live
  in each scope.
- **`--gate`**: only run merge-blocking rules (`test-files`, `doc-links`, `surface-parity` today
  — spec §8).
- **`--format <format>`**: `pretty` (default), `json`, `github`, or `sarif`.
- **`--changed`**: restrict file discovery to files changed vs `main`/`origin/main`, for fast
  pre-commit feedback.
- **`--update-baseline`** / **`--no-baseline`**: baseline ratchet controls — see spec §4.
- **`--list`**: print the full rule catalog (slug, legacy id, scope, gate, description).

## CI Gate

`.github/workflows/ci.yml`'s audit job runs `pineguard audit --gate --format github` on every
PR. `test-files` (legacy Rule50, Theory-only + `Tests`/`TestData` pairing), `doc-links` (legacy
Rule11), and `surface-parity` (legacy Rule12) are the three merge-blocking rules — a violation
in any of them is a merge blocker. Reproduce the gate locally before pushing:

```sh
pnpm -C apps/cli exec tsx src/index.ts audit --gate
```

> [!NOTE]
> Until `apps/cli/config/baseline.json` is snapshotted at cutover (plan §9.2 P6.1), the gate is
> **expected** to fail on pre-existing debt in `doc-links` (real drift the rebuild newly
> surfaced — see the spec's §3.2 and the plan's §2.4) — this is by design, not a regression
> introduced by your change. Once the baseline lands, a gate failure means your change
> introduced a genuinely new, un-baselined finding.

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run all audit rules (recommended)**

   ```sh
   pnpm -C apps/cli exec tsx src/index.ts audit
   ```

   Show the full accumulated debt, ignoring the baseline ratchet (useful for a debt survey, not
   for deciding mergeability):

   ```sh
   pnpm -C apps/cli exec tsx src/index.ts audit --no-baseline
   ```

   Scope to one area:

   ```sh
   pnpm -C apps/cli exec tsx src/index.ts audit --scope library
   pnpm -C apps/cli exec tsx src/index.ts audit --scope testing
   pnpm -C apps/cli exec tsx src/index.ts audit --scope docs
   ```

2. **Run a single rule (when iterating)**

   By slug (preferred) or legacy id — both resolve to the same rule:

   ```sh
   pnpm -C apps/cli exec tsx src/index.ts audit rules-usage
   pnpm -C apps/cli exec tsx src/index.ts audit nullability
   pnpm -C apps/cli exec tsx src/index.ts audit ordering
   pnpm -C apps/cli exec tsx src/index.ts audit Rule50   # legacy alias for test-files
   ```

3. **Inspect outputs**
   - Pretty output prints directly to the terminal. `--format json` also writes
     `artifacts/audit/<slug>.json` per rule plus `artifacts/audit/summary.json`.
   - Treat any un-baselined finding as blocking; a baselined finding is pre-existing debt (spec
     §4) — a *new* finding in an already-baselined file still surfaces and still blocks, so
     baselined debt never grants a file blanket immunity.

4. **Triage + remediate**
   - Fix the highest-signal violations first: the three gate rules (`test-files`, `doc-links`,
     `surface-parity`), then naming/collisions and missing mappings in the `library` scope.
   - Re-run the specific rule you're iterating on, then re-run the full `pineguard audit` before
     finalizing.
