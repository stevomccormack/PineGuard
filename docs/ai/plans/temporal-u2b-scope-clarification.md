<!-- metadata_header
type: receipt
id: temporal-u2b-scope-clarification
version: 1.0
status: recorded
last_updated: 2026-09-27
-->

# T1-U2/U2b Scope Clarification — Receipt

## Status

Recorded. This is a factual receipt of an already-issued binding ruling; it makes no new design
decisions.

## Source

[`artifacts/reviews/T1-U2-U2b-scope-ruling.json`](../../../artifacts/reviews/T1-U2-U2b-scope-ruling.json)

- Reviewer model: `claude-fable-5-1`
- Reviewer session: `d5ce9d36-8fc9-4f52-b489-fc7a2eb4e4db`
- Binding outcome: **EXTEND NOW**

## Scope extension

The ruling extends U2b's scope to include `DateOnly` `PastOrPresent` and `FutureOrPresent`,
wiring both to the same guarded calendar reference as the already-listed `DateOnly` `Past`,
`Future` and `MinimumAge` clauses. `DateTime` relative predicates (`Past`, `PastOrPresent`,
`Future`, `FutureOrPresent`) remain instant comparisons against `GetUtcNow` and are unchanged
by this extension.

## Transcription note

The ruling's binding wording clarification for U2b has been transcribed verbatim into
`docs/ai/plans/temporal-01-foundation.md` U2b.

## Acceptance status

This receipt records the scope ruling only. Implementation remains governed by the outstanding
U2/U2b code findings and is not marked as accepted here.
