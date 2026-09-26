<!-- metadata_header
type: plan
id: technical-readiness-to-1.0-w18
version: 1.2
status: planned
last_updated: 2026-09-26
-->
# W18 — Culture, time and deterministic behavior

> **Mandatory completion gate:** This workstream and every major block inherit the [knowledge, documentation and tooling closure contract](../references/knowledge-and-tooling-closure.md). Save evidence-linked lessons/decisions (or an explicit no-new-lesson outcome), synchronize all relevant BRAIN/Markdown/indexes, build or update required supporting scripts/tools and test/run them (or record reviewed N/A), then complete full Astra review → feedback → fix → recheck. Persist the impact matrix, feedback and owned commit evidence; reopen the unit when relevant context changes. Feature implementation alone cannot close this work.

## Purpose

W18 establishes reproducible behavior across supported environmental conditions, particularly the early string-to-DateOnly past-date pilot. The handoff and v1.6 §§6–8 identify environment-sensitive correctness as a review dimension; specific clock/culture rules remain Astra decisions.

Inputs: W01 semantics, W04 path/message/order policies and W05 scenarios. Luna inventories culture/time/date/randomness/environment dependencies and exact test paths. Output: `docs/reports/readiness-determinism.md`. D16/D22 must be resolved before normative expectations or abstractions are introduced.

## Tasks

| Task | Action | Output |
|---|---|---|
| 18.1 | Trace ambient culture, comparisons/parsing, date/time acquisition, timezone/calendar and output ordering | Repository-wide dependency inventory and scope map |
| 18.2 | Define invariant-versus-culture-aware rules and clock/timezone semantics | Astra decisions, explicit defaults and required injection/control points |
| 18.3 | Author independent cases for selected supported cultures and time boundaries | Deterministic oracle tables, including today/past/future behavior |
| 18.4 | Implement shared abstractions only where necessary to satisfy chosen semantics | Global source/test diff with no local ambient-state patches |
| 18.5 | Exercise culture switching, calendar/date boundaries, ordering and repeated execution | Reproducible matrix with isolated/restored process state |
| 18.6 | Document environment contracts and connect CI coverage | W20 contract text and W00 evidence/artifact paths |

## Case-selection guidance

Select cultures with materially different parsing/casing behavior; do not enumerate many cultures without a reason. DateOnly semantics must specify how today is obtained and whether timezones/calendars are relevant. DST and UTC/local cases belong only where the chosen date/time contract uses those concepts. Determinism requirements must distinguish stable contractual output from intentionally environment-aware behavior.

## Acceptance

No test depends accidentally on wall-clock date, developer locale or nondeterministic collection order. Replaying a case under its declared environment reproduces its outcome. Any environment-dependent behavior is explicit and independently tested.100% validation coverage includes branches introduced for environmental behavior; exclusions cannot hide them.

## Controls

Checkpoint after inventory and semantic decisions. Sol implements approved shared mechanisms; Luna reads and executes evidence collection under approved commands; Astra reviews. Stop for global test-state leakage, speculative clock abstractions unused by the chosen design, undocumented culture defaults or flaky assertions repaired through retries. W19 owns concurrency interaction with ambient state; coordinate exact file ownership rather than editing the same harness concurrently.

Follow the [execution protocol](../references/execution-protocol.md), [decision register](../references/decision-register.md), [quality constitution](../references/quality-constitution.md) and [estimates and waves](../references/estimates-and-waves.md). This child remains Planned.

## Worked code example

This example is documentation, not an implemented PineGuard change. Its classification, dependencies and verification scope are stated below; complete source and reproduction instructions are in [Code examples](../references/code-examples.md).

```csharp
var saved = CultureInfo.CurrentCulture;
try
{
    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
    Checks.Require(Boundary.Invoke("1").Status == 200, "invariant parse");
    Checks.Require(DateOnly.FromDateTime(new FixedClock().GetUtcNow().UtcDateTime) ==
        new DateOnly(2030, 1, 2), "fixed UTC day");
    Checks.Require(DateOnly.TryParseExact("2030-01-02", "yyyy-MM-dd", CultureInfo.InvariantCulture,
        DateTimeStyles.None, out var date) && date == new DateOnly(2030, 1, 2), "date conversion");
}
finally { CultureInfo.CurrentCulture = saved; }
Checks.Require(ReferenceEquals(CultureInfo.CurrentCulture, saved), "culture restored");
```

Purpose: finally restores ambient culture, fixed TimeProvider provides UTC date, ISO string converts independently. fr-FR+`1`→200; fixed clock→2030-01-02; `2030-01-02`→same DateOnly; saved culture restored even on throw. Use existing FixedTimeProvider fixture for actual DateOnlyRules cases. Status pending.
