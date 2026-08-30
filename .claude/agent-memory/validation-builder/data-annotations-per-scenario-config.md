---
name: data-annotations-per-scenario-config
description: How to reuse a fixture whose config parameter varies per scenario in a DataAnnotations test, when the spec's Pattern C assumes a fixed per-Op-Group config
metadata:
  type: feedback
---

When a DataAnnotations attribute takes constructor config (scale, precision, cidr, …) and the
fixture's `RuleScenario<T>` tuples vary that config **per scenario**, box the whole tuple into
`DataAnnotationCase.Value` with `ToDataAnnotationCases(v => (object?)v, expectedFactory)` and
destructure it in the test's `// Arrange` block:

```csharp
var (value, scale) = ((decimal? value, int scale))tc.Value!;
var attr = new ScaleAtMostAttribute(scale);
```

**Why:** `docs/ai/specs/data-annotations/unit-test.md` Pattern C assumes the config is fixed for the
whole Op Group (a `static readonly` field on it, `.Except(...)` for the rows that disagree). That
works for fixtures like `IsInCidr` where only the value column varies. It cannot express
`DecimalRulesFixtures.HasMaxScale`, where every row carries its own scale and four rows exist purely
to exercise *invalid configuration*. Fixing the config would force either duplicating fixture data
inline or dropping the configuration rows — both worse than boxing. `AssertResult` only reads
`tc.Expected`, so nothing downstream cares what shape `Value` is.

**How to apply:** only when the config genuinely varies per row. If a single config value serves the
whole group, stay on Pattern C — it reads better. Landed in the Batch D Decimal attributes
(`DecimalAttributesTests`); see [[must-codes-catalogue]] for the sibling rule that the attribute's
`Code` is the clause's *terminal* semantic code, not the configuration-guard code, even on rows that
fail configuration validation.
