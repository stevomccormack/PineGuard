/**
 * Barrel of every registered audit rule.
 *
 * Each rule module (`./<slug>.ts`) calls `registerRule(...)` for its side
 * effect at import time — see `../catalog.ts`'s header comment for the full
 * "adding a new rule" recipe. This file is the one place that side effect is
 * actually triggered for a real `pineguard audit` run: `src/commands/audit.ts`
 * imports this barrel once, which in turn imports every rule module below.
 *
 * Add one `import "./<slug>.js";` line per rule as plan P2 lands it. A rule
 * file that exists on disk but is missing its line here never registers —
 * `pineguard audit --list` simply won't show it.
 *
 * Empty on purpose until plan P2 lands the first rule
 * (docs/ai/plans/audit-cli-rebuild.md §9.2, P2.1-P2.14).
 */
export {};
