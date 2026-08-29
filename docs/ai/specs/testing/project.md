---
spec:
  id: pineguard.ai.testing.project-spec
  title: "PineGuard.Testing Project Spec (Shared Test Infrastructure)"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../project.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "tests/PineGuard.Testing/**"
---

# PineGuard.Testing Project Spec (Shared Test Infrastructure)

This document governs **`tests/PineGuard.Testing/`** — the shared test-infrastructure library
consumed by every `*.UnitTests` project and **shipped as a NuGet package**. It closes the gap
where the other five projects each have a `project.md` but the Testing library's own public
surface had no normative owner.

What this file does NOT restate:

- The base-class / case-record / Expected-type architecture — canonical in
  [`fixture.md`](fixture.md) (§1–§6) and [`unit-test.md`](unit-test.md) §2.2.
- How consuming test projects use the library — [`unit-test.md`](unit-test.md).
- Coverage targets — [`coverage.md`](coverage.md) (the `Testing` scope runs
  `tests/PineGuard.Testing.UnitTests/`).

## 1. Position in the solution

- Lives under `tests/`, but it is a **shipped library**, not a test project: it contains no test
  methods and is never executed directly. Its own tests live in
  `tests/PineGuard.Testing.UnitTests/`.
- Targets `net8.0` and `net10.0` only (its fixtures use `TimeOnly`); it does not target
  `netstandard2.1` like the five `src/` packages.
- May reference the `src/` packages it builds fixtures for; nothing under `src/` may ever
  reference it (`../dependencies.md`).

## 2. Public surface — namespaces and folders

| Namespace | Folder | Contents |
|-----------|--------|----------|
| `PineGuard.Testing.Common` | `Common/` | Abstract/shared types (`IExpectedResult`, `ReturnExpected`, `ThrowExpected`, `ExpectedException`) |
| `PineGuard.Testing.UnitTests` | `UnitTests/` | Layer-agnostic helpers (`BaseUnitTest`, `ValueCase`, `ThrowsCase`, `ThrowsCaseAssert`, soft-deprecated `IsCase<T>`/`HasCase<T>`) |
| `PineGuard.Testing.UnitTests.{Rules, MustClauses, GuardClauses, FluentValidation, DataAnnotations}` | `UnitTests/<Layer>/` | One folder per layer: its `*Expected`, `*Case`, scenario-extension and base-test types |
| `PineGuard.Testing.Fixtures` | `Fixtures/` | `XxxRulesFixtures` partials mirroring `src/PineGuard.Core/Rules/` (`fixture.md` §10) |

One type per file; file name matches type name (`../project.md` §2.1). New layer-specific types
go in that layer's folder — never in `Common/` unless genuinely shared by two or more layers.

## 3. Rules for extending the library

1. **Additive, consumer-driven design.** A new helper enters this library only when at least two
   `*.UnitTests` projects need it, or when a spec (`unit-test.md`, `fixture.md`) mandates it.
   One-off helpers stay in the consuming test project.
2. **Follow the established type families.** A new layer support set means the full family —
   `XxxExpected` (extending `ReturnExpected` or `ThrowExpected`), `XxxCase` (extending
   `ReturnCase<,>`), a `.ToXxxCases()` scenario extension, and a `BaseXxxUnitTest` with
   `AssertResult` — named and shaped per `fixture.md` §1/§3/§4/§6.
3. **Breaking changes ripple to 13,000+ tests.** Treat every public-signature change as a
   migration: plan it, batch it, and update `fixture.md`/`unit-test.md` in the same change.
4. **Deprecation is soft by default.** `Directory.Build.props` sets `TreatWarningsAsErrors`, so
   `[Obsolete]` on a widely-derived type breaks every consumer at once. Mark superseded types
   with `[Description("Use X instead.")]`, document the replacement in `fixture.md`, and treat a
   hard removal as its own migration (`fixture.md` §3 — `IsCase<T>`/`HasCase<T>` precedent).
5. **Determinism.** Helpers must be deterministic (`../spec.md` §5.3): no wall-clock, no
   ambient culture, no unseeded randomness. Randomness goes through
   `CreateDeterministicRandom(seed)`; culture through `UseCulture(...)`.
6. **XML docs are NOT enforced here.** The project sets `<NoWarn>CS1591</NoWarn>` by design —
   the public surface is thousands of self-describing fixture constants, and there is
   deliberately no `/document-testing` command (`docs/ai/agents/document-all.md`). Document the
   framework types (`Common/`, `UnitTests/`) where it genuinely helps; never doc-comment fixture
   constants.

## 4. Verification

- Build: `dotnet build PineGuard.slnx`.
- Tests: `/test-testing` (runs `tests/PineGuard.Testing.UnitTests/`).
- Coverage: `/coverage-testing` — 100% line and branch, like every other scope
  ([`coverage.md`](coverage.md)).
