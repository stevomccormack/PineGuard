---
name: temporal-phase1-audit
description: Design-fitness review of Temporal Program Phase 1 (codex/temporal-foundation @ 6b3cab3, Nov 2026) — today derivation, clock injection, TimeOnly wrap, TimeSpan rename, precision enums, Testing clocks; what was found sound/weak and the owner decisions the code made silently
metadata:
  type: project
---

Phase 1 of the Temporal & Time-Zone Program was reviewed for design fitness on 2026-11-08
(worktree `C:\Users\stevo\.codex\worktrees\temporal-foundation\PineGuard`, branch
`codex/temporal-foundation`, HEAD 6b3cab3). Verdict given: FIT WITH CAVEATS.

**Why:** the owner asked "is anything NOT WORKING / WEAK / AMBIGUOUS" before merging Phase 1 as
foundational 1.0 semantics; correctness-of-tests was a separate reviewer's job.

**How to apply (patterns to re-check in later temporal phases):**
- "Today" = `DateTimeUtility.TryGetTodayDate` (internal): one `GetUtcNow` sample, `provider.LocalTimeZone.GetUtcOffset(instant)`, checked `DateTimeOffsetUtility.TryGetDateAtOffset`. Null provider -> system clock -> UTC calendar date (D-1 ratified by the owner 2026-11-09 on the 2026-11-08 council's recommendation; the interim Phase 1 code used the HOST zone and was reverted); an injected provider's LocalTimeZone is honoured. Documented in XML remarks as "PineGuard never guesses the host's zone". All five layers reach it via Must; DA resolves `TimeProvider` from `ValidationContext.GetService`. Verify any new today-relative rule still goes through this helper and not `GetLocalNow`.
- Production ergonomics gap: there is no BCL `TimeProvider` with a settable `LocalTimeZone`, so "today in <user zone>" requires a custom `TimeProvider` subclass; no `TimeZoneInfo` overload exists for Past/Future/MinimumAge (D-14 explicitly promises none). Flag if a later phase claims zone-aware age.
- `DateTime` convention split (decided, A-03): instant predicates (`IsInPast`, `IsBefore`) treat `Kind.Unspecified` as UTC via `ToUtc`; calendar predicates (`HasMinimumAge`, `IsSameDay`, `IsWeekday`) use `.Date` and ignore Kind. Not documented at class level on `DateTimeRules` — only per-member.
- `TimeOnlyRules.IsBetween` wraps when `start > end`; equal endpoints = one instant (Inclusive) / none (Exclusive), never full day. `TimeOnlyRange` still rejects start > end — two TimeOnly range vocabularies coexist by design (A-09).
- Guard TimeSpan polarity: `Guard.Against.ShorterThan(v,t,incl)` -> `Must.Be.LongerThan(v,t,complement(incl))`; v == t passes under default Exclusive. Guard test data derives from `F.IsLongerThan` with the inclusion complemented in the tuple — remember this when reading Guard TestData.
- `TimePrecision.Tick` moved 4 -> 5 (Microsecond inserted). Enum values are inlined into consumer IL and attribute metadata; Standards (pinned alpha.7) must be recompiled in Phase 3 — binary mismatch produces no compile error.
- `IsWithinDaysFromNow` summary still says "from UTC now" — D-6 wanted "in the past or the future" but T-01 U2 said "do not touch"; charter-internal conflict, cosmetic, Phase 4 deletes the member.
- D-1 commit bda5704 (2026-11-09) reviewed APPROVE WITH REQUIRED FIXES: code/tests/XML docs correct; the stale bit was the `ci.yml` comment block above `core-null-provider-timezone` (still said "proves that fallback reads the host zone") — when a commit flips a contract, grep YAML/markdown comments near the renamed test, not just `<remarks>`. Also recurring: test comments cite "unit-test.md §6 (time-relative ops)" but §6 is Helper Types; determinism is §7. `GetLocalNow(null)` remains the one public host-zone read in Core; charter has not said whether it stays.
- `vocabulary.json` opposites list has no `LongerThan`/`ShorterThan` pair (nor GreaterThan/LessThan); not required by the parity audit but worth adding when the TimeSpan sign family lands in T-05.
