# @pineguard/cli

TypeScript rebuild of `tools/audit-cli` as `pineguard audit`. See
[`docs/ai/plans/audit-cli-rebuild.md`](../../docs/ai/plans/audit-cli-rebuild.md) for the full
plan; this package is the P1.1 scaffold only — **no rules are implemented yet**.

## Status (P1.1 scaffold)

- `src/index.ts` registers a commander root with one subcommand: `audit`.
- `src/commands/audit.ts` is a stub: `--list` prints an empty catalog table; every other flag
  from plan §4.2 is parsed but no-ops with a `// TODO(P1.5/P3)` marker.
- `src/audit/{parsing,reporters,rules}/` are empty placeholders (`.gitkeep`) for P1.2–P1.5 and P2.
- `config/exceptions.json` is `{}` — real content is ported from
  `tools/audit-cli/test-audit-exceptions.json` in a later phase, not here.

## Running it

Two ways, both work without a build:

```sh
# dev mode — runs the TypeScript source directly via tsx (fastest iteration loop)
pnpm -C apps/cli exec tsx src/index.ts audit --list

# via the package script
pnpm -C apps/cli dev audit --list
```

Or build once and run the compiled bin (this is what `bin.pineguard` in `package.json` points at):

```sh
pnpm -C apps/cli build
node apps/cli/dist/index.js audit --list
```

### Bin-wiring decision

`package.json` declares `"bin": { "pineguard": "./dist/index.js" }` — i.e. the published/installed
bin is the **built** output, not the TypeScript source. Rationale: a `bin` entry has to be a
runnable script the moment the package is linked (e.g. via `pnpm link` or a future npm publish),
and shipping a pre-built ESM file avoids requiring `tsx`/`ts-node` at runtime for end users. `tsup`
does the build (`pnpm -C apps/cli build` → `dist/index.js`), preserving the `#!/usr/bin/env node`
shebang from `src/index.ts` and marking the output executable. For day-to-day development
(including every later phase's agents), prefer `tsx` against the source as shown above — no build
step, no stale `dist/` to forget to rebuild.

## Parser dependency decision (tree-sitter-c-sharp)

Plan §4.5 calls for `web-tree-sitter` (WASM) + the `tree-sitter-c-sharp` grammar, with a spike
(P1.2) to prove it loads and queries real files. As part of this scaffold we researched whether a
prebuilt wasm ships on npm, so P1.2 doesn't have to build/vendor one:

- **`tree-sitter-c-sharp@0.23.5`** (the official grammar package, maintained by the tree-sitter
  org) publishes `tree-sitter-c_sharp.wasm` directly in the npm tarball (confirmed via
  `npm pack` + tarball inspection) — no separate wasm-only package is needed.
- Smoke-tested in isolation: `web-tree-sitter@0.27.0` loads that wasm via `Language.load(...)` and
  parses a trivial C# snippet into a `compilation_unit` root node without error.
- Both packages are pinned as exact versions in `apps/cli/package.json` (`web-tree-sitter` under
  `dependencies`, `tree-sitter-c-sharp` under `dependencies`). P1.2 should not need to change
  either version to get the spike working — it starts from a known-good pair.
- Caveat for P1.2: the npm package also ships native prebuilds (`prebuilds/<platform>/*.node`)
  behind a `node-gyp-build` install script, for the non-WASM `tree-sitter` native binding. We only
  need the `.wasm` file for `web-tree-sitter`; the native install step is harmless (prebuilds cover
  all six major OS/arch triples so it doesn't attempt a native compile) but worth knowing about if
  install time or CI reproducibility ever matters.

## TypeScript version note

`typescript` is pinned to **6.0.3**, not the newer `7.0.2` that is otherwise `latest` on npm right
now. `typescript-eslint@8.69.0`'s peer range is `>=4.8.4 <6.1.0`, which TS 7 falls outside of; 6.0.3
is the newest release still inside that range. Revisit this pin once `typescript-eslint` supports
TS 7.

## Scripts

| Script       | Purpose                                        |
| ------------ | ---------------------------------------------- |
| `build`      | `tsup` → `dist/index.js` (ESM, Node 22 target) |
| `dev`        | `tsx src/index.ts` — run source directly       |
| `typecheck`  | `tsc --noEmit`                                 |
| `test`       | `vitest run`                                   |
| `test:watch` | `vitest` (watch mode)                          |
| `lint`       | `eslint .`                                     |
| `format`     | `prettier --write .`                           |
