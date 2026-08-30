---
name: fluent-adapter-nuances
description: Writing a new Fluent*Extensions file — which null convention to follow when the repo shows two, how a config-param failure message reaches the user, how to add extra scenarios without a second dataset, and why an unconsumed validator class hides an uncovered extension
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

**Extra scenarios fold into `Cases`, they do not get their own dataset.**
`docs/ai/specs/fluent-validation/unit-test.md` explicitly forbids `ValidCases`/`InvalidCases`/etc. at
this layer (Must and Guard allow them; Fluent does not). To add a config-failure or other off-fixture
group, spread two `ToFluentCases(...)` calls into the one `Cases` property with a collection
expression — `TheoryData<T>` is spreadable into `TheoryData<T>`, which is also how the Guard layer
joins datasets. See [[MEMORY]] for the per-layer dataset rules and [[must-codes-catalogue]] for why
those extra cases still assert the rule's build-time code rather than the code the clause actually
returned.

**A declared-but-unconsumed `XxxValidator` is a silently uncovered extension.**
Adding a Fluent extension touches three places — the extension, a `Cases` group, a validator nested
class — but the `[Theory]` that ties them together is a fourth, and nothing fails if you skip it.
The validator is `private sealed`, so an unused one is not even a warning; the suite still goes green
with a higher test count than before, because the *other* groups grew. Only a full-scope
`-Enforce100` coverage run catches it, and if the batch runs that gate once at the end rather than
per commit, the gap can sit through several commits. (It did: `Base64Url` shipped in `4979580` with a
validator and a full `Cases` group and no test method.)

**Why:** the per-commit signal (build clean, tests pass, count went up) is indistinguishable between
"wired up" and "wired up except the theory". The coverage gate is the only real check.

**How to apply:** after editing a Fluent Tests file, count `XxxValidator` declarations against
`[Theory]` methods — they must be 1:1. Do this before committing rather than trusting the end-of-batch
gate. Same audit applies to any layer whose test entry point is a separate declaration from the
subject under test.
