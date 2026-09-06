# `apps/cli` test conventions

This directory holds the shared test harness (`test/support/`) plus per-rule
fixtures and tests for `pineguard audit`. See
[`docs/ai/plans/audit-cli-rebuild.md`](../../../docs/ai/plans/audit-cli-rebuild.md)
§4.1 for the full plan context; this file is the practical "how do I test a
rule" reference every P2 rule-implementer agent should read first.

## Fixture convention

Every rule slug gets a fixture pair:

```
test/fixtures/<slug>/pass/...   a minimal, realistic source tree the rule must NOT flag
test/fixtures/<slug>/fail/...   a minimal, realistic source tree the rule MUST flag (>=1 finding)
```

- **Minimal**: only what's needed to trip (or not trip) the rule. Don't copy
  a whole real file if three lines make the point.
- **Realistic**: valid C#/Markdown/JSON shaped like the repo's own code, not
  contrived syntax the rule would never encounter in production.
- **`pass/` must be a real accept case**, not an empty directory. An empty
  fixture proves nothing — it can't distinguish "the rule correctly ignores
  this" from "the rule never ran at all".

## Running a rule against a fixture

Use the harness in [`test/support/runRule.ts`](support/runRule.ts):

```ts
import { join } from "node:path";
import { expect, test } from "vitest";

import { myRule } from "../../src/audit/rules/my-rule.js";
import { expectRuleCanFail, runRuleOnFixture } from "../support/runRule.js";

const FIXTURES = join(import.meta.dirname, "../fixtures/my-rule");

test("passes on a conforming tree", async () => {
    expect(await runRuleOnFixture(myRule, join(FIXTURES, "pass"))).toEqual([]);
});

test("fails on a violating tree", async () => {
    await expectRuleCanFail(myRule, join(FIXTURES, "fail"));
});
```

- `runRuleOnFixture(rule, fixtureDir): Promise<Finding[]>` runs `rule` with
  `{ rootDir: fixtureDir }` as its context and returns whatever `Finding[]`
  it produces.
- `expectRuleCanFail(rule, fixtureDir): Promise<Finding[]>` is **mandatory**
  for every rule's fail-fixture test. It runs the rule and throws a
  descriptive error if the result is empty (or if `fixtureDir` doesn't
  exist). Do not substitute a bare
  `expect(findings.length).toBeGreaterThan(0)` — see the doc comment at the
  top of `runRule.ts` for why that reads the same but is weaker.

This is the direct fix for the old `tools/audit-cli` tool's exact failure
mode: Rules 03, 04, 05 and 07 all "passed" for months because a quoting bug
silently produced zero findings, and nothing ever asserted otherwise (plan
§2.2-2.3). `expectRuleCanFail` — and the later "can it fail" audit in plan
§9.2 P4.2 — exist so that can't happen silently again.

## Types

`Rule` and `Finding` are declared in
[`src/audit/types.ts`](../src/audit/types.ts) (types only — see that file's
header comment). `src/audit/engine.ts` and `src/audit/catalog.ts` (plan
P1.5) wrap `Rule` with catalog metadata (slug, legacyId, scope, gate,
description) and may extend `RuleContext`; neither should need to change the
fields declared today.

## Self-test

[`test/support/runRule.test.ts`](support/runRule.test.ts) proves the harness
itself works, using a trivial demo rule (flags the literal string
`BADWORD`) against `test/fixtures/_demo-harness/{pass,fail}/`. It is **not**
a real audit rule or a template to copy structurally — it just demonstrates
the four properties every rule test relies on: a clean pass fixture returns
`[]`, a fail fixture returns findings, `expectRuleCanFail` accepts a genuine
failure, and it rejects a fixture that doesn't actually violate anything.

Real rule tests go in `test/rules/<slug>.test.ts`, one per rule, each backed
by its own `test/fixtures/<slug>/{pass,fail}/`.
