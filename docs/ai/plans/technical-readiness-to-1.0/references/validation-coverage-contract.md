# Hard validation coverage contract

The user's100% requirement is retained in full for all validation rules and projections, including Core/Rules, Must, Guard, FluentValidation, DataAnnotations and every other declared validation surface. It does not automatically impose100% on unrelated application infrastructure in the late sample projects.

## Verified existing authority

Luna verified `docs/ai/specs/testing/coverage.md` version3, §§Primary goal, Non-Negotiables, What Filtered Scope Means, and `tools/code-coverage/coverlet.runsettings`. Coverlet is the sole gate authority; dotCover snapshots cannot gate. Existing thresholds are exactly100% line and100% branch **within filtered scope**.

Runsettings includes `[PineGuard.*]*`; excludes `System.Text.RegularExpressions.Generated.*`, obj/bin files, and GeneratedCodeAttribute, CompilerGeneratedAttribute and ExcludeFromCodeCoverageAttribute. This is not literal measurement of every generated line. Existing coverage specifications name Core, Must, Guard, DataAnnotations, FluentValidation, Options, DI, ASP.NET, ErrorOr, FluentResults, OneOf, MediatR, Analyzers and Testing. Paired per-library specifications include:
- `docs/ai/specs/core/coverage.md`
- `docs/ai/specs/must-clauses/coverage.md`
- `docs/ai/specs/guard-clauses/coverage.md`
- `docs/ai/specs/data-annotations/coverage.md`
- `docs/ai/specs/fluent-validation/coverage.md`

## Mandatory W00 tasks and gate

1. Inventory every current/new validation assembly, surface, dedicated test job, target framework and expected Coverlet report. Map semantics to the measured denominator.
2. Audit every exclusion and generated-code boundary. Show that validation behavior is not hidden; approve explicit generated-source/generator tests and deployment/conformance proof for excluded generated logic. Do not add exclusions to manufacture100%.
3. Derive expected reports from the selected job/target matrix before reading results. Missing, empty, corrupt, wrong-target or stale reports fail. An empty expected set is accepted only for an explicitly legitimate no-coverage-required change.
4. Enforce100% line AND100% branch for every required valid filtered report/approved aggregation scope. Do not let full-suite totals hide an uncovered affected surface.
5. Seed an uncovered line, uncovered branch, exclusion abuse and missing expected report in controlled fixtures; each must fail appropriately. Restore seeds and verify green.
6. Re-run the relevant coverage gates after every validation change and the complete declared validation scope before final readiness. Record commands, revision, targets, denominators, exclusions and artifact hashes.

The current workflow's found=false path can skip thresholds when no XML is found (rubric R2); this must be corrected through the expected-artifact contract, not by pretending missing reports are zero-applicable coverage.

## Reporting

State covered/total line and branch counts, scope, targets, exclusions and artifact locations. The37,749 historical passing framework executions collected no fresh coverage. A conformance matrix or property run cannot support a100% coverage claim without the required Coverlet artifacts.

Any material coverage gap blocks completion of the affected validation workstream and the final10/10 assessment. Only an explicit user change can relax this hard requirement; agents cannot substitute a lower target or a convenient exclusion.
