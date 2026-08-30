---
name: fluent-adapter-nuances
description: Writing a new Fluent*Extensions file — which null convention to follow when the repo shows two, how a config-param failure message reaches the user, and how to add extra scenarios without a second dataset
metadata:
  type: feedback
---

Three things about the FluentValidation adapter layer that are not obvious from reading a
neighbouring file, because a neighbouring file may be the wrong model.

**A new file uses null-skip, even though some existing files do not.**
`docs/ai/specs/fluent-validation/project.md` §5 is normative: a reference-type property that is
`null` must return `MustResult<T>.Ok(null!)` and skip the clause entirely —
`val is not null ? Must.Be.X(val, …, paramName: null) : MustResult<T>.Ok(null!)`. But
`FluentFilePathExtensions` and `FluentCsvExtensions` pass `val` straight through to a clause that
fails on null, and their XML docs say so ("If the value is null, validation fails"). Those are
pre-existing deviations, not the pattern.

**Why:** the same null header is a Must failure, a Guard throw, and a Fluent pass; the layer's job is
FluentValidation UX, where presence is expressed by chaining `.NotNull()`. Copying `FilePath` because
it is the nearest domain neighbour silently forks that behaviour.

**How to apply:** follow §5 and the `FluentStringExtensions`/`FluentBufferExtensions` shape (20+ methods
use it). The test-data consequence is the null scenario mapping to `new FluentExpected(true)` even
though the identical fixture scenario is an invalid case at Must and Guard. It also gives the ternary
two coverable branches, both of which a fixture with a null scenario exercises for free.

**A clause that attributes a failure to a config parameter produces an already-formatted message.**
`MustResult.Fail(code, template, paramName, value)` calls `MustMessage.Format`, which substitutes
whenever `paramName` is non-empty. A config-param early return passes `nameof(theConfigParam)`, so by
the time `FluentExtension.MustBe` runs its own `{paramName}` replacement there is no token left and the
property's display name never appears. Expect the literal config message in the test data
(`"extension must have a registered file signature."`), not `"Value must …"`. Only the clause's real
check — which passes the adapter's `paramName: null` — keeps the token for the property name.
DataAnnotations inherits this verbatim: `ValidationAttributeBase.BuildFailureResult` also does a plain
`{paramName}` `Replace`, so a config-param failure surfaces the same config-named message there, with
the attribute's own fixed `Code` still asserted beside it.

When a clause has **several** config guards (a bound pair's `min`/`max`, plus an inverted-range check),
each gets its own named switch arm above `_ when s.IsValid`, each with its own literal message
(`"min requires a non-negative minimum count."`, `"max requires a non-negative maximum count."`,
`"min requires a valid count range."` — the range check is attributed to `min`). The complement's
switch repeats those arms unchanged except for the swapped `Code:`, because a config guard fires
before the check is negated and so fails identically in both directions.

**Which file a new adapter goes in mirrors the Must layer's file split.** Clauses living in
`MustStringClauses` append to `FluentStringExtensions`; a clause file that is its own sub-scope type
(`MustStringGraphemesClauses`) gets its own `FluentStringGraphemesExtensions`, even though the property
type is still `string?` and the domain is still "string". Reading the two files side by side is the
point, so method order follows the Must file — each positive immediately followed by its complement —
not alphabetical and not all-positives-first.

**Extra scenarios fold into `Cases`, they do not get their own dataset.**
`docs/ai/specs/fluent-validation/unit-test.md` explicitly forbids `ValidCases`/`InvalidCases`/etc. at
this layer (Must and Guard allow them; Fluent does not). To add a config-failure or other off-fixture
group, spread two `ToFluentCases(...)` calls into the one `Cases` property with a collection
expression — `TheoryData<T>` is spreadable into `TheoryData<T>`, which is also how the Guard layer
joins datasets. See [[MEMORY]] for the per-layer dataset rules and [[must-codes-catalogue]] for why
those extra cases still assert the rule's build-time code rather than the code the clause actually
returned.
