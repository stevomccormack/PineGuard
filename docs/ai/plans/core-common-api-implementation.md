# Core / Common API implementation tracker

Approved by the owner on 2026-11-08: implement the corrected
[design](core-common-api-decisions.md) in full, verify 100% line and branch coverage,
then commit and push the feature branch without a PR.

Baseline: `e88762c`, branch `feature/api-decisions-refresh`. The corrected plan was the
only pre-existing change in this worktree and is included in the approved work.

## Checkpoints

- [x] Recheck repository policies and worktree ownership.
- [x] Inventory public declarations and exact naming transformations before applying renames; reconcile the final surface against the pristine starting commit.
- [x] Temporal range semantics and required-bound consistency (C-1, C-4/C-5/C-6, temporal F-4/B-6).
- [x] Enumeration catalogue lifecycle and identity (E-1, F-1/F-2/F-5).
- [x] Core utility/leaf corrections (A-1/A-2/A-4, E-2, F-3/F-6/F-8, H-1).
- [x] CSV, configuration precedence, comparisons and nullable collections (A-5, B-1/B-2/B-7, D-1/D-2, F-4/F-7).
- [x] Try outputs, Guid symmetry, coordinate styles and target parity (A-6, B-3/B-4/B-5/B-8, G-1).
- [x] Cross-layer range and parsed-type vocabulary, duplicate removal (A-3/A-8, N-1/N-2/N-3).
- [x] Canonical policy, XML onboarding, vocabulary and documentation reconciliation (N-4, C-3, exception register).
- [x] Compile consumer/API checks and all required target frameworks.
- [x] Tests, 100% line and branch coverage, formatting and audit gates.
- [x] Independent implementation review and resolution of all findings.
- [x] Prepare the reviewed file set for the owner's authorized commit and push; the delivery message records the resulting commit and remote verification.

## Coordination

Independent workers own temporal Core files, Enumeration files, and leaf utilities respectively.
The primary agent owns remaining implementation, cross-layer naming integration and verification.
Builds, tests and coverage are serialized. Global renames wait until relevant file owners finish.

## Evidence

Verification commands, reports and results are recorded below as executed. No unchecked
checkpoint is a claim of completion. Circular time-of-day windows remain the distinct future
feature explicitly excluded from the approved linear-range correction in the design.

### Temporal implementation verification

Implemented closed default overlap, representable open-interval overlap, overflow-safe adjacency,
normalized read-only Union results and separate Enclose, UTC elapsed DateTime duration,
DateOnly endpoint-distance duration, required ordering bounds and configuration precedence.
Temporal scalar and range TryParse outputs are nonnullable structs with default on failure.
The duplicate DateOnly range annotation was consolidated.

Scoped tests passed on both net8.0 and net10.0: Core 4,911; Must 1,026; Guard 786;
FluentValidation 1,112; DataAnnotations 624 (8,459 per framework). This evidence precedes
the final temporal XML/fixture cleanup and attribute-consumer checks. Coverage gates remain
pending; passing tests are not a claim of 100% line or branch coverage.

### Decision-to-implementation record

| Decisions | Delivered behavior and evidence |
|---|---|
| A-1, A-2, A-4, N-3 | Canonical buffer/hostname names; deleted casing forwarders and Guard whitespace alias. Compiler inventory and lexical/audit checks cover callers, fixtures and XML references. |
| A-3, A-8, N-1 | `InRange`/`OutOfRange` and concept-first parsed-type qualifiers propagate through Rules, Must, Guard, Fluent and annotations. Exact declaration dispositions are in [the surface map](core-common-api-surface-map.json). |
| A-5, D-1, D-2 | Explicit collection comparers govern every implementation path; nullable value-type item validation produces a non-null-item snapshot; one-shot and self-subset enumeration regressions pass. |
| A-6, A-7, C-2 | Added valid-domain Guid `IsEmpty` twin; retained documented control-character wording. Calendar predicates were already resolved at the starting commit. |
| B-1, F-7 | CSV separator and schema configuration are checked before input; header and row contracts, schema names/lengths and adapters have regression cases. |
| B-2 | `Ok(result, paramName, value)` aligns metadata ordering; named metadata callers and positional metadata tests distinguish the two values. |
| B-3, G-1 | Coordinate styles are exposed across layers; concrete decimal/string-number functionality is available on every supported target. Genuine platform boundaries are recorded in [the capability matrix](../specs/language/api-capabilities.md). |
| B-4, B-5, B-8 | Value-type Try outputs use nonnullable `out T` and default on failure. Reference outputs carry accurate success-flow annotations; documented successful-null generic results remain distinct. |
| B-6, F-4 | Ordering uses required `other` bounds; invalid ranges, inclusion, precision, styles and comparisons cannot disappear behind null/negative shortcuts. Nullable Fluent selectors do not run for absent values; null selectors fail at rule construction. |
| B-7, N-2 | CIDR configuration wins over input errors; malformed addresses fail both range predicates. IP and phone clauses preserve parsed/normalized `Result` and original `Value`; redundant raw-result twins are removed. |
| C-1, C-4, C-5, C-6 | Closed overlap/intersection/adjacency laws agree; exclusive interiors use representable points. `Union` preserves disconnected sets, `Enclose` names the hull. Duration is elapsed endpoint distance; DateOnly cardinality is `DayCount`. DST, extrema and singleton laws are exercised. Circular time windows remain outside the approved linear-range scope. |
| C-3 | Header lookup is exact, whole-value matching. Repeated headers, order, case and whitespace tests prevent claims of browser-policy parsing. |
| E-1, F-1, F-2, F-5 | Enumeration discovery uses a cached immutable declared-field catalogue, deterministic membership and role-specific comparers. Reentrant initialization, inheritance, duplicates and failures are tested; linked external consumers run on both test frameworks. |
| E-2, F-3, F-6, F-8, H-1 | Digit separators are defensively copied; OWASP predicates share utility implementations; SpaceCase is lowercase; unused signed-integer regex declarations are deleted; ASCII-space phone formatting matches the documented normalization example. |
| N-4 | XML syntax and schema validity remain distinct. Scope onboarding and exact audit exceptions are present; combined BCL/PineGuard imports compile. Required schema services are checked before optional annotation null skipping. |

### Review findings resolved during implementation

Decimal annotation configuration now delegates through Must; it does not bypass the layer stack.
Attribute arguments use metadata-compatible invariant strings for decimal configuration, and
approximation requires an explicit tolerance. Fluent typed numeric comparisons use the entire
`*Number` family to avoid collisions with framework-native comparisons. These exceptions are
documented in the canonical collision policy rather than implemented as aliases.

The trimmed consumer exposed a linker incompatibility in explicit class initialization. Discovery
now retains the required constructor metadata and invokes the runtime type initializer; reflected
field access initializes each declaring type. The net8.0 and net10.0 linked executables both passed
with linker warnings treated as errors and no warning suppression. The permanent recipe lives in
[the trim consumer README](../../../tests/PineGuard.Core.TrimSmoke/README.md).

The CLI layer-parity audit now uses boundary-safe canonicalization and recognizes reflective
`nameof` dispatch. Nullability auditing discovers declared struct/enum types instead of treating
them as nullable references. Regression fixtures protect unrelated names and arrays. Existing
audit baselines and coverage exclusions were not broadened.

The existing pnpm lockfile referenced Vite 8.2.2 while the checked-in CLI manifest required 8.2.4.
The lockfile was reconciled to that manifest so the CLI checks could run reproducibly; no package
manifest or dependency policy was changed.

### Final verification

- .NET 8: **23,214** tests across all 15 test projects pass; aggregate line coverage **24,922/24,922** and branch coverage
  **8,048/8,048**. The XML consumer test was rerun after correcting its validation context;
  the aggregate gate then passed using all current .NET 8 reports.
- .NET 10: **23,239** tests across all 15 test projects pass; aggregate line coverage
  **24,968/24,968** and branch coverage **8,068/8,068**. Both framework gates require exact
  100%, with zero failed or skipped tests in the final results.
- Final all-target solution build: **zero warnings and zero errors**, including netstandard2.1
  production assets and the analyzer projects' own targets.
- CLI: 208 tests, type checking, lint and changed-file formatting pass.
- Required audit gate and targeted documentation/naming/nullability audits: zero findings.
- Final solution formatting verification and standalone trimmed-consumer formatting verification
  passed. The formatter's design-time workspace emits notices for existing analyzer release-file
  duplication and project metadata references; compilation itself reports zero warnings/errors.
  No formatting diagnostics remain. The standalone consumer needed three final-newline fixes.

Coverage is collected separately with
`tools/code-coverage/Run-CodeCoverage.ps1 -Scope All -Framework <tfm> -Enforce100 -NoOpen -SkipHtml`.
The reporter selects the latest report per project, so a combined multi-target run alone is not
used as evidence that both frameworks meet 100%. Raw .NET 8 reports are archived separately
before the .NET 10 collection replaces the latest reports.

The [surface evidence summary](core-common-api-surface-map.md) and detailed JSON account for
all **4,745** baseline and **4,786** final unique externally visible public declaration IDs.
The pristine baseline is the Git archive of `e88762c`; the historical intermediate rename map
is retained separately. Changed contract metadata includes return types, named parameters,
defaults, nullability and generic constraints. The final rename-rule inventory reports zero
remaining targeted names.

Independent final source review found no remaining semantic blocker in the parent and temporal
changes. It checked interval laws, configuration precedence, nullable selectors, XML/CIDR
configuration and parsed-result/original-input contracts; numerical and catalogue reviews were
performed separately. The declaration inventory and per-framework coverage are separate proofs,
not conclusions inferred from that review.
