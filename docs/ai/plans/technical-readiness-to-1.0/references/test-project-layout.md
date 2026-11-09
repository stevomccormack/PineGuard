# Approved testing project layout

The user approved this six-project layout on 2026-09-26. It groups new crosscutting work by responsibility, with techniques separated into folders. It replaces the earlier suggestion to add these cases to existing library unit-test projects and the unapproved one-project-per-technique proposals. This is an approved planning decision; the projects do not yet exist and this document does not authorize marking their execution gates complete.

## Existing shared infrastructure

`tests/PineGuard.Testing/PineGuard.Testing.csproj` is the shared fixture, case and projection library. `tests/PineGuard.Testing.UnitTests/PineGuard.Testing.UnitTests.csproj` tests that infrastructure itself. Neither becomes a second home for the new crosscutting suites. Existing per-library unit-test projects retain their normal rule and adapter coverage.

Reuse the canonical fixtures and existing typed layer projections. Do not create a competing fixture library, a second rule implementation, or independently maintained copies of the same expected cases. Expected outcomes must be independently authored; invoking production validation to generate expected truth is forbidden.

The existing infrastructure boundary is documented in `docs/ai/specs/testing/project.md`, sections 1–3. New projects must be registered in `PineGuard.slnx` when they are actually implemented. The approved suffixes below express the user's layout choice; they are not represented as pre-existing BRAIN naming requirements.

## Canonical project identities

| Stable ID | Project and planned directory | Responsibility | Execution kind |
| --- | --- | --- | --- |
| TP-RULES | `+ tests/PineGuard.Testing.UnitTests.Rules/PineGuard.Testing.UnitTests.Rules.csproj` | Crosscutting rule semantics and in-process adapter contracts | Test project |
| TP-ARCH | `+ tests/PineGuard.Testing.UnitTests.Architecture/PineGuard.Testing.UnitTests.Architecture.csproj` | Structural dependency, manifest and public-contract checks | Test project |
| TP-INTEGRATION | `+ tests/PineGuard.Testing.IntegrationTests/PineGuard.Testing.IntegrationTests.csproj` | Real host, container and package composition | Test project; scenario-specific host requirements |
| TP-FUZZ | `+ tests/PineGuard.Testing.Fuzz/PineGuard.Testing.Fuzz.csproj` | Instrumented hostile-input entrypoints and corpus lifecycle | Dedicated fuzz executable/harness |
| TP-BENCH | `+ benchmarks/PineGuard.Benchmarks/PineGuard.Benchmarks.csproj` | Representative performance and allocation measurements | Benchmark executable. The string→DateOnly pilot PR's BenchmarkDotNet project is TP-BENCH; its exact path is fixed by the PR's file list (D22). |
| TP-CONSUMERS | `+ tests/PineGuard.Testing.Consumers/PineGuard.Testing.Consumers.csproj` | Minimal consumers built and executed in normal, trimmed and native configurations | Consumer executable |

The project name and directory name must agree. `PropertyBased` is a folder within TP-RULES, not another project. Earlier suggested suffixes `.Conformance`, `.Properties`, `.PropertyBased` and `.Differential` are not approved project names. No new project uses those technique-based suffixes.

## Folder responsibilities

### TP-RULES

- `Conformance/`: shared independent cases through Core, Must, Guard, object validation, FluentValidation and DataAnnotations; verify codes, paths, ordering, parsed results and intentional projection differences.
- `PropertyBased/`: declared invariants, boundary generation and metamorphic relationships, with reproducible seeds and shrinking evidence when supported by the chosen tool.
- `Differential/`: comparisons with independent references over explicitly equivalent input domains and contracts.
- `Determinism/`: fixed clocks, culture and time-zone boundaries, stable output ordering and restoration of ambient state.
- `Concurrency/`: the reviewed lifecycle contract, isolation and concurrent reuse; do not infer mutable validator safety from static predicate tests.
- `Security/`: deterministic regression cases for regex errors and negation, resource boundaries, diagnostic privacy and hostile inputs.

These are in-process contract tests. Real servers, HTTP transport, external processes and deployment behavior belong to the appropriate host or consumer project. Mutation configuration targets these suites and relevant existing surface tests; it does not create a duplicate mutation-test project.

### TP-ARCH

- `Dependencies/`: allowed project references, layer direction, cycles and boundary violations.
- `Manifest/`: completeness, unique identifiers and codes, missing/orphan projections, and seeded drift detection.
- `PublicContracts/`: the newly reviewed public API and consistency across supported target frameworks. No legacy compatibility obligation is introduced.
- `Conventions/`: reliable automated naming, taxonomy and registration checks, with explicit positive and negative fixtures.

Reuse existing CLI audits where they already own a check. Do not implement the same enforcement independently in C# and TypeScript. W00/W02/W12/W22 must decide the authoritative enforcement home for each check and record how the other surface consumes its result.

### TP-INTEGRATION

- `AspNetCore/`: actual host/HTTP validation, transport mapping and async/cancellation behavior.
- `DependencyInjection/`: container registrations, scopes and lifecycle composition.
- `Options/`: real options binding and startup validation.
- `Packages/`: consumers restoring, building and executing packed PineGuard artifacts with their declared dependencies.

An adapter extension method alone does not make a test an integration test. Keep pure adapter contract checks in TP-RULES and host-dependent checks here. Dependency and support decisions determine the applicable target-framework matrix; do not import every adapter or host into every project.

### TP-FUZZ

- `Strings/`, `Parsing/`, `Regex/` and `Composition/`: instrumented entrypoints and target-specific harness code.
- Corpus and reproduction artifacts follow the selected tool's reviewed conventions; persist seed, input, revision, runtime and outcome.

This project is for tool-driven fuzz execution. Small deterministic regressions discovered by fuzzing return to the authoritative canonical fixture and appropriate test suite. Corpus replay alone does not establish coverage-guided fuzzing evidence.

### TP-BENCH

- `Rules/`: representative canonical rule workloads.
- `Projections/`: Must, Guard and adapter overhead where contracts are comparable.
- `Composition/`: realistic object and collection validation.
- `Failures/`: rejected inputs, error construction and allocation behavior.

Use the reviewed benchmark runner and configuration. Consume results, declare environment and workload, and record uncertainty. Do not place benchmark timing thresholds in ordinary unit tests or use the documentation miniature as a benchmark baseline.

### TP-CONSUMERS

- `Scenarios/Rules/`, `Scenarios/Adapters/` and `Scenarios/Composition/`: small executable consumer scenarios.
- Run applicable scenarios under ordinary, trimmed and NativeAOT publish configurations and execute the resulting artifacts.

One consumer project is the initial approved layout. A dependency or toolchain incompatibility may justify a later Astra-reviewed split; do not pre-create three copies for the three publish modes. A publish success without artifact execution is insufficient. Large API, DDD and Clean Architecture sample applications remain final-phase demonstrations under W20 and are not replaced by this small consumer harness.

## Workstream mapping

| Workstream | Planned project or shared responsibility |
| --- | --- |
| W00 CI/evidence | Discovery/classification and fail-closed evidence for all applicable project kinds |
| W01 semantics | Canonical fixtures and existing surface tests; crosscutting expectations in TP-RULES |
| W02 manifest | Existing audit authority plus TP-ARCH/Manifest; no duplicate gate |
| W03 kernel | Existing surface tests, TP-RULES and early TP-BENCH measurement |
| W04 failure/projections | Existing surface tests and TP-RULES/Conformance |
| W05 conformance | TP-RULES/Conformance |
| W06 properties | TP-RULES/PropertyBased |
| W07 fuzzing | TP-FUZZ; regressions return to canonical fixtures |
| W08 differential | TP-RULES/Differential |
| W09 mutation | Existing test suites plus TP-RULES; runner/configuration rather than another project |
| W10 performance | TP-BENCH |
| W11 trim/native | TP-CONSUMERS |
| W12 public contracts | TP-ARCH/PublicContracts and TP-INTEGRATION/Packages |
| W13 dependencies | TP-ARCH/Dependencies and TP-INTEGRATION/Packages |
| W14 threat model | TP-RULES/Security plus applicable fuzz/host/consumer evidence |
| W15 regex | TP-RULES/Security and TP-FUZZ/Regex |
| W16 resources | TP-RULES/Security plus applicable host/fuzz/benchmark evidence |
| W17 diagnostics | TP-RULES/Security and applicable host integration |
| W18 determinism | TP-RULES/Determinism |
| W19 lifecycle | TP-RULES/Concurrency and TP-INTEGRATION/DependencyInjection |
| W20 samples/support | TP-CONSUMERS and TP-INTEGRATION early; large applications last |
| W21 readiness | Evidence rollup by project identity, execution kind, revision and configuration |
| W22 taxonomy | TP-ARCH/Conventions plus the existing authoritative BRAIN/audit mechanism |

## Discovery, scripts and evidence gates

Luna inventories the actual solution, CI, test commands, coverage tooling and documentation registries before Sol changes discovery. Record each affected path, its current classification assumption and the necessary update or evidence-backed no-change decision. Prefer existing registration and shared discovery mechanisms; do not add a parallel registry merely for this plan.

Classify test projects separately from fuzz, benchmark and consumer executables. Only applicable test projects are sent to the ordinary test runner. Harnesses have explicit build/run or tool-specific jobs, expected outputs and failure behavior. Paths under `tests/` do not automatically identify a project as runnable by `dotnet test`.

The source audit found that the pre-amendment default in `tools/.shared/dotnet-projects.ps1` discovers `*.UnitTests.csproj`, which excludes the approved `.UnitTests.Rules`, `.UnitTests.Architecture` and `.IntegrationTests` names. The immediate script correction must extend the existing multi-filter mechanism with explicit test-name families and verify positive and negative fixture projects. Define the default once and reuse it across registry, discovery, coverage entrypoint and engine consumers; preserve explicit caller filters and scope-specific overrides. The registry has two distinct responsibilities: `Testing.TestCsproj` identifies the helper library's own tests for `Run-Tests -Scope Testing`, whereas `Testing.DefaultProjectFilter` deliberately discovers other test projects whose execution covers that helper library. Extend the latter's broad coverage discovery without changing the former's run target. Fuzz, Benchmarks and Consumers remain excluded from default ordinary-test discovery. An explicit caller filter remains an intentional override; name-based discovery is not proof that an arbitrary selected project is a test project. Do not add absent projects to active CI jobs or solution entries prematurely.

The audit also found that `tests/Directory.Build.props` applies test defaults across the `tests/` tree, including test TFMs and analyzer settings. Before TP-FUZZ or TP-CONSUMERS is created, explicitly review that inheritance and choose a scoped override or an appropriate narrowing of the shared defaults. The executable must not accidentally inherit a test project's runtime/analyzer contract. This is a blocking implementation prerequisite, not a claim that the shared props file has already been corrected. The existing CI matrix and solution registration must be extended when each required project is implemented, with seeded missing-project/artifact checks.

The absence of these planned projects is expected while the implementation workstreams are Planned. Once a phase declares a project/configuration required, missing projects, missing runs or missing artifacts must fail that phase's gate. Discovering only successfully produced outputs must never define the expected matrix. Required validation coverage remains 100% line and branch within the reviewed denominator, with every declared validation surface accounted for; the layout does not justify exclusions or weaker thresholds.

For each implemented project, closeout requires: solution membership, references and package/TFM decisions; authoritative discovery/routing changes; positive and seeded-negative verification of those changes; exact evidence paths; updated root/per-project documentation and affected BRAIN guidance; and the shared knowledge/tooling feedback loop. Future jobs and scripts described here are requirements, not claims that they already exist or have run.

## Approval boundary

The six names, responsibilities and grouping are approved. Exact tool/package selection, detailed test classes, support matrices and additional project splits remain governed by their workstream decisions. Existing BRAIN conventions apply unless the user's explicit layout choice changes a convention; record that narrow precedence and cascade relevant specs/rules/docs rather than leaving contradictory instructions. Source project creation and library implementation remain future work.
