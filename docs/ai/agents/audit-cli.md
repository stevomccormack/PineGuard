<!-- metadata_header
type: agent
id: agent-audit-cli
version: 2.0
-->

# Agent: Run Audit CLI (Library / Testing / Docs / All)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/audit.md` and the specification at
   `docs/ai/specs/tools/audit-cli/spec.md` (v2).

2. Choose the scope and run the matching `pineguard audit` invocation (recommended defaults):
   - **Reproduce the CI gate (`test-files`, `doc-links`, `surface-parity`)** — start here; this
     is the only invocation that blocks a merge (see [CI parity](#ci-parity)).

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --gate
     ```

   - **Audit the library layer** (`rules-usage`, `must-usage`, `layer-parity`, `nullability`,
     `must-collisions`, `ordering`, `must-codes`):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --scope library
     ```

   - **Audit testing conventions** (`test-files`, `test-structure`, `test-records`,
     `test-orphans`, `test-tuples`) — `test-structure`/`test-records`/`test-orphans`/`test-tuples`
     carry real pre-existing debt tracked by the baseline ratchet (spec §4); treat a baselined
     finding as known debt, not something this run introduced.

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --scope testing
     ```

   - **Audit docs/adapter parity** (`doc-links`, `surface-parity`) — both are hard gates (spec
     §8):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --scope docs
     ```

   - **Audit all (full suite)** — useful for a debt survey, not for deciding whether a change is
     mergeable on its own (use `--gate` for that):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit
     ```

3. Optional iteration flags (useful while tightening policy or diagnosing failures):
   - **Show the full debt, ignoring the baseline ratchet**:

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --no-baseline
     ```

   - **Restrict to files changed vs `main`** (fast pre-commit feedback):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --changed
     ```

   - **Emit a JSON summary** (writes `artifacts/audit/<slug>.json` + `artifacts/audit/summary.json`):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --format json
     ```

   - **Run one rule by slug or legacy id** (both resolve to the same rule — spec §7):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit ordering
     pnpm -C apps/cli exec tsx src/index.ts audit Rule08   # legacy alias for `ordering`
     ```

   - **List the full catalog** (slug, legacy id, scope, gate, description):

     ```sh
     pnpm -C apps/cli exec tsx src/index.ts audit --list
     ```

## CI parity

Pull requests are gated on **`test-files`, `doc-links`, and `surface-parity`** only (spec §8),
via `pineguard audit --gate --format github`. Permanent exemptions live in
[`apps/cli/config/exceptions.json`](../../../apps/cli/config/exceptions.json); bulk
pre-existing debt is absorbed by the baseline ratchet in
[`apps/cli/config/baseline.json`](../../../apps/cli/config/baseline.json) (spec §4). Everything
else in the catalog runs on demand and reports but does not block a merge:

| Rules | Merge-blocking | Why |
|-------|----------------|-----|
| `test-files` (Rule50) | Yes | Carried over from the legacy tool with confirmed real-tool parity (`docs/ai/plans/audit-cli-rebuild.md` §9.2 P4.1). |
| `doc-links` (Rule11) | Yes | Promoted to a hard gate in the rebuild. |
| `surface-parity` (Rule12) | Yes | Promoted to a hard gate in the rebuild. |
| The other 11 `library`/`testing` rules | No | Real, tracked pre-existing debt absorbed by the baseline ratchet; the gate widens as each rule's baseline entry shrinks to zero (spec §4), not by editing the CI job. |

So do not report a baselined finding in a non-gate rule as a regression introduced by the change
under audit — only an un-baselined finding, or a failure in one of the three gate rules, blocks.
The CI job is section 7 of [`.github/workflows/ci.yml`](../../../.github/workflows/ci.yml).
