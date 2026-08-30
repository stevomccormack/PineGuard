---
name: result-bridge-layer
description: Result bridges (ErrorOr/FluentResults/OneOf) sit outside the Utils→Rules→Must→Guard→Integrations stack — how my standing layer invariants do and don't apply to them
metadata:
  type: project
---

# Result bridges are not an Integrations layer

The `PineGuard.ErrorOr` / `PineGuard.FluentResults` / `PineGuard.OneOf` micro-packages are a
**fourth family** alongside the classic layer stack, not another Integrations entry beside
FluentValidation and DataAnnotations.

**Why:** an Integrations adapter *authors a validation call* and therefore has to obey "Must owns
messages, reuse via `paramName: null`". A bridge authors nothing — it takes an already-finished
`MustResult<T>` / `MustValidationResult` / `MustFailure` and re-shapes it into a third-party result
type. There is no message to own, no clause to call, no `paramName` decision to make. Applying the
Must-owns-messages invariant to a bridge produces nonsense (there is no Must call to defer to).

**How to apply:**
- My "Layer order" and "Must owns messages" invariants are silent on bridges. Do not force-fit them.
- The invariants that *do* bind: 100% line + branch on both TFMs, no IO, and no bridge referencing
  another bridge (each is standalone, `PineGuard.Core` + its own target package only).
- The real contract to check a bridge against is Plan 04 §3.2–3.5 in
  `docs/ai/plans/new-surfaces-missing-validation-cases-04-mediatr-result-bridges.md`: exact method
  signatures per package, camelCase metadata keys (`"code"`, `"propertyPath"`) in every bridge,
  `global::`-qualified target types (the package namespace shadows the library's own simple names),
  and a `null` successful `Result` crossing as `default!` rather than becoming a failure.
- Each package's own `README.md` was written at scaffold time and is a faithful second source for
  the intended behaviour — worth diffing an implementation against before assuming a drafted
  adapter is wrong.

Test shape for these lives with the other per-layer notes in [[test-data-shapes]]; the bridges use
`BaseUnitTest` directly with project-local `XxxCase`/`XxxExpected` records rather than any
`PineGuard.Testing` layer base class, because no layer base class fits them.
