# @pineguard/cli

TypeScript rebuild of `tools/audit-cli` as `pineguard audit`. See
[`docs/ai/plans/audit-cli-rebuild.md`](../../docs/ai/plans/audit-cli-rebuild.md) for the full
plan and [`docs/ai/specs/tools/audit-cli/spec.md`](../../docs/ai/specs/tools/audit-cli/spec.md)
for the command surface and rule catalog.

## Status

- `src/index.ts` registers a commander root with two subcommands: `audit` and `banner`.
- `src/commands/audit.ts` runs arg parsing -> engine -> reporter -> exit code, and implements every
  flag from plan §4.2.
- `src/audit/parsing/`, `src/audit/reporters/` and `src/audit/rules/` hold the C# and Markdown
  parsers, the `pretty`, `json`, `github` and `sarif` reporters, and the 14 rules, one file per
  rule.
- `config/exceptions.json` was populated at P6.2 (cutover cleanup) with entries ported from legacy
  `tools/audit-cli/test-audit-exceptions.json`; it also carries `doc-links` entries. Its single
  top-level `description` field records why the entries exist.

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
shebang from `src/index.ts` and marking the output executable. For day-to-day development, prefer
`tsx` against the source as shown above — no build step, no stale `dist/` to forget to rebuild.

## Parser dependency decision (tree-sitter-c-sharp)

Plan §4.5 chose `web-tree-sitter` (WASM) + the `tree-sitter-c-sharp` grammar, and that is what the
CLI parses C# with: `src/audit/parsing/csharp.ts` calls `Parser.init()`, loads the grammar's wasm
with `Language.load(...)` and caches the result across calls. `test/audit/parsing/csharp.test.ts`
covers the loader.

- **`tree-sitter-c-sharp@0.23.5`** (the official grammar package, maintained by the tree-sitter
  org) publishes `tree-sitter-c_sharp.wasm` directly in the npm tarball, so no wasm is built or
  vendored here and no separate wasm-only package is needed.
- Both packages are pinned as exact versions under `dependencies` in `apps/cli/package.json`:
  `web-tree-sitter` at `0.27.0` and `tree-sitter-c-sharp` at `0.23.5`.
- The grammar package also ships native prebuilds (`prebuilds/<platform>/*.node`) behind a
  `node-gyp-build` install script, for the non-WASM `tree-sitter` native binding. The CLI needs only
  the `.wasm` file, so the root `pnpm-workspace.yaml` leaves that install script disabled
  (`allowBuilds`, `tree-sitter-c-sharp: false`).

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
