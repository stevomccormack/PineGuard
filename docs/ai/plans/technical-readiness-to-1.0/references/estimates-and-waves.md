# Effort, waves and dispatch budgets

These are preliminary planning ranges, not promises. **Engineering-equivalent hours** estimate task complexity as hands-on engineering work. **Active agent/tool hours** estimate cumulative model/tool execution across role-separated packets, including measurement runs but excluding user waits. **Review hours** estimate Astra plus human decision/review attention. They are separate views, not quantities to add into a single completion date. Parallel elapsed time depends on tool/model availability, decisions, CI duration and exact scope.

Confidence is medium-low for small source-backed CI/documentation work and low for new architecture, hostile harnesses, deployment targets and major applications. Re-estimate after each wave with actuals. Ranges assume existing source remains usable, no major external toolchain blocker, bounded pilot-first rollout, and no new feature catalog.

| Work | Engineering-equivalent h | Active agent/tool h | Review h |
|---|---:|---:|---:|
| W00 | 8–16 | 3–6 | 1–3 |
| W01 | 12–24 | 4–9 | 2–4 |
| W02 | 12–24 | 4–9 | 2–4 |
| W03 | 16–32 | 5–12 | 3–5 |
| W04 | 10–20 | 3–8 | 2–4 |
| W05 | 16–32 | 5–12 | 2–4 |
| W06 | 8–16 | 3–6 | 1–2 |
| W07 | 8–20 | 3–8 | 1–3 |
| W08 | 6–14 | 2–5 | 1–2 |
| W09 | 6–14 | 2–5 | 1–2 |
| W10 | 8–16 | 3–7 | 1–3 |
| W11 | 8–20 | 3–8 | 1–3 |
| W12 | 10–20 | 3–8 | 2–4 |
| W13 | 4–10 | 2–4 | 1–2 |
| W14 | 6–12 | 2–5 | 2–3 |
| W15 | 6–14 | 2–6 | 1–3 |
| W16 | 8–16 | 3–6 | 1–3 |
| W17 | 4–10 | 2–4 | 1–2 |
| W18 | 6–14 | 2–5 | 1–2 |
| W19 | 8–18 | 3–7 | 2–3 |
| W20 light | 6–12 | 2–5 | 1–3 |
| W21 | 6–12 | 2–5 | 2–4 |
| W22 | 10–20 | 3–8 | 3–5 |
| W20A | 16–32 | 5–12 | 2–4 |
| W20B | 20–40 | 6–15 | 3–5 |
| W20C | 24–48 | 8–18 | 3–6 |
| Core + light docs subtotal | 192–406 | 66–158 | 35–73 |
| Including all three major samples | 252–526 | 85–203 | 43–88 |

These totals are conservative sums of separately scoped work; shared discoveries may reduce work, unresolved design or broader scope may increase it. Do not claim the four-agent limit produces fourfold acceleration. Roles and dependencies often serialize the critical path.

## Ordered waves

| Wave | Work and exit | Scheduling guidance |
|---|---|---|
| A — small high-value trust and decisions | W00 CI/coverage, W22 naming inventory/decision, W01 reproductions, W10 early baseline, W19 characterization | Start immediately after activation; independent Luna packets then Astra decisions |
| B — early new structure/manifest | W22-approved W03 structure, W02 executable manifest, W01 pilot truth, W04 consistent failures; remeasure W10 | Commit pilot vertically before wider family rollout |
| C — validation proof | W05 conformance/coverage, W06–W09 separate hostile proof, W14–W19 policies/evidence | Prioritize measured risk; no all-at-once harness rewrite |
| D — operational/package proof | W11 lightweight trimmed/native consumers, W12 new API drift, W13 dependencies, final budgets | Begin small consumer proof once contracts stabilize; no legacy migration work |
| E — consumer polish then largest apps | W20 lightweight docs first; W20A API, W20B DDD, W20C Clean Architecture last | Each major app gets a fresh exact interface/scope decision |
| F — every-row10/10 adjudication | W21 final evidence/review/council/release decision | No automatic score from finishing tasks |

Allocate each child task once in the execution ledger; do not count full workstreams in multiple waves. Initial engineering-equivalent wave allocation: A24–48 hours; B44–88; C76–164; D28–62; E74–152 including major samples; F6–12. These sum to252–526 hours, matching the all-workstream envelope. They are preliminary task allocations, not phase-duration promises; reallocate with recorded actuals without silently changing the controlling total.

## Packet sizes and model budgets

| Role/task | Requested runtime | Requested token budget | Checkpoint/output |
|---|---|---|---|
| Luna targeted read/BRAIN/source packet |5–10 min |2k–6k | Exact excerpts/path/hash and unknowns; no full-file dump by default |
| Luna bounded research/campaign evidence |10–15 min |4k–8k | Source register or artifact facts with dates/limits |
| Sol implementation slice |10–20 min |6k–12k | One coherent root-cause change, proof and exact owned paths |
| Sol comparative analysis |8–12 min |6k–10k | Factual comparison, tradeoffs, unknowns; no architecture approval |
| Astra decision/review |10–15 min |8k–16k | Explicit decisions/findings and exact acceptance |
| Astra plan/document batch |10–20 min |8k–16k |2–4 complete reviewable artifacts/checkpoint |
| Orchestrator |Short coordination turns |Keep handoffs compact | Dispatch/status/dependency/ownership only |

Requested budgets are not tool-enforced caps. Record actual cap/timeout mechanisms only when present. Request a heartbeat/checkpoint at least every two minutes; missing two checkpoints triggers status inquiry and bounded replan/interruption through available authorized controls. Never secretly substitute models or claim a timeout mechanism exists when it does not.

## Token optimization

Luna maintains a canonical source packet cache keyed by repository revision plus per-file content hashes, dirty-tree scope, source URL/checked date and relevant BRAIN version. Before reuse, verify relevant hashes/freshness; invalidate downstream decisions when inputs change. A commit hash alone cannot establish freshness in a dirty tree.

Dispatch only the task-specific excerpts, decisions, interfaces and artifact references; send deltas after a checkpoint. Keep full source/research/transcripts in canonical references and preserve their hashes. Sol and Astra request missing evidence from Luna rather than repeating searches. The orchestrator reads status/decision summaries, not whole codebases or logs.

Cache facts, not conclusions indefinitely: external version/support claims refresh before publishing comparisons; semantic decisions are reopened when contradictory evidence appears. Batch independent Luna reads, retain one normalized inventory, and avoid duplicate agents researching the same fact.
