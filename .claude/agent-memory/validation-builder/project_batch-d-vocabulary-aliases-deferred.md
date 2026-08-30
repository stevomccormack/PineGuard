---
name: batch-d-vocabulary-aliases-deferred
description: Batch D's ScaleAbove/PrecisionAbove Guard names need vocabulary.json aliases that were deliberately NOT added because of the standing docs/ freeze
metadata:
  type: project
---

Phase 5 Batch D shipped `Guard.Against.ScaleAbove` and `Guard.Against.PrecisionAbove`
(commit `7930f01`) **without** the `docs/ai/specs/language/vocabulary.json` alias rows
(`ScaleAbove -> ScaleAtMost`, `PrecisionAbove -> PrecisionAtMost`) that the Batch D plan
row calls for.

**Why:** the New Surfaces orchestration plan carries a standing owner instruction
(2026-08-30) forbidding any edit under `docs/` except `docs/ai/plans/`, and it explicitly
overrides phase-plan W-step instructions to touch specs. `vocabulary.json` is a spec file.
The same instruction says a gate depending on the deferred work must not block the unit —
skip it and record the skip.

**How to apply:** if a Guard/Fluent naming-parity audit rule flags `ScaleAbove` or
`PrecisionAbove` as an un-mapped synonym, that is this known gap, not a naming defect —
the names are the plan's own. Add the two alias rows when the batched `docs/` cascade is
finally run, alongside every other unit's deferred Brain/adapter work.

Related: [[must-codes-catalogue]]
