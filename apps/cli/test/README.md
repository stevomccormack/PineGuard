# `apps/cli` test conventions

This directory holds the shared test harness (`test/support/`) plus per-rule
fixtures and tests for `pineguard audit`. See
[`docs/ai/plans/audit-cli-rebuild.md`](../../../docs/ai/plans/audit-cli-rebuild.md)
§4.6 for the full plan context; this file is the practical "how do I test a
rule" reference every P2 rule-implementer agent should read first.

## Fixture convention — VIBE (Valid / Invalid / Boundary-Edge)

Every rule slug gets a fixture triad:

```
test/fixtures/<slug>/valid/      a minimal, realistic source tree the rule must NOT flag (always [])
test/fixtures/<slug>/invalid/    a minimal, realistic source tree the rule MUST flag (always >=1 finding)
test/fixtures/<slug>/boundary/   edge-condition source trees that probe the rule's own decision boundary
```

This is the same three-way split already normative for the rest of the
repo's C# tests (`docs/ai/specs/testing/unit-test.md` v11 §4.1/§4.4:
`ValidCases` → `EdgeCases` → `InvalidCases`), applied to this CLI's own
fixtures.

- **`valid/` and `invalid/` are mandatory for every rule.** Non-negotiable —
  this is the direct fix for the old `tools/audit-cli` tool's exact failure
  mode (below).
- **`boundary/` is optional, and only when meaningful.** Add it when the rule
  has a real edge to probe — an empty collection vs. one item, exactly the
  threshold value, the last item in an ordered list, a partial-class split,
  a moved/renamed type. Per plan §4.6/§10.3's "no empty dataset" policy,
  don't scaffold an empty `boundary/` placeholder for a rule with no
  meaningful edge condition — omit the directory entirely instead.
- Boundary fixtures carry **no blanket valid/invalid expectation**. Each one
  is asserted individually, case by case, in the rule's own test — a
  boundary case can legitimately turn out still-valid (no finding) or
  still-invalid (a finding); the point is probing the boundary, not
  producing a uniform verdict.
- **Minimal**: only what's needed to trip (or not trip) the rule. Don't copy
  a whole real file if three lines make the point.
- **Realistic**: valid C#/Markdown/JSON shaped like the repo's own code, not
  contrived syntax the rule would never encounter in production.
- **`valid/` must be a real accept case**, not an empty directory. An empty
  fixture proves nothing — it can't distinguish "the rule correctly ignores
  this" from "the rule never ran at all".

## Running a rule against a fixture

Use the harness in [`test/support/runRule.ts`](support/runRule.ts):

```ts
import { expect, test } from "vitest";

import { myRule } from "../../src/audit/rules/my-rule.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

test("passes on a conforming tree", async () => {
    await expectValid(myRule, "my-rule");
});

test("fails on a violating tree", async () => {
    await expectInvalid(myRule, "my-rule");
});

test("boundary: <describe the specific edge>", async () => {
    const findings = await runBoundary(myRule, "my-rule");
    // assert on `findings` for exactly what this boundary case should
    // produce — there is no built-in pass/fail assumption here.
});
```

- `expectValid(rule, slug): Promise<void>` runs `rule` against
  `test/fixtures/<slug>/valid/` and throws if it produces **any** finding.
- `expectInvalid(rule, slug): Promise<Finding[]>` runs `rule` against
  `test/fixtures/<slug>/invalid/` and throws a descriptive error if the
  result is empty. **Mandatory** for every rule's invalid-fixture test — do
  not substitute a bare `expect(findings.length).toBeGreaterThan(0)`; see
  the doc comment at the top of `runRule.ts` for why that reads the same but
  is weaker.
- `runBoundary(rule, slug): Promise<Finding[]>` runs `rule` against
  `test/fixtures/<slug>/boundary/` and returns the raw findings for the test
  to assert against explicitly — no assumption either way.
- `runRuleOnFixture(rule, fixtureDir): Promise<Finding[]>` is the generic,
  lower-level runner the three helpers above are built on. It's still
  exported for an ad-hoc fixture path (e.g. asserting on one specific file
  inside `boundary/` in isolation) if a rule test needs that.

This is the direct fix for the old `tools/audit-cli` tool's exact failure
mode: Rules 03, 04, 05 and 07 all "passed" for months because a quoting bug
silently produced zero findings, and nothing ever asserted otherwise (plan
§2.2-2.3). `expectInvalid` — and the later "can it fail" audit in plan §9.2
P4.2 — exist so that can't happen silently again.

## Types

`Rule` and `Finding` are declared in
[`src/audit/types.ts`](../src/audit/types.ts) (types only — see that file's
header comment). `src/audit/engine.ts` and `src/audit/catalog.ts` (plan
P1.5) wrap `Rule` with catalog metadata (slug, legacyId, scope, gate,
description) and may extend `RuleContext`; neither should need to change the
fields declared today.

## Self-test

[`test/support/runRule.test.ts`](support/runRule.test.ts) proves the harness
itself works, using a trivial demo rule (flags the literal, case-sensitive
substring `BADWORD`) against `test/fixtures/_demo-harness/{valid,invalid,boundary}/`
plus a harness-only `test/fixtures/_demo-harness-vacuous/invalid/` used to
prove `expectInvalid` isn't vacuous. It is **not** a real audit rule or a
template to copy structurally — it demonstrates the properties every rule
test relies on: a clean `valid/` fixture returns `[]`, an `invalid/` fixture
returns findings, `expectInvalid` rejects a fixture that doesn't actually
violate anything, `runBoundary` returns findings a test can assert on case
by case, and both throw a distinct error for a missing fixture path.

Real rule tests go in `test/rules/<slug>.test.ts`, one per rule, each backed
by its own `test/fixtures/<slug>/{valid,invalid}/` (plus `boundary/` when
meaningful).
