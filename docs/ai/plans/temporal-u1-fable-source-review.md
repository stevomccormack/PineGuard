# T-01 U1 source review — Claude Fable 5.1

**Outcome:** Required source fixes; this is not unit acceptance.
**Reviewer:** Claude Fable 5.1, fresh bounded read-only session `7ecbfc2e-3612-45de-86b4-bf2c6cbecb5e`.
**Date:** 2026-09-27. Only the five U1-owned source/test files and controlling Brain documents were reviewed. No files or tests were changed/run by Fable.
**Public result:** `artifacts/reviews/T1-U1-fable-source-review.json` in the temporal worktree.

## Required fixes

1. **Correct the half-hour fall-back XML.** In `tests/PineGuard.Testing/Common/TestTimeZones.cs`, the `HalfHourDaylightSaving` summary says the fall-back interval on 5 April 2026 is 02:00–02:29. With base offset +10:30, delta +00:30 and fall-back at 02:00 daylight time, the clock returns to 01:30 standard time; the repeated interval is 01:30–01:59. Correct only the prose. The plan's `HalfHourAmbiguousLocalTime` at 01:45 and test data are already consistent with the rule; this is not a contract change.

   Required summary clause: `In 2026 that is 4 October (02:00–02:29 do not exist) and 5 April (01:30–01:59 occur twice).`

2. **Fail fast when the declared platform-zone prerequisite is unavailable.** In `tests/PineGuard.Testing.UnitTests/Common/TestTimeZonesTests.cs`, remove the `TimeZoneNotFoundException` catch and early return from `PlatformSample_ClassifiesLocalTimes`. Keep its existing Act/Assert blocks. Update the XML summaries on both `PlatformSampleId_Resolves` and `PlatformSample_ClassifiesLocalTimes` to state that these host integration tests require the platform ID and fail, rather than skip, when host zone data cannot resolve it.

The resolution test's assertion and the fixture's “resolves on this host” contract make the zone an explicit prerequisite; silently returning from the classification test is inconsistent and cannot make the suite green because the resolution test independently fails. Keep host lookup isolated to these named tests and keep all DST semantic tests on custom zones.

## Conforming scope and remaining gates

Fable found the five-file implementation otherwise aligned with the U1 contract, including zone members/rules, deterministic custom-zone fixtures, test-data naming, and the extra `DateTimeOffset.Offset` assertion. The review did not accept U1. Format, compiler diagnostics, tests, coverage per TFM, audit, and ownership-diff checks remain pending. Optional notes (not blockers): add a shape row to pin the half-hour zone's +10:30 base offset; the platform resolve test's inline Throws assertion could match the existing paired ThrowsCase pattern, though the plan groups both outcomes in one test.
