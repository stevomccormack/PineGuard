# Test Writer Memory

> **Role:** `docs/ai/roles/verifier.md` (Verifier)
> Directives: Trust Nothing, Coverage Matters, Isolation, Test the Contract, CI-Ready Evidence.
> Constraints: No modifying `src/` just for assertions. No flaky tests.

## Topic Files
- [core-test-structure.md](core-test-structure.md) — Test Class + TestData structural rules, naming, ordering, MemberData wiring (unit-test.md §4-5)
- [expected-and-assertions.md](expected-and-assertions.md) — Uniform `Expected` naming, per-layer Expected types, composite records, exact assertion lines per layer
- [coverage-and-mistakes.md](coverage-and-mistakes.md) — 100% line+branch coverage requirements and recurring mistakes to avoid
- [fixture-architecture-v2.md](fixture-architecture-v2.md) — v2 Fixture/Case type hierarchy, extension methods, flat v2 Tests pattern, CallerArgumentExpression fix, reflection-based generics
- [guard-testdata-patterns.md](guard-testdata-patterns.md) — Guard-layer quirks: string DateOnly/TimeOnly inline pattern, `.Except()` import, nullable fixture mapping, inversion rule, precision mismatches, positive variants
- [fact-to-theory-conversion.md](fact-to-theory-conversion.md) — Converting overload-resolution `[Fact]`s to `[Theory]` via `Func<TResult>` case Value; avoid nested Op Group names like `GetHashCode`/`Equals` (CS0108)
- [utils-layer-test-pattern.md](utils-layer-test-pattern.md) — Utils/"Other"-layer tests use plain `BaseUnitTest` + `ReturnCase`/`ThrowsCase` (not `RuleCase`); `_BehavesAsExpected`/`_ThrowsAsExpected` split; best precedent is `MustCodesTests.cs`, not most existing `Utils/*Tests.cs`; trace the Rule's short-circuit chain before assuming a Utils class needs its own direct test file for coverage
- [plain-object-model-test-pattern.md](plain-object-model-test-pattern.md) — MustFailure/MustValidationResult/MustValidationException use the same "(Other)" BaseUnitTest pattern; array-vs-IEnumerable overload disambiguation via typed locals; MustFailure.ToString() leaks Value despite the XML doc's PII-safety claim
- [host-composition-test-pattern.md](host-composition-test-pattern.md) — Testing composition with a Microsoft.Extensions.* extension the package never calls itself (e.g. ValidateOnStart()/IStartupValidator via a real HostBuilder + Assert.ThrowsAsync(Type, Func<Task>)); confirmed empirically, reusable for Phase 3/4
