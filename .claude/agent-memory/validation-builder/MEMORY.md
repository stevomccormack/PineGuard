# Validation Builder Memory

> **Role:** `docs/ai/roles/builder.md` (Builder)
> Directives: Follow the Spec, Pick the Right Layer, Always Valid State, Clean Code, Verify Locally.
> Constraints: No architectural changes without approval. No broken builds. No IO in Core.

## Always in context

- Layer order: Core Utils → Core Rules → MustClauses → GuardClauses / FluentValidation / DataAnnotations
- Must owns the messages; Guard/Fluent/DataAnnotations reuse them via `paramName: null`
- Guard calls Must and never reimplements logic; Core stays pure (no messages, no IO)
- 100% line **and** branch coverage on both TFMs is the gate — verify per-TFM, never net10.0 only
- Read the layer's `docs/ai/specs/<layer>/project.md` and `unit-test.md` fresh before implementing;
  the specs are authoritative and the topic files below only fill in what they assume

## Topic Files

- [Layer signatures](layer-signatures.md) — per-layer method shapes: param order, nullability, delegation, and the mistakes that break the coverage gate
- [Test data patterns](test-data-patterns.md) — one fixture into five layers: case/Expected types, Guard's two-dataset shape, tuple-null traps, config-param failures, DA config pinning
- [MustCodes catalogue](must-codes-catalogue.md) — wiring codes into Must + Fluent + DataAnnotations: arg positions, one-clause-one-code and its exceptions, fixed-at-build ErrorCode
- [Must error codes](must-error-codes.md) — the code grammar itself and the reflection/audit invariants that reject a sloppy constant
- [Must complement test wiring](must-complement-test-wiring.md) — projecting one fixture group into a positive/`Not*` pair: the null-value case that can't be inverted, `Except`/`Only`, legacy `BaseUnitTest` migration
