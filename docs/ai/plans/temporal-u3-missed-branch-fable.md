# T1-U3 missed-branch ruling — Claude Fable 5.1

**Status:** Binding narrow test-vector decision. U3 implementation and gates remain in progress; this ruling is not unit acceptance.

**Review provenance:** Claude Fable 5.1, resumed source-review session `c5e314e0-c32a-4f3d-9bb3-0f684a289797`, 2026-09-27. The review used read-only repository tools with an explicitly empty MCP configuration. Only the public verdict is recorded here.

## Finding and decision

The Core net8 isolated coverage report showed 4,660/4,660 lines and 3,196/3,197 branches for `PineGuard.Core`; all 5,408 Core tests passed. The sole uncovered outcome is the `value > start` conditional in the exclusive midnight-wrap arm of `TimeOnlyRules.IsBetween`. Existing exclusive wrap cases exercise its false outcome at the start and end boundaries.

Add one scenario to [`TimeOnlyRulesFixtures.IsBetween`](../../../tests/PineGuard.Testing/Fixtures/TimeOnlyRulesFixtures.cs):

```csharp
public static readonly (TimeOnly? value, TimeOnly start, TimeOnly end, Inclusion inclusion) WrapLateNightInsideExclusive = (new TimeOnly(23, 0), new TimeOnly(22, 0), new TimeOnly(2, 0), Inclusion.Exclusive);
```

Add the corresponding row to `ValidScenarios`:

```csharp
new(nameof(WrapLateNightInsideExclusive), WrapLateNightInsideExclusive, true),
```

Place the declaration after `WrapEarlyMorningInside` and the row after its `ValidScenarios` entry. The expected result is `true`: 23:00 lies strictly after 22:00 in the open interval that wraps through midnight to 02:00. The existing Core `AllScenarios` projection carries this row into the Core test, so no TestData changes are required.

One scenario is sufficient for the reported branch: it exercises the missing true outcome of `value > start`. The instrumentation does not report `value < end` as another branch point, so an additional early-morning exclusive case is not required for coverage. No production logic, coverage exclusions, or adapter test data should change for this finding.

## Evidence

- Implementation: [`TimeOnlyRules.cs`](../../../src/PineGuard.Core/Rules/TimeOnlyRules.cs), `IsBetween`.
- Shared scenarios: [`TimeOnlyRulesFixtures.cs`](../../../tests/PineGuard.Testing/Fixtures/TimeOnlyRulesFixtures.cs), `IsBetween`.
- Original Core coverage report SHA-256: `50A07871FE110F2A9CC1F526EF2AEA4D8B9481980DB5322E2BFE000FB408593B`.
- Original report location: `artifacts/reviews/t1-u3-coverage/Core/net8.0/` (the Core unit-test Cobertura report under the retained run directory).
