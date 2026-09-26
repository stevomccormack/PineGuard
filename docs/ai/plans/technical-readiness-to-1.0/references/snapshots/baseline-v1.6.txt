<!-- metadata_header
type: plan
id: technical-readiness-to-1.0
version: 1.6
status: planned
last_updated: 2026-09-25
-->

# Technical readiness to 1.0

> **Status:** Planned. This document records the agreed planning scope; no implementation,
> new test run, benchmark, publication or release is performed by creating it.
> **Design owner:** Astra. Astra owns architecture decisions, sequencing, plans and review;
> scoped implementation and evidence gathering go to appropriate execution subagents.

## 1. Objective and boundaries

Make PineGuard's validation contracts explicit, its projections mechanically consistent, and
its correctness, performance and deployment claims supported by reproducible evidence before
1.0. This is a technical readiness program, not a promise of a perfect score, complete security,
or demonstrated market adoption.

The program covers the canonical rule contract, a Rule Manifest, cross-surface conformance,
independent correctness testing, benchmarks, security boundaries, API/package compatibility,
consumer samples, documentation and staged release of integration packages. External feedback
is a later validation stream; it cannot substitute for engineering gates or be fabricated by
internal examples.

Preserve the current Core Utils/Rules → Must → Guard/integrations direction. Core algorithms
already provide substantial executable reuse; Must owns canonical messages and typed results.
Do not start with a mass rewrite into a class-per-rule runtime hierarchy. A descriptor or
generator must earn its complexity through a small, measured pilot.

This plan complements the [new-surfaces program](new-surfaces-missing-validation-cases-00-program.md)
and [Core/Common API decisions](core-common-api-decisions.md). Reconcile their current state
before execution; their historical statuses are not proof that an issue still exists. The
separate schema-driven validation-agent proposal is outside this implementation scope.

## 2. Evidence baseline, dated 25 September 2026

| Area | Established baseline | Limitation |
|---|---|---|
| Tests | 18,862 passing .NET 8 executions plus 18,887 passing .NET 10 executions; 37,749 total, zero failed/skipped | Local Windows Debug run across 15 test projects; totals include repeated framework executions, not distinct test definitions |
| Earlier analyzer failures | Resolved after restoring missing compiler reference-cache files; no product/test-source repair was needed | Do not report the original 115 failures as outstanding product failures |
| Coverage | CI defaults to 100% line/branch thresholds; a committed March 2026 snapshot covers six assemblies | The September 25 test run collected no new coverage; historical or ignored local reports do not establish current coverage |
| Packages | Prior audit snapshot: 15 packable projects, six published at alpha.7 and nine source-only | Dated inventory, not a live feed check or permission to publish; verify versions/availability before release work |
| Enforcement | Existing layer-parity and failure-code audits, shared fixtures and extensive per-layer tests | No complete executable rule manifest or cross-surface semantic contract established by this review |
| Additional assurance | No dedicated property/fuzz harness, BenchmarkDotNet baseline or automated cross-release API compatibility gate found in the reviewed configuration | Missing evidence is not proof of a defect or poor performance |

The authoritative test narrative is the [verification report](../../reports/readme-verification-2026-09-25.md).
Its ignored local evidence directory contains the 30 TRX files and aggregate summary. Future
reports must record source revision, configuration, frameworks, OS, tool versions, exclusions
and artifact provenance. Do not relabel historical metrics as current results.

The [technical rubric review](../../reports/technical-readiness-rubric-2026-09-25.md) records the
fresh whole-repository assessment: coarse readiness scores, observable 10/10 criteria, confidence,
strengths and evidence-linked gaps. Its new highest-priority findings concern CI routing, missing
expected evidence and dormant semantic audits. They are source-derived; no workflows were run
to reproduce them during planning. No overall numeric certification or market score is claimed.

### Concrete findings to resolve first

1. **Command-newline normalization, source-derived and not runtime-confirmed in the review.**
   [OwaspRules.IsCommandInjectionSafe](../../../src/PineGuard.Core/Rules/OwaspRules.cs) trims before
   testing a newline pattern. Thus leading/trailing CR/LF can disappear before validation;
   [MustOwaspClauses](../../../src/PineGuard.MustClauses/MustOwaspClauses.cs) returns the original
   string. The inferred example is `"\nhello"` passing the individual command check while
   `"hello\nworld"` fails. The composite OwaspSafe also checks raw CR/LF: do not generalize this
   example to the composite or claim an exploitable application without evidence. Existing
   [OWASP fixtures](../../../tests/PineGuard.Testing/Fixtures/OwaspRulesFixtures.cs) cover an
   interior command newline and separately cover leading/trailing CR/LF for the header rule.
2. **Regex evaluation failures and negation.**
   [StringRules.IsMatch](../../../src/PineGuard.Core/Rules/StringRules.cs) directly invokes a regex;
   [Must Match/NotMatch](../../../src/PineGuard.MustClauses/MustStringClauses.cs) do not map timeout
   exceptions. Built-in [OWASP regexes](../../../src/PineGuard.Core/Rules/Owasp/OwaspRegex.cs) have
   finite timeouts. Define the evaluation-error contract before changing this: converting an
   error to predicate false can incorrectly make a negative check succeed. Reproduce relevant
   cases without claiming a timing-independent failure for every input or machine.

## 3. Decisions and constraints

| Decision | Direction | Required review point |
|---|---|---|
| Algorithm ownership | Keep one executable implementation; retain current static primitives where suitable | Distinguish actual duplicated algorithms from repeated adapter declarations |
| Contract ownership | One typed definition owns metadata/policies, or generates them from authoritative declarations | No second manually maintained list of methods, codes and support flags |
| Class-per-rule design | Optional implementation detail, not a prerequisite | Pilot must justify runtime hierarchy, dispatch, allocation or reflection costs |
| Public compatibility | Preserve existing behaviour by default; identify intentional fixes and pre-1.0 breaking decisions explicitly | Reconcile the temporary greenfield policy and open API decisions before changing public signatures/codes |
| Null semantics | Preserve declared surface differences, including optional DataAnnotations values | Nullable signature, null acceptance and skip-on-null are different concepts |
| Parsing/results | Preserve original input separately from parsed/normalized result | No double parsing or loss of TResult merely to fit a universal interface |
| Negation | Complement only within the declared evaluable domain | Null, invalid options, parsing errors and evaluation errors must not invert into success |
| Time/culture/async | Make dependencies explicit; preserve TimeProvider, culture and cancellation seams | Decide clock snapshot scope, ambient-culture use, cancellation propagation and callback exception ownership |
| Security | Pattern heuristics describe limited checks, not safe execution of arbitrary input | Independent threat-model review; no promise of universal XSS/SQLi/SSRF prevention |
| Performance | Measure first; derive gates from stable evidence and intended workloads | No invented nanosecond/allocation budgets or noisy CI thresholds |

Cancellation is not automatically a validation failure. An arbitrary application callback
throwing is not automatically an invalid user value. Phase 0 must document those boundaries
alongside regex evaluation errors before a general-purpose error mapper is introduced.

## 4. Delivery sequence

All items below are **planned**, not completed. Each execution unit should produce a small,
reviewable change and a recorded acceptance result. Priority reflects dependency and risk,
not a calendar estimate.

| Phase | Priority | Depends on | Exit evidence |
|---|---|---|---|
| 0. Semantics and trustworthy verification | P0 | Current-source verification | Reproduced findings, approved contracts, reliable CI routing and required evidence |
| 1. Contract and manifest pilot | P1 | Phase 0 contract decisions | Three-rule pilot, enforced bindings, measured edit-point reduction |
| 2. Cross-surface conformance | P1 | Phase 1 pilot; can design alongside it | Policy-aware runner, independent expected outcomes, deliberate drift detected |
| 3. Independent correctness and security | P1 | Phase 0; grows with Phase 2 | Reproducible generated/corpus tests, threat model, independent review dispositions |
| 4. Performance and deployment evidence | P1 | Stable pilot behaviour | Benchmarks, allocation reports, selected trimming/AOT consumer results |
| 5. Compatibility, samples and docs | P1 | Pilot decisions; baseline work can start earlier | API/package gates, executable consumer examples, versioned documentation |
| 6. Expansion and release readiness | P2 | Phases 0–5 acceptance | Risk-ordered migration, release evidence, staged integration-package rollout |

Phase 3 corpus work, Phase 4 baseline measurement and Phase 5 API inventory can proceed in
parallel after contracts are clear. Builds/tests/coverage must still obey repository coordination
rules; parallel agents must not contend for shared build outputs or edit the same files.

## 5. Phase 0 — establish and repair semantic contracts

### 5.1 Semantic findings

- [ ] Confirm current source and reproduce command-newline behaviour with focused tests.
- [ ] Decide whether the individual command heuristic inspects raw input or deliberately
  normalizes it; ensure the value returned to callers matches the documented guarantee.
  Prefer inspecting raw control characters when the rule promises to reject them.
- [ ] Add leading, trailing, interior, encoded, whitespace-only and null cases, without changing
  unrelated security heuristics under the same patch.
- [ ] Specify outcomes for successful evaluation, validation failure, invalid configuration,
  regex evaluation failure, cancellation and application callback failure.
- [ ] Define positive/negative behaviour for each relevant outcome. Do not swallow every
  exception or reinterpret an evaluation failure as a successful negative assertion.
- [ ] Add focused regression tests and apply the smallest implementation corrections through
  execution subagents, with Astra reviewing the contract and resulting change.

**Acceptance:** each claimed defect has a failing-before/passing-after regression or is explicitly
reclassified with evidence. Documentation and both positive/negative APIs reflect the chosen
contract. Shared algorithm fixes are verified through affected public surfaces. No source-derived
example is presented as a confirmed exploit.

### 5.2 Make verification routing and evidence requirements trustworthy

Execute this alongside semantic fixes and before relying on new manifest/testing gates. See
rubric findings R1–R3 for the current source evidence; do not mistake a proposed gate for an
already active one.

- [ ] Define changed-input → required-job/scope mappings for shared test/build configuration,
  CLI/workspace/lockfile changes, workflows, coverage settings and documentation. Include
  `tests/Directory.Build.props`; review transitive project dependencies when selecting suites.
  Keep legitimate narrow checks and changes requiring no .NET tests explicit rather than running
  everything blindly.
- [ ] Add focused routing regression fixtures showing which jobs must run for those inputs and
  representative library/integration edits. Ensure the fixture evaluates the policy actually used
  by CI, not an independently copied list that can drift.
- [ ] Run the audit CLI's existing tests, typecheck and lint in its appropriate CI path, including
  PRs that change the CLI or its dependencies. Building/running its gate does not test the gate's
  own implementation. Preserve existing hard failures after report-upload steps.
- [ ] Derive the expected test/coverage artifact set from selected scopes/frameworks. Fail if
  expected artifacts are missing, malformed or incomplete; explicitly permit an empty set only
  when no test evidence was required. Test those negative cases as well as successful downloads.
- [ ] Inventory active versus dormant audit rules and baseline policy. Triage current parity/code
  findings, select high-value semantic checks for blocking enforcement, and give accepted debt
  a specific owner/reason. Do not enable every style rule or automatically refresh baselines.
- [ ] Demonstrate deliberate policy/contract regressions are detected and that report generation
  cannot convert failed required verification into a green result.

**Acceptance:** every reviewed shared/tooling input routes to its required checks; CLI tests run
on CLI changes; missing expected test/coverage evidence fails; explicitly selected semantic audits
block a seeded regression. Legitimately skipped scopes remain explainable. Retain the routing
fixtures and a current active-gate inventory with the verification evidence.

## 6. Phase 1 — canonical contract and Rule Manifest

### 6.1 Minimal conceptual descriptor

The following is a design shape, not a mandated runtime class or serialization format:

```text
RuleDescriptor
  Id
  Category
  InputType
  ResultType
  EvaluationBinding
  FailureOutcomes -> existing MustCodes references
  InputPolicy -> null, blank, normalization, result behaviour
  Options -> names, types, defaults, constraints
  Execution -> sync/async; culture/clock/cancellation dependencies
  Projections[]
    Surface
    MemberBinding
    PolicyOverrides
    FrameworkAvailability
```

Use compiler-checked symbols/typed bindings or reliable extraction where possible. Reference
existing [MustCodes](../../../src/PineGuard.Core/Codes/MustCodes.Email.cs); do not create another
set of literal failure strings. A concept ID is distinct from a failure outcome code. Permit
explicitly named outcome mappings where a single failure code cannot represent the contract.

Replace `SupportsNull` with explicit policy. Replace `SupportsMust`/`SupportsGuard`/`SupportsFV`/
`SupportsAnnotations` booleans with projection entries: presence indicates support and the entry
records member bindings and intentional differences. Unsupported projections need a recorded
reason. Model framework/overload availability rather than claiming every concept has every form.

Do not conflate the descriptor with an application configuration:

- Definition: in-range input/output types and its minimum, maximum and inclusion parameters.
- Configured rule: minimum 1, maximum 100, inclusive, attached to a particular application property.
- Invocation: current value and explicitly supplied execution context.

Do not put application paths, captured delegates, service instances, clock instances or actual
culture selections in the global manifest. Describe their contracts. Identify concrete callable
variants when async mode, types, options or target-framework availability differ.

### 6.2 Pilot and enforcement

- [ ] Pilot Email, a bounded numeric rule and string-to-DateOnly past-date validation. These cover
  simple predicates, options/boundaries and parsed output with a clock dependency.
- [ ] Inventory their existing algorithms, codes, messages, wrappers, fixtures and documentation.
- [ ] Choose the smallest authoritative representation; record why a typed declaration,
  extracted descriptor or generator is preferred. Keep it internal until the public extension
  contract is justified. Update normative specs before changing their ownership rules.
- [ ] Extend existing [layer-parity](../../../apps/cli/src/audit/rules/layer-parity.ts) and
  [must-codes](../../../apps/cli/src/audit/rules/must-codes.ts) enforcement where useful.
  They currently declare `gate: false`; Phase 0 must establish the intended blocking policy.
  Do not replace useful source audits with an unchecked catalog or describe dormant rules as gates.
- [ ] Check unique IDs, resolvable bindings, declared outcomes, option/default agreement,
  projection completeness and justified omissions in CI.
- [ ] Generate documentation and mechanical adapter declarations only after semantics are
  explicit. Keep exceptions narrowly declared and checked.
- [ ] Compare independent edit points and runtime costs with the existing implementation.

**Acceptance:** removing a declared adapter, changing its code mapping or altering a bound
signature causes a focused CI failure. The pilot preserves intentional behaviour, typed results
and supported frameworks. Generated outputs are reproducible and stale outputs are detected.
Expansion requires evidence of reduced maintenance burden, not merely fewer handwritten lines.

## 7. Phase 2 — cross-surface conformance

Build on the existing shared `RuleScenario<TInputs>` fixtures and per-layer tests. Do not replace
the current suite wholesale or calculate every expected outcome by calling Must.

```text
RuleCase<TInput, TResult>
  RuleId
  Input
  Options
  Context                 // fixed clock/culture where relevant
  ExpectedOutcome
  ExpectedResult
  ExpectedFailure         // outcome/code and attribution
```

The case is an independent expectation. The runner applies only manifest-declared projection
policies; avoid five separately entered validity booleans that can conceal accidental drift.

| Surface | Assertions |
|---|---|
| Core | Predicate/parser contract, safe parse defaults and deterministic context |
| Must | Success/failure, original Value, typed Result, code, template and parameter attribution |
| Guard | Correct counterpart mapping, return value, declared exception type and code/path stamps |
| FluentValidation | Targeted property failure, code, path and expected message rendering |
| DataAnnotations | Explicit skip/null policy, validation result and display-name rendering; check codes through the PineGuard metadata/adapter boundary that exposes them |

The standard DataAnnotations ValidationResult has no universal failure-code field. Do not invent
one in tests or claim code equivalence at a boundary where it is not exposed. Likewise, Guard's
bad-state names do not imply blindly negating the expected result: `NotEmail` maps to Must Email.

- [ ] Cover null/blank, valid/invalid, range endpoints, invalid options, parsing failures and
  typed results for the pilot, including target-framework variants.
- [ ] Assert messages with the expected property/display-name transformation, rather than
  byte-identical messages despite intentionally different names.
- [ ] Add fixed-clock/culture cases and declared async/cancellation cases where applicable.
- [ ] Demonstrate that deliberate changes to a code, null policy, mapping or result are caught.
- [ ] Enumerate all applicable manifest projections; report omissions rather than silently
  excluding unhandled shapes. Expand by rule family after the pilot passes.

**Acceptance:** every pilot projection has an explicit tested disposition, deliberate drift fails,
and independent expected cases remain distinct from differential surface comparisons. All surfaces
agreeing on a shared mistaken predicate must still be capable of failing an independent oracle test.

## 8. Phase 3 — independent correctness and security

### 8.1 Property-based testing

Generate inputs to check justified invariants across many cases, complementing the existing
example fixtures. A property needs an independent expectation, not an expected value calculated
by the same implementation under test.

- [ ] Choose a maintained property-testing framework after checking target-framework, xUnit,
  shrinking and reproducibility support. No framework is selected by this plan. Extend fixture
  conventions explicitly where generated cases need it; avoid redundant frameworks.
- [ ] Start with Email, the bounded numeric rule and PastDateOnly. Generate accepted/rejected
  email cases from an independently specified supported grammar subset; range inputs/options
  around boundaries; and dates around a fixed clock with explicit format/culture.
- [ ] State each property's preconditions: range monotonicity under compatible valid bounds,
  parsing/formatting round trips only for supported canonical formats, original/parsed value
  preservation, and positive/negative complements only within the declared evaluable domain.
  Handle null, invalid options, NaN and evaluation failures explicitly rather than assuming
  universal inversion. A BCL parser is not automatically an oracle for a stricter rule.
- [ ] Feed generated cases into the conformance runner with manifest-declared adapter policies,
  including DataAnnotations null skipping. Keep independent semantic assertions separate from
  cross-surface agreement, so a shared predicate error can still fail the suite.
- [ ] Shrink failures while preserving relevant preconditions. Retain the seed, minimized input,
  options, clock/culture and tool/runtime versions, and promote confirmed failures into shared
  regression fixtures. Report generated/admissible cases and discarded inputs to expose vacuous
  properties or generators that rarely exercise the intended domain.
- [ ] Run bounded reproducible PR checks and expanded scheduled campaigns. Expand after the pilot
  to URI/hostname canonicalization, Unicode normalization/graphemes and security-related rules,
  using only invariants their documented contracts justify.

**Acceptance:** pilot properties have reviewed independent expectations, exercise meaningful valid
and invalid partitions, and produce replayable minimized failures. Generated conformance respects
intentional surface differences. Seeds and confirmed regressions are retained; no finite campaign
is described as proof over all possible inputs.

### 8.2 Fuzz parsers and hostile input surfaces

Exercise robustness and declared outcomes with adversarial inputs; acceptance by a heuristic
does not establish that input is safe for a downstream application.

| Target group | Initial scope |
|---|---|
| Address parsers | URI parsing/rules, email parsing and hostname/IDN handling in UriUtility, EmailUtility and NetworkUtility |
| Typed parsers | StringUtility number, DateOnly, DateTimeOffset and date/time range parsing; valid options and invalid-option cases kept distinct |
| Unicode | Grapheme handling, normalization assumptions, combining sequences and malformed UTF-16 |
| Security checks | OwaspUtility/OwaspRules and regex-backed checks, including raw versus trimmed/decoded input boundaries |

- [ ] Select compatible fuzz tooling after checking the test/runtime fit and isolation needs;
  no specific framework is mandated. Begin with a small harness per target group, then expand
  according to findings and risk rather than fuzzing the entire public API without contracts.
- [ ] Seed from existing valid/invalid fixtures and confirmed property-test counterexamples.
  Add malformed, truncated and oversized inputs; embedded NUL/control characters; unpaired
  surrogates and combining marks; repeated delimiters; nested/repeated encoding; and numeric/date
  extremes. Include valid neighbours so the harness also detects unintended rejection.
- [ ] Test declared invariants: termination within the harness budget, expected parse defaults
  on failure, documented original/parsed output behaviour, and absence of unexpected exceptions.
  Honour Phase 0's evaluation-timeout/cancellation contracts. Do not universally require hostile
  strings to fail or reinterpret regex evaluation errors as successful negative validation.
- [ ] Fix clocks, cultures and supported options for reproducible runs; vary them explicitly in
  named campaigns. Record source revision, framework/tool versions, seed and corpus provenance.
- [ ] Exercise direct Core parsers/predicates and representative Must/Guard/integration entry
  points. Apply declared null and exception policies when comparing public-surface outcomes.
- [ ] Set input-size, per-case/campaign time, memory and concurrency budgets for each harness.
  Isolate campaigns that may hang or exhaust resources so the supervisor can stop them safely;
  classify resource-limit findings separately from ordinary validation failures and assertions.
- [ ] Minimize findings while preserving the triggering conditions. Retain replay inputs and
  promote confirmed defects into focused shared regression fixtures/corpus entries; reuse these
  across fuzz, property and conformance tests rather than keeping disconnected copies.
- [ ] Run bounded deterministic corpus replays on PRs and longer exploration campaigns on a
  schedule. Report crashes, unexpected exceptions, resource limits and contract mismatches with
  reproduction details and triage status.

**Acceptance:** each initial target group has a bounded, reproducible harness and meaningful seed
corpus; findings have minimized reproductions and explicit dispositions. Confirmed fixes have
regressions through affected public surfaces. Reports state budgets, coverage of input classes
and remaining assumptions; neither successful fuzzing nor heuristic acceptance proves security.

### 8.3 Differential testing

Compare PineGuard with a suitable independent implementation over an explicitly shared contract.
Agreement is useful evidence, not an oracle of truth; a mismatch may expose a PineGuard defect,
a comparator defect, an adapter mistake or an intentional semantic difference.

- [ ] Pilot **PineGuard SemVer versus NuGet.Versioning**. Current PineGuard exposes
  [VersionRules.IsSemVer](../../../src/PineGuard.Core/Rules/VersionRules.cs) and a Must clause
  returning the original string, not parsed version components or version ordering. Initially
  compare acceptance only; add component/normalization/ordering comparisons only if PineGuard
  actually exposes the corresponding supported contracts.
- [ ] Document the comparison domain before writing adapters: PineGuard's SemVer 2.0.0 grammar
  and trimming behaviour versus the selected NuGet.Versioning parser's package-version dialect,
  numeric limits, component counts, prerelease/build handling and normalization. Select the exact
  comparator API after this review; do not assume NuGet acceptance equals strict SemVer validity.
  For example, NuGetVersion accepts `1.0`, while PineGuard requires three components; that
  disagreement is expected. Document comparator numeric representation limits so large numeric
  components do not automatically become false PineGuard defect reports. See the official
  [NuGet versioning differences](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning#where-nugetversion-diverges-from-semantic-versioning).
- [ ] Separate overlapping-domain cases from intentional-difference cases, with independently
  justified expectations from the relevant specification and reviewed examples. Preserve original
  input; an adapter must not silently trim, normalize or discard information to force agreement.
- [ ] Combine generated paired cases with the shared regression/fuzz corpus, including valid
  boundaries and malformed neighbours. Compare declared outcomes under fixed culture/context;
  replay minimized mismatches with the exact comparator version and adapter configuration.
- [ ] Pin comparator versions as test-only dependencies. Triage mismatches before turning them
  into blocking assertions; classify the cause, retain the example and rationale, and add a
  regression for confirmed defects. Review comparator upgrades for changed behaviour.
- [ ] Consider further URI, date and number comparisons only where contracts overlap and a
  comparator adds useful independence. Account for culture, styles and numeric ranges. Comparing
  a wrapper to the same BCL parser it invokes checks adaptation, not independent parser correctness;
  do not copy external behaviour wholesale or change PineGuard solely to eliminate disagreement.
- [ ] Replay the established bounded comparison corpus on PRs and run expanded generated cases
  on a schedule. Keep resource budgets and mismatch reporting aligned with the fuzz harnesses.

**Acceptance:** the SemVer pilot has a reviewed domain/difference matrix, pinned test-only
comparator, replayable paired cases and dispositions for mismatches. Unsupported comparison
dimensions are explicitly excluded. Independent specification cases remain alongside comparisons,
and deliberate expected dialect differences do not become false failures or hidden exclusions.

### 8.4 Stryker.NET mutation testing

Line/branch coverage shows which code tests execute. Mutation testing probes whether assertions
detect deliberate changes to that code; neither establishes complete correctness or security.
Use **Stryker.NET** for this work. This plan does not install or run it.

- [ ] Check current Stryker.NET compatibility with the chosen SDK, test runner and target framework;
  pin the selected tool version during implementation and record the tested configuration.
- [ ] Start with the three-rule pilot and selected high-risk Core kernels. Target meaningful
  boundary comparisons, null/parse checks and predicate negation with deterministic tests and fixed
  clock/culture inputs. Keep generated code and unrelated projects outside the initial scope.
- [ ] Establish a report-only baseline before enabling a mutation-score failure threshold. Record
  source revision, scope, exclusions, configuration, duration and retained reports so runs can be
  compared. Do not set a blanket 100% mutation target or borrow the coverage threshold.
- [ ] Triage surviving mutants as possible assertion gaps; review no-coverage mutants as execution
  gaps. Investigate timeouts, compile errors and ignored mutants separately. A timeout is not proof
  that the intended assertion detected a defect. Document suspected equivalent mutants and scoped
  exclusions with reasons rather than hiding them to improve the score.
- [ ] Strengthen meaningful behaviour assertions for confirmed gaps, then rerun the affected scope.
  Use small bounded PR runs for established high-value scopes and a scheduled broader campaign;
  select concurrency and budgets that respect repository build/test coordination.
- [ ] Introduce blocking thresholds only after repeatable baselines and survivor triage justify them;
  record the scope, rationale and reviewed exceptions. Expand by risk and useful findings.

**Acceptance:** representative semantic mutants are killed by relevant assertions; surviving,
no-coverage, timeout, ignored and compile-error outcomes have reviewable dispositions. Baselines
and reports are reproducible, and any blocking gate has an evidence-based rationale. Equivalent
mutants are assessed explicitly; a favourable score is not presented as proof of complete testing.

Implementation references: official [Stryker.NET configuration](https://stryker-mutator.io/docs/stryker-net/configuration/)
and [reporters](https://stryker-mutator.io/docs/stryker-net/reporters/).

### 8.5 Security contracts and independent review

- [ ] Write a threat model for OwaspSafe and individual XSS, SQL injection, command, path,
  redirect and SSRF-scheme heuristics. Record decoding/canonicalization assumptions, false
  positives/negatives, input-size boundaries and downstream trust boundaries.
- [ ] Carry the existing candid heuristic limitations into public method, package and sample
  documentation. Validate any proposed renames or stronger claims through compatibility review.
- [ ] Explain contextual output encoding, parameterized queries and appropriate allowlists.
  SSRF scheme checks do not establish destination safety, DNS/IP policy or redirect safety.
  Do not introduce networking/IO into pure Core predicates to imply otherwise.
- [ ] Obtain review from an independent security reviewer outside this AI assessment; this planning
  review does not satisfy that requirement. The reviewer must examine the contracts and high-risk
  implementations, not merely approve tests generated from the same implementation. Record findings,
  reproduction evidence, severity rationale and remediation/accepted-limit decisions.
- [ ] Review validation failure payloads/logging for unintended exposure of original values.

**Acceptance:** each security-oriented API states what it establishes and what it does not;
critical/high findings are resolved or explicitly block the affected claim/release. Lower-severity
accepted limits have an owner and rationale. A library-wide security guarantee is not an exit claim.

## 9. Phase 4 — performance, allocation and deployment evidence

### 9.1 Establish the measurement baseline

- [ ] Add a BenchmarkDotNet project in the repository's approved project layout after scope review.
  Benchmark direct Core predicates, Must success/failure, Guard success/throw, parsers, regexes,
  representative collections, object-validator construction/reuse and framework adapters.
- [ ] Measure time and allocated bytes for realistic small/large inputs and adversarial inputs.
  Separate cold construction from steady-state execution, and successful validation from throwing.
- [ ] Include value-type boxing and MustResult allocation, repeated parsing, expression compilation
  and reflective adapter setup in the investigation. Do not assume those costs are unacceptable.
- [ ] Record source/runtime/SDK/OS/CPU/build configuration and statistical variation. Compare
  equivalent workloads and semantics; publish a reproducible baseline before performance claims.
- [ ] Run packed consumer publish/smoke scenarios for trimming and NativeAOT where support is
  intended, including dynamic generic DataAnnotations paths. Reflection/expression use alone is
  not proof of incompatibility. Document unsupported paths precisely if support is deferred.
- [ ] Begin performance reporting without blocking PRs. After repeated stable measurements,
  approve the explicit budgets below, grounded in variation and intended consumer workloads.
  Keep noisy shared-runner measurements informative until a reliable gate is established.

### 9.2 Approve and enforce explicit performance budgets

A statement such as "email validation takes 80 ns" is not a performance contract. Each budget
must identify the callable/surface, input corpus and size range, success/error path, cold/warm
mode, concurrency, target runtime/build and reference environment, plus the measured statistic
and sampling method. Limits apply to that defined workload, not every machine or input.

The matrix below is a budget template. **TBD means unresolved pending measured baselines and
workload review, not unlimited or accepted.** Expand rows by surface/path/mode where their costs
differ; do not average successful checks and exception paths into one passing result.

| Workload and measurement mode | Absolute time/throughput limit | Allocation/resource limit | Allowed relative regression and variation | Decision owner |
|---|---|---|---|---|
| Core Email/range/date parsing, warm, named short/typical/long input sets | TBD: named latency statistic per operation | TBD: allocated bytes/op | TBD: tolerance justified by repeated runs | Astra |
| Must and Guard, separate success/failure/throw paths and value/reference inputs | TBD: per-surface/path latency | TBD: allocated bytes/op | TBD: time and allocation tolerances separately | Astra |
| Validator construction and first use versus reused validation | TBD: cold construction/first-use and warm-operation limits | TBD: bytes/construction and bytes/validation | TBD: separate cold/warm tolerances | Astra |
| Representative adapter/request pipeline at specified load/concurrency | TBD: throughput floor and, where measured suitably, tail-latency ceiling | TBD: bytes/request and bounded memory | TBD: load-harness tolerances | Astra |
| Regex/parser hostile corpus with explicit input-size bounds | TBD: completion/evaluation deadline per bounded case | TBD: memory/input limits and supervisor budget | TBD: reproducible resource-growth limits | Astra |

- [ ] Have an execution agent record the benchmark case/configuration and evidence for each row;
  Astra approves the limit, rationale, supported scope and reviewer/maintenance owner. Resolve
  applicable TBDs before declaring performance readiness or enabling the corresponding stable gate.
- [ ] Set both absolute budgets and relative regression limits. A slowdown inside a generous
  absolute budget may still be a regression; a stable result over the absolute budget still fails.
  Distinguish observed measurement variation from the regression allowance rather than granting
  arbitrary extra headroom. Record any justified not-applicable metric explicitly.
- [ ] Measure throughput/tail latency only with a workload and harness that support those claims.
  Do not label a BenchmarkDotNet microbenchmark statistic as application request p99. Treat cold
  startup and steady-state results separately, with warm-up and sampling methodology recorded.
- [ ] Align hostile-input budgets with fuzzing limits and Phase 0's timeout/evaluation-error
  contract. A supervisor stopping a campaign is a resource finding, not a successful validation.
  Do not introduce broad input caps or change accepted inputs merely to hit a benchmark without
  reviewing the resulting public contract.
- [ ] Move from report-only baselines to blocking checks on a stable reference environment once
  limits are approved. Retain informative reports on noisier runners and schedule heavier load/
  hostile-input checks; make the reproducible blocking path explicit in CI/release evidence.
- [ ] Triage breaches against the same workload/environment: investigate noise, implementation
  regressions and intentional tradeoffs. Any baseline/budget revision requires recorded evidence
  and Astra review; never automatically update the baseline to make a failing change pass.

**Acceptance:** benchmark results are reproducible; pilot changes have before/after evidence;
supported trimming/AOT scenarios publish and run; applicable budget rows have approved concrete
limits and an enforceable verification path before performance readiness is claimed. Every limit
has a measured rationale and breach-review process. No arbitrary numeric promise is made while
the baseline is unknown, and unresolved applicable TBDs cannot silently pass the release review.

## 10. Phase 5 — compatibility, consumer samples and documentation

### 10.1 Public API and packed-artifact gates

- [ ] Inventory supported packages/frameworks and reconcile outstanding pre-1.0 API decisions.
  Record intentional framework differences, breaking changes, deprecations and migration needs.
- [ ] Add public API baselines for declaration review and package/API compatibility checks for
  binary/framework/cross-release compatibility. These are related but distinct checks.
- [ ] Select the proper comparison release per package; new source-only packages need an initial
  baseline, not a fictitious previously published version. Review intentional breaks explicitly.
- [ ] Test fresh consumers against locally packed artifacts, including transitive dependencies,
  target-framework selection, XML docs, symbols/source links and analyzer assets where applicable.
- [ ] Run representative Windows/Linux and culture-sensitive consumer cases where advertised
  portability depends on runtime/OS behaviour. Record exactly which combinations CI covers.
- [ ] Define a reproducible reference SDK/toolchain and reviewed upgrade policy, while retaining
  compatibility testing against supported updates. Record exact versions in evidence rather than
  assuming moving SDK selectors are identical between runs.
- [ ] Extend dependency maintenance to the pnpm/CLI tooling as well as NuGet and GitHub Actions;
  choose the existing repository mechanism where suitable. No new dependency/security platform
  is required merely to satisfy this task.

**Acceptance:** a deliberate public breaking change triggers a reviewable gate; intended TFM
differences are enumerated; supported package combinations restore, compile and execute using
packed artifacts rather than only project references.

### 10.2 Executable application examples

Create a compact shared business scenario with distinct, understandable examples. Check existing
samples before adding directories; proposed sample files are not assumed to exist today.

| Example | Behaviour to demonstrate |
|---|---|
| Minimal API | Bound request validation, error codes/property paths, ProblemDetails and cancellation |
| Clean Architecture | Application validation with dependency direction preserved and framework adapters at the boundary |
| Vertical Slice with MediatR | Request/pipeline validation, result mapping, cancellation and duplicate-validation avoidance |
| DDD/domain model | Guarded construction or value objects, invariant failures, parsed values and application/domain boundary distinctions |

- [ ] Keep examples runnable with deterministic local inputs; avoid requiring production secrets
  or external services to demonstrate validation.
- [ ] Test success and meaningful failure paths against packed prerelease artifacts.
- [ ] Use examples to discover API friction; record resulting design decisions. Internal examples
  are engineering validation, not evidence of independent customer adoption.

**Acceptance:** documented startup commands work from a clean checkout, example tests run in CI,
and readers can follow an input through validation to its expected application outcome.

### 10.3 Documentation structure and honest claims

- [ ] Keep the root README focused on purpose, a quick start, surface selection, current package
  availability and links. Move detailed API catalogs, framework recipes and long examples into
  user-facing documentation/package guides; keep AI engineering specs in the Brain.
- [ ] Generate only genuinely derivable manifest reference material. Explain semantics, tradeoffs
  and threat boundaries in reviewed prose rather than generated marketing assertions.
- [ ] Version examples against a source revision or published package version. Clearly label
  source-only APIs so a released alpha consumer is not shown unavailable members as usable today.
- [ ] Preserve the corrected README metrics and dated verification report. Keep measured tests,
  current coverage, historical screenshots, configured thresholds and external review separate.
- [ ] Reconcile CONTRIBUTING's stale test-project count and unconditional throwing claims, plus
  stale workflow comments about absent audit baselines. Check claims against current contracts
  and active gate configuration, not comment text or baseline file length.
- [ ] Add migration notes for intentional public changes and a precise support/deployment matrix.

**Acceptance:** links/examples are checked, package-version claims match the release inventory,
and every quality/performance/security claim points to appropriately dated evidence and limits.

### 10.4 Consumer lifecycle and extension contracts

- [ ] Specify when validator configuration ends, including InlineMustValidator and retained
  MustPropertyRule handles. Choose deliberately between documented configure-before-use behaviour
  and enforced freezing; preserve existing consumers or document any intentional breaking change.
- [ ] Qualify singleton/concurrent reuse for consumer predicates, getters, nested validators and
  injected dependencies. Add targeted tests for supported concurrent reuse and selected mutation
  behaviour; do not claim arbitrary stateful user callbacks become thread-safe automatically.
- [ ] Preserve and exercise existing ordered async execution, cancellation, aggregate/first-failure
  modes, code/message/path overrides, localization and scoped exception policies in consumer tests.
  Audit gaps first: existing cancellation/override tests are not missing features to rebuild.
- [ ] Review failure payload/logging/serialization boundaries across supported adapters, recording
  which paths can expose original values and how consumers apply redaction. Credit existing
  omission of raw Value from MustFailure printing; do not generalize it to every serializer.

**Acceptance:** documented lifecycle claims match executable supported behaviour; representative
concurrent/async/customized consumers pass; unsupported mutation/dependency patterns and sensitive
value boundaries are explicit. Additional APIs are introduced only where the review justifies them.

## 11. Phase 6 — expand, release and learn

- [ ] Review pilot acceptance with Astra; expand descriptor/conformance coverage in risk-ordered
  families, preserving hand-written algorithms and public entry points where suitable.
- [ ] Freeze the intended 1.0 contract only after outstanding compatibility/null/result/error
  decisions are resolved. Keep an explicit list of deferred capabilities and supported exceptions.
- [ ] Gather a release-candidate evidence bundle: clean coordinated Release builds/tests,
  fresh coverage for the intended assemblies, contract audits, API/package checks, consumer
  samples, security dispositions and benchmark/deployment baselines. Existing CI coverage
  requirements continue to apply; do not silently weaken them to pass the program.
- [ ] Verify the live package inventory and stage the integration packages through prereleases
  with coherent dependencies, notes and rollback/unlisting decisions documented in advance.
  Publication requires the applicable release authorization; this plan does not publish anything.
- [ ] Require the selected release gates for the exact revision/artifact being published, either
  by verified same-revision CI evidence or a reusable release validation workflow. Include required
  coverage/audit/API/packed-consumer evidence, artifact identity and the supported toolchain.
  Inspect actual remote protections before relying on them; their presence/absence is unverified
  by this plan. Preserve short-lived OIDC publishing credentials and scoped permissions.
- [ ] Collect external feedback through independently used integrations and examples. Record
  reports about usability, interoperability and real workloads with provenance and consent.
  Feed confirmed defects into the same regression/contract process.

**Acceptance:** each package included in 1.0 has an explicit support contract and release evidence;
the release inventory is accurate; no unresolved defect is concealed by test counts or coverage.
External feedback is reported as received, not assumed. Lack of market evidence is labelled
separately from completed technical gates.

## 12. Execution controls and completion record

- Astra owns scope decisions, architectural tradeoffs, review and plan updates. Execution agents
  receive bounded file ownership, acceptance criteria and appropriate model routing for the work.
- Read root/nearest AGENTS, applicable specs, skills and safety rules for each execution unit.
  Follow [coordination rules](../rules/coordination.md) for shared build/test/coverage resources.
- Use a small first change for Phase 0, then the three-rule pilot. Do not start parallel bulk
  generation before the manifest and conformance acceptance criteria have been demonstrated.
- Each unit records changed files, decisions, verification commands/results and retained evidence.
  Update checklist/status when evidence exists; planned work must not be marked complete merely
  because a specification or stub was written.
- Preserve unrelated working-tree changes, including the existing README/report work, local
  Codex configuration and separate schema-driven-agent plan. Do not stage or revert them as part
  of this program without their own authorized scope.
- Keep new temporary evidence under artifacts/logs using the repository conventions. Commit
  durable summaries where appropriate; ignored local artifacts alone are not portable proof.
- Stop expansion if the pilot adds a second source of truth, obscures error semantics, breaks
  supported consumers or adds unjustified runtime cost. Retain useful tests/audits and revise the
  representation rather than forcing the original design through the whole library.

### Program completion checklist

- [ ] Confirmed semantic findings resolved, or accurately reclassified with evidence.
- [ ] CI routing regression fixtures pass; required test/coverage artifacts fail closed; CLI self-tests and selected semantic audits are active.
- [ ] Canonical descriptor and manifest ownership proven by the pilot and enforced during expansion.
- [ ] Applicable projections covered by policy-aware conformance plus independent correctness cases.
- [ ] Property-testing pilot has justified invariants, meaningful generated cases and replayable shrunk failures.
- [ ] Fuzz campaigns and shared regression corpus reproducible; security boundaries and independent review recorded.
- [ ] Differential SemVer pilot has an explicit comparison domain, pinned comparator and replayable triaged mismatches.
- [ ] Stryker.NET pilot baselined, mutant outcomes triaged and any score gate justified by evidence.
- [ ] Performance/allocations and intended deployment modes measured; applicable absolute and relative budgets approved and enforced, with no unresolved release-critical TBDs.
- [ ] Public/package compatibility, executable samples and truthful versioned docs enforced in CI.
- [ ] Consumer lifecycle/concurrency and customization claims verified; reference toolchain and tooling-dependency maintenance documented.
- [ ] Release checks bind to the exact revision/artifact; remote protection assumptions verified before use.
- [ ] Release inventory and technical evidence reviewed; external feedback tracked separately.

<!-- footer
last_verified: 2026-09-25
-->
