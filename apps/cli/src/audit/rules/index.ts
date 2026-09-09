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
 */

/* Side-effect imports: each rule registers itself into the catalog at module load. */
import "./doc-links.js";
import "./layer-parity.js";
import "./must-codes.js";
import "./must-collisions.js";
import "./must-usage.js";
import "./nullability.js";
import "./ordering.js";
import "./rules-usage.js";
import "./surface-parity.js";
import "./test-files.js";
import "./test-orphans.js";
import "./test-records.js";
import "./test-structure.js";
import "./test-tuples.js";
