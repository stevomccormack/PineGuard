# Astra disposition of the ten-project comparison

Read [Luna's official-source dossier](competitor-source-dossier.md) and [Sol's independent comparative analysis](sol-comparative-analysis.md). Both are preserved unchanged with hashes. All research was checked2026-09-26. The set is purpose-selected and unranked; 'top ten' is not an evidence-backed market/adoption ranking.

Sol's analysis is accepted as comparative input. Its old P4 performance/P5 compatibility sequencing is explicitly superseded by the latest user instructions: early benchmarks/new structure/manifest; greenfield new API; largest sample apps last. No recommendation automatically creates a feature commitment.

| Comparator | Accepted/adapted decision | Deferred/rejected implication | Evidence |
|---|---|---|---|
| FluentValidation | Review nested/null/collection/conditions/cascade/error codes and explicit async invocation in W01/W04/W05 | Defer adapter breadth; reject 'lacks composition/async' | [Validators](https://docs.fluentvalidation.net/en/latest/built-in-validators.html), [async](https://docs.fluentvalidation.net/en/latest/async.html), [ASP.NET](https://docs.fluentvalidation.net/en/latest/aspnet.html) |
| DataAnnotations/platform validation | Caller-specific attribute/MVC/Minimal API/generated-metadata expectations; real consumer invocation W05/W11 | Reject universal claims of no generation/nesting/AOT; defer broader generators until justified | [Platform validation](https://learn.microsoft.com/en-us/aspnet/core/validation/overview?view=aspnetcore-10.0), [options generator](https://learn.microsoft.com/en-us/dotnet/core/extensions/options-validation-generator) |
| Ardalis.GuardClauses | Concise guard/value-return/exception ergonomics as W12 design cases | Defer catalog expansion; no implied object-validator equivalence | [Repository](https://github.com/ardalis/GuardClauses), [releases](https://github.com/ardalis/GuardClauses/releases) |
| Dawn.Guard | Historical chaining/context/allocation hypotheses for W10/W12 | No dependency/generator mandate; unimplemented v2 is not evidence | [Repository](https://github.com/safakgur/guard), [releases](https://github.com/safakgur/guard/releases) |
| CommunityToolkit.Diagnostics | Primitive/nullable guard workloads, overload consistency and boxing measurements early | Reject faster/slower claims without matched evidence; generation only if justified | [Guard](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/diagnostics/guard), [null API](https://learn.microsoft.com/en-us/dotnet/api/communitytoolkit.diagnostics.guard.isnotnull?view=dotnet-comm-toolkit-8.4) |
| Validot | Specification composition/nullable/localization questions | Archived historical design reference; defer broad relation/localization expansion; reject owner performance claims as comparisons | [Repository](https://github.com/bartoszlenar/Validot), [releases](https://github.com/bartoszlenar/Validot/releases) |
| Valit | Strategies/conditions/errors as explicit execution-contract questions | No second tag metadata catalog; maintenance/support remains unknown | [Repository](https://github.com/valit-stack/Valit), [package](https://www.nuget.org/packages/Valit/2.0.0) |
| ExpressiveAnnotations | Cross-property cases and early configuration errors; explicit trust boundaries | Defer string-expression/client evaluator; reject assumed server/client equivalence or unsupported exploit claim | [Repository](https://github.com/jwaliszko/ExpressiveAnnotations), [releases](https://github.com/jwaliszko/ExpressiveAnnotations/releases) |
| Vogen | Raw/normalized/parsed/construction outcome separation in pilot and later DDD sample | Defer wrapper generator/adapter; generation alone does not prove AOT | [Repository](https://github.com/SteveDunn/Vogen), [releases](https://github.com/SteveDunn/Vogen/releases) |
| Zod | Clear success/error and input/output contracts, immutable-composition/async questions | Defer schema/inference/JSON Schema expansion; schema-driven agent remains excluded | [Documentation](https://zod.dev/), [repository](https://github.com/colinhacks/zod) |

## Positioning and gaps

Accepted hypothesis: PineGuard aims to reuse explicit executable semantics across several .NET call-site styles. It becomes a defensible advantage only after source/package availability, conformance, usability, measured cost and deployment proof. The research does not establish unique breadth, superior correctness, faster execution or universal deployment support.

New review questions are incorporated without new feature scope: caller-specific skip/null/metadata behavior; failure ordering/cascade; input versus parsed output; mutable lifecycle; low-level guard boxing; expression trust; and dependency maintenance status. Match guards with guards, object validators with equivalent composition, and parsers with equivalent transformations. Do not compare dissimilar language/runtime scenarios as a common performance ranking.

Versions/TFMs/licenses unresolved in the dossier stay unresolved. Refresh before publishing product comparisons. Archived repositories remain useful design references but are not automatically selected dependencies.
