---
spec:
  id: pineguard.ai.tools.audit-cli.spec
  title: "PineGuard Audit CLI Specification"
  version: 2
  template:
    - ../../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "apps/cli/**"
---

# PineGuard Audit CLI Specification (`pineguard audit`)

> [!IMPORTANT]
> **v2 — TypeScript rebuild.** This specification now describes `pineguard audit`, the
> TypeScript CLI in `apps/cli` built per `docs/ai/plans/audit-cli-rebuild.md`. It supersedes
> v1, which described the PowerShell/Roslyn tool under `tools/audit-cli/`. That tool still
> exists on disk at the time of writing — its removal is a separate, later step
> (plan §9.2 P6.2, after cutover sign-off) — but it is no longer the audited implementation;
> treat `tools/audit-cli/` as frozen, not maintained. See `apps/cli/README.md` for
> package-level developer notes (build/dev scripts, dependency-pinning rationale) that this
> spec does not repeat.

This specification details the structure, command surface, rule catalog, baseline policy,
parsing approach, and self-testing convention of `pineguard audit` — PineGuard's own machine
enforcement of its cross-layer validation invariants (Must → Guard/Fluent/DataAnnotations
usage and ordering, nullability policy, error-code catalogue integrity, doc-link and
adapter-surface parity, unit-test structural conformance).

## 1. Package and layout

- **Location**: `apps/cli/` — an npm workspace package (`@pineguard/cli`, pnpm workspace root
  at repo root) with bin name `pineguard`.
- **Runtime**: Node >= 22, ESM (`"type": "module"`). No .NET build, no Windows dependency — the
  tool parses C# source directly (§5) instead of reflecting over built assemblies.

```
apps/cli/
    package.json                 name @pineguard/cli, bin "pineguard"
    tsconfig.json
    src/
      index.ts                   commander root; registers `audit` and `banner`
      banner.ts                  renderBanner()/readVersion() — the ASCII banner
      commands/
        audit.ts                 arg parsing -> engine -> reporter -> exit code
        banner.ts                `pineguard banner`
      audit/
        catalog.ts                rule registry: registerRule(), RuleScope, CatalogEntry
        engine.ts                 selection, shared context, exceptions + baseline, runAudit()
        types.ts                  Rule, Finding, RuleContext (types only)
        repo.ts                   git ls-files, repo-root discovery, --changed diffing
        parsing/csharp.ts         web-tree-sitter loader, cached parse, walk-then-filter helpers
        parsing/markdown.ts       remark/mdast link + front-matter extraction
        reporters/
          pretty.ts               default human-readable output
          json.ts                 artifacts/audit/<slug>.json + summary.json
          github.ts               ::error/::warning workflow-command annotations
          sarif.ts                SARIF 2.1.0
        rules/<slug>.ts           one file per rule (14 today — §3), each self-registers
        rules/index.ts            barrel importing every rule module for its registration side effect
    config/
      exceptions.json            keyed by slug: { "<slug>": ["<file-or-message-substring>", ...] }
      baseline.json              ratchet snapshot of accepted pre-existing findings (§4)
    test/
      support/runRule.ts         VIBE harness: expectValid/expectInvalid/runBoundary (§6)
      rules/<slug>.test.ts       vitest, one file per rule
      fixtures/<slug>/{valid,invalid,boundary}/   per-rule fixture trees (§6)
```

`docs/ai/specs/language/vocabulary.json` stays in the Brain (it is vocabulary, not code); the
`layer-parity` rule reads it via `RuleContext.vocabulary`. Audit output is written to
`artifacts/audit/` (`<slug>.json` per rule plus `summary.json`) when `--format json` is used,
keeping the repo's existing file-hygiene convention for generated artifacts.

## 2. Command surface

```
pineguard                            # bare invocation: prints the banner + commander's help listing
pineguard banner                     # print the ASCII banner on its own
pineguard audit                      # all rules, pretty output, exit 1 on any un-baselined finding
pineguard audit <slug|RuleNN> [...]  # one or more rules by slug or legacy id, e.g. `layer-parity test-files`
pineguard audit --scope <scope>      # restrict to a scope: library|testing|docs
pineguard audit --gate               # only run merge-blocking rules (gate: true — what CI runs)
pineguard audit --list               # print slug/legacyId/scope/gate/description for every rule
pineguard audit --format <format>    # pretty (default) | json | github | sarif
pineguard audit --changed            # restrict file discovery to files changed vs main/origin-main
pineguard audit --update-baseline    # accept current findings as the new ratchet floor (§4)
pineguard audit --no-baseline        # show the full debt, ignoring the ratchet (§4)
```

Explicit rule selection (one or more slugs/legacy ids as positional arguments) overrides
`--scope`/`--gate` and runs exactly what was named; with no selection (or the literal token
`all`), every registered rule runs and is then narrowed by `--scope`/`--gate` if given.

**Exit codes**: `0` clean (no un-baselined findings), `1` findings present, `2` usage or config
error (unknown rule/scope/format, or a rule/context failure) — a crashing rule is a `2`, never
an uncaught exception. `--update-baseline` is a maintenance operation, not a pass/fail check: it
always exits `0` unless the baseline file itself could not be written.

**`--format github`** emits one GitHub Actions annotation per finding —
`::error file=<file>[,line=<n>],title=<rule>::<message>` for a `gate: true` rule's findings,
`::warning ...` for every other rule — so a PR run's Checks tab and diff view show findings
inline. **`--format sarif`** emits a SARIF 2.1.0 log with the same error/warning split via each
rule's `defaultConfiguration.level`.

Dev-mode invocation (no build step): `pnpm -C apps/cli exec tsx src/index.ts audit ...`. The
built form (`pnpm -C apps/cli build` once, then run `node apps/cli/dist/index.js audit ...`) is
what the `pineguard` bin points at once the package is linked; see `apps/cli/README.md` for the
bin-wiring rationale. This spec and every Brain doc that invokes the tool (`docs/ai/workflows/
audit.md`, `docs/ai/agents/audit-cli.md`) use the `tsx`-against-source form, matching the
package's own documented dev-mode path.

## 3. Rule catalog

14 rules today, grouped by scope. `legacyId` is the old PowerShell/Roslyn tool's numeric id,
kept as a resolvable alias (§7) — slugs are the primary, current name; use them in new
Brain/doc references. `gate: true` marks a merge-blocking rule (`pineguard audit --gate`
selects exactly these three). Descriptions below are copied verbatim from each rule's
`registerRule({ ..., description: "..." })` call and match `pineguard audit --list` output
exactly.

### 3.1 `library` scope

| Slug | Legacy id | Gate | Description |
|---|---|---|---|
| `rules-usage` | Rule02 | false | Every public static method on a Core Rules class (`src/PineGuard.Core/Rules/*.cs`) has at least one call site in `src/PineGuard.MustClauses/*.cs`. |
| `must-usage` | Rule03 (supersedes Rule03/04/05) | false | Every public Must clause (`src/PineGuard.MustClauses`) is invoked from Guard, Fluent, and DataAnnotations. Supersedes legacy Rule03 (Guard), Rule04 (Fluent), and Rule05 (DataAnnotations) — see this module's header comment. |
| `layer-parity` | Rule06 | false | Every normalised validation concept implemented by Must, Guard, Fluent, or DataAnnotations is implemented by all of them (concept-set parity via `vocabulary.json`; no build, no `dotnet publish`). |
| `nullability` | Rule07 | false | Every Must/Guard clause's primary parameter follows the hybrid nullability policy: reference types are declared nullable, value types are not (a generic parameter is judged by its own `where` constraint). |
| `must-collisions` | Rule01 | false | Flags public static Must clause overloads invocable with a single argument whose first parameter would all accept a bare `null` literal — an ambiguous or unpredictable `Must.Be.Xxx(null)` call site. |
| `ordering` | Rule08 | false | Must/Guard/FluentValidation/DataAnnotations/Rules declare the same validation concepts in the same relative order. |
| `must-codes` | Rule13 | false | Must error-code catalogue integrity: every clause resolves to a domain, every constant is used, DataAnnotations codes match their dispatch, Guard passes `IMustResult` not a string — domain map derived from `Codes/*.cs` `Serves:` comments, never hardcoded. |

### 3.2 `docs` scope

| Slug | Legacy id | Gate | Description |
|---|---|---|---|
| `doc-links` | Rule11 | **true** | Every backticked path, markdown link, and `.vscode/tasks.json` script path in tracked docs resolves to a real file. |
| `surface-parity` | Rule12 | **true** | Every Brain agent is represented consistently across each full adapter surface and palette declared in `docs/ai/meta/adapter-surfaces.md`, honouring its declared parity exceptions (agent-specific exemptions and the Copilot one-representative-per-family subset policy). |

### 3.3 `testing` scope

| Slug | Legacy id | Gate | Description |
|---|---|---|---|
| `test-files` | Rule50 | **true** | Every `*Tests.cs` has a paired `*TestData.cs` (and vice versa); `*Tests.cs` files use `[Theory]` only, never `[Fact]`. |
| `test-structure` | Rule51 | false | TestData/Tests structural conformance to v11 §4-5: sealed, flat Tests class with instance test methods, one `<Group>_BehavesAsExpected` per Operation Group in order, no orphans, `ValidCases`→`EdgeCases`→`InvalidCases` dataset ordering with no empty scaffolding. |
| `test-records` | Rule52 | false | Every layer's `*Case` test-data records follow that layer's own base-record convention (root spec §4.2 + the per-layer addenda) instead of one blanket repo-wide rule. |
| `test-orphans` | Rule53 | false | Every `*Tests.cs` file's subject resolves to a class/record/struct/interface/enum declaration — or to a source file of that name — in its package's source project or a project it references. |
| `test-tuples` | Rule54 | false | Every input tuple in a `*TestData.cs` or fixture file — a `*Case`/`*Scenario` generic's first type argument, a case record's `Value` property, or a fixture tuple field — has camelCase element names that, where the paired source method is resolvable, match its exact parameter names. |

Rules dropped in the rebuild (not ported, per plan §4.3 and §5 D4/D9): the legacy tool's Rule09
(catalog self-consistency — now a load-time invariant enforced by `catalog.ts`'s own
`registerRule` collision check, plus `apps/cli/test/audit/catalog.test.ts`) and Rule10 (PowerShell
param/help normalisation — dropped from the audit entirely per decision D4; PowerShell hygiene
belongs to PSScriptAnalyzer, or disappears with the PowerShell→bash migration).

## 4. Baseline ratchet (`apps/cli/config/baseline.json`)

The audit gate widens without a big-bang debt clean-up via a baseline ratchet, not a per-rule
on/off switch:

- `apps/cli/config/baseline.json` shape: `{ "<slug>": ["<key1>", "<key2>", ...] }`, keyed by rule
  slug. A clean rule has **no entry at all** — not an empty array — because an absent entry is
  what lets a rule become a hard gate once its debt reaches zero.
- Every `Finding` carries a stable `key` (convention: `` `${rule}:${file}:${normalisedMessage}` ``
  — see `apps/cli/src/audit/types.ts`'s doc comment). Two runs against unchanged input must produce
  byte-identical keys; two different violations, even in the same file for the same rule, must
  produce different keys.
- `pineguard audit --update-baseline` runs every registered rule (ignoring any rule
  selection/`--scope`/`--gate` also passed — a partial snapshot would silently drift the ratchet
  floor for the rules not selected), applies exceptions (not the ratchet), and overwrites
  `apps/cli/config/baseline.json` with the resulting finding keys, grouped by slug.
- A normal `pineguard audit` run applies exceptions, then the baseline: a finding whose key is
  already in that rule's baseline entry is suppressed; anything else counts toward the exit code
  and, under `--gate`, blocks CI.
- `pineguard audit --no-baseline` skips the ratchet entirely and reports the full accumulated
  debt for every selected rule — useful for a debt survey, never for deciding mergeability.
- **Anti-regression property**: because suppression is by exact key (rule + file + normalised
  message), not a blanket per-file allowance, a *new* finding in a file that already has
  baselined debt still surfaces. Baselining a file's existing findings never grants that file
  immunity from new violations.
- Debt is burned down separately from this cascade: when a rule's baseline entry count reaches
  zero, delete the entry (or run `--update-baseline` again once the debt is gone) and the rule
  becomes, in effect, a hard gate the moment `--gate` is widened to include it.

`apps/cli/config/exceptions.json` is a distinct, permanent mechanism from the baseline: `{ "<slug>":
["<file-or-message-substring>", ...] }`, a substring match against `finding.file` or
`finding.message`, applied by the engine before the baseline. Use it for a small number of
permanent, reviewed exemptions (the two live legacy `Rule50` allowlist entries carried forward
from `tools/audit-cli/test-audit-exceptions.json` are the working example); use the baseline for
bulk pre-existing debt you intend to burn down over time.

## 5. Parser approach

C# source is parsed with `web-tree-sitter` (WASM — no native build, runs unmodified on every CI
runner) and the official `tree-sitter-c-sharp` grammar package, which ships a prebuilt
`tree-sitter-c_sharp.wasm` directly in its npm tarball (no separate wasm-only package or vendoring
step needed; confirmed in the P1.2 spike, plan §4.5).

Rules query the parsed tree with **walk-then-filter** helpers
(`Node#descendantsOfType`, `Node#childForFieldName`, `Node#children` — `apps/cli/src/audit/parsing/csharp.ts`), not tree-sitter's `Query` s-expression engine. Every shape the rules need (method
and record declarations, tuple types, attributes, invocation member-access chains, generic
constraint clauses) is expressible as "find nodes of type X, then read a couple of named
fields," and the walk-then-filter style fails loudly (empty results, a compile-time type error
on a wrong node-type string) rather than an opaque query-string parse error at runtime. A rule
with a genuinely query-shaped need (multiple alternative patterns, predicates) may still use
`Query` directly against a `ParsedFile`'s `tree` — the module does not hide the underlying
tree-sitter API.

Markdown (docs, front matter) is parsed with `remark`/`remark-frontmatter` (`apps/cli/src/audit/parsing/markdown.ts`), walking the resulting mdast tree for `link` and `inlineCode` nodes; front matter
is captured by a small regex around the leading `---` block and parsed with the `yaml` package.
Tracked-file discovery uses plain `git ls-files` (`apps/cli/src/audit/repo.ts`), which is immune to
nested worktree checkouts inflating counts — the exact class of bug that made the legacy tool's
Rule10 miscount when run from inside `.claude/worktrees/`.

No rule shells out to `dotnet`, MSBuild, or a compiled assembly. Every check the audit performs
is a source-shape check (names, order, attributes, file pairing, invocation targets by name, doc
links, error-code constants) resolvable from syntax; none needs a compiler or a semantic model.

## 6. Testing the tool itself — the VIBE fixture convention

Every rule's own correctness is proven by fixtures, not by reading the rule's code — a
reviewer reading the legacy tool's source is exactly how four rules (Rule03/04/05/07) went
silently vacuous for months (plan §2.3). `apps/cli`'s own test fixtures follow **VIBE — Valid /
Invalid / Boundary-Edge**, the same three-way split already normative for the rest of the
repo's C# tests (`docs/ai/specs/testing/unit-test.md` v11 §4.1/§4.4: `ValidCases` →
`EdgeCases` → `InvalidCases`), applied to this CLI's own test data:

```
apps/cli/test/fixtures/<slug>/
  valid/      a minimal, realistic source tree the rule must NOT flag — always [] findings.
  invalid/    a minimal, realistic source tree the rule MUST flag — always >=1 finding. This is
              the mandatory "can it fail" fixture: the exact gap that let Rules 03/04/05/07 pass
              vacuously for months.
  boundary/   optional — only when the rule has a real edge to probe (an empty collection vs.
              one item, exactly a threshold value, a partial-class split, a moved/renamed type).
              Boundary fixtures carry no blanket valid/invalid expectation; each is asserted
              individually, case by case, in the rule's own test file.
```

`valid/` and `invalid/` are **mandatory** for every rule — non-negotiable. `boundary/` is added
only when meaningful; per the root spec's own "no empty dataset" policy (§10.3), an empty
placeholder `boundary/` is never scaffolded for a rule with no meaningful edge condition.

The harness lives in `apps/cli/test/support/runRule.ts` and exposes three helpers over that
shape, plus the generic runner they're built on:

- `expectValid(rule, slug): Promise<void>` — runs against `valid/`; throws if the rule produces
  any finding at all.
- `expectInvalid(rule, slug): Promise<Finding[]>` — runs against `invalid/`; throws a
  descriptive error (naming Rules 03/04/05/07 as the historical precedent) if the result comes
  back empty. Mandatory for every rule's invalid-fixture test — a bare
  `expect(findings.length).toBeGreaterThan(0)` is a weaker substitute and must not be used in its
  place.
- `runBoundary(rule, slug): Promise<Finding[]>` — runs against `boundary/` and returns the raw
  findings for the calling test to assert against explicitly; no built-in pass/fail assumption.
- `runRuleOnFixture(rule, fixtureDir): Promise<Finding[]>` — the generic, lower-level runner the
  three helpers above are built on, for an ad-hoc fixture path.

Every one of the 14 rules in §3 has a passing `apps/cli/test/rules/<slug>.test.ts` backed by its own
`valid/`/`invalid/` fixtures (`boundary/` where meaningful). Plan §9.2 P4.2 additionally
mutation-tested every rule (forcing each `run()` to `return [];` as its first statement — the
exact legacy-bug shape) and confirmed all 14 fail their own suite under that mutation, proving
no rule's fixture assertion is vacuous. See `apps/cli/test/README.md` for the full convention
reference and worked example.

## 7. Legacy rule ids (backwards compatibility)

Numeric `RuleNN` ids from the PowerShell/Roslyn tool remain resolvable as aliases —
`pineguard audit Rule50` and `pineguard audit test-files` select the same rule — because
roughly thirty Brain files still cite them at the time of this rewrite (plan §9.2 P5.5 sweeps
those to slugs separately). `CatalogEntry.legacyId` holds exactly one id per registered slug;
where three legacy rules collapsed into one (Rule03/04/05 → `must-usage`) or one legacy rule
split into two (Rule01 → `must-collisions` + half of `nullability`), the mapping is documented
in that rule module's header comment and only the primary id is registered as the resolvable
alias. Slugs are the primary name going forward — use them in new references; the numeric
aliases are expected to be dropped in a later release once the Brain-wide sweep is complete.

## 8. Enforcement (CI gate)

Three rules are hard, merge-blocking gates today — `pineguard audit --gate` selects exactly
these three:

- `test-files` (Rule50) — the CI gate carried over unchanged from the legacy tool, now verified
  with real-tool parity (plan §9.2 P4.1 confirmed an exact match to the legacy tool's live
  behaviour on the same commit: both report the same two pre-existing allowlisted findings,
  both are clean after exceptions are applied).
- `doc-links` (Rule11) — promoted to a hard gate in the rebuild.
- `surface-parity` (Rule12) — promoted to a hard gate in the rebuild.

Every other rule in the catalog (§3.1's seven `library`-scope rules, plus `test-structure`,
`test-records`, `test-orphans`, `test-tuples`) runs and reports but does not block a merge; its
pre-existing findings are absorbed by the baseline ratchet (§4) and burned down over time,
widening the gate set without a single big-bang clean-up. `.github/workflows/ci.yml`'s audit
job is the authoritative CI invocation — see `docs/ai/workflows/audit.md` for the reproduction
command and `docs/ai/agents/audit-cli.md` for the agent playbook.

## 9. PowerShell normalization note (legacy tool only)

`tools/audit-cli/`'s own PowerShell scripts (not `apps/cli`) remain, until deleted (plan P6.2),
subject to `docs/ai/specs/tools/spec.md`'s parse-safety rules. This does not apply to
`apps/cli`, which has no PowerShell.
