<!-- metadata_header
type: plan
id: audit-cli-rebuild
version: 1.0
status: proposed
last_updated: 2026-09-06
-->

# Plan — Audit CLI: Review, Verdict and Rebuild as `pineguard audit` (TypeScript, `apps/cli`)

> **Status**: Proposed — awaiting owner sign-off on the §5 decision gates | **Author**: Fable review pass, 2026-09-06
>
> **Audience**: the owner (decisions in §5) and the Sonnet 5 orchestrator that will execute §9.
> The orchestrator coordinates only; every edit happens inside a dispatched sub-agent (same
> discipline as [new-surfaces-orchestration.md](new-surfaces-orchestration.md) §4).
>
> **Path convention**: files this plan *creates* are written with a leading `+ ` inside the code
> span (`+ apps/cli/package.json`) so the doc-links audit does not report them as broken references.

## 1. Executive summary

The audit CLI is **not stale in the commit sense** (last touched 2026-08-31, Rule13 extended
through August) but it is **stale in the trust sense**: a full run today takes 77 s and 9 of 18
rules fail, and more importantly **five rules cannot fail at all** and one enforces a spec that
was superseded. The cross-layer parity checks that are the whole reason the tool exists
(Must → Guard / Fluent / DataAnnotations usage, nullability policy, adapter parity) have been
printing PASS unconditionally since they were written.

**Verdict.** Keep the *intent* (it is the only machine enforcement of the repo's defining
invariants; nothing in `src/PineGuard.Analyzers` or `tests/` covers it). Do not keep the
*implementation*. Rebuild it as a TypeScript CLI in `+ apps/cli` with a real C# parser
(tree-sitter), its own test suite, and a baseline ratchet so the CI gate can widen from one
rule to all of them without a big-bang debt clean-up. Delete `tools/audit-cli` on cutover.

The CLI becomes the first resident of the `apps/` layout the owner now uses elsewhere
(`apps/cli` today, `apps/web` for the marketing site later) and the natural front door for
consolidating the 88 PowerShell scripts under `tools/` over time — but that consolidation is
explicitly out of scope here (§11).

## 2. What exists today (review findings)

### 2.1 Shape

| Component | Files | LOC | Notes |
|---|---|---|---|
| PowerShell orchestrators (`Run-All.ps1`, library/testing/legacy shims) | 4 | 275 | Three near-identical entrypoints plus a "legacy compatibility shim" |
| Rule wrappers `tools/audit-cli/rules/` | 19 | ~2,100 | Catalog + 18 thin-to-thick wrappers |
| Helpers `tools/audit-cli/helpers/` | 14 | ~2,700 | Where most logic lives; regex over C# source, reflection over built DLLs |
| .NET console app `tools/audit-cli/solution/` | 2 + csproj | 1,411 | Roslyn; ~35–40 % CLI/DTO plumbing; last touched 2026-07-08 |
| Utils, exceptions JSON, READMEs | 6 | ~450 | |
| **Total** | **46** | **~7,760** | Three languages (PowerShell, C#, JSON), Windows CI runner |

Consumers: `.github/workflows/ci.yml` job 7 (Rule50 only, `windows-latest`), 21 tasks in
`.vscode/tasks.json`, the PR template, `docs/ai/agents/audit-cli.md`, `docs/ai/workflows/audit.md`,
`docs/ai/specs/tools/audit-cli/spec.md`, and ~30 Brain files that cite rule ids
(Rule06/07/08/11/13/50 are named as clean-targets in the New Surfaces plans).

### 2.2 Live run, 2026-09-06 (`Run-All.ps1 -ContinueOnError -ShowFailures -AllowViolations`)

| Rule | Purpose | Result today | Trustworthy? | Root cause |
|---|---|---|---|---|
| Rule01 | Must naming / nullability / overload collisions (Roslyn, MSBuildWorkspace) | **crash** (16 s) | No | Requires `artifacts/audit/naming-spec.json`, never shipped, never auto-created; `Program.cs:48` has no existence guard. Documented as "fails on every fresh checkout". |
| Rule02 | Core Rules → Must usage | PASS (39/39) | Yes | Reflection over built `bin/Release/net8.0` DLL + source regex; hardcoded TFM path |
| Rule03 | Must → Guard usage | PASS, "Total Must Methods Found: **0**" | **No — cannot fail** | `'^PineGuard\\.MustClauses'` in a single-quoted string: the doubled backslash makes the regex require a literal `\`, so the type filter matches zero types and "Unused: 0" prints unconditionally |
| Rule04 | Must → Fluent usage | PASS, total 0 | **No — cannot fail** | Same line, byte-for-byte |
| Rule05 | Must → DataAnnotations usage | PASS, total 0 | **No — cannot fail** | Same line, byte-for-byte |
| Rule06 | Adapters ↔ Must concept parity | **crash** | No | `dotnet publish` without `-f` on a multi-targeted project → NETSDK1129; broken since multi-targeting landed 2026-04-17 |
| Rule07 | Hybrid nullability policy | PASS, 0 violations | **No — cannot fail** | Hand-rolled signature parser uses `\\w`, `\\s`, `\\.` in single-quoted strings; no real parameter ever parses, so every method is skipped |
| Rule08 | Cross-layer method ordering (Roslyn syntax only) | PASS by flag; 4 violations, 46 warnings | Yes (noisy) | Works; "which Must clause does this Guard call" is identifier string-matching, first match wins; missing whole layers are only warnings |
| Rule09 | Catalog self-consistency | PASS | Yes | Audits the auditor; disappears in a rewrite (a unit test) |
| Rule10 | PowerShell param/help normalisation | FAIL (15) | Yes | Real findings in `tools/**`; earlier runs also scanned nested worktrees under `.claude/` (uses directory walk, not `git ls-files`) |
| Rule11 | Doc / script path references resolve | FAIL (11) | Yes | Real drift: eight package-level `AGENTS.md` files under the source tree point at per-package rules files that do not exist; two plan links |
| Rule12 | Agent ↔ adapter-surface command parity | PASS (84 agents × 4 surfaces) | Yes | Good rule, well-scoped |
| Rule13 | MustCodes catalogue integrity | FAIL (6) | Yes | 3 findings are the audit's own hardcoded `$domainMap` missing Cron/Token/Version (added 2026-08-31); 3 are real DataAnnotations code-parity drift from the same batch |
| Rule50 | `*Tests.cs` ↔ `*TestData.cs` pairing, Theory-only | PASS | Yes | The CI gate. Works. |
| Rule51 | Nested static groups inside `*Tests.cs` | FAIL (**3,961**) | **No — enforces the opposite of the spec** | `unit-test.md` v11 §5.1: the Tests class is *flat*; operation groups live in TestData only. Every finding is the rule being stale. |
| Rule52 | ValidCase record inheritance | FAIL (37) | Partly | Regex-based; conflates positional records and inheritance forms — see review notes §2.4 |
| Rule53 | Every `*Tests.cs` maps to a source class | FAIL (10) | Partly | Filename heuristic: false positives on moved types (`MustResult` now in Core), partial-class splits, plural drift (`MustStringNumbersClauses`) |
| Rule54 | camelCase tuple element names in TestData | FAIL (253) | Partly | Spec §4.3 does mandate camelCase, so some are real debt, but the regex also reads positional record parameter lists as tuples |

**Net:** 8 of 18 rules work as designed (02, 08, 09, 10, 11, 12, 13, 50); of those, two (09, 10)
audit the tooling itself rather than the library. The tool has never had a test of its own,
which is how four "cannot fail" rules survived for months.

### 2.3 Why it drifted

- **No tests for the auditor.** A one-line "does this rule produce a finding on a known-bad
  fixture" test would have caught Rules 03–07 on day one.
- **Regex over C# source** in a language (PowerShell) where escaping rules differ between
  quote styles; the `\\.` bug is a quoting mistake, not a logic mistake.
- **Hardcoded coupling** to build outputs (`bin/Release/net8.0`), to domain names (Rule13
  `$domainMap`), to file-name shapes (Rule53), and to a bootstrap JSON that was never committed.
- **Three languages, one owner.** The Roslyn half needs MSBuildLocator + MSBuildWorkspace + a
  full compilation for one project, yet the only place semantics would matter (collision
  analysis) never uses the `Compilation` it loads. Rule08 is syntax-only and could drop Roslyn
  entirely.
- **Spec moved, rule did not.** Rule51 encodes the pre-v11 test shape.
- **Real drift the working rules surfaced today**: the package-level agent files under the
  source tree (eight of them) link to per-package rules files that do not exist.

### 2.4 Review notes per area (from the four sub-agent reviews)

- **.NET auditor**: Rule01 is the only Roslyn-semantic consumer; Rule08 parses files with
  `CSharpSyntaxTree.ParseText` and never binds symbols. `Test-SpecOrdering.ps1` creates a
  per-run GUID temp `obj/bin` under `artifacts/audit/tmp/` and never deletes it, and does not
  propagate `dotnet`'s exit code. The `--help` text cites a project path that does not exist.
- **Library rules**: the escaping-bug family (Rules 03/04/05/07) is copy-pasted from one source.
  Rule13's domain map must be edited by hand for every new clause family — the audit is coupled
  to the thing it audits — and its signature regex assumes single-line signatures while the
  clause files routinely wrap return type, name and `[CallerArgumentExpression]` over three lines.
  Rules 02–06 need a prior Release build of the framework-specific DLL. `Test-ParityFluentGuard.ps1`
  (323 lines) is wired to nothing. `docs/ai/specs/language/vocabulary.json` carries `concepts`
  and `opposites` arrays that no script reads. The reviewer's own preference was reflection-based
  xUnit tests plus Roslyn analyzers (option B in §3) — recorded so D1 is decided with that view on
  the table.
- **Doc & testing rules**: Rule51 checks the wrong file kind for the v11 shape. Rule52 applies the
  layered-clause base-record convention repo-wide, but the root spec's own example
  (`unit-test.md` §4.2) is a bare `record ValidCase(string Name, …)` and 82 TestData files use that
  value-object shape legitimately — so `test-records` must follow the *per-layer* unit-test spec,
  not one global rule. Rule54's tuple regex is not anchored to a tuple's own paren and splices
  record parameters into tuple element lists. Rule53's 1:1 `<Pkg>.UnitTests → <Pkg>` model flags
  cross-project consumer tests (`MustResultTests` in MustClauses.UnitTests testing a Core type) but
  also finds real drift (`MustStringNumberClausesTests` vs `MustStringNumbersClauses.cs`,
  `FluentDateExtensionsTests` whose subject now lives in `FluentSqlDateTimeExtensions.cs`).
  Rule11 only checks bare paths ending in `.md`/`.ps1` and skips references rooted under the source tree; Rule12
  implements the Copilot-subset policy as a blanket skip of `.github/` and fails silently if the
  exceptions section is malformed. Rule10 walks the filesystem (nested `.claude/worktrees/`
  checkouts inflate its counts); Rule11 uses `git ls-files` and is immune. The three `utils/`
  scripts and `Run-AuditRules.ps1` have no callers outside the folder.

## 3. Options considered

| # | Option | Pros | Cons | Verdict |
|---|---|---|---|---|
| A | Fix in place (PowerShell + Roslyn) | Cheapest today; docs already match | Keeps 3-language stack, Windows runner, regex-over-C#, no tests; contrary to the owner's tooling direction | Reject |
| B | C# convention-test project (`+ tests/PineGuard.Conventions.UnitTests`: reflection over built assemblies + `Microsoft.CodeAnalysis.CSharp` syntax for ordering/tests) | The conventional .NET answer; semantics for free; runs in `dotnet test`; IDE-integrated | Needs a build before it can run; doc/adapter rules (11, 12) are awkward in xUnit; does not move toward `apps/cli`; still no `pineguard audit <rule>` UX | Runner-up. Right choice if the owner were *not* moving tooling to TypeScript |
| C | **TypeScript CLI in `apps/cli`, C# via tree-sitter, markdown via remark, tracked files via `git ls-files`** | Real AST (no regex fragility); no .NET build needed → sub-second, runs on `ubuntu-latest`; one language for all future tooling and the website; fixtures + unit tests per rule are natural; `pineguard audit all | <rule>` UX | New toolchain in the repo (Node, pnpm, lockfile, Dependabot surface); syntax-only — no semantic symbol resolution | **Recommended** |

**Why C wins on the merits, not just on preference.** Every check the audit performs is a
*source-shape* check: names, order, attributes, file pairing, invocation targets by name, doc
links, error-code constants. None needs a compiler; the current tool proves it by never using
the semantic model it loads. A real parser removes the entire class of bug that broke five
rules, and dropping the build dependency turns a 77 s Windows job into a ~2 s Linux job.
The one semantic question — nullability of a generic `T` primary parameter — is answerable from
syntax plus the repo's own convention (`T?` vs. a `where T : struct` constraint), and can be
backstopped later by a small reflection test in B's style if it ever matters.

## 4. Target design

### 4.1 Layout

```
+ apps/cli/
    package.json                 name @pineguard/cli, bin "pineguard", private
    tsconfig.json
    src/
      index.ts                   commander root; `audit` is the first subcommand
      commands/audit.ts          arg parsing → engine → reporter → exit code
      audit/
        catalog.ts               rule registry: slug, legacyId, scope, gate, description, run()
        engine.ts                runs rules, collects Findings, applies exceptions + baseline
        types.ts                 Finding { rule, file, line?, message, key }  (key = stable hash for baseline)
        repo.ts                  git ls-files, repo-root discovery, path normalisation
        parsing/csharp.ts        web-tree-sitter loader, cached parse, query helpers
        parsing/markdown.ts      remark/mdast link + front-matter extraction
        reporters/{pretty,json,github,sarif}.ts
        rules/<slug>.ts          one file per rule (see §4.3)
    config/
      exceptions.json            moved from tools/audit-cli/test-audit-exceptions.json, keyed by slug
      baseline.json              ratchet snapshot of accepted pre-existing findings (see §4.4)
    test/
      rules/<slug>.test.ts       vitest; every rule has ≥1 passing fixture, ≥1 failing fixture,
      fixtures/<slug>/…          and a "can-fail" assertion (the mutation test the old tool lacked)
+ package.json                   pnpm workspace root (apps/*), engines.node >= 22
+ pnpm-workspace.yaml
+ .node-version
```

`docs/ai/specs/language/vocabulary.json` stays where it is (the Brain owns vocabulary); the CLI
reads it. Output still goes to `artifacts/audit/` (`<slug>.json` per rule + `summary.json`), which
keeps the repo's file-hygiene rule.

### 4.2 Command surface

```
pineguard audit                      # all rules, pretty output, exit 1 on any un-baselined finding
pineguard audit all                  # explicit alias of the above
pineguard audit <slug|RuleNN> [...]  # one or more rules, e.g. `pineguard audit layer-parity test-files`
pineguard audit --scope library|testing|docs
pineguard audit --gate               # only merge-blocking rules (what CI runs)
pineguard audit --list               # catalog with slug, legacy id, scope, gate flag
pineguard audit --format pretty|json|github|sarif
pineguard audit --changed            # restrict to files changed vs main (pre-commit speed)
pineguard audit --update-baseline    # accept current findings as the new ratchet floor
pineguard audit --no-baseline        # show the full debt, ignoring the ratchet
pineguard banner                     # print the ASCII banner
```

Exit codes: 0 clean, 1 findings, 2 usage/config error. `--format github` emits
`::error file=…,line=…::` annotations so PR checks show inline.

### 4.3 Rule catalog (old → new)

Numeric ids survive as aliases because ~30 Brain files cite them; the cascade (§9 P5) moves the
Brain to slugs and the aliases can be dropped in a later release.

| Legacy | Slug (proposed) | Scope | Gate at cutover | Change from today |
|---|---|---|---|---|
| Rule02 | `rules-usage` | library | via baseline | Same intent; source-only (no DLL): every `public static` in `Core/Rules` is invoked somewhere in MustClauses |
| Rule03/04/05 | `must-usage` (`--layer guard\|fluent\|annotations`, default all) | library | via baseline | Three copies collapse into one rule with a layer parameter; **now actually able to fail** |
| Rule06 | `layer-parity` | library | via baseline | Concept-set parity via vocabulary.json; no `dotnet publish`; missing-layer is a finding, not a warning |
| Rule07 | `nullability` | library | via baseline | Hybrid policy on primary parameter, Must + Guard; syntax-based |
| Rule01 | `must-collisions` | library | via baseline | Overload ambiguity on `null` literal; the nullability half merges into `nullability`; exemptions live in `config/exceptions.json`, no bootstrap file |
| Rule08 | `ordering` | library | via baseline | Same normalisation rules, data-driven irregulars; whole-layer absence is a finding |
| Rule13 | `must-codes` | library | via baseline | Domain map **derived** from `Codes/` folder + clause file names, not hardcoded |
| Rule11 | `doc-links` | docs | **yes** | Same; tracked files only |
| Rule12 | `surface-parity` | docs | **yes** | Same (renamed so "adapter" is not overloaded with Guard/Fluent/DA) |
| Rule50 | `test-files` | testing | **yes** (today's gate) | Same |
| Rule51 | `test-structure` | testing | via baseline | **Rewritten to v11 §4–5**: TestData has nested operation groups; Tests is flat with one `<Group>_BehavesAsExpected` per group; dataset names ∈ {ValidCases, EdgeCases, InvalidCases}; §4.5 ordering |
| Rule52 | `test-records` | testing | via baseline | AST-based; records vs. tuples unambiguous |
| Rule53 | `test-orphans` | testing | via baseline | Resolves *type declarations* in the paired source project; handles partials and moved types |
| Rule54 | `test-tuples` | testing | via baseline | AST-based; camelCase + exact-parameter-name check per §4.3 |
| Rule09 | — | — | — | Dropped; catalog integrity becomes a unit test of `catalog.ts` |
| Rule10 | — (see D4) | — | — | Dropped from the audit; PowerShell hygiene belongs to PSScriptAnalyzer or vanishes with the PS→bash migration |
| utils/* , `Run-AuditRules.ps1` | — | — | — | Dropped (no callers) |

### 4.4 Baseline ratchet (how the gate widens without a big bang)

At cutover, `pineguard audit --update-baseline` snapshots every current finding by stable key
(rule + file + normalised message). CI runs `pineguard audit --gate` where the gate set is
*every* rule; a rule passes if it produces no finding outside the baseline. Debt is burned
down separately; when a rule's baseline entry count hits zero the entry is deleted and the rule
is a hard gate. This replaces today's "Rule50 only, widen later" comment in `ci.yml` with a
mechanism.

### 4.5 Parser choice and the spike

`web-tree-sitter` (WASM, no native build, works on every runner) + the `tree-sitter-c-sharp`
grammar (C# 1–13 supported; ships `nodeTypeInfo`, queries like
`(method_declaration name: (identifier) @name)`). The P1 spike must prove: load the wasm grammar,
parse three real files (`src/PineGuard.MustClauses/MustStringClauses.cs`, one Guard file, one
TestData file), and run the queries the rules need (method declarations with modifiers and
parameters, attributes, record declarations, tuple types, invocation expressions). Fallback if
the wasm artefact is not published for the pinned version: the native `tree-sitter` binding with
prebuilds. This is the only technical risk in the plan and it is retired first.

### 4.6 Test fixture convention (VIBE) — owner directive, 2026-09-06

The CLI's own test fixtures (every `test/fixtures/<slug>/…` under `apps/cli`, for every rule in
P2 and the harness that runs them) MUST follow **VIBE — Valid / Invalid / Boundary-Edge** — the
same three-way split already normative for the rest of the repo's C# tests
(`docs/ai/specs/testing/unit-test.md` v11 §4.1, §4.4: `ValidCases` → `EdgeCases` → `InvalidCases`).
This **replaces** the plain binary `pass`/`fail` split described earlier in this document
(§4.1's `test/fixtures/<slug>/…` line and P1.4's original brief) — wherever an earlier section of
this plan says `{pass,fail}`, read it as superseded by this section.

**Directory shape**, per rule slug:

```
apps/cli/test/fixtures/<slug>/
  valid/      — a minimal, realistic source tree the rule must NOT flag. Zero findings, always.
  invalid/    — a minimal, realistic source tree the rule MUST flag. ≥1 finding, always — this is
                the mandatory "can it fail" fixture (the exact gap that let the old tool's Rules
                03/04/05/07 go silently vacuous for months).
  boundary/   — edge-condition source trees that probe the rule's own decision boundary (an empty
                collection vs. one item, exactly the threshold value, the last item in an ordered
                list, a partial-class split, a moved/renamed type). Boundary fixtures do NOT carry
                a blanket valid/invalid expectation — each one is asserted individually in the
                rule's test file for whatever that specific boundary should produce (mirroring the
                C# spec's own `ValidEdgeScenarios`/`InvalidEdgeScenarios` split: a boundary case can
                legitimately be either still-valid or still-invalid).
```

**Harness API** (in `apps/cli/test/support/runRule.ts`, built by P1.4): expose three helpers over
the shape above —
- `expectValid(rule, slug)` — runs against `valid/`, asserts `[]`.
- `expectInvalid(rule, slug)` — runs against `invalid/`, asserts `≥1` finding with the same
  descriptive failure message P1.4 already wrote for the vacuous-rule case (naming Rules
  03/04/05/07 as the historical precedent) if it comes back empty. This is the renamed/refocused
  form of P1.4's original `expectRuleCanFail`.
- `runBoundary(rule, slug)` — runs against `boundary/` and returns the raw `Finding[]` for the
  calling test to assert against explicitly, case by case; no built-in pass/fail assumption.

Every rule in P2 MUST populate `valid/` and `invalid/` (non-negotiable, mirrors §10.3's "no empty
dataset" policy — don't scaffold an empty `boundary/` either; only add it when the rule actually
has a meaningful boundary to probe). Per §10.3 of the C# spec, omit `boundary/` entirely for a rule
with no meaningful edge condition rather than leaving an empty placeholder directory.

## 5. Decision gates (owner sign-off before P1 starts)

Per the repo's naming standard every public name below is a *proposal with rejected
alternatives*; the owner signs off, and the orchestrator does not proceed on a recommendation
alone for rows marked **owner**.

> **2026-09-06 — Owner sign-off recorded.** The owner instructed direct autonomous execution of
> this plan in full, in a worktree, with this thread as orchestrator ("/goal complete in full
> autonomously"). No owner was available synchronously for a per-decision round trip, and the
> instruction is read as delegating D1–D8 to this plan's own recommendations (each already
> carries rejected alternatives per the repo's naming standard). **Decisions taken: D1 = C
> (TypeScript CLI in `apps/cli`), D2 = `@pineguard/cli` / bin `pineguard` / `pineguard audit`,
> D3 = slugs as listed in §4.3, D4 = drop Rule10 from the audit, D7 = delete
> `tools/audit-cli/` + `PineGuard.AuditCli.slnx` at cutover, no shim.** D5/D6/D8 apply per policy
> as already marked. This does **not** extend to merging the resulting branch into `main` or
> pushing to the remote — P6.1's merge and any push are held for explicit owner confirmation
> after P5 cascade review, per the repo's safety spec for actions on shared state.

| # | Decision | Recommendation | Alternatives considered | Handling |
|---|---|---|---|---|
| D1 | Rebuild in TypeScript under `apps/cli` (option C) vs C# convention tests (option B) | **C** | B is the conventional .NET answer and the library-rules reviewer's preference (§2.4); it is documented in §3 so it can be chosen instead. The two are not exclusive: a reflection backstop can be added later without touching the CLI | **owner** |
| D9 | `vocabulary.json` dead keys (`concepts`, `opposites`) | Remove unless `layer-parity` gains a use for them | Wire `opposites` into Guard `Not*` complement resolution | Fable, logged |
| D2 | Package / bin / command names | `@pineguard/cli`, bin `pineguard`, `pineguard audit` | `pineguard-cli`; `@pineguard/tools`; bin `pg` (too terse, collides with PostgreSQL's `pg`) | **owner** |
| D3 | Rule slugs (§4.3 column 2) | as listed; `layer-parity` vs `surface-parity` deliberately disambiguates two "adapter" meanings | `parity`/`adapter-parity` (ambiguous); `usage-guard`/`usage-fluent`/`usage-data` (three rules instead of one) | **owner** |
| D4 | Rule10 (PowerShell normalisation) disposition | Drop from the audit; add a PSScriptAnalyzer task only if the PS→bash plan stalls | Port as `ps-normalization` (keeps a PowerShell-specific rule inside a TypeScript tool) | **owner** |
| D5 | Package manager / runtime | pnpm workspaces, Node 22 LTS | npm workspaces (no extra tool, weaker workspace ergonomics); Bun (fast, less ubiquitous on CI) | apply per policy, log |
| D6 | CI audit job moves to `ubuntu-latest`, no .NET setup | yes | keep `windows-latest` for parity with other jobs | apply per policy, log |
| D7 | Delete `tools/audit-cli` + `PineGuard.AuditCli.slnx` at cutover (no shim, per the no-back-compat rule) | yes, after P4 parity is signed off | keep `.ps1` shims that shell out to the new CLI | **owner** |
| D8 | Config location | `+ apps/cli/config/` (self-contained) | repo-root `pineguard.audit.json` (ESLint-style; but root is already crowded and the hygiene rule discourages new root files) | apply per policy, log |

## 6. Business plan

- **Value**: restores the only machine enforcement of the layer invariants; turns a 77 s, Windows-only,
  build-dependent check into a ~2 s Linux step that can run on every PR for every rule; removes
  ~7.8 k lines across three languages; establishes `apps/` for the website and future tooling.
- **Cost**: roughly 3.3 M tokens across the tiers in §9.4 and three to four orchestrator sessions;
  one new toolchain (Node/pnpm) in the repo and in CI.
- **Success metrics**: every rule has a failing fixture test; `pineguard audit --gate` green on
  `main` at cutover with the baseline committed; CI audit job < 60 s including install;
  `tools/audit-cli` gone; Brain and all ten adapter surfaces cite slugs and the new command;
  `surface-parity` and `doc-links` pass on the cascaded docs.

## 7. Functional plan

1. Catalog exposes slug, legacy id, scope, gate, description, and a `run(ctx) → Finding[]`.
2. Engine resolves selection (`all`, slugs, legacy ids, `--scope`, `--gate`), builds a shared
   context once (tracked files, parsed C# cache, vocabulary, exceptions, baseline), runs rules
   (in parallel where independent), applies exceptions then baseline, hands findings to the
   reporter, returns the exit code.
3. Reporters: `pretty` (default; per-rule header, counts, first N findings, summary table),
   `json` (`artifacts/audit/summary.json` + per-rule files), `github` (annotations), `sarif`
   (optional, last).
4. Every rule reads only from the shared context — no rule shells out to `dotnet`, `git`, or
   PowerShell.

## 8. Technical plan (per-rule notes the implementer must honour)

- `must-usage`: enumerate `public static` extension methods on `IMustClause`-typed first
  parameter in `src/PineGuard.MustClauses/*.cs`; for each layer, collect invocation targets
  `Must.Be.<Name>` / `Must.Not.Be.<Name>` (member-access chains) from that layer's sources; report
  each Must name with zero call sites. Fixture: a Must method with no Guard caller **must** fail.
- `layer-parity`: concept normalisation exactly as `docs/ai/specs/language/vocabulary.json`
  defines (Guard `Not*` complements, Fluent positive names, DataAnnotations "no unknown
  concepts" policy from `docs/ai/specs/tools/audit-cli/spec.md` §3.1–3.2, which the rewritten
  spec carries forward).
- `nullability`: primary parameter = second parameter of a Must extension / first value parameter
  of a Guard method; reference types must be `T?`, value types non-nullable; generic `T` is treated
  by constraint (`struct` → value). Exemptions by method name in `config/exceptions.json`.
- `ordering`: keep the existing normalisation table (strip `Is`/`In`/`Attribute`/domain affixes and
  `Not`/`Non`/`Invalid` with word boundaries); a missing sibling layer is a finding; Guard ordering
  is compared by the Must clause each Guard method invokes (first `Must.` member-access chain in
  the body, reported if more than one distinct target exists).
- `must-codes`: derive domain from the `Codes/<Domain>Codes.cs` file set; checks (a)–(f) as today.
- `doc-links`: tracked `.md` files + `.vscode/tasks.json`; resolve backticked paths and markdown
  links for *any* extension and any root (source-tree paths included), not only `.md`/`.ps1`; honour the
  `+ path` convention for planned files.
- `surface-parity`: parse `docs/ai/meta/adapter-surfaces.md` for full surfaces, palettes, and subset
  policies as Rule12 does today, but fail loudly if the exceptions section is missing, and check
  the Copilot "one representative per family" policy instead of skipping `.github/` wholesale.
- `test-structure`: implement v11 §4.1, §4.5, §5.1, §5.2 verbatim; add a fixture pair copied from a
  real conforming `XxxTests.cs`/`XxxTestData.cs`.
- `test-records`: the base-record requirement comes from the *per-layer* spec
  (`docs/ai/specs/<layer>/unit-test.md`); bare `record ValidCase(...)` is valid where the layer spec
  (or the root spec §4.2) shows it. Scope by test project, not repo-wide.
- `test-orphans`: for `FooTests.cs` in `tests/<Pkg>.UnitTests/…` find a `class|record|struct|interface Foo`
  declaration anywhere in the `<Pkg>` source project **or any project it references** (read `ProjectReference`
  items from the csproj); partials included; exceptions by path. Keep the real-drift signal
  (singular/plural and renamed-subject cases) — those are findings, not noise.
- `test-tuples`: operate on `tuple_type` nodes inside record/parameter declarations, never on text;
  check camelCase and, where the paired source method is resolvable, exact parameter names (§4.3).
- Vocabulary: `layer-parity` reads `stripPrefixes`, `ignoreMethods`, `aliases`; the unused
  `concepts`/`opposites` arrays are either wired in or removed from `vocabulary.json` (Fable call,
  logged in §5).

## 9. Execution playbook (Sonnet 5 orchestrator; sub-agents only)

### 9.1 Model routing

| Tier | Model | Used for here |
|---|---|---|
| Judgment (high) | **Fable** | D1–D8 naming brief with rejected alternatives; any spec/plan contradiction found mid-flight |
| Judgment (light) | **Opus** | P4 parity verification; "can this rule fail" audit; spec-conformance review of the five `test-*` rules; cascade completeness |
| Implementation | **Sonnet 5** | Scaffold, engine, every rule, reporters, spec rewrite |
| Bulk IO | **Haiku** | Inventory sweeps, moving/rewriting the ten adapter surfaces, tasks.json, PR template, deleting the old tree, memory updates |

Every sub-agent prompt names this file and the section numbers it executes; the agent reads them
from disk. One task per agent. Parallel groups fan out in a single dispatch.

### 9.2 Progress tracker (source of truth — the orchestrator updates Status in place)

Status values: `todo` · `in-flight` · `blocked` · `review` · `done`. "∥" = may run in parallel
with every other row in the same group; "→" = depends on the row(s) named.

| # | Task | Agent | ∥ / → | Status | Notes |
|---|---|---|---|---|---|
| **P0 — Decide** | | | | | |
| P0.1 | Naming + decision brief for D1–D8 (recommendation, rejected alternatives, one paragraph each) | Fable | — | done | Already present in §5 as authored 2026-09-06 |
| P0.2 | Owner sign-off on D1, D2, D3, D4, D7 | owner | → P0.1 | done | Recorded in §5 2026-09-06 note (autonomous delegation) |
| **P1 — Foundation** (all ∥ except P1.5) | | | | | |
| P1.1 | Workspace scaffold: root `package.json`, `pnpm-workspace.yaml`, `.node-version`, `apps/cli` package with tsconfig (strict, ESM), vitest, eslint/prettier minimal, `bin` wiring, `.gitignore` additions | Sonnet | ∥ | done | No rules yet; `pineguard audit --list` prints an empty catalog. Deviations: (1) pinned `tree-sitter-c-sharp@0.23.5` + `web-tree-sitter@0.27.0` as dependencies — confirmed via `npm pack` that the official grammar package ships a prebuilt `tree-sitter-c_sharp.wasm`, and a smoke test loaded it and parsed C# successfully, so P1.2 starts from a known-good pair instead of building/vendoring wasm itself; (2) `typescript` pinned to `6.0.3`, not npm-latest `7.0.2`, because `typescript-eslint@8.69.0`'s peer range (`>=4.8.4 <6.1.0`) doesn't yet cover TS 7; (3) pnpm 12's new install-script gate required an `allowBuilds` block in `pnpm-workspace.yaml` (esbuild allowed for tsup/vite; tree-sitter-c-sharp's native-binding install script left disabled since only its wasm is used); (4) bin is wired to the built `dist/index.js` (via `tsup`), with `tsx src/index.ts` documented in `apps/cli/README.md` as the dev-mode path — both verified working |
| P1.2 | Parser spike: load `tree-sitter-c-sharp` via `web-tree-sitter`, parse the three §4.5 files, run the §4.5 queries, write `parsing/csharp.ts` + tests | Sonnet | ∥ | done | **Risk retired — wasm works, no native fallback needed.** `parsing/csharp.ts` loads the wasm via `import.meta.resolve("tree-sitter-c-sharp/tree-sitter-c_sharp.wasm")` (no exports map on that package, so the subpath resolves under plain Node ESM) and caches parsed `Tree`s by absolute path + mtime/size. Queries are **walk-then-filter** (`Node#descendantsOfType` + `childForFieldName`/`children`), not `Query` s-expressions — simpler and more robust for this grammar; documented in the module's header comment. Verified against `src/PineGuard.MustClauses/MustStringClauses.cs`, `src/PineGuard.GuardClauses/GuardStringClauses.cs`, `tests/PineGuard.MustClauses.UnitTests/MustStringClausesTestData.cs` — 9 tests in `test/audit/parsing/csharp.test.ts`, all green. |
| P1.3 | Repo + markdown infra: `repo.ts` (`git ls-files`, root discovery), `parsing/markdown.ts` (remark links, YAML front-matter), tests | Sonnet | ∥ | done | `repo.ts`: `findRepoRoot(startPath?)` walks up looking for a `.git` entry (file or dir, so worktree checkouts resolve correctly); `listTrackedFiles(patterns?, startPath?)` spawns plain `git ls-files` (newline-split, not `-z`) and normalises to forward slashes; `readRepoFile(relativePath, startPath?)` reads by repo-relative path. Regression test proves the worktree-immunity claim: running from inside `.claude/worktrees/audit-cli-rebuild` itself, `listTrackedFiles()` includes this plan file and contains no `.claude/worktrees/` path. `parsing/markdown.ts`: `extractPathReferences` walks the mdast tree (`remark` + `remark-frontmatter` so the front-matter block parses as one opaque node) for `link` and `inlineCode` nodes, tagging `+ ` prefixed code spans `{planned: true}` per the plan's own convention; `extractFrontMatter` deliberately parses the leading `---` block with a plain regex (not the mdast tree) and hands the captured YAML to the new `yaml` npm package (`2.9.0`, added as a direct dependency — not in P1.1's installed set; `remark-frontmatter` alone only exposes the raw block text, not a parsed object) — this sidesteps needing `@types/mdast`/`@types/unist`, which are only *transitive* deps and are not resolvable through this workspace's isolated pnpm `node_modules` (confirmed empirically: `tsc --noEmit` needs no `mdast`/`unist` type import anywhere in either file). |
| P1.4 | Fixture convention + test harness: `test/fixtures/<slug>/{pass,fail}/…`, `runRule(slug, fixtureDir)` helper, the mandatory "fail fixture yields ≥1 finding" assertion template | Sonnet | ∥ | done | Harness API for every P2 agent: `apps/cli/test/support/runRule.ts` exports `runRuleOnFixture(rule: Rule, fixtureDir: string): Promise<Finding[]>` and `expectRuleCanFail(rule: Rule, fixtureDir: string): Promise<Finding[]>` (mandatory for every fail-fixture assertion; throws a descriptive error — naming Rules 03/04/05/07 — if findings come back empty, and a separate clear error if `fixtureDir` doesn't exist). `Rule`/`Finding`/`RuleContext` land in `apps/cli/src/audit/types.ts` (types-only, additive — P1.5 wraps/extends, does not replace). Convention (`test/fixtures/<slug>/{pass,fail}/…`, pair with `test/rules/<slug>.test.ts`) documented in `apps/cli/test/README.md`. Self-test `apps/cli/test/support/runRule.test.ts` + demo fixtures `test/fixtures/_demo-harness/{pass,fail}/Greeter.cs` (inline demo rule flags literal `BADWORD`) prove: pass fixture → `[]`, fail fixture → ≥1 finding, `expectRuleCanFail` passes on the real fail fixture and throws when pointed at `pass/`. **CORRECTED 2026-09-06 (P1.4b, before any P2 agent started)**: the binary `pass`/`fail` convention above is superseded by §4.6's VIBE (Valid/Invalid/Boundary-Edge) convention — read this note, not the original text above, for the current API. Fixtures now live at `test/fixtures/<slug>/{valid,invalid,boundary}/`. `runRule.ts` now exports `expectValid(rule: Rule, slug: string): Promise<void>` (runs `<slug>/valid/`, throws if any finding), `expectInvalid(rule: Rule, slug: string): Promise<Finding[]>` (the renamed `expectRuleCanFail`; runs `<slug>/invalid/`, throws the same Rules-03/04/05/07 message if empty), and `runBoundary(rule: Rule, slug: string): Promise<Finding[]>` (runs `<slug>/boundary/`, returns raw findings, no built-in pass/fail assumption). `runRuleOnFixture(rule, fixtureDir)` stays exported as the generic lower-level runner all three are built on. Demo fixtures renamed `_demo-harness/{pass→valid,fail→invalid}/Greeter.cs`; added `_demo-harness/boundary/{IdentifierSubstring.cs,CaseVariant.cs}` (probing the demo rule's naive-substring-match edges: an all-caps token embedded inside a longer identifier still trips it — still-invalid; the same token in lowercase inside a comment does not — still-valid) and a harness-only `_demo-harness-vacuous/invalid/Clean.cs` (proves `expectInvalid` throws when an `invalid/` fixture doesn't actually violate the rule). `apps/cli/test/README.md` rewritten for VIBE. `tsc --noEmit`, `vitest run` (37/37), `eslint .`, `prettier --check .` all clean. Types in `src/audit/types.ts` needed no changes (no pass/fail terminology there). |
| P1.5 | Engine core: catalog, types, engine (selection, context, exceptions), `pretty` + `json` reporters, exit codes, `audit` command | Sonnet | → P1.1 | todo | Baseline + `github`/`sarif`/`--changed` are P3 |
| P1.6 | ASCII TUI banner (`pineguard` bare invocation + a `pineguard banner` command) and a polished `pineguard --help` root output | Sonnet | ∥ | done | `src/banner.ts` (pure `renderBanner()`, reads version from `package.json`) + `src/commands/banner.ts`; bare invocation prints the banner then commander's own help listing (its default "no command" behaviour, exit 1, like bare `git`); `--help` stays banner-free (exit 0); tests in `test/banner.test.ts`. **Redone 2026-09-06 (owner feedback: the first pass's hand-rolled 5x5 block font looked amateurish next to OpenClaw / Hermes Agent / Claude Code).** Now rendered live through `figlet@1.11.4` (ships its own types; no `@types/figlet`) in the **"ANSI Shadow"** font — the block-capital face the reference CLIs actually use (Hermes Agent, create-t3-app and Gemini CLI all ship it pre-rendered; none use `cfonts`). ~30 bundled fonts were rendered and measured for "PineGuard": ANSI Shadow is 6 rows x 71 cols (fits 80 with margin); `cfonts/block` is the same face letter-spaced out to exactly 80 cols, so it was rejected along with its extra deps. Colour via `picocolors@1.1.1` (`createColors(enabled)` keeps the original gate: TTY only, off under `NO_COLOR`/`CI`): glyph bodies green, the font's drop-shadow strokes dim green, version dim; no gradient library. `renderBanner({ color })` and `shouldUseColor(env, isTTY)` take explicit inputs so both paths are unit-tested; integration tests now key on the tagline and figlet's own first row instead of a hand-drawn marker. |
| **P2 — Rules** (each row ∥; fan out after P1) | | | | | |
| P2.1 | `rules-usage` | Sonnet | → P1.2, P1.4, P1.5 | todo | Port of Rule02, source-only |
| P2.2 | `must-usage` (guard/fluent/annotations) | Sonnet | → P1 | todo | Replaces Rule03/04/05; must-fail fixture mandatory |
| P2.3 | `layer-parity` | Sonnet | → P1 | todo | Replaces Rule06 |
| P2.4 | `nullability` | Sonnet | → P1 | todo | Replaces Rule07 + Rule01 nullability half |
| P2.5 | `must-collisions` | Sonnet | → P1 | todo | Replaces Rule01 collision half |
| P2.6 | `ordering` | Sonnet | → P1 | todo | Port of `MethodOrderingAudit.cs` normalisation |
| P2.7 | `must-codes` | Sonnet | → P1 | todo | Derived domain map |
| P2.8 | `doc-links` | Sonnet | → P1.3, P1.4, P1.5 | todo | Port of Rule11 incl. `+ path` convention |
| P2.9 | `surface-parity` | Sonnet | → P1.3, P1.4, P1.5 | todo | Port of Rule12 |
| P2.10 | `test-files` | Sonnet | → P1 | todo | Port of Rule50 — today's gate; parity must be exact |
| P2.11 | `test-structure` | Sonnet | → P1 | todo | **Rewrite** to v11 §4–5, not a port of Rule51 |
| P2.12 | `test-records` | Sonnet | → P1 | todo | AST-based |
| P2.13 | `test-orphans` | Sonnet | → P1 | todo | Type-declaration resolution |
| P2.14 | `test-tuples` | Sonnet | → P1 | todo | AST-based |
| **P3 — Engine extras** (∥ with P2) | | | | | |
| P3.1 | Baseline ratchet: stable finding keys, `config/baseline.json`, `--update-baseline`, `--no-baseline`, `--gate` semantics per §4.4 | Sonnet | → P1.5 | todo | |
| P3.2 | `github` reporter + `--changed` (diff vs `main` via `git diff --name-only`) | Sonnet | → P1.5 | todo | |
| P3.3 | `sarif` reporter | Sonnet | → P1.5 | todo | Optional; last |
| **P4 — Verify** | | | | | |
| P4.1 | Parity diff: run old `Run-All.ps1` and new `pineguard audit --no-baseline --format json` on the same commit; per rule explain every delta (expected: new rules find what old ones could not; `test-files` identical) | Opus | → P2.*, P3.1 | todo | Requires one Release build for the old tool; take the `dotnet-ops` lock |
| P4.2 | "Can it fail" audit: for every rule confirm the fail fixture produces a finding and that the assertion is not vacuous | Opus | → P2.* | todo | The test the old tool never had |
| P4.3 | Spec conformance of `test-*` rules against `docs/ai/specs/testing/unit-test.md` v11 and the per-layer unit-test specs | Opus | → P2.10–P2.14 | todo | Any contradiction → Fable, spec wins |
| **P5 — Cascade** (rows ∥ unless noted) | | | | | |
| P5.1 | Rewrite `docs/ai/specs/tools/audit-cli/spec.md` (v2: layout, catalog, baseline policy, parser, fixture rule), update `docs/ai/workflows/audit.md`, `docs/ai/agents/audit-cli.md`, `docs/ai/commands/` entry | Sonnet | → P4 | todo | Keep the "rule ids" concept but cite slugs |
| P5.2 | `ci.yml` job 7 → `ubuntu-latest`, setup-node + pnpm, `pineguard audit --gate --format github`; drop `setup-dotnet` from that job | Sonnet | → P3.1 | todo | D6 |
| P5.3 | `.vscode/tasks.json`: replace the 21 audit tasks with slug-based `pineguard audit …` tasks | Haiku | → P5.1 | todo | |
| P5.4 | Ten adapter surfaces + root boot files + `.github/PULL_REQUEST_TEMPLATE.md` + `.github/instructions/testing.instructions.md` + `copilot-instructions.md`: replace `Run-All.ps1 -RuleId RuleNN` and rule ids with the new command and slugs | Haiku | → P5.1 | todo | Then run `pineguard audit surface-parity doc-links` as the self-check |
| P5.5 | Brain sweep: every remaining `RuleNN` citation in `docs/ai/**` (≈30 files) → slug; `cross-platform-tools-migration.md` no longer defers audit-cli (it is gone) | Haiku | → P5.1 | todo | |
| P5.6 | Cascade completeness review | Opus | → P5.1–P5.5 | todo | |
| **P6 — Cutover** | | | | | |
| P6.1 | Commit baseline snapshot; CI green on the branch; merge | Sonnet | → P5 | todo | Commit message per the repo's conventional-commit + prose-body standard |
| P6.2 | Delete `tools/audit-cli/` and `PineGuard.AuditCli.slnx`; remove `artifacts/audit/tmp` leftovers; update any tools-level readme that lists the folder | Haiku | → P6.1, D7 | todo | |
| P6.3 | Memory: update `project_audit-cli-state-2026-09` to "rebuilt"; note slugs and the baseline mechanism | Haiku | → P6.2 | todo | |
| **P7 — Debt burn-down** (separate plan) | | | | | |
| P7.x | Reduce `config/baseline.json` to empty, rule by rule (test-structure, test-tuples, layer-parity, ordering, must-codes DA drift, doc-links AGENTS.md links) | — | out of scope | — | Tracked as its own plan once P6 lands |

### 9.3 Fan-out shape

- Dispatch 1 (after P0.2): P1.1, P1.2, P1.3, P1.4 together (4 Sonnet).
- Dispatch 2: P1.5.
- Dispatch 3: P2.1–P2.14 and P3.1–P3.3 together (17 Sonnet) — split into two waves of 8–9 if the
  session's workflow-size guideline applies; library rules first because they share the C# parse
  cache and surface parser gaps early.
- Dispatch 4: P4.1–P4.3 together (3 Opus).
- Dispatch 5: P5.1 + P5.2 (Sonnet), then P5.3–P5.5 (Haiku) together, then P5.6 (Opus).
- Dispatch 6: P6.1 → P6.2 → P6.3 serially.

### 9.4 Token budget (rough)

| Tier | Agents | Est. tokens |
|---|---|---|
| Fable | 1 | ~60 k |
| Opus | 5 | ~550 k |
| Sonnet | ~25 | ~2.4 M |
| Haiku | ~7 | ~300 k |
| **Total** | | **~3.3 M**, three to four orchestrator sessions |

## 10. Definition of Done

- [ ] D1–D8 logged in §5 with the owner's answers.
- [ ] `pnpm install && pnpm -C apps/cli test` green; every rule has pass and fail fixtures and the
      fail fixture is asserted to produce a finding.
- [ ] `pineguard audit --list` shows 14 rules with slugs, legacy ids, scope, gate.
- [ ] P4.1 parity report checked in under `artifacts/audit/` is reviewed and every delta explained.
- [ ] `pineguard audit --gate` green on `main` with `config/baseline.json` committed.
- [ ] CI audit job on `ubuntu-latest`, no .NET setup, < 60 s.
- [ ] `tools/audit-cli/` and the `.slnx` deleted; no `Run-All.ps1` reference remains anywhere
      (`doc-links` proves it).
- [ ] Spec, workflow, agent, command, ten adapter surfaces, tasks.json, PR template updated;
      `surface-parity` and `doc-links` pass.
- [ ] Memory updated.

## 11. Risks and out of scope

**Risks**
- tree-sitter wasm availability for the pinned grammar version → P1.2 spike first; native fallback.
- Syntax-only nullability on generic parameters → constraint heuristic + documented exemption path;
  reflection backstop possible later (option B style) without changing the CLI.
- New toolchain surface (Dependabot, lockfile churn) → pin versions, `pnpm` `minimumReleaseAge`,
  Dependabot grouped updates for `apps/cli`.
- Baseline masking real regressions in already-dirty rules → keys include file + message; a new
  finding in an already-failing file still fails.

**Out of scope**
- Burning down the baseline (P7, separate plan).
- Consolidating the other 48 PowerShell scripts under `tools/` into `pineguard <command>`; the
  Bash migration plan stands as written except that audit-cli is no longer "deferred" — it is removed.
- `apps/web` (marketing site) — only the `apps/` workspace root is created here.
- Publishing `@pineguard/cli` to npm; it stays `private: true`.
