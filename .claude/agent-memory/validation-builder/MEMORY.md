# Validation Builder Memory

> **Role:** `docs/ai/roles/builder.md` (Builder)
> Directives: Follow the Spec, Pick the Right Layer, Always Valid State, Clean Code, Verify Locally.
> Constraints: No architectural changes without approval. No broken builds. No IO in Core.

## Invariants (always in context)

- Layer order: Utils → Rules → MustClauses → GuardClauses → Integrations (Fluent / DataAnnotations).
- Must owns messages. Guard/Fluent/DataAnnotations reuse them via `paramName: null`.
- Guard calls Must; never duplicates logic. Core is pure — no user-facing messages, no IO.
- 100% line + branch coverage on new code, both TFMs.
- Dependency chain: `Core Utils → Core Rules → MustClauses → { GuardClauses, FluentValidation, DataAnnotations }`.

## Topic Files

- [Layer signatures](layer-signatures.md) — per-layer method shape, parameter ordering, the parsed-result contract, enum-config pass-through, DA `init`/netstandard2.1 trap, recurring mistakes
- [Test data shapes](test-data-shapes.md) — fixture-v2 Case/Expected types, per-layer dataset rules, tuple-fixture and DA-config traps, null differing at every layer
- [MustCodes catalogue](must-codes-catalogue.md) — wiring codes into Must + Fluent + DataAnnotations: arg positions, one-clause-one-code, bound/complement code swapping, the Rule13 domain map a new clause file needs
- [Batch D vocabulary aliases deferred](project_batch-d-vocabulary-aliases-deferred.md) — ScaleAbove/PrecisionAbove shipped without their vocabulary.json rows; docs/ freeze, not a defect
- [DA per-scenario config](data-annotations-per-scenario-config.md) — box the fixture tuple into `DataAnnotationCase.Value` when config varies per row; Pattern C only fits a fixed config
- [Rule06/Rule08 publish defect](project_audit-rule06-rule08-publish-defect.md) — those audits fail on a multi-TFM `dotnet publish` bug, not your code; orchestrator needs pwsh 7
- [Must complement test wiring](must-complement-test-wiring.md) — projecting one fixture group into a positive/`Not*` pair: the null-value case that can't be inverted, `Except`/`Only`, legacy `BaseUnitTest` migration
- [Fluent adapter nuances](fluent-adapter-nuances.md) — the two null conventions in-repo and which is normative, config-param messages arriving pre-formatted, folding extra scenarios into `Cases`, unconsumed validators hiding gaps
- [Result bridges are not Integrations](result-bridge-layer.md) — why "Must owns messages" doesn't bind on ErrorOr/FluentResults/OneOf, and what does
- [Batch E TimeProvider gate](project_batch-e-timeprovider-gate.md) — net8-gated families are clock-injected already; the netstandard2.1 ones wait on a Tier 1 package edit, plus one open leap-day call
- [TestServer end-to-end cases](testserver-integration-tests.md) — capture a framework-written body before writing its expectations; where the `EndToEnd` group lives
- [Analyzer package verification](analyzer-package-verification.md) — Info diagnostics never print in `dotnet build`; consumer smoke projects under `artifacts/` need local props stubs
