<!-- metadata_header
type: evidence
id: temporal-review-council-rerun
version: 1.1
status: complete
last_updated: 2026-09-26
-->

# Revised temporal plan — council execution receipt

**Design authority:** Astra adjudicated A-01–A-12 under the owner's authorization. The genuine revised Fable council and corrective follow-ups preserve those decisions; no further owner row-signing is required. The historical council remains in `temporal-00-program.council-transcript.md`. The actual revised public outputs are in [`temporal-00-program.fable-council-transcript.md`](temporal-00-program.fable-council-transcript.md).

## Execution record

An initial local Claude CLI attempt used `--bare` and returned `Not logged in` with zero model usage. This was an invocation configuration failure, not evidence that Claude Fable was unavailable: `--bare` skips keychain reads and OAuth. It is retained as failed-attempt history only and is not counted as a council call.

The revised council then completed on 2026-09-26 through Claude Code 2.1.283 using the installed Claude Fable 5.1 route. The invocation omitted `--bare`, used `--permission-mode dontAsk`, an empty tool/MCP configuration, no session persistence and one turn. Five advisors, five randomized/anonymized peer reviewers and one chairman all returned successful calls with nonzero model usage. The exact model, provider, token counts and durations for each call are recorded alongside that call's public text in the transcript. Only public assistant text is persisted; hidden reasoning and thinking content are excluded.

The formal chairman verdict was **not accepted as written**. Follow-up #1's proposed separate `Window` API and Must host-ID lookup were retracted by follow-up #3. Follow-up #2 only described tool intent and produced no decision. Controlling follow-up #3 preserves A-01–A-12 and A-09 TimeOnly wrap semantics, rejects a separate Window family, and keeps zone-ID resolution at the DataAnnotations boundary with no Core/Must host lookup. Follow-ups #4 and #5 specify the test-only counting/advancing providers and the exact unknown-zone/null-ID message contract. The transcript records each correction and its precedence.

## Final correction review status

A separate full-context final plan review completed on 2026-09-26 with Claude Fable 5.1 and nonzero usage (cache-created input 215,010; cache-read input 4,020; output 27,283; duration 334,271 ms; CLI exit 0). Its verdict was **NOT ACCEPTED**: six critical defects and additional polish were reported. The complete public findings are reproduced in `temporal-00-program.fable-council-transcript.md`; the machine-readable stream `artifacts/fable-final-acceptance-2026-09-26.stream.jsonl` is retained for provenance and includes system metadata. The transcript excludes hidden reasoning.

That gate found an outdated receipt (now corrected here), duplicate planned `MustCodes.Date.Kind` constants (the current Core source already contains them; plans must reuse them), an audit-cohort issue for `Date.Reference.OutOfRange`, conflicting Phase 1 test-file ownership, missing T-05 code constants, and an underspecified A-10 half-hour transition fixture. Its assertion that Core lacked `InternalsVisibleTo("PineGuard.MustClauses")` was factually incorrect: the current `src/PineGuard.Core/PineGuard.Core.csproj` already grants that access. No new friend assembly attribute is needed. The plan must cite the existing grant. T-01 U2/U2b are being treated as one atomic acceptance cohort so the `must-codes` audit runs after the first consumers exist and before Phase 1 merge; no audit exemption is authorized.

The current revised plan text and actual source evidence must be included in the next Fable gate. Until a later public Fable verdict accepts the corrected plan set, do not mark P0 accepted, implementation Active, or verification passed. No production code, generated class, commit, push or release is authorized or claimed by this receipt.


## Final correction acceptance review — 2026-09-27

A real Claude Fable 5.1 final acceptance review ran with read-only repository tools against the live PineGuard and PineGuard.Standards worktrees. Provenance: Claude CLI session `e31dcbce-b2b5-4d0f-a83b-7f4d19f0dd9e`; provider `firstParty`; exit 0; 93 turns; duration 587,569 ms (API 585,645 ms). Model usage was input 290, cache-read 2,175,377, cache-created 396,195, output 42,534; web search requests 0; cost USD 10.59734425. Only Fable's public review is persisted in the transcript; no hidden reasoning is included.

Verdict: **NOT ACCEPTED**. The six prior blockers and Fable #3–#5 follow-up defects are fixed. One must-codes audit conflict remains: plans require cross-domain code references rejected by the unchanged mandatory audit. Fable's precise minimal correction reopens affected code-code ownership decisions, not A-01–A-12 generally and not by preference: use layer-local domain codes, put zone-taking clauses in MustTimeZoneClauses, define the required TimeZone and Date interval codes, and update `// Serves:` mappings. No audit exemption is allowed. Fable also requires accurate Qodana token/`-Linter auto` documentation, explicit owner authorization for merge/push under Safety Tier 1, and a concrete T1-U2b internal evaluation sequence. See the appended full public verdict in the council transcript.

The scanner gate remains Qodana + Sonar per the Brain; Qodana unavailability is an environmental prerequisite, and InspectCode is not an automatic equivalent. No scan pass, implementation activation, commit, push, release, or generation is claimed. P0 remains blocked pending Sol's plan corrections and a focused Fable acceptance review.

## Focused MustCodes audit correction ruling — 2026-09-27

Claude Fable 5.1 returned **ACCEPTED as corrected** for the focused audit-ownership ruling. This accepts only the enumerated corrective code-domain contracts; it does not accept the full temporal plan set or authorize implementation. The mandatory audit remains unchanged and no exemption or Brain deviation is approved. This ruling supersedes only the cross-domain code-ownership sentences it names; A-01–A-12 otherwise stand.

Provenance: Claude CLI 2.1.283, persistent session 7342dc25-a35f-42a2-a77e-3ea894c6b4eb, provider firstParty, exit 0, 32 turns, duration 234,465 ms, model usage input 162, cache-read 380,100, cache-created 137,773, output 20,727, web search 0, cost USD 3.888455. The ruling cites the audit implementation and relevant Brain/source precedents. Only the public result is retained in temporal-audit-code-ruling.md; private reasoning and intermediate tool payloads are excluded.

Sol must apply the listed changes in the affected plans. After those corrections and consistency checks, the remaining gate is focused Fable acceptance of the enumerated findings and exact plan diff. P0 remains planned pending that review. No implementation, production source changes, commit, push, release or generation is claimed.
## Final plan acceptance and corrective rulings — 2026-09-27

The focused final plan acceptance is **P0 NOT READY** at the reviewed revision. Its public closure matrix confirms the earlier six blockers, the prior corrections/polish, and the focused MustCodes audit ruling are closed. It then found three new executable-gate blockers (B1–B3) and four non-blocking polish items. Exact source citations and findings are in [temporal-final-fable-acceptance.md](temporal-final-fable-acceptance.md).

Fable's next binding audit-gate ruling and scope-fidelity addendum are in [temporal-audit-gate-ruling.md](temporal-audit-gate-ruling.md) and [temporal-audit-gate-followup.md](temporal-audit-gate-followup.md). They direct a narrow source-level audit model correction rather than baseline debt or broad suppression: a versioned per-concept layerAbsences declaration; strict validation that declared absences remain absent; corrected string-family Rules discovery for ordering; VIBE coverage; no new baselines; and per-phase cohort audit after the last adapter unit. They also restore the planned IsDaylightSavingTime capability as a complete Must/Guard/Fluent/DataAnnotations concept rather than dropping it for audit convenience. Exact rule descriptions, file paths, codes/messages, tests, gates and unit order are recorded in those public rulings.

At the time of this receipt update, Sol is applying plan-only corrections for B1–B3 and the remaining polish. The current reviewed revision remains not ready until those edits are complete and an actual focused Fable acceptance returns P0 READY FOR IMPLEMENTATION. No source implementation, tests, scans, commit, push, release or generation is claimed.

CLI provenance values are recorded exactly as returned by each resumed-session result. Their accounting scope is not independently verified, so they are not presented as independently measured per-call wall time, token usage or cost. Only public Fable results are retained; private reasoning and intermediate tool payloads are excluded.
## Final plan-readiness acceptance — 2026-09-27

Claude Fable 5.1 completed the focused final acceptance against the corrected PineGuard and PineGuard.Standards worktree documents. The public result and complete closure matrix are preserved in [temporal-final-fable-acceptance-2.md](temporal-final-fable-acceptance-2.md) (SHA-256 `49FBAA61E6DDBF51F7608E50F893152CC38C3CBFDA2BD18C98D0AAE00D9E3025`).

**Verdict: P0 READY FOR IMPLEMENTATION.** All seven finding groups close, including the follow-up decisions, B1–B3 audit-gate corrections, DST scope preservation and operational constraints. This is plan readiness only; it claims no implementation, tests, scanner pass, merge, release, generated classes or InspectCode gate substitution.

Fable identified two non-blocking editorial residues for the activation edit: T-00 §2's Phase 4/5 summary fan-out cells should match the dependency tables in T-04/T-05; T-01 should cite the existing MustClauses friend grant at PineGuard.Core.csproj line 19 only (line 20 is GuardClauses). Fable found neither residue changes execution because T-00 §0.1 and unit `Depends on` rows control dispatch. These corrections are being applied as editorial cleanup; they do not reopen the approved temporal decisions or block P0.

Provenance as reported by the resumed Claude Code CLI result: Claude Code 2.1.283, model `claude-fable-5-1`, provider `firstParty`, persistent session `7342dc25-a35f-42a2-a77e-3ea894c6b4eb`, subtype success, exit 0, 26 turns, duration_api_ms 1,348,461, total_cost_usd 19.94766475; ModelUsage input 682, cache-read 8,785,139, cache-creation 609,168, output 111,224; web searches 0. Resumed-session accounting scope is not independently verified; these are CLI-reported counters, not asserted as independent per-call time or usage. Only public Fable text is persisted; hidden reasoning and intermediate payloads are excluded.

## Post-acceptance editorial closeout and worktree fingerprint — 2026-09-27

Sol applied both editorial residues explicitly listed as non-blocking in the final Fable acceptance: the T-00 Phase 4/5 fan-out summaries now match T-04/T-05 dependencies, and T-01 cites only the MustClauses IVT grant line. T-00 §0 and D-3 status now cite the completed P0 READY verdict. T-00 metadata/README are Active; orchestration records P0 Done and T1-U0 Running under Claude Sonnet 5 via Luna_research. No implementation unit is marked Done by this status update.

The final acceptance artifact remains `temporal-final-fable-acceptance-2.md` (SHA-256 `49FBAA61E6DDBF51F7608E50F893152CC38C3CBFDA2BD18C98D0AAE00D9E3025`). The following hashes fingerprint the post-acceptance documents after only the two stated editorial corrections and activation/status metadata updates; they do not imply Fable reread those exact post-edit bytes:

| File | SHA-256 |
|---|---|
| `temporal-00-program.md` | `FA57C554CDE7A9CB5FBE2372DF548C50880CDD7151BED185660BD48139F602AB` |
| `temporal-01-foundation.md` | `1AF0AD905B6C284C4AF1626E3EBA449A303D44168A3C2F542A295CFAD41B5636` |
| `temporal-02-timezone-core.md` | `BF10383E59F8DE35012F0CB76996805F104152CE6F3B6DFD76378A8BBF07EAEB` |
| `temporal-03-release-standards-sync.md` | `7C9A0D3C0EE25E407F1F4DEA0D43471269624C0E1B769FAD3C7AFF4E87440CA6` |
| `temporal-04-validity-relative.md` | `F0B81A477002929CA97516EB79944941D93EB1B87ECFF50116F96F1C176A0207` |
| `temporal-05-durations-calendar.md` | `AC1C0115C259A67FB13444D1D0844A359BFE455E19856C51C72B4D0FC9FAB8AC` |
| `temporal-06-formats-epoch.md` | `E486EE647A3298CA1401D8E990401FC012DFDB6F575C0A88FA832C4FB90DBAB2` |
| `temporal-07-closeout.md` | `D9AEAD23FD9359D25019CB3E5227728B78A62451FAF2B6B55CE7089125F24484` |
| `temporal-astra-decisions.md` | `FD92225D4D72097E313445E3B6CB302799E854724BCE5B4C69BB86B0FEEF6F8A` |
| `temporal-orchestration.md` | `3FB54F9AFE9F3F34D0756E3E834FBA24C64A3B5549BA535A3285F188AADC1C17` |
| `docs/ai/README.md` | `EA4D3CA642550296C59F4DFE37896CBB2150BDC1F678C47A60F5F976FCE98081` |
| Standards `temporal-timezone-sync.md` | `001EF2D997307D0B7F54E2FCE467C617EC436BDD84926ED287747BBF88E1188B` |


## T1-U0 unit review and coverage-runner ruling — 2026-09-27

A fresh, bounded Claude Fable 5.1 code review was run read-only against the T1-U0 worktree. Public findings and the binding coverage-runner contract are preserved in `artifacts/reviews/T1-U0-fable-review.md` (SHA-256 `C3B78C75BA0725486DEAC8661946FF807171EDE17CAE90DF5489E87ECD9F419B`).

**Code review: FIX. Unit acceptance: BLOCKED.** Fable found three concrete items: (1) the null/default zone assertions use `Assert.Equal`, which cannot distinguish `TimeZoneInfo.Local` from `TimeZoneInfo.Utc` on UTC CI; change the three zone assertions to `Assert.Same`; (2) add plan-required remarks to Fixed, Counting and Advancing providers clarifying that wall-clock reads are controlled but inherited timestamps/timers remain real-clock behavior; (3) low-priority recommended improvement: assert a new counting provider has count 0 and count 1 after one read, since future consumers rely on a strict single-read contract.

Fable bound the isolated multi-target coverage fix to shared framework discovery and publish helpers, per-TFM Coverlet isolated execution, reuse of the resolver/publish helpers in the existing dotCover path, and a Pester regression suite in `tools/.tests` covering selected and unbound TFMs, isolated/non-isolated behavior, error visibility, and one slow end-to-end Testing run. The detailed ownership, behavior matrix, exact test vectors and documentation changes are in the public review artifact. The unchanged 100% coverage gate and `-Isolated` behavior remain required; no bypass is authorized.

Verification reported to Fable: all-scope format passed; compiler diagnostics passed with zero warnings/errors; all-scope tests passed with 678 Testing tests per TFM and no failures/skips. The Testing coverage command failed before test execution because isolated multi-target publish omitted the target framework and produced NETSDK1129. The first audit attempt had aborted during pnpm preflight, so Fable correctly treated it as unexecuted based on its review input. After that review, Luna_research resolved pnpm using the pinned frozen-lockfile install in the ignored worktree artifact area; `audit --gate` then exited 0 with surface-parity 0 and test-files 0. Thus audit is now passed, while coverage remains blocked pending the approved runner repair and successful net8.0/net10.0 isolated Testing coverage runs.

Provenance as reported by Claude Code: fresh session `04825da4-0e28-4721-b665-2adbc91d7796`, model `claude-fable-5-1`, provider `firstParty`, exit 0, 63 turns, duration_ms 413,772, total_cost_usd 6.638071; ModelUsage input 194, output 31,373, cache-read 791,204, cache-creation 243,484, webSearchRequests 0. CLI accounting scope is not independently verified; counters are recorded as reported. The session was restricted to Read/Glob/Grep with explicit strict empty MCP configuration. No hidden reasoning or intermediate payloads are retained.

## Review-status clarification — 2026-09-27

Correction to the preceding status summary: T1-U0 is not blocked solely on coverage. Fable's two blocking provider findings remain open until implemented and reviewed: (1) use reference identity assertions to prove UTC fallback instead of host-local fallback; and (2) document that inherited timestamp/timer behavior is real-clock behavior. In addition, the required isolated Testing coverage gate must pass for both net8.0 and net10.0 after the runner fix. The audit gate has since passed. The separate low-priority CountingTimeProvider zero/one-read assertion is recommended but is not independently blocking.

