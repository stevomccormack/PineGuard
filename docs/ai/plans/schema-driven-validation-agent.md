<!-- metadata_header
type: plan
id: schema-driven-validation-agent
version: 0.1
status: draft
last_updated: 2026-09-22
-->

# Plan — Schema-driven validation agent (CLI + skill)

**Status**: draft (2026-09-22) — basic plan; charter-level decisions D-1..D-4 open
**Home**: new repository (see §8) — this document lives here until that repository exists, then moves with it
**Governed by**: `new-surfaces-missing-validation-cases-00-program.md` for anything that touches PineGuard itself (package conventions §4, gates §7/§9); `docs/ai/specs/safety.md` for every action taken inside a consumer's repository
**Related**: [library-expansion-roadmap.md](library-expansion-roadmap.md), [competitive-analysis.md](competitive-analysis.md), [new-surfaces-orchestration.md](new-surfaces-orchestration.md) (model-routing rules reused as-is)

## 0. Why

Every PineGuard consumer does the same tedious work by hand: open the schema, read the column lengths,
write `MaxLength(100)` on the model, decide *where* the validation should run, then wire an adapter.
Nothing in that sequence needs a human except the last decision, and even that is usually dictated by the
codebase (MediatR present → pipeline behaviour; aggregates present → guards in constructors).

The deliverable is an agent that does the sequence for them, inside their own repository:

1. **Discover** — what the schema says, what the architecture is, what house style is already in use.
2. **Propose** — a placement matrix (one row per model: where, which PineGuard layer, why) for sign-off.
3. **Generate** — validators, adapters and normalizers on a branch, build green, tests green.

At minimum (MVP, §9 Phase 1–3) it reads an EF Core model snapshot and emits one `MustValidator<T>` per
model in the requested style. Everything else is additive.

PineGuard already ships the targets the agent wires into: `src/PineGuard.MustClauses`,
`src/PineGuard.GuardClauses`, `src/PineGuard.FluentValidation`, `src/PineGuard.DataAnnotations`,
`src/PineGuard.MediatR`, `src/PineGuard.AspNetCore`, `src/PineGuard.ErrorOr`, `src/PineGuard.OneOf`,
`src/PineGuard.FluentResults`, `src/PineGuard.Extensions.Options`. The agent adds no runtime code to
PineGuard; where it exposes a gap (§10) that gap is a PineGuard workstream, filed separately.

## 1. Principle — deterministic in the CLI, judgment in the skill

The single design rule that makes this cheap and reliable:

| Concern | Lives in | Why |
|---|---|---|
| Schema extraction (tables, columns, types, lengths, precision, nullability, uniqueness, FKs) | CLI | Pure I/O; must be exact and reproducible |
| Project detection (solution layout, packages, existing validators, result style) | CLI | File-system facts, no interpretation needed |
| First-pass column-name → rule heuristics (`Email`, `FirstName`, `CountryCode`, …) | CLI (curated table) | 80% case; testable; zero tokens |
| Long-tail semantic classification, ambiguity resolution | Skill (LLM) | Genuine judgment |
| Placement decisions (which layer, which seam) | Skill, driven by a decision table in the skill's docs | Judgment, but constrained and explainable |
| Code generation | Skill → Sonnet-tier sub-agent, from CLI-produced manifests | Mechanical once the matrix is approved |
| Verification (build, tests, placement correctness) | Skill → Opus-tier sub-agent + CLI `verify` | Independent check of generated code |

The CLI emits two machine-readable manifests; the skill only ever reads manifests, never raw databases or
whole solutions. That is where the token and context efficiency comes from.

- `+ .pineguard/schema.json` — one entry per table/collection: columns with type, length, precision/scale, nullability, unique, PK/FK, default, check-constraint text where available.
- `+ .pineguard/project.json` — architecture shape, target frameworks, packages, existing validators (FluentValidation, DataAnnotations, `IValidatableObject`), result style (ErrorOr / OneOf / FluentResults / exceptions), MediatR behaviours present, endpoint style (Minimal API / controllers), DI registration entry points.

Both are gitignored by default in the consumer's repo; the agent offers to commit them if the consumer wants a record.

## 2. Schema readers

**Source-first, connection second.** Most .NET projects already carry their schema in-repo, and reading it needs no credentials, no native drivers and no infrastructure-layer archaeology.

| Priority | Reader | Notes |
|---|---|---|
| 1 | EF Core `*ModelSnapshot.cs` / migrations | `HasMaxLength`, `HasPrecision`, `IsRequired`, `IsUnique`, `HasColumnType`. Covers the majority of code-first shops. Roslyn-free: regex/AST-lite over the fluent calls is enough because snapshot output is machine-generated and regular |
| 1 | DDL scripts (`*.sql`), DbUp / FluentMigrator / Flyway folders | Parse `CREATE TABLE` / `ALTER TABLE` |
| 1 | JSON Schema (`maxLength`, `pattern`, `format`, `enum`, `minimum`/`maximum`) and OpenAPI component schemas | The "JSON database" case — document stores (MongoDB `$jsonSchema`, RavenDB, LiteDB, Cosmos DB) carry no column lengths; their contract *is* the JSON Schema. Also the right reader for API request/response models |
| 2 | Live connection: SQL Server, PostgreSQL, MySQL/MariaDB, SQLite | `INFORMATION_SCHEMA` / catalog queries only, read-only, single connection, redacted logging. Drivers: `mssql`, `pg`, `mysql2`, `better-sqlite3` |
| 3 | Oracle | Deferred: `oracledb` needs Instant Client; support cost outweighs demand until asked for |

Connection-string resolution order (live readers only): explicit `--connection`, named `--connection-name` looked up in `appsettings*.json` / user secrets / environment, else prompt. The string is never written to a manifest, log, or model context.

## 3. Discovery (skill phase 1, read-only)

Inputs: the two manifests. Output: a proposal document in the consumer's repo (`+ .pineguard/proposal.md`) and nothing else — no code is written until the proposal is approved.

The proposal contains:

1. **Detected shape** — architecture (Clean / Hexagonal / DDD / layered monolith / vertical slice / single project), API style, result style, existing validation, MediatR presence.
2. **Model ↔ table mapping** — by convention (`Order` ↔ `Orders`), by EF configuration, or flagged as unmapped.
3. **Placement matrix** — see §4.
4. **Semantic inferences** — see §5, each with its confidence and the rule it maps to, so low-confidence rows can be struck before generation.
5. **Choices the consumer must make** — only those the codebase does not already answer (for example: no existing validation library and no MediatR → "Must clauses on the application service, or Guard clauses on the aggregate?").

The consumer is never asked "which validator?" up front. The codebase answers most of it; the proposal asks only the remainder.

## 4. Placement decision table

Encoded as a markdown table in the skill (documentation, not code) so it can be argued with and amended. First cut:

| Signal | Model kind | Placement | PineGuard layer / adapter |
|---|---|---|---|
| Aggregate / entity with private setters, factory or constructor | Domain | Guard in constructor / factory | `Guard.Against.*` (`src/PineGuard.GuardClauses`) |
| Value object | Domain | Guard in constructor, or `Must` + `ErrorOr`/`OneOf` factory when the codebase returns results from factories | GuardClauses or MustClauses + result bridge |
| MediatR command / query | Application | `IPipelineBehavior` | `src/PineGuard.MediatR` with one `MustValidator<T>` per request |
| Minimal API request DTO | Presentation | Endpoint filter | `src/PineGuard.AspNetCore` |
| Controller action model | Presentation | MVC filter, or `IValidatableObject` bridge if the codebase already relies on `ModelState` | `src/PineGuard.AspNetCore`, or `MustValidationResultExtension.ToValidationResults()` in `src/PineGuard.DataAnnotations` |
| Options class | Infrastructure / host | `IValidateOptions<T>` | `src/PineGuard.Extensions.Options` |
| FluentValidation already present | any | Generate `AbstractValidator<T>` using PineGuard rule extensions, do not introduce a second validation style | `src/PineGuard.FluentValidation` |
| DataAnnotations already present and pervasive | any | Attributes | `src/PineGuard.DataAnnotations` |
| ErrorOr / OneOf / FluentResults in return types | any | Bridge `MustValidationResult` to the result type in use | `src/PineGuard.ErrorOr`, `src/PineGuard.OneOf`, `src/PineGuard.FluentResults` |

Rule of the table: **never introduce a second style where one exists**. Precedence when signals conflict is a Phase 4 deliverable.

## 5. Semantic inference and Normalize / Sanitize

Two tiers.

**Tier 1 — curated heuristics table (CLI, zero tokens).** Column-name patterns → PineGuard rule, with a confidence. Seed list:

| Pattern family | Rule intent |
|---|---|
| `FirstName`, `LastName`, `GivenName`, `Surname`, `FullName`, `DisplayName` | Trim, collapse whitespace, letters/marks/apostrophe/hyphen/space, length from schema |
| `Email`, `EmailAddress` | Email format, normalize case of domain part, length from schema |
| `Phone`, `Mobile`, `Telephone`, `Fax` | E.164 or national format, strip formatting |
| `AddressLine1/2/3`, `Street`, `City`, `Suburb`, `State`, `Region`, `PostCode`, `ZipCode` | Length from schema; postcode format by detected country if the table has a country column |
| `CountryCode`, `Country` | ISO 3166 alpha-2/alpha-3 (Core ISO rules) |
| `CurrencyCode`, `Currency` | ISO 4217 |
| `LanguageCode`, `Culture`, `Locale` | ISO 639 / CLDR |
| `TimeZone`, `TimeZoneId` | IANA / Windows id |
| `Iban`, `Bic`, `Swift`, `Abn`, `Acn`, `Tfn`, `VatNumber`, `TaxId` | Identifier checksum rules where PineGuard has them |
| `Url`, `Uri`, `Website`, `Slug`, `Username`, `Handle` | Format + length |
| `Password`, `PasswordHash`, `Secret`, `Token`, `ApiKey` | Length only; **never** echo values; flag as sensitive so `MustFailure.Value` is suppressed |
| `Latitude`, `Longitude` | Range |
| `Percent`, `Percentage`, `Rate` | 0–100 or 0–1 by detected precision |
| `Quantity`, `Count`, `Sku`, `Barcode`, `Gtin`, `Isbn` | Non-negative / checksum |
| `DateOfBirth`, `Dob` | In the past; optional min-age |
| `CreatedAt`, `UpdatedAt`, `*Utc` | Not in the future (clock-injected) |
| `Amount`, `Price`, `Total`, `Balance` | Precision/scale from schema, non-negative unless `Balance`/`Adjustment` |

**Tier 2 — long tail (skill, Haiku-tier classification).** Columns the table misses are batched into one classification call per table, output constrained to the same rule vocabulary, confidence attached, low confidence surfaced in the proposal rather than silently applied.

**Normalize / Sanitize.** Generated per inferred family (trim/collapse names, lower-case email domain, strip phone formatting, upper-case ISO codes). PineGuard's own normalization surface is currently thin (a handful of string clauses across `src/PineGuard.MustClauses/MustStringClauses.cs`, `src/PineGuard.GuardClauses/GuardStringClauses.cs`, `src/PineGuard.DataAnnotations/StringAttributes.cs`). Phase 1–3 generates normalizers as plain consumer-side code; a proper `Normalize`/`Sanitize` vocabulary in PineGuard is filed under §10, not built here.

## 6. Generation and verification

- All writes go to a new branch in the consumer's repo (`pineguard/validation-<date>` by default). Never to the checked-out branch, never to `main`. Tier 1 under `docs/ai/specs/safety.md`.
- One commit per placement row group (all MediatR behaviours, all aggregate guards, …), conventional-commit subjects, so the consumer can cherry-pick.
- Generated code mirrors the consumer's conventions detected in `project.json` (namespaces, file-scoped namespaces, nullable, `sealed`, folder layout), not PineGuard's.
- `verify` step (CLI + Opus-tier reviewer): `dotnet build` clean, existing tests green, one generated smoke test per validator proving a schema-length violation is rejected, and a placement check that every matrix row has a corresponding file.
- Report: `+ .pineguard/report.md` — what was generated, what was skipped and why, gaps in PineGuard encountered (§10).

## 7. Orchestration and model routing

Reuses the routing rules in [new-surfaces-orchestration.md](new-surfaces-orchestration.md) verbatim:

| Work | Tier |
|---|---|
| Read manifests, summarise, Tier-2 column classification | Haiku |
| Code generation from an approved matrix | Sonnet |
| Verification, placement review | Opus |
| Proposal authoring, ambiguity resolution, final review | Fable when available, else Opus |

Every phase is resumable from its on-disk artefact (`schema.json` → `project.json` → `proposal.md` → branch → `report.md`), never from conversation state, so a mid-flight failure restarts one phase, not the run.

The skill is the orchestrator and never edits files itself; it dispatches sub-agents per phase.

## 8. Where it lives — repository, naming, distribution (open)

**Recommendation: a separate repository.** It runs inside the *consumer's* workspace, so it must be installable (Claude Code plugin and/or npm package), not discovered through PineGuard's own `.claude/`. Release cadence also differs: the agent iterates daily, PineGuard is semver'd NuGet. Cost: cross-repo drift against PineGuard's public API — mitigated by pinning a PineGuard version per release and by un-deferring the core-vs-library docs manifest / PineGuard.Standards sync idea, of which this agent becomes the second consumer.

Structure inside that repository (indicative): `+ apps/cli` (TypeScript, same toolchain as this repo's `apps/cli`), `+ skills/<name>/SKILL.md` (orchestrator), `+ agents/` (sub-agent definitions), `+ .claude-plugin/plugin.json` (plugin manifest), `+ docs/` (placement decision table, heuristics table as the reviewable source of truth).

**Name — owner sign-off required.** Candidates and rejected alternatives per the naming standard:

| Candidate | Verdict | Reason |
|---|---|---|
| `PineGuard.Skills` | Rejected | Dotted PascalCase reads as a NuGet package; this is a plugin + npm CLI |
| `PineGuard.Integrations` | Rejected | "Integrations" already means the adapter libraries in `src/` (FluentValidation, MediatR, …) |
| `pineguard-agent` | Weak | Accurate to the mechanism, says nothing about the job |
| `pineguard-fit` | Lean | Fits validation to your schema and architecture; short; no collision found yet — precedent check outstanding |
| `pineguard-forge`, `pineguard-scaffold` | Alternates | Job-on-the-tin; "scaffold" collides with this repo's `scaffold-*` skills |

**Distribution — decision pending.** (a) Claude Code plugin first, CLI bundled; (b) npm-first CLI with the skill embedded and the plugin as a thin wrapper. (b) keeps the CLI usable without Claude (CI, other agents); (a) is the shortest path to the intended experience. Lean: (b).

## 9. Phases

| Phase | Deliverable | Exit criterion |
|---|---|---|
| 0 | Charter: name, repo, distribution (D-1..D-4), plugin skeleton, this plan moved | Owner sign-off on D-1..D-4 |
| 1 | CLI `discover`: EF snapshot + DDL + JSON Schema readers → `schema.json`; project detector → `project.json` | Runs against three fixture repos (Clean/MediatR, Minimal API, DDD) with golden manifests |
| 2 | Skill `propose`: discovery, placement matrix v1 (§4), Tier-1 heuristics (§5) | Proposal produced for the three fixtures, reviewed by hand |
| 3 | Skill `generate` + CLI `verify`: `MustValidator<T>` per model, MediatR and AspNetCore placements, branch + report | **MVP** — build and tests green on all three fixtures |
| 4 | Remaining placements (Guard on domain, FluentValidation, DataAnnotations, Options, result bridges), conflict precedence | Table complete; every row exercised by a fixture |
| 5 | Tier-2 semantic classification, Normalize/Sanitize generation | Long-tail columns classified with confidence surfaced |
| 6 | Live readers (SQL Server, PostgreSQL, MySQL, SQLite), connection-string resolution and redaction | Read-only integration tests against containers |
| 7 | Packaging: npm publish, plugin marketplace entry, docs site page, PineGuard README cross-link | Installable from a clean machine in one command |

## 10. Gaps in PineGuard this will surface (file separately, do not build here)

- A first-class `Normalize` / `Sanitize` vocabulary (§5).
- A published machine-readable manifest of PineGuard's public rule vocabulary (the deferred core-vs-library docs manifest / PineGuard.Standards sync) so the agent does not hard-code rule names.
- Any placement row in §4 that has no adapter today.

## 11. Risks

| Risk | Mitigation |
|---|---|
| Generated code in someone else's repo does damage | Branch-only writes; never `main`; Tier-1 confirmation; `verify` gate; report of every file touched |
| Connection strings leak into logs or model context | CLI resolves and redacts; secrets never enter a manifest; live readers are Phase 6, not MVP |
| Two validation styles introduced into one codebase | §4 rule: never add a second style where one exists |
| LLM classifies a column wrongly and a rule rejects valid production data | Confidence surfaced in the proposal; low-confidence rows struck by default; smoke tests generated per validator |
| Drift against PineGuard API | Pinned PineGuard version per release; manifest of rule vocabulary (§10) |
| Token blow-up on large solutions | Manifests only; skill never reads the solution; Haiku for all reads |
| Workflow mid-flight failure | Every phase resumable from its on-disk artefact (§7) |

## 12. Out of scope (this plan)

- Runtime code in PineGuard packages.
- Non-.NET consumers.
- Oracle live reader (deferred, §2).
- Editing the consumer's schema or migrations — the agent reads schema, it never writes it.

## 13. Decision log

| # | Decision | Status | Notes |
|---|---|---|---|
| D-1 | Separate repository, not a folder in PineGuard | Proposed | §8 |
| D-2 | Repository / package name | **Open — owner** | Lean `pineguard-fit`; precedent check outstanding |
| D-3 | Distribution: npm-first CLI with embedded skill, plugin as wrapper | Proposed | §8 |
| D-4 | Deterministic-in-CLI / judgment-in-skill boundary | Proposed | §1 — the load-bearing rule of the design |
| D-5 | Source-first readers; live connection is Phase 6 | Proposed | §2 |
| D-6 | Never introduce a second validation style where one exists | Proposed | §4 |
| D-7 | Branch-only writes in consumer repos | Proposed | §6, safety spec |
