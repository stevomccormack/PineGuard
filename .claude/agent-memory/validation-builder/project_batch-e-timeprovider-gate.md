---
name: batch-e-timeprovider-gate
description: Batch E's clock injection is done for every net8-gated temporal family; what remains is blocked on a Tier 1 Microsoft.Bcl.TimeProvider package edit, plus one unresolved leap-day semantic in the minimum-age row
metadata:
  type: project
---

Batch E (`rules-temporal`) splits by target framework, not by concept, and only one side of that
split can be built without owner sign-off.

**The retrofit's design is NOT open.** Plan 05 §3 (Batch E preamble) and §2's "One deliberate
signature change" bullet between them pre-decide every dimension: the exact parameter
(`TimeProvider? timeProvider = null`), its position per layer (Core trailing; Must immediately
before `paramName`; Guard and Fluent before `message`; DataAnnotations takes no parameter and
resolves the provider off `ValidationContext` instead), the null-means-system default routed
through a single `DateTimeUtility.GetUtcNow`, a grep-derived member work list, and the
breaking-change posture. An agent that reports this as "ambiguous" has not read §2's bullet, which
is where the policy half lives — it is easy to miss because it sits in the conventions section,
far above the Batch E table.

**What actually blocks the rest** is that `TimeProvider` does not exist on netstandard2.1. The
dividing line is per source file, not per project:

- **Already `#if NET8_0_OR_GREATER`-gated in full** (`DateOnlyRules`, `StringRules.DateOnly`,
  `DateTimeUtility.GetUtcNow` itself): giving these a clock forks nothing, because the whole
  vertical is net8+ already. **Done** — past/future in `fb699b3`, `HasMinimumAge` in `fb95c0a`.
- **Compiles for netstandard2.1** (`DateTimeRules`, `DateTimeOffsetRules`,
  `StringRules.DateTimeOffset`, and the `DateTimeRules.HasMinimumAge` variant Plan 05 asks for in
  the same row as the DateOnly one): these need `Microsoft.Bcl.TimeProvider` in
  `Directory.Packages.props` + the Core csproj. Plan 05 §5's W6 calls that a Tier 1 edit and says
  to "state the lines first"; the orchestration plan's §4 separately carves new external
  dependencies out of this program's standing commit authorization. Gating them net8+ instead is
  the per-TFM API fork Plan 05 §7's risk table explicitly names as the rejected alternative.

**Why:** the owner reserves new-dependency decisions even under the New Surfaces blanket commit
authorization, and Core advertises a "first-party BCL packages only" policy in its own package
description — so adding any reference is a product-surface commitment, not an implementation
detail.

**How to apply:** do not open the netstandard2.1 half until the package line is confirmed; report
it as an authorization gate needing a yes/no, never as an ambiguity needing a Fable consult — they
call for opposite responses from the orchestrator. `DateTimeUtility.GetUtcNow` carries a comment
saying lifting its one `#if` is the whole of the change when the reference lands.

**One unresolved semantic, deliberately shipped and flagged.** Plan 05's minimum-age row states the
boundary as `today.AddYears(-years) >= value` and then glosses it as making a 29-Feb birthday
mature on 28 Feb in a non-leap year. The formula does not do that — shifting a 28 Feb back by whole
years lands on a 28th, which precedes the 29th the birth date carries, so maturity falls on 1 Mar.
`fb95c0a` ships the formula (it is the normative half of the row, matches the age idiom .NET code
overwhelmingly uses, and is the conservative reading for a permission gate), documents 1 Mar as the
actual behaviour, and pins it with a named test,
`HasMinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch`. If the owner prefers the gloss, the flip
is one expression (`value.AddYears(years) <= today`) plus that test's expectations — but the Must,
Guard, Fluent and DataAnnotations layers built on top will inherit whichever reading stands, so it
is worth settling before they land.

Related: [[layer-signatures]], [[project_no-backcompat-before-first-release]].
