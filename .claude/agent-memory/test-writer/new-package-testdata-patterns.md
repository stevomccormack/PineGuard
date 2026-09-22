---
name: new-package-testdata-patterns
description: Patterns for testing a brand-new package (no PineGuard.Testing fixtures yet) — hand-built cases, namespace-visibility rules, DA attributes needing ValidationContext services, and empirically grounding message assertions.
metadata:
  type: feedback
---

### Hand-built cases are correct when no fixture exists yet
When a new package's domain type (e.g. `XmlSchemaSet`) doesn't belong in `PineGuard.Testing` (would
force that dependency on every layer), its sample data lives in a project-local class instead (e.g.
`tests/Xxx.UnitTests/Schemas/TestSchemas.cs`). TestData then builds `MustCase<T>`/`GuardCase<T>`/
`DataAnnotationCase` entries by hand (`new(nameof(TestSchemas.Foo), TestSchemas.Foo, new XxxExpected(...))`)
instead of `.ToXxxCases()` off a `RuleScenario<T>[]`. Still obey ValidCases/InvalidCases (Must/Guard) or
single `Cases` (DA) split rules, dataset-per-line, `nameof` for names. This is not a workaround — it's
the correct shape when fixture.md §9.6 ("one-off, not shared across layers") applies to the new package
itself.

### C# enclosing-namespace visibility saves imports — but only for ancestors
A test project's own namespace (e.g. `PineGuard.Xml.UnitTests`) automatically sees types in its
*enclosing* namespace (`PineGuard.Xml`) with no `using` — that's why `MustXmlClausesTests.cs` (namespace
`PineGuard.MustClauses.UnitTests`) never imports `PineGuard.MustClauses`. It does NOT extend to sibling
or child namespaces: a *new* package's test project must add explicit `using PineGuard.MustClauses;` /
`using PineGuard.GuardClauses;` for `Must.Be`/`Guard.Against`/`MustResult<T>` (those live in Core, not an
ancestor of the new package's namespace), and an explicit `using <Package>.UnitTests.Schemas;` (or
wherever) for a child namespace like a local `TestSchemas` helper.

### DataAnnotations attribute needing a `ValidationContext` service (e.g. resolved via `GetService`)
Don't force every case in an Op Group's `Cases` through one fixed `ValidationContext` — different service
configurations (e.g. schema present vs. absent, options present vs. default-fallback) are different
Operation Groups, each building its own `ctx` once in the test method's Arrange step and iterating its
own `Cases`. The `IServiceProvider` test double itself is a "test double implementing an interface" per
fixture.md §9.7 → define it as a `private sealed class` in the TestData file's shared-fields section
(§4.6), expose configured instances as `public static readonly IServiceProvider` fields, and reference
them from the Tests file's Arrange (`new ValidationContext(new object(), TestData.SomeProvider, items: null)`).
Missing-service and non-string-value throw scenarios don't need the provider at all — use the DA addendum's
Pattern E (`ActionThrowsCase` + `IThrowsCase`) with a bare `new ValidationContext(new object())`.

### Ground exact message assertions empirically before hardcoding them
When a Must-clause failure message embeds a dynamically computed value (element path, violation count,
namespace URI) that depends on validation-engine internals, don't guess the string. If a throwaway `[Fact]`
spike test already exists in the project logging the real `Kind`/`Path`/`Message` (e.g. left behind by a
sibling agent), run it (`dotnet test --filter ... --logger "console;verbosity=detailed"`) and read the
actual `Standard Output Messages` — don't delete or convert that spike (it's not one of your files).
Re-run it again after any concurrent source fix lands, since path-building logic is exactly the kind of
thing such a fix changes (a "which element does this violation's path end at" tweak silently changed a
warning's path from `Document/SplmtryData` to `Document/SplmtryData/Extra` mid-task here). For a
`ValidXmlAttribute`-style DA wrapper with no `context.MemberName` set, `ValidationContext.DisplayName`
falls back to `ObjectType.Name` — for `new ValidationContext(new object())` that's the literal string
`"Object"`, which gets substituted for the `{paramName}` token in the message.

### Concurrent-source-fix protocol (multi-agent same task)
If a coordinator message says another agent is actively editing the very `src/` files under test: don't
edit `src/` yourself (even to fix an obvious mid-edit compile error), and don't burn the whole tool budget
retrying `dotnet build` in a tight foreground loop — launch one bounded `run_in_background` poll
(`until dotnet build ...; do sleep 15; done`) and wait for its completion notification instead of manually
re-invoking `dotnet build` every turn. Re-verify every empirically-grounded assumption (exact messages,
paths, throw ordering) after the poll reports success, since the whole point of the concurrent fix was to
change that behavior.
